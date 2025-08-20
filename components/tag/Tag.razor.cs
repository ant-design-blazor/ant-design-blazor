// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
        public bool Closable { get; set; }

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
                .If($"{StylePrefix}-has-color", () => !_isPresetColor)
                .If($"{StylePrefix}-hidden", () => !Visible)
                .GetIf(() => $"{StylePrefix}-{_colorMap[_presetColor]}", () => _isPresetColor)
                .If($"{StylePrefix}-checkable", () => Checkable)
                .If($"{StylePrefix}-checkable-checked", () => Checked)
                .If($"{StylePrefix}-rtl", () => RTL)
                .If($"{StylePrefix}-clickable", () => OnClick.HasDelegate)
                ;
        }

        private string GetStyle()
        {
            var styleBuilder = new StringBuilder()
                .AppendIfNotNullOrEmpty(Style)
                .AppendIf($"background-color: {_customColor};", !_isPresetColor);

            return styleBuilder.Length > 0 ? styleBuilder.ToString() : null;
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
            var closeEvent = new CloseEventArgs<MouseEventArgs>(e);

            if (OnClosing.HasDelegate)
            {
                await this.OnClosing.InvokeAsync(closeEvent);
            }

            if (closeEvent.Cancel)
            {
                return;
            }

            this._closed = true;

            if (OnClose.HasDelegate)
            {
                await this.OnClose.InvokeAsync(e);
            }
        }

        private async Task ClickTag(MouseEventArgs e)
        {
            await this.UpdateCheckedStatus();

            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(this);
            }
        }
    }
}
