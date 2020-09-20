using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public abstract class StatisticComponentBase<T> : AntDomComponentBase
    {
        /// <summary>
        /// 设置数值的前缀
        /// </summary>
        [Parameter] public string Prefix { get; set; }

        [Parameter] public RenderFragment PrefixTemplate { get; set; }

        /// <summary>
        /// 设置数值的后缀
        /// </summary>
        [Parameter] public string Suffix { get; set; }

        [Parameter] public RenderFragment SuffixTemplate { get; set; }

        /// <summary>
        /// 数值的标题
        /// </summary>
        [Parameter] public string Title { get; set; }

        [Parameter] public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// 数值内容
        /// </summary>
        [Parameter] public T Value { get; set; }

        /// <summary>
        /// 设置数值的样式
        /// </summary>
        [Parameter] public string ValueStyle { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        private void SetClassMap()
        {
            string prefixName = "ant-statistic";
            ClassMapper.Clear().Add(prefixName);
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetClassMap();
        }

    }
}
