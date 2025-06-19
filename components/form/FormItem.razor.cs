// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AntDesign.Core.Helpers.MemberPath;
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
        private static readonly Dictionary<string, object> _noneColAttributes = [];

        private readonly string _prefixCls = "ant-form-item";

        [CascadingParameter(Name = "Form")]
        private IForm Form { get; set; }

        [CascadingParameter(Name = "FormItem")]
        private IFormItem ParentFormItem { get; set; }

        /// <summary>
        /// Specific the name of the form item. It also can used as the Member Path for binding the property of the Model.
        /// </summary>
        [Parameter]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                _nameChanged?.Invoke();
            }
        }

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

        private bool _isRequiredByValidationRuleOrAttribute;

        private bool IsRequired => _isRequiredByValidationRuleOrAttribute || Required || ParentFormItem?.IsRequired == true;

        bool IFormItem.IsRequired => IsRequired;

        IForm IFormItem.Form => Form;

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

        /// <summary>
        /// Whether to show feedback icon on error. If set to false, it will not show the icon even if the field is in error state.
        /// </summary>
        [Parameter]
        public bool ShowFeedbackOnError { get; set; }

        /// <summary>
        /// Validation status, if not set, it will be automatically generated according to validation rules
        /// </summary>
        [Parameter]
        public FormValidateStatus ValidateStatus
        {
            get => _validateStatus;
            set
            {
                _originalValidateStatus ??= value;

                if (_validateStatus != value)
                {
                    _validateStatus = value;
                    _vaildateStatusChanged?.Invoke();
                }
            }
        }

        /// <summary>
        /// Prompt information
        /// </summary>
        [Parameter]
        public string Help { get; set; }

        /// <summary>
        /// FormItem Help Tooltip information
        /// </summary>
        [Parameter]
        public string ToolTip { get; set; }

        private static readonly Dictionary<FormValidateStatus, (IconThemeType theme, string type)> _iconMap = new()
    {
        { FormValidateStatus.Success, (IconThemeType.Fill, Outline.CheckCircle) },
        { FormValidateStatus.Warning, (IconThemeType.Fill, Outline.ExclamationCircle) },
        { FormValidateStatus.Error, (IconThemeType.Fill, Outline.CloseCircle) },
        { FormValidateStatus.Validating, (IconThemeType.Outline, Outline.Loading) }
    };

        private bool IsShowIcon => HasFeedback && _iconMap.ContainsKey(ValidateStatus);

        private bool IsShowFeedbackOnError => ShowFeedbackOnError && !_isValid;

        private EditContext EditContext => Form?.EditContext;

        private string[] _validationMessages = [];

        private bool _isValid = true;

        private string _labelCls = string.Empty;

        private IControlValueAccessor _control;

        private PropertyReflector? _propertyReflector;

        private readonly ClassMapper _labelClassMapper = new();

        private AntLabelAlignType? FormLabelAlign => LabelAlign ?? Form?.LabelAlign;

        private string DisplayName => Label ?? _propertyReflector?.DisplayName;

        private string[] ValidationMessages => _validationMessages.Length > 0 ? _validationMessages : [Help];

        private string _name;
        private Action _nameChanged;

        private Type _valueUnderlyingType;

        private FieldIdentifier _fieldIdentifier;
        private Func<object, object> _fieldValueGetter;

        private EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;
        private EventHandler<ValidationRequestedEventArgs> _validationRequestedHandler;
        private FormValidateStatus _validateStatus;
        private FormValidateStatus? _originalValidateStatus;
        private Action _vaildateStatusChanged;

        private Action<string[]> _onValidated = _ => { };

        private IEnumerable<FormValidationRule> _rules;

        RenderFragment IFormItem.FeedbackIcon => IsShowIcon ? builder =>
        {
            builder.OpenElement(1, "span");
            builder.AddAttribute(2, "class", $"ant-form-item-feedback-icon ant-form-item-feedback-icon-{_validateStatus.ToString().ToLowerInvariant()}");

            builder.OpenComponent<Icon>(11);
            builder.AddAttribute(12, nameof(Icon.Type), _iconMap[_validateStatus].type);
            builder.AddAttribute(13, nameof(Icon.Theme), _iconMap[_validateStatus].theme);
            builder.CloseComponent();
            builder.CloseElement();
        }
        : null;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Form ??= ParentFormItem?.Form;

            if (Form == null)
            {
                throw new InvalidOperationException("Form is null. FormItem should be childContent of Form.");
            }

            SetClass();

            Form.AddFormItem(this);

            if (ShowFeedbackOnError && ValidateStatus == FormValidateStatus.Default)
            {
                ValidateStatus = FormValidateStatus.Error;
            }

            SetInternalIsRequired();

            SetEventHandlers();
        }

        private void SetClass()
        {
            ClassMapper
                .Add(_prefixCls)
                .If($"{_prefixCls}-with-help {_prefixCls}-has-error", () => !_isValid)
                .If($"{_prefixCls}-rtl", () => RTL)
                .If($"{_prefixCls}-has-feedback", () => HasFeedback || IsShowFeedbackOnError)
                .If($"{_prefixCls}-is-validating", () => ValidateStatus == FormValidateStatus.Validating)
                .GetIf(() => $"{_prefixCls}-has-{ValidateStatus.ToString().ToLowerInvariant()}", () => ValidateStatus.IsIn(FormValidateStatus.Success, FormValidateStatus.Error, FormValidateStatus.Warning))
                .If($"{_prefixCls}-with-help", () => !string.IsNullOrEmpty(Help))
               ;

            _labelClassMapper
                .Add($"{_prefixCls}-label")
                .If($"{_prefixCls}-label-left", () => FormLabelAlign == AntLabelAlignType.Left)
                ;
        }

        private void SetRules()
        {
            if (Form == null)
            {
                return;
            }
            _rules = Form.ValidateMode switch
            {
                FormValidateMode.Default => GetRulesFromAttributes(),
                FormValidateMode.Rules => Rules ?? [],
                _ => [.. GetRulesFromAttributes(), .. Rules ?? []]
            };

            if (Required && !_rules.Any(rule => rule.Required == true || rule.ValidationAttribute is RequiredAttribute))
            {
                _rules = [.. _rules, new FormValidationRule { Required = true }];
            }
        }

        private void SetEventHandlers()
        {
            if (Form?.ValidateOnChange == true)
            {
                _validationStateChangedHandler = (s, e) =>
                {
                    UpdateValidateMessage();
                };
                CurrentEditContext.OnValidationStateChanged += _validationStateChangedHandler;
            }
            else
            {
                _validationRequestedHandler = (s, e) =>
                {
                    UpdateValidateMessage();
                };
                CurrentEditContext.OnValidationRequested += _validationRequestedHandler;
            }
        }

        private void SetInternalIsRequired()
        {
            if (Form is null)
            {
                return;
            }
            var isRequired = false;

            if (Form.ValidateMode.IsIn(FormValidateMode.Default, FormValidateMode.Complex)
                && _propertyReflector?.RequiredAttribute != null)
            {
                isRequired = true;
            }

            if (Form.ValidateMode.IsIn(FormValidateMode.Rules, FormValidateMode.Complex)
                 && Rules != null && Rules.Any(rule => rule.Required == true))
            {
                isRequired = true;
            }

            _isRequiredByValidationRuleOrAttribute = isRequired;
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

        private string GetLabelClass() => IsRequired && Form?.RequiredMark == FormRequiredMark.Required
            ? $"{_prefixCls}-required"
            : _labelCls;

        protected override void Dispose(bool disposing)
        {
            if (CurrentEditContext != null)
            {
                if (_validationStateChangedHandler != null)
                {
                    CurrentEditContext.OnValidationStateChanged -= _validationStateChangedHandler;
                }
                if (_validationRequestedHandler != null)
                {
                    CurrentEditContext.OnValidationRequested -= _validationRequestedHandler;
                }
            }

            Form?.RemoveFormItem(this);

            base.Dispose(disposing);
        }

        private void UpdateValidateMessage()
        {
            if (_fieldIdentifier.Model == null)
            {
                return;
            }
            _validationMessages = [.. CurrentEditContext.GetValidationMessages(_fieldIdentifier).Distinct()];
            _isValid = _validationMessages.Length == 0;

            _validateStatus = _isValid ? _originalValidateStatus ?? FormValidateStatus.Default : FormValidateStatus.Error;

            _onValidated(_validationMessages);

            _vaildateStatusChanged?.Invoke();
            InvokeAsync(StateHasChanged);
        }

        void IFormItem.AddControl<TValue>(AntInputComponentBase<TValue> control)
        {
            if (_control != null) return;

            _vaildateStatusChanged = control.UpdateStyles;
            _nameChanged = control.OnNameChanged;
            _onValidated = control.OnValidated;
            _valueUnderlyingType = control.ValueUnderlyingType;

            if (control.FieldIdentifier.Model == null)
            {
                return;
                //throw new InvalidOperationException($"Please use @bind-Value (or @bind-Values for selected components) in the control with generic type `{typeof(TValue)}`.");
            }

            _fieldIdentifier = control.FieldIdentifier;
            _control = control;

            if (control.PopertyReflector is not null)
            {
                _propertyReflector = control.PopertyReflector;
            }
            else if (control.ValueExpression is not null)
            {
                _propertyReflector = PropertyReflector.Create(control.ValueExpression);
            }
            else if (control.ValuesExpression is not null)
            {
                _propertyReflector = PropertyReflector.Create(control.ValuesExpression);
            }

            _fieldValueGetter = _propertyReflector?.GetValueDelegate;

            SetInternalIsRequired();
            SetRules();

            StateHasChanged();
        }

        private void BuildPropertyWithName()
        {
            var type = Form.Model.GetType();
            var dataIndex = Name;
            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                dataIndex = $"['{dataIndex}']";
            }

            LambdaExpression exp = PathHelper.GetLambda<object>(dataIndex, type);

            if (exp.Body is UnaryExpression unary)
            {
                if (unary.Operand is MemberExpression member)
                {
                    var perpertyInfo = member.Member as PropertyInfo;
                    _propertyReflector = new PropertyReflector(perpertyInfo);
                }
            }
            else if (exp.Body is MemberExpression member)
            {
                var perpertyInfo = member.Member as PropertyInfo;
                _propertyReflector = new PropertyReflector(perpertyInfo);
            }
            else
            {
                var getValueDelegate = PathHelper.GetDelegate<object>(dataIndex, type);
                _propertyReflector = new PropertyReflector
                {
                    GetValueDelegate = getValueDelegate.Invoke,
                    PropertyName = Name,
                    DisplayName = Name,
                    ValidationAttributes = []
                };
            }
            _fieldValueGetter = _propertyReflector?.GetValueDelegate;
            _valueUnderlyingType = THelper.GetUnderlyingType(_propertyReflector.PropertyInfo.PropertyType);
            _fieldIdentifier = new FieldIdentifier(Form.Model, Name);
            SetRules();
        }

        IEnumerable<ValidationResult> IFormItem.ValidateFieldWithRules()
        {
            if (_propertyReflector is null)
            {
                if (!string.IsNullOrWhiteSpace(Name))
                {
                    BuildPropertyWithName();
                }
            }

            if (_propertyReflector is null)
            {
                return [];
            }

            if (_rules?.Any() != true)
            {
                return [];
            }

            if (_fieldValueGetter == null)
            {
                return [];
            }

            List<ValidationResult> results = [];

            var propertyValue = _fieldValueGetter.Invoke(_fieldIdentifier.Model);

            var validateMessages = Form?.Locale.DefaultValidateMessages ?? ConfigProvider?.Form?.ValidateMessages ?? new FormValidateErrorMessages();

            foreach (var rule in _rules)
            {
                if (rule.Required == true && !IsRequired)
                {
                    continue;
                }

                var validationContext = new FormValidationContext()
                {
                    Rule = rule,
                    Value = propertyValue,
                    FieldName = _fieldIdentifier.FieldName,
                    DisplayName = DisplayName ?? _propertyReflector.PropertyName,
                    FieldType = _valueUnderlyingType,
                    ValidateMessages = validateMessages,
                    Model = Form.Model
                };

                var result = FormValidateHelper.GetValidationResult(validationContext);

                if (result != null)
                {
                    results.Add(result);
                }
            }

            return results;
        }

        FieldIdentifier IFormItem.GetFieldIdentifier() => _fieldIdentifier;

        void IFormItem.SetValidationMessage(string[] errorMessages)
        {
            _validationMessages = errorMessages;
            _isValid = errorMessages.Length == 0;
            _validateStatus = _isValid ? FormValidateStatus.Default : FormValidateStatus.Error;

            _onValidated(_validationMessages);

            _vaildateStatusChanged?.Invoke();
            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// tansform attributes to FormValidationRule for using locale validation message templates
        /// </summary>
        /// <returns></returns>
        private IEnumerable<FormValidationRule> GetRulesFromAttributes()
        {
            if (_propertyReflector?.ValidationAttributes is null)
            {
                yield break;
            }

            foreach (var attribute in _propertyReflector.ValidationAttributes)
            {
                yield return new FormValidationRule { ValidationAttribute = attribute, Enum = _valueUnderlyingType.IsEnum ? _valueUnderlyingType : null };
            }
        }
    }
}
