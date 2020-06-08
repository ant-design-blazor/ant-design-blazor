using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
   public abstract class StatisticComponentBase<T>:AntDomComponentBase
    {
        /// <summary>
        /// 设置数值的前缀
        /// </summary>
        [Parameter] public OneOf<string, RenderFragment> Prefix { get; set; } = string.Empty;

        /// <summary>
        /// 设置数值的后缀
        /// </summary>
        [Parameter] public OneOf<string, RenderFragment> Suffix { get; set; } = string.Empty;

        /// <summary>
        /// 数值的标题
        /// </summary>
        [Parameter] public OneOf<string, RenderFragment> Title { get; set; } = string.Empty;

        /// <summary>
        /// 数值内容
        /// </summary>
        [Parameter] public T Value { get; set; }

        /// <summary>
        /// 设置数值的样式
        /// </summary>
        [Parameter] public string ValueStyle { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }
    }
}
