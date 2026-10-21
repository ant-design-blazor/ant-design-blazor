// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>Displays checkable tags as a single- or multiple-selection group.</summary>
    public partial class CheckableTagGroup
    {
        /// <summary>Options to render.</summary>
        [Parameter] public IReadOnlyList<CheckableTagOption> Options { get; set; }

        /// <summary>Use multiple selection when true.</summary>
        [Parameter] public bool Multiple { get; set; }

        /// <summary>Selected value in single-selection mode.</summary>
        [Parameter] public string Value { get; set; }

        /// <summary>Callback for single-selection changes.</summary>
        [Parameter] public EventCallback<string> ValueChanged { get; set; }

        /// <summary>Selected values in multiple-selection mode.</summary>
        [Parameter] public IReadOnlyList<string> Values { get; set; }

        /// <summary>Callback for multiple-selection changes.</summary>
        [Parameter] public EventCallback<IReadOnlyList<string>> ValuesChanged { get; set; }

        /// <summary>Disables all options.</summary>
        [Parameter] public bool Disabled { get; set; }

        /// <summary>Semantic classes keyed by root and item.</summary>
        [Parameter] public IReadOnlyDictionary<string, string> ClassNames { get; set; }

        /// <summary>Semantic inline styles keyed by root and item.</summary>
        [Parameter] public IReadOnlyDictionary<string, string> Styles { get; set; }

        private string SemanticClass(string slot, string baseClass)
        {
            var customClass = ClassNames != null && ClassNames.TryGetValue(slot, out var value) ? value : null;
            var classes = string.Join(" ", new[] { baseClass, slot == "root" ? Class : null, customClass }.Where(x => !string.IsNullOrWhiteSpace(x)));
            return classes;
        }

        private string SemanticStyle(string slot, string baseStyle = null)
        {
            var customStyle = Styles != null && Styles.TryGetValue(slot, out var value) ? value : null;
            return JoinStyles(baseStyle, customStyle);
        }

        private string GetItemClass(CheckableTagOption option)
        {
            var customClass = ClassNames != null && ClassNames.TryGetValue("item", out var value) ? value : null;
            return string.Join(" ", new[] { "ant-tag-checkable-group-item", customClass, option.Class }.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        private string GetItemStyle(CheckableTagOption option)
        {
            var semanticStyle = Styles != null && Styles.TryGetValue("item", out var value) ? value : null;
            return JoinStyles(semanticStyle, option.Style);
        }

        private static string JoinStyles(params string[] styles)
        {
            return string.Join(" ", styles.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim().TrimEnd(';') + ";"));
        }

        private async Task HandleChange(string optionValue, bool checkedValue)
        {
            if (Multiple)
            {
                var values = Values?.ToList() ?? new List<string>();
                if (checkedValue && !values.Contains(optionValue))
                    values.Add(optionValue);
                else if (!checkedValue)
                    values.Remove(optionValue);
                await ValuesChanged.InvokeAsync(values);
            }
            else
            {
                await ValueChanged.InvokeAsync(checkedValue ? optionValue : null);
            }
        }
    }
}
