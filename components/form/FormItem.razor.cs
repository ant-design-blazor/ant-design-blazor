using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AntDesign.Forms;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq;

namespace AntDesign
{
    public partial class FormItem : AntDomComponentBase, IFormItem
    {
        private static readonly Dictionary<string, object> _noneColAttributes = new Dictionary<string, object>();

        private readonly string _prefixCls = "ant-form-item";

        [CascadingParameter(Name = "Form")]
        private IForm Form { get; set; }

        [CascadingParameter(Name = "FormItem")]
        private IFormItem ParentFormItem { get; set; }

        [CascadingParameter]
        private EditContext CurrentEditContext { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public ColLayoutParam LabelCol { get; set; }

        [Parameter]
        public ColLayoutParam WrapperCol { get; set; }

        [Parameter]
        public bool NoStyle { get; set; } = false;

        [Parameter]
        public bool Required { get; set; } = false;

        private EditContext EditContext => Form?.EditContext;

        private bool _isValid = true;

        private string _labelCls = "";

        private IControlValueAccessor _control;

        private RenderFragment _formValidationMessages;

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
            if (NoStyle || ParentFormItem != null)
            {
                return _noneColAttributes;
            }

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
            if (NoStyle || ParentFormItem != null)
            {
                return _noneColAttributes;
            }

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
            if (control.FieldIdentifier.Model == null)
            {
                throw new InvalidOperationException($"Please use @bind-Value in the control with generic type `{typeof(TValue)}`.");
            }

            this._control = control;

            CurrentEditContext.OnValidationStateChanged += (s, e) =>
            {
                control.ValidationMessages = CurrentEditContext.GetValidationMessages(control.FieldIdentifier).ToArray();
                this._isValid = !control.ValidationMessages.Any();

                StateHasChanged();
            };

            _formValidationMessages = builder =>
            {
                var i = 0;
                builder.OpenComponent<FormValidationMessage<TValue>>(i++);
                builder.AddAttribute(i++, "Control", control);
                builder.CloseComponent();
            };

            if (control.FieldIdentifier.TryGetValidateProperty(out var propertyInfo))
            {
                var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), true);
                if (requiredAttribute.Length > 0)
                {
                    _labelCls = $"{_prefixCls}-required";
                }
            }
            else
            {
                throw new InvalidOperationException($"Unable to match property `{control.FieldIdentifier.FieldName}` in Model `{control.FieldIdentifier.Model.GetType()}`, please confirm that the Model contains this property.");
            }
        }
    }
}
