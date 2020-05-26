using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntBlazor
{
    public partial class Form : AntDomComponentBase
    {
        private readonly string _prefixCls = "ant-form";

        [Parameter]
        public FormLayout Layout { get; set; } = FormLayout.Horizontal;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private List<FormItem> _formItems = new List<FormItem>();
        private EditContext _editContext;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            SetClass();
        }

        protected void SetClass()
        {
            this.ClassMapper.Clear()
                .Add(_prefixCls)
                .Add($"{_prefixCls}-{Layout.Name}")
               ;
        }

        internal void AddFormItem(FormItem item)
        {
            _formItems.Add(item);
        }

        public void HandleValidSubmit()
        {

        }
    }
}
