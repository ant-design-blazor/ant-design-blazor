using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AntDesign.Forms;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign
{
    public partial class FormItem : AntDomComponentBase, IFormItem
    {
        private readonly string _prefixCls = "ant-form-item";

        [CascadingParameter(Name = "Form")]
        private IForm Form { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public ColLayoutParam LabelCol { get; set; }

        [Parameter]
        public ColLayoutParam WrapperCol { get; set; }

        private EditContext EditContext => Form?.EditContext;

        private bool _isValid = true;

        private string _labelCls = "";

        private RenderFragment<FormItem> _formValidation;

        private IControlValueAccessor _control;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Form == null)
            {
                throw new InvalidOperationException("Form is null.FormItem should be childContent of Form.");
            }

            Form.AddFormItem(this);
        }

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

        private Dictionary<string, object> GetLabelColAttributes()
        {
            ColLayoutParam labelColParameter;

            if (LabelCol != null)
            {
                labelColParameter = LabelCol;
            }
            else if (Form.LabelCol != null)
            {
                labelColParameter = Form.LabelCol;
            }
            else
            {
                labelColParameter = new ColLayoutParam();
            }

            return labelColParameter.ToAttributes();
        }

        private Dictionary<string, object> GetWrapperColAttributes()
        {
            ColLayoutParam wrapperColParameter;

            if (WrapperCol != null)
            {
                wrapperColParameter = WrapperCol;
            }
            else if (Form.WrapperCol != null)
            {
                wrapperColParameter = Form.WrapperCol;
            }
            else
            {
                wrapperColParameter = new ColLayoutParam();
            }

            return wrapperColParameter.ToAttributes();
        }

        void IFormItem.AddControl<TValue>(AntInputComponentBase<TValue> control)
        {
            this._control = control;
            _formValidation = form =>
            {
                return builder =>
                {
                    var i = 0;
                    builder.OpenComponent<FormValidationMessage<TValue>>(i++);
                    builder.AddAttribute(i++, "For", control.ValueExpression);
                    builder.AddAttribute(i++, "OnStateChange", EventCallback.Factory.Create<bool>(form, valid => form._isValid = valid));
                    builder.CloseComponent();
                };
            };

            if (control.FieldIdentifier.TryGetValidateProperty(out var propertyInfo))
            {
                var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), true);
                if (requiredAttribute.Length > 0)
                {
                    _labelCls = $"{_prefixCls}-required";
                }
            }
        }
    }
}
