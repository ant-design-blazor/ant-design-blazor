using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class DatePicker<TValue> : DatePickerBase<TValue>
    {
        [Parameter]
        public EventCallback<DateTimeChangedEventArgs> OnChange { get; set; }

        private DateTime _pickerValuesAfterInit;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ProcessDefaults();
            _pickerValuesAfterInit = PickerValues[0];

        }

        private void ProcessDefaults()
        {
            UseDefaultPickerValue[0] = true;
            if (DefaultPickerValue.Equals(default(TValue)))
            {
                if ((IsNullable && Value != null) || (!IsNullable && !Value.Equals(default(TValue))))
                {
                    DefaultPickerValue = Value;
                }
                else if ((IsNullable && DefaultValue != null) || (!IsNullable && !DefaultValue.Equals(default(TValue))))
                {
                    DefaultPickerValue = DefaultValue;
                }
                else if (!IsNullable && Value.Equals(default(TValue)))
                {
                    DefaultPickerValue = Value;
                }
                else
                {
                    UseDefaultPickerValue[0] = false;
                }
            }
            if (UseDefaultPickerValue[0])
            {
                PickerValues[0] = Convert.ToDateTime(DefaultPickerValue, CultureInfo);
            }
        }

        private async Task OnInputClick()
        {
            //Reset Picker to default in case it the picker value was changed
            //but no value was selected (for example when a user clicks next 
            //month but does not select any value)
            if (UseDefaultPickerValue[0] && DefaultPickerValue != null)
            {
                PickerValues[0] = _pickerValuesAfterInit;
            }
            await _dropDown.Show();

            // clear status
            _pickerStatus[0]._currentShowHadSelectValue = false;

            if (!_inputStart.IsOnFocused && _pickerStatus[0]._hadSelectValue && !UseDefaultPickerValue[0])
            {
                GetIfNotNull(Value, notNullValue =>
                {
                    ChangePickerValue(notNullValue);
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
            UseDefaultPickerValue[0] = false;
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

            if (!IsNullable && DefaultValue != null)
                CurrentValue = DefaultValue;
            else
                CurrentValue = default;
            Close();
        }

        private void GetIfNotNull(TValue value, Action<DateTime> notNullAction)
        {
            if (!IsNullable)
            {
                DateTime dateTime = Convert.ToDateTime(value, CultureInfo);
                if (dateTime != DateTime.MinValue)
                {
                    notNullAction?.Invoke(dateTime);
                }
            }
            if (IsNullable && value != null)
            {
                notNullAction?.Invoke(Convert.ToDateTime(value, CultureInfo));
            }
        }
    }
}
