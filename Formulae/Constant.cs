using Formulae.Units;

namespace Formulae;

public class Constant : Variable
{
    private readonly Number _number;

    public Constant(string name, Number number, Unit unit)
        : base(name, unit)
    {
        _number = number;
    }
    
    public Constant(string name, Number number)
        : this(name,number, Unit.UnitLess)
    {
    }

    protected override Number GetNumber(bool _)
    {
        return _number;
    }
}