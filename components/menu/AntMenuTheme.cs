// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public sealed class AntMenuTheme : EnumValue<AntMenuTheme>
    {
        public static readonly AntMenuTheme Light = new AntMenuTheme(nameof(Light).ToLowerInvariant(), 1);
        public static readonly AntMenuTheme Dark = new AntMenuTheme(nameof(Dark).ToLowerInvariant(), 2);

        private AntMenuTheme(string name, int value) : base(name, value)
        {
        }
    }
}
