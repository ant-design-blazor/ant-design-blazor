// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public sealed class MenuTheme : EnumValue<MenuTheme>
    {
        public static readonly MenuTheme Light = new MenuTheme(nameof(Light).ToLowerInvariant(), 1);
        public static readonly MenuTheme Dark = new MenuTheme(nameof(Dark).ToLowerInvariant(), 2);

        private MenuTheme(string name, int value) : base(name, value)
        {
        }
    }
}
