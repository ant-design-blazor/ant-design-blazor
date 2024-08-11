// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using AntDesign.Core.Reflection;
using AntDesign.Form.Locale;
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

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public RenderFragment LabelTemplate { get; set; }

        [Parameter]
        public ColLayoutParam LabelCol { get; set; }

        [Parameter]
        public AntLabelAlignType? LabelAlign { get; set; }

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

        [Parameter]
        public ColLayoutParam WrapperCol { get; set; }

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

        [Parameter]
        public bool NoStyle { get; set; } = false;

        private bool _isRequiredByValidationRuleOrAttribute;

        private bool IsRequired => _isRequiredByValidationRuleOrAttribute || Required;

        bool IFormItem.IsRequiredByValidation => _isRequiredByValidationRuleOrAttribute;

        IForm IFormItem.Form => Form;

        [Parameter]
        public bool Required { get; set; } = false;

        /// <summary>
        /// Style that will only be applied to <label></label> element.
        /// Will not be applied if LabelTemplate is set.
        /// </summary>
        [Parameter]
        public string LabelStyle { get; set; }

        [Parameter]
        public FormValidationRule[] Rules { get; set; }

        [Parameter]
        public bool HasFeedback { get; set; }

        [Parameter]
        public bool ShowFeedbackOnError { get; set; }

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

        [Parameter]
        public string Help { get; set; }

        private static readonly Dictionary<FormValidateStatus, (string theme, string type)> _iconMap = new Dictionary<FormValidateStatus, (string theme, string type)>
        {
            { FormValidateStatus.Success, (IconThemeType.Fill, Outline.CheckCircle) },
            { FormValidateStatus.Warning, (IconThemeType.Fill, Outline.ExclamationCircle) },
            { FormValidateStatus.Error, (IconThemeType.Fill, Outline.CloseCircle) },
            { FormValidateStatus.Validating, (IconThemeType.Outline, Outline.Loading) }
        };

        private bool IsShowIcon => HasFeedback && _iconMap.ContainsKey(ValidateStatus);

        private bool IsShowFeedbackOnError => (ShowFeedbackOnError && !_isValid);

        private EditContext EditContext => Form?.EditContext;

        private string[] _validationMessages = Array.Empty<string>();

        private bool _isValid = true;

        private string _labelCls = string.Empty;

        private IControlValueAccessor _control;

        private PropertyReflector? _propertyReflector;

        private ClassMapper _labelClassMapper = new ClassMapper();

        private AntLabelAlignType? FormLabelAlign => LabelAlign ?? Form?.LabelAlign;

        private string DisplayName => Label ?? _propertyReflector?.DisplayName;

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

        private Action<string[]> _onValidated;

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

            if (Form == null)
            {
                Form = ParentFormItem?.Form;
            }

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
        }

        private void SetClass()
        {
            ClassMapper
                .Add(_prefixCls)
                .If($"{_prefixCls}-with-help {_prefixCls}-has-error", () => !_isValid)
                .If($"{_prefixCls}-rtl", () => RTL)
                .If($"{_prefixCls}-has-feedback", () => HasFeedback || IsShowFeedbackOnError)
                .If($"{_prefixCls}-is-validating", () => ValidateStatus == FormValidateStatus.Validating)
                .GetIf(() => $"{_prefixCls}-has-{ValidateStatus.ToString().ToLower()}", () => ValidateStatus.IsIn(FormValidateStatus.Success, FormValidateStatus.Error, FormValidateStatus.Warning))
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

        protected override void OnParametersSet()
        {
            if (!string.IsNullOrWhiteSpace(Help))
            {
                _validationMessages = new[] { Help };
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

        private string GetLabelClass() => IsRequired && Form.RequiredMark == FormRequiredMark.Required
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
            if (_control == null)
            {
                return;
            }
            _validationMessages = CurrentEditContext.GetValidationMessages(_fieldIdentifier).Distinct().ToArray();
            _isValid = !_validationMessages.Any();

            _validateStatus = _isValid ? _originalValidateStatus ?? FormValidateStatus.Default : FormValidateStatus.Error;

            _onValidated(_validationMessages);

            if (!string.IsNullOrWhiteSpace(Help))
            {
                _validationMessages = new[] { Help };
            }
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
            this._control = control;

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

        ValidationResult[] IFormItem.ValidateFieldWithRules()
        {
            if (_propertyReflector is null)
            {
                return [];
            }

            if (IsRequired)
            {
                _rules ??= [];
            }

            if (_rules?.Any() != true)
            {
                return [];
            }

            var results = new List<ValidationResult>();

            if (_fieldValueGetter != null)
            {
                var propertyValue = _fieldValueGetter.Invoke(_fieldIdentifier.Model);

                var validateMessages = Form?.Locale.DefaultValidateMessages ?? ConfigProvider?.Form?.ValidateMessages ?? new FormValidateErrorMessages();

                foreach (var rule in _rules)
                {
                    var validationContext = new FormValidationContext()
                    {
                        Rule = rule,
                        Value = propertyValue,
                        FieldName = _fieldIdentifier.FieldName,
                        DisplayName = DisplayName,
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

        void IFormItem.SetValidationMessage(string[] errorMessages)
        {
            _validationMessages = errorMessages;
            _isValid = !errorMessages.Any();
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
            if (_propertyReflector is null)
            {
                yield break;
            }

            var attributes = _propertyReflector?.ValidationAttributes;

            foreach (var attribute in attributes)
            {
                FormFieldType? type = _valueUnderlyingType switch
                {
                    { IsGenericType: true } => FormFieldType.Number,
                    { IsEnum: true } => FormFieldType.Enum,
                    { IsArray: true } => FormFieldType.Array,
                    _ when _valueUnderlyingType == typeof(string) => FormFieldType.String,
                    _ => null
                };
                yield return new FormValidationRule { ValidationAttribute = attribute, Type = type };
            }
        }
    }
}
