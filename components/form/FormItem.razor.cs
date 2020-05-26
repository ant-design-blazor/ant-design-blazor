using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntBlazor
{
    public partial class FormItem : AntDomComponentBase
    {
        private readonly string _prefixCls = "ant-form-item";

        [CascadingParameter(Name = "EditContext")]
        public EditContext EditContext { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public FormRule[] Rules { get; set; }

        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public string Name { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            SetClass();
        }

        protected void SetClass()
        {
            this.ClassMapper.Clear()
                .Add(_prefixCls)
               ;
        }

        public void TestSetValue(object value)
        {
            Console.WriteLine($"测试赋值：{value}");
        }
    }
}
