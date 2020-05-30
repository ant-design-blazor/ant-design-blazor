using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign
{
    public partial class Form : AntDomComponentBase
    {
        private readonly string _prefixCls = "ant-form";

        [Parameter]
        public FormLayout Layout { get; set; } = FormLayout.Horizontal;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public object Model { get; set; }

        private EditContext _editContext;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _editContext = new EditContext(Model);
            _editContext.OnFieldChanged += HandleFieldChanged;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            SetClass();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _editContext.OnFieldChanged -= HandleFieldChanged;
        }

        protected void SetClass()
        {
            this.ClassMapper.Clear()
                .Add(_prefixCls)
                .Add($"{_prefixCls}-{Layout.Name}")
               ;
        }

        private void HandleFieldChanged(object sender, FieldChangedEventArgs e)
        {
            Console.WriteLine("HandleFieldChanged");
            _editContext.Validate();
            StateHasChanged();
        }

        public void HandleValidSubmit()
        {

        }
    }
}
