// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using AntDesign.Icons;

namespace AntDesign
{
    internal static class IconStore
    {
        internal static Lazy<IDictionary<IconThemeType, string[]>> AllIconsByTheme = new(() => new Dictionary<IconThemeType, string[]>
        {
            [IconThemeType.Fill] = IconCatalog.ForTheme("filled").Select(icon => icon.Name).ToArray(),
            [IconThemeType.Outline] = IconCatalog.ForTheme("outlined").Select(icon => icon.Name).ToArray(),
            [IconThemeType.TwoTone] = IconCatalog.ForTheme("twotone").Select(icon => icon.Name).ToArray()
        });

        internal static string GetIcon(string iconName, IconThemeType iconTheme)
            => IconCatalog.GetSourceSvg(iconName, GetCatalogTheme(iconTheme));

        private static string GetCatalogTheme(IconThemeType iconTheme) => iconTheme switch
        {
            IconThemeType.Fill => "filled",
            IconThemeType.TwoTone => "twotone",
            _ => "outlined"
        };
    }
}
