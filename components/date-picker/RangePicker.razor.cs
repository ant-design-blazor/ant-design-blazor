using System;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Core.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class RangePicker<TValue> : DatePickerBase<TValue>
    {
        private TValue _value;
        private TValue _lastValue;

        /// <summary>
        /// Gets or sets the value of the input. This should be used with two-way binding.
        /// </summary>
        /// <example>
        /// @bind-Value="model.PropertyName"
        /// </example>
        [Parameter]
        public override sealed TValue Value
        {
            get { return _value; }
            set
            {
                // initial value is null, return directly
                if (value is null && _lastValue is null)
                {
                    return;
                }

                // set null, then clear the values
                if (value is null && _lastValue is not null)
                {
                    ClearValue();
                    return;
                }

                TValue orderedValue = SortValue(value);

                var hasChanged = _lastValue is null || (IsNullable ? !Enumerable.SequenceEqual(orderedValue as DateTime?[], _lastValue as DateTime?[]) :
                                                            !Enumerable.SequenceEqual(orderedValue as DateTime[], _lastValue as DateTime[]));
                if (hasChanged)
                {
                    _value = orderedValue;

                    _lastValue ??= CreateInstance();
                    Array.Copy(orderedValue as Array, _lastValue as Array, 2);

                    GetIfNotNull(_value, 0, (notNullValue) => PickerValues[0] = notNullValue);
                    GetIfNotNull(_value, 1, (notNullValue) => PickerValues[1] = notNullValue);

                    OnValueChange(orderedValue);
                }
            }
        }

        private readonly DateTime[] _pickerValuesAfterInit = new DateTime[2];

        [Parameter]
        public EventCallback<DateRangeChangedEventArgs> OnChange { get; set; }

        private bool ShowFooter => !IsShowTime && (RenderExtraFooter != null || ShowRanges);

        private bool ShowRanges => Ranges?.Count > 0;

        public RangePicker()
        {
            IsRange = true;

            DisabledDate = (date) =>
            {
                int? index = null;

                if (_inputEnd.IsOnFocused && GetIndexValue(0) is not null)
                {
                    index = 0;
                }
                else if (_inputStart.IsOnFocused && GetIndexValue(1) is not null)
                {
                    index = 1;
                }

                if (index is null)
                {
                    return false;
                }

                if (_pickerStatus[index.Value].SelectedValue is null)
                {
                    return false;
                }

                DateTime? value = GetIndexValue(index.Value);

                if (value is null)
                {
                    return false;
                }

                var date1 = date.Date;
                var date2 = ((DateTime)value).Date;

                if (Picker == DatePickerType.Week)
                {
                    var date1Week = DateHelper.GetWeekOfYear(date1, Locale.FirstDayOfWeek);
                    var date2Week = DateHelper.GetWeekOfYear(date2, Locale.FirstDayOfWeek);
                    return index == 0 ? date1Week < date2Week : date1Week > date2Week;
                }
                else
                {
                    var formattedDate1 = DateHelper.FormatDateByPicker(date1, Picker);
                    var formattedDate2 = DateHelper.FormatDateByPicker(date2, Picker);
                    return index == 0 ? formattedDate1 < formattedDate2 : formattedDate1 > formattedDate2;
                }
            };
        }

        private async Task OnInputClick(int index)
        {
            _duringFocus = false;
            if (_duringManualInput)
            {
                return;
            }
            _openingOverlay = !_dropDown.IsOverlayShow();

            //Reset Picker to default in case the picker value was changed
            //but no value was selected (for example when a user clicks next
            //month but does not select any value)

            var currentValue = GetIndexValue(index);

            if (currentValue is not null)
            {
                if (index == 0 || IsShowTime)
                {
                    PickerValues[index] = currentValue.Value;
                }
                else
                {
                    var otherValue = GetIndexValue(Math.Abs(index - 1));

                    PickerValues[index] = Picker switch
                    {
                        DatePickerType.Year when DateHelper.IsSameDecade(currentValue, otherValue) => currentValue.Value,
                        DatePickerType.Week or DatePickerType.Date when DateHelper.IsSameMonth(currentValue, otherValue) => currentValue.Value,
                        DatePickerType.Quarter or DatePickerType.Month when DateHelper.IsSameYear(currentValue, otherValue) => currentValue.Value,
                        _ => GetClosingDate(currentValue.Value, -1)
                    };
                }
            }
            else if (UseDefaultPickerValue[index] && DefaultPickerValue is not null)
            {
                PickerValues[index] = Convert.ToDateTime(DefaultPickerValue, CultureInfo);
            }
            else
            {
                PickerValues[index] = _pickerValuesAfterInit[index];
            }

            await _dropDown.Show();

            if (index == 0)
            {
                // change start picker value
                if (!_inputStart.IsOnFocused && _pickerStatus[index].IsValueSelected && !UseDefaultPickerValue[index])
                {
                    GetIfNotNull(Value, index, notNullValue =>
                    {
                        ChangePickerValue(notNullValue, index);
                    });
                }

                ChangeFocusTarget(true, false);
            }
            else
            {
                // change end picker value
                if (!_inputEnd.IsOnFocused && _pickerStatus[index].IsValueSelected && !UseDefaultPickerValue[index])
                {
                    GetIfNotNull(Value, index, notNullValue =>
                    {
                        ChangePickerValue(notNullValue, index);
                    });
                }

                ChangeFocusTarget(false, true);
            }
        }

        protected void OnInput(ChangeEventArgs args, int index = 0)
        {
            if (args == null)
            {
                return;
            }

            if (!_duringManualInput)
            {
                _duringManualInput = true;
            }

            if (FormatAnalyzer.TryPickerStringConvert(args.Value.ToString(), out DateTime parsedValue, false))
            {
                _pickerStatus[index].SelectedValue = parsedValue;
                ChangePickerValue(parsedValue, index);
            }
        }

        /// <summary>
        /// Method is called via EventCallBack if the keyboard key is no longer pressed inside the Input element.
        /// </summary>
        /// <param name="e">Contains the key (combination) which was pressed inside the Input element</param>
        /// <param name="index">Refers to picker index - 0 for starting date, 1 for ending date</param>
        protected async Task OnKeyDown(KeyboardEventArgs e, int index)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var key = e.Key.ToUpperInvariant();

            var isEnter = key == "ENTER";
            var isTab = key == "TAB";
            var isEscape = key == "ESCAPE";
            var isOverlayShown = _dropDown.IsOverlayShow();

            if (isEnter || isTab || isEscape)
            {
                if (_duringManualInput)
                {
                    //A scenario when there are a lot of controls;
                    //It may happen that incorrect values were entered into one of the input
                    //followed by ENTER key. This event may be fired before input manages
                    //to get the value. Here we ensure that input will get that value.
                    await Task.Delay(5);
                    _duringManualInput = false;
                }
                var input = (index == 0 ? _inputStart : _inputEnd);

                if (isEnter || isTab)
                {
                    if (HasTimeInput && _pickerStatus[index].SelectedValue is not null)
                    {
                        await OnOkClick();
                    }
                    else if (_pickerStatus[index].SelectedValue is not null)
                    {
                        await OnSelect(_pickerStatus[index].SelectedValue.Value, index);
                    }
                    else if (isOverlayShown)
                    {
                        if (_pickerStatus[index].SelectedValue is null && _pickerStatus[index].IsValueSelected)
                        {
                            _pickerStatus[index].SelectedValue = GetIndexValue(index);
                        }
                        if (isTab || !await SwitchFocus(index))
                        {
                            Close();

                            if (isTab && index == 1)
                            {
                                AutoFocus = false;
                            }
                        }

                    }
                    else if (!isTab)
                    {
                        await _dropDown.Show();
                    }
                }
                else if (isEscape && isOverlayShown)
                {
                    Close();
                    await Js.FocusAsync(input.Ref);
                }
            }
            else if (key == "ARROWUP")
            {
                if (isOverlayShown)
                {
                    Close();
                    AutoFocus = true;
                }
            }
            else if (!isOverlayShown)
                await _dropDown.Show();
        }


        private async Task OnFocus(int index)
        {
            _duringFocus = true;
            if (index == 0)
            {
                if (!_inputStart.IsOnFocused)
                {
                    await Blur(1);
                    await Focus(0);
                }
            }
            else
            {
                if (!_inputEnd.IsOnFocused)
                {
                    await Blur(0);
                    await Focus(1);
                }
            }
            AutoFocus = true;
        }

        protected override async Task OnBlur(int index)
        {
            //Await for Focus event - if it is going to happen, it will be
            //right after OnBlur. Best way to achieve that is to wait.
            //Task.Yield() does not work here.
            await Task.Delay(1);
            if (_duringFocus)
            {
                _duringFocus = false;
                _shouldRender = false;
                return;
            }
            if (_openingOverlay)
            {
                return;
            }
            _duringManualInput = false;
            AutoFocus = false;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            RangePickerDefaults.ProcessDefaults(Value, DefaultValue, DefaultPickerValue, PickerValues, UseDefaultPickerValue);
            _pickerValuesAfterInit[0] = PickerValues[0];
            _pickerValuesAfterInit[1] = PickerValues[1];
            if (_value == null)
            {
                _value = CreateInstance();
                ValueChanged.InvokeAsync(_value);
            }
            ResetPlaceholder();
        }

        /// <summary>
        /// Handle change of values.
        /// When values are changed, PickerValues should point to those new values
        /// or current date if no values were passed.
        /// </summary>
        /// <param name="value"></param>
        protected override void OnValueChange(TValue value)
        {
            base.OnValueChange(value);
            //reset all only if not changed using picker
            if (_inputStart?.IsOnFocused != true && _inputEnd?.IsOnFocused != true) // is null or false
            {
                UseDefaultPickerValue[0] = false;
                UseDefaultPickerValue[1] = false;
                _pickerStatus[0].IsValueSelected = true;
                _pickerStatus[1].IsValueSelected = true;
            }
        }

        /// <summary>
        /// Get value by picker index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override DateTime? GetIndexValue(int index)
        {
            if (_pickerStatus[index].SelectedValue is null)
            {
                var isFocused = index == 0 && _inputStart?.IsOnFocused == true ||
                     index == 1 && _inputEnd?.IsOnFocused == true;

                DateTime? currentValue;

                if (isFocused && (currentValue = GetValue(index)) is not null
                    && _pickerStatus[Math.Abs(index - 1)].SelectedValue is not null
                    && !IsValidRange(currentValue.Value, index))
                {
                    return null;
                }
            }

            if (_pickerStatus[index].SelectedValue is not null)
            {
                return _pickerStatus[index].SelectedValue;
            }

            if (Value != null)
            {
                return GetValue(index);
            }
            else if (!IsTypedValueNull(DefaultValue, index, out var defaultValue))
            {
                return defaultValue;
            }
            return null;
        }

        private DateTime? GetValue(int index)
        {
            var array = Value as Array;
            var indexValue = array.GetValue(index);

            if (indexValue == null)
            {
                return null;
            }

            return Convert.ToDateTime(indexValue, CultureInfo);
        }

        private static bool IsTypedValueNull(TValue value, int index, out DateTime? outValue)
        {
            outValue = (DateTime?)(value as Array)?.GetValue(index);
            return outValue == null;
        }

        public override void ChangeValue(DateTime value, int index = 0, bool closeDropdown = true)
        {
            bool isValueInstantiated = Value == null;
            if (isValueInstantiated)
            {
                Value = CreateInstance();
            }
            UseDefaultPickerValue[index] = false;

            var array = Value as Array;

            var currentValue = array.GetValue(index) as DateTime?;

            var isValueChanged = currentValue != _pickerStatus[index].SelectedValue;

            if (isValueChanged)
            {
                array.SetValue(value, index);
            }

            //if Value was just now instantiated then set the other index to existing DefaultValue
            if (isValueInstantiated && DefaultValue != null)
            {
                var arrayDefault = DefaultValue as Array;
                int oppositeIndex = index == 1 ? 0 : 1;
                array.SetValue(arrayDefault.GetValue(oppositeIndex), oppositeIndex);
            }

            _pickerStatus[index].IsValueSelected = true;

            if (closeDropdown && !HasTimeInput)
            {
                if (_pickerStatus[0].SelectedValue is not null
                    && _pickerStatus[1].SelectedValue is not null)
                {
                    Close();
                }
                // if the other DatePickerInput is disabled, then close picker panel
                else if (IsDisabled(Math.Abs(index - 1)))
                {
                    Close();
                }
            }

            var startDate = array.GetValue(0) as DateTime?;
            var endDate = array.GetValue(1) as DateTime?;

            if (isValueChanged && startDate is not null
                                    && endDate is not null)
            {
                InvokeOnChange();
            }

            if (_isNotifyFieldChanged && (Form?.ValidateOnChange == true))
            {
                EditContext?.NotifyFieldChanged(FieldIdentifier);
            }
        }

        public override void ClearValue(int index = -1, bool closeDropdown = true)
        {
            _isSetPicker = false;

            var array = CurrentValue as Array;
            int[] indexToClear = index == -1 ? new[] { 0, 1 } : new[] { index };

            foreach (var i in indexToClear)
            {
                if (!IsNullable && DefaultValue != null)
                {
                    var defaults = DefaultValue as Array;
                    array.SetValue(defaults.GetValue(i), i);
                }
                else
                {
                    array.SetValue(default, i);
                }

                _pickerStatus[i].SelectedValue = null;
                _pickerStatus[i].IsValueSelected = false;
                PickerValues[i] = _pickerValuesAfterInit[i];
                ResetPlaceholder(i);
            }

            if (closeDropdown)
            {
                Close();
            }

            if (array.GetValue(0) is null && array.GetValue(1) is null)
            {
                InvokeOnChange();
            }

            if (OnClearClick.HasDelegate)
                OnClearClick.InvokeAsync(null);

            _dropDown.SetShouldRender(true);
        }

        private void InvokeOnChange()
        {
            var array = Value as Array;
            OnChange.InvokeAsync(new DateRangeChangedEventArgs
            {
                Dates = new DateTime?[] { array.GetValue(0) as DateTime?, array.GetValue(1) as DateTime? },
                DateStrings = new string[] { GetInputValue(0), GetInputValue(1) }
            });
        }

        private void GetIfNotNull(TValue value, int index, Action<DateTime> notNullAction)
        {
            var array = value as Array;
            var indexValue = array.GetValue(index);

            if (!IsNullable)
            {
                DateTime dateTime = Convert.ToDateTime(indexValue, CultureInfo);
                if (dateTime != DateTime.MinValue)
                {
                    notNullAction?.Invoke(dateTime);
                }
            }
            if (IsNullable && indexValue != null)
            {
                notNullAction?.Invoke(Convert.ToDateTime(indexValue, CultureInfo));
            }
        }

        private TValue CreateInstance()
        {
            if (DefaultValue is not null)
                return (TValue)(DefaultValue as Array).Clone();

            if (IsNullable)
            {
                return (TValue)Array.CreateInstance(typeof(DateTime?), 2).Clone();
            }
            else
            {
                return (TValue)Array.CreateInstance(typeof(DateTime), 2).Clone();
            }
        }

        protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
        {
            result = default;
            validationErrorMessage = $"{FieldIdentifier.FieldName} field isn't valid.";

            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            string[] values = value.Split(",");

            if (values.Length != 2)
            {
                return false;
            }

            var success0 = BindConverter.TryConvertTo<DateTime>(values[0], CultureInfo, out var dateTime0);
            var success1 = BindConverter.TryConvertTo<DateTime>(values[1], CultureInfo, out var dateTime1);

            if (success0 && success1)
            {
                result = CreateInstance();

                var array = result as Array;

                array.SetValue(dateTime0, 0);
                array.SetValue(dateTime1, 1);

                validationErrorMessage = null;

                return true;
            }

            return false;
        }

        private void OverlayVisibleChange(bool visible)
        {
            _openingOverlay = false;
            _duringFocus = false;
            OnOpenChange.InvokeAsync(visible);
            InvokeInternalOverlayVisibleChanged(visible);
        }

        private async Task OnSuffixIconClick()
        {
            await Focus();
            await OnInputClick(0);
        }
    }
}
