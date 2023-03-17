using Formulae.Units;
using Flee.PublicTypes;

namespace Formulae;

public class Formula : Variable
{
    private readonly string _expression;
    private readonly ICollection<Variable> _parameters;

    public Formula(string name, string expression, ICollection<Variable> parameters, Unit unit)
        : base(name, unit)
    {
        _expression = expression;
        _parameters = parameters;
    }

    public Formula(string name, string expression, ICollection<Variable> parameters)
        : this(name, expression, parameters, Unit.UnitLess)
    {
    }

    protected override Number GetNumber(bool force)
    {
        var expressionContext = new ExpressionContext();
        var evaluations = _parameters.ToDictionary(parameter => parameter.Name, parameter => force ? parameter.Reevaluate() : parameter.Evaluate());

        foreach (var evaluationKvp in evaluations)
        {
            expressionContext.Variables.Add(evaluationKvp.Key, evaluationKvp.Value.Number.Value);
        }
        
        var genericExpression = expressionContext.CompileGeneric<double>(_expression);
        var value = genericExpression.Evaluate();

        var precision = GetPrecision(evaluations.Values.Select(x => x.Number.Precision).ToArray());
        return new Number(value, precision);
    }
    
    private static int GetPrecision(int[] precisions)
    {
        var precision = 15;
        if (precisions.Any())
        {
            precision = precisions.Min(x => x);
        }

        return precision;
    }
}