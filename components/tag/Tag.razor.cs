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
        /// <default value="default" />
        [Parameter]
        public OneOf<TagColor, string> Color { get; set; }

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
        private TagColor _color = TagColor.Default;
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
            [TagColor.Red] = "red",
            [TagColor.Volcano] = "volcano",
            [TagColor.Orange] = "orange",
            [TagColor.Gold] = "gold",
            [TagColor.Yellow] = "yellow",
            [TagColor.Lime] = "lime",
            [TagColor.Green] = "green",
            [TagColor.Cyan] = "cyan",
            [TagColor.Blue] = "blue",
            [TagColor.GeekBlue] = "geekBlue",
            [TagColor.Purple] = "purple",
            [TagColor.Magenta] = "magenta",
            [TagColor.Pink] = "pink",
            [TagColor.Success] = "success",
            [TagColor.Processing] = "processing",
            [TagColor.Error] = "error",
            [TagColor.Warning] = "warning",
            [TagColor.Default] = "default",
        };

        private string _prefix = "ant-tag";

        private void UpdateClassMap()
        {
            this.ClassMapper.Add(_prefix)
                .If($"{_prefix}-has-color", () => Color.IsT1)
                .If($"{_prefix}-hidden", () => Visible == false)
                .GetIf(() => $"{_prefix}-{_colorMap[Color.AsT0]}", () => Color.IsT0)
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
                styleBuilder.Append($"background-color: {Color.AsT1};");
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
