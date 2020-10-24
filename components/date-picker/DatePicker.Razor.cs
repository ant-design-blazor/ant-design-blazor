using System;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class DatePicker<TValue> : DatePickerBase<TValue>
    {
        [Parameter]
        public EventCallback<OnDateChangeEventArgs> OnChange { get; set; }

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
                    PickerValues[index] = notNullValue;
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
            else if (DefaultValues[index] != null)
            {
                return DefaultValues[index];
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

            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(new OnDateChangeEventArgs
                {
                    Date = value,
                    DateString = GetInputValue(index)
                });
            }
        }

        protected override void OnValueChange(TValue value)
        {
            base.OnValueChange(value);

            _pickerStatus[0]._hadSelectValue = true;
        }

        public override void ClearValue(int index = 0)
        {
            _isSetPicker = false;
            CurrentValue = default;
            Close();
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
