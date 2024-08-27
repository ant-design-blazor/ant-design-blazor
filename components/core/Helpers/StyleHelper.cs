// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public static class StyleHelper
    {
        //fix the user set 100% or xxxVH etc..
        public static string ToCssPixel(string value) => int.TryParse(value, out var _) ? $"{value}px" : value;

        public static string ToCssPixel(int value) => $"{value}px";
    }
}
