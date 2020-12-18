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
        private Element _sliderDom;
        private Element _leftHandleDom;
        private Element _rightHandleDom;
        private ElementReference _slider;
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
        private DomEventService DomEventService { get; set; }

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
        [Parameter]
        public double? Step { get; set; } = 1;

        /// <summary>
        /// Slider will pass its value to tipFormatter, and display its value in Tooltip, and hide Tooltip when return value is null.
        /// </summary>
        [Parameter]
        public Func<string> TipFormatter { get; set; }

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
                    (double, double) typedValue = DataConvertionExtensions.Convert<TValue, (double, double)>(CurrentValue);
                    if (value != typedValue.Item1)
                        CurrentValue = DataConvertionExtensions.Convert<(double, double), TValue>((_leftValue, RightValue));
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
                        (double, double) typedValue = DataConvertionExtensions.Convert<TValue, (double, double)>(CurrentValue);
                        if (value != typedValue.Item2)
                            CurrentValue = DataConvertionExtensions.Convert<(double, double), TValue>((LeftValue, _rightValue));
                    }
                    else
                    {
                        double typedValue = DataConvertionExtensions.Convert<TValue, double>(CurrentValue);
                        if (value != typedValue)
                            //CurrentValue = DoubleToGeneric(_rightValue);
                            CurrentValue = DataConvertionExtensions.Convert<double, TValue>(_rightValue);
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
        public Action<TValue> OnAfterChange { get; set; } //use Action here intead of EventCallback, otherwise VS will not complie when user add a delegate

        /// <summary>
        /// Callback function that is fired when the user changes the slider's value.
        /// </summary>
        [Parameter]
        public Action<TValue> OnChange { get; set; }

        /// <summary>
        /// Set Tooltip display position. Ref Tooltip
        /// </summary>
        [Parameter]
        public string TooltipPlacement { get; set; }

        /// <summary>
        /// If true, Tooltip will show always, or it will not show anyway, even if dragging or hovering.
        /// </summary>
        [Parameter]
        public bool TooltipVisible { get; set; }

        /// <summary>
        /// The DOM container of the Tooltip, the default behavior is to create a div element in body.
        /// </summary>
        [Parameter]
        public object GetTooltipPopupContainer { get; set; }

        #endregion Parameters

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        public async override Task SetParametersAsync(ParameterView parameters)
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
                        //if (typeof(T) == typeof((int, int)))
                        //{
                        //    defaultValue = parameters.GetValueOrDefault(nameof(DefaultValue), DataConvertionExtensions.Convert<(int, int), T>((0, 0)));
                        //}
                        //else
                        //{
                        defaultValue = parameters.GetValueOrDefault(nameof(DefaultValue), DataConvertionExtensions.Convert<(double, double), TValue>((0, 0)));
                        //}
                        LeftValue = DataConvertionExtensions.Convert<TValue, (double, double)>(defaultValue).Item1;
                        RightValue = DataConvertionExtensions.Convert<TValue, (double, double)>(defaultValue).Item2;
                    }
                    else
                    {
                        //if (typeof(T) == typeof(int))
                        //{
                        //    defaultValue = parameters.GetValueOrDefault(nameof(DefaultValue), DataConvertionExtensions.Convert<int, T>(0));
                        //}
                        //else
                        //{
                        defaultValue = parameters.GetValueOrDefault(nameof(DefaultValue), DataConvertionExtensions.Convert<double, TValue>(0));
                        //}
                        RightValue = DataConvertionExtensions.Convert<TValue, double>(defaultValue);
                    }
                }
                else
                {
                    if (Range)
                    {
                        LeftValue = DataConvertionExtensions.Convert<TValue, (double, double)>(CurrentValue).Item1;
                        RightValue = DataConvertionExtensions.Convert<TValue, (double, double)>(CurrentValue).Item2;
                    }
                    else
                    {
                        RightValue = DataConvertionExtensions.Convert<TValue, double>(CurrentValue);
                    }
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
                .If($"{PreFixCls}-vertical", () => Vertical)
                .If($"{PreFixCls}-with-marks", () => Marks != null);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                DomEventService.AddEventListener("window", "mousemove", OnMouseMove, false);
                DomEventService.AddEventListener("window", "mouseup", OnMouseUp, false);
            }

            base.OnAfterRender(firstRender);
        }

        protected override void Dispose(bool disposing)
        {
            DomEventService.RemoveEventListerner<JsonElement>("window", "mousemove", OnMouseMove);
            DomEventService.RemoveEventListerner<JsonElement>("window", "mouseup", OnMouseUp);

            base.Dispose(disposing);
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

            if (Step != null && (Max - Min) / Step % 1 != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Step), $"Must be divided by ({Max} - {Min}).");
            }
        }

        private async void OnMouseDown(MouseEventArgs args)
        {
            //// _sliderDom = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _slider);
            //_sliderDom = await JsInvokeAsync<Element>(JSInteropConstants.GetDomInfo, _slider);
            //decimal x = (decimal)args.ClientX;
            //decimal y = (decimal)args.ClientY;

            //_mouseDown = !Disabled
            //    && _sliderDom.clientLeft <= x && x <= _sliderDom.clientLeft + _sliderDom.clientWidth
            //    && _sliderDom.clientTop <= y && y <= _sliderDom.clientTop + _sliderDom.clientHeight;

            _mouseDown = !Disabled;
        }

        private MouseEventArgs _edgeClickArgs;
        private bool? _edgeClicked;
        private void OnMouseDownEdge(MouseEventArgs args, bool right)
        {
            _right = right;
            _initialLeftValue = _leftValue;
            _initialRightValue = _rightValue;
            _edgeClickArgs = args;
        }

        private bool IsMoveInEdgeBoundary(JsonElement jsonElement)
        {            
            if (_edgeClicked == null)
            {
                double clientX = jsonElement.GetProperty("clientX").GetDouble();
                double clientY = jsonElement.GetProperty("clientY").GetDouble();
                bool altKey = jsonElement.GetProperty("altKey").GetBoolean();
                bool ctrlKey = jsonElement.GetProperty("ctrlKey").GetBoolean();
                bool metaKey = jsonElement.GetProperty("metaKey").GetBoolean();
                bool shiftKey = jsonElement.GetProperty("shiftKey").GetBoolean();

                _edgeClicked = _edgeClickArgs != null
                            && _edgeClickArgs.ClientX == clientX
                            && _edgeClickArgs.ClientY == clientY
                            && _edgeClickArgs.CtrlKey == ctrlKey
                            && _edgeClickArgs.MetaKey == metaKey
                            && _edgeClickArgs.AltKey == altKey
                            && _edgeClickArgs.ShiftKey == shiftKey;
            }
            return _edgeClicked.Value;
        }

        private async void OnMouseMove(JsonElement jsonElement)
        {
            if (_mouseDown)
            {
                _mouseMove = true;
                await CalculateValueAsync(Vertical ? jsonElement.GetProperty("pageY").GetDouble() : jsonElement.GetProperty("pageX").GetDouble());

                OnChange?.Invoke(CurrentValue);
            }
        }

        private async void OnMouseUp(JsonElement jsonElement)
        {
            if (_mouseDown)
            {
                _mouseDown = false;
                if (!IsMoveInEdgeBoundary(jsonElement))
                {
                    await CalculateValueAsync(Vertical ? jsonElement.GetProperty("pageY").GetDouble() : jsonElement.GetProperty("pageX").GetDouble());
                    OnAfterChange?.Invoke(CurrentValue);
                }
            }
            if (_edgeClicked != null)
            {
                _edgeClicked = null;
                _initialLeftValue = _leftValue;
                _initialRightValue = _rightValue;
            }
        }

        private async Task CalculateValueAsync(double clickClient)
        {
            _sliderDom = await JsInvokeAsync<Element>(JSInteropConstants.GetDomInfo, _slider);
            double sliderOffset = (double)(Vertical ? _sliderDom.absoluteTop: _sliderDom.absoluteLeft);
            double sliderLength = (double)(Vertical ? _sliderDom.clientHeight : _sliderDom.clientWidth);
            double handleNewPosition;
            if (_right)
            {
                if (_rightHandleDom == null)
                {
                    _rightHandleDom = await JsInvokeAsync<Element>(JSInteropConstants.GetDomInfo, _rightHandle);
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

                double rightV = Max * handleNewPosition / sliderLength;
                if (rightV < LeftValue)
                {
                    _right = false;
                    if (_mouseDown)
                        RightValue = _initialLeftValue;
                    LeftValue = rightV;
                    await JsInvokeAsync(JSInteropConstants.Focus, _leftHandle);
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
                    _leftHandleDom = await JsInvokeAsync<Element>(JSInteropConstants.GetDomInfo, _leftHandle);
                }
                if (_rightHandleDom == null)
                {
                    _rightHandleDom = await JsInvokeAsync<Element>(JSInteropConstants.GetDomInfo, _rightHandle);
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

                double leftV = Max * handleNewPosition / sliderLength;
                if (leftV > RightValue)
                {
                    _right = true;
                    if (_mouseDown)
                        LeftValue = _initialRightValue;
                    RightValue = leftV;
                    await JsInvokeAsync(JSInteropConstants.Focus, _rightHandle);
                }
                else
                {
                    LeftValue = leftV;
                }
            }
        }

        private void SetStyle()
        {
            _rightHandleStyle = string.Format(CultureInfo.CurrentCulture, RightHandleStyleFormat, Formatter.ToPercentWithoutBlank(RightValue / Max));
            if (Range)
            {
                _trackStyle = string.Format(CultureInfo.CurrentCulture, TrackStyleFormat, Formatter.ToPercentWithoutBlank(LeftValue / Max), Formatter.ToPercentWithoutBlank((RightValue - LeftValue) / Max));
                _leftHandleStyle = string.Format(CultureInfo.CurrentCulture, LeftHandleStyleFormat, Formatter.ToPercentWithoutBlank(LeftValue / Max));
            }
            else
            {
                _trackStyle = string.Format(CultureInfo.CurrentCulture, TrackStyleFormat, "0%", Formatter.ToPercentWithoutBlank(RightValue / Max));
            }

            StateHasChanged();
        }

        private string SetMarkPosition(double key)
        {
            return Formatter.ToPercentWithoutBlank(key / Max);
        }

        private string IsActiveMark(double key)
        {
            bool active = (Range && key >= LeftValue && key <= RightValue)
                || (!Range && key <= RightValue);

            return active ? "ant-slider-dot-active" : string.Empty;
        }

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
                LeftValue = DataConvertionExtensions.Convert<TValue, (double, double)>(value).Item1;
                RightValue = DataConvertionExtensions.Convert<TValue, (double, double)>(value).Item2;
            }
            else
            {
                RightValue = DataConvertionExtensions.Convert<TValue, double>(value);
            }
        }

        private bool IsLeftAndRightChanged(TValue value)
        {
            (double, double) typedValue = DataConvertionExtensions.Convert<TValue, (double, double)>(value);
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

        private TValue SortValue(TValue value)
        {
            TValue orderedValue = value;
            if (Range)
            {
                //sort if needed
                (double, double) typedValue = DataConvertionExtensions.Convert<TValue, (double, double)>(value);
                if (typedValue.Item1 > typedValue.Item2)
                {
                    orderedValue = DataConvertionExtensions.Convert<(double, double), TValue>((typedValue.Item2, typedValue.Item1));
                }
            }
            return orderedValue;
        }
    }
}
