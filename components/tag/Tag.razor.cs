// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
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
        public OneOf<TagColor, string> Color {
            get
            {
                if (_isPresetColor)
                    return _presetColor;
                else
                    return _customColor;
            }
            set
            {
                if (value.IsT0)
                {
                    _isPresetColor = true;
                    _isCustomColor = false;
                    _presetColor = value.AsT0;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(value.AsT1) && _colorMap.ContainsValue(value.AsT1.ToLowerInvariant()))
                    {
                        foreach (TagColor color in Enum.GetValues(typeof(TagColor)))
                        {
                            if ((string)_colorMap[color] == value.AsT1.ToLowerInvariant())
                            {
                                _presetColor = color;
                                _isPresetColor = true;
                                _isCustomColor = false;
                                break;
                            }
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(value.AsT1))
                    {
                        _presetColor = TagColor.Default;
                        _isPresetColor = true;
                        _isCustomColor = false;
                    }
                    else
                    {
                        _isPresetColor = false;
                        _isCustomColor = true;
                        _customColor = value.AsT1;
                    }
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

        private bool _isPresetColor = true;
        private bool _isCustomColor;
        private bool _closed;
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

        private readonly Hashtable _colorMap = new Hashtable()
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

        private string _prefix = "ant-tag";

        private void UpdateClassMap()
        {
            var hashId = UseStyle(_prefix, TagStyle.UseComponentStyle);
            this.ClassMapper.Add(_prefix)
                .Add(hashId)
                .If($"{_prefix}-has-color", () => _isCustomColor)
                .If($"{_prefix}-hidden", () => Visible == false)
                .GetIf(() => $"{_prefix}-{_colorMap[_presetColor]}", () => _isPresetColor)
                .If($"{_prefix}-checkable", () => Checkable)
                .If($"{_prefix}-checkable-checked", () => Checked)
                .If($"{_prefix}-rtl", () => RTL)
                .If($"{_prefix}-clickable", () => OnClick.HasDelegate)
                ;
        }

        private string GetStyle()
        {
            var styleBuilder = new StringBuilder();

            styleBuilder.Append(Style);

            if (!string.IsNullOrEmpty(Style) && !Style.EndsWith(";"))
            {
                styleBuilder.Append(";");
            }

            if (_isCustomColor)
            {
                styleBuilder.Append($"background-color: {_customColor};");
            }

            var newStyle = styleBuilder.ToString();
            return string.IsNullOrEmpty(newStyle) ? null : newStyle;
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
