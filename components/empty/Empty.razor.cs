// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
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
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/alicdn/MNbKfLBVb/Empty.svg", Columns = 1, Title = "Empty", SubTitle = "空状态")]
    public partial class Empty : AntDomComponentBase
    {
        private string _prefixCls = "ant-empty";

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
        /// Locale configuration for the empty view
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

        /// <summary>
        /// Semantic class names keyed by root, image, description, and footer.
        /// </summary>
        [Parameter]
        public IReadOnlyDictionary<string, string> ClassNames { get; set; }

        /// <summary>
        /// Semantic inline styles keyed by root, image, description, and footer.
        /// </summary>
        [Parameter]
        public IReadOnlyDictionary<string, string> Styles { get; set; }

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

        protected void SetClass()
        {
            this.ClassMapper.Clear()
                .Add(_prefixCls)
                .If($"{_prefixCls}-normal", () => Simple)
                .If($"{_prefixCls}-small", () => Small)
                .If($"{_prefixCls}-rtl", () => RTL)
                ;
        }

        protected override void OnInitialized()
        {
            this.SetClass();
        }

        protected override void OnParametersSet()
        {
            this.SetClass();
            base.OnParametersSet();
        }
    }
}
