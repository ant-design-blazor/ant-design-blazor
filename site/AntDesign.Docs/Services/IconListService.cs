// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AntDesign.Icons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntDesign.Docs.Services
{
    /// <summary>
    /// Joins the generated SVG inventory from AntDesign.Icons with the docs-owned
    /// category catalog synchronized from Ant Design's IconSearch metadata.
    /// </summary>
    public class IconListService
    {
        private IList<IconItem>? _icons;

        public IList<IconItem> GetIcons()
        {
            _icons ??= IconSearchCategories.Categories
                .Select(category => new IconItem
                {
                    Category = category,
                    IconNames = IconCatalog.Items
                        .Where(icon => IconSearchCategories.GetCategory(icon.ComponentType.Name) == category)
                        .OrderBy(icon => IconSearchCategories.GetOrder(icon.ComponentType.Name))
                        .ThenBy(icon => icon.Name, StringComparer.Ordinal)
                        .Select(icon => icon.ComponentType.Name)
                        .Distinct(StringComparer.Ordinal)
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
