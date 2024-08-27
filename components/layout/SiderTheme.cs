// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public sealed class SiderTheme : EnumValue<SiderTheme>
    {
        public static readonly SiderTheme Light = new SiderTheme(nameof(Light).ToLowerInvariant(), 1);
        public static readonly SiderTheme Dark = new SiderTheme(nameof(Dark).ToLowerInvariant(), 2);

        private SiderTheme(string name, int value) : base(name, value)
        {
        }
    }
}
