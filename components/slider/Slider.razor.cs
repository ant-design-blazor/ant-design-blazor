using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.core.Extensions;
using AntDesign.Core.Helpers;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Slider<TValue> : AntInputComponentBase<TValue>
    {
        private const string PreFixCls = "ant-slider";
        private HtmlElement _sliderDom;
        private HtmlElement _leftHandleDom;
        private HtmlElement _rightHandleDom;
        private ElementReference _leftHandle;
        private ElementReference _rightHandle;
        private string _leftHandleStyle = "left: 0%; right: auto; transform: translateX(-50%);";
        private string _rightHandleStyle = "left: 0%; right: auto; transform: translateX(-50%);";
        private string _trackStyle = "left: 0%; width: 0%; right: auto;";
        private bool _mouseDown;
        private bool _mouseMove;
        private bool _right = true;
        private bool _initialized = false;
        private double _initialLeftValue;
        private double _initialRightValue;
        private Tooltip _toolTipRight;
        private Tooltip _toolTipLeft;

        private string RightHandleStyleFormat
        {
            get
            {
                if (Reverse)
                {
                    if (Vertical)
                    {
                        return "bottom: auto; top: {0}; transform: translateY(-50%);";
                    }
                    else
                    {
                        return "right: {0}; left: auto; transform: translateX(50%);";
                    }
                }
                else
                {
                    if (Vertical)
                    {
                        return "top: auto; bottom: {0}; transform: translateY(50%);";
                    }
                    else
                    {
                        return "left: {0}; right: auto; transform: translateX(-50%);";
                    }
                }
            }
        }

        private string LeftHandleStyleFormat
        {
            get
            {
                if (Reverse)
                {
                    if (Vertical)
                    {
                        return "bottom: auto; top: {0}; transform: translateY(-50%);";
                    }
                    else
                    {
                        return "right: {0}; left: auto; transform: translateX(50%);";
                    }
                }
                else
                {
                    if (Vertical)
                    {
                        return "top: auto; bottom: {0}; transform: translateY(50%);";
                    }
                    else
                    {
                        return "left: {0}; right: auto; transform: translateX(-50%);";
                    }
                }
            }
        }

        private string TrackStyleFormat
        {
            get
            {
                if (Reverse)
                {
                    if (Vertical)
                    {
                        return "bottom: auto; height: {1}; top: {0};";
                    }
                    else
                    {
                        return "right: {0}; width: {1}; left: auto;";
                    }
                }
                else
                {
                    if (Vertical)
                    {
                        return "top: auto; height: {1}; bottom: {0};";
                    }
                    else
                    {
                        return "left: {0}; width: {1}; right: auto;";
                    }
                }
            }
        }

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        #region Parameters

        /// <summary>
        /// The default value of slider. When <see cref="Range"/> is false, use number, otherwise, use [number, number]
        /// </summary>
        [Parameter]
        public TValue DefaultValue { get; set; }

        /// <summary>
        /// If true, the slider will not be interactable
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Whether the thumb can drag over tick only
        /// </summary>
        [Parameter]
        public bool Dots { get; set; }

        /// <summary>
        /// Make effect when <see cref="Marks"/> not null, true means containment and false means coordinative
        /// </summary>
        [Parameter]
        public bool Included { get; set; } = true;

        /// <summary>
        /// Tick mark of Slider, type of key must be number, and must in closed interval [min, max], each mark can declare its own style
        /// </summary>
        [Parameter]
        public SliderMark[] Marks { get; set; }

        /// <summary>
        /// The maximum value the slider can slide to
        /// </summary>
        [Parameter]
        public double Max { get; set; } = 100;

        /// <summary>
        /// The minimum value the slider can slide to
        /// </summary>
        [Parameter]
        public double Min { get; set; } = 0;

        private double MinMaxDelta => Max - Min;

        /// <summary>
        /// dual thumb mode
        /// </summary>
        //[Parameter]
        private bool? _range;

        public bool Range
        {
            get
            {
                if (_range == null)
                {
                    Type type = typeof(TValue);
                    Type doubleType = typeof(double);
                    Type tupleDoubleType = typeof((double, double));
                    if (type == doubleType)
                    {
                        _range = false;
                    }
                    else if (type == tupleDoubleType)
                    {
                        _range = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException($"Type argument of Slider should be one of {doubleType}, {tupleDoubleType}");
                    }
                }
                return _range.Value;
            }
            //private set { _range = value; }
        }

        /// <summary>
        /// reverse the component
        /// </summary>
        private bool _reverse;

        [Parameter]
        public bool Reverse
        {
            get { return _reverse; }
            set
            {
                if (_reverse != value)
                {
                    _reverse = value;
                    SetStyle();
                }
            }
        }

        /// <summary>
        /// The granularity the slider can step through values. Must greater than 0, and be divided by (<see cref="Max"/> - <see cref="Min"/>) . When <see cref="Marks"/> no null, <see cref="Step"/> can be null.
        /// </summary>
        private double? _step = 1;

        private int _precision;

        [Parameter]
        public double? Step
        {
            get { return _step; }
            set
            {
                _step = value;
                //no need to evaluate if no tooltip
                if (_step != null && _isTipFormatterDefault)
                {
                    char separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
                    string[] number = _step.ToString().Split(separator);
                    if (number.Length > 1)
                    {
                        _precision = number[1].Length;
                        _tipFormatter = (d) => string.Format(CultureInfo.CurrentCulture, "{0:N02}", Math.Round(d, _precision));
                    }
                }
            }
        }

        private double _leftValue = double.MinValue;

        private double LeftValue
        {
            get => _leftValue;
            set
            {
                double candidate = Clamp(value, Min, Max);
                if (_leftValue != candidate)
                {
                    _leftValue = candidate;
                    SetStyle();
                    (double, double) typedValue = DataConversionExtensions.Convert<TValue, (double, double)>(CurrentValue);
                    if (value != typedValue.Item1)
                        CurrentValue = DataConversionExtensions.Convert<(double, double), TValue>((_leftValue, RightValue));
                }
            }
        }

        private double _rightValue = double.MaxValue;

        // the default non-range value
        private double RightValue
        {
            get => _rightValue;
            set
            {
                double candidate;
                if (Range)
                {
                    candidate = Clamp(value, LeftValue, Max);
                }
                else
                {
                    candidate = Clamp(value, Min, Max);
                }

                if (_rightValue != candidate)
                {
                    _rightValue = candidate;
                    SetStyle();
                    if (Range)
                    {
                        //CurrentValue = TupleToGeneric((LeftValue, _rightValue));
                        (double, double) typedValue = DataConversionExtensions.Convert<TValue, (double, double)>(CurrentValue);
                        if (value != typedValue.Item2)
                            CurrentValue = DataConversionExtensions.Convert<(double, double), TValue>((LeftValue, _rightValue));
                    }
                    else
                    {
                        double typedValue = DataConversionExtensions.Convert<TValue, double>(CurrentValue);
                        if (value != typedValue)
                            //CurrentValue = DoubleToGeneric(_rightValue);
                            CurrentValue = DataConversionExtensions.Convert<double, TValue>(_rightValue);
                    }
                }
            }
        }

        private double Clamp(
            double value, double inclusiveMinimum, double inclusiveMaximum)
        {
            if (value < inclusiveMinimum)
            {
                value = inclusiveMinimum;
            }
            if (value > inclusiveMaximum)
            {
                value = inclusiveMaximum;
            }
            return GetNearestStep(value);
        }

        /// <summary>
        /// If true, the slider will be vertical.
        /// </summary>
        [Parameter]
        public bool Vertical { get; set; }

        /// <summary>
        /// Fire when onmouseup is fired.
        /// </summary>
        [Parameter]
        public EventCallback<TValue> OnAfterChange { get; set; }

        /// <summary>
        /// Callback function that is fired when the user changes the slider's value.
        /// </summary>
        [Parameter]
        public EventCallback<TValue> OnChange { get; set; }

        [Parameter]
        public bool HasTooltip { get; set; } = true;

        /// <summary>
        /// Slider will pass its value to tipFormatter, and display its value in Tooltip
        /// </summary>
        private bool _isTipFormatterDefault = true;

        private Func<double, string> _tipFormatter = (d) => d.ToString(LocaleProvider.CurrentLocale.CurrentCulture);

        [Parameter]
        public Func<double, string> TipFormatter
        {
            get { return _tipFormatter; }
            set
            {
                _tipFormatter = value;
                _isTipFormatterDefault = false;
            }
        }

        /// <summary>
        /// Set Tooltip display position. Ref Tooltip
        /// </summary>
        [Parameter]
        public Placement TooltipPlacement { get; set; }

        /// <summary>
        /// If true, Tooltip will show always, or it will not show anyway, even if dragging or hovering.
        /// </summary>
        private bool _tooltipVisible;

        private bool _tooltipRightVisible;
        private bool _tooltipLeftVisible;

        [Parameter]
        public bool TooltipVisible
        {
            get { return _tooltipVisible; }
            set
            {
                if (_tooltipVisible != value)
                {
                    _tooltipVisible = value;
                    //ensure parameter loading is not happening because values are changing during mouse moving
                    //otherwise the tooltip will be vanishing when mouse moves out of the edge
                    if (!_mouseDown)
                    {
                        _tooltipRightVisible = _tooltipVisible;
                        _tooltipLeftVisible = _tooltipVisible;
                    }
                }
            }
        }

        /// <summary>
        /// The DOM container of the Tooltip, the default behavior is to create a div element in body.
        /// </summary>
        [Parameter]
        public object GetTooltipPopupContainer { get; set; }

        #endregion Parameters

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetStyle();
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);

            var dict = parameters.ToDictionary();
            if (!_initialized)
            {
                if (!dict.ContainsKey(nameof(Value)))
                {
                    TValue defaultValue;
                    if (Range)
                    {
                        defaultValue = parameters.GetValueOrDefault(nameof(DefaultValue), DataConversionExtensions.Convert<(double, double), TValue>((0, 0)));
                        LeftValue = DataConversionExtensions.Convert<TValue, (double, double)>(defaultValue).Item1;
                        RightValue = DataConversionExtensions.Convert<TValue, (double, double)>(defaultValue).Item2;
                    }
                    else
                    {
                        defaultValue = parameters.GetValueOrDefault(nameof(DefaultValue), DataConversionExtensions.Convert<double, TValue>(0));
                        RightValue = DataConversionExtensions.Convert<TValue, double>(defaultValue);
                    }
                }
                else
                {
                    if (Range)
                    {
                        LeftValue = DataConversionExtensions.Convert<TValue, (double, double)>(CurrentValue).Item1;
                        RightValue = DataConversionExtensions.Convert<TValue, (double, double)>(CurrentValue).Item2;
                    }
                    else
                    {
                        RightValue = DataConversionExtensions.Convert<TValue, double>(CurrentValue);
                    }
                }
                if (!dict.ContainsKey(nameof(TooltipPlacement)))
                {
                    if (Vertical)
                        TooltipPlacement = Placement.Right;
                    else
                        TooltipPlacement = Placement.Top;
                }
            }

            _initialized = true;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            ValidateParameter();

            ClassMapper.Clear()
                .Add(PreFixCls)
                .If($"{PreFixCls}-disabled", () => Disabled)
                .If($"{PreFixCls}-horizontal", () => !Vertical)
                .If($"{PreFixCls}-vertical", () => Vertical)
                .If($"{PreFixCls}-with-marks", () => Marks != null)
                .If($"{PreFixCls}-rtl", () => RTL);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                DomEventListener.AddShared<JsonElement>("window", "mousemove", OnMouseMove);
                DomEventListener.AddShared<JsonElement>("window", "mouseup", OnMouseUp);
            }

            base.OnAfterRender(firstRender);
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener?.Dispose();
            base.Dispose(disposing);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                if (_toolTipRight != null && HasTooltip)
                {
                    _rightHandle = _toolTipRight.Ref;
                    if (_toolTipLeft != null)
                    {
                        _leftHandle = _toolTipLeft.Ref;
                    }
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private void ValidateParameter()
        {
            if (Step == null && Marks == null)
            {
                throw new ArgumentOutOfRangeException(nameof(Step), $"{nameof(Step)} can only be null when {nameof(Marks)} is not null.");
            }

            if (Step <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Step), "Must greater than 0.");
            }

            var minMaxStepComparison = (Max - Min) / Step % 1;
            if (Step != null && (minMaxStepComparison < 0 || minMaxStepComparison >= 0.1))
            {
                throw new ArgumentOutOfRangeException(nameof(Step), $"Must be divided by ({Max} - {Min}).");
            }
        }

        private void OnMouseDown(MouseEventArgs args)
        {
            _mouseDown = !Disabled;
        }

        private double _trackedClientX;
        private double _trackedClientY;

        private void OnMouseDownEdge(MouseEventArgs args, bool right)
        {
            _right = right;
            _initialLeftValue = _leftValue;
            _initialRightValue = _rightValue;
            _trackedClientX = args.ClientX;
            _trackedClientY = args.ClientY;
            if (_toolTipRight != null)
            {
                if (_right)
                {
                    _tooltipRightVisible = true;
                }
                else
                {
                    _tooltipLeftVisible = true;
                }
            }
        }

        private bool IsMoveInEdgeBoundary(JsonElement jsonElement)
        {
            double clientX = jsonElement.GetProperty("clientX").GetDouble();
            double clientY = jsonElement.GetProperty("clientY").GetDouble();

            return (clientX == _trackedClientX && clientY == _trackedClientY);
        }

        private async void OnMouseMove(JsonElement jsonElement)
        {
            if (_mouseDown)
            {
                _trackedClientX = jsonElement.GetProperty("clientX").GetDouble();
                _trackedClientY = jsonElement.GetProperty("clientY").GetDouble();
                _mouseMove = true;
                await CalculateValueAsync(Vertical ? jsonElement.GetProperty("pageY").GetDouble() : jsonElement.GetProperty("pageX").GetDouble());

                if (OnChange.HasDelegate)
                    await InvokeAsync(() => OnChange.InvokeAsync(CurrentValue));
            }
        }

        private async void OnMouseUp(JsonElement jsonElement)
        {
            bool isMoveInEdgeBoundary = IsMoveInEdgeBoundary(jsonElement);
            if (_mouseDown)
            {
                _mouseDown = false;
                if (!isMoveInEdgeBoundary)
                {
                    await CalculateValueAsync(Vertical ? jsonElement.GetProperty("pageY").GetDouble() : jsonElement.GetProperty("pageX").GetDouble());
                }
                if (OnAfterChange.HasDelegate)
                    await InvokeAsync(() => OnAfterChange.InvokeAsync(CurrentValue));
            }
            if (_toolTipRight != null)
            {
                if (_tooltipRightVisible != TooltipVisible)
                {
                    _tooltipRightVisible = TooltipVisible;
                    _toolTipRight.SetVisible(TooltipVisible);
                }

                if (_tooltipLeftVisible != TooltipVisible)
                {
                    _tooltipLeftVisible = TooltipVisible;
                    _toolTipLeft.SetVisible(TooltipVisible);
                }
            }

            _initialLeftValue = _leftValue;
            _initialRightValue = _rightValue;
        }

        private async Task CalculateValueAsync(double clickClient)
        {
            _sliderDom = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, Ref);
            double sliderOffset = (double)(Vertical ? _sliderDom.AbsoluteTop : _sliderDom.AbsoluteLeft);
            double sliderLength = (double)(Vertical ? _sliderDom.ClientHeight : _sliderDom.ClientWidth);
            double handleNewPosition;
            if (_right)
            {
                if (_rightHandleDom == null)
                {
                    _rightHandleDom = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _rightHandle);
                }
                if (Reverse)
                {
                    if (Vertical)
                    {
                        handleNewPosition = clickClient - sliderOffset;
                    }
                    else
                    {
                        handleNewPosition = sliderLength - (clickClient - sliderOffset);
                    }
                }
                else
                {
                    if (Vertical)
                    {
                        handleNewPosition = sliderOffset + sliderLength - clickClient;
                    }
                    else
                    {
                        handleNewPosition = clickClient - sliderOffset;
                    }
                }

                double rightV = (MinMaxDelta * handleNewPosition / sliderLength) + Min;
                if (rightV < LeftValue)
                {
                    _right = false;
                    if (_mouseDown)
                        RightValue = _initialLeftValue;
                    LeftValue = rightV;
                    await FocusAsync(_leftHandle);
                }
                else
                {
                    RightValue = rightV;
                }
            }
            else
            {
                if (_leftHandleDom == null)
                {
                    _leftHandleDom = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _leftHandle);
                }
                if (_rightHandleDom == null)
                {
                    _rightHandleDom = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _rightHandle);
                }
                if (Reverse)
                {
                    if (Vertical)
                    {
                        handleNewPosition = clickClient - sliderOffset;
                    }
                    else
                    {
                        handleNewPosition = sliderLength - (clickClient - sliderOffset);
                    }
                }
                else
                {
                    if (Vertical)
                    {
                        handleNewPosition = sliderOffset + sliderLength - clickClient;
                    }
                    else
                    {
                        handleNewPosition = clickClient - sliderOffset;
                    }
                }

                double leftV = (MinMaxDelta * handleNewPosition / sliderLength) + Min;
                if (leftV > RightValue)
                {
                    _right = true;
                    if (_mouseDown)
                        LeftValue = _initialRightValue;
                    RightValue = leftV;
                    await FocusAsync(_rightHandle);
                }
                else
                {
                    LeftValue = leftV;
                }
            }
        }

        private void SetStyle()
        {
            var rightHandPercentage = (RightValue - Min) / MinMaxDelta;
            _rightHandleStyle = string.Format(CultureInfo.CurrentCulture, RightHandleStyleFormat, Formatter.ToPercentWithoutBlank(rightHandPercentage));
            if (Range)
            {
                var leftHandPercentage = (LeftValue - Min) / MinMaxDelta;
                _trackStyle = string.Format(CultureInfo.CurrentCulture, TrackStyleFormat, Formatter.ToPercentWithoutBlank(leftHandPercentage), Formatter.ToPercentWithoutBlank((RightValue - LeftValue) / MinMaxDelta));
                _leftHandleStyle = string.Format(CultureInfo.CurrentCulture, LeftHandleStyleFormat, Formatter.ToPercentWithoutBlank(leftHandPercentage));
            }
            else
            {
                _trackStyle = string.Format(CultureInfo.CurrentCulture, TrackStyleFormat, "0%", Formatter.ToPercentWithoutBlank(rightHandPercentage));
            }

            StateHasChanged();
        }

        private string SetMarkPosition(double key)
        {
            return Formatter.ToPercentWithoutBlank((key - Min) / MinMaxDelta);
        }

        private bool IsActiveMark(double key) => (Range && key >= LeftValue && key <= RightValue)
            || (!Range && key <= RightValue);

        private double GetNearestStep(double value)
        {
            if (Step.HasValue && (Marks == null || Marks.Length == 0))
            {
                return Math.Round(value / Step.Value, 0) * Step.Value;
            }
            else if (Step.HasValue)
            {
                return new double[2] { Math.Round(value / Step.Value) * Step.Value, Math.Round(value / Step.Value + 1) * Step.Value }.Union(Marks.Select(m => m.Key)).OrderBy(v => Math.Abs(v - value)).First();
            }
            else if (Marks.Length == 0)
            {
                return Min;
            }
            else
            {
                return Marks.Select(m => m.Key).OrderBy(v => Math.Abs(v - value)).First();
            }
        }

        protected override void OnValueChange(TValue value)
        {
            base.OnValueChange(value);

            if (Range)
            {
                if (IsLeftAndRightChanged(value))
                {
                    _leftValue = double.MinValue;
                    _rightValue = double.MaxValue;
                }
                LeftValue = DataConversionExtensions.Convert<TValue, (double, double)>(value).Item1;
                RightValue = DataConversionExtensions.Convert<TValue, (double, double)>(value).Item2;
            }
            else
            {
                RightValue = DataConversionExtensions.Convert<TValue, double>(value);
            }
        }

        private bool IsLeftAndRightChanged(TValue value)
        {
            (double, double) typedValue = DataConversionExtensions.Convert<TValue, (double, double)>(value);
            return (typedValue.Item1 != LeftValue) && (typedValue.Item2 != RightValue);
        }

        private TValue _value;

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
                TValue orderedValue = SortValue(value);
                var hasChanged = !EqualityComparer<TValue>.Default.Equals(orderedValue, Value);
                if (hasChanged)
                {
                    _value = orderedValue;
                    OnValueChange(orderedValue);
                }
            }
        }

        private TValue SortValue(TValue value)
        {
            TValue orderedValue = value;
            if (Range)
            {
                //sort if needed
                (double, double) typedValue = DataConversionExtensions.Convert<TValue, (double, double)>(value);
                if (typedValue.Item1 > typedValue.Item2)
                {
                    orderedValue = DataConversionExtensions.Convert<(double, double), TValue>((typedValue.Item2, typedValue.Item1));
                }
            }
            return orderedValue;
        }
    }
}
