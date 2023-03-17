using Formulae.Units;

namespace Formulae;

public abstract class Variable
{
    private readonly HashSet<Evaluation> _evaluations = new();

    public string Name { get; }
    public Unit Unit { get; }

    protected abstract Number GetNumber(bool force);

    public Variable(string name, Unit unit)
    {
        Name = name;
        Unit = unit;
    }

    public Variable(string name)
        : this(name, Unit.UnitLess)
    {
    }

    public Evaluation Evaluate(EvaluationTrace trace)
    {
        var evaluation = GetLastEvaluation();
        return evaluation != Evaluation.NotEvaluated ? evaluation : Reevaluate(trace);
    }
  
    public Evaluation Evaluate()
    {
        return Evaluate(EvaluationTrace.Current());
    }

    public Evaluation Reevaluate(EvaluationTrace trace)
    {
        var evaluation = new Evaluation(GetNumber(true), trace);
        AddEvaluation(evaluation);

        return evaluation;
    }
  
    public Evaluation Reevaluate()
    {
        return Reevaluate(EvaluationTrace.Current());
    }
    
    public Evaluation GetLastEvaluation()
    {
        return _evaluations.LastOrDefault() ?? Evaluation.NotEvaluated;
    }
    
    private void AddEvaluation(Evaluation evaluation)
    {
        _evaluations.Add(evaluation);
    }
}