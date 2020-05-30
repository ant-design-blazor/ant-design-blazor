using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntBlazor
{
    public partial class FormItem<TValue> : AntDomComponentBase
    {
        private readonly string _prefixCls = "ant-form-item";

        [CascadingParameter(Name = "EditContext")]
        public EditContext EditContext { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public Expression<Func<TValue>> For { get; set; }

        private bool _isValid = true;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            SetClass();
        }

        protected void SetClass()
        {
            this.ClassMapper.Clear()
                .Add(_prefixCls)
                .If($"{_prefixCls}-with-help {_prefixCls}-has-error", () => _isValid == false)
               ;
        }

        public void TestSetValue(object value)
        {
            Console.WriteLine($"测试赋值：{value}");
        }
    }
}
