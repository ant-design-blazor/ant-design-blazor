using System;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class DatePicker<TValue> : DatePickerBase<TValue>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Value != null)
            {
                GetIfNotNull(Value, notNullValue =>
                {
                    ChangeValue(notNullValue, 0);
                });
            }
        }

        protected void OnInput(ChangeEventArgs args, int index = 0)
        {
            if (args == null)
            {
                return;
            }

            if (TryParseValueFromString(args.Value.ToString(), out TValue changeValue, out _))
            {
                Value = changeValue;

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
                if (Value == null)
                {
                    return null;
                }

                return Convert.ToDateTime(Value, this.CultureInfo);
            }
            else if (_defaultValues[index] != null)
            {
                return _defaultValues[index];
            }

            return null;
        }

        public override void ChangeValue(DateTime value, int index = 0)
        {
            bool result = BindConverter.TryConvertTo<TValue>(
               value.ToString(CultureInfo), CultureInfo, out var dateTime);

            if (result)
            {
                Value = dateTime;
            }

            _pickerStatus[index]._hadSelectValue = true;

            UpdateCurrentValueAsString();

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
            _pickerStatus[index]._hadSelectValue = false;
            UpdateCurrentValueAsString();
            Close();
        }
    }
}
