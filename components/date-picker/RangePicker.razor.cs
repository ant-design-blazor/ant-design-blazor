using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

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
                array.SetValue(changeValue, index);
                PickerValues[index] = changeValue;

                StateHasChanged();
            }

            UpdateCurrentValueAsString();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Value != null)
            {
                GetIfNotNull(Value, 0, notNullValue =>
                {
                    ChangeValue(notNullValue, 0);
                });

                GetIfNotNull(Value, 1, notNullValue =>
                {
                    ChangeValue(notNullValue, 1);
                });
            }
            else
            {
                Value = CreateInstance();
            }
        }

        /// <summary>
        /// Get value by picker index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override DateTime? GetIndexValue(int index)
        {
            if (_pickerStatus[index]._hadSelectValue)
            {
                var array = Value as Array;
                var indexValue = array.GetValue(index);

                if (indexValue == null)
                {
                    return null;
                }

                return Convert.ToDateTime(indexValue, CultureInfo);
            }
            else if (GetTypedValue(DefaultValue, index, out var defaultValue) != null)
            {
                return defaultValue;
            }
            return null;
        }

        private DateTime? GetTypedValue(TValue value, int index, out DateTime? outValue)
        {
            if (IsNullable)
            {
                outValue = (value as DateTime?[])[index];
            }
            else
            {
                outValue = (value as DateTime[])[index];
            }
            return outValue;
        }

        public override void ChangeValue(DateTime value, int index = 0)
        {
            var array = Value as Array;

            array.SetValue(value, index);

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
            array.SetValue(default, 0);
            array.SetValue(default, 1);

            _pickerStatus[0]._hadSelectValue = false;
            _pickerStatus[1]._hadSelectValue = false;

            Close();
        }

        private async Task OnInputClick(int index)
        {
            await _dropDown.Show();

            // clear status
            _pickerStatus[0]._currentShowHadSelectValue = false;
            _pickerStatus[1]._currentShowHadSelectValue = false;

            if (index == 0)
            {
                // change start picker value
                if (!_inputStart.IsOnFocused && _pickerStatus[index]._hadSelectValue)
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
                if (!_inputEnd.IsOnFocused && _pickerStatus[index]._hadSelectValue)
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

            if (IsNullable)
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
