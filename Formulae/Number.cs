using System.Globalization;

namespace Formulae
{
    public class Number
    {
        private readonly double _doubleValue;

        public static CultureInfo DefaultCulture => CultureInfo.InvariantCulture;

        public CultureInfo Culture { get; set; } = DefaultCulture;

        public int Precision { get; }

        public double Value
        {
            get
            {
                var precision = Math.Max(0, Precision);
                precision = Math.Min(15, precision);
                return Math.Round(_doubleValue, precision);
            }

            private init => _doubleValue = value;
        }
        
        public static Number NaN => new(double.NaN);

        public Number() 
            : this(0)
        {
        }

        public Number(double value)
        {
            Value = value;
            Precision = GetPrecision(value);

        }

        public Number(string value)
            : this(value, DefaultCulture)
        {
        }
        
        public Number(string value, IFormatProvider formatProvider)
        {
            if (double.TryParse(value, NumberStyles.Float, formatProvider, out var doubleValue))
            {
                Value = doubleValue;
                Precision = GetPrecision(doubleValue);
            }
            else
            {
                throw new FormatException($"{nameof(value)} is not in the correct format");
            }
        }

        public Number(double value, int precision)
        {
            var valuePrecision = GetPrecision(value);
            if (precision > valuePrecision)
            {
                var valueString = value.ToString(CultureInfo.InvariantCulture);
                throw new ArgumentOutOfRangeException(nameof(precision),
                    $"Must be <= {valuePrecision}, i.e. precision of {valueString}");
            }
            
            Value = value;
            Precision = precision;
        }

        public void ResetCulture()
        {
            Culture = DefaultCulture;
        }

        private static int GetPrecision(double value)
        {
            var valueString = value.ToString("G", CultureInfo.InvariantCulture);
            var decimalPointIndex = valueString.LastIndexOf('.');

            if (!valueString.Contains('E'))
            {
                return decimalPointIndex == -1 ? 0 : valueString[(decimalPointIndex + 1)..].Length;
            }

            var decimals = 0;

            var exponentSignPosition = valueString.LastIndexOf('E');
            if (decimalPointIndex > -1)
            {
                decimals = exponentSignPosition - decimalPointIndex;
            }

            var exponent = int.Parse(valueString[(exponentSignPosition + 1)..]);
            return decimals - exponent;
        }

        public override string ToString()
        {
            return Value.ToString("G", Culture);
        }
    }
}