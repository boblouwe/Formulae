using Formulae.Dimensions;

namespace Formulae.Units;

public class BaseUnit
{
    public string Name { get; }
    public Dimension Dimension { get; }
    public UnitSystem UnitSystem { get; }

    public static BaseUnit Unitless = new(string.Empty, Dimensions.Dimensions.Dimensionless);
    public static BaseUnit Gram = new("g", Dimensions.Dimensions.Mass);
    public static BaseUnit Celsius = new("°C", Dimensions.Dimensions.Temperature);
    public static BaseUnit Kelvin = new("K", Dimensions.Dimensions.Temperature);
    
    private BaseUnit(string name, Dimension dimension, UnitSystem unitSystem)
    {
        Name = name;
        Dimension = dimension;
        UnitSystem = unitSystem;
    }

    private BaseUnit(string name, Dimension dimension)
        : this(name, dimension, UnitSystem.InternationalSystemOfUnits)
    {
    }
}