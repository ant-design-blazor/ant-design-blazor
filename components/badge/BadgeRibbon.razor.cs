// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// Small numerical value or status descriptor for UI elements.
    /// </summary>
    public partial class BadgeRibbon : AntDomComponentBase
    {
        /// <summary>
        /// Customize ribbon color
        /// </summary>
        [Parameter]
        public OneOf<BadgeColor?, string> Color { get; set; }

        /// <summary>
        /// Text string of ribbon.
        /// </summary>
        [Parameter]
        public string Text { get; set; }

        /// <summary>
        /// Text content of the ribbon. Takes priority over <see cref="Text"/>
        /// </summary>
        [Parameter]
        public RenderFragment TextTemplate { get; set; }

        /// <summary>
        /// Set placement of ribbon.
        /// </summary>
        /// <default value="end"/>
        [Parameter]
        public string Placement { get; set; } = "end";

        /// <summary>
        /// Wrapping this item.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private string PresetColor => Color.IsT0 && Color.AsT0.HasValue ?
            Enum.GetName(typeof(BadgeColor), Color.AsT0).ToLowerInvariant()
            : Color.IsT1 && Color.AsT1.ToLowerInvariant().IsIn(_badgePresetColors) ?
            Color.AsT1.ToLowerInvariant() : null;

        private string _colorStyle;

        private string _cornerColorStyle;

        /// <summary>
        /// Sets the default CSS classes.
        /// </summary>
        private void SetClassMap()
        {
            var prefixName = "ant-ribbon";
            ClassMapper.Clear()
                .Add(prefixName)
                .Add($"{prefixName}-placement-{Placement}")
                .If($"{prefixName}-rtl", () => RTL)
                .If($"{prefixName}-color-{PresetColor}", () => PresetColor != null)
                ;
        }

        private void SetStyle()
        {
            if (PresetColor == null && Color.IsT1 && !string.IsNullOrWhiteSpace(Color.AsT1))
            {
                _colorStyle = $"background:{Color.AsT1.ToLowerInvariant()}; {Style}";
                _cornerColorStyle = $"color:{Color.AsT1.ToLowerInvariant()}; {Style}";
            }
            else
            {
                _colorStyle = Style;
                _cornerColorStyle = Style;
            }
        }

        /// <summary>
        /// Startup code
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
            SetStyle();
        }

        /// <summary>
        /// Runs every time a parameter is set.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClassMap();
            SetStyle();
        }

        private readonly static string[] _badgePresetColors =
        [
            "pink", "red", "yellow", "orange", "cyan", "green", "blue", "purple", "geekblue", "magenta", "volcano",
            "gold", "lime"
        ];
    }
}
