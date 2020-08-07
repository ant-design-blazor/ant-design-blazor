using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    internal static class InputNumberMath
    {
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

        public static int Round(int value, int digits)
        {
            return value;
        }

        public static int? Round(int? value, int digits)
        {
            return value;
        }
    }
}
