using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AntDesign.Internal;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public class DatePickerBase<TValue> : AntInputComponentBase<TValue>
    {
        [Parameter]
        public string PrefixCls { get; set; } = "ant-picker";

        protected string _picker;
        protected bool _isSetPicker = false;

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
        public string PopupContainerSelector { get; set; }

        [Parameter]
        public bool Disabled { get; set; } = false;

        [Parameter]
        public bool Bordered { get; set; } = true;

        [Parameter]
        public bool AutoFocus { get; set; } = false;

        [Parameter]
        public bool Open { get; set; }

        [Parameter]
        public bool InputReadOnly { get; set; } = false;

        [Parameter]
        public bool ShowToday { get; set; } = true;

        public bool IsShowTime { get; protected set; } = false;
        public string ShowTimeFormat { get; protected set; } = "HH:mm:ss";
        protected OneOf<bool, string> _showTime = null;

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

        protected string[] _placeholders = new string[] { "", "" };
        protected OneOf<string, string[]> _placeholder;

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
        public string Size { get; set; } = DatePickerSize.Default;

        [Parameter]
        public string Format { get; set; }

        protected readonly DateTime?[] _defaultValues = new DateTime?[2];
        protected OneOf<DateTime, DateTime[]> _defaultValue;

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
        public Func<DateTime, DatePickerDisabledTime> DisabledTime { get; set; } = null;

        [Parameter]
        public Func<DateTime, DateTime, RenderFragment> DateRender { get; set; }

        // TODO: need locale
        [Parameter]
        public Func<DateTime, RenderFragment> MonthCellRender { get; set; }

        //private readonly DateTime[] _values = new DateTime[2];
        //private OneOf<DateTime, DateTime[]> _value;

        //[Parameter]
        //public OneOf<DateTime, DateTime[]> Value
        //{
        //    get => _value;
        //    set
        //    {
        //        _value = value;
        //        value.Switch(single =>
        //        {
        //            _values[0] = single;
        //        }, arr =>
        //        {
        //            _values[0] = arr.Length > 0 ? arr[0] : _values[0];
        //            _values[1] = arr.Length > 1 ? arr[1] : _values[1];
        //        });
        //    }
        //}

        public DateTime CurrentDate { get; protected set; } = DateTime.Now;

        protected readonly DateTime[] _pickerValues = new DateTime[] { DateTime.Now, DateTime.Now };
        protected OneOf<DateTime, DateTime[]> _pickerValue;

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

        public bool IsRange { get; set; }

        protected DatePickerInput _inputStart;
        protected DatePickerInput _inputEnd;
        protected OverlayTrigger _dropDown;

        protected string _activeBarStyle = "";

        protected DatePickerStatus[] _pickerStatus
            = new DatePickerStatus[] { new DatePickerStatus(), new DatePickerStatus() };

        protected Stack<string> _prePickerStack = new Stack<string>();
        protected bool _isClose = true;
        protected bool _needRefresh;

        protected override void OnInitialized()
        {
            // set default picker type
            if (_isSetPicker == false)
            {
                Picker = DatePickerType.Date;
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
                .Add($"{PrefixCls}-{Size}")
                .If($"{PrefixCls}-borderless", () => Bordered == false)
                .If($"{PrefixCls}-disabled", () => Disabled == true)
                .If($"{ClassName}", () => !string.IsNullOrEmpty(ClassName))
                .If($"{PrefixCls}-range", () => IsRange == true)
                .If($"{PrefixCls}-focused", () => AutoFocus == true)
               //.If($"{PrefixCls}-normal", () => Image.IsT1 && Image.AsT1 == Empty.PRESENTED_IMAGE_SIMPLE)
               //.If($"{PrefixCls}-{Direction}", () => Direction.IsIn("ltr", "rlt"))
               ;
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (_needRefresh && IsRange)
            {
                if (_inputStart.IsOnFocused)
                {
                    Element element = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _inputStart.Ref);
                    _activeBarStyle = $"width: {element.clientWidth - 10}px; position: absolute; transform: translate3d(0px, 0px, 0px);";
                }
                else if (_inputEnd.IsOnFocused)
                {
                    Element element = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _inputStart.Ref);
                    _activeBarStyle = $"width: {element.clientWidth - 10}px; position: absolute; transform: translate3d({element.clientWidth + 16}px, 0px, 0px);";
                }
                else
                {
                    _activeBarStyle = "display: none";
                }

                StateHasChanged();
            }

            _needRefresh = false;
        }

        protected string GetInputValue(int index = 0)
        {
            DateTime? tryGetValue = GetIndexValue(index);

            if (tryGetValue == null)
            {
                return "";
            }

            DateTime value = (DateTime)tryGetValue;

            if (!string.IsNullOrEmpty(Format))
            {
                // TODO：Locale
                return value.ToString(Format, CultureInfo.CurrentCulture);
            }

            // TODO：Locale
            string formater = _pickerStatus[index]._initPicker switch
            {
                DatePickerType.Date => IsShowTime ? $"yyyy-MM-dd {ShowTimeFormat}" : "yyyy-MM-dd",
                DatePickerType.Week => $"{value.Year}-{DateHelper.GetWeekOfYear(value)}周",
                DatePickerType.Month => "yyyy-MM",
                DatePickerType.Quarter => $"{value.Year}-{DateHelper.GetDayOfQuarter(value)}",
                DatePickerType.Year => "yyyy",
                DatePickerType.Time => "HH:mm:dd",
                _ => "yyyy-MM-dd",
            };

            return value.ToString(formater, CultureInfo.CurrentCulture);
        }

        protected void ChangeFocusTarget(bool inputStartFocus, bool inputEndFocus)
        {
            if (!IsRange)
            {
                return;
            }

            _needRefresh = true;
            _inputStart.IsOnFocused = inputStartFocus;
            _inputEnd.IsOnFocused = inputEndFocus;
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
                if (IsRange && (!IsShowTime || Picker == DatePickerType.Time))
                {
                    if (index == 0 && !_pickerStatus[1]._hadSelectValue && !_inputEnd.IsOnFocused)
                    {
                        await Blur(0);
                        await Focus(1);
                    }
                    if (index == 1 && !_pickerStatus[0]._hadSelectValue && !_inputStart.IsOnFocused)
                    {
                        await Blur(1);
                        await Focus(0);
                    }
                }
            }
            else
            {
                _picker = _prePickerStack.Pop();
            }

            ChangePickerValue(date, index);
        }

        protected void InitPicker(string picker, int index = 0)
        {
            if (string.IsNullOrEmpty(_pickerStatus[index]._initPicker))
            {
                // note first picker type
                _pickerStatus[index]._initPicker = picker;

                // set default placeholder
                _placeholders[index] = DatePickerPlaceholder.GetPlaceholderByType(_pickerStatus[index]._initPicker);

                if (IsRange && index != 0)
                {
                    DateTime now = DateTime.Now;
                    _pickerValues[index] = picker switch
                    {
                        DatePickerType.Date => now.AddMonths(1),
                        DatePickerType.Week => now.AddMonths(1),
                        DatePickerType.Month => now.AddYears(1),
                        DatePickerType.Decade => now.AddYears(1),
                        DatePickerType.Quarter => now.AddYears(1),
                        DatePickerType.Year => now.AddYears(10),
                        _ => now,
                    };
                }
            }
        }

        protected void UpdateCurrentValueAsString(int index = 0)
        {
            if (EditContext != null)
            {
                CurrentValueAsString = GetInputValue(index);
            }
        }

        public void Close()
        {
            _dropDown.Hide();
        }

        public async Task Focus(int index = 0)
        {
            DatePickerInput input = null;

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
                await JsInvokeAsync(JSInteropConstants.focus, input.Ref);
                _needRefresh = true;
            }
        }

        public async Task Blur(int index = 0)
        {
            DatePickerInput input = null;

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
                await JsInvokeAsync(JSInteropConstants.blur, input.Ref);
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

        public virtual void ChangeValue(DateTime value, int index = 0)
        {
        }

        public virtual DateTime? GetIndexValue(int index)
        {
            return null;
        }
    }
}
