using System;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class DatePicker : DatePickerBase<DateTime>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Value != null && Value != DateTime.MinValue)
            {
                ChangeValue(Value, 0);
            }
        }


        protected void OnInput(ChangeEventArgs args, int index = 0)
        {
            if (args == null)
            {
                return;
            }

            if (DateTime.TryParse(args.Value.ToString(), out DateTime changeValue))
            {
                Value = changeValue;
                _pickerValues[index] = changeValue;

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
                return Value;
            }
            else if (_defaultValues[index] != null)
            {
                return (DateTime)_defaultValues[index];
            }
            else
            {
                return null;
            }
        }

        public override void ChangeValue(DateTime value, int index = 0)
        {
            Value = value;

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
    }
}
