// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public static class MathExtensions
    {
        public static double Map(double sourceMin, double sourceMax, double targetMin, double targetMax, double value) =>
            ((value) / (sourceMax - sourceMin)) * (targetMax - targetMin);
    }
}
