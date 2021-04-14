using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class RangePicker<TValue> : DatePickerBase<TValue>
    {
        private TValue _value;

        /// <summary>
        /// Gets or sets the value of the input. This should be used with two-way binding.
        /// </summary>
        /// <example>
        /// @bind-Value="model.PropertyName"
        /// </example>
        [Parameter]
        public sealed override TValue Value
        {
            get { return _value; }
            set
            {
                TValue orderedValue = SortValue(value);
                var hasChanged = !EqualityComparer<TValue>.Default.Equals(orderedValue, Value);
                if (hasChanged)
                {
                    _value = orderedValue;
                    OnValueChange(orderedValue);
                }
            }
        }

        private DateTime[] _pickerValuesAfterInit = new DateTime[2];

        [Parameter]
        public EventCallback<DateRangeChangedEventArgs> OnChange { get; set; }

        public RangePicker()
        {
            IsRange = true;

            DisabledDate = (date) =>
            {
                var array = Value as Array;

                if (_pickerStatus[0]._hadSelectValue && _inputEnd.IsOnFocused)
                {
                    DateTime? value = null;
                    GetIfNotNull(Value, 0, notNullValue =>
                    {
                        value = notNullValue;
                    });

                    if (value != null)
                    {
                        return DateHelper.FormatDateByPicker(date.Date, Picker) < DateHelper.FormatDateByPicker(((DateTime)value).Date, Picker);
                    }
                }
                if (_pickerStatus[1]._hadSelectValue && _inputStart.IsOnFocused)
                {
                    DateTime? value = null;
                    GetIfNotNull(Value, 1, notNullValue =>
                    {
                        value = notNullValue;
                    });

                    if (value != null)
                    {
                        return DateHelper.FormatDateByPicker(date.Date, Picker) > DateHelper.FormatDateByPicker(((DateTime)value).Date, Picker);
                    }
                }

                return false;
            };
        }

        protected void OnInput(ChangeEventArgs args, int index = 0)
        {
            if (args == null)
            {
                return;
            }

            var array = Value as Array;

            if (BindConverter.TryConvertTo(args.Value.ToString(), CultureInfo, out DateTime changeValue))
            {
                if (Picker == DatePickerType.Date)
                {
                    if (IsDateStringFullDate(args.Value.ToString()))
                        array.SetValue(changeValue, index);
                }
                else
                    array.SetValue(changeValue, index);
                ChangePickerValue(changeValue, index);

                if (OnChange.HasDelegate)
                {
                    OnChange.InvokeAsync(new DateRangeChangedEventArgs
                    {
                        Dates = new DateTime?[] { array.GetValue(0) as DateTime?, array.GetValue(1) as DateTime? },
                        DateStrings = new string[] { GetInputValue(0), GetInputValue(1) }
                    });
                }

                StateHasChanged();
            }

            UpdateCurrentValueAsString();
        }

        /// <summary>
        /// Method is called via EventCallBack if the keyboard key is no longer pressed inside the Input element.
        /// </summary>
        /// <param name="e">Contains the key (combination) which was pressed inside the Input element</param>
        /// <param name="index">Refers to picker index - 0 for starting date, 1 for ending date</param>
        protected async Task OnKeyUp(KeyboardEventArgs e, int index)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var key = e.Key.ToUpperInvariant();
            if (key == "ENTER")
            {
                var input = (index == 0 ? _inputStart : _inputEnd);
                if (string.IsNullOrWhiteSpace(input.Value))
                    ClearValue(index);
                else
                {
                    if (BindConverter.TryConvertTo(input.Value, CultureInfo, out DateTime changeValue))
                    {
                        var array = Value as Array;
                        array.SetValue(changeValue, index);
                    }
                    if (index == 0)
                    {
                        await Blur(0);
                        await Focus(1);
                    }
                    else
                        Close();
                }
            }
            if (key == "ARROWDOWN" && !_dropDown.IsOverlayShow())
            {
                await _dropDown.Show();
            }
            if (key == "ARROWUP" && _dropDown.IsOverlayShow())
            {
                Close();
            }
        }

        private async Task OnFocus(int index)
        {
            if (index == 0)
            {
                if (!_inputStart.IsOnFocused)
                {
                    await Blur(1);
                    await Focus(0);
                }
            }
            else
            {
                if (!_inputEnd.IsOnFocused)
                {
                    await Blur(0);
                    await Focus(1);
                }
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            RangePickerDefaults.ProcessDefaults(Value, DefaultValue, DefaultPickerValue, PickerValues, UseDefaultPickerValue);
            _pickerValuesAfterInit[0] = PickerValues[0];
            _pickerValuesAfterInit[1] = PickerValues[1];
            if (_value == null)
            {
                _value = CreateInstance();
                ValueChanged.InvokeAsync(_value);
            }
        }

        /// <summary>
        /// Handle change of values.
        /// When values are changed, PickerValues should point to those new values
        /// or current date if no values were passed.
        /// </summary>
        /// <param name="value"></param>
        protected override void OnValueChange(TValue value)
        {
            base.OnValueChange(value);
            //reset all only if not changed using picker
            if (_inputStart?.IsOnFocused != true && _inputEnd?.IsOnFocused != true) // is null or false
            {
                UseDefaultPickerValue[0] = false;
                UseDefaultPickerValue[1] = false;
                _pickerStatus[0]._hadSelectValue = true;
                _pickerStatus[1]._hadSelectValue = true;
            }
        }

        /// <summary>
        /// Get value by picker index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override DateTime? GetIndexValue(int index)
        {
            if (Value != null)
            {
                var array = Value as Array;
                var indexValue = array.GetValue(index);

                if (indexValue == null)
                {
                    return null;
                }

                return Convert.ToDateTime(indexValue, CultureInfo);
            }
            else if (!IsTypedValueNull(DefaultValue, index, out var defaultValue))
            {
                return defaultValue;
            }
            return null;
        }

        private static bool IsTypedValueNull(TValue value, int index, out DateTime? outValue)
        {
            outValue = (DateTime?)(value as Array)?.GetValue(index);
            return outValue == null;
        }

        public override void ChangeValue(DateTime value, int index = 0)
        {
            bool isValueInstantiated = Value == null;
            if (isValueInstantiated)
            {
                Value = CreateInstance();
            }
            UseDefaultPickerValue[index] = false;
            var array = Value as Array;

            array.SetValue(value, index);

            //if Value was just now instantiated then set the other index to existing DefaultValue
            if (isValueInstantiated && IsRange && DefaultValue != null)
            {
                var arrayDefault = DefaultValue as Array;
                int oppositeIndex = index == 1 ? 0 : 1;
                array.SetValue(arrayDefault.GetValue(oppositeIndex), oppositeIndex);
            }

            _pickerStatus[index]._hadSelectValue = true;
            _pickerStatus[index]._currentShowHadSelectValue = true;

            UpdateCurrentValueAsString(index);

            if (!IsShowTime && Picker != DatePickerType.Time)
            {
                if (_pickerStatus[0]._currentShowHadSelectValue && _pickerStatus[1]._currentShowHadSelectValue)
                {
                    Close();
                }
            }

            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(new DateRangeChangedEventArgs
                {
                    Dates = new DateTime?[] { array.GetValue(0) as DateTime?, array.GetValue(1) as DateTime? },
                    DateStrings = new string[] { GetInputValue(0), GetInputValue(1) }
                });
            }
        }

        public override void ClearValue(int index = 0)
        {
            _isSetPicker = false;

            var array = CurrentValue as Array;
            if (!IsNullable && DefaultValue != null)
            {
                var defaults = DefaultValue as Array;
                array.SetValue(defaults.GetValue(0), 0);
                array.SetValue(defaults.GetValue(1), 1);
            }
            else
            {
                array.SetValue(default, 0);
                array.SetValue(default, 1);
            }

            (string first, string second) = DatePickerPlaceholder.GetRangePlaceHolderByType(_pickerStatus[0]._initPicker, Locale);
            _placeholders[0] = first;
            _placeholders[1] = second;

            _pickerStatus[0]._hadSelectValue = false;
            _pickerStatus[1]._hadSelectValue = false;

            Close();
        }

        private async Task OnInputClick(int index)
        {
            //Reset Picker to default in case the picker value was changed
            //but no value was selected (for example when a user clicks next
            //month but does not select any value)
            if (UseDefaultPickerValue[index] && DefaultPickerValue != null)
            {
                PickerValues[index] = _pickerValuesAfterInit[index];
            }
            await _dropDown.Show();

            // clear status
            _pickerStatus[0]._currentShowHadSelectValue = false;
            _pickerStatus[1]._currentShowHadSelectValue = false;

            if (index == 0)
            {
                // change start picker value
                if (!_inputStart.IsOnFocused && _pickerStatus[index]._hadSelectValue && !UseDefaultPickerValue[index])
                {
                    GetIfNotNull(Value, index, notNullValue =>
                    {
                        ChangePickerValue(notNullValue, index);
                    });
                }

                ChangeFocusTarget(true, false);
            }
            else
            {
                // change end picker value
                if (!_inputEnd.IsOnFocused && _pickerStatus[index]._hadSelectValue && !UseDefaultPickerValue[index])
                {
                    GetIfNotNull(Value, index, notNullValue =>
                    {
                        ChangePickerValue(notNullValue, index);
                    });
                }

                ChangeFocusTarget(false, true);
            }
        }

        private void GetIfNotNull(TValue value, int index, Action<DateTime> notNullAction)
        {
            var array = value as Array;
            var indexValue = array.GetValue(index);

            if (!IsNullable)
            {
                DateTime dateTime = Convert.ToDateTime(indexValue, CultureInfo);
                if (dateTime != DateTime.MinValue)
                {
                    notNullAction?.Invoke(dateTime);
                }
            }
            if (IsNullable && indexValue != null)
            {
                notNullAction?.Invoke(Convert.ToDateTime(indexValue, CultureInfo));
            }
        }

        private TValue CreateInstance()
        {
            if (DefaultValue is not null)
                return (TValue)(DefaultValue as Array).Clone();

            if (IsNullable)
            {
                return (TValue)Array.CreateInstance(typeof(DateTime?), 2).Clone();
            }
            else
            {
                return (TValue)Array.CreateInstance(typeof(DateTime), 2).Clone();
            }
        }

        protected override void UpdateCurrentValueAsString(int index = 0)
        {
            if (EditContext != null)
            {
                CurrentValueAsString = $"{GetInputValue(0)},{GetInputValue(1)}";
            }
        }

        protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
        {
            result = default;
            validationErrorMessage = $"{FieldIdentifier.FieldName} field isn't valid.";

            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            string[] values = value.Split(",");

            if (values.Length != 2)
            {
                return false;
            }

            var success0 = BindConverter.TryConvertTo<DateTime>(values[0], CultureInfo, out var dateTime0);
            var success1 = BindConverter.TryConvertTo<DateTime>(values[1], CultureInfo, out var dateTime1);

            if (success0 && success1)
            {
                result = CreateInstance();

                var array = result as Array;

                array.SetValue(dateTime0, 0);
                array.SetValue(dateTime1, 1);

                validationErrorMessage = null;

                return true;
            }

            return false;
        }
    }
}
