using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class RangePicker<TValue> : DatePickerBase<TValue[]>
    {
        public RangePicker()
        {
            IsRange = true;

            Value = new TValue[2];

            DisabledDate = (date) =>
            {
                if (_pickerStatus[0]._hadSelectValue && _inputEnd.IsOnFocused)
                {
                    DateTime? value = null;
                    GetIfNotNull(Value[0], notNullValue =>
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
                    GetIfNotNull(Value[1], notNullValue =>
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

            if (BindConverter.TryConvertTo(args.Value.ToString(), CultureInfo, out TValue changeValue))
            {
                Value[index] = changeValue;

                GetIfNotNull(changeValue, (notNullValue) =>
                {
                    _pickerValues[index] = notNullValue;
                });

                StateHasChanged();
            }

            UpdateCurrentValueAsString();
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
                if (Value[index] == null)
                {
                    return null;
                }

                return Convert.ToDateTime(Value[index], this.CultureInfo);
            }
            else if (_defaultValues[index] != null)
            {
                return (DateTime)_defaultValues[index];
            }

            return null;
        }

        public override void ChangeValue(DateTime value, int index = 0)
        {
            bool result = BindConverter.TryConvertTo<TValue>(
               value.ToString(CultureInfo), CultureInfo, out var dateTime);

            if (result)
            {
                Value[index] = dateTime;
            }

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
            CurrentValue[0] = default;
            CurrentValue[1] = default;
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
                    GetIfNotNull(Value[index], notNullValue =>
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
                    GetIfNotNull(Value[index], notNullValue =>
                    {
                        ChangePickerValue(notNullValue, index);
                    });
                }

                ChangeFocusTarget(false, true);
            }
        }

        private void GetIfNotNull(TValue value, Action<DateTime> notNullAction)
        {
            if (!_isNullable)
            {
                DateTime dateTime = Convert.ToDateTime(value, CultureInfo);
                if (dateTime != DateTime.MinValue)
                {
                    notNullAction?.Invoke(dateTime);
                }
            }
            if (_isNullable && value != null)
            {
                notNullAction?.Invoke(Convert.ToDateTime(value, CultureInfo));
            }
        }

    }
}
