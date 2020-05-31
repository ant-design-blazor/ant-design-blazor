using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign
{
    public partial class Form : AntDomComponentBase
    {
        private readonly string _prefixCls = "ant-form";

        [Parameter]
        public string Layout { get; set; } = FormLayout.Horizontal;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public ColLayoutParam LabelCol { get; set; }

        [Parameter]
        public ColLayoutParam WrapperCol { get; set; }

        [Parameter]
        public object Model { get; set; }

        [Parameter]
        public EventCallback<EditContext> OnFinish { get; set; }

        [Parameter]
        public EventCallback<EditContext> OnFinishFailed { get; set; }

        private EditContext _editContext;
        private List<FormItemBase> _formItems = new List<FormItemBase>();

        internal Dictionary<string, object> FieldDefaultValues { get; private set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _editContext = new EditContext(Model);
            _editContext.OnFieldChanged += HandleFieldChanged;

            FieldDefaultValues = Model.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(Model));

            FieldDefaultValues.ForEach(item => {
                Console.WriteLine($"___________FieldDefaultValues,{item.Key},{item.Value}");
            });
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
                .Add($"{_prefixCls}-{Layout.ToLower()}")
               ;
        }

        private void HandleFieldChanged(object sender, FieldChangedEventArgs e)
        {
            _editContext.Validate();
            StateHasChanged();
        }

        internal void AddFormItem(FormItemBase formItem)
        {
            _formItems.Add(formItem);
        }

        public void HandleValidSubmit()
        {
        }

        public void Reset()
        {
            _editContext.OnFieldChanged -= HandleFieldChanged;

            _formItems.ForEach(item => item.Reset());

            _editContext.OnFieldChanged += HandleFieldChanged;
        }
    }
}
