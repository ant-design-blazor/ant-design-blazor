// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Runtime.CompilerServices;

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
        private readonly bool _parsed;

        private readonly CssSizeLengthUnit _unit;

        public override string ToString()
        {
            var numericValue = _value?.ToString(CultureInfo.InvariantCulture);
            var unit = _unit switch
            {
                CssSizeLengthUnit.Percent => "%",
                CssSizeLengthUnit.Calc or CssSizeLengthUnit.NoUnit => "",
#if NET5_0_OR_GREATER
                _ => Enum.GetName(_unit).ToLowerInvariant()
#else
                _ => Enum.GetName(typeof(CssSizeLengthUnit), _unit).ToLowerInvariant()
#endif
            };

            return $"{numericValue ?? StringValue}{unit}";
        }

        private CssSizeLength(decimal value, CssSizeLengthUnit unit)
        {
            _value = value;
            _stringValue = null;
            _unit = unit;
            _parsed = true;
        }

        private static CssSizeLengthUnit EvalUnitless(bool noUnit) => (noUnit ? CssSizeLengthUnit.NoUnit : CssSizeLengthUnit.Px);

        public CssSizeLength(int value, bool noUnit = false) : this(value, EvalUnitless(noUnit))
        {
        }

        public CssSizeLength(double value, bool noUnit = false) : this(Convert.ToDecimal(value), EvalUnitless(noUnit))
        {
        }

        public CssSizeLength(decimal value, bool noUnit = false) : this(value, EvalUnitless(noUnit))
        {
        }

        public CssSizeLength(string value)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(value);
#else
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
#endif
            value = value.Trim().ToLowerInvariant();
            if (value.Length < 1)
            {
                // Should it throw an ArgumentException?
                _stringValue = value;
                _value = null;
                _unit = CssSizeLengthUnit.Calc;
                _parsed = false;
                return;
            }

            _stringValue = value;
            _parsed = true;

            if (TryResolveByPrefix(value, out var unit))
            {
                if (unit == CssSizeLengthUnit.NoUnit)
                {
                    _parsed = false;
                }
                _value = null;
                _unit = unit;
                return;
            }

            _value = ResolveBySuffix(value, out unit);

            switch (unit)
            {
                case CssSizeLengthUnit.NoUnit:
                    _parsed = false;
                    break;

                case CssSizeLengthUnit.Px:
                    if (IsNumberOrDot(value[^1]))
                    {
                        _stringValue = null;
                    }
                    break;
            }
            _unit = unit;
        }

        public static implicit operator CssSizeLength(int value) => new(value);

        public static implicit operator CssSizeLength(double value) => new(value);

        public static implicit operator CssSizeLength(decimal value) => new(value);

        public static implicit operator CssSizeLength(string value) => new(value);

        public static implicit operator string(CssSizeLength value) => value.ToString();

        public override bool Equals(object obj) => obj is CssSizeLength other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(_value, _unit);

        public static bool operator ==(CssSizeLength left, CssSizeLength right) => left.Equals(right);

        public static bool operator !=(CssSizeLength left, CssSizeLength right) => !(left == right);

        public bool Equals(CssSizeLength other) => other._value == _value && other._unit == _unit;

        public static bool TryParse(string value, out CssSizeLength cssSizeLength)
        {
            cssSizeLength = new(value);
            return cssSizeLength._parsed;
        }

        private static bool TryResolveByPrefix(string trimmedValue, out CssSizeLengthUnit unit)
        {
            if (trimmedValue.StartsWith("calc", StringComparison.OrdinalIgnoreCase))
            {
                unit = CssSizeLengthUnit.Calc;
                return true;
            }

            if (!IsNumberOrDot(trimmedValue[0]))
            {
                unit = CssSizeLengthUnit.NoUnit;
                return true;
            }

            unit = default;
            return false;
        }

        private static decimal ResolveBySuffix(string trimmedValue, out CssSizeLengthUnit unit)
        {
            // Notice: before this method, we've made sure that at least the first char is number or dot.
            // So all the logics below are based on this promise.
            // Also, we don't need to handle all kinds of input ("1@1", "1@1%", "1 px", ".1.1" for example).
            // Let `decimal.Parse` decides whether throws a format exception if the input is out of expect.

            if (trimmedValue[^1] == '%')
            {
                unit = CssSizeLengthUnit.Percent;
                return decimal.Parse(trimmedValue.AsSpan(0, trimmedValue.Length - 1), provider: CultureInfo.InvariantCulture);
            }

            if (IsNumberOrDot(trimmedValue[^1]))
            {
                unit = CssSizeLengthUnit.Px;
                return decimal.Parse(trimmedValue, provider: CultureInfo.InvariantCulture);
            }

            var index = trimmedValue.Length - 2;
            while (index >= 0)
            {
                if (IsNumberOrDot(trimmedValue[index]))
                {
                    break;
                }
                index--;
            }

#if NET6_0_OR_GREATER
            if (!Enum.TryParse(trimmedValue.AsSpan(index + 1), ignoreCase: true, out unit))
#else
            if (!Enum.TryParse(trimmedValue[(index + 1)..], ignoreCase: true, out unit))
#endif
            {
                unit = CssSizeLengthUnit.NoUnit;
            }
            return decimal.Parse(trimmedValue.AsSpan(0, index + 1), provider: CultureInfo.InvariantCulture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNumberOrDot(char c) => (c >= '0' && c <= '9') || c == '.';
    }
}
