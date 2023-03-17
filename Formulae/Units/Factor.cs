namespace Formulae.Units;

public class Factor
{
    public string Symbol { get; }
    
    public Number Number { get; }

    public static Factor One = new Factor("", new Number(1));
    public static Factor Kilo = new Factor("k", new Number(1000));
    public static Factor Milli = new Factor("m", new Number(0.1));

    private Factor(string symbol, Number number)
    {
        Symbol = symbol;
        Number = number;
    }
}