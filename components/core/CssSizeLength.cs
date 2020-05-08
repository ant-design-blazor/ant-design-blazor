using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AntBlazor
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
    }

    public readonly struct CssSizeLength : IEquatable<CssSizeLength>
    {
        private readonly int _value;

        private readonly CssSizeLengthUnit _unit;

        public override string ToString() => _value.ToString(CultureInfo.InvariantCulture) + _unit switch
        {
            CssSizeLengthUnit.Percent => "%",
            _ => Enum.GetName(typeof(CssSizeLengthUnit), _unit).ToLowerInvariant()
        };

        private CssSizeLength(int value, CssSizeLengthUnit unit)
        {
            _value = value;
            _unit = unit;
        }

        public CssSizeLength(int value) : this(value, CssSizeLengthUnit.Px) { }

        public CssSizeLength(string value)
        {
            value = value?.ToLowerInvariant() ?? throw new ArgumentNullException(nameof(value));

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
                return;
            }

            if (!Enum.TryParse(value.Substring(index).Trim(), out _unit))
            {
                throw new FormatException();
            }
        }

        public static implicit operator CssSizeLength(int value) => new CssSizeLength(value);

        public static implicit operator CssSizeLength(string value) => new CssSizeLength(value);

        public override bool Equals(object obj) => obj is CssSizeLength other && Equals(other);

        public override int GetHashCode() => unchecked(_value.GetHashCode() + _unit.GetHashCode());

        public static bool operator ==(CssSizeLength left, CssSizeLength right) => left.Equals(right);

        public static bool operator !=(CssSizeLength left, CssSizeLength right) => !(left == right);

        public bool Equals(CssSizeLength other) => other._value == _value && other._unit == _unit;
    }
}
