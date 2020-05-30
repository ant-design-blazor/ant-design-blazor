using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class RangePicker : DatePickerBase<DateTime[]>
    {
        public RangePicker()
        {
            IsRange = true;

            Value = new DateTime[2];

            DisabledDate = (date) =>
            {
                if (_pickerStatus[0]._hadSelectValue && _inputEnd.IsOnFocused)
                {
                    return date.CompareTo(Value[0]) < 0;
                }
                if (_pickerStatus[1]._hadSelectValue && _inputStart.IsOnFocused)
                {
                    return date.CompareTo(Value[1]) > 0;
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

            if (DateTime.TryParse(args.Value.ToString(), out DateTime changeValue))
            {
                Value[index] = changeValue;
                _pickerValues[index] = changeValue;

                StateHasChanged();
            }

            UpdateCurrentValueAsString(index);
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
                return Value[index];
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
            Value[index] = value;

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
    }
}
