using System.Diagnostics;

namespace Formulae;

public class Evaluation
{
    public Number Number { get; }
    
    public EvaluationTrace EvaluationTrace { get; }
    
    public static readonly Evaluation NotEvaluated = new();
    
    public Evaluation(Number value, EvaluationTrace trace)
    {
        EvaluationTrace = trace;
        Number = value;
    }

    public Evaluation() 
        : this(Number.NaN, EvaluationTrace.Empty)
    {
    }

    public Evaluation(Number number)
        : this(number, EvaluationTrace.Current())
    {
    }
}