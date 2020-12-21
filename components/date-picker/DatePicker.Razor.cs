using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class DatePicker<TValue> : DatePickerBase<TValue>
    {
        [Parameter]
        public EventCallback<DateTimeChangedEventArgs> OnChange { get; set; }

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

        private async Task OnInputClick()
        {
            await _dropDown.Show();

            // clear status
            _pickerStatus[0]._currentShowHadSelectValue = false;

            if (!_inputStart.IsOnFocused && _pickerStatus[0]._hadSelectValue)
            {
                GetIfNotNull(Value, notNullValue =>
                {
                    ChangePickerValue(notNullValue, 0);
                });
            }
        }

        protected void OnInput(ChangeEventArgs args, int index = 0)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException("DatePicker should have only single picker.");
            }
            if (args == null)
            {
                return;
            }

            if (TryParseValueFromString(args.Value.ToString(), out TValue changeValue, out _))
            {
                CurrentValue = changeValue;

                GetIfNotNull(changeValue, (notNullValue) =>
                {
                    PickerValues[0] = notNullValue;
                });

                StateHasChanged();
            }

            UpdateCurrentValueAsString();
        }

        /// <summary>
        /// Get value of the picker
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override DateTime? GetIndexValue(int index = 0)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException("DatePicker should have only single picker.");
            }
            if (_pickerStatus[0]._hadSelectValue)
            {
                if (Value == null)
                {
                    return null;
                }

                return Convert.ToDateTime(Value, CultureInfo);
            }
            else if (DefaultValue != null)
            {
                return Convert.ToDateTime(DefaultValue, CultureInfo);
            }

            return null;
        }

        public override void ChangeValue(DateTime value, int index = 0)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException("DatePicker should have only single picker.");
            }
            bool result = BindConverter.TryConvertTo<TValue>(
               value.ToString(CultureInfo), CultureInfo, out var dateTime);

            if (result)
            {
                CurrentValue = dateTime;
            }

            _pickerStatus[0]._hadSelectValue = true;

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
                OnChange.InvokeAsync(new DateTimeChangedEventArgs
                {
                    Date = value,
                    DateString = GetInputValue(0)
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
