using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class RangePicker<TValue> : DatePickerBase<TValue>
    {
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
                _pickerValues[index] = changeValue;

                StateHasChanged();
            }

            UpdateCurrentValueAsString();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Value == null)
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

                return Convert.ToDateTime(indexValue, this.CultureInfo);
            }
            else if (_defaultValues[index] != null)
            {
                return (DateTime)_defaultValues[index];
            }

            return null;
        }

        public override void ChangeValue(DateTime value, int index = 0)
        {
            var array = Value as Array;

            array.SetValue(value, index);

            _pickerStatus[index]._hadSelectValue = true;

            UpdateCurrentValueAsString(index);

            if (IsRange && !IsShowTime && Picker != DatePickerType.Time)
            {
                if (_pickerStatus[0]._hadSelectValue && _pickerStatus[1]._hadSelectValue)
                {
                    Close();
                }
            }
            else if (!IsShowTime && Picker != DatePickerType.Time)
            {
                Close();
            }
        }

        public override void ClearValue(int index = 0)
        {
            _isSetPicker = false;

            var array = CurrentValue as Array;
            array.SetValue(default, 0);
            array.SetValue(default, 1);

            Close();
        }

        private async Task OnInputClick(int index)
        {
            await _dropDown.Show();

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

            if (!_isNullable)
            {
                DateTime dateTime = Convert.ToDateTime(indexValue, CultureInfo);
                if (dateTime != DateTime.MinValue)
                {
                    notNullAction?.Invoke(dateTime);
                }
            }
            if (_isNullable && indexValue != null)
            {
                notNullAction?.Invoke(Convert.ToDateTime(indexValue, CultureInfo));
            }
        }

        private TValue CreateInstance()
        {
            if (_isNullable)
            {
                return (TValue)Array.CreateInstance(typeof(DateTime?), 2).Clone();
            }
            else
            {
                return (TValue)Array.CreateInstance(typeof(DateTime), 2).Clone();
            }
        }

        protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
        {
            var success = BindConverter.TryConvertTo<DateTime>(
               value, CultureInfo, out var dateTime);

            if (success)
            {
                int focusIndex = GetOnFocusPickerIndex();

                result = CreateInstance();

                var array = result as Array;
                if (focusIndex == 0)
                {
                    array.SetValue(dateTime, 0);
                    array.SetValue(GetIndexValue(1), 1);
                }
                else
                {
                    array.SetValue(GetIndexValue(0), 0);
                    array.SetValue(dateTime, 1);
                }

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
    }
}
