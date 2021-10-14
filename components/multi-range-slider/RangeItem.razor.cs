using System;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Core.Helpers;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{

    public partial class RangeItem : AntInputComponentBase<(double, double)>
    {
        private const string PreFixCls = "ant-multi-range-slider";
        private HtmlElement _sliderDom;
        private ElementReference _firstHandle;
        private ElementReference _lastHandle;
        private string _firstHandleCssPosition = "left: 0%; right: auto; transform: translateX(-50%);";
        private string _lastHandleCssPosition = "left: 0%; right: auto; transform: translateX(-50%);";
        private string _trackCssPosition = "left: 0%; width: 0%; right: auto;";
        private bool _isFocused;
        private string _focusClass = "";
        private string _firstFocusZIndex = "z-index: 2;";
        private string _lastFocusZIndex = "z-index: 2;";
        private bool _mouseDown;
        private bool _mouseDownOnTrack;
        private bool _last = true;
        private bool _isInitialized = false;
        private double _initialFirstValue;
        private double _initialLastValue;
        private Tooltip _toolTipFirst;
        private Tooltip _toolTipLast;
        private bool _customStyleChange;
        private string _customTrackStyle = "";
        private string _customDescriptionStyle = "";
        private string _customFocusStyle = "";
        private string _focusStyle = "";
        private string _customEdgeBorderStyle = "";
        private bool _isDataSet;
        private double _firstValue = double.MinValue;
        private double _lastValue = double.MaxValue;
        private bool _tooltipVisible;
        private bool _tooltipVisibleSet;
        private bool _tooltipLastVisible;
        private bool _tooltipFirstVisible;
        private bool _tooltipPlacementSet;
        private OneOf<Color, string> _fontColor;
        private string _fontColorAsString = "";
        private string _colorAsString = "";
        private OneOf<Color, string> _color;
        OneOf<Color, string> _focusColor;
        string _focusColorAsString = "";
        OneOf<Color, string> _focusBorderColor;
        private string _focusBorderColorAsString = "";
        private (double, double) _value;
        private MultiRangeSlider _parent;
        private bool _hasAttachedEdgeWithGap;
        private bool _hasAttachedEdge;
        private Placement _tooltipPlacement;
        private double _trackedClientX;
        private double _trackedClientY;
        private double _trackedClientWidth;
        private double _trackedClientHeight;
        private bool _shouldRender = true;
        private bool _hasToolTip = true;
        private bool _hasToolTipSet;
        private double _distanceToFirstHandle;
        private double _distanceToLastHandle;
        private string _attachedFirstHandleClass = "";
        private string _attachedLastHandleClass = "";
        /// <summary>
        /// Used to evaluate if OnAfterChange needs to be called
        /// </summary>
        private (double, double) _valueCache;

        /// <summary>
        /// The maximum value the slider can slide to
        /// </summary>
        internal double Max => Parent.Max;

        /// <summary>
        /// The minimum value the slider can slide to
        /// </summary>

        internal double Min => Parent.Min;

        private string LastHandleStyleFormat
        {
            get
            {
                if (Parent.Reverse)
                {
                    if (Parent.Vertical)
                    {
                        return "bottom: auto; top: {0};";
                    }
                    else
                    {
                        return "right: {0}; left: auto; transform: translateX(50%);";
                    }
                }
                else
                {
                    if (Parent.Vertical)
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

        private string FirstHandleStyleFormat
        {
            get
            {
                if (Parent.Reverse)
                {
                    if (Parent.Vertical)
                    {
                        return "bottom: auto; top: {0};";
                    }
                    else
                    {
                        return "right: {0}; left: auto; transform: translateX(50%);";
                    }
                }
                else
                {
                    if (Parent.Vertical)
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
                if (Parent.Reverse)
                {
                    if (Parent.Vertical)
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
                    if (Parent.Vertical)
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


        protected static readonly EventCallbackFactory CallbackFactory = new EventCallbackFactory();

        /// <summary>
        /// Used to figure out how much to move first and last when range is moved
        /// </summary>
        internal bool IsRangeDragged { get; set; }
        internal bool HasAttachedEdge
        {
            get => _hasAttachedEdge;
            set
            {
                _hasAttachedEdge = value;
                Parent.HasAttachedEdges = value;
            }
        }

        internal bool HasAttachedEdgeWithGap
        {
            get => _hasAttachedEdgeWithGap;
            set
            {
                _hasAttachedEdgeWithGap = value;
                HasAttachedEdge = value;

            }
        }
        internal RangeEdge AttachedHandleNo { get; set; }
        internal RangeEdge HandleNoRequestingAttaching { get; set; }
        internal RangeItem AttachedItem { get; set; }
        internal Action ChangeAttachedItem { get; set; }
        internal double GapDistance { get; private set; }
        internal bool Master { get; set; }
        internal bool Slave { get; set; }

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        #region Parameters
        [CascadingParameter(Name = "Range")]
        public MultiRangeSlider Parent { get => _parent; set => _parent = value; }

        /// <summary>
        /// Color of the range item.
        /// </summary>
        [Parameter]
        public OneOf<Color, string> Color
        {
            get => _color;
            set
            {
                if (!_color.Equals(value))
                {
                    _customStyleChange = true;
                    _colorAsString = GetColorStyle(value, "background-color");
                    _color = value;
                }
            }
        }

        /// <summary>
        /// Data object implementing `AntDesign.IRangeItemData` that will be used to render the ranges.
        /// </summary>
        [Parameter]
        public IRangeItemData Data { get; set; }

        /// <summary>
        /// The default value of slider. 
        /// </summary>
        [Parameter]
        public (double, double) DefaultValue { get; set; }

        /// <summary>
        /// Text visible on the range.
        /// </summary>
        [Parameter]
        public string Description { get; set; }

        /// <summary>
        /// If true, the slider will not be intractable
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        //TODO: remove when two-tone color algorithm is going to be applied
        /// <summary>
        /// Color of the range item's border when focused.
        /// </summary>
        [Parameter]
        public OneOf<Color, string> FocusBorderColor
        {
            get => _focusBorderColor;
            set
            {
                if (!_focusBorderColor.Equals(value))
                {
                    _customStyleChange = true;
                    _focusBorderColorAsString = GetColorStyle(value, "border-color");
                    _focusBorderColor = value;
                }
            }
        }

        /// <summary>
        /// Color of the range item when focused.
        /// </summary>
        [Parameter]
        public OneOf<Color, string> FocusColor
        {
            get => _focusColor;
            set
            {
                if (!_focusColor.Equals(value))
                {
                    _customStyleChange = true;
                    _focusColorAsString = GetColorStyle(value, "background-color");
                    _focusColor = value;
                }
            }
        }

        /// <summary>
        /// Color of the text visible on the range.
        /// </summary>
        [Parameter]
        public OneOf<Color, string> FontColor
        {
            get => _fontColor;
            set
            {
                if (!_fontColor.Equals(value))
                {
                    _customStyleChange = true;
                    _fontColorAsString = GetColorStyle(value, "color");
                    _fontColor = value;
                }
            }
        }

        /// <summary>
        /// Set the range's icon 
        /// </summary>
        [Parameter]
        public string Icon { get; set; }

        [Parameter]
        public bool HasTooltip
        {
            get => _hasToolTip;
            set
            {
                _hasToolTip = value;
                _hasToolTipSet = true;
            }
        }

        /// <summary>
        /// Fire when changes are done (onmouseup and onkeyup).
        /// </summary>
        [Parameter]
        public EventCallback<(double, double)> OnAfterChange { get; set; }

        /// <summary>
        /// Called when the user changes one of the values.
        /// </summary>
        [Parameter]
        public EventCallback<(double, double)> OnChange { get; set; }

        /// <summary>
        /// Set `Tooltip` display position.
        /// </summary>
        [Parameter]
        public Placement TooltipPlacement
        {
            get => _tooltipPlacement;
            set
            {
                _tooltipPlacementSet = true;
                _tooltipPlacement = value;
            }
        }

        /// <summary>
        /// If true, Tooltip will show always, or it will not show anyway, even if dragging or hovering.
        /// </summary>
        [Parameter]
        public bool TooltipVisible
        {
            get { return _tooltipVisible; }
            set
            {
                _tooltipVisibleSet = true;
                if (_tooltipVisible != value)
                {
                    _tooltipVisible = value;
                    //ensure parameter loading is not happening because values are changing during mouse moving
                    //otherwise the tooltip will be vanishing when mouse moves out of the edge
                    if (!_mouseDown)
                    {
                        _tooltipLastVisible = _tooltipVisible;
                        _tooltipFirstVisible = _tooltipVisible;
                    }
                }
            }
        }
        #endregion Parameters

        private string GetColorStyle(OneOf<Color, string> color, string colorProperty)
        {
            return color.Match<string>(
                colorValue => $"{colorProperty}: {ColorHelper.GetColor(colorValue)};",
                stringValue => $"{colorProperty}: {stringValue};"
                );
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent.AddRangeItem(this);
            SetPositions();
            SetHasTooltipFromParent();
            SetTooltipPacementFromParent();
            SetTooltipVisibleFromParent();
        }

        internal double FirstValue
        {
            get => _firstValue;
            set
            {
                double candidate = value;
                if (_isInitialized)
                {
                    if (!Slave)
                    {
                        candidate = Clamp(value, Parent.GetFirstEdgeBoundary(Id, RangeEdge.First, AttachedHandleNo), Parent.GetLastEdgeBoundary(Id, RangeEdge.First, AttachedHandleNo));
                    }
                    else if (Parent.AllowOverlapping)
                    {
                        candidate = Clamp(value, Min, Max);
                    }
                }
                if (_firstValue != candidate)
                {
                    ChangeFirstValue(candidate, value);
                }
            }
        }

        private void ChangeFirstValue(double value, double previousValue)
        {
            if (_isInitialized && Parent.OnEdgeMoving is not null &&
                !Parent.OnEdgeMoving.Invoke((range: this, edge: RangeEdge.First, value: value)))
            {
                return;
            }
            _firstValue = value;

            if (previousValue != CurrentValue.Item1)
            {
                CurrentValue = (_firstValue, LastValue);
                RaiseOnChangeCallback();
            }
            if (_isInitialized && Parent.OnEdgeMoved.HasDelegate)
            {
                Parent.OnEdgeMoved.InvokeAsync((range: this, edge: RangeEdge.First, value: value));
            }
            SetPositions();
        }

        private void RaiseOnChangeCallback()
        {
            if (_isInitialized)
            {
                if (OnChange.HasDelegate)
                {
                    OnChange.InvokeAsync(CurrentValue);
                }
                if (Parent.OnChange.HasDelegate)
                {
                    Parent.OnChange.InvokeAsync(CurrentValue);
                }
                if (_isDataSet && Data.OnChange.HasDelegate)
                {
                    Data.OnChange.InvokeAsync(CurrentValue);
                }
            }
        }

        // the default non-range value
        internal double LastValue
        {
            get => _lastValue;
            set
            {
                double candidate = value;
                if (_isInitialized)
                {
                    if (!Slave)
                    {
                        candidate = Clamp(value, Parent.GetFirstEdgeBoundary(Id, RangeEdge.Last, AttachedHandleNo), Parent.GetLastEdgeBoundary(Id, RangeEdge.Last, AttachedHandleNo));
                    }
                    else if (Parent.AllowOverlapping)
                    {
                        candidate = Clamp(value, Min, Max);
                    }
                }
                if (_lastValue != candidate)
                {
                    ChangeLastValue(candidate, value);
                }
            }
        }

        private void ChangeLastValue(double value, double previousValue)
        {
            if (_isInitialized && Parent.OnEdgeMoving is not null &&
                !Parent.OnEdgeMoving.Invoke((range: this, edge: RangeEdge.Last, value: value)))
            {
                return;
            }
            _lastValue = value;

            if (previousValue != CurrentValue.Item2)
            {
                CurrentValue = (FirstValue, _lastValue);
                RaiseOnChangeCallback();
            }
            if (_isInitialized && Parent.OnEdgeMoved.HasDelegate)
            {
                Parent.OnEdgeMoved.InvokeAsync((range: this, edge: RangeEdge.Last, value: value));
            }
            SetPositions();
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
            return Parent.GetNearestStep(value);
        }

        internal void SetHasTooltipFromParent()
        {
            if (!_hasToolTipSet)
            {
                _hasToolTip = Parent.HasTooltip;
            }
        }

        internal void SetTooltipPacementFromParent()
        {
            if (!_tooltipPlacementSet)
            {
                _tooltipPlacement = Parent.TooltipPlacement;
            }
        }

        internal void SetTooltipVisibleFromParent()
        {
            if (!_tooltipVisibleSet)
            {
                _tooltipVisible = Parent.TooltipVisible;
                if (!_mouseDown)
                {
                    _tooltipLastVisible = _tooltipVisible;
                    _tooltipFirstVisible = _tooltipVisible;
                    if (_toolTipLast != null)
                    {
                        if (_tooltipVisible)
                        {
                            InvokeAsync(async () => await _toolTipFirst.Show());
                            InvokeAsync(async () => await _toolTipLast.Show());
                        }
                        else
                        {
                            InvokeAsync(async () => await _toolTipFirst.Hide());
                            InvokeAsync(async () => await _toolTipLast.Hide());
                        }
                    }
                }
            }
        }

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
            var dict = parameters.ToDictionary();
            if (!_isInitialized)
            {
                MultiRangeSlider parent;
                if (!dict.ContainsKey(nameof(Parent)))
                {
                    throw new ArgumentNullException($"{nameof(RangeItem)} cannot be used independently. It has to be nested inside {nameof(MultiRangeSlider)}.");
                }
                else parameters.TryGetValue(nameof(Parent), out parent);
                {
                    Parent = parent;
                }

                if (dict.ContainsKey(nameof(Data)))
                {
                    Data = parameters.GetValueOrDefault<IRangeItemData>(nameof(Data), default);
                    _isDataSet = true;
                    ApplyData(true);
                }
                if (!dict.ContainsKey(nameof(Value)))
                {
                    (double, double) defaultValue = parameters.GetValueOrDefault(nameof(DefaultValue), (0d, 0d));
                    FirstValue = defaultValue.Item1;
                    LastValue = defaultValue.Item2;
                }
                else
                {
                    FirstValue = CurrentValue.Item1;
                    LastValue = CurrentValue.Item2;
                }

            }

            base.SetParametersAsync(parameters);

            if (!_isInitialized)
            {
                if (!dict.ContainsKey(nameof(TooltipPlacement)))
                {
                    if (Parent.Vertical && !_tooltipPlacementSet)
                        _tooltipPlacement = Placement.Right;
                    else
                        _tooltipPlacement = Placement.Top;
                }
            }

            _isInitialized = true;
            return Task.CompletedTask;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            ClassMapper.Clear()
                .Add($"{PreFixCls}-item")
                .If($"{PreFixCls}-disabled", () => Disabled || _parent.Disabled)
                .If($"{PreFixCls}-vertical", () => Parent.Vertical)
                //.If($"{PreFixCls}-with-marks", () => Parent.Marks != null)
                .If($"{PreFixCls}-rtl", () => RTL);

            if (_isInitialized)
            {
                ApplyData();
            }
            SetCustomStyle();
        }

        private void ApplyData(bool force = false)
        {
            if (!_isDataSet)
            {
                return;
            }
            if (force || Data.Description != Description)
            {
                Description = Data.Description;
            }
            if (force || Data.Icon != Icon)
            {
                Icon = Data.Icon;
            }
            if (force || Data.FontColor.Equals(FontColor))
            {
                FontColor = Data.FontColor;
            }
            if (force || Data.FocusColor.Equals(FocusColor))
            {
                FocusColor = Data.FocusColor;
            }
            if (force || Data.FocusBorderColor.Equals(FocusBorderColor))
            {
                FocusBorderColor = Data.FocusBorderColor;
            }
            if (force || Data.Color.Equals(Color))
            {
                Color = Data.Color;
            }
        }

        private void SetCustomStyle()
        {
            if (_customStyleChange)
            {
                if (string.IsNullOrWhiteSpace(FontColor.Value.ToString()))
                {
                    _customDescriptionStyle = "";
                }
                else
                {
                    _customDescriptionStyle = _fontColorAsString;
                }
                if (string.IsNullOrWhiteSpace(Color.Value.ToString()))
                {
                    _customTrackStyle = "";
                    _customEdgeBorderStyle = "";
                }
                else
                {
                    _customTrackStyle = _colorAsString;
                    _customEdgeBorderStyle = GetColorStyle(_color, "border-color");
                }
                if (!string.IsNullOrWhiteSpace(FocusColor.Value.ToString()) || !string.IsNullOrWhiteSpace(FocusBorderColor.Value.ToString()))
                {
                    _customFocusStyle = _focusBorderColorAsString + _focusColorAsString;
                }
                else
                {
                    _customFocusStyle = "";
                }
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                DomEventListener.AddShared<JsonElement>("window", "beforeunload", Reloading);
            }
            base.OnAfterRender(firstRender);
        }

        /// <summary>
        /// Indicates that a page is being refreshed
        /// </summary>
        private bool _isReloading;
        private void Reloading(JsonElement jsonElement) => _isReloading = true;

        protected override void Dispose(bool disposing)
        {
            DomEventListener.Dispose();
            Parent.RemoveRangeItem(this);
            base.Dispose(disposing);
        }

        private async Task OnKeyDown(KeyboardEventArgs e, RangeEdge handle)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            var key = e.Key.ToUpperInvariant();
            double modifier = 0;
            if (Parent.Vertical)
            {
                if (key == "ARROWUP")
                {
                    modifier = 1;
                }
                if (key == "ARROWDOWN")
                {
                    modifier = -1;
                }
            }
            else
            {
                if (key == "ARROWLEFT")
                {
                    modifier = -1;
                }
                if (key == "ARROWRIGHT")
                {
                    modifier = 1;
                }
            }
            if (modifier != 0)
            {
                _valueCache = _value;
                if (Parent.Step is not null)
                {
                    if (handle == default) //moving range
                    {
                        double newFirst = FirstValue + (Parent.Step.Value * modifier);
                        double newLast = LastValue + (Parent.Step.Value * modifier);
                        await KeyMoveByValues(newFirst, newLast);
                    }
                    else //moving one of the edges
                    {
                        double oldValue = handle == RangeEdge.First ? FirstValue : LastValue;
                        await KeyMoveByValue(handle, oldValue + (Parent.Step.Value * modifier));
                    }
                }
                else
                {
                    if (handle == default)
                    {
                        if (!(FirstValue == Min && modifier < 0) && !(LastValue == Max && modifier > 0))
                        {
                            double newFirst = Parent.Marks.Select(m => Math.Abs(m.Key + modifier * FirstValue)).Skip(1).First();
                            double newLast = Parent.Marks.Select(m => Math.Abs(m.Key + modifier * LastValue)).Skip(1).First();
                            await KeyMoveByValues(newFirst, newLast);
                        }
                    }
                    else
                    {
                        double oldValue = handle == RangeEdge.First ? FirstValue : LastValue;
                        double newValue = Parent.Marks.Select(m => Math.Abs(m.Key + modifier * oldValue)).Skip(1).First();
                        await KeyMoveByValue(handle, newValue);
                    }
                }
            }
        }

        private Task OnKeyUp(KeyboardEventArgs e)
        {
            if (OnAfterChange.HasDelegate || Parent.OnAfterChange.HasDelegate || (_isDataSet && Data.OnAfterChange.HasDelegate))
            {
                if (e == null) throw new ArgumentNullException(nameof(e));
                var key = e.Key.ToUpperInvariant();
                bool raiseEvent = false;
                if (Parent.Vertical)
                {
                    raiseEvent = key == "ARROWUP" || key == "ARROWDOWN";
                }
                else if (!Parent.Vertical)
                {
                    raiseEvent = key == "ARROWLEFT" || key == "ARROWRIGHT";
                }
#pragma warning disable CS4014 // Does not return anything, fire & forget
                RaiseOnAfterChangeCallback(() => raiseEvent && _valueCache != _value);
#pragma warning restore CS4014 // Does not return anything, fire & forget
            }
            return Task.CompletedTask;

        }

        private async Task KeyMoveByValues(double newFirst, double newLast)
        {
            double firstCandidate = Clamp(newFirst, Parent.GetFirstEdgeBoundary(Id, RangeEdge.First, AttachedHandleNo), Parent.GetLastEdgeBoundary(Id, RangeEdge.First, AttachedHandleNo));
            double lastCandidate = Clamp(newLast, Parent.GetFirstEdgeBoundary(Id, RangeEdge.Last, AttachedHandleNo), Parent.GetLastEdgeBoundary(Id, RangeEdge.Last, AttachedHandleNo));

            if (firstCandidate == newFirst && lastCandidate == newLast)
            {
                ChangeFirstValue(firstCandidate, FirstValue);
                ChangeLastValue(lastCandidate, LastValue);
                var tooltipFirst = _toolTipFirst.Show();
                var tooltipLast = _toolTipLast.Show();
                await Task.WhenAll(tooltipFirst, tooltipLast);
            }
        }

        private async Task KeyMoveByValue(RangeEdge handle, double value)
        {
            if (FirstValue == LastValue)
            {
                if (handle == RangeEdge.First)
                {
                    await SwitchToLastHandle(value);
                }
                else
                {
                    await SwitchToFirstHandle(value);
                }
            }
            else
            {
                if (handle == RangeEdge.First)
                {
                    FirstValue = value;
                    await _toolTipFirst.Show();
                }
                else
                {
                    LastValue = value;
                    await _toolTipLast.Show();
                }
                if (AttachedHandleNo == handle)
                {
                    ChangeAttachedItem?.Invoke();
                }
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                if (_toolTipLast != null && Parent.HasTooltip)
                {
                    _lastHandle = _toolTipLast.Ref;
                    if (_toolTipFirst != null)
                    {
                        _firstHandle = _toolTipFirst.Ref;
                    }
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task<(double, double, double width, double height)> GetSliderDimensions(ElementReference reference)
        {
            _sliderDom = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, reference);
            return ((double)(Parent.Vertical ? _sliderDom.AbsoluteTop : _sliderDom.AbsoluteLeft),
                    (double)(Parent.Vertical ? _sliderDom.ClientHeight : _sliderDom.ClientWidth),
                    (double)_sliderDom.ClientWidth, (double)_sliderDom.ClientHeight);

        }

        internal void SetFocus(bool isFocused)
        {
            if (_isFocused == isFocused)
            {
                return;
            }
            _isFocused = isFocused;

            if (_isFocused)
            {
                _focusClass = $"{PreFixCls}-track-focus";
                _focusStyle = _customFocusStyle;
                if (!(HasAttachedEdge && AttachedHandleNo == RangeEdge.First))
                {
                    _firstFocusZIndex = "z-index: 3;"; //just below default overlay zindex
                }
                if (!(HasAttachedEdge && AttachedHandleNo == RangeEdge.Last))
                {
                    _lastFocusZIndex = "z-index: 3;";
                }
            }
            else
            {
                _focusClass = "";
                _focusStyle = "";
                if (!(HasAttachedEdge && AttachedHandleNo == RangeEdge.First))
                {
                    _firstFocusZIndex = "z-index: 2;";
                }
                if (!(HasAttachedEdge && AttachedHandleNo == RangeEdge.Last))
                {
                    _lastFocusZIndex = "z-index: 2;";
                }
            }
            if (!isFocused)
            {
                StateHasChanged();
            }
        }

        private void OnDoubleClick(RangeEdge handle)
        {
            //TODO: bUnit: attach overlapping edge when opposite overlapping edge already attached
            if (!HasAttachedEdge || AttachedHandleNo == GetOppositeEdge(handle))
            {
                RangeItem overlappingEdgeCandidate = handle == RangeEdge.First ? Parent.GetPrevNeighbour(Id) : Parent.GetNextNeighbour(Id);
                if (overlappingEdgeCandidate is null && !Parent.AllowOverlapping) //will be null when there are no other items or edge is closes to either Min or Max
                {
                    ResetAttached();
                    return;
                }
                bool isAttached;
                if (IsEdgeOverlapping(handle, overlappingEdgeCandidate))
                {
                    isAttached = AttachOverlappingEdges(handle, overlappingEdgeCandidate, false);
                }
                else
                {
                    isAttached = AttachNotOverlappingEdges(handle, false);
                }
                if (_isInitialized && isAttached && Parent.OnEdgeAttached.HasDelegate)
                {
                    if (handle == RangeEdge.First)
                    {
                        Parent.OnEdgeAttached.InvokeAsync((first: AttachedItem, last: this));
                    }
                    else
                    {
                        Parent.OnEdgeAttached.InvokeAsync((first: this, last: AttachedItem));
                    }
                }
                return;
            }
            ResetAttached();
        }

        /// <summary>
        /// Will attach 2 edges. If <see cref="MultiRangeSlider.AllowOverlapping"/> is set to false,
        /// will only allow attaching neighboring edges.
        /// </summary>
        /// <param name="currentRangeEdge">Which edge of the current <see cref="RangeItem "/> needs to be attached.</param>
        /// <param name="attachToRange">RangeItem that will be attached</param>
        /// <param name="attachToRangeEdge">Which edge of the requested <see cref="RangeItem "/> needs to be attached.</param>
        /// <param name="detachExisting">Whether to detach if already attached</param>
        /// <returns>Whether attaching was successful. Returns false if attachment already exists.</returns>
        public bool AttachEdges(RangeEdge currentRangeEdge, RangeItem attachToRange, RangeEdge attachToRangeEdge, bool detachExisting = false)
        {
            if (Parent.HasAttachedEdges)
            {
                if (!detachExisting)
                {
                    return false;
                }
                else if (AttachedItem is not null && AttachedItem.Id == attachToRange.Id && AttachedHandleNo == currentRangeEdge && AttachedItem.AttachedHandleNo == attachToRangeEdge)
                {
                    return true; //are already attached
                }
                ResetAttached(true);
            }
            var currentRangeAttachResult = AttachFirstNotOverlappingEdge(currentRangeEdge, true);
            if (!currentRangeAttachResult)
            {
                return false;
            }
            attachToRange.AttachNotOverlappingEdges(attachToRangeEdge, true);

            return attachToRange.HasAttachedEdge;
        }

        /// <summary>
        /// Will initiate attaching. Same as double clicking on an edge (that is not overlapping
        /// with another edge).
        /// </summary>
        /// <param name="currentRangeEdge">Which edge of the current <see cref="RangeItem "/> needs to be attached.</param>
        /// <param name="detachExisting">Whether to detach if already attached</param>
        /// <returns>Whether attaching was successful.</returns>
        public bool AttachSingle(RangeEdge currentRangeEdge, bool detachExisting = false)
        {
            if (Parent.HasAttachedEdges)
            {
                if (!detachExisting)
                {
                    return false;
                }
                ResetAttached(true);
            }

            return AttachNotOverlappingEdges(currentRangeEdge, true, false);
        }

        /// <summary>
        /// Will attach overlapping edges. Same as double clicking on overlapping edges.
        /// </summary>
        /// <param name="currentRangeEdge">Which edge of the current <see cref="RangeItem "/> needs to be attached.</param>
        /// <param name="detachExisting">Whether to detach if already attached</param>
        /// <returns>Whether attaching was successful. Returns true if already attached.</returns>
        public bool AttachOverlappingEdges(RangeEdge currentRangeEdge, bool detachExisting = false)
        {
            if (Parent.HasAttachedEdges)
            {
                if (!detachExisting)
                {
                    return false;
                }
            }

            RangeItem overlappingEdgeCandidate = currentRangeEdge == RangeEdge.First ? Parent.GetPrevNeighbour(Id) : Parent.GetNextNeighbour(Id);
            if (overlappingEdgeCandidate is null) //will be null when there are no other items or edge is closes to either Min or Max
            {
                return false;
            }
            if (Parent.HasAttachedEdges &&
                (
                    Parent.ItemRequestingAttach.Id == overlappingEdgeCandidate.Id
                    ||
                    Parent.ItemRespondingToAttach.Id == overlappingEdgeCandidate.Id
                ))
            {
                return true; //is already attached
            }
            else
            {
                ResetAttached(true);
            }

            if (IsEdgeOverlapping(currentRangeEdge, overlappingEdgeCandidate))
            {
                AttachOverlappingEdges(currentRangeEdge, overlappingEdgeCandidate, true);
            }
            return HasAttachedEdge;
        }

        /// <summary>
        /// Detaches edge(s).
        /// </summary>        
        /// <returns>Whether detachment was successful. Returns true if no attachment existed.</returns>
        public bool DetachEdges()
        {
            if (Parent.HasAttachedEdges || AttachedItem is not null || HandleNoRequestingAttaching != default)
            {
                ResetAttached(HandleNoRequestingAttaching != default);
                if (AttachedItem is not null)
                {
                    DirectReset();
                }
                return true;
            }
            return false;
        }

        public RangeEdge GetAttachedEdge() => AttachedHandleNo;
        internal bool AttachNotOverlappingEdges(RangeEdge handle, bool outsideCall, bool detachExisting = true)
        {
            if (Parent.ItemRequestingAttach is null)
            {
                return AttachFirstNotOverlappingEdge(handle, outsideCall);
            }
            if (Parent.AllowOverlapping)
            {
                if (Parent.ItemRespondingToAttach is null && Parent.ItemRequestingAttach.Id != Id) //do not allow same item edge locks, use dragging
                {
                    return AttachSecondNotOverlappingEdge(handle, outsideCall,
                        handle == Parent.ItemRequestingAttach.HandleNoRequestingAttaching);
                }
                else if (detachExisting)
                {
                    ResetAttached(true);
                    return false;
                }
            }
            else
            {
                if (AreEdgesNeighbours(handle))
                {
                    return AttachSecondNotOverlappingEdge(handle, outsideCall);
                }
                else if (detachExisting)
                {
                    ResetAttached(true);
                    return false;
                }
            }
            return true;
        }

        private bool ShouldCancelAttaching(bool outsideCall, RangeEdge handle, RangeItem currentItem, RangeItem attachedItem)
        {
            if (_isInitialized && !outsideCall && Parent.OnEdgeAttaching is not null)
            {
                bool allowAttaching;
                bool detachExistingOnCancel;
                if (handle == RangeEdge.First)
                {
                    (allowAttaching, detachExistingOnCancel) = Parent.OnEdgeAttaching((first: currentItem, last: attachedItem));
                }
                else
                {
                    (allowAttaching, detachExistingOnCancel) = Parent.OnEdgeAttaching((first: attachedItem, last: currentItem));
                }
                if (!allowAttaching)
                {
                    if (detachExistingOnCancel)
                    {
                        ResetAttached(true);
                    }
                    return true;
                }
            }
            return false;

        }

        private bool AttachFirstNotOverlappingEdge(RangeEdge handle, bool outsideCall)
        {
            if (ShouldCancelAttaching(outsideCall, handle, this, null))
            {
                return false;
            }

            Parent.ItemRequestingAttach = this;
            HandleNoRequestingAttaching = handle;
            SetLockEdgeStyle(handle);
            return true;
        }

        private bool AttachSecondNotOverlappingEdge(RangeEdge handle, bool outsideCall, bool isSameHandle = false)
        {
            if (ShouldCancelAttaching(outsideCall, handle, Parent.ItemRequestingAttach, this)) //reversed order is intentional
            {
                return false;
            }

            Parent.ItemRespondingToAttach = this;
            Master = true;
            Parent.ItemRequestingAttach.Slave = true;
            HasAttachedEdgeWithGap = true;
            Parent.ItemRequestingAttach.HasAttachedEdgeWithGap = true;
            Parent.ItemRequestingAttach.AttachedHandleNo = Parent.ItemRequestingAttach.HandleNoRequestingAttaching;
            AttachedHandleNo = handle;
            AttachedItem = Parent.ItemRequestingAttach;
            Parent.ItemRequestingAttach.AttachedItem = this;
            if (!isSameHandle)
            {
                if (handle == RangeEdge.First)
                {
                    GapDistance = this.FirstValue - AttachedItem.LastValue;
                    ChangeAttachedItem = () => AttachedItem.LastValue = this.FirstValue - GapDistance;
                    AttachedItem.ChangeAttachedItem = () => this.FirstValue = AttachedItem.LastValue + GapDistance;
                }
                else
                {
                    GapDistance = AttachedItem.FirstValue - this.LastValue;
                    ChangeAttachedItem = () => AttachedItem.FirstValue = this.LastValue + GapDistance;
                    AttachedItem.ChangeAttachedItem = () => this.LastValue = AttachedItem.FirstValue - GapDistance;
                }
                Parent.ItemRequestingAttach.SetLockEdgeStyle(GetOppositeEdge(handle), true, true);
            }
            else
            {
                if (handle == RangeEdge.First)
                {
                    GapDistance = this.FirstValue - AttachedItem.FirstValue;
                    ChangeAttachedItem = () => AttachedItem.FirstValue = this.FirstValue - GapDistance;
                    AttachedItem.ChangeAttachedItem = () => this.FirstValue = AttachedItem.FirstValue + GapDistance;
                }
                else
                {
                    GapDistance = AttachedItem.LastValue - this.LastValue;
                    ChangeAttachedItem = () => AttachedItem.LastValue = this.LastValue + GapDistance;
                    AttachedItem.ChangeAttachedItem = () => this.LastValue = AttachedItem.LastValue - GapDistance;
                }
                Parent.ItemRequestingAttach.SetLockEdgeStyle(handle, true, true);
            }
            AttachedItem.GapDistance = GapDistance;
            SetLockEdgeStyle(handle, true);
            return true;
        }

        private double CalculateGapDistance()
        {
            if (AttachedHandleNo != AttachedItem.AttachedHandleNo)
            {
                if (AttachedHandleNo == RangeEdge.First)
                {
                    return this.FirstValue - AttachedItem.LastValue;
                }
                return AttachedItem.FirstValue - this.LastValue;
            }
            if (AttachedHandleNo == RangeEdge.First)
            {
                return this.FirstValue - AttachedItem.FirstValue;
            }
            return AttachedItem.LastValue - this.LastValue;
        }

        private bool AreEdgesNeighbours(RangeEdge handle)
        {
            if (Parent.ItemRespondingToAttach is not null)
            {
                //nothing to attach, since is already attached
                return false;
            }
            if (Parent.ItemRequestingAttach.HandleNoRequestingAttaching == handle)
            {
                //if same edge, then not a neighbor
                return false;
            }

            return IsPrevNeighbor(handle)
                || IsNextNeighbor(handle);
        }

        private bool IsNextNeighbor(RangeEdge handle)
        {
            return handle == RangeEdge.Last
                && Parent.ItemRequestingAttach.Id == Parent.GetNextNeighbour(Id).Id;
        }

        private bool IsPrevNeighbor(RangeEdge handle)
        {
            return handle == RangeEdge.First
                && Parent.ItemRequestingAttach.Id == Parent.GetPrevNeighbour(Id).Id;
        }

        /// <summary>
        /// Evaluates overlapping edges (neighboring)
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="overlappingEdgeCandidate"></param>
        /// <returns></returns>
        private bool IsEdgeOverlapping(RangeEdge handle, RangeItem overlappingEdgeCandidate)
        {
            if (overlappingEdgeCandidate is null)
            {
                return false;
            }

            return IsOverlappingWithFirstEdge(handle, overlappingEdgeCandidate)
                || IsOverlappingWithNextEdge(handle, overlappingEdgeCandidate);

        }

        private bool IsOverlappingWithFirstEdge(RangeEdge handle, RangeItem overlappingEdgeCandidate)
        {
            return handle == RangeEdge.First && overlappingEdgeCandidate.LastValue == FirstValue;
        }

        private bool IsOverlappingWithNextEdge(RangeEdge handle, RangeItem overlappingEdgeCandidate)
        {
            return handle == RangeEdge.Last && overlappingEdgeCandidate.FirstValue == LastValue;
        }

        private bool AttachOverlappingEdges(RangeEdge handle, RangeItem item, bool outsideCall)
        {
            if (ShouldCancelAttaching(outsideCall, handle, this, item))
            {
                return false;
            }
            if (Parent.HasAttachedEdges)
            {
                if (!ResetAttached(true))
                {
                    return false;
                }
            }

            AttachedItem = item;
            HasAttachedEdge = true;
            AttachedHandleNo = handle;
            Master = true;

            AttachedItem.Slave = true;
            AttachedItem.HasAttachedEdge = true;
            AttachedItem.AttachedItem = this;
            AttachedItem.AttachedHandleNo = GetOppositeEdge(handle);

            Parent.ItemRequestingAttach = this;
            Parent.ItemRespondingToAttach = AttachedItem;

            if (handle == RangeEdge.First)
            {
                ChangeAttachedItem = () => AttachedItem.LastValue = this.FirstValue;
                AttachedItem.ChangeAttachedItem = () => this.FirstValue = AttachedItem.LastValue;
            }
            else
            {
                ChangeAttachedItem = () => AttachedItem.FirstValue = this.LastValue;
                AttachedItem.ChangeAttachedItem = () => this.LastValue = AttachedItem.FirstValue;
            }
            SetLockEdgeStyle(handle, true);
            Parent.HasAttachedEdges = true;
            return true;
        }

        private static RangeEdge GetOppositeEdge(RangeEdge edge)
        {
            if (edge == RangeEdge.First)
            {
                return RangeEdge.Last;
            }
            return RangeEdge.First;
        }

        private void SetLockEdgeStyle(RangeEdge handle, bool locked = false, bool requestStateChange = false)
        {
            if (handle == RangeEdge.First)
            {
                ApplyLockEdgeStyle(locked, ref _attachedFirstHandleClass, ref _firstHandleFill, ref _firstFocusZIndex);
            }
            else
            {
                ApplyLockEdgeStyle(locked, ref _attachedLastHandleClass, ref _lastHandleFill, ref _lastFocusZIndex);
            }
            _shouldRender = true;
            if (requestStateChange)
            {
                StateHasChanged();
            }
        }

        private static void ApplyLockEdgeStyle(bool locked, ref string attachHandleClass, ref RenderFragment handleFill, ref string focusIndex)
        {
            if (locked)
            {
                attachHandleClass = $" {PreFixCls}-handle-lock {PreFixCls}-handle-lock-closed";
                handleFill = _locked;
            }
            else
            {
                attachHandleClass = $" {PreFixCls}-handle-lock {PreFixCls}-handle-lock-open";
                handleFill = _unlocked;
            }
            focusIndex = "z-index: 4;";
        }

        internal void ResetLockEdgeStyle(bool requestStateChange)
        {
            _shouldRender = true;
            _attachedFirstHandleClass = "";
            _attachedLastHandleClass = "";
            _firstHandleFill = null;
            _lastHandleFill = null;
            _firstFocusZIndex = "z-index: 2;";
            _lastFocusZIndex = "z-index: 2;";
            if (requestStateChange)
            {
                StateHasChanged();
            }
        }

        private bool ResetAttached(bool forceReset = false)
        {
            if (!Parent.HasAttachedEdges && !HasAttachedEdge && !forceReset)
            {
                return true; //nothing to detach, don't fail
            }
            RangeItem first = null, last = null;
            if (Parent.OnEdgeDetaching is not null || Parent.OnEdgeDetached.HasDelegate)
            {
                Parent.GetAttachedInOrder(out first, out last);
                if (Parent.OnEdgeDetaching is not null && !Parent.OnEdgeDetaching.Invoke((first, last)))
                {
                    return false;
                }
            }

            bool requestStateChange = Parent.HasAttachedEdges && !HasAttachedEdge;

            //reset all attached
            if (HasAttachedEdgeWithGap || forceReset)
            {
                ResetNotOverlapping(Parent.ItemRequestingAttach, Id);
                Parent.ItemRequestingAttach = null;
                ResetNotOverlapping(Parent.ItemRespondingToAttach, Id);
                Parent.ItemRespondingToAttach = null;
                //re-sort, because order could have been altered
                //and proper order is needed to pick-up attaching on 
                //overlapping edges
                Parent.SortRangeItems();
            }
            else
            {
                DirectReset();
            }
            SetFocus(_isFocused);
            HasAttachedEdgeWithGap = false;
            if (Parent.OnEdgeDetached.HasDelegate)
            {
                Parent.OnEdgeDetached.InvokeAsync((first, last));
            }
            return true;
        }

        private void DirectReset()
        {
            ResetLockEdgeStyle(false);
            Parent.ItemRequestingAttach = null;
            Parent.ItemRespondingToAttach = null;

            Master = false;
            if (AttachedItem is not null)
            {
                AttachedItem.Slave = false;
                AttachedItem.HasAttachedEdge = false;
                AttachedItem.AttachedItem = null;
                AttachedItem.AttachedHandleNo = 0;
                AttachedItem.ChangeAttachedItem = default;
                AttachedItem.ResetLockEdgeStyle(false);

                AttachedItem = default;
            }
            AttachedHandleNo = 0;

            ChangeAttachedItem = default;
        }

        private static void ResetNotOverlapping(RangeItem item, string currentRangeId)
        {
            if (item is not null)
            {
                item.ResetLockEdgeStyle(item.Id != currentRangeId);
                item.ChangeAttachedItem = default;
                item.HandleNoRequestingAttaching = 0;
                item.AttachedHandleNo = 0;
                item.AttachedItem = null;
                item.HasAttachedEdgeWithGap = false;
                item.GapDistance = 0;
                item.Master = false;
                item.Slave = false;
                item.AttachedItem = null;
            }
        }

        private async Task OnRangeItemClick(MouseEventArgs args)
        {
            _mouseDownOnTrack = !Disabled && !Parent.Disabled;
            if (!_mouseDownOnTrack)
            {
                return;
            }
            if (!_isFocused)
            {
                SetFocus(true);
                Parent.SetRangeItemFocus(this, true);
            }
            _initialFirstValue = _firstValue;
            _initialLastValue = _lastValue;
            _trackedClientX = args.ClientX;
            _trackedClientY = args.ClientY;
            if (_toolTipLast != null)
            {
                _tooltipLastVisible = true;
                _tooltipFirstVisible = true;
                var tooltipFirst = _toolTipFirst.Show();
                var tooltipLast = _toolTipLast.Show();
                await Task.WhenAll(tooltipFirst, tooltipLast);
            }

            //evaluate clicked position in respect to each edge
            await AddJsEvents();
            (double sliderOffset, double sliderLength, double sliderWidth, double sliderHeight)
                = await GetSliderDimensions(Parent._railRef);
            _trackedClientWidth = sliderWidth;
            sliderHeight = _trackedClientHeight;
#if NET_6_0_OR_GRATER
            double clickedValue = CalculateNewHandleValue(Parent.Vertical ? args.PageY : args.PageX, sliderOffset, sliderLength);
#else
            double clickedValue = CalculateNewHandleValue(Parent.Vertical ? args.ClientY : args.ClientX, sliderOffset, sliderLength);
#endif
            _distanceToFirstHandle = clickedValue - FirstValue;
            _distanceToLastHandle = LastValue - clickedValue;
            _valueCache = _value;
            if (HasAttachedEdge && !Master)
            {
                SetAsMaster();
            }
        }

        private async Task OnMouseDownEdge(MouseEventArgs args, RangeEdge edge)
        {
            _mouseDown = !Disabled && !Parent.Disabled;
            if (!_mouseDown)
            {
                return;
            }
            await AddJsEvents();
            _mouseDown = true;
            SetFocus(true);
            Parent.SetRangeItemFocus(this, true);
            _last = edge == RangeEdge.Last;
            _initialFirstValue = _firstValue;
            _initialLastValue = _lastValue;
            _trackedClientX = args.ClientX;
            _trackedClientY = args.ClientY;
            _trackedClientWidth = _trackedClientX;
            _trackedClientHeight = _trackedClientY;
            _valueCache = _value;
            if (_toolTipLast != null)
            {
                if (_last)
                {
                    _tooltipLastVisible = true;
                }
                else
                {
                    _tooltipFirstVisible = true;
                }
            }
            if (HasAttachedEdgeWithGap && !Master)
            {
                SetAsMaster();
            }
        }

        private void SetAsMaster()
        {
            Master = true;
            Slave = false;
            AttachedItem.Master = false;
            AttachedItem.Slave = true;
        }

        private bool IsMoveWithinBoundary(JsonElement jsonElement)
        {
            double clientX = jsonElement.GetProperty("clientX").GetDouble();
            double clientY = jsonElement.GetProperty("clientY").GetDouble();
            bool xCoordinateIsWithinBoundary = _trackedClientX <= clientX && clientX <= _trackedClientX + _trackedClientWidth;
            bool yCoordinateIsWithinBoundary = _trackedClientY <= clientY && clientY <= _trackedClientY + _trackedClientHeight;
            return xCoordinateIsWithinBoundary && yCoordinateIsWithinBoundary;
        }

        private async void OnMouseMove(JsonElement jsonElement)
        {
            if (_mouseDown)
            {
                await ApplyMouseMove(jsonElement, CalculateValueAsync);
            }
            if (_mouseDownOnTrack)
            {
                IsRangeDragged = true;
                await ApplyMouseMove(jsonElement, CalculateValuesAsync);
            }
        }

        private async Task ApplyMouseMove(JsonElement jsonElement, Func<double, Task<bool>> predicate)
        {
            _trackedClientX = jsonElement.GetProperty("clientX").GetDouble();
            _trackedClientY = jsonElement.GetProperty("clientY").GetDouble();
            double clickPosition = Parent.Vertical ? jsonElement.GetProperty("pageY").GetDouble() : jsonElement.GetProperty("pageX").GetDouble();
            if (!await predicate(clickPosition))
            {
                _shouldRender = false;
            }
        }

        private async void OnMouseUp(JsonElement jsonElement)
        {
            bool isMoveInEdgeBoundary = IsMoveWithinBoundary(jsonElement);
            if (!_mouseDown && !_mouseDownOnTrack && isMoveInEdgeBoundary)
            {
                //force blazor OnMouseDown events to run first
                await AsyncHelper.WaitFor(() => _mouseDown | _mouseDownOnTrack);
            }
            _shouldRender = true;

            bool mouseWasClicked = _mouseDown || _mouseDownOnTrack;
            if (_mouseDown)
            {
                _mouseDown = false;
                _trackedClientHeight = double.MinValue;
                _trackedClientWidth = double.MinValue;
                if (!isMoveInEdgeBoundary)
                {
                    await CalculateValueAsync(Parent.Vertical ? jsonElement.GetProperty("pageY").GetDouble() : jsonElement.GetProperty("pageX").GetDouble());
                }
            }
            if (_mouseDownOnTrack)
            {
                _mouseDownOnTrack = false;
                IsRangeDragged = false;
                _trackedClientHeight = double.MinValue;
                _trackedClientWidth = double.MinValue;
                if (HasAttachedEdgeWithGap)
                {
                    GapDistance = CalculateGapDistance();
                    AttachedItem.GapDistance = GapDistance;
                }
            }
            if (mouseWasClicked)
            {
                RemoveJsEvents();
#pragma warning disable CS4014 // Does not return anything, fire & forget            
                RaiseOnAfterChangeCallback(() => _valueCache != _value);
#pragma warning restore CS4014 // Does not return anything, fire & forget
            }
            if (_toolTipLast != null)
            {
                if (_tooltipLastVisible != TooltipVisible)
                {
                    _tooltipLastVisible = TooltipVisible;
                    _toolTipLast.SetVisible(TooltipVisible);
                }

                if (_tooltipFirstVisible != TooltipVisible)
                {
                    _tooltipFirstVisible = TooltipVisible;
                    _toolTipFirst.SetVisible(TooltipVisible);
                }
            }

            _initialFirstValue = _firstValue;
            _initialLastValue = _lastValue;
        }

        private bool _jsEventsSet;
        private async Task AddJsEvents()
        {
            await Task.Yield();
            if (!_jsEventsSet)
            {
                _jsEventsSet = true;
                DomEventListener.AddShared<JsonElement>("window", "mousemove", OnMouseMove, false);
                DomEventListener.AddShared<JsonElement>("window", "mouseup", OnMouseUp);
            }
        }

        private void RemoveJsEvents()
        {
            if (_jsEventsSet)
            {
                DomEventListener.RemoveShared<JsonElement>("window", "mousemove", OnMouseMove);
                DomEventListener.RemoveShared<JsonElement>("window", "mouseup", OnMouseUp);
                _jsEventsSet = false;
            }
        }

        private Task RaiseOnAfterChangeCallback(Func<bool> predicate)
        {
            if (predicate.Invoke())
            {
                if (OnAfterChange.HasDelegate)
                {
                    OnAfterChange.InvokeAsync(CurrentValue);
                }
                if (Parent.OnAfterChange.HasDelegate)
                {
                    Parent.OnAfterChange.InvokeAsync(CurrentValue);
                }
                if (_isDataSet && Data.OnAfterChange.HasDelegate)
                {
                    Data.OnAfterChange.InvokeAsync(CurrentValue);
                }
            }
            return Task.CompletedTask;
        }

        private async Task<bool> CalculateValueAsync(double clickClient)
        {
            (double sliderOffset, double sliderLength, _, _) = await GetSliderDimensions(Parent._railRef);
            bool hasChanged;
            if (_last)
            {
                double lastV = CalculateNewHandleValue(clickClient, sliderOffset, sliderLength);
                hasChanged = await HasValueChanged(ref _lastValue, () => ProcessNewLastValue(lastV));
            }
            else
            {
                double firstV = CalculateNewHandleValue(clickClient, sliderOffset, sliderLength);
                hasChanged = await HasValueChanged(ref _firstValue, () => ProcessNewFirstValue(firstV));
            }
            if (hasChanged)
            {
                ChangeAttachedItem?.Invoke();
            }
            return hasChanged;
        }

        private async Task<bool> CalculateValuesAsync(double clickClient)
        {
            (double sliderOffset, double sliderLength, _, _) = await GetSliderDimensions(Parent._railRef);

            double dragPosition = CalculateNewHandleValue(clickClient, sliderOffset, sliderLength);
            double lastV = dragPosition + _distanceToLastHandle;
            double firstV = dragPosition - _distanceToFirstHandle;
            if (lastV - firstV != LastValue - FirstValue)
            {
                //movement is shrinking the range, abort
                return false;
            }
            if (HasAttachedEdge)
            {
                return await CalculateValuesWithAttachedEdgesAsync(lastV, firstV);
            }
            else
            {
                //evaluate if both lastV & firstV are within acceptable values
                double lastCandidate = Clamp(lastV, Parent.GetFirstEdgeBoundary(Id, RangeEdge.Last, AttachedHandleNo), Parent.GetLastEdgeBoundary(Id, RangeEdge.Last, AttachedHandleNo));
                double firstCandidate = Clamp(firstV, Parent.GetFirstEdgeBoundary(Id, RangeEdge.First, AttachedHandleNo), Parent.GetLastEdgeBoundary(Id, RangeEdge.First, AttachedHandleNo));
                if (firstCandidate != FirstValue && lastCandidate != LastValue)
                {
                    ChangeFirstValue(firstCandidate, firstV);
                    ChangeLastValue(lastCandidate, lastV);
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> CalculateValuesWithAttachedEdgesAsync(double lastV, double firstV)
        {
            bool hasChanged = false;
            if (AttachedHandleNo == RangeEdge.First)
            {
                hasChanged = await HasValueChanged(ref _firstValue, () => ProcessNewFirstValue(firstV));
            }
            else
            {
                hasChanged = await HasValueChanged(ref _lastValue, () => ProcessNewLastValue(lastV));
            }

            if (hasChanged)
            {
                if (AttachedHandleNo == RangeEdge.First)
                {
                    await ProcessNewLastValue(lastV);
                }
                else
                {
                    await ProcessNewFirstValue(firstV);
                }
                ChangeAttachedItem?.Invoke();
            }
            return true;
        }

        private Task<bool> HasValueChanged(ref double value, Func<Task> predicate)
        {
            double valueB4Change = value;
            predicate.Invoke();
            double newValue = value;
            return Task.FromResult(valueB4Change != newValue);
        }

        private async Task ProcessNewLastValue(double lastV)
        {
            if (lastV < FirstValue)
            {
                if (Parent.AllowOverlapping && HasAttachedEdge) //push
                {
                    LastValue = lastV;
                    FirstValue = lastV;
                }
                else if (!HasAttachedEdge) //do not allow switching if locked with another range item
                {
                    await SwitchToFirstHandle(lastV);
                }
                else
                {
                    return;
                }
            }
            else
            {
                LastValue = lastV;
            }
        }

        private async Task SwitchToFirstHandle(double lastV)
        {
            _last = false;
            if (_mouseDown)
                LastValue = _initialFirstValue;
            FirstValue = lastV;
            SwitchTooltip(RangeEdge.First);
            await FocusAsync(_firstHandle);
        }

        private async Task ProcessNewFirstValue(double firstV)
        {
            if (firstV > LastValue)
            {
                if (Parent.AllowOverlapping && HasAttachedEdge) //push
                {
                    LastValue = firstV;
                    FirstValue = firstV;
                }
                else if (!HasAttachedEdge) //do not allow switching if locked with another range item
                {
                    await SwitchToLastHandle(firstV);
                }
                else
                {
                    return;
                }
            }
            else
            {
                FirstValue = firstV;
            }
        }

        private async Task SwitchToLastHandle(double firstV)
        {
            _last = true;
            if (_mouseDown)
                FirstValue = _initialLastValue;
            LastValue = firstV;
            SwitchTooltip(RangeEdge.Last);
            await FocusAsync(_lastHandle);
        }

        private void SwitchTooltip(RangeEdge toHandle)
        {
            if (_toolTipLast == null)
            {
                return;
            }

            if (toHandle == RangeEdge.First)
            {

                if (_tooltipLastVisible != TooltipVisible)
                {
                    _tooltipLastVisible = TooltipVisible;
                    _toolTipLast.SetVisible(TooltipVisible);
                }
                _tooltipFirstVisible = true;
                return;
            }

            if (_tooltipFirstVisible != TooltipVisible)
            {
                _tooltipFirstVisible = TooltipVisible;
                _toolTipFirst.SetVisible(TooltipVisible);
            }
            _tooltipLastVisible = true;
        }

        private double CalculateNewHandleValue(double clickClient, double sliderOffset, double sliderLength)
        {
            double handleNewPosition;
            if (Parent.Reverse)
            {
                if (Parent.Vertical)
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
                if (Parent.Vertical)
                {
                    handleNewPosition = sliderOffset + sliderLength - clickClient;
                }
                else
                {
                    handleNewPosition = clickClient - sliderOffset;
                }
            }

            return (Parent.MinMaxDelta * handleNewPosition / sliderLength) + Min;
        }

        internal void SetPositions()
        {
            var firstHandPercentage = (FirstValue - Min) / Parent.MinMaxDelta;
            var lastHandPercentage = (LastValue - Min) / Parent.MinMaxDelta;
            string firstHandStyle;
            string lastHandStyle;
            string trackStart;
            string trackSize;
            double trackStartAdjust = 0;
            double trackSizeAdjust = 0;
            if (FirstValue != Min)
            {
                trackStartAdjust = Parent.ItemAdjust;
                trackSizeAdjust = Parent.ItemAdjust;
            }
            if (LastValue != Max)
            {
                trackSizeAdjust += Parent.ItemAdjust;
            }

            //TODO: consider using delegates
            if (Parent.Vertical)
            {
                lastHandStyle = MultiRangeSlider.GetVerticalCoordinate(lastHandPercentage);
                firstHandStyle = MultiRangeSlider.GetVerticalCoordinate(firstHandPercentage);
                trackStart = MultiRangeSlider.GetVerticalCoordinate(firstHandPercentage - trackStartAdjust);
                trackSize = MultiRangeSlider.GetVerticalTrackSize(firstHandPercentage - trackStartAdjust, lastHandPercentage + (trackSizeAdjust - trackStartAdjust));
            }
            else
            {
                lastHandStyle = Formatter.ToPercentWithoutBlank(lastHandPercentage);
                firstHandStyle = Formatter.ToPercentWithoutBlank(firstHandPercentage);
                trackStart = Formatter.ToPercentWithoutBlank(firstHandPercentage - trackStartAdjust);
                trackSize = Formatter.ToPercentWithoutBlank(((LastValue - FirstValue) / Parent.MinMaxDelta) + trackSizeAdjust);
            }
            _lastHandleCssPosition = string.Format(CultureInfo.CurrentCulture, LastHandleStyleFormat, lastHandStyle);
            _trackCssPosition = string.Format(CultureInfo.CurrentCulture, TrackStyleFormat, trackStart, trackSize);
            _firstHandleCssPosition = string.Format(CultureInfo.CurrentCulture, FirstHandleStyleFormat, firstHandStyle);
            _shouldRender = true;
            StateHasChanged();
        }

        protected override void OnValueChange((double, double) value)
        {
            base.OnValueChange(value);

            if (FirstValue != value.Item1)
            {
                FirstValue = value.Item1;
            }
            if (LastValue != value.Item2)
            {
                LastValue = value.Item2;
            }
        }

        private bool IsFirstAndLastChanged((double, double) value) =>
            (value.Item1 != FirstValue) && (value.Item2 != LastValue);

        /// <summary>
        /// Gets or sets the value of the input. This should be used with two-way binding.
        /// </summary>
        /// <example>
        /// @bind-Value="model.PropertyName"
        /// </example>
        [Parameter]
        public sealed override (double, double) Value
        {
            get { return _value; }
            set
            {
                (double, double) orderedValue = SortValue(value);
                var hasChanged = orderedValue.Item1 != Value.Item1 || orderedValue.Item2 != Value.Item2;
                if (hasChanged)
                {
                    _value = orderedValue;
                    OnValueChange(orderedValue);
                }
            }
        }

        private (double, double) SortValue((double, double) value)
        {
            if (value.Item1 > value.Item2)
            {
                return (value.Item2, value.Item1);
            }
            return value;
        }
    }


}
