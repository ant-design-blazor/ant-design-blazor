// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AntDesign
{
    internal static class KeepInRangeExtensions
    {
        internal static double EnsureRange(this double input, double max) => EnsureRange(input, 0.0, max);
        internal static double EnsureRange(this double input, double min, double max) => Math.Max(min, Math.Min(max, input));
        internal static byte EnsureRange(this byte input, byte max) => EnsureRange(input, (byte)0, max);
        internal static byte EnsureRange(this byte input, byte min, byte max) => Math.Max(min, Math.Min(max, input));
        internal static byte EnsureRangeToByte(this int input) => (byte)EnsureRange(input, 0, 255);
        internal static int EnsureRange(this int input, int max) => EnsureRange(input, 0, max);
        internal static int EnsureRange(this int input, int min, int max) => Math.Max(min, Math.Min(max, input));
    }
}
