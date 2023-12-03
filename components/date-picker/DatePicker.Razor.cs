using System;
using System.Threading.Tasks;
using AntDesign.Core.Extensions;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class DatePicker<TValue> : DatePickerBase<TValue>
    {
        [Parameter]
        public EventCallback<DateTimeChangedEventArgs<TValue>> OnChange { get; set; }

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
            if (DefaultPickerValue?.Equals(default(TValue)) == true)
            {
                if ((IsNullable && Value != null) || (!IsNullable && !Value.Equals(default(TValue))))
                {
                    DefaultPickerValue = Value;
                }
                else if ((IsNullable && DefaultValue != null) || (!IsNullable && !DefaultValue.Equals(default(TValue))))
                {
                    DefaultPickerValue = DefaultValue;
                    if (!DefaultValue.Equals(Value))
                    {
                        CurrentValue = DefaultValue;
                    }
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
            if (UseDefaultPickerValue[0] && DefaultPickerValue is not null)
            {
                PickerValues[0] = InternalConvert.ToDateTime(DefaultPickerValue).Value;
            }
        }

        private async Task OnInputClick()
        {
            if (_duringManualInput)
            {
                return;
            }
            _openingOverlay = !_dropDown.IsOverlayShow();

            AutoFocus = true;
            //Reset Picker to default in case it the picker value was changed
            //but no value was selected (for example when a user clicks next
            //month but does not select any value)
            if (!_pickerStatus[0].IsValueSelected && UseDefaultPickerValue[0] && DefaultPickerValue != null)
            {
                PickerValues[0] = _pickerValuesAfterInit;
            }
            await _dropDown.Show();

            if (!_inputStart.IsOnFocused && _pickerStatus[0].IsValueSelected && !UseDefaultPickerValue[0])
            {
                GetIfNotNull(Value, 0, notNullValue =>
                {
                    ChangePickerValue(notNullValue);
                });
            }
        }

        protected override void OnInput(ChangeEventArgs args, int index = 0)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException("DatePicker should have only single picker.");
            }

            base.OnInput(args, index);
        }

        protected override async Task OnBlur(int index)
        {
            await base.OnBlur(index);

            if (_openingOverlay)
                return;

            AutoFocus = false;

            if (!_dropDown.IsOverlayShow())
            {
                _pickerStatus[0].SelectedValue = null;
            }

            await Task.Yield();
        }

        /// <summary>
        /// Method is called via EventCallBack if the keyboard key is no longer pressed inside the Input element.
        /// </summary>
        /// <param name="e">Contains the key (combination) which was pressed inside the Input element</param>
        protected async Task OnKeyDown(KeyboardEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            var key = e.Key.ToUpperInvariant();

            var isEnter = key == "ENTER";
            var isTab = key == "TAB";
            var isEscape = key == "ESCAPE";
            var isOverlayShown = _dropDown.IsOverlayShow();

            if (isEnter || isTab || isEscape)
            {
                _duringManualInput = false;

                if (isEscape && isOverlayShown)
                {
                    Close();
                    await Js.FocusAsync(_inputStart.Ref);
                }
                else if (isEnter || isTab)
                {
                    if (HasTimeInput && _pickerStatus[0].SelectedValue is not null)
                    {
                        await OnOkClick();
                    }
                    else if (_pickerStatus[0].SelectedValue is not null)
                    {
                        await OnSelect(_pickerStatus[0].SelectedValue.Value, 0);
                    }
                    else if (isOverlayShown)
                    {
                        Close();
                    }
                    else if (!isTab)
                    {
                        await _dropDown.Show();
                    }
                }
            }
            else if (key == "ARROWUP")
            {
                if (isOverlayShown)
                    Close();
            }
            else if (!isOverlayShown)
            {
                await _dropDown.Show();
            }
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

            if (_pickerStatus[index].SelectedValue is not null)
            {
                return _pickerStatus[index].SelectedValue.Value;
            }

            if (_pickerStatus[0].IsValueSelected)
            {
                if (Value == null)
                {
                    return null;
                }

                return InternalConvert.ToDateTime(Value);
            }

            if (DefaultValue != null)
            {
                return InternalConvert.ToDateTime(DefaultValue);
            }

            return null;
        }

        public override void ChangeValue(DateTime value, int index = 0, bool closeDropdown = true)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException("DatePicker should have only single picker.");
            }

            UseDefaultPickerValue[0] = false;

            if (closeDropdown && !IsShowTime && Picker != DatePickerType.Time)
            {
                Close();
            }

            if (IsDisabledDate(value))
            {
                return;
            }

            ToDateTimeOffset(value, out DateTimeOffset? currentValue, out DateTimeOffset newValue);

            if (currentValue != newValue)
            {
                CurrentValue = InternalConvert.FromDateTimeOffset<TValue>(newValue);

                _ = InvokeOnChange();
            }
        }

        protected override void OnValueChange(TValue value)
        {
            base.OnValueChange(value);

            _pickerStatus[0].IsValueSelected = !(Value is null && (DefaultValue is not null || DefaultPickerValue is not null));

            GetIfNotNull(CurrentValue, 0, (notNullValue) => PickerValues[0] = notNullValue);

            _dropDown?.SetShouldRender(true);
        }

        public override void ClearValue(int index = 0, bool closeDropdown = true)
        {
            _isSetPicker = false;

            if (!IsNullable && DefaultValue != null)
                CurrentValue = DefaultValue;
            else
                CurrentValue = default;

            PickerValues[0] = _pickerValuesAfterInit;

            if (closeDropdown)
                Close();

            OnClear.InvokeAsync(null);
            OnClearClick.InvokeAsync(null);

            OnChange.InvokeAsync(new DateTimeChangedEventArgs<TValue>
            {
                Date = Value,
                DateString = GetInputValue(0)
            });

            _dropDown.SetShouldRender(true);

            OnChange.InvokeAsync(new DateTimeChangedEventArgs<TValue>
            {
                Date = Value,
                DateString = GetInputValue(0)
            });
        }

        private void OverlayVisibleChange(bool visible)
        {
            OnOpenChange.InvokeAsync(visible);
            _openingOverlay = false;
            InvokeInternalOverlayVisibleChanged(visible);
        }

        private async Task OnSuffixIconClick()
        {
            await Focus();
            await OnInputClick();
        }

        private async Task InvokeOnChange()
        {
            await OnChange.InvokeAsync(new DateTimeChangedEventArgs<TValue>
            {
                Date = Value,
                DateString = GetInputValue(0)
            });
        }
    }
}
