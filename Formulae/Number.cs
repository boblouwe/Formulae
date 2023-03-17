using System.ComponentModel.DataAnnotations;

namespace Formulae
{
    public class Number
    {
        private readonly double _doubleValue;

        public int IntegralPart { get; }
        public int FractionalPart { get; }
        
        [Range(0, 15, ErrorMessage = "Precision must be between 0 and 15")]
        public int Precision { get; init; }

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
            : this(value, 15)
        {
        }

        public Number(double value, int precision)
        {
            if (precision is < 0 or > 15)
            {
                throw new ArgumentOutOfRangeException(nameof(precision), "Must be between 0 and 15");
            }

            Value = value;
            Precision = precision;
            IntegralPart = 0;
            FractionalPart = 0;
        }

        public override string ToString()
        {
            return $"{Value}";
        }
    }
}