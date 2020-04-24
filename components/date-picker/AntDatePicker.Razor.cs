﻿using System;
using System.Threading.Tasks;
using AntBlazor.Internal;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntBlazor
{
    public partial class AntDatePicker : AntDomComponentBase
    {
        [Parameter]
        public string PrefixCls { get; set; } = "ant-picker";

        private readonly string[] _pickers = new string[] { AntDatePickerType.Date, AntDatePickerType.Date };
        private string _picker;
        private bool _isSetPicker = false;

        [Parameter]
        public string Picker
        {
            get => _picker;
            set
            {
                _isSetPicker = true;
                _picker = value;

                _pickers[0] = value;
                _pickers[1] = value;

                NoteInitPicker(value, 0);
                NoteInitPicker(value, 1);
            }
        }

        [Parameter]
        public bool Disabled { get; set; } = false;

        [Parameter]
        public bool Bordered { get; set; } = true;

        [Parameter]
        public bool AutoFocus { get; set; } = false;

        [Parameter]
        public bool Open { get; set; } = false;

        [Parameter]
        public bool InputReadOnly { get; set; } = false;

        [Parameter]
        public bool AllowClear { get; set; } = true; // TODO

        private string[] _placeholders = new string[] { "", "" };
        private OneOf<string, string[]> _placeholder;

        [Parameter]
        public OneOf<string, string[]> Placeholder
        {
            get => _placeholder;
            set
            {
                _placeholder = value;
                value.Switch(single =>
                {
                    _placeholders[0] = single;
                }, arr =>
                {
                    _placeholders[0] = arr.Length > 0 ? arr[0] : _placeholders[0];
                    _placeholders[1] = arr.Length > 1 ? arr[1] : _placeholders[1];
                });
            }
        }

        [Parameter]
        public string PopupStyle { get; set; }

        [Parameter]
        public string ClassName { get; set; }

        [Parameter]
        public string DropdownClassName { get; set; }

        [Parameter]
        public string Size { get; set; } = AntDatePickerSize.Middle;

        [Parameter]
        public string Format { get; set; }

        private readonly DateTime?[] _defaultValues = new DateTime?[2];
        private OneOf<DateTime, DateTime[]> _defaultValue;

        [Parameter]
        public OneOf<DateTime, DateTime[]> DefaultValue
        {
            get => _defaultValue;
            set
            {
                _defaultValue = value;
                value.Switch(single =>
                {
                    _defaultValues[0] = single;
                }, arr =>
                {
                    _defaultValues[0] = arr.Length > 0 ? arr[0] : _defaultValues[0];
                    _defaultValues[1] = arr.Length > 1 ? arr[1] : _defaultValues[1];
                });
            }
        }

        private readonly DateTime?[] _defaultPickerValues = new DateTime?[2];
        private OneOf<DateTime, DateTime[]> _defaultPickerValue;

        [Parameter]
        public OneOf<DateTime, DateTime[]> DefaultPickerValue
        {
            get => _defaultPickerValue;
            set
            {
                _defaultPickerValue = value;
                value.Switch(single =>
                {
                    _defaultPickerValues[0] = single;
                }, arr =>
                {
                    _defaultPickerValues[0] = arr.Length > 0 ? arr[0] : _defaultPickerValues[0];
                    _defaultPickerValues[1] = arr.Length > 1 ? arr[1] : _defaultPickerValues[1];
                });
            }
        }

        [Parameter]
        public RenderFragment SuffixIcon { get; set; }

        [Parameter]
        public Action<bool> OnOpenChange { get; set; }

        [Parameter]
        public Action<DateTime, string> OnPanelChange { get; set; }

        [Parameter]
        public Action<DateTime, string> OnChange { get; set; }

        [Parameter]
        public Func<DateTime, bool> DisabledDate { get; set; } = null;

        [Parameter]
        public Func<DateTime, DateTime, RenderFragment> DateRender { get; set; }

        private readonly DateTime[] _values = new DateTime[2];
        private OneOf<DateTime, DateTime[]> _value;

        [Parameter]
        public OneOf<DateTime, DateTime[]> Value
        {
            get => _value;
            set
            {
                _value = value;
                value.Switch(single =>
                {
                    _values[0] = single;
                }, arr =>
                {
                    _values[0] = arr.Length > 0 ? arr[0] : _values[0];
                    _values[1] = arr.Length > 1 ? arr[1] : _values[1];
                });
            }
        }

        public DateTime CurrentDate { get; private set; } = DateTime.Now;

        private readonly DateTime[] _pickerValues = new DateTime[] { DateTime.Now, DateTime.Now.AddMonths(1) };
        private OneOf<DateTime, DateTime[]> _pickerValue;

        [Parameter]
        public OneOf<DateTime, DateTime[]> PickerValue
        {
            get => _pickerValue;
            set
            {
                _pickerValue = value;
                value.Switch(single =>
                {
                    _pickerValues[0] = single;
                }, arr =>
                {
                    _pickerValues[0] = arr.Length > 0 ? arr[0] : _pickerValues[0];
                    _pickerValues[1] = arr.Length > 1 ? arr[1] : _pickerValues[1];
                });
            }
        }

        private bool _isRange = false;

        public bool IsRange
        {
            get => _isRange;
            set
            {
                _isRange = value;

                if (value == true)
                {
                    DisabledDate = (date) =>
                    {
                        if (_hadSelectValues[0] && _inputEnd.IsOnFocused)
                        {
                            return date.CompareTo(_values[0]) < 0;
                        }
                        if (_hadSelectValues[1] && _inputStart.IsOnFocused)
                        {
                            return date.CompareTo(_values[1]) > 0;
                        }

                        return false;
                    };
                }
            }
        }

        private AntDatePickerInput _inputStart;
        private AntDatePickerInput _inputEnd;

        private string _activeBarStyle = "";

        private string[] _initPickers = new string[2];
        private string[] _prePickers = new string[2];
        private bool[] _hadSelectValues = new bool[] { false, false };
        private bool _isClose = true;
        private bool _needRefresh;

        protected override void OnInitialized()
        {
            // set default picker type
            if (_isSetPicker == false)
            {
                Picker = AntDatePickerType.Date;
            }

            this.SetClass();

            base.OnInitialized();
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            _needRefresh = true;

            return base.SetParametersAsync(parameters);
        }

        protected override void OnParametersSet()
        {
            this.SetClass();

            base.OnParametersSet();
        }

        protected void SetClass()
        {
            this.ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-borderless", () => Bordered == false)
                .If($"{PrefixCls}-disabled", () => Disabled == true)
                .If($"{ClassName}", () => !string.IsNullOrEmpty(ClassName))
                .If($"{PrefixCls}-range", () => IsRange == true)
                .If($"{PrefixCls}-focused", () => AutoFocus == true)
               //.If($"{PrefixCls}-normal", () => Image.IsT1 && Image.AsT1 == AntEmpty.PRESENTED_IMAGE_SIMPLE)
               //.If($"{PrefixCls}-{Direction}", () => Direction.IsIn("ltr", "rlt"))
               ;
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);

            if (_needRefresh && IsRange)
            {
                if (_inputStart.IsOnFocused)
                {
                    Element element = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _inputStart.Ref)
                        .ConfigureAwait(false);
                    _activeBarStyle = $"width: {element.clientWidth - 10}px; position: absolute; transform: translate3d(0px, 0px, 0px);";
                }
                else if (_inputEnd.IsOnFocused)
                {
                    Element element = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _inputStart.Ref)
                        .ConfigureAwait(false);
                    _activeBarStyle = $"width: {element.clientWidth - 10}px; position: absolute; transform: translate3d({element.clientWidth + 16}px, 0px, 0px);";
                }
                else
                {
                    _activeBarStyle = "";
                }

                StateHasChanged();
            }

            _needRefresh = false;
        }

        protected string GetInputValue(int index = 0)
        {
            DateTime value;

            if (_hadSelectValues[index])
            {
                value = _values[index];
            }
            else if (_defaultValues[index] != null)
            {
                value = (DateTime)_defaultValues[index];
            }
            else
            {
                return "";
            }

            if (!string.IsNullOrEmpty(Format))
            {
                // TODO：Locale
                return value.ToString(Format);
            }

            // TODO：Locale
            string formatValue = _pickers[index] switch
            {
                AntDatePickerType.Date => value.ToString("yyyy-MM-dd"),
                AntDatePickerType.Week => $"{value.Year}-{DateHelper.GetWeekOfYear(value)}周",
                AntDatePickerType.Month => value.ToString("yyyy-MM"),
                AntDatePickerType.Quarter => $"{value.Year}-{DateHelper.GetDayOfQuarter(value)}",
                AntDatePickerType.Year => value.ToString("yyyy"),
                _ => value.ToString("yyyy-MM-dd"),
            };

            return formatValue;
        }

        protected void TryOpen()
        {
            if (!_isClose)
            {
                return;
            }

            _isClose = !_isClose;

            AutoFocus = !_isClose;

            _needRefresh = !_isClose;

            OnOpenChange?.Invoke(!_isClose);
        }

        protected void TryClose()
        {
            if (_isClose)
            {
                return;
            }

            AutoFocus = false;
            _isClose = true;
            _needRefresh = true;
        }

        protected async Task OnSelect(DateTime date)
        {
            int index = 0;

            // change focused picker
            if (IsRange && _inputEnd.IsOnFocused)
            {
                index = 1;
            }

            // InitPicker is the finally value
            if (_pickers[index] == _initPickers[index])
            {
                ChangeValue(date, index);

                OnChange?.Invoke(date, GetInputValue(index));

                // auto focus the other input
                if (IsRange)
                {
                    if (index == 0 && !_hadSelectValues[1] && !_inputEnd.IsOnFocused)
                    {
                        await Focus(1).ConfigureAwait(false);
                    }
                    if (index == 1 && !_hadSelectValues[0] && !_inputStart.IsOnFocused)
                    {
                        await Focus(0).ConfigureAwait(false);
                    }
                }
            }
            else
            {
                _pickers[index] = _prePickers[index];

                if (IsRange)
                {
                    int otherIndex = index == 0 ? 1 : 0;
                    _pickers[otherIndex] = _prePickers[otherIndex];
                }
            }

            ChangePickerValue(date, index);
        }

        private void ChangeValue(DateTime date, int index = 0)
        {
            _values[index] = date;

            _hadSelectValues[index] = true;

            if (IsRange)
            {
                if (_hadSelectValues[0] && _hadSelectValues[1])
                {
                    _isClose = true;
                }
            }
            else
            {
                _isClose = true;
            }
        }

        private void NoteInitPicker(string picker, int index = 0)
        {
            if (string.IsNullOrEmpty(_initPickers[index]))
            {
                // note first picker type
                _initPickers[index] = picker;

                // set default placeholder
                _placeholders[index] = AntDatePickerPlaceholder.GetPlaceholderByType(_initPickers[index]);
            }
        }

        public async Task Focus(int index = 0)
        {
            AntDatePickerInput input = null;

            if (index == 0)
            {
                input = _inputStart;
            }
            else if (index == 1 && IsRange)
            {
                input = _inputEnd;
            }

            if (input != null)
            {
                input.IsOnFocused = true;
                await JsInvokeAsync(JSInteropConstants.focus, input.Ref).ConfigureAwait(false);
            }
        }

        public async Task Blur(int index = 0)
        {
            AntDatePickerInput input = null;

            if (index == 0)
            {
                input = _inputStart;
            }
            else if (index == 1 && IsRange)
            {
                input = _inputEnd;
            }

            if (input != null)
            {
                input.IsOnFocused = false;
                await JsInvokeAsync(JSInteropConstants.blur, input.Ref).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Get picker by picker index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetIndexPicker(int index)
        {
            return _pickers[index];
        }

        /// <summary>
        /// Get pickerValue by picker index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DateTime GetIndexPickerValue(int index)
        {
            return _pickerValues[index];
        }

        /// <summary>
        /// Get value by picker index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DateTime GetIndexValue(int index)
        {
            return _values[index];
        }

        public void ChangePickerValue(DateTime date, int index = 0)
        {
            TimeSpan interval = date - _pickerValues[index];

            _pickerValues[index] = date;

            if (IsRange)
            {
                if (index == 0)
                {
                    _pickerValues[1] = _pickerValues[1].Add(interval);
                }
                else
                {
                    _pickerValues[0] = _pickerValues[0].Add(interval);
                }
            }

            OnPanelChange?.Invoke(_pickerValues[index], _pickers[index]);

            StateHasChanged();
        }

        public void ChangePickerType(string type, int index = 0)
        {
            _prePickers[index] = _pickers[index];
            _pickers[index] = type;

            if (IsRange)
            {
                int otherIndex = index == 0 ? 1 : 0;

                _prePickers[otherIndex] = _pickers[otherIndex];
                _pickers[otherIndex] = type;
            }

            OnPanelChange?.Invoke(_pickerValues[index], _pickers[index]);

            StateHasChanged();
        }
    }
}
