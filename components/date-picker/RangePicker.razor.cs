using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace AntDesign
{
    public partial class RangePicker<TValue> : DatePickerBase<TValue>
    {
        private TValue _value;

        /// <summary>
        /// Gets or sets the value of the input. This should be used with two-way binding.
        /// </summary>
        /// <example>
        /// @bind-Value="model.PropertyName"
        /// </example>
        [Parameter]
        public sealed override TValue Value
        {
            get { return _value; }
            set
            {
                TValue orderedValue = SortValue(value);
                var hasChanged = !EqualityComparer<TValue>.Default.Equals(orderedValue, Value);
                if (hasChanged)
                {
                    _value = orderedValue;
                    OnValueChange(orderedValue);
                }
            }
        }

        private DateTime[] _pickerValuesAfterInit = new DateTime[2];

        [Parameter]
        public EventCallback<DateRangeChangedEventArgs> OnChange { get; set; }

        public RangePicker()
        {
            IsRange = true;

            DisabledDate = (date) =>
            {
                var array = Value as Array;

                if (_pickerStatus[0]._hadSelectValue && _inputEnd.IsOnFocused)
                {
                    DateTime? value = null;
                    GetIfNotNull(Value, 0, notNullValue =>
                    {
                        value = notNullValue;
                    });

                    if (value != null)
                    {
                        return DateHelper.FormatDateByPicker(date.Date, Picker) < DateHelper.FormatDateByPicker(((DateTime)value).Date, Picker);
                    }
                }
                if (_pickerStatus[1]._hadSelectValue && _inputStart.IsOnFocused)
                {
                    DateTime? value = null;
                    GetIfNotNull(Value, 1, notNullValue =>
                    {
                        value = notNullValue;
                    });

                    if (value != null)
                    {
                        return DateHelper.FormatDateByPicker(date.Date, Picker) > DateHelper.FormatDateByPicker(((DateTime)value).Date, Picker);
                    }
                }

                return false;
            };
        }

        private async Task OnInputClick(int index)
        {
            if (_duringManualInput)
            {
                return;
            }
            //Reset Picker to default in case the picker value was changed
            //but no value was selected (for example when a user clicks next
            //month but does not select any value)
            if (UseDefaultPickerValue[index] && DefaultPickerValue != null)
            {
                PickerValues[index] = _pickerValuesAfterInit[index];
            }
            await _dropDown.Show();

            // clear status
            _pickerStatus[0]._currentShowHadSelectValue = false;
            _pickerStatus[1]._currentShowHadSelectValue = false;

            if (index == 0)
            {
                // change start picker value
                if (!_inputStart.IsOnFocused && _pickerStatus[index]._hadSelectValue && !UseDefaultPickerValue[index])
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
                if (!_inputEnd.IsOnFocused && _pickerStatus[index]._hadSelectValue && !UseDefaultPickerValue[index])
                {
                    GetIfNotNull(Value, index, notNullValue =>
                    {
                        ChangePickerValue(notNullValue, index);
                    });
                }

                ChangeFocusTarget(false, true);
            }
        }

        private DateTime? _cacheDuringInput;
        private DateTime _pickerValueCache;

        protected void OnInput(ChangeEventArgs args, int index = 0)
        {
            if (args == null)
            {
                return;
            }
            var array = Value as Array;
            if (!_duringManualInput)
            {
                _duringManualInput = true;
                _cacheDuringInput = array.GetValue(index) as DateTime?;
                _pickerValueCache = PickerValues[index];
            }
            if (FormatAnalyzer.TryPickerStringConvert(args.Value.ToString(), out DateTime changeValue, false))
            {
                array.SetValue(changeValue, index);

                ChangePickerValue(changeValue, index);


                StateHasChanged();
            }

            UpdateCurrentValueAsString();
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
            if (key == "ENTER" || key == "TAB")
            {
                _duringManualInput = false;
                var input = (index == 0 ? _inputStart : _inputEnd);
                if (string.IsNullOrWhiteSpace(input.Value))
                {
                    ClearValue(index, false);
                    await Focus(index);
                    return;
                }
                else if (!await TryApplyInputValue(index, input.Value))
                    return;

                if (index == 1)
                {
                    if (key != "TAB")
                    {
                        //needed only in wasm, details: https://github.com/dotnet/aspnetcore/issues/30070
                        await Task.Yield();
                        await Js.InvokeVoidAsync(JSInteropConstants.InvokeTabKey);
                        Close();
                    }
                    else if (!e.ShiftKey)
                    {
                        Close();
                    }
                }
                if (index == 0)
                {
                    if (key == "TAB" && e.ShiftKey)
                    {
                        Close();
                        AutoFocus = false;
                    }
                    else if (key != "TAB")
                    {
                        await Blur(0);
                        await Focus(1);
                    }
                }
                return;
            }
            if (key == "ARROWDOWN" && !_dropDown.IsOverlayShow())
            {
                await _dropDown.Show();
                return;
            }
            if (key == "ARROWUP" && _dropDown.IsOverlayShow())
            {
                Close();
                await Task.Yield();
                AutoFocus = true;
                return;
            }
        }

        private async Task<bool> TryApplyInputValue(int index, string inputValue)
        {
            if (FormatAnalyzer.TryPickerStringConvert(inputValue, out DateTime changeValue, false))
            {
                var array = Value as Array;
                array.SetValue(changeValue, index);
                var validationSuccess = await ValidateRange(index, changeValue, array);
                if (OnChange.HasDelegate)
                {
                    await OnChange.InvokeAsync(new DateRangeChangedEventArgs
                    {
                        Dates = new DateTime?[] { array.GetValue(0) as DateTime?, array.GetValue(1) as DateTime? },
                        DateStrings = new string[] { GetInputValue(0), GetInputValue(1) }
                    });
                }
                return validationSuccess;
            }
            return false;
        }
        private async Task<bool> ValidateRange(int index, DateTime newDate, Array array)
        {
            if (index == 0 && array.GetValue(1) is not null && ((DateTime)array.GetValue(1)).CompareTo(newDate) < 0)
            {
                ClearValue(1, false);
                await Blur(0);
                await Focus(1);
                return false;
            }
            else if (index == 1)
            {
                if (array.GetValue(0) is not null && newDate.CompareTo((DateTime)array.GetValue(0)) < 0)
                {
                    ClearValue(0, false);
                    await Blur(1);
                    await Focus(0);
                    return false;
                }
                else if (array.GetValue(0) is null)
                {
                    await Blur(1);
                    await Focus(0);
                    return false;
                }
            }
            return true;
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

        protected override Task OnBlur(int index)
        {
            if (_duringManualInput)
            {
                var array = Value as Array;

                if (!array.GetValue(index).Equals(_cacheDuringInput))
                {
                    //reset picker to Value
                    if (IsNullable)
                        array.SetValue(_cacheDuringInput, index);
                    else
                        array.SetValue(_cacheDuringInput.GetValueOrDefault(), index);

                    _pickerStatus[index]._hadSelectValue = !(Value is null && (DefaultValue is not null || DefaultPickerValue is not null));
                    ChangePickerValue(_pickerValueCache, index);
                }
                _duringManualInput = false;
            }
            AutoFocus = false;
            return Task.CompletedTask;
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
                _pickerStatus[0]._hadSelectValue = true;
                _pickerStatus[1]._hadSelectValue = true;
            }
        }

        /// <summary>
        /// Get value by picker index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override DateTime? GetIndexValue(int index)
        {
            if (Value != null)
            {
                var array = Value as Array;
                var indexValue = array.GetValue(index);

                if (indexValue == null)
                {
                    return null;
                }

                return Convert.ToDateTime(indexValue, CultureInfo);
            }
            else if (!IsTypedValueNull(DefaultValue, index, out var defaultValue))
            {
                return defaultValue;
            }
            return null;
        }

        private static bool IsTypedValueNull(TValue value, int index, out DateTime? outValue)
        {
            outValue = (DateTime?)(value as Array)?.GetValue(index);
            return outValue == null;
        }

        public override void ChangeValue(DateTime value, int index = 0)
        {
            bool isValueInstantiated = Value == null;
            if (isValueInstantiated)
            {
                Value = CreateInstance();
            }
            UseDefaultPickerValue[index] = false;
            var array = Value as Array;

            array.SetValue(value, index);

            //if Value was just now instantiated then set the other index to existing DefaultValue
            if (isValueInstantiated && IsRange && DefaultValue != null)
            {
                var arrayDefault = DefaultValue as Array;
                int oppositeIndex = index == 1 ? 0 : 1;
                array.SetValue(arrayDefault.GetValue(oppositeIndex), oppositeIndex);
            }

            _pickerStatus[index]._hadSelectValue = true;
            _pickerStatus[index]._currentShowHadSelectValue = true;

            UpdateCurrentValueAsString(index);

            if (!IsShowTime && Picker != DatePickerType.Time)
            {
                if (_pickerStatus[0]._currentShowHadSelectValue && _pickerStatus[1]._currentShowHadSelectValue)
                {
                    Close();
                }
            }

            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(new DateRangeChangedEventArgs
                {
                    Dates = new DateTime?[] { array.GetValue(0) as DateTime?, array.GetValue(1) as DateTime? },
                    DateStrings = new string[] { GetInputValue(0), GetInputValue(1) }
                });
            }
        }

        public override void ClearValue(int index = -1, bool closeDropdown = true)
        {
            _isSetPicker = false;

            var array = CurrentValue as Array;
            int[] indexToClear = index == -1 ? new[] { 0, 1 } : new[] { index };

            string[] pickerHolders = new string[2];
            (pickerHolders[0], pickerHolders[1]) = DatePickerPlaceholder.GetRangePlaceHolderByType(_pickerStatus[0]._initPicker, Locale);

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
                _placeholders[i] = pickerHolders[i];
                _pickerStatus[i]._hadSelectValue = false;
            }

            if (closeDropdown)
                Close();
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

        protected override void UpdateCurrentValueAsString(int index = 0)
        {
            if (EditContext != null)
            {
                CurrentValueAsString = $"{GetInputValue(0)},{GetInputValue(1)}";
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
    }
}
