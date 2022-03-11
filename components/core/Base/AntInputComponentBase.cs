﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        [CascadingParameter(Name = "FormItem")]
        private IFormItem FormItem { get; set; }

        [CascadingParameter(Name = "Form")]
        protected IForm Form { get; set; }

        public string[] ValidationMessages { get; set; } = Array.Empty<string>();

        private string _formSize;

        [CascadingParameter(Name = "FormSize")]
        public string FormSize
        {
            get
            {
                return _formSize;
            }
            set
            {
                _formSize = value;
                Size = value;
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
                var hasChanged = !EqualityComparer<TValue>.Default.Equals(value, Value);
                if (hasChanged)
                {
                    _value = value;
                    OnValueChange(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a callback that updates the bound value.
        /// </summary>
        [Parameter]
        public virtual EventCallback<TValue> ValueChanged { get; set; }

        /// <summary>
        /// Gets or sets an expression that identifies the bound value.
        /// </summary>
        [Parameter]
        public Expression<Func<TValue>> ValueExpression { get; set; }

        [Parameter]
        public Expression<Func<IEnumerable<TValue>>> ValuesExpression { get; set; }

        /// <summary>
        /// The size of the input box. Note: in the context of a form, 
        /// the `large` size is used. Available: `large` `default` `small`
        /// </summary>
        [Parameter]
        public string Size { get; set; } = AntSizeLDSType.Default;

        /// <summary>
        /// What Culture will be used when converting string to value and value to string
        /// Useful for InputNumber component.
        /// </summary>
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

                    ValueChanged.InvokeAsync(value);

                    if (_isNotifyFieldChanged && (Form?.ValidateOnChange == true))
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


        /// <summary>
        /// Constructs an instance of <see cref="InputBase{TValue}"/>.
        /// </summary>
        protected AntInputComponentBase()
        {
            _validationStateChangedHandler = (sender, eventArgs) => StateHasChanged();
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
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                validationErrorMessage = null;
                return true;
            }

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

        protected override void OnInitialized()
        {
            _isValueGuid = THelper.GetUnderlyingType<TValue>() == typeof(Guid);
            base.OnInitialized();

            FormItem?.AddControl(this);
            Form?.AddControl(this);

            _firstValue = Value;
        }

        /// <inheritdoc />
        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (EditContext == null)
            {
                // This is the first run
                // Could put this logic in OnInit, but its nice to avoid forcing people who override OnInit to call base.OnInit()

                if (Form?.EditContext == null)
                {
                    return base.SetParametersAsync(ParameterView.Empty);
                }

                if (ValueExpression == null && ValuesExpression == null)
                {
                    return base.SetParametersAsync(ParameterView.Empty);
                }

                EditContext = Form?.EditContext;
                if (ValuesExpression == null)
                    FieldIdentifier = FieldIdentifier.Create(ValueExpression);
                else
                    FieldIdentifier = FieldIdentifier.Create(ValuesExpression);
                _nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(TValue));

                EditContext.OnValidationStateChanged += _validationStateChangedHandler;
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
    }
}
