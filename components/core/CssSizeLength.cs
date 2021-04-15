using System;
using System.Globalization;
using System.Linq;

namespace AntDesign
{
    internal enum CssSizeLengthUnit
    {
        Px,
        Cm,
        Mm,
        In,
        Pt,
        Pc,
        Em,
        Rem,
        Percent,
        Vh,
        Vw,
        Vmin,
        Vmax,
        Fr,
        Calc,
        NoUnit,
    }

    public readonly struct CssSizeLength : IEquatable<CssSizeLength>
    {
        public decimal Value => _value ?? 0;

        public string StringValue => _stringValue;

        internal CssSizeLengthUnit Unit => _unit;

        private readonly decimal? _value;
        private readonly string _stringValue;

        private readonly CssSizeLengthUnit _unit;

        public override string ToString()
        {
            var numericValue = _value?.ToString(CultureInfo.InvariantCulture);
            var unit = _unit switch
            {
                CssSizeLengthUnit.Percent => "%",
                CssSizeLengthUnit.Calc or CssSizeLengthUnit.NoUnit => "",
                _ => Enum.GetName(typeof(CssSizeLengthUnit), _unit).ToLowerInvariant()
            };

            return $"{numericValue ?? StringValue}{unit}";
        }

        private CssSizeLength(decimal value, CssSizeLengthUnit unit)
        {
            _value = value;
            _stringValue = null;
            _unit = unit;
        }

        private static CssSizeLengthUnit EvalUnitless(bool noUnit) => (noUnit ? CssSizeLengthUnit.NoUnit : CssSizeLengthUnit.Px);

        public CssSizeLength(int value, bool noUnit = false) : this(value, EvalUnitless(noUnit)) { }
        public CssSizeLength(double value, bool noUnit = false) : this(Convert.ToDecimal(value), EvalUnitless(noUnit)) { }
        public CssSizeLength(decimal value, bool noUnit = false) : this(value, EvalUnitless(noUnit)) { }

        public CssSizeLength(string value)
        {
            value = value?.ToLowerInvariant() ?? throw new ArgumentNullException(nameof(value));
            _stringValue = value;

            if (value.StartsWith("calc", StringComparison.OrdinalIgnoreCase))
            {
                _stringValue = value;
                _value = null;
                _unit = CssSizeLengthUnit.Calc;
                return;
            }

            var index = value
                .Select((c, i) => ((char c, int i)?)(c, i))
                .FirstOrDefault(x => !(x.Value.c == '.' || x.Value.c >= '0' && x.Value.c <= '9'))
                ?.i
                ?? value.Length;

            if (index == 0)
            {
                throw new FormatException();
            }

            _value = int.Parse(value.Substring(0, index), CultureInfo.InvariantCulture);

            if (index == value.Length)
            {
                _unit = CssSizeLengthUnit.Px;
                _stringValue = null;
                return;
            }

            var unit = value.Substring(index).Trim();
            if (!Enum.TryParse(unit, ignoreCase: true, out _unit))
            {
                _unit = unit switch
                {
                    "%" => CssSizeLengthUnit.Percent,
                    _ => throw new FormatException(),
                };
            }
        }

        public static implicit operator CssSizeLength(int value) => new CssSizeLength(value);

        public static implicit operator CssSizeLength(double value) => new CssSizeLength(value);

        public static implicit operator CssSizeLength(decimal value) => new CssSizeLength(value);

        public static implicit operator CssSizeLength(string value) => new CssSizeLength(value);

        public override bool Equals(object obj) => obj is CssSizeLength other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(_value, _unit);

        public static bool operator ==(CssSizeLength left, CssSizeLength right) => left.Equals(right);

        public static bool operator !=(CssSizeLength left, CssSizeLength right) => !(left == right);

        public bool Equals(CssSizeLength other) => other._value == _value && other._unit == _unit;
    }
}
