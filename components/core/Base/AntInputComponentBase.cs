using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AntDesign.Forms;
using AntDesign.Internal;

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
        private IForm Form { get; set; }

        /// <summary>
        /// Gets or sets a collection of additional attributes that will be applied to the created element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

        /// <summary>
        /// Gets or sets the value of the input. This should be used with two-way binding.
        /// </summary>
        /// <example>
        /// @bind-Value="model.PropertyName"
        /// </example>
        [Parameter]
        public TValue Value { get; set; }

        /// <summary>
        /// Gets or sets a callback that updates the bound value.
        /// </summary>
        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        /// <summary>
        /// Gets or sets an expression that identifies the bound value.
        /// </summary>
        [Parameter]
        public Expression<Func<TValue>> ValueExpression { get; set; }

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
                    _ = ValueChanged.InvokeAsync(value);

                    if (_isNotifyFieldChanged)
                    {
                        EditContext.NotifyFieldChanged(FieldIdentifier);
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

                    if (_parsingValidationMessages == null)
                    {
                        _parsingValidationMessages = new ValidationMessageStore(EditContext);
                    }

                    _parsingValidationMessages.Add(FieldIdentifier, validationErrorMessage);

                    // Since we're not writing to CurrentValue, we'll need to notify about modification from here
                    EditContext.NotifyFieldChanged(FieldIdentifier);
                }

                // We can skip the validation notification if we were previously valid and still are
                if (parsingFailed || _previousParsingAttemptFailed)
                {
                    EditContext.NotifyValidationStateChanged();
                    _previousParsingAttemptFailed = parsingFailed;
                }
            }
        }

        private TValue _firstValue { get; set; }
        private bool _isNotifyFieldChanged = true;

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
            var success = BindConverter.TryConvertTo<TValue>(
               value, CultureInfo.CurrentCulture, out var parsedValue);

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
        /// Gets a string that indicates the status of the field being edited. This will include
        /// some combination of "modified", "valid", or "invalid", depending on the status of the field.
        /// </summary>
        private string FieldClass
            => EditContext.FieldCssClass(FieldIdentifier);

        /// <summary>
        /// Gets a CSS class string that combines the <c>class</c> attribute and <see cref="FieldClass"/>
        /// properties. Derived components should typically use this value for the primary HTML element's
        /// 'class' attribute.
        /// </summary>
        protected string CssClass
        {
            get
            {
                if (AdditionalAttributes != null &&
                    AdditionalAttributes.TryGetValue("class", out var @class) &&
                    !string.IsNullOrEmpty(Convert.ToString(@class)))
                {
                    return $"{@class} {FieldClass}";
                }

                return FieldClass; // Never null or empty
            }
        }

        protected override void OnInitialized()
        {
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
                    //throw new InvalidOperationException($"{GetType()} requires a cascading parameter " +
                    //    $"of type {nameof(EditContext)}. For example, you can use {GetType().FullName} inside " +
                    //    $"an {nameof(EditForm)}.");

                    return base.SetParametersAsync(ParameterView.Empty);
                }

                if (ValueExpression == null)
                {
                    //throw new InvalidOperationException($"{GetType()} requires a value for the 'ValueExpression' " +
                    //    $"parameter. Normally this is provided automatically when using 'bind-Value'.");

                    return base.SetParametersAsync(ParameterView.Empty);
                }

                EditContext = Form?.EditContext;
                FieldIdentifier = FieldIdentifier.Create(ValueExpression);
                _nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(TValue));

                EditContext.OnValidationStateChanged += _validationStateChangedHandler;
            }
            else if (Form?.EditContext != EditContext)
            {
                // Not the first run

                // We don't support changing EditContext because it's messy to be clearing up state and event
                // handlers for the previous one, and there's no strong use case. If a strong use case
                // emerges, we can consider changing this.
                throw new InvalidOperationException($"{GetType()} does not support changing the " +
                    $"{nameof(EditContext)} dynamically.");
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

            base.Dispose(disposing);
        }

        internal void ResetValue()
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
