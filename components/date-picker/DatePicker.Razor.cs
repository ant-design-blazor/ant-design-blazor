﻿using System;
using System.Threading.Tasks;
using AntDesign.Core.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

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
            if (UseDefaultPickerValue[0])
            {
                PickerValues[0] = Convert.ToDateTime(DefaultPickerValue, CultureInfo);
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
                GetIfNotNull(Value, notNullValue =>
                {
                    ChangePickerValue(notNullValue);
                });
            }
        }

        private DateTime? _cacheDuringInput;

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
            if (!_duringManualInput)
            {
                _duringManualInput = true;
                _cacheDuringInput = GetIndexValue(0);
            }

            if (FormatAnalyzer.TryPickerStringConvert(args.Value.ToString(), out TValue changeValue, IsNullable))
            {
                GetIfNotNull(changeValue, parsed =>
                {
                    _pickerStatus[0].SelectedValue = parsed;
                    ChangePickerValue(parsed, index);
                });
            }
        }

        protected override async Task OnBlur(int index)
        {
            if (_openingOverlay)
                return;

            if (_duringManualInput)
            {
                if (Value is null && _pickerStatus[0].SelectedValue is not null ||
                    !Convert.ToDateTime(Value, CultureInfo).Equals(_pickerStatus[0].SelectedValue))
                {
                    _pickerStatus[0].SelectedValue = null;
                    ChangePickerValue(_cacheDuringInput ?? _pickerValuesAfterInit);
                }
                _duringManualInput = false;
            }
            AutoFocus = false;
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

            if (key == "ENTER" || key == "TAB" || key == "ESCAPE")
            {
                _duringManualInput = false;

                if (key == "ESCAPE" && _dropDown.IsOverlayShow())
                {
                    Close();
                    await Js.FocusAsync(_inputStart.Ref);
                }
                else if (key == "ENTER")
                {
                    if (string.IsNullOrEmpty(_inputStart.Value))
                    {
                        if (!_dropDown.IsOverlayShow())
                            await _dropDown.Show();
                    }
                    else if (HasTimeInput && _pickerStatus[0].SelectedValue is not null)
                    {
                        await OnOkClick();
                    }
                    else if (_pickerStatus[0].SelectedValue is not null)
                    {
                        await OnSelect(_pickerStatus[0].SelectedValue.Value, 0);
                    }
                    else
                    {
                        if (!_dropDown.IsOverlayShow())
                            await _dropDown.Show();
                        else
                            Close();
                    }

                }
                else if (key == "TAB")
                {
                    Close();
                    AutoFocus = false;
                }
            }
            else if (key == "ARROWDOWN")
            {
                if (!_dropDown.IsOverlayShow())
                    await _dropDown.Show();
            }
            else if (key == "ARROWUP")
            {
                if (_dropDown.IsOverlayShow())
                    Close();
            }
            else if (!_dropDown.IsOverlayShow())
                await _dropDown.Show();
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
                return _pickerStatus[index].SelectedValue;
            }

            if (_pickerStatus[0].IsValueSelected)
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

            var currentValue = CurrentValue is not null ?
                Convert.ToDateTime(CurrentValue, CultureInfo)
                : (DateTime?)null;

            if (currentValue != value)
            {
                CurrentValue = THelper.ChangeType<TValue>(value);

                _ = InvokeOnChange();
            }
        }

        protected override void OnValueChange(TValue value)
        {
            base.OnValueChange(value);

            _pickerStatus[0].IsValueSelected = !(Value is null && (DefaultValue is not null || DefaultPickerValue is not null));

            GetIfNotNull(CurrentValue, (notNullValue) => PickerValues[0] = notNullValue);

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
            if (OnClearClick.HasDelegate)
                OnClearClick.InvokeAsync(null);

            _dropDown.SetShouldRender(true);

            OnChange.InvokeAsync(new DateTimeChangedEventArgs
            {
                Date = GetIndexValue(0),
                DateString = GetInputValue(0)
            });

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
            await OnChange.InvokeAsync(new DateTimeChangedEventArgs
            {
                Date = GetIndexValue(0),
                DateString = GetInputValue(0)
            });
        }
    }
}
