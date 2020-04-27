using System;
using System.Globalization;
using System.Threading.Tasks;
using AntBlazor.Internal;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using OneOf;

namespace AntBlazor
{
    public partial class AntDatePicker : AntDomComponentBase
    {
        [Parameter]
        public string PrefixCls { get; set; } = "ant-picker";

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

                InitPicker(value, 0);
                InitPicker(value, 1);
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
        public bool ShowToday { get; set; } = true;

        public bool IsShowTime { get; private set; } = false;
        public string ShowTimeFormat { get; private set; } = "HH:mm:ss";
        private OneOf<bool, string> _showTime = null;

        [Parameter]
        public OneOf<bool, string> ShowTime
        {
            get => _showTime;
            set
            {
                _showTime = value;

                value.Switch(booleanValue =>
                {
                    IsShowTime = booleanValue;
                }, strValue =>
                {
                    IsShowTime = true;
                    ShowTimeFormat = strValue;
                });
            }
        }

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
        public string Size { get; set; } = AntDatePickerSize.Default;

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
        public RenderFragment RenderExtraFooter { get; set; }

        [Parameter]
        public EventCallback<bool> OnOpenChange { get; set; }

        [Parameter]
        public Action<DateTime, string> OnPanelChange { get; set; }

        [Parameter]
        public Action<DateTime, string> OnChange { get; set; }

        [Parameter]
        public Func<DateTime, bool> DisabledDate { get; set; } = null;

        [Parameter]
        public Func<DateTime, int[]> DisabledHours { get; set; } = null;

        [Parameter]
        public Func<DateTime, int[]> DisabledMinutes { get; set; } = null;

        [Parameter]
        public Func<DateTime, int[]> DisabledSeconds { get; set; } = null;

        [Parameter]
        public Func<DateTime, AntDatePickerDisabledTime> DisabledTime { get; set; } = null;

        [Parameter]
        public Func<DateTime, DateTime, RenderFragment> DateRender { get; set; }

        // TODO: need locale
        [Parameter]
        public Func<DateTime, RenderFragment> MonthCellRender { get; set; }

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

        private readonly DateTime[] _pickerValues = new DateTime[] { DateTime.Now, DateTime.Now };
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
                        if (_pickerStatus[0]._hadSelectValue && _inputEnd.IsOnFocused)
                        {
                            return date.CompareTo(_values[0]) < 0;
                        }
                        if (_pickerStatus[1]._hadSelectValue && _inputStart.IsOnFocused)
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

        private AntDatePickerStatus[] _pickerStatus
            = new AntDatePickerStatus[] { new AntDatePickerStatus(), new AntDatePickerStatus() };

        private Stack<string> _prePickerStack = new Stack<string>();
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

            if (_pickerStatus[index]._hadSelectValue)
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
                return value.ToString(Format, CultureInfo.CurrentCulture);
            }

            // TODO：Locale
            string formater = _pickerStatus[index]._initPicker switch
            {
                AntDatePickerType.Date => IsShowTime ? $"yyyy-MM-dd {ShowTimeFormat}" : "yyyy-MM-dd",
                AntDatePickerType.Week => $"{value.Year}-{DateHelper.GetWeekOfYear(value)}周",
                AntDatePickerType.Month => "yyyy-MM",
                AntDatePickerType.Quarter => $"{value.Year}-{DateHelper.GetDayOfQuarter(value)}",
                AntDatePickerType.Year => "yyyy",
                AntDatePickerType.Time => "HH:mm:dd",
                _ => "yyyy-MM-dd",
            };

            return value.ToString(formater, CultureInfo.CurrentCulture);
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

            OnOpenChange.InvokeAsync(!_isClose);

            StateHasChanged();
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

            StateHasChanged();
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
            if (_picker == _pickerStatus[index]._initPicker)
            {
                ChangeValue(date, index);

                OnChange?.Invoke(date, GetInputValue(index));

                // auto focus the other input
                if (IsRange && (!IsShowTime || Picker == AntDatePickerType.Time))
                {
                    if (index == 0 && !_pickerStatus[1]._hadSelectValue && !_inputEnd.IsOnFocused)
                    {
                        await Blur(0).ConfigureAwait(false);
                        await Focus(1).ConfigureAwait(false);
                    }
                    if (index == 1 && !_pickerStatus[0]._hadSelectValue && !_inputStart.IsOnFocused)
                    {
                        await Blur(1).ConfigureAwait(false);
                        await Focus(0).ConfigureAwait(false);
                    }
                }
            }
            else
            {
                _picker = _prePickerStack.Pop();
            }

            ChangePickerValue(date, index);
        }

        protected void OnInput(ChangeEventArgs args, int index = 0)
        {
            if (args == null)
            {
                return;
            }

            if (DateTime.TryParse(args.Value.ToString(), out DateTime changeValue))
            {
                _values[index] = changeValue;
                _pickerValues[index] = changeValue;

                StateHasChanged();
            }
        }
   
        private void InitPicker(string picker, int index = 0)
        {
            if (string.IsNullOrEmpty(_pickerStatus[index]._initPicker))
            {
                // note first picker type
                _pickerStatus[index]._initPicker = picker;

                // set default placeholder
                _placeholders[index] = AntDatePickerPlaceholder.GetPlaceholderByType(_pickerStatus[index]._initPicker);

                if (IsRange && index != 0)
                {
                    DateTime now = DateTime.Now;
                    _pickerValues[index] = picker switch
                    {
                        AntDatePickerType.Date => now.AddMonths(1),
                        AntDatePickerType.Week => now.AddMonths(1),
                        AntDatePickerType.Month => now.AddYears(1),
                        AntDatePickerType.Decade => now.AddYears(1),
                        AntDatePickerType.Quarter => now.AddYears(1),
                        AntDatePickerType.Year => now.AddYears(10),
                        _ => now,
                    };
                }
            }
        }

        public void Close()
        {
            TryClose();
        }

        public void ChangeValue(DateTime date, int index = 0)
        {
            _values[index] = date;

            _pickerStatus[index]._hadSelectValue = true;

            if (IsRange && !IsShowTime && Picker != AntDatePickerType.Time)
            {
                if (_pickerStatus[0]._hadSelectValue && _pickerStatus[1]._hadSelectValue)
                {
                    Close();
                }
            }
            else if (!IsShowTime && Picker != AntDatePickerType.Time)
            {
                Close();
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
                _needRefresh = true;
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
                _needRefresh = true;
            }
        }

        public int GetOnFocusPickerIndex()
        {
            if (_inputStart != null && _inputStart.IsOnFocused)
            {
                return 0;
            }

            if (_inputEnd != null && _inputEnd.IsOnFocused)
            {
                return 1;
            }

            return 0;
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

            OnPanelChange?.Invoke(_pickerValues[index], _picker);

            StateHasChanged();
        }

        public void ChangePickerType(string type, int index = 0)
        {
            _prePickerStack.Push(_picker);
            _picker = type;

            OnPanelChange?.Invoke(_pickerValues[index], _picker);

            StateHasChanged();
        }
    }
}
