// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /**
    <summary>
        <para>A divider line separates different content.</para>

        <h2>When To Use</h2>

        <list type="bullet">
            <item>Divide sections of article.</item>
            <item>Divide inline text and links such as the operation column of table.</item>
        </list>
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Layout, "https://gw.alipayobjects.com/zos/alicdn/5swjECahe/Divider.svg", Title = "Divider", SubTitle = "分割线")]
    public partial class Divider : AntDomComponentBase
    {
        /// <summary>
        /// Content to show inside the divider
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Text to show inside the divider
        /// </summary>
        [Parameter]
        public string Text { get; set; }

        /// <summary>
        /// When false, the text will not be a header style. When true it will be header style.
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Plain { get; set; } = false;

        /// <summary>
        /// Type of divider - `DividerType.Horizontal` | `DividerType.Vertical`
        /// </summary>
        /// <default value="DividerType.Horizontal" />
        [Parameter]
        public DividerType Type { get; set; } = DividerType.Horizontal;

        /// <summary>
        /// Content/Text orientation - `DividerOrientation.Left` | `DividerOrientation.Right` | `DividerOrientation.Center`. Ignored when not using `Text` or `ChildContent`
        /// </summary>
        /// <default value="DividerOrientation.Center" />
        [Parameter]
        public DividerOrientation Orientation { get; set; } = DividerOrientation.Center;

        /// <summary>
        /// Whether to style the line as dashed or not.
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Dashed { get; set; } = false;

        /// <summary>
        /// Whether line is dashed, dotted or solid.
        /// </summary>
        /// <default value="solid" />
        [Parameter]
        public string Variant { get; set; } = "solid";

        /// <summary>
        /// Horizontal divider spacing size. Supported values are small, middle, and large.
        /// </summary>
        [Parameter]
        public string Size { get; set; }

        /// <summary>
        /// Semantic class names keyed by root, rail, and content.
        /// </summary>
        [Parameter]
        public IReadOnlyDictionary<string, string> ClassNames { get; set; }

        /// <summary>
        /// Semantic inline styles keyed by root, rail, and content.
        /// </summary>
        [Parameter]
        public IReadOnlyDictionary<string, string> Styles { get; set; }

        private void SetClass()
        {
            ClassMapper.Clear()
                .Add("ant-divider")
                .If("ant-divider", () => RTL)
                .Get(() => $"ant-divider-{Type.ToString().ToLowerInvariant()}")
                .If("ant-divider-with-text", () => Text != null || ChildContent != null)
                .GetIf(() => $"ant-divider-with-text-{GetTitlePlacement()}", () => Text != null || ChildContent != null)
                .If($"ant-divider-plain", () => Plain && (Text != null || ChildContent != null))
                .If("ant-divider-dashed", () => Dashed || string.Equals(Variant, "dashed", StringComparison.OrdinalIgnoreCase))
                .If("ant-divider-dotted", () => string.Equals(Variant, "dotted", StringComparison.OrdinalIgnoreCase))
                .If("ant-divider-rail", () => Text == null && ChildContent == null)
                .If("ant-divider-sm", () => string.Equals(Size, "small", StringComparison.OrdinalIgnoreCase))
                .If("ant-divider-md", () => string.Equals(Size, "middle", StringComparison.OrdinalIgnoreCase) || string.Equals(Size, "medium", StringComparison.OrdinalIgnoreCase))
                ;
        }

        internal string SemanticClass(string slot, string baseClass)
        {
            if (ClassNames == null || !ClassNames.TryGetValue(slot, out var customClass) || string.IsNullOrWhiteSpace(customClass))
                return baseClass;
            return string.IsNullOrWhiteSpace(baseClass) ? customClass : $"{baseClass} {customClass}";
        }

        internal string SemanticStyle(string slot, string baseStyle = null)
        {
            var semanticStyle = Styles != null && Styles.TryGetValue(slot, out var value) ? value : null;
            if (string.IsNullOrWhiteSpace(baseStyle)) return semanticStyle;
            if (string.IsNullOrWhiteSpace(semanticStyle)) return baseStyle;
            return $"{baseStyle.Trim().TrimEnd(';')}; {semanticStyle}";
        }

        private string GetTitlePlacement()
        {
            return Orientation switch
            {
                DividerOrientation.Left => RTL ? "end" : "start",
                DividerOrientation.Right => RTL ? "start" : "end",
                _ => "center",
            };
        }

        protected override void OnParametersSet()
        {
            SetClass();
            base.OnParametersSet();
        }
    }
}
