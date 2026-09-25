// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AntDesign.Icons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntDesign.Docs.Services
{
    /// <summary>
    /// Adapts the icon catalog emitted by AntDesign.Icons' SVG source generator
    /// to the category/string model used by the docs and Select listbox demo.
    /// </summary>
    public class IconListService
    {
        private IList<IconItem>? _icons;

        public IList<IconItem> GetIcons()
        {
            _icons ??= IconCatalog.Categories
                .Select(category => new IconItem
                {
                    Category = category,
                    IconNames = IconCatalog.Items
                        .Where(icon => icon.Category == category)
                        .Select(icon => icon.Name)
                        .Distinct(StringComparer.Ordinal)
                        .OrderBy(name => name, StringComparer.Ordinal)
                        .ToList()
                })
                .Where(item => item.IconNames.Count > 0)
                .ToList();
            return _icons;
        }

        public List<IconItem> Search(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return GetIcons().ToList();
            var query = word.Trim();
            return GetIcons()
                .Select(item => new IconItem
                {
                    Category = item.Category,
                    IconNames = item.IconNames
                        .Where(name => name.Contains(query, StringComparison.OrdinalIgnoreCase))
                        .ToList()
                })
                .Where(item => item.IconNames.Count > 0)
                .ToList();
        }
    }

    public class IconItem
    {
        public string Category { get; set; } = string.Empty;
        public List<string> IconNames { get; set; } = new();
    }
}
