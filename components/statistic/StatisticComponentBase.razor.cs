// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public abstract class StatisticComponentBase<T> : AntDomComponentBase
    {
        /// <summary>
        /// Prefix text for before the displayed value
        /// </summary>
        [Parameter]
        public string Prefix { get; set; }

        /// <summary>
        /// Prefix content for before the displayed value
        /// </summary>
        [Parameter]
        public RenderFragment PrefixTemplate { get; set; }

        /// <summary>
        /// Suffix string for after the displayed value
        /// </summary>
        [Parameter]
        public string Suffix { get; set; }

        /// <summary>
        /// Suffix content for after the displayed value
        /// </summary>
        [Parameter]
        public RenderFragment SuffixTemplate { get; set; }

        /// <summary>
        /// Title string for the value
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// Title content for the value
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// Value for displaying
        /// </summary>
        [Parameter]
        public virtual T Value { get; set; }

        /// <summary>
        /// Style for the value display
        /// </summary>
        [Parameter]
        public string ValueStyle { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private void SetClassMap()
        {
            string prefixName = "ant-statistic";
            var hashId = UseStyle(prefixName, StatisticStyle.UseComponentStyle);
            ClassMapper
                .Add(prefixName)
                .Add(hashId)
                .If($"{prefixName}-rtl", () => RTL);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetClassMap();
        }
    }
}
