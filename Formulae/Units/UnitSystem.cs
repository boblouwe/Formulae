namespace Formulae.Units;

public abstract class UnitSystem
{
    public abstract string Name { get; }

    public static InternationalSystemOfUnits InternationalSystemOfUnits => new();
}

public class InternationalSystemOfUnits : UnitSystem
{
    public override string Name => nameof(InternationalSystemOfUnits);

    static InternationalSystemOfUnits()
    {
    }
}