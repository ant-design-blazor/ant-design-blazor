using System;
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
        private TValue _swpValue;

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

        /// <summary>
        /// Overlay adjustment strategy (when for example browser resize is happening)
        /// </summary>
        [Parameter]
        public TriggerBoundaryAdjustMode BoundaryAdjustMode { get; set; } = TriggerBoundaryAdjustMode.InView;

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
                    _locale = LocaleProvider.GetLocale(value).DatePicker;
                }
                base.CultureInfo = value;
            }
        }

        public bool IsShowTime { get; protected set; }
        public string ShowTimeFormat { get; protected set; }
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
        public Dictionary<string, DateTime?[]> Ranges { get; set; } = new Dictionary<string, DateTime?[]>();

        [Parameter]
        public RenderFragment RenderExtraFooter { get; set; }

        /// <summary>
        /// Called when  clear button clicked.
        /// </summary>
        [Parameter]
        public EventCallback OnClearClick { get; set; }

        [Parameter]
        public EventCallback OnOk { get; set; }

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

        [Parameter]
        public bool Use12Hours { get; set; }

        public DateTime CurrentDate { get; set; } = DateTime.Today;

        protected DateTime[] PickerValues { get; } = new DateTime[] { DateTime.Today, DateTime.Today };

        public bool IsRange { get; set; }

        protected DatePickerInput _inputStart;
        protected DatePickerInput _inputEnd;
        protected OverlayTrigger _dropDown;
        protected bool _duringFocus;

        protected string _activeBarStyle = "";
        protected string _rangeArrowStyle = "";

        internal DatePickerStatus[] _pickerStatus
            = new DatePickerStatus[] { new DatePickerStatus(), new DatePickerStatus() };

        protected Stack<string> _prePickerStack = new Stack<string>();
        protected bool _isClose = true;
        protected bool _needRefresh;
        protected bool _duringManualInput;
        private bool _isLocaleSetOutside;
        private DatePickerLocale _locale = LocaleProvider.CurrentLocale.DatePicker;
        protected bool _openingOverlay;

        protected ClassMapper _panelClassMapper = new ClassMapper();

        private static readonly int[] _hours = Enumerable.Range(0, 24).ToArray();
        private static readonly int[] _minutesSeconds = Enumerable.Range(0, 60).ToArray();

        internal event EventHandler<bool> OverlayVisibleChanged;

        private readonly object _eventLock = new();

        event EventHandler<bool> IDatePicker.OverlayVisibleChanged
        {
            add
            {
                lock (_eventLock)
                {
                    OverlayVisibleChanged += value;
                }
            }

            remove
            {
                lock (_eventLock)
                {
                    OverlayVisibleChanged -= value;
                }
            }
        }

        protected override void OnInitialized()
        {
            // set default picker type
            if (_isSetPicker == false)
            {
                Picker = DatePickerType.Date;
            }

            if (_placeholder.Value is null)
            {
                _placeholder = IsRange ? new string[] { } : string.Empty;
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

            if (!_timeFormatProvided || string.IsNullOrEmpty(ShowTimeFormat))
            {
                ShowTimeFormat = Use12Hours ? Locale.Lang.TimeFormat12Hour : Locale.Lang.TimeFormat;
            }

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
                .If($"{PrefixCls}-status-error", () => ValidationMessages.Length > 0)
               //.If($"{PrefixCls}-normal", () => Image.IsT1 && Image.AsT1 == Empty.PRESENTED_IMAGE_SIMPLE)
               //.If($"{PrefixCls}-{Direction}", () => Direction.IsIn("ltr", "rlt"))
               ;

            _panelClassMapper
                .Add($"{PrefixCls}-panel")
                .If($"{PrefixCls}-panel-rtl", () => RTL);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
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

            SetDisabledTime();
        }

        protected virtual Task PickerClicked()
        {
            AutoFocus = true;
            return Task.CompletedTask;
        }

        protected virtual async Task OnSelect(DateTime date, int index)
        {
            _duringManualInput = false;

            // InitPicker is the finally value
            if (_picker == _pickerStatus[index].InitPicker)
            {
                ChangeValue(date, index);

                // auto focus the other input
                if (IsRange && (!IsShowTime || Picker == DatePickerType.Time))
                {
                    await SwitchFocus(index);
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

            if (!IsRange || IsShowTime)
            {
                ChangePickerValue(date, index);
            }

            SetDisabledTime();
        }

        public async Task OnOkClick()
        {
            var index = GetOnFocusPickerIndex();

            _pickerStatus[index].IsNewValueSelected = true;

            if (IsRange)
            {
                var otherIndex = Math.Abs(index - 1);

                if (!_pickerStatus[otherIndex].IsNewValueSelected)
                {
                    var otherValue = GetIndexValue(otherIndex);
                    _pickerStatus[otherIndex].IsNewValueSelected = otherValue is not null
                                                                    && otherValue != _pickerStatus[otherIndex].OldValue;
                }
            }
            else
            {
                Close();
                return;
            }

            if (!(await SwitchFocus(index)))
            {
                Close();
            }

            await OnOk.InvokeAsync(null);
        }

        internal void OnRangeItemOver(DateTime?[] range)
        {
            _swpValue = Value;
            Value = DataConvertionExtensions.Convert<DateTime?[], TValue>(new DateTime?[] { range[0], range[1] });
        }

        internal void OnRangeItemOut(DateTime?[] range) => Value = _swpValue;

        internal void OnRangeItemClicked(DateTime?[] range)
        {
            _swpValue = DataConvertionExtensions.Convert<DateTime?[], TValue>(new DateTime?[] { range[0], range[1] });
            ChangeValue((DateTime)range[0], 0);
            ChangeValue((DateTime)range[1], 1);
            _pickerStatus[0].IsNewValueSelected = true;
            _pickerStatus[1].IsNewValueSelected = true;
            Close();
        }

        private async Task<bool> SwitchFocus(int index)
        {
            if (index == 0 && (!_pickerStatus[1].IsNewValueSelected || Open) && !_inputEnd.IsOnFocused && !IsDisabled(1))
            {
                await Blur(0);
                await Focus(1);
            }
            else if (index == 1 && (!_pickerStatus[0].IsNewValueSelected || Open) && !_inputStart.IsOnFocused && !IsDisabled(0))
            {
                await Blur(1);
                await Focus(0);
            }
            else
            {
                await Focus(index); //keep focus on current input
                return false;
            }

            SetDisabledTime();

            return true;
        }

        protected virtual async Task OnBlur(int index) => await Task.Yield();

        protected void InitPicker(string picker)
        {
            if (string.IsNullOrEmpty(_pickerStatus[0].InitPicker))
            {
                _pickerStatus[0].InitPicker = picker;
            }
            if (string.IsNullOrEmpty(_pickerStatus[1].InitPicker))
            {
                _pickerStatus[1].InitPicker = picker;
            }
            ResetPlaceholder();
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

            var pickerValue = PickerValues[tempIndex];

            if (index == 0 || IsShowTime || Picker == DatePickerType.Time)
            {
                return pickerValue;
            }
            else
            {
                //First picker panel will show the value, second panel shows next
                //expected value that depends on Picker type
                return GetClosingDate(pickerValue);
            }
        }

        internal DateTime GetClosingDate(DateTime pickerValue, int offset = 1)
        {
            return Picker switch
            {
                DatePickerType.Year => pickerValue.AddYears(offset * 10),
                DatePickerType.Quarter or DatePickerType.Decade or DatePickerType.Month => pickerValue.AddYears(offset),
                _ => pickerValue.AddMonths(offset),
            };
        }

        public void ChangePlaceholder(string placeholder, int index = 0)
        {
            _placeholders[index] = placeholder;

            StateHasChanged();
        }

        public void ResetPlaceholder(int index = -1)
        {
            _placeholder.Switch(single =>
            {
                var placeholder = string.IsNullOrEmpty(single) ? DatePickerPlaceholder.GetPlaceholderByType(Picker, Locale) : single;
                _placeholders[0] = placeholder;
                _placeholders[1] = placeholder;
            }, arr =>
            {
                var rangePickerIndex = index >= 0 ? index : GetOnFocusPickerIndex();

                var placeholder = arr.Length > rangePickerIndex ? arr[rangePickerIndex] : null;

                if (placeholder is null)
                {
                    var (startPlaceholder, endPlaceholder) = DatePickerPlaceholder.GetRangePlaceHolderByType(Picker, Locale);

                    placeholder = rangePickerIndex == 0 ? startPlaceholder : endPlaceholder;
                }

                _placeholders[rangePickerIndex] = placeholder;
            });
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
                        _internalFormat = _pickerStatus[0].InitPicker switch
                        {
                            DatePickerType.Date => GetTimeFormat(),
                            DatePickerType.Month => Locale.Lang.YearMonthFormat,
                            DatePickerType.Year => Locale.Lang.YearFormat,
                            DatePickerType.Time when Use12Hours => Locale.Lang.TimeFormat12Hour,
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
                return Use12Hours ? Locale.Lang.DateTimeFormat12Hour : Locale.Lang.DateTimeFormat;
            }
            return Locale.Lang.DateFormat;
        }

        private FormatAnalyzer _formatAnalyzer;
        public FormatAnalyzer FormatAnalyzer => _formatAnalyzer ??= new(InternalFormat, Picker, Locale);

        public string GetFormatValue(DateTime value, int index)
        {
            string format;
            if (string.IsNullOrEmpty(Format))
                format = _pickerStatus[index].InitPicker switch
                {
                    DatePickerType.Week => $"{Locale.Lang.YearFormat}-{DateHelper.GetWeekOfYear(value, Locale.FirstDayOfWeek)}{Locale.Lang.Week}",
                    DatePickerType.Quarter => $"{Locale.Lang.YearFormat}-{DateHelper.GetDayOfQuarter(value)}",
                    _ => InternalFormat,
                };
            else
                format = InternalFormat;
            return value.ToString(format, CultureInfo);
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
                if (!UseDefaultPickerValue[1] && !_pickerStatus[1].IsValueSelected && index == 0)
                {
                    PickerValues[1] = date;
                }
                else if (!UseDefaultPickerValue[0] && !_pickerStatus[0].IsValueSelected && index == 1)
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

        protected void InvokeInternalOverlayVisibleChanged(bool visible)
        {
            if (IsRange)
            {
                if (!visible && (!_pickerStatus[0].IsNewValueSelected || !_pickerStatus[1].IsNewValueSelected))
                {
                    if (_pickerStatus[0].OldValue.HasValue && _pickerStatus[1].OldValue.HasValue)
                    {
                        var startDate = GetIndexValue(0);
                        var endDate = GetIndexValue(1);

                        if (startDate is not null && startDate != _pickerStatus[0].OldValue)
                        {
                            ChangeValue(_pickerStatus[0].OldValue.Value, 0);
                        }
                        if (endDate is not null && endDate != _pickerStatus[1].OldValue)
                        {
                            ChangeValue(_pickerStatus[1].OldValue.Value, 1);
                        }
                    }
                    else if (_pickerStatus[0].IsValueSelected || _pickerStatus[1].IsValueSelected)
                    {
                        ClearValue(-1);
                    }

                    AutoFocus = false;
                }
                else if (visible)
                {
                    _pickerStatus[0].OldValue = GetIndexValue(0);
                    _pickerStatus[1].OldValue = GetIndexValue(1);
                }
            }
            else if (IsShowTime || Picker == DatePickerType.Time)
            {
                var index = GetOnFocusPickerIndex();

                if (!_pickerStatus[index].IsNewValueSelected && !visible)
                {
                    if (_pickerStatus[index].OldValue.HasValue)
                    {
                        ChangeValue(_pickerStatus[index].OldValue.Value, index);
                    }
                    else
                    {
                        ClearValue(index);
                    }
                }
                else if (visible)
                {
                    _pickerStatus[index].OldValue = GetIndexValue(index);
                }
            }
            if (visible)
            {
                _pickerStatus[0].IsNewValueSelected = false;
                _pickerStatus[1].IsNewValueSelected = false;
            }
            OverlayVisibleChanged?.Invoke(this, visible);
        }

        protected void SetDisabledTime()
        {
            if (!IsRange || !IsShowTime)
            {
                return;
            }

            var endValue = GetIndexValue(1);
            var startValue = GetIndexValue(0);
            var isSameDate = startValue?.Date == endValue?.Date;

            if (_inputStart.IsOnFocused)
            {
                DisabledHours = dateTime => isSameDate ?
                   _hours.Where(h => h > endValue?.Hour).ToArray() : Array.Empty<int>();
                DisabledMinutes = dateTime => isSameDate && startValue?.Hour == endValue?.Hour ?
                   _minutesSeconds.Where(m => m > endValue?.Minute).ToArray() : Array.Empty<int>();
                DisabledSeconds = dateTime => isSameDate && startValue?.Hour == endValue?.Hour && startValue?.Minute == endValue?.Minute ?
                   _minutesSeconds.Where(s => s > endValue?.Second).ToArray() : Array.Empty<int>();
            }
            else if (_inputEnd.IsOnFocused)
            {
                DisabledHours = dateTime => isSameDate ?
                   _hours.Where(h => h < startValue?.Hour).ToArray() : Array.Empty<int>();
                DisabledMinutes = dateTime => isSameDate && startValue?.Hour == endValue?.Hour ?
                   _minutesSeconds.Where(m => m < startValue?.Minute).ToArray() : Array.Empty<int>();
                DisabledSeconds = dateTime => isSameDate && startValue?.Hour == endValue?.Hour && startValue?.Minute == endValue?.Minute ?
                   _minutesSeconds.Where(s => s < startValue?.Second).ToArray() : Array.Empty<int>();
            }
        }

        internal async Task OnNowClick()
        {
            var pickerIndex = GetOnFocusPickerIndex();
            await OnSelect(DateTime.Now, GetOnFocusPickerIndex());
            _pickerStatus[pickerIndex].IsNewValueSelected = true;
            Close();
        }
    }
}
