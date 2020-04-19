using Microsoft.AspNetCore.Components;
using OneOf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    public class AntEmptyBase : AntDomComponentBase
    {
        [Parameter] public string prefixCls { get; set; } = "ant-empty";
        /// <summary>
        /// "ltr"|"rtl"
        /// </summary>
        [Parameter] public string direction { get; set; } = "ltr";
        [Parameter] public string imageStyle { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter]
        public OneOf<string, bool, RenderFragment> description { get; set; } = "暂无数据";

        [Parameter]
        public OneOf<string, RenderFragment> image { get; set; } = AntEmpty.PRESENTED_IMAGE_DEFAULT;

        protected void SetClass()
        {
            this.ClassMapper.Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-normal", () => image.IsT1 && image.AsT1 == AntEmpty.PRESENTED_IMAGE_SIMPLE)
                .If($"{prefixCls}-{direction}", () => direction.IsIn("ltr", "rlt"))
               ;
        }

        protected override void OnInitialized()
        {
            this.SetClass();

            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            this.SetClass();

            base.OnParametersSet();
        }
    }
}
