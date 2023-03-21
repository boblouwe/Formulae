using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Pack);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    
    [Solution(GenerateProjects = true)]
    readonly Solution Solution;
    
    static AbsolutePath ArtifactsDirectory => RootDirectory / ".artifacts";
    AbsolutePath CompileOutputDirectory => Solution.Formulae.Directory / "bin" / Configuration / "net7.0";
    static string Version => "0.0.1-alpha";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            DotNetClean(x => x.SetProject(Solution.Formulae));
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Before(Compile)
        .Executes(() =>
        {
            DotNetRestore(x => x.SetProjectFile(Solution.Formulae));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Before(Test)
        .Executes(() =>
        {
            DotNetBuild(x => x
                .SetProjectFile(Solution.Formulae)
                .SetConfiguration(Configuration)
                .SetVersion(Version)
                .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Before(Pack)
        .Executes(() =>
        {
            DotNetTest(x => x
                .SetProjectFile(Solution.FormulaeTests)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target Pack => _ => _
        .OnlyWhenDynamic(() => ShouldPack())
        .DependsOn(Test)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution.Formulae)
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableNoRestore()
                .SetVersion(Version)
                .EnableNoDependencies()
                .SetOutputDirectory(ArtifactsDirectory / "nuget"));
        });

    bool ShouldPack()
    {
        if (!Configuration.Equals(Configuration.Release)) return false;
        
        var compiledDllFile = CompileOutputDirectory / Solution.Formulae.Name + ".dll";
        var versionInfo =  FileVersionInfo.GetVersionInfo(compiledDllFile);
        var productVersion = versionInfo.ProductVersion?.ToLower();
        if (productVersion == null) return false;
        var isPrerelease = productVersion.Contains('-');
        return !isPrerelease || productVersion.Contains("-rc");
    }
}
