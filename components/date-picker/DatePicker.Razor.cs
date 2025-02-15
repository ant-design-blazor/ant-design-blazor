// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using AntDesign.Core.Documentation;
using AntDesign.Core.Extensions;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /**
    <summary>
    <para>To select or input a date.</para>

    <h2>When To Use</h2>

    <para>By clicking the input box, you can select a date from a popup calendar.</para>
    </summary>
    <seealso cref="MonthPicker{TValue}"/>
    <seealso cref="RangePicker{TValue}"/>
    <seealso cref="WeekPicker{TValue}"/>
    <seealso cref="YearPicker{TValue}"/>
    <seealso cref="QuarterPicker{TValue}"/>
    <seealso cref="TriggerBoundaryAdjustMode"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "https://gw.alipayobjects.com/zos/alicdn/RT_USzA48/DatePicker.svg", Title = "DatePicker", SubTitle = "日期选择框")]
    public partial class DatePicker<TValue> : DatePickerBase<TValue>
    {
        /// <summary>
        /// Callback executed when the selected value changes
        /// </summary>
        [Parameter]
        public EventCallback<DateTimeChangedEventArgs<TValue>> OnChange { get; set; }

        /// <summary>
        /// Disable the date picker. 
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        private DateTime _pickerValuesAfterInit;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ProcessDefaults();
            _pickerValuesAfterInit = PickerValues[0];
        }


        /// <summary>
        /// Add focus to picker input
        /// </summary>
        /// <returns></returns>
        [PublicApi("1.0.0")]
        public async Task FocusAsync()
        {
            await base.Focus();
        }

        /// <summary>
        /// Remove focus from picker input
        /// </summary>
        /// <returns></returns>
        [PublicApi("1.0.0")]
        public async Task BlurAsync()
        {
            await base.Blur();
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
            _duringManualInput = false;

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

            // Key is not always present with autoprefill so we skip
            if (e.Key == null) return;

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
        internal override DateTime? GetIndexValue(int index = 0)
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

        internal override void ChangeValue(DateTime value, int index = 0, bool closeDropdown = true)
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

            ToDateTimeOffset(FormatDateTime(value), out DateTimeOffset? currentValue, out DateTimeOffset newValue);

            if (currentValue != newValue)
            {
                CurrentValue = InternalConvert.FromDateTimeOffset<TValue>(newValue);

                InvokeOnChange();
            }
        }

        protected override void OnValueChange(TValue value)
        {
            base.OnValueChange(value);

            _pickerStatus[0].IsValueSelected = !(Value is null && (DefaultValue is not null || DefaultPickerValue is not null));

            GetIfNotNull(CurrentValue, 0, (notNullValue) => PickerValues[0] = notNullValue);

            _dropDown?.SetShouldRender(true);
        }

        internal override void ClearValue(int index = 0, bool closeDropdown = true)
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

        protected override void InvokeOnChange()
        {
            OnChange.InvokeAsync(new DateTimeChangedEventArgs<TValue>
            {
                Date = Value,
                DateString = GetInputValue(0)
            });
        }

        protected override bool IsDisabled(int? index = null)
        {
            return Disabled;
        }
    }
}
