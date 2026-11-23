// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AntDesign.Core.Documentation;
using AntDesign.Core.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
    <para>Tag for categorizing or markup.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>It can be used to tag by dimension or property.</item>
        <item>When categorizing.</item>
    </list>
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/alicdn/cH1BOLfxC/Tag.svg", Title = "Tag", SubTitle = "标签")]
    public partial class Tag : AntDomComponentBase
    {
        /// <summary>
        /// Tag content
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Whether the Tag can be closed
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool? Closable { get; set; }

        /// <summary>Custom close icon content. Supplying it also enables the close control unless <see cref="Closable"/> is explicitly false.</summary>
        [Parameter]
        public RenderFragment CloseIcon { get; set; }

        /// <summary>URL that turns the tag root into an anchor element.</summary>
        [Parameter]
        public string Href { get; set; }

        /// <summary>Target browsing context when <see cref="Href"/> is set.</summary>
        [Parameter]
        public string Target { get; set; }

        /// <summary>Additional class applied to the tag root, matching React's <c>rootClassName</c>.</summary>
        [Parameter]
        public string RootClassName { get; set; }

        /// <summary>Deprecated compatibility alias. Use <see cref="Variant"/> instead.</summary>
        [Parameter]
        [Obsolete("Use Variant instead")]
        public bool? Bordered { get; set; }

        /// <summary>
        /// Whether the Tag can be checked
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Checkable { get; set; }

        /// <summary>
        /// Checked status of Tag
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Checked { get; set; }

        /// <summary>Whether the checkable tag is disabled.</summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>Whether to prevent the default action when the Tag is pressed with a mouse.</summary>
        [Parameter]
        public bool PreventMouseDownDefault { get; set; }

        /// <summary>Whether to stop mouse down events from bubbling past the Tag.</summary>
        [Parameter]
        public bool StopMouseDownPropagation { get; set; }

        /// <summary>Visual variant. Defaults to filled, or solid for legacy inverse colors.</summary>
        [Parameter] public TagVariant? Variant { get; set; }

        private TagVariant ResolvedVariant => Variant ?? (_isPresetColor && _colorMap[_presetColor].EndsWith("-inverse", StringComparison.Ordinal) ? TagVariant.Solid : TagVariant.Filled);

        internal bool ShowCloseIcon => Closable ?? CloseIcon != null;

        /// <summary>
        /// Callback executed when Tag is checked/unchecked
        /// </summary>
        [Parameter]
        public EventCallback<bool> CheckedChanged { get; set; }

        /// <summary>
        /// Tag color. Can either be a predefined color (TagColor)
        /// or hex color.
        /// </summary>
        /// <default value="TagColor.Default" />
        [Parameter]
        public OneOf<TagColor, string> Color
        {
            get => _isPresetColor ? _presetColor : _customColor;

            set
            {
                if (value.IsT0)
                {
                    _isPresetColor = true;
                    _presetColor = value.AsT0;
                }
                else
                {
                    ResolveColorStrInput(value.AsT1);
                }
            }
        }

        /// <summary>
        /// Tag color from the PresetColor list
        /// </summary>
        [Parameter]
        [Obsolete("Use Color instead")]
        public string PresetColor { get; set; }

        /// <summary>
        /// Set the tag's icon 
        /// </summary>
        [Parameter]
        public string Icon { get; set; }

        /// <summary>
        /// Define the icon theme.
        /// </summary>
        /// <default value="IconThemeType.Outline" />
        [Parameter]
        public IconThemeType IconTheme { get; set; } = IconThemeType.Outline;

        /// <summary>
        /// Custom icon template for rendering custom content before the tag content
        /// </summary>
        [Parameter]
        [PublicApi("1.5.2")]
        public RenderFragment IconTemplate { get; set; }

        /// <summary>Semantic classes keyed by root, icon, content, and close.</summary>
        [Parameter]
        public IReadOnlyDictionary<string, string> ClassNames { get; set; }

        /// <summary>Semantic inline styles keyed by root, icon, content, and close.</summary>
        [Parameter]
        public IReadOnlyDictionary<string, string> Styles { get; set; }

        [Parameter]
        [Obsolete("Parameter is not used and does not affect functionality")]
        public bool NoAnimation { get; set; }

        /// <summary>
        /// Callback executed when tag is closed
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClose { get; set; }

        /// <summary>
        /// Triggered before true closing, can prevent the closing
        /// </summary>
        [Parameter]
        public EventCallback<CloseEventArgs<MouseEventArgs>> OnClosing { get; set; }

        /// <summary>
        /// Callback executed when tag is clicked (it is not called 
        /// when closing icon is clicked).
        /// </summary>
        [Parameter]
        public EventCallback OnClick { get; set; }

        /// <summary>Callback invoked for key presses on the Tag root.</summary>
        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }

        /// <summary>
        /// Whether the Tag is closed or not
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool Visible { get; set; } = true;

        private bool _closed;
        private bool _isPresetColor = true;
        private TagColor _presetColor = TagColor.Default;
        private string _customColor = "";
        private string _style;

        protected override void OnParametersSet()
        {
            this._style = GetStyle();
            base.OnParametersSet();
        }

        protected override void OnInitialized()
        {
            this.UpdateClassMap();
            base.OnInitialized();
        }

        private static readonly Dictionary<TagColor, string> _colorMap = new()
        {
            [TagColor.Default] = "default",
            [TagColor.Red] = "red",
            [TagColor.Volcano] = "volcano",
            [TagColor.Orange] = "orange",
            [TagColor.Gold] = "gold",
            [TagColor.Yellow] = "yellow",
            [TagColor.Lime] = "lime",
            [TagColor.Green] = "green",
            [TagColor.Cyan] = "cyan",
            [TagColor.Blue] = "blue",
            [TagColor.GeekBlue] = "geekblue",
            [TagColor.Purple] = "purple",
            [TagColor.Magenta] = "magenta",
            [TagColor.Pink] = "pink",
            [TagColor.Success] = "success",
            [TagColor.Processing] = "processing",
            [TagColor.Error] = "error",
            [TagColor.Warning] = "warning",
            [TagColor.DefaultInverse] = "default-inverse",
            [TagColor.RedInverse] = "red-inverse",
            [TagColor.VolcanoInverse] = "volcano-inverse",
            [TagColor.OrangeInverse] = "orange-inverse",
            [TagColor.GoldInverse] = "gold-inverse",
            [TagColor.YellowInverse] = "yellow-inverse",
            [TagColor.LimeInverse] = "lime-inverse",
            [TagColor.GreenInverse] = "green-inverse",
            [TagColor.CyanInverse] = "cyan-inverse",
            [TagColor.BlueInverse] = "blue-inverse",
            [TagColor.GeekBlueInverse] = "geekblue-inverse",
            [TagColor.PurpleInverse] = "purple-inverse",
            [TagColor.MagentaInverse] = "magenta-inverse",
            [TagColor.PinkInverse] = "pink-inverse",
            [TagColor.SuccessInverse] = "success-inverse",
            [TagColor.ProcessingInverse] = "processing-inverse",
            [TagColor.ErrorInverse] = "error-inverse",
            [TagColor.WarningInverse] = "warning-inverse",
        };

        private void ResolveColorStrInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                _isPresetColor = true;
                _presetColor = TagColor.Default;
                return;
            }
            var trimmedInput = input.Trim();
            // Fast check
            if (trimmedInput.StartsWith('#') || trimmedInput.StartsWith("rgb(", StringComparison.OrdinalIgnoreCase))
            {
                _isPresetColor = false;
                _customColor = trimmedInput;
                return;
            }
            // Slow check
            foreach (var item in _colorMap)
            {
                if (trimmedInput.Equals(item.Value, StringComparison.OrdinalIgnoreCase))
                {
                    _isPresetColor = true;
                    _presetColor = item.Key;
                    return;
                }
            }
            _isPresetColor = false;
            _customColor = trimmedInput;
        }

        private const string StylePrefix = "ant-tag";

        private void UpdateClassMap()
        {
            this.ClassMapper.Add(StylePrefix)
                .Get(() => $"{StylePrefix}-{ResolvedVariant.ToString().ToLowerInvariant()}")
                .Add(GetSemanticValue(ClassNames, "root"))
                .Add(RootClassName)
                .If($"{StylePrefix}-has-color", () => !_isPresetColor)
                .If($"{StylePrefix}-hidden", () => !Visible || _closed)
                .If($"{StylePrefix}-disabled", () => Disabled)
                .GetIf(() => $"{StylePrefix}-{_colorMap[_presetColor]}", () => _isPresetColor)
                .GetIf(() => $"{StylePrefix}-{_colorMap[_presetColor].Replace("-inverse", string.Empty)}", () => _isPresetColor && _colorMap[_presetColor].EndsWith("-inverse", StringComparison.Ordinal))
                .If($"{StylePrefix}-checkable", () => Checkable)
                .If($"{StylePrefix}-checkable-checked", () => Checked)
                .If($"{StylePrefix}-checkable-disabled", () => Checkable && Disabled)
                .If($"{StylePrefix}-rtl", () => RTL)
                .If($"{StylePrefix}-clickable", () => OnClick.HasDelegate)
                ;
        }

        internal string SemanticClass(string slot, string baseClass)
        {
            var customClass = GetSemanticValue(ClassNames, slot);
            return string.IsNullOrWhiteSpace(customClass) ? baseClass : $"{baseClass} {customClass}";
        }

        internal string SemanticStyle(string slot, string baseStyle = null)
        {
            var customStyle = GetSemanticValue(Styles, slot);
            if (string.IsNullOrWhiteSpace(customStyle))
            {
                return baseStyle;
            }

            return string.IsNullOrWhiteSpace(baseStyle)
                ? customStyle
                : $"{baseStyle.Trim().TrimEnd(';')}; {customStyle}";
        }

        private static string GetSemanticValue(IReadOnlyDictionary<string, string> values, string key)
        {
            return values != null && values.TryGetValue(key, out var value) ? value : null;
        }

        private string GetStyle()
        {
            var styleBuilder = new StringBuilder();
            if (!_isPresetColor && !Disabled)
            {
                styleBuilder.Append($"background-color: {(ResolvedVariant == TagVariant.Solid ? _customColor : AntDesign.Colors.ThemeColorMath.WithHslLightness(_customColor, .95))};");
                if (ResolvedVariant != TagVariant.Solid) styleBuilder.Append($"color:{_customColor};");
                if (ResolvedVariant == TagVariant.Outlined) styleBuilder.Append($"border-color:{_customColor};");
            }

            styleBuilder.AppendIfNotNullOrEmpty(Style);
            return SemanticStyle("root", styleBuilder.Length > 0 ? styleBuilder.ToString() : null);
        }

        private async Task UpdateCheckedStatus()
        {
            if (!Checkable)
            {
                return;
            }

            this.Checked = !this.Checked;
            if (this.CheckedChanged.HasDelegate)
            {
                await this.CheckedChanged.InvokeAsync(this.Checked);
            }
        }

        private async Task CloseTag(MouseEventArgs e)
        {
            if (Disabled)
            {
                return;
            }

            var closeEvent = new CloseEventArgs<MouseEventArgs>(e);

            if (OnClosing.HasDelegate)
            {
                await this.OnClosing.InvokeAsync(closeEvent);
            }

            if (closeEvent.Cancel)
            {
                return;
            }

            if (OnClose.HasDelegate)
            {
                await this.OnClose.InvokeAsync(e);
            }

            this._closed = true;
        }

        private Task CloseTagFromKeyboard(KeyboardEventArgs e)
        {
            return e.Key is "Enter" or " " ? CloseTag(new MouseEventArgs()) : Task.CompletedTask;
        }

        private async Task ClickTag(MouseEventArgs e)
        {
            if (Disabled)
            {
                return;
            }

            await this.UpdateCheckedStatus();

            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(this);
            }
        }

        private async Task HandleKeyDown(KeyboardEventArgs e)
        {
            if (OnKeyDown.HasDelegate)
            {
                await OnKeyDown.InvokeAsync(e);
            }

            if (Checkable && !Disabled && e.Key == " ")
            {
                await UpdateCheckedStatus();
            }
        }
    }
}
