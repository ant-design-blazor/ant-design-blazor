﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.core.Extensions;
using AntDesign.Datepicker.Locale;
using AntDesign.Internal;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OneOf;

namespace AntDesign
{
    public class DatePickerBase<TValue> : AntInputComponentBase<TValue>, IDatePicker
    {
        DateTime? IDatePicker.HoverDateTime { get; set; }

        public const int START_PICKER_INDEX = 0;
        public const int END_PICKER_INDEX = 0;

        [Parameter]
        public string PrefixCls { get; set; } = "ant-picker";

        protected string _picker;
        protected bool _isSetPicker = false;
        private bool _isNullableEvaluated;
        private bool _isNullable;

        /// <summary>
        /// Stores information if TValue is a nullable type.
        /// </summary>
        protected bool IsNullable
        {
            get
            {
                if (!_isNullableEvaluated)
                {
                    Type type = typeof(TValue);
                    if (type.IsAssignableFrom(typeof(DateTime?)) || type.IsAssignableFrom(typeof(DateTime?[])))
                    {
                        _isNullable = true;
                    }
                    else
                    {
                        _isNullable = false;
                    }
                    _isNullableEvaluated = true;
                }
                return _isNullable;
            }
        }

        [Parameter]
        public string Picker
        {
            get => _picker;
            set
            {
                _isSetPicker = true;
                _picker = value;
                InitPicker(value);
            }
        }

        [Parameter]
        public string PopupContainerSelector { get; set; }

        [Parameter]
        public OneOf<bool, bool[]> Disabled { get; set; } = new bool[] { false, false };

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

        [Parameter]
        public DatePickerLocale Locale
        {
            get { return _locale; }
            set
            {
                _locale = value;
                _isLocaleSetOutside = true;
            }
        }

        [Parameter]
        public override CultureInfo CultureInfo
        {
            get
            {
                return base.CultureInfo;
            }
            set
            {
                if (!_isLocaleSetOutside && 
                    (
                    (base.CultureInfo != value && base.CultureInfo.Name != value.Name)
                    ||
                    LocaleProvider.CurrentLocale.LocaleName != value.Name
                    ))
                {
                    _locale = LocaleProvider.GetLocale(value.Name).DatePicker;
                }
                base.CultureInfo = value;
            }
        }

        public bool IsShowTime { get; protected set; }
        public string ShowTimeFormat { get; protected set; } = "HH:mm:ss";
        protected OneOf<bool, string> _showTime = null;

        private bool _timeFormatProvided;
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
                    _timeFormatProvided = true;
                    ShowTimeFormat = strValue;
                });
            }
        }

        [Parameter]
        public bool AllowClear { get; set; } = true;

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
        public string Format { get; set; }

        private TValue _defaultValue;

        [Parameter]
        public TValue DefaultValue
        {
            get => _defaultValue;
            set => _defaultValue = SortValue(value);
        }

        protected bool[] UseDefaultPickerValue { get; } = new bool[2];
        private TValue _defaultPickerValue;

        [Parameter]
        public TValue DefaultPickerValue
        {
            get => _defaultPickerValue;
            set => _defaultPickerValue = SortValue(value);
        }

        [Parameter]
        public RenderFragment SuffixIcon { get; set; }

        [Parameter]
        public RenderFragment RenderExtraFooter { get; set; }

        /// <summary>
        /// Called when  clear button clicked.
        /// </summary>
        [Parameter]
        public EventCallback OnClearClick { get; set; }

        [Parameter]
        public EventCallback<bool> OnOpenChange { get; set; }

        [Parameter]
        public EventCallback<DateTimeChangedEventArgs> OnPanelChange { get; set; }

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

        public DateTime CurrentDate { get; set; } = DateTime.Now;

        protected DateTime[] PickerValues { get; } = new DateTime[] { DateTime.Now, DateTime.Now };

        public bool IsRange { get; set; }

        protected DatePickerInput _inputStart;
        protected DatePickerInput _inputEnd;
        protected OverlayTrigger _dropDown;
        protected bool _duringFocus;

        protected string _activeBarStyle = "";
        protected string _rangeArrowStyle = "";

        protected DatePickerStatus[] _pickerStatus
            = new DatePickerStatus[] { new DatePickerStatus(), new DatePickerStatus() };

        protected Stack<string> _prePickerStack = new Stack<string>();
        protected bool _isClose = true;
        protected bool _needRefresh;
        protected bool _duringManualInput;
        private bool _isLocaleSetOutside;
        private DatePickerLocale _locale = LocaleProvider.CurrentLocale.DatePicker;
        protected bool _openingOverlay;

        protected ClassMapper _panelClassMapper = new ClassMapper();

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

        protected bool _shouldRender = true;
        protected override bool ShouldRender()
        {
            if (!_shouldRender)
            {
                _shouldRender = true;
                return false;
            }
            return base.ShouldRender();
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            _needRefresh = true;

            return base.SetParametersAsync(parameters);
        }

        protected void SetClass()
        {
            this.ClassMapper.Clear()
                .Add(PrefixCls)
                .Get(() => $"{PrefixCls}-{Size}")
                .If($"{PrefixCls}-rtl", () => RTL)
                .If($"{PrefixCls}-borderless", () => Bordered == false)
                .If($"{PrefixCls}-disabled", () => IsDisabled() == true)
                .If($"{ClassName}", () => !string.IsNullOrEmpty(ClassName))
                .If($"{PrefixCls}-range", () => IsRange == true)
                .If($"{PrefixCls}-focused", () => AutoFocus == true)
               //.If($"{PrefixCls}-normal", () => Image.IsT1 && Image.AsT1 == Empty.PRESENTED_IMAGE_SIMPLE)
               //.If($"{PrefixCls}-{Direction}", () => Direction.IsIn("ltr", "rlt"))
               ;

            _panelClassMapper
                .Add($"{PrefixCls}-panel")
                .If($"{PrefixCls}-panel-rtl", () => RTL);
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                await Js.InvokeVoidAsync(JSInteropConstants.AddPreventKeys, _inputStart.Ref, new[] { "ArrowUp", "ArrowDown" });
            }

            if (_needRefresh && IsRange)
            {
                if (_inputStart.IsOnFocused)
                {
                    HtmlElement element = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _inputStart.Ref);
                    _activeBarStyle = $"width: {element.ClientWidth - 10}px; position: absolute; transform: translate3d(0px, 0px, 0px);";
                    _rangeArrowStyle = $"left: 12px";
                }
                else if (_inputEnd.IsOnFocused)
                {
                    HtmlElement element = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _inputEnd.Ref);
                    int translateDistance = element.ClientWidth + 16;

                    if (RTL)
                    {
                        translateDistance = -translateDistance;
                    }

                    _activeBarStyle = $"width: {element.ClientWidth - 10}px; position: absolute; transform: translate3d({translateDistance}px, 0px, 0px);";
                    _rangeArrowStyle = $"left: {element.ClientWidth + 30}px";
                }
                else
                {
                    _activeBarStyle = "display: none";
                }

                StateHasChanged();
            }

            _needRefresh = false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _ = InvokeAsync(async () =>
            {
                await Js.InvokeVoidAsync(JSInteropConstants.RemovePreventKeys, _inputStart.Ref);
            });
        }

        protected string GetInputValue(int index = 0)
        {
            DateTime? tryGetValue = GetIndexValue(index);
            if (tryGetValue == null)
            {
                return "";
            }

            DateTime value = (DateTime)tryGetValue;

            return GetFormatValue(value, index);
        }

        protected void ChangeFocusTarget(bool inputStartFocus, bool inputEndFocus)
        {
            if (!IsRange)
            {
                return;
            }
            _duringManualInput = false;
            _needRefresh = true;
            _inputStart.IsOnFocused = inputStartFocus;
            _inputEnd.IsOnFocused = inputEndFocus;
        }

        protected virtual Task PickerClicked()
        {
            AutoFocus = true;
            return Task.CompletedTask;
        }

        protected virtual async Task OnSelect(DateTime date)
        {
            int index = GetOnFocusPickerIndex();
            _duringManualInput = false;

            // InitPicker is the finally value
            if (_picker == _pickerStatus[index]._initPicker)
            {
                ChangeValue(date, index);

                // auto focus the other input
                if (IsRange && (!IsShowTime || Picker == DatePickerType.Time))
                {
                    if (index == 0 && !_pickerStatus[1]._currentShowHadSelectValue && !_inputEnd.IsOnFocused && !IsDisabled(1))
                    {
                        await Blur(0);
                        await Focus(1);
                    }
                    else if (index == 1 && !_pickerStatus[0]._currentShowHadSelectValue && !_inputStart.IsOnFocused && !IsDisabled(0))
                    {
                        await Blur(1);
                        await Focus(0);
                    }
                    else
                    {
                        await Focus(index); //keep focus on current input
                    }
                }
                else
                {
                    await Focus(index);
                }
            }
            else
            {
                _picker = _prePickerStack.Pop();
            }

            ChangePickerValue(date, index);
        }

        protected virtual async Task OnBlur(int index)
        {
        }

        protected void InitPicker(string picker)
        {
            if (string.IsNullOrEmpty(_pickerStatus[0]._initPicker))
            {
                _pickerStatus[0]._initPicker = picker;
            }
            if (string.IsNullOrEmpty(_pickerStatus[1]._initPicker))
            {
                _pickerStatus[1]._initPicker = picker;
            }
            if (IsRange)
            {
                (string first, string second) = DatePickerPlaceholder.GetRangePlaceHolderByType(picker, Locale);
                _placeholders[0] = first;
                _placeholders[1] = second;

                DateTime now = DateTime.Now;
                PickerValues[1] = picker switch
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
            else
            {
                string first = DatePickerPlaceholder.GetPlaceholderByType(picker, Locale);
                _placeholders[0] = first;
                _placeholders[1] = first;
            }
        }

        protected bool IsDisabled(int? index = null)
        {
            bool disabled = false;

            Disabled.Switch(single =>
            {
                disabled = single;
            }, arr =>
            {
                if (index == null || index > 1 || index < 0)
                {
                    disabled = arr[0] && arr[1];
                }
                else
                {
                    disabled = arr[(int)index];
                }
            });

            return disabled;
        }

        public void Close()
        {
            _duringManualInput = false;
            _dropDown?.Hide();
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
                await FocusAsync(input.Ref);
                _needRefresh = true;
            }
        }

        public async Task Blur(int index = 0)
        {
            DatePickerInput input = null;
            _duringManualInput = false;
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
                await JsInvokeAsync(JSInteropConstants.Blur, input.Ref);
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
        /// Get pickerValue by picker index. Note that index refers to a picker panel
        /// and not to input text. For RangePicker 2 inputs generate 2 panels.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DateTime GetIndexPickerValue(int index)
        {
            int tempIndex = GetOnFocusPickerIndex();
            if (index == 0)
            {
                return PickerValues[tempIndex];
            }
            else
            {
                //First picker panel will show the value, second panel shows next
                //expected value that depends on Picker type
                return Picker switch
                {
                    DatePickerType.Date => PickerValues[tempIndex].AddMonths(1),
                    DatePickerType.Week => PickerValues[tempIndex].AddMonths(1),
                    DatePickerType.Month => PickerValues[tempIndex].AddYears(1),
                    DatePickerType.Decade => PickerValues[tempIndex].AddYears(1),
                    DatePickerType.Quarter => PickerValues[tempIndex].AddYears(1),
                    DatePickerType.Year => PickerValues[tempIndex].AddYears(10),
                    _ => DateTime.Now,
                };
            }
        }

        public void ChangePlaceholder(string placeholder, int index = 0)
        {
            _placeholders[index] = placeholder;

            StateHasChanged();
        }

        private int _htmlInputSize;
        protected int HtmlInputSize
        {
            get
            {
                if (_htmlInputSize == 0)
                {
                    _htmlInputSize = InternalFormat.Length + (int)(InternalFormat.Count(ch => ch > 127) * 1.34) + 2;
                    if (_htmlInputSize < 12)
                    {
                        _htmlInputSize = 12;
                    }
                }
                return _htmlInputSize;
            }
        }

        private string _internalFormat;
        private string InternalFormat
        {
            get
            {
                if (string.IsNullOrEmpty(_internalFormat))
                {
                    if (!string.IsNullOrEmpty(Format))
                        _internalFormat = Format;
                    else
                        _internalFormat = _pickerStatus[0]._initPicker switch
                        {
                            DatePickerType.Date => GetTimeFormat(),
                            DatePickerType.Month => Locale.Lang.YearMonthFormat,
                            DatePickerType.Year => Locale.Lang.YearFormat,
                            DatePickerType.Time => Locale.Lang.TimeFormat,
                            DatePickerType.Week => $"{Locale.Lang.YearFormat}-0{Locale.Lang.Week}",
                            DatePickerType.Quarter => $"{Locale.Lang.YearFormat}-Q0",
                            _ => Locale.Lang.DateFormat,
                        };

                }
                return _internalFormat;
            }
        }
        private string GetTimeFormat()
        {
            if (IsShowTime)
            {
                if (_timeFormatProvided)
                {
                    return $"{Locale.Lang.DateFormat} {ShowTimeFormat}";
                }
                return Locale.Lang.DateTimeFormat;
            }
            return Locale.Lang.DateFormat;
        }

        private FormatAnalyzer _formatAnalyzer;
        public FormatAnalyzer FormatAnalyzer => _formatAnalyzer ??= new(InternalFormat, Picker, Locale);

        public string GetFormatValue(DateTime value, int index)
        {
            string format;
            if (string.IsNullOrEmpty(Format))
                format = _pickerStatus[index]._initPicker switch
                {
                    DatePickerType.Week => $"{Locale.Lang.YearFormat}-{DateHelper.GetWeekOfYear(value, Locale.FirstDayOfWeek)}{Locale.Lang.Week}",
                    DatePickerType.Quarter => $"{Locale.Lang.YearFormat}-{DateHelper.GetDayOfQuarter(value)}",
                    _ => InternalFormat,
                };
            else
                format = InternalFormat;
            return value.ToString(format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Changes what date(s) will be visible on the picker.
        /// </summary>
        /// <param name="date">New date to be saved.</param>
        /// <param name="index">Index of the input box, where 0 = inputStart and 1 = inputEnd (only RangePicker)</param>
        internal void ChangePickerValue(DateTime date, int? index = null)
        {
            if (index == null)
                index = GetOnFocusPickerIndex();

            PickerValues[index.Value] = date;
            if (IsRange)
            {
                if (!UseDefaultPickerValue[1] && !_pickerStatus[1]._hadSelectValue && index == 0)
                {
                    PickerValues[1] = date;
                }
                else if (!UseDefaultPickerValue[0] && !_pickerStatus[0]._hadSelectValue && index == 1)
                {
                    PickerValues[0] = date;
                }
            }

            if (OnPanelChange.HasDelegate)
            {
                OnPanelChange.InvokeAsync(new DateTimeChangedEventArgs
                {
                    Date = PickerValues[index.Value],
                    DateString = _picker
                });
            }

            StateHasChanged();
        }

        public void ChangePickerType(string type)
        {
            ChangePickerType(type, 0);
        }

        public virtual void ChangePickerType(string type, int index)
        {
            _prePickerStack.Push(_picker);
            _picker = type;

            if (OnPanelChange.HasDelegate)
            {
                OnPanelChange.InvokeAsync(new DateTimeChangedEventArgs
                {
                    Date = PickerValues[index],
                    DateString = _picker
                });
            }

            StateHasChanged();
        }

        /// <summary>
        /// 修改值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public virtual void ChangeValue(DateTime value, int index = 0)
        {
        }

        public virtual void ClearValue(int index = 0, bool closeDropdown = true)
        {
        }

        public virtual DateTime? GetIndexValue(int index)
        {
            return null;
        }

        public void InvokeStateHasChanged()
        {
            StateHasChanged();
        }

        protected TValue SortValue(TValue value)
        {
            if (value == null)
            {
                return value;
            }
            TValue orderedValue = value;
            if (IsRange)
            {
                if (IsNullable)
                {
                    var tempValue = value as DateTime?[];
                    if (tempValue[0] == null || tempValue[1] == null)
                        return orderedValue;

                    if ((tempValue[0] ?? DateTime.Now).CompareTo((tempValue[1] ?? DateTime.Now)) > 0)
                        orderedValue = DataConvertionExtensions.Convert<DateTime?[], TValue>(new DateTime?[] { tempValue[1], tempValue[0] });
                }
                else
                {
                    var tempValue = value as DateTime[];
                    if (tempValue[0].CompareTo(tempValue[1]) > 0)
                        orderedValue = DataConvertionExtensions.Convert<DateTime[], TValue>(new DateTime[] { tempValue[1], tempValue[0] });
                }
            }
            return orderedValue;
        }
    }
}
