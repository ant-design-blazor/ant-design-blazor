// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
        <para>Empty state placeholder.</para>

        <h2>When To Use</h2>

        <list type="bullet">
            <item>When there is no data provided, display for friendly tips.</item>
            <item>User tutorial to create something in fresh new situation.</item>
        </list>
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/alicdn/MNbKfLBVb/Empty.svg", Columns = 1)]
    public partial class Empty : AntDomComponentBase
    {
        [Parameter]
        public string PrefixCls { get; set; } = "ant-empty";

        /// <summary>
        /// Style for the wrapper of the image. Always used regardless of image type.
        /// </summary>
        [Parameter]
        public string ImageStyle { get; set; }

        /// <summary>
        /// Use small variant of Empty
        /// </summary>
        [Parameter]
        public bool Small { get; set; }

        /// <summary>
        /// Use simple variant of Empty. Changes image as well.
        /// </summary>
        [Parameter]
        public bool Simple { get; set; }

        /// <summary>
        /// Content displayed after the empty view
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Description for the view
        /// </summary>
        /// <default value="No Data (in current locale)" />
        [Parameter]
        public OneOf<string, bool?> Description { get; set; } = LocaleProvider.CurrentLocale.Empty.Description;

        /// <summary>
        /// Description content for the view. Takes priority over <see cref="Description"/>
        /// </summary>
        [Parameter]
        public RenderFragment DescriptionTemplate { get; set; }

        /// <summary>
        /// Image URL for view. Takes priority over <see cref="Simple"/>
        /// </summary>
        [Parameter]
        public string Image { get; set; }

        /// <summary>
        /// Image content for empty view. Takes priority over <see cref="Image"/> and <see cref="Simple"/>
        /// </summary>
        [Parameter]
        public RenderFragment ImageTemplate { get; set; }

        protected void SetClass()
        {
            this.ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-normal", () => Simple)
                .If($"{PrefixCls}-small", () => Small)
                .If($"{PrefixCls}-rtl", () => RTL)
                ;
        }

        protected override void OnInitialized()
        {
            this.SetClass();
        }
    }
}
