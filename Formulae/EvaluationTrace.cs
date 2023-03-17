namespace Formulae;

public class EvaluationTrace
{
    public string EvaluatedBy { get; private set; }
    public DateTimeOffset Timestamp { get; private init; }

    public static readonly EvaluationTrace Empty = new();
    public static EvaluationTrace Current() => new();
    public static EvaluationTrace Current(string evaluatedBy) => new(evaluatedBy);

    public EvaluationTrace(string evaluatedBy)
    {
        EvaluatedBy = evaluatedBy;
        Timestamp = DateTimeOffset.UtcNow;
    }
    
    public EvaluationTrace()
        : this("Unknown")
    {
    }
}