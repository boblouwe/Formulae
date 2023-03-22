using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Formulae
{
    public class Number
    {
        private readonly double _doubleValue;

        [Range(0, 15, ErrorMessage = "Precision must be between 0 and 15")]
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
            : this(value, CultureInfo.CurrentCulture)
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
            if (precision is < 0 or > 15)
            {
                throw new ArgumentOutOfRangeException(nameof(precision), "Must be >= 0 and <= 15");
            }

            var valuePrecision = GetPrecision(value);
            if (precision > valuePrecision)
            {
                throw new ArgumentOutOfRangeException(nameof(precision),
                    $"Must be <= {valuePrecision}, i.e. precision of {value}");
            }
            
            Value = value;
            Precision = precision;
        }

        private static int GetPrecision(double value)
        {
            var valueString = value.ToString(CultureInfo.InvariantCulture);
            var decimalPointIndex = valueString.LastIndexOf('.');

            return decimalPointIndex == -1 ? 0 : valueString[(decimalPointIndex + 1)..].Length;
        }

        public override string ToString()
        {
            return $"{Value}";
        }
    }
}