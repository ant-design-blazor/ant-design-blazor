using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign
{
    public partial class FormItem<TValue> : FormItemBase
    {
        private readonly string _prefixCls = "ant-form-item";

        [CascadingParameter(Name = "EditContext")]
        EditContext EditContext { get; set; }

        [CascadingParameter(Name = "Form")]
        Form Form { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public string ValuePropName { get; set; }

        [Parameter]
        public ColLayoutParam LabelCol { get; set; }

        [Parameter]
        public ColLayoutParam WrapperCol { get; set; }

        [Parameter]
        public Expression<Func<TValue>> For { get; set; }

        private TValue _initValue;
        private bool _isValid = true;
        private string _labelCls = "";

        AntInputComponentBase<TValue> _inputComponent;
        private FieldIdentifier _fieldIdentifier;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Form == null)
            {
                throw new InvalidOperationException("Form is null.FormItem should be childContent of Form.");
            }

            if (For != null)
            {
                Form.AddFormItem(this);
            }
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

        internal void BindInputComponent(AntInputComponentBase<TValue> inputComponent, FieldIdentifier fieldIdentifier)
        {
            _initValue = inputComponent.Value;

            _inputComponent = inputComponent;
            _fieldIdentifier = fieldIdentifier;

            if (For != null)
            {
                if (fieldIdentifier.TryGetValidateProperty(out var propertyInfo))
                {
                    var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), true);

                    if (requiredAttribute.Length > 0)
                    {
                        _labelCls = $"{_prefixCls}-required";
                    }
                }
            }
        }

        public override void Reset()
        {
            base.Reset();

            _inputComponent.ResetValue();
        }
    }
}
