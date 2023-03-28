using System.Globalization;
using FluentAssertions;
using Xunit;

namespace FormulaeTests;

public class NumberTests
{
    [Fact]
    public void Number_construct()
    {
        var act = () => new Number();
        act.Should().NotThrow();
    }

    [Fact]
    public void Number_NaN()
    {
        var act = Number.NaN.Value;

        act.Should().Be(double.NaN);
    }
    
    
    [Fact]
    public void Number_ResetCulture()
    {
        var culture = new CultureInfo(CultureInfo.CurrentCulture.Name)
        {
            NumberFormat =
            {
                NumberDecimalSeparator = ",", NumberGroupSeparator = ".", NumberGroupSizes = new[] { 3 }
            }
        };
        
        var sut = new Number("1,1", culture);
        sut.ResetCulture();
        var act = sut.ToString();

        act.Should().Be("1.1");
    }
    
    [Theory]
    [InlineData(0, "0")]
    [InlineData(0.0, "0")]
    [InlineData(0.01, "0.01")]
    [InlineData(1, "1")]
    [InlineData(1.0, "1")]
    [InlineData(1.01, "1.01")]
    public void Number_ToString(double value, string valueString)
    {
        var act = new Number(value).ToString();
        act.Should().Be(valueString);
    }
    
    [Theory]
    [InlineData(0, "0")]
    [InlineData(0.0, "0")]
    [InlineData(0.01, "0,01")]
    [InlineData(1, "1")]
    [InlineData(1.0, "1")]
    [InlineData(1.01, "1,01")]
    public void Number_ToString_with_culture(double value, string valueString)
    {
        var culture = new CultureInfo(CultureInfo.CurrentCulture.Name)
        {
            NumberFormat =
            {
                NumberDecimalSeparator = ",", NumberGroupSeparator = ".", NumberGroupSizes = new[] { 3 }
            }
        };
        
        var act = new Number(value)
        {
            Culture = culture
        }.ToString();

        act.Should().Be(valueString);
    }
    
    [Theory]
    [InlineData("0")]
    [InlineData("0.0")]
    [InlineData("0.000000000000001")]
    [InlineData("0.0000000000000001")]
    [InlineData("", true, typeof(FormatException), "value is not in the correct format")]
    [InlineData("0,0", true, typeof(FormatException), "value is not in the correct format")]
    [InlineData("1,000.0", true, typeof(FormatException), "value is not in the correct format")]
    [InlineData("1.000,0", true, typeof(FormatException), "value is not in the correct format")]
    [InlineData("1.000.0", true, typeof(FormatException), "value is not in the correct format")]
    public void Number_construct_from_string(string value, bool shouldThrowException = false, Type? exceptionType = null,  string message = "")
    {
        var act = () => new Number(value);

        if (!shouldThrowException)
        {
            act.Should().NotThrow();
            return; 
        }
        
        act.Should().Throw<Exception>().Where(e => e.GetType() == exceptionType).WithMessage(message);
    }
    
    [Theory]
    [InlineData("0,0")]
    [InlineData("1000,0")]
    public void Number_construct_from_string_with_culture(string value)
    {
        var act = () =>
        {
            var culture = new CultureInfo(CultureInfo.CurrentCulture.Name)
            {
                NumberFormat =
                {
                    NumberDecimalSeparator = ",", NumberGroupSeparator = ".", NumberGroupSizes = new[] { 3 }
                }
            };
            
            return new Number(value, culture);
        };

        act.Should().NotThrow();
    }
    
    [Theory]
    [InlineData("0.0")]
    [InlineData("1000.0")]
    public void Number_construct_from_string_with_culture_set(string value)
    {
        var act = () =>
        {
            var culture = new CultureInfo(CultureInfo.CurrentCulture.Name)
            {
                NumberFormat =
                {
                    NumberDecimalSeparator = ",", NumberGroupSeparator = ".", NumberGroupSizes = new[] { 3 }
                }
            };

            return new Number(value) { Culture = culture};
        };

        act.Should().NotThrow();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(0.0)]
    [InlineData(1E-14)]
    [InlineData(1E-14, 0)]
    [InlineData(1E-14, 1)]
    [InlineData(1E-14, 14)]
    [InlineData(1E-14, 15, true, typeof(ArgumentOutOfRangeException), "Must be <= 14, i.e. precision of 1E-14")]
    [InlineData(1E-15)]
    [InlineData(1E-15, 0)]
    [InlineData(1E-15, 15)]
    [InlineData(1E-15, 16, true, typeof(ArgumentOutOfRangeException), "Must be <= 15, i.e. precision of 1E-15")]
    [InlineData(1E+14)]
    [InlineData(1E+14, 0)]
    [InlineData(1E+14, 1, true, typeof(ArgumentOutOfRangeException), "Must be <= 0, i.e. precision of 100000000000000")]
    [InlineData(123.456, 2)]
    [InlineData(123.456, 3)]
    [InlineData(123.456, 4, true, typeof(ArgumentOutOfRangeException), "Must be <= 3, i.e. precision of 123.456")]
    [InlineData(123.45670, 5, true, typeof(ArgumentOutOfRangeException), "Must be <= 4, i.e. precision of 123.456")]
    [InlineData(123456789.123456789, 9, true, typeof(ArgumentOutOfRangeException), "Must be <= 8, i.e. precision of 123456789.12345679")]
    public void Number_construct_from_double(double value, int precision = -1, bool shouldThrowException = false, Type? exceptionType = null,  string message = "")
    {
        var act = () => precision < 0 ? new Number(value) : new Number(value, precision);

        if (!shouldThrowException)
        {
            act.Should().NotThrow();
            return; 
        }

        act.Should().Throw<Exception>().Where(e => e.GetType() == exceptionType).WithMessage($"{message}*");
    }
}