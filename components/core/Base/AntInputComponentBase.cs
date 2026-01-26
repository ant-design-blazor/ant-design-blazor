// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AntDesign.Core.Helpers.MemberPath;
using AntDesign.Core.Reflection;
using AntDesign.Forms;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign
{
    /// <summary>
    /// Base class for any input control that optionally supports an <see cref="EditContext"/>.
    /// reference:https://github.com/dotnet/aspnetcore/blob/master/src/Components/Web/src/Forms/InputBase.cs
    /// </summary>
    /// <typeparam name="TValue">the natural type of the input's value</typeparam>
    public abstract class AntInputComponentBase<TValue> : AntDomComponentBase, IControlValueAccessor
    {
        private readonly EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;
        private bool _previousParsingAttemptFailed;
        private ValidationMessageStore _parsingValidationMessages;
        private Type _nullableUnderlyingType;
        private PropertyReflector? _propertyReflector;
        private string? _formattedValueExpression;
        private bool _shouldGenerateFieldNames;
        private bool _hasInitializedParameters;

        private Action<object, TValue> _setValueDelegate;

        private Func<object, TValue> _getValueDelegate;

        protected string PropertyName => _propertyReflector?.PropertyName;

        internal PropertyReflector? PopertyReflector => _propertyReflector;

        internal Type ValueUnderlyingType => _nullableUnderlyingType ?? typeof(TValue);

        [CascadingParameter(Name = "FormItem")]
        protected IFormItem FormItem { get; set; }

        [CascadingParameter] private EditContext? CascadedEditContext { get; set; }

#if NET8_0_OR_GREATER

        [CascadingParameter] private HtmlFieldPrefix FieldPrefix { get; set; } = default!;
#endif

        protected IForm Form => FormItem?.Form;

        /// <summary>
        /// Validation messages for the FormItem
        /// </summary>
        public string[] ValidationMessages { get; private set; } = [];

        private FormSize _formSize;

        [CascadingParameter(Name = "FormSize")]
        public FormSize? FormSize
        {
            get => _formSize;
            set
            {
                _formSize = value.GetValueOrDefault(AntDesign.FormSize.Default);
                Size = _formSizeMap[_formSize];
            }
        }

        /// <summary>
        /// Gets or sets a collection of additional attributes that will be applied to the created element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

        private TValue _value;

        /// <summary>
        /// Gets or sets the value of the input. This should be used with two-way binding.
        /// </summary>
        /// <example>
        /// @bind-Value="model.PropertyName"
        /// </example>
        [Parameter]
        public virtual TValue Value
        {
            get { return _value; }
            set
            {
                var hasChanged = !EqualityComparer<TValue>.Default.Equals(value, _value);
                if (hasChanged)
                {
                    _value = value;
                    OnValueChange(value);
                }
            }
        }

        /// <summary>
        /// Callback that updates the bound value.
        /// </summary>
        [Parameter]
        public virtual EventCallback<TValue> ValueChanged { get; set; }

        /// <summary>
        /// An expression that identifies the bound value.
        /// </summary>
        [Parameter]
        public Expression<Func<TValue>> ValueExpression { get; set; }

        /// <summary>
        /// An expression that identifies the enumerable of bound values.
        /// </summary>
        [Parameter]
        public Expression<Func<IEnumerable<TValue>>> ValuesExpression { get; set; }

        /// <summary>
        /// The size of the input box. Note: in the context of a form,
        /// `InputSize.Large` is used. Available: `InputSize.Large` `InputSize.Default` `InputSize.Small`
        /// </summary>
        /// <default value="InputSize.Default"/>
        [Parameter]
        public InputSize Size { get; set; } = InputSize.Default;

        /// <summary>
        /// What Culture will be used when converting string to value and value to string
        /// Useful for InputNumber component.
        /// </summary>
        /// <default value="CultureInfo.CurrentCulture"/>
        [Parameter]
        public virtual CultureInfo CultureInfo { get; set; } = CultureInfo.CurrentCulture;

        /// <summary>
        /// Gets the associated <see cref="EditContext"/>.
        /// </summary>
        protected EditContext EditContext { get; set; }

        /// <summary>
        /// Gets the <see cref="FieldIdentifier"/> for the bound value.
        /// </summary>
        internal FieldIdentifier FieldIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the current value of the input.
        /// </summary>
        protected TValue CurrentValue
        {
            get => Value;
            set
            {
                var hasChanged = !EqualityComparer<TValue>.Default.Equals(value, Value);
                if (hasChanged)
                {
                    Value = value;
                    _setValueDelegate?.Invoke(Form.Model, value);
                    ValueChanged.InvokeAsync(value);

                    OnCurrentValueChange(value);

                    if (_isNotifyFieldChanged && FieldIdentifier is { Model: not null, FieldName: not null })
                    {
                        EditContext?.NotifyFieldChanged(FieldIdentifier);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the current value of the input, represented as a string.
        /// </summary>
        protected string CurrentValueAsString
        {
            get => FormatValueAsString(CurrentValue);
            set
            {
                _parsingValidationMessages?.Clear();

                bool parsingFailed;

                if (_nullableUnderlyingType != null && string.IsNullOrEmpty(value))
                {
                    // Assume if it's a nullable type, null/empty inputs should correspond to default(T)
                    // Then all subclasses get nullable support almost automatically (they just have to
                    // not reject Nullable<T> based on the type itself).
                    parsingFailed = false;
                    CurrentValue = default;
                }
                else if (TryParseValueFromString(value, out var parsedValue, out var validationErrorMessage))
                {
                    parsingFailed = false;
                    CurrentValue = parsedValue;
                }
                else
                {
                    parsingFailed = true;

                    if (EditContext != null)
                    {
                        if (_parsingValidationMessages == null)
                        {
                            _parsingValidationMessages = new ValidationMessageStore(EditContext);
                        }

                        _parsingValidationMessages.Add(FieldIdentifier, validationErrorMessage);

                        // Since we're not writing to CurrentValue, we'll need to notify about modification from here
                        EditContext.NotifyFieldChanged(FieldIdentifier);
                    }
                }

                // We can skip the validation notification if we were previously valid and still are
                if ((parsingFailed || _previousParsingAttemptFailed) && EditContext != null)
                {
                    EditContext.NotifyValidationStateChanged();
                    _previousParsingAttemptFailed = parsingFailed;
                }
            }
        }

        private TValue _firstValue;
        protected bool _isNotifyFieldChanged = true;
        private bool _isValueGuid;

        protected void ForceUpdateValueString(string value)
        {
            CurrentValueAsString = null;

            CallAfterRender(async () =>
            {
                _value = default;
                CurrentValueAsString = value;
                if (TryParseValueFromString(value, out var parsedValue, out var validationErrorMessage))
                {
                    _value = parsedValue;
                    _setValueDelegate?.Invoke(Form.Model, parsedValue);
                    await ValueChanged.InvokeAsync(parsedValue);

                    OnCurrentValueChange(parsedValue);

                    if (_isNotifyFieldChanged && FieldIdentifier is { Model: not null, FieldName: not null })
                    {
                        EditContext?.NotifyFieldChanged(FieldIdentifier);
                    }
                }
            });
            StateHasChanged();
        }

        /// <summary>
        /// Constructs an instance of <see cref="InputBase{TValue}"/>.
        /// </summary>
        protected AntInputComponentBase()
        {
            _validationStateChangedHandler = (sender, eventArgs) => InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Formats the value as a string. Derived classes can override this to determine the formating used for <see cref="CurrentValueAsString"/>.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A string representation of the value.</returns>
        protected virtual string FormatValueAsString(TValue value)
            => value?.ToString();

        /// <summary>
        /// Parses a string to create an instance of <typeparamref name="TValue"/>. Derived classes can override this to change how
        /// <see cref="CurrentValueAsString"/> interprets incoming values.
        /// </summary>
        /// <param name="value">The string value to be parsed.</param>
        /// <param name="result">An instance of <typeparamref name="TValue"/>.</param>
        /// <param name="validationErrorMessage">If the value could not be parsed, provides a validation error message.</param>
        /// <returns>True if the value could be parsed; otherwise false.</returns>
        protected virtual bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
        {
            TValue parsedValue = default;
            bool success;

            // BindConverter.TryConvertTo<Guid> doesn't work for a incomplete Guid fragment. Remove this when the BCL bug is fixed.
            if (_isValueGuid)
            {
                success = Guid.TryParse(value, out Guid parsedGuidValue);
                if (success)
                    parsedValue = THelper.ChangeType<TValue>(parsedGuidValue);
            }
            else
            {
                success = BindConverter.TryConvertTo(value, CultureInfo, out parsedValue);
            }

            if (success)
            {
                result = parsedValue;
                validationErrorMessage = null;

                return true;
            }
            else
            {
                result = default;
                validationErrorMessage = $"{FieldIdentifier.FieldName} field isn't valid.";

                return false;
            }
        }

        /// <summary>
        /// When this method is called, Value is only has been modified, but the ValueChanged is not triggered, so the outside bound Value is not changed.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnValueChange(TValue value)
        {
        }

        /// <summary>
        /// When this method is called, Value is only has been modified, but the ValueChanged is not triggered, so the outside bound Value is not changed.
        /// </summary>
        /// <param name="value"></param>
        protected virtual Task OnValueChangeAsync(TValue value)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// When this method is called, Value and CurrentValue have been modified, and the ValueChanged has been triggered, so the outside bound Value is changed.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnCurrentValueChange(TValue value)
        {
        }

        protected override void OnInitialized()
        {
            _isValueGuid = THelper.GetUnderlyingType<TValue>() == typeof(Guid);

            base.OnInitialized();

            if (Form != null && !string.IsNullOrWhiteSpace(FormItem?.Name))
            {
                var type = Form.Model.GetType();
                var dataIndex = FormItem.Name;
                if (typeof(IDictionary).IsAssignableFrom(type))
                {
                    dataIndex = $"['{dataIndex}']";
                }
                _setValueDelegate = PathHelper.SetDelegate<TValue>(dataIndex, type);
                _getValueDelegate = PathHelper.GetDelegate<TValue>(dataIndex, type);
                Value = _getValueDelegate.Invoke(Form.Model);

                if (PathHelper.GetLambda<TValue>(dataIndex, type).Body is MemberExpression lambda)
                {
                    var perpertyInfo = lambda.Member as PropertyInfo;
                    _propertyReflector = new PropertyReflector(perpertyInfo);
                }
                else
                {
                    _propertyReflector = new PropertyReflector { GetValueDelegate = (object m) => _getValueDelegate.Invoke(m), PropertyName = FormItem?.Name, DisplayName = FormItem?.Name };
                }
            }

            FormItem?.AddControl(this);
            Form?.AddControl(this);

            _firstValue = Value;
        }

        /// <summary>
        /// Gets the value to be used for the input's "name" attribute.
        /// </summary>
        protected string NameAttributeValue
        {
            get
            {
                if (AdditionalAttributes?.TryGetValue("name", out var nameAttributeValue) ?? false)
                {
                    return Convert.ToString(nameAttributeValue, CultureInfo.InvariantCulture) ?? string.Empty;
                }
#if NET8_0_OR_GREATER
                if (_shouldGenerateFieldNames)
                {
                    if (_formattedValueExpression is null)
                    {
                        if (ValueExpression is not null)
                        {
                            _formattedValueExpression = FieldPrefix != null ? FieldPrefix.GetFieldName(ValueExpression) : ExpressionFormatter.FormatLambda(ValueExpression);
                        }
                        else if (!string.IsNullOrWhiteSpace(Form?.Name) && !string.IsNullOrWhiteSpace(FormItem?.Name))
                        {
                            _formattedValueExpression = $"{Form?.Name}.{FormItem.Name}";
                        }
                    }

                    return _formattedValueExpression ?? string.Empty;
                }
#endif
                return string.Empty;
            }
        }

        private static readonly Dictionary<FormSize, InputSize> _formSizeMap = new()
        {
            [AntDesign.FormSize.Large] = InputSize.Large,
            [AntDesign.FormSize.Default] = InputSize.Default,
            [AntDesign.FormSize.Small] = InputSize.Small,
        };


        /// <inheritdoc />
        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (!_hasInitializedParameters)
            {
                // This is the first run
                // Could put this logic in OnInit, but its nice to avoid forcing people who override OnInit to call base.OnInit()

#if NET8_0_OR_GREATER
                if (CascadedEditContext != null)
                {
                    EditContext = CascadedEditContext;
                    _shouldGenerateFieldNames = EditContext.ShouldUseFieldIdentifiers;
                }
                else
                {
                    // Ideally we'd know if we were in an SSR context but we don't
                    //_shouldGenerateFieldNames = !OperatingSystem.IsBrowser();
                }
#endif

                if (EditContext == null)
                {
                    if (Form?.EditContext == null)
                    {
                        return base.SetParametersAsync(ParameterView.Empty);
                    }

                    EditContext = Form?.EditContext;
                }

                _nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(TValue));

                EditContext.OnValidationStateChanged += _validationStateChangedHandler;

                if (ValueExpression != null)
                {
                    FieldIdentifier = FieldIdentifier.Create(ValueExpression);
                    _propertyReflector = PropertyReflector.Create(ValueExpression);
                }
                else if (ValuesExpression != null)
                {
                    FieldIdentifier = FieldIdentifier.Create(ValuesExpression);
                    _propertyReflector = PropertyReflector.Create(ValuesExpression);
                }
                else if (Form?.Model != null && FormItem?.Name != null)
                {
                    FieldIdentifier = new FieldIdentifier(Form.Model, FormItem.Name);
                }
                else
                {
                    return base.SetParametersAsync(ParameterView.Empty);
                }

                _hasInitializedParameters = true;
            }
            else if (Form?.EditContext != EditContext)
            {
                // Not the first run

                //Be careful when changing this. New EditContext carried from Form should
                //already have all events transferred from original EditContext. The
                //transfer is done in Form.BuildEditContext() method. State is lost
                //though.
                EditContext = Form?.EditContext;
            }

            // For derived components, retain the usual lifecycle with OnInit/OnParametersSet/etc.
            return base.SetParametersAsync(ParameterView.Empty);
        }

        protected override void Dispose(bool disposing)
        {
            if (EditContext != null)
            {
                EditContext.OnValidationStateChanged -= _validationStateChangedHandler;
            }

            Form?.RemoveControl(this);

            base.Dispose(disposing);
        }

        internal virtual void OnValidated(string[] validationMessages)
        {
            this.ValidationMessages = validationMessages;
        }

        internal virtual void UpdateStyles()
        {
        }

        internal virtual void ResetValue()
        {
            _isNotifyFieldChanged = false;
            CurrentValue = _firstValue;
            _isNotifyFieldChanged = true;
        }

        void IControlValueAccessor.Reset()
        {
            ResetValue();
        }

        internal void OnNameChanged()
        {
            if (_getValueDelegate != null)
            {
                CurrentValue = _getValueDelegate.Invoke(Form.Model);
                InvokeAsync(StateHasChanged);
            }
        }
    }
}
