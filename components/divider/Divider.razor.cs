// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        /// Type of divider - 'horizontal' | 'vertical'
        /// </summary>
        /// <default value="DirectionVHType.Horizontal" />
        [Parameter]
        public DirectionVHType Type { get; set; } = DirectionVHType.Horizontal;

        /// <summary>
        /// Content/Text orientation - 'left' | 'right' | 'center'. Ignored when not using `Text` or `ChildContent`
        /// </summary>
        /// <default value="center" />
        [Parameter]
        public string Orientation { get; set; } = "center";

        /// <summary>
        /// Whether to style the line as dashed or not.
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Dashed { get; set; } = false;

        private void SetClass()
        {
            var prefixCls = "ant-divider";
            var hashId = UseStyle(prefixCls, DividerStyle.UseComponentStyle);
            ClassMapper.Clear()
                .Add(prefixCls)
                .Add(hashId)
                .If(prefixCls, () => RTL)
                .Get(() => $"{prefixCls}-{this.Type.Name.ToLowerInvariant()}")
                .If($"{prefixCls}-with-text", () => Text != null || ChildContent != null)
                .GetIf(() => $"{prefixCls}-with-text-{this.Orientation.ToLowerInvariant()}", () => Text != null || ChildContent != null)
                .If($"{prefixCls}-plain", () => Plain && (Text != null || ChildContent != null))
                .If($"{prefixCls}-dashed", () => Dashed)
                ;
        }

        protected override void OnInitialized()
        {
            SetClass();

            base.OnInitialized();
        }
    }
}
