namespace Formulae.Units;

public class Unit
{
    public string Name { get; }
    public BaseUnit BaseUnit { get; }
    public Factor Factor { get; }

    public static Unit UnitLess = new(BaseUnit.Unitless);
    public static Unit Gram = new(BaseUnit.Gram);
    public static Unit Kilogram = new("kg", BaseUnit.Gram, Factor.Kilo);
    public static Unit Milligram = new("mg", BaseUnit.Gram, Factor.Milli);
    
    public static Unit Celsius = new(BaseUnit.Celsius);
    public static Unit Kelvin = new(BaseUnit.Kelvin);
    
    private Unit(string name, BaseUnit baseUnit, Factor factor)
    {
        Name = name;
        BaseUnit = baseUnit;
        Factor = factor;
    }

    private Unit(BaseUnit baseUnit)
        : this(baseUnit.Name, baseUnit, Factor.One)
    {
    }
}