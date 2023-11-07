using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Core.Extensions;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class RangePicker<TValue> : DatePickerBase<TValue>
    {
        private TValue _value;
        private TValue _lastValue;
        private TValue _initValue;

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

                var hasChanged = _lastValue is null || !InternalConvert.SequenceEqual(orderedValue, _lastValue);

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
        public EventCallback<DateRangeChangedEventArgs<TValue>> OnChange { get; set; }

        private bool ShowFooter => !IsShowTime && (RenderExtraFooter != null || ShowRanges);

        private bool ShowRanges => Ranges is { Count: > 0 };

        private readonly Func<DateTime, bool> _defaultDisabledDateCheck;

        private Func<DateTime, bool> _disabledDate;

        [Parameter]
        public override Func<DateTime, bool> DisabledDate
        {
            get
            {
                return _disabledDate;
            }
            set
            {
                _disabledDate = (date) => (value?.Invoke(date) is true) || _defaultDisabledDateCheck(date);
            }
        }

        public RangePicker()
        {
            IsRange = true;

            _defaultDisabledDateCheck = (date) =>
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

                var value = GetIndexValue(index.Value);

                if (value is null)
                {
                    return false;
                }

                var date1 = date.Date;
                var date2 = value.Value.Date;

                if (Picker == DatePickerType.Week)
                {
                    var calendar = CultureInfo.Calendar;
                    var calendarWeekRule = CultureInfo.DateTimeFormat.CalendarWeekRule;

                    var date1Week = calendar.GetWeekOfYear(date1, calendarWeekRule, Locale.FirstDayOfWeek);
                    var date2Week = calendar.GetWeekOfYear(date2, calendarWeekRule, Locale.FirstDayOfWeek);
                    return index == 0 ? date1Week < date2Week && date1.Year <= date2.Year
                                        : date1.Year >= date2.Year && date1Week > date2Week;
                }
                else
                {
                    var formattedDate1 = DateHelper.FormatDateByPicker(date1, Picker);
                    var formattedDate2 = DateHelper.FormatDateByPicker(date2, Picker);
                    return index == 0 ? formattedDate1 < formattedDate2 : formattedDate1 > formattedDate2;
                }
            };
            DisabledDate = null;
        }

        private async Task OnInputClick(int index)
        {
            if (_duringManualInput)
            {
                return;
            }
            _openingOverlay = !_dropDown.IsOverlayShow();

            //Reset Picker to default in case the picker value was changed
            //but no value was selected (for example when a user clicks next
            //month but does not select any value)

            var currentValue = GetIndexValue(index);

            if (currentValue.HasValue)
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
                PickerValues[index] = InternalConvert.ToDateTimeOffset(DefaultPickerValue).Value.DateTime;
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

            if (ChangeOnClose && _duringManualInput)
            {
                if (_pickerStatus[index].SelectedValue is not null)
                {
                    await OnSelect(_pickerStatus[index].SelectedValue.Value, index);
                }
                else if (AllowClear)
                {
                    ClearValue(index);
                }
            }

            if (_openingOverlay || _dropDown.IsOverlayShow())
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

            _initValue = (TValue)(_value as Array).Clone();
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
                return defaultValue?.DateTime;
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

            return InternalConvert.ToDateTime(indexValue);
        }

        private bool IsTypedValueNull(TValue value, int index, out DateTimeOffset? outValue)
        {
            var dateValue = (value as Array)?.GetValue(index);
            outValue = InternalConvert.ToDateTimeOffset(dateValue);
            return outValue == null;
        }

        public override void ChangeValue(DateTime value, int index = 0, bool closeDropdown = true)
        {
            if (DisabledDate(value))
            {
                return;
            }

            bool isValueInstantiated = Value == null;

            if (isValueInstantiated)
            {
                Value = CreateInstance();
            }

            UseDefaultPickerValue[index] = false;

            var defaultValue = InternalConvert.ToDateTimeOffset((DefaultValue as Array)?.GetValue(index));

            var currentValueArray = Value as Array;
            var currentIndexValue = InternalConvert.ToDateTimeOffset(currentValueArray?.GetValue(index));

            var newValue = new DateTimeOffset(DateTime.SpecifyKind(value, DateTimeKind.Unspecified), defaultValue?.Offset ?? currentIndexValue?.Offset ?? DateTimeOffset.Now.Offset);

            var isValueChanged = InternalConvert.ToDateTimeOffset(currentValueArray?.GetValue(index)) != newValue;

            if (isValueChanged)
            {
                if (currentValueArray is DateTime[] dateTimeArray)
                {
                    dateTimeArray.SetValue(newValue.DateTime, index);
                }
                else if (currentValueArray is DateTime?[] nullableDateTimeArray)
                {
                    nullableDateTimeArray.SetValue(newValue.DateTime, index);
                }
                else if (currentValueArray is DateTimeOffset[] dateTimeOffsetArray)
                {
                    dateTimeOffsetArray.SetValue(newValue, index);
                }
                else if (currentValueArray is DateTimeOffset?[] nullableDateTimeOffsetArray)
                {
                    nullableDateTimeOffsetArray.SetValue(newValue, index);
                }
#if NET6_0_OR_GREATER
                else if (currentValueArray is DateOnly[] dateOnly)
                {
                    dateOnly.SetValue(DateOnly.FromDateTime(newValue.DateTime), index);
                }
                else if (currentValueArray is DateOnly?[] nullableDateOnly)
                {
                    nullableDateOnly.SetValue(DateOnly.FromDateTime(newValue.DateTime), index);
                }
                else if (currentValueArray is TimeOnly[] timeOnly)
                {
                    timeOnly.SetValue(TimeOnly.FromDateTime(newValue.DateTime), index);
                }
                else if (currentValueArray is TimeOnly?[] nullableTimeOnly)
                {
                    nullableTimeOnly.SetValue(TimeOnly.FromDateTime(newValue.DateTime), index);
                }
#endif
                else
                {
                    throw new NotImplementedException("Type not supported");
                }
            }

            var otherIndex = Math.Abs(index - 1);

            //if Value was just now instantiated then set the other index to existing DefaultValue
            if (isValueInstantiated && DefaultValue != null)
            {
                var arrayDefault = DefaultValue as Array;
                currentValueArray.SetValue(arrayDefault.GetValue(otherIndex), otherIndex);
            }

            var startDate = currentValueArray.GetValue(0);
            var endDate = currentValueArray.GetValue(1);

            if (isValueChanged && startDate is not null && endDate is not null)
            {
                InvokeOnChange();
            }

            if (_isNotifyFieldChanged && (Form?.ValidateOnChange is true))
            {
                EditContext?.NotifyFieldChanged(FieldIdentifier);
            }

            _pickerStatus[index].IsValueSelected = true;

            if (closeDropdown && !HasTimeInput
                && _pickerStatus[index].SelectedValue is not null
                && (_pickerStatus[otherIndex].SelectedValue is not null || IsDisabled(otherIndex)))
            {
                Close();
            }
        }

        public override void ClearValue(int index = -1, bool closeDropdown = true)
        {
            _isSetPicker = false;

            var array = CurrentValue as Array;
            ReadOnlySpan<int> indexToClear;
            if (index == -1)
            {
                indexToClear = new[] { 0, 1 }; // For .NET 8+, using `ReadOnlySpan<int>` can avoid this array allocation
            }
            else
            {
                indexToClear = new[] { index };
            }

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

            if (array.GetValue(0) is null || array.GetValue(1) is null)
            {
                InvokeOnChange();
            }

            OnClear.InvokeAsync(null);
            OnClearClick.InvokeAsync(null);

            _dropDown.SetShouldRender(true);
        }

        internal override void ResetValue()
        {
            if (CurrentValue is Array currentArray)
            {
                _isNotifyFieldChanged = false;
                (_initValue as Array).CopyTo(currentArray, 0);
                _isNotifyFieldChanged = true;
            }
            else
            {
                base.ResetValue();
            }
        }

        private void InvokeOnChange()
        {
            OnChange.InvokeAsync(new DateRangeChangedEventArgs<TValue>
            {
                Dates = Value,
                DateStrings = new string[] { GetInputValue(0), GetInputValue(1) }
            });
        }

        private TValue CreateInstance()
        {
            if (DefaultValue is not null)
                return (TValue)(DefaultValue as Array).Clone();

            var type = typeof(TValue);

            if (IsNullable)
            {
                if (type.IsAssignableFrom(typeof(DateTime?[])))
                {
                    return (TValue)Array.CreateInstance(typeof(DateTime?), 2).Clone();
                }
                else if (type.IsAssignableFrom(typeof(DateTimeOffset?[])))
                {
                    return (TValue)Array.CreateInstance(typeof(DateTimeOffset?), 2).Clone();
                }
#if NET6_0_OR_GREATER
                else if (type.IsAssignableFrom(typeof(DateOnly?[])))
                {
                    return (TValue)Array.CreateInstance(typeof(DateOnly?), 2).Clone();
                }
                else if (type.IsAssignableFrom(typeof(TimeOnly?[])))
                {
                    return (TValue)Array.CreateInstance(typeof(TimeOnly?), 2).Clone();
                }
#endif
                else
                {
                    throw new NotSupportedException($"{type.FullName} not supported");
                }
            }
            else
            {
                if (type.IsAssignableFrom(typeof(DateTime[])))
                {
                    return (TValue)Array.CreateInstance(typeof(DateTime), 2).Clone();
                }
                else if (type.IsAssignableFrom(typeof(DateTimeOffset[])))
                {
                    return (TValue)Array.CreateInstance(typeof(DateTimeOffset), 2).Clone();
                }
#if NET6_0_OR_GREATER
                else if (type.IsAssignableFrom(typeof(DateOnly[])))
                {
                    return (TValue)Array.CreateInstance(typeof(DateOnly), 2).Clone();
                }
                else if (type.IsAssignableFrom(typeof(TimeOnly[])))
                {
                    return (TValue)Array.CreateInstance(typeof(DateOnly), 2).Clone();
                }
#endif
                else
                {
                    throw new NotSupportedException($"{type.FullName} not supported");
                }
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

            string[] values = value.Split(',');

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

        private async Task OverlayVisibleChange(bool isVisible)
        {
            _openingOverlay = false;
            await OnOpenChange.InvokeAsync(isVisible);
            InvokeInternalOverlayVisibleChanged(isVisible);
            if (!isVisible)
            {
                var index = GetOnFocusPickerIndex();
                await Focus(index);
            }
        }

        private async Task OnSuffixIconClick()
        {
            await Focus();
            await OnInputClick(0);
        }

        public bool ShowClear()
        {
            return CurrentValue is Array array && (array.GetValue(0) is not null || array.GetValue(1) is not null) && AllowClear;
        }
    }
}
