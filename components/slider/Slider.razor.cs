using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AntDesign
{
    using SliderValueType = OneOf<double, (double, double)>;

    public partial class Slider : AntDomComponentBase
    {
        private const string PreFixCls = "ant-slider";
        private DomRect _sliderDom;
        private DomRect _leftHandleDom;
        private DomRect _rightHandleDom;
        private ElementReference _slider;
        private ElementReference _leftHandle;
        private ElementReference _rightHandle;
        private string _leftHandleStyle = "left: 0%; right: auto; transform: translateX(-50%);";
        private string _rightHandleStyle = "left: 0%; right: auto; transform: translateX(-50%);";
        private string _trackStyle = "left: 0%; width: 0%; right: auto;";
        private bool _mouseDown;
        private bool _right = true;

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
        public SliderValueType DefaultValue { get; set; } = 0;

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
        public bool Range { get; set; }

        /// <summary>
        /// reverse the component
        /// </summary>
        [Parameter]
        public bool Reverse { get; set; }

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
                _leftValue = Math.Max(value, Min);
                _leftValue = Math.Min(_leftValue, RightValue);
                _leftValue = GetNearestStep(_leftValue);
                SetStyle();
            }
        }

        private double _rightValue = double.MaxValue;

        // the default non-range value
        private double RightValue
        {
            get => _rightValue;
            set
            {
                _rightValue = Math.Min(value, Max);
                if (Range)
                {
                    _rightValue = Math.Max(LeftValue, _rightValue);
                }
                else
                {
                    _rightValue = Math.Max(Min, _rightValue);
                }
                _rightValue = GetNearestStep(_rightValue);
                SetStyle();
            }
        }

        /// <summary>
        /// The value of slider. When range is false, use number, otherwise, use [number, number]
        /// </summary>
        [Parameter]
        public SliderValueType Value
        {
            get
            {
                if (Range)
                {
                    return (LeftValue, RightValue);
                }
                else
                {
                    return _rightValue;
                }
            }
            set
            {
                value.Switch(d => RightValue = d, l =>
                {
                    Range = true;

                    var values = l;
                    LeftValue = values.Item1;
                    RightValue = values.Item2;
                });
            }
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
        public EventCallback<SliderValueType> OnAfterChange { get; set; }

        /// <summary>
        /// Callback function that is fired when the user changes the slider's value.
        /// </summary>
        [Parameter]
        public Action<SliderValueType> OnChange { get; set; }

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

            DomEventService.AddEventListener("window", "mousemove", OnMouseMove);
            DomEventService.AddEventListener("window", "mouseup", OnMouseUp);
        }

        public async override Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);

            var dict = parameters.ToDictionary();
            if (dict.ContainsKey(nameof(DefaultValue)) && !dict.ContainsKey(nameof(Value)))
            {
                Value = parameters.GetValueOrDefault(nameof(DefaultValue), SliderValueType.FromT0(0));
            }
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

        private void OnMouseDown()
        {
            _mouseDown = true && !Disabled;
        }

        private async void OnMouseMove(JsonElement jsonElement)
        {
            if (_mouseDown)
            {
                await CalculateValueAsync(jsonElement.GetProperty(Vertical ? "clientY" : "clientX").GetDouble());

                OnChange?.Invoke(RightValue);
            }
        }

        private void OnMouseUp(JsonElement jsonElement)
        {
            _mouseDown = false;
            OnAfterChange.InvokeAsync(RightValue);
        }

        private async void OnClick(MouseEventArgs args)
        {
            if (!Disabled)
            {
                await CalculateValueAsync(Vertical ? args.ClientY : args.ClientX);
            }
        }

        private async Task CalculateValueAsync(double clickClient)
        {
            _sliderDom = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, _slider);
            double sliderOffset = (double)(Vertical ? _sliderDom.top : _sliderDom.left);
            double sliderLength = (double)(Vertical ? _sliderDom.height : _sliderDom.width);
            double handleNewPosition;
            if (_right)
            {
                if (_rightHandleDom == null)
                {
                    _rightHandleDom = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, _rightHandle);
                }
                double handleLength = (double)(Vertical ? _rightHandleDom.height : _rightHandleDom.width);
                if (Reverse)
                {
                    if (Vertical)
                    {
                        handleNewPosition = clickClient - sliderOffset + handleLength / 2;
                    }
                    else
                    {
                        handleNewPosition = sliderLength - (clickClient - sliderOffset) + handleLength / 2;
                    }
                }
                else
                {
                    if (Vertical)
                    {
                        handleNewPosition = sliderOffset + sliderLength - clickClient - handleLength / 2;
                    }
                    else
                    {
                        handleNewPosition = clickClient - sliderOffset - handleLength / 2;
                    }
                }

                double rightV = Max * handleNewPosition / sliderLength;
                if (rightV < LeftValue)
                {
                    _right = false;
                    LeftValue = rightV;
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
                    _leftHandleDom = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, _leftHandle);
                }
                double handleLength = (double)(Vertical ? _rightHandleDom.height : _rightHandleDom.width);
                if (Reverse)
                {
                    if (Vertical)
                    {
                        handleNewPosition = clickClient - sliderOffset + handleLength / 2;
                    }
                    else
                    {
                        handleNewPosition = sliderLength - (clickClient - sliderOffset) + handleLength / 2;
                    }
                }
                else
                {
                    if (Vertical)
                    {
                        handleNewPosition = sliderOffset + sliderLength - clickClient - handleLength / 2;
                    }
                    else
                    {
                        handleNewPosition = clickClient - sliderOffset - handleLength / 2;
                    }
                }

                double leftV = Max * handleNewPosition / sliderLength;
                if (leftV > RightValue)
                {
                    _right = true;
                    RightValue = leftV;
                }
                else
                {
                    LeftValue = leftV;
                }
            }
        }

        private void SetStyle()
        {
            _rightHandleStyle = string.Format(CultureInfo.CurrentCulture, RightHandleStyleFormat, (RightValue / Max).ToString("p", CultureInfo.CurrentCulture));
            if (Range)
            {
                _trackStyle = string.Format(CultureInfo.CurrentCulture, TrackStyleFormat, (LeftValue / Max).ToString("p", CultureInfo.CurrentCulture), ((RightValue - LeftValue) / Max).ToString("p", CultureInfo.CurrentCulture));
                _leftHandleStyle = string.Format(CultureInfo.CurrentCulture, LeftHandleStyleFormat, (LeftValue / Max).ToString("p", CultureInfo.CurrentCulture));
            }
            else
            {
                _trackStyle = string.Format(CultureInfo.CurrentCulture, TrackStyleFormat, "0%", (RightValue / Max).ToString("p", CultureInfo.CurrentCulture));
            }

            StateHasChanged();
        }

        private string SetMarkPosition(double key)
        {
            return (key / Max).ToString("p", CultureInfo.CurrentCulture);
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
                return Math.Round(value / Step.Value, 0) * Step.Value + Min;
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
    }
}
