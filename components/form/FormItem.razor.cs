using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Core.Reflection;
using AntDesign.Forms;
using AntDesign.Internal;
using AntDesign.Internal.Form.Validate;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OneOf;
using static AntDesign.IconType;

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

        /// <summary>
        /// Content for the form item. Typically will contain one of the input elements
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Custom label for the item. If neither <see cref="Label"/> or <see cref="LabelTemplate"/> are provided, the DisplayName attribute value or field name will be used (in that order).
        /// </summary>
        [Parameter]
        public string Label { get; set; }

        /// <summary>
        /// Custom label content for the item. Takes priority over <see cref="Label"/>. If neither <see cref="Label"/> or <see cref="LabelTemplate"/> are provided, the DisplayName attribute value or field name will be used (in that order).
        /// </summary>
        [Parameter]
        public RenderFragment LabelTemplate { get; set; }

        /// <summary>
        /// Control the layout of the label. Commonly used to set widths for different screen sizes.
        /// </summary>
        [Parameter]
        public ColLayoutParam LabelCol { get; set; }

        /// <summary>
        /// Align the label to the left or right
        /// </summary>
        [Parameter]
        public AntLabelAlignType? LabelAlign { get; set; }

        /// <summary>
        /// Gets/sets the <c>Span</c> property on <see cref="LabelCol"/>.
        /// </summary>
        [Parameter]
        public OneOf<string, int> LabelColSpan
        {
            get { return LabelCol?.Span ?? null; }
            set
            {
                if (LabelCol == null) LabelCol = new ColLayoutParam();
                LabelCol.Span = value;
            }
        }

        /// <summary>
        /// Gets/sets the <c>Offset</c> property on <see cref="LabelCol"/>.
        /// </summary>
        [Parameter]
        public OneOf<string, int> LabelColOffset
        {
            get { return LabelCol?.Offset ?? null; }
            set
            {
                if (LabelCol == null) LabelCol = new ColLayoutParam();
                LabelCol.Offset = value;
            }
        }

        /// <summary>
        /// Control the layout of the input element's wrapper. Commonly used to set widths for different screen sizes.
        /// </summary>
        [Parameter]
        public ColLayoutParam WrapperCol { get; set; }

        /// <summary>
        /// Gets/sets the <c>Span</c> property on <see cref="WrapperCol"/>.
        /// </summary>
        [Parameter]
        public OneOf<string, int> WrapperColSpan
        {
            get { return WrapperCol?.Span ?? null; }
            set
            {
                if (WrapperCol == null) WrapperCol = new ColLayoutParam();
                WrapperCol.Span = value;
            }
        }

        /// <summary>
        /// Gets/sets the <c>Offset</c> property on <see cref="WrapperColOffset"/>.
        /// </summary>
        [Parameter]
        public OneOf<string, int> WrapperColOffset
        {
            get { return WrapperCol?.Offset ?? null; }
            set
            {
                if (WrapperCol == null) WrapperCol = new ColLayoutParam();
                WrapperCol.Offset = value;
            }
        }

        /// <summary>
        /// No style when true, it is used as a pure field control
        /// </summary>
        [Parameter]
        public bool NoStyle { get; set; } = false;

        /// <summary>
        /// Mark this item as required for validation purposes
        /// </summary>
        [Parameter]
        public bool Required { get; set; } = false;

        /// <summary>
        /// Style that will only be applied to label element.
        /// Will not be applied if LabelTemplate is set.
        /// </summary>
        [Parameter]
        public string LabelStyle { get; set; }

        /// <summary>
        /// Validation rules to apply to this item
        /// </summary>
        [Parameter]
        public FormValidationRule[] Rules { get; set; }

        /// <summary>
        /// Used in conjunction with <see cref="ValidateStatus"/> to display the verification status icon. It is recommended to use it only with the Input component
        /// </summary>
        [Parameter]
        public bool HasFeedback { get; set; }

        [Parameter]
        public bool ShowFeedbackOnError { get; set; }

        /// <summary>
        /// Validation status, if not set, it will be automatically generated according to validation rules
        /// </summary>
        [Parameter]
        public FormValidateStatus ValidateStatus { get; set; }

        /// <summary>
        /// Prompt information
        /// </summary>
        [Parameter]
        public string Help { get; set; }

        private static readonly Dictionary<FormValidateStatus, (string theme, string type)> _iconMap = new Dictionary<FormValidateStatus, (string theme, string type)>
            {
                { FormValidateStatus.Success, (IconThemeType.Fill, Outline.CheckCircle) },
                { FormValidateStatus.Warning, (IconThemeType.Fill, Outline.ExclamationCircle) },
                { FormValidateStatus.Error, (IconThemeType.Fill, Outline.CloseCircle) },
                { FormValidateStatus.Validating, (IconThemeType.Outline, Outline.Loading) }
            };

        private bool IsShowIcon => (HasFeedback && _iconMap.ContainsKey(ValidateStatus));

        private bool IsShowFeedbackOnError => (ShowFeedbackOnError && !_isValid);

        private EditContext EditContext => Form?.EditContext;

        private string[] _validationMessages = Array.Empty<string>();

        private bool _isValid = true;

        private string _labelCls = "";

        private IControlValueAccessor _control;

        private PropertyReflector _propertyReflector;

        private ClassMapper _labelClassMapper = new ClassMapper();
        private AntLabelAlignType? FormLabelAlign => LabelAlign ?? Form.LabelAlign;

        private FieldIdentifier _fieldIdentifier;
        private PropertyInfo _fieldPropertyInfo;

        private EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Form == null)
            {
                throw new InvalidOperationException("Form is null.FormItem should be childContent of Form.");
            }

            SetClass();
            SetRequiredCss();

            Form.AddFormItem(this);

            if (!string.IsNullOrWhiteSpace(Help))
            {
                _validationMessages = new[] { Help };
            }

            if (ShowFeedbackOnError && ValidateStatus == FormValidateStatus.Default)
            {
                ValidateStatus = FormValidateStatus.Error;
            }
        }

        protected void SetClass()
        {
            this.ClassMapper
                .Add(_prefixCls)
                .If($"{_prefixCls}-with-help {_prefixCls}-has-error", () => !_isValid)
                .If($"{_prefixCls}-rtl", () => RTL)
                .If($"{_prefixCls}-has-feedback", () => HasFeedback || IsShowFeedbackOnError)
                .If($"{_prefixCls}-is-validating", () => ValidateStatus == FormValidateStatus.Validating)
                .GetIf(() => $"{_prefixCls}-has-{ValidateStatus.ToString().ToLower()}", () => (HasFeedback || IsShowFeedbackOnError) && ValidateStatus.IsIn(FormValidateStatus.Success, FormValidateStatus.Error, FormValidateStatus.Warning))
                .If($"{_prefixCls}-with-help", () => !string.IsNullOrEmpty(Help))
               ;

            _labelClassMapper
                .Add($"{_prefixCls}-label")
                .If($"{_prefixCls}-label-left", () => FormLabelAlign == AntLabelAlignType.Left)
                ;
        }

        private void SetRequiredCss()
        {
            bool isRequired = false;

            if (Form.ValidateMode.IsIn(FormValidateMode.Default, FormValidateMode.Complex)
                && _propertyReflector.RequiredAttribute != null)
            {
                isRequired = true;
            }

            if (Form.ValidateMode.IsIn(FormValidateMode.Rules, FormValidateMode.Complex)
                 && Rules != null && Rules.Any(rule => rule.Required == true))
            {
                isRequired = true;
            }

            if (isRequired)
            {
                _labelCls = $"{_prefixCls}-required";
            }
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

        private string GetLabelClass()
        {
            return Required ? $"{_prefixCls}-required" : _labelCls;
        }

        protected override void Dispose(bool disposing)
        {
            if (CurrentEditContext != null && _validationStateChangedHandler != null)
            {
                CurrentEditContext.OnValidationStateChanged -= _validationStateChangedHandler;
            }

            Form?.RemoveFormItem(this);

            base.Dispose(disposing);
        }

        void IFormItem.AddControl<TValue>(AntInputComponentBase<TValue> control)
        {
            if (_control != null) return;

            if (control.FieldIdentifier.Model == null)
            {
                throw new InvalidOperationException($"Please use @bind-Value (or @bind-Values for selected components) in the control with generic type `{typeof(TValue)}`.");
            }

            _fieldIdentifier = control.FieldIdentifier;
            this._control = control;

            if (Form.ValidateMode.IsIn(FormValidateMode.Rules, FormValidateMode.Complex))
            {
                _fieldPropertyInfo = _fieldIdentifier.Model.GetType().GetProperty(_fieldIdentifier.FieldName);
            }

            _validationStateChangedHandler = (s, e) =>
            {
                _validationMessages = CurrentEditContext.GetValidationMessages(control.FieldIdentifier).Distinct().ToArray();
                this._isValid = !_validationMessages.Any();
                control.ValidationMessages = _validationMessages;

                if (!string.IsNullOrWhiteSpace(Help))
                {
                    _validationMessages = new[] { Help };
                }

                InvokeAsync(StateHasChanged);
            };

            CurrentEditContext.OnValidationStateChanged += _validationStateChangedHandler;

            if (control.ValueExpression is not null)
                _propertyReflector = PropertyReflector.Create(control.ValueExpression);
            else
                _propertyReflector = PropertyReflector.Create(control.ValuesExpression);

            if (_propertyReflector.RequiredAttribute != null)
            {
                _labelCls = $"{_prefixCls}-required";
            }
            if (_propertyReflector.DisplayName != null)
            {
                Label ??= _propertyReflector.DisplayName;
            }
        }

        ValidationResult[] IFormItem.ValidateField()
        {
            if (Rules == null)
            {
                return Array.Empty<ValidationResult>();
            }

            var results = new List<ValidationResult>();

            var displayName = string.IsNullOrEmpty(Label) ? _fieldIdentifier.FieldName : Label;

            if (_fieldPropertyInfo != null)
            {
                var propertyValue = _fieldPropertyInfo.GetValue(_fieldIdentifier.Model);

                var validateMessages = Form.ValidateMessages ?? ConfigProvider?.Form?.ValidateMessages ?? new FormValidateErrorMessages();

                foreach (var rule in Rules)
                {
                    var validationContext = new FormValidationContext()
                    {
                        Rule = rule,
                        Value = propertyValue,
                        FieldName = _fieldIdentifier.FieldName,
                        DisplayName = displayName,
                        ValidateMessages = validateMessages,
                    };

                    var result = FormValidateHelper.GetValidationResult(validationContext);

                    if (result != null)
                    {
                        results.Add(result);
                    }
                }
            }

            return results.ToArray();
        }

        FieldIdentifier IFormItem.GetFieldIdentifier() => _fieldIdentifier;
    }
}
