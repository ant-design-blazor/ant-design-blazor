using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AntDesign
{
    internal static class InputNumberMath
    {
        #region Round

        public static float Round(float x, int digits)
        {
            return MathF.Round(x, digits);
        }

        public static float? Round(float? x, int digits)
        {
            if (x.HasValue == false) return x;
            return MathF.Round(x.Value, digits);
        }

        public static decimal Round(decimal d, int decimals)
        {
            return Math.Round(d, decimals);
        }

        public static decimal? Round(decimal? d, int decimals)
        {
            if (d.HasValue == false) return d;
            return Math.Round(d.Value, decimals);
        }

        public static double Round(double value, int digits)
        {
            return Math.Round(value, digits);
        }
        public static double? Round(double? value, int digits)
        {
            if (value.HasValue == false) return value;
            return Math.Round(value.Value, digits);
        }
        #endregion

        #region Add
        public static sbyte Add(sbyte left, sbyte right)
        {
            return (sbyte)(left + right);
        }

        public static byte Add(byte left, byte right)
        {
            return (byte)(left + right);
        }

        public static short Add(short left, short right)
        {
            return (short)(left + right);
        }

        public static ushort Add(ushort left, ushort right)
        {
            return (ushort)(left + right);
        }
        #endregion

        #region Subtract
        public static sbyte Subtract(sbyte left, sbyte right)
        {
            return (sbyte)(left - right);
        }

        public static byte Subtract(byte left, byte right)
        {
            return (byte)(left - right);
        }

        public static short Subtract(short left, short right)
        {
            return (short)(left - right);
        }

        public static ushort Subtract(ushort left, ushort right)
        {
            return (ushort)(left - right);
        }
        #endregion

        #region Parse
        public static sbyte Parse(string input, sbyte defaultValue, CultureInfo culture)
        {
            return sbyte.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static sbyte? Parse(string input, sbyte? defaultValue, CultureInfo culture)
        {
            return sbyte.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static byte Parse(string input, byte defaultValue, CultureInfo culture)
        {
            return byte.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static byte? Parse(string input, byte? defaultValue, CultureInfo culture)
        {
            return byte.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static short Parse(string input, short defaultValue, CultureInfo culture)
        {
            return short.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static short? Parse(string input, short? defaultValue, CultureInfo culture)
        {
            return short.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static ushort Parse(string input, ushort defaultValue, CultureInfo culture)
        {
            return ushort.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static ushort? Parse(string input, ushort? defaultValue, CultureInfo culture)
        {
            return ushort.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static int Parse(string input, int defaultValue, CultureInfo culture)
        {
            return int.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static int? Parse(string input, int? defaultValue, CultureInfo culture)
        {
            return int.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static uint Parse(string input, uint defaultValue, CultureInfo culture)
        {
            return uint.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static uint? Parse(string input, uint? defaultValue, CultureInfo culture)
        {
            return uint.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static long Parse(string input, long defaultValue, CultureInfo culture)
        {
            return long.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static long? Parse(string input, long? defaultValue, CultureInfo culture)
        {
            return long.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static ulong Parse(string input, ulong defaultValue, CultureInfo culture)
        {
            return ulong.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static ulong? Parse(string input, ulong? defaultValue, CultureInfo culture)
        {
            return ulong.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static float Parse(string input, float defaultValue, CultureInfo culture)
        {
            return float.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static float? Parse(string input, float? defaultValue, CultureInfo culture)
        {
            return float.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static double Parse(string input, double defaultValue, CultureInfo culture)
        {
            return double.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static double? Parse(string input, double? defaultValue, CultureInfo culture)
        {
            return double.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static decimal Parse(string input, decimal defaultValue, CultureInfo culture)
        {
            return decimal.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }

        public static decimal? Parse(string input, decimal? defaultValue, CultureInfo culture)
        {
            return decimal.TryParse(input, NumberStyles.Any, culture, out var number) ? number : defaultValue;
        }
    }
    #endregion
}
