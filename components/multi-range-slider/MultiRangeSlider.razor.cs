// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntDesign.Core.Helpers;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System;
using System.Reflection;
using System.Threading.Tasks;
using AntDesign.JsInterop;

namespace AntDesign
{
    public partial class MultiRangeSlider : AntInputComponentBase<IEnumerable<(double, double)>>
    {
        //TODO: performance - minimize re-renders

        //TODO: customize scrollbars: https://www.youtube.com/watch?v=lvKK2fs6h4I&t=36s&ab_channel=KevinPowell        
        //TODO: fix multiple js errors on refersh         
        //TODO: MAYBE: show 3rd/4th tooltip for attached edges when range is dragged
        //TODO: Tooltip should not be visible when edge is overflowing (solved, except for vertical - only first rangeitem works)
        internal const int VerticalTrackAdjust = 14;
        private const string PreFixCls = "ant-multi-range-slider";
        private bool _isAtfterFirstRender = false;
        private string _overflow = "display: inline;";
        private string _sizeType = "width";
        private string _railStyle = "";
        private double _boundaryAdjust = 0;
        private bool _isInitialized;
        private bool _oversized;
        private bool _orientationHasChanged;
        private bool _expandStepHasChanged;
        private bool _reverseHasChanged;
        private double _visibleMin;
        private double _visibleMax;
        protected IEnumerable<(double, double)> _value;
        private IEnumerable<(double, double)> _valueClone = null;
        private bool _isTipFormatterDefault = true;
        private bool _tooltipVisibleChanged;
        private bool _tooltipPlacementChanged;
        private double? _step = 1;
        private int _precision;
        private bool _reverse;
        private bool _hasTooltipChanged;
        private Func<double, string> _tipFormatter = (d) => d.ToString(LocaleProvider.CurrentLocale.CurrentCulture);
        private List<RangeItem> _items = new();
        private List<string> _keys = new();
        private Dictionary<string, (RangeItem prevNeighbor, RangeItem nextNeighbor, RangeItem item)> _boundaries;
        internal ElementReference _railRef;
        private ElementReference _scrollableAreaRef;
        private string _trackSize = "";
        private RangeItem _focusedItem;
        private bool _vertical;
        private bool _expandStep;
        private bool _hasTooltip = true;
        private Placement _tooltipPlacement;
        private bool _tooltipVisible;
        private RangeItemMark[] _marks;

        internal RangeItem ItemRequestingAttach { get; set; }
        internal RangeItem ItemRespondingToAttach { get; set; }
        internal bool HasAttachedEdges { get; set; }
        internal double MinMaxDelta => Max - Min;
        internal bool Oversized { get => _oversized; set => _oversized = value; }
        internal double ItemAdjust { get; private set; }

        [CascadingParameter(Name = "RangeGroup")]
        public MultiRangeGroup Parent { get; set; }

        /// <summary>
        /// Use AllowOverlapping to toggle overlapping mode.
        /// </summary>
        [Parameter]
        public bool AllowOverlapping { get; set; }

        /// <summary>
        /// Used for rendering range items manually.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Collection of data objects implementing <see cref="IRangeItemData"/> 
        /// that will be used to render the ranges. 
        /// </summary>
        [Parameter]
        public IEnumerable<IRangeItemData> Data { get; set; }

        /// <summary>
        /// Gets or sets a callback that updates the bound value.
        /// </summary>
        [Parameter]
        public EventCallback<IEnumerable<IRangeItemData>> DataChanged { get; set; }

        /// <summary>
        /// If true, the slider will not be intractable
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Useful only when <see cref="AllowOverlapping"/>is set to false.
        /// Does not allow edges to meet, because treats equal edge values
        /// as overlapping. 
        /// </summary>
        [Parameter]
        public bool EqualIsOverlap { get; set; }

        /// <summary>
        /// Whether to expand visually each step.
        /// </summary>
        [Parameter]
        public bool ExpandStep
        {
            get => _expandStep;
            set
            {

                if (_expandStep != value)
                {
                    _expandStepHasChanged = true;
                    _expandStep = value;
                }
            }
        }

        /// <summary>
        /// <see cref="RangeItem"/> that will receive focus. If multiple
        /// <see cref="RangeItem"/> have the same value (overlapping), then 
        /// first matching will receive focus. Use this parameter only during
        /// initialization. Otherwise use <see cref="MultiRangeSlider.Focus((double, double))"/>
        /// method.
        /// </summary>
        [Parameter]
        public (double, double)? Focused { get; set; }

        /// <summary>
        /// Will not render Tooltip if set to false
        /// </summary>
        [Parameter]
        public bool HasTooltip
        {
            get => _hasTooltip;
            set
            {
                if (_hasTooltip != value)
                {
                    _hasTooltipChanged = true;
                    _hasTooltip = value;
                }
            }
        }

        /// <summary>
        /// Tick mark of the `MultiRangeSlider`, type of key must 
        /// be number, and must in closed interval [min, max], 
        /// each mark can declare its own style.
        /// </summary>
        [Parameter]
        public RangeItemMark[] Marks { get => _marks; set => _marks = value; }

        /// <summary>
        /// The maximum value the range slider
        /// </summary>
        [Parameter]
        public double Max { get; set; } = 100;

        /// <summary>
        /// The minimum value the range slider
        /// </summary>
        [Parameter]
        public double Min { get; set; } = 0;

        /// <summary>
        /// Called when changes are done (onmouseup and onkeyup).
        /// </summary>
        [Parameter]
        public EventCallback<(double, double)> OnAfterChange { get; set; }

        /// <summary>
        /// Called when range item looses focus.
        /// </summary>
        [Parameter]
        public EventCallback<(double, double)> OnBlur { get; set; }

        /// <summary>
        /// Called before edge is attached. If returns 'false', attaching is stopped.
        /// </summary>
        [Parameter]
        public Func<(RangeItem first, RangeItem last), (bool allowAttaching, bool detachExistingOnCancel)> OnEdgeAttaching { get; set; }

        /// <summary>
        /// Called after edge is attached.
        /// </summary>
        [Parameter]
        public EventCallback<(RangeItem first, RangeItem last)> OnEdgeAttached { get; set; }

        /// <summary>
        /// Called before edge is detached. If returns 'false', detaching is stopped.
        /// </summary>
        [Parameter]
        public Func<(RangeItem first, RangeItem last), bool> OnEdgeDetaching { get; set; }

        /// <summary>
        /// Called after edge is detached.
        /// </summary>
        [Parameter]
        public EventCallback<(RangeItem first, RangeItem last)> OnEdgeDetached { get; set; }

        /// <summary>
        /// Called before edge is moved. If returns `false`, moving is canceled.
        /// </summary>
        [Parameter]
        public Func<(RangeItem range, RangeEdge edge, double value), bool> OnEdgeMoving { get; set; }

        /// <summary>
        /// Called after edge is moved.
        /// </summary>
        [Parameter]
        public EventCallback<(RangeItem range, RangeEdge edge, double value)> OnEdgeMoved { get; set; }

        /// <summary>
        /// Called when range item receives focus.
        /// </summary>
        [Parameter]
        public EventCallback<(double, double)> OnFocus { get; set; }

        /// <summary>
        /// Called when the user changes one of the values.
        /// </summary>
        [Parameter]
        public EventCallback<(double, double)> OnChange { get; set; }

        /// <summary>
        /// Used to style overflowing container. Avoid using unless in over-sized 
        /// mode (when `VisibleMin` &gt; `Min` and/or `VisibleMax` &lt; 'Max' ).
        /// </summary>
        [Parameter]
        public string OverflowStyle { get; set; } = "";

        /// <summary>
        /// Render the slider with scale starting form left side 
        /// to right or from bottom towards top.
        /// </summary>
        [Parameter]
        public bool Reverse
        {
            get { return _reverse; }
            set
            {
                if (_reverse != value)
                {
                    _reverse = value;
                    _reverseHasChanged = true;
                }
            }
        }

        /// <summary>
        /// The granularity the slider can step through values. 
        /// Must be greater than 0, and be divided by (<see cref="VisibleMax"/> - <see cref="VisibleMin"/>) . 
        /// When <see cref="Marks"/> no null, <see cref="Step"/> can be null.
        /// </summary>
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

        /// <summary>
        /// Slider will pass its value to tipFormatter, and display its value in Tooltip
        /// </summary>
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
        /// Set `Tooltip` display position. 
        /// </summary>
        [Parameter]
        public Placement TooltipPlacement
        {
            get => _tooltipPlacement;
            set
            {
                if (_tooltipPlacement != value)
                {
                    _tooltipPlacementChanged = true;
                    _tooltipPlacement = value;
                }
            }
        }

        /// <summary>
        /// If `true`, `Tooltip` will show always, or it will 
        /// not show anyway, even if dragging or hovering.
        /// </summary>
        [Parameter]
        public bool TooltipVisible
        {
            get => _tooltipVisible;
            set
            {
                if (_tooltipVisible != value)
                {
                    _tooltipVisibleChanged = true;
                    _tooltipVisible = value;
                }
            }
        }

        /// <summary>
        /// Get or set the values.
        /// </summary>
        [Parameter]
        public override IEnumerable<(double, double)> Value
        {
            get => _value;
            set
            {
                if (value != null && _valueClone != null)
                {
                    var hasChanged = value.Count() != _valueClone.Count(); //!value.SequenceEqual(_valueClone);

                    if (!hasChanged)
                    {
                        return;
                    }
                    if (ChildContent is null)
                    {
                        RebuildKeys(value, _valueClone); ;
                    }
                    _valueClone = _value.ToArray();
                    _ = OnValueChangeAsync(value);
                }
                else if (value != null && _valueClone == null)
                {
                    _value = value;
                    _valueClone = _value.ToArray();
                    _ = OnValueChangeAsync(value);
                }
                else if (value == null && _valueClone != null)
                {
                    _value = default;
                    _valueClone = default;
                    _ = OnValueChangeAsync(value);
                }

                if (_isNotifyFieldChanged && Form?.ValidateOnChange == true)
                {
                    EditContext?.NotifyFieldChanged(FieldIdentifier);
                }
            }
        }

        /// <summary>
        /// If true, the slider will be vertical.
        /// </summary>
        [Parameter]
        public bool Vertical
        {
            get => _vertical;
            set
            {
                if (_vertical != value)
                {
                    _orientationHasChanged = true;
                    _vertical = value;
                }
            }
        }

        /// <summary>
        /// Used in connection with <see cref="VisibleMax"/>. If grater than 
        /// <see cref="Min"/>, the slider is rendered with an overflow 
        /// (oversized/zoomed). If lesser than <see cref="Min"/>, will be set
        /// to <see cref="Min"/>;
        /// </summary>
        [Parameter]
        public double VisibleMin
        {
            get => _visibleMin;
            set
            {
                var hasChanged = value != _visibleMin;
                if (hasChanged)
                {
                    if (value < Min)
                    {
                        _visibleMin = Min;
                    }
                    else
                    {
                        _visibleMin = value;
                    }
                    Oversized = Min < _visibleMin || Max > _visibleMax;
                    SetOrientationStyles();
                    _trackSize = GetRangeFullSize();
                }

            }
        }

        /// <summary>
        /// Used in connection with <see cref="VisibleMin"/>. If lesser than 
        /// <see cref="Max"/>, the slider is rendered with an overflow 
        /// (oversized/zoomed). If grater than <see cref="Max"/>, will be set
        /// to <see cref="Max"/>;
        /// </summary>
        [Parameter]
        public double VisibleMax
        {
            get => _visibleMax;
            set
            {
                var hasChanged = value != _visibleMax;
                if (hasChanged)
                {
                    if (value > Max)
                    {
                        _visibleMax = Max;
                    }
                    else
                    {
                        _visibleMax = value;
                    }
                    Oversized = Min < _visibleMin || Max > _visibleMax;
                    SetOrientationStyles();
                    _trackSize = GetRangeFullSize();
                }
            }
        }

        private void SetOrientationStyles()
        {
            if (_vertical)
            {
                //padding is 20px of the track width + "margin" of possible scroll;
                //without padding, vertical scroll hides most of the rendered elements
                if (Oversized)
                {
                    _overflow = "overflow-y: auto;overflow-x: hidden; padding-right: 8px; height: inherit;";
                }
                else
                {
                    _overflow = "display: inline;";
                }
                _railStyle = $"height: calc(100% - {2 * VerticalTrackAdjust}px);top: {VerticalTrackAdjust}px;";
                _sizeType = "height";
            }
            else
            {
                if (Oversized)
                {
                    _overflow = "overflow-x: auto; height: inherit;" + (HasTooltip ? "padding-top: 40px;" : ""); //padding-top needed, otherwise tooltips are not visible
                }
                else
                {
                    _overflow = "display: inline;";
                }
                _sizeType = "width";
                _railStyle = "";
            }
        }

        /// <summary>
        ///     The Method is called every time if the value of the @bind-Values was changed by the two-way binding.
        /// </summary>
        protected async Task OnValueChangeAsync(IEnumerable<(double, double)> values)
        {
            if (!_isInitialized) // This is important because otherwise the initial value is overwritten by the EventCallback of ValueChanged and would be NULL.
            {
                return;
            }

            if (!_items.Any())
            {
                return;
            }

            if (values == null)
            {
                await ValueChanged.InvokeAsync(default);
                return;
            }
            await ValueChanged.InvokeAsync(Value);
        }

        async Task RangeItemDataChanged(IRangeItemData data, (double, double) value)
        {
            data.Value = value;
            await DataChanged.InvokeAsync(Data);
        }

        void RangeItemValueChanged(int index, (double, double) value)
        {
            //TODO: check if _value can be switched to a List of tuples or other wrapped object, so it is passed as reference to RangeItem and can be used with @bind modifier
            var temp = _value.ToList();
            temp[index] = value;
            _value = temp;
            _ = OnValueChangeAsync(temp);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent?.AddMultiRangeSliderItem(this);
            _trackSize = GetRangeFullSize();
            _isInitialized = true;
        }

        Dictionary<string, MethodInfo> _methods = new();
        private MethodInfo GetRangeItemMethod(string methodName)
        {
            MethodInfo method;
            if (!_methods.TryGetValue(methodName, out method))
            {
                method = typeof(RangeItem).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
                _methods.Add(methodName, method);
            }
            return method;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            ValidateParameter();

            ClassMapper.Clear()
                .Add(PreFixCls)
                .If($"{PreFixCls}-disabled", () => Disabled)
                .If($"{PreFixCls}-vertical", () => Vertical)
                .If($"{PreFixCls}-vertical-oversized", () => Vertical && Oversized)
                .If($"{PreFixCls}-with-marks", () => Marks != null)
                .If($"{PreFixCls}-group", () => Parent is not null)
                .If($"{PreFixCls}-group-last-child", () => Parent is not null && Parent.IsLast(this))
                .If($"{PreFixCls}-rtl", () => RTL);

            ForwardChangesToChildren();

            if (Step is not null)
            {
                if (EqualIsOverlap)
                {
                    _boundaryAdjust = Step.Value;
                }
                else
                {
                    _boundaryAdjust = 0;
                }
            }
            if (_expandStepHasChanged && Step is not null)
            {
                if (ExpandStep)
                {
                    ItemAdjust = (Step.Value / (Max - Min)) / 2d;
                }
                else
                {
                    ItemAdjust = 0;
                }
            }
        }

        private void ForwardChangesToChildren()
        {
            List<MethodInfo> delegatesToExecute = new();
            if (_orientationHasChanged || !_isInitialized)
            {
                SetOrientationStyles();
                if (_orientationHasChanged && _items.Count > 0)
                {
                    delegatesToExecute.Add(GetRangeItemMethod(nameof(RangeItem.SetPositions)));
                }
                if (Vertical)
                {
                    TooltipPlacement = Placement.Right;
                }
                else
                {
                    TooltipPlacement = Placement.Top;
                }
                _orientationHasChanged = false;
            }
            if (_isInitialized && _items.Count > 0)
            {
                if (_hasTooltipChanged)
                {
                    delegatesToExecute.Add(GetRangeItemMethod(nameof(RangeItem.SetHasTooltipFromParent)));
                    _hasTooltipChanged = false;
                }
                if (_tooltipPlacementChanged)
                {
                    delegatesToExecute.Add(GetRangeItemMethod(nameof(RangeItem.SetTooltipPacementFromParent)));
                    _tooltipPlacementChanged = false;
                }
                if (_tooltipVisibleChanged)
                {
                    delegatesToExecute.Add(GetRangeItemMethod(nameof(RangeItem.SetTooltipVisibleFromParent)));
                    _tooltipVisibleChanged = false;
                }
                if (_reverseHasChanged)
                {
                    delegatesToExecute.Add(GetRangeItemMethod(nameof(RangeItem.SetPositions)));
                    _reverseHasChanged = false;
                }
            }
            if (delegatesToExecute.Count > 0)
            {
                _items.ForEach(i => delegatesToExecute.ForEach(d => d.Invoke(i, null)));
            }
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

            if (Data is not null && Value is not null)
            {
                throw new ArgumentException($"{nameof(Data)}, {nameof(Value)}", $"Either {nameof(Data)} or {nameof(Value)} parameters can be set. Not both.");
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                _isAtfterFirstRender = true;
                SortRangeItems();
            }
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            if (Oversized)
            {
                double x = 0, y = 0;
                if (Vertical)
                {
                    y = VisibleMin / 100d;
                }
                else
                {
                    x = VisibleMin / 100d;
                }
                await JsInvokeAsync(JSInteropConstants.DomMainpulationHelper.ScrollToPoint, _scrollableAreaRef, x, y, true);
            }
            await base.OnFirstAfterRenderAsync();
        }

        protected override void Dispose(bool disposing)
        {
            Parent?.RemoveMultiRangeSliderItem(this);
            base.Dispose(disposing);
        }

        internal void AddRangeItem(RangeItem item)
        {
            _items.Add(item);
            if (_keys.Count < _items.Count)
            {
                _keys.Add(Guid.NewGuid().ToString());
            }
            SortRangeItems();
            if (Focused is not null && item.Value.Equals(Focused))
            {
                SetRangeItemFocus(item, true);
                item.SetFocus(true);
            }
        }

        internal void RemoveRangeItem(RangeItem item)
        {
            int index = _items.IndexOf(item);
            if (index >= 0)
            {
                _items.RemoveAt(index);
                _keys.RemoveAt(index);
                SortRangeItems();
            }
        }

        private string GetOrAddKey(int index)
        {
            if (_keys.Count <= index)
            {
                string newKey = Guid.NewGuid().ToString();
                _keys.Add(newKey);
                return newKey;
            }
            return _keys[index];
        }

        private void RebuildKeys(IEnumerable<(double, double)> newValues, IEnumerable<(double, double)> oldValues)
        {
            if (_isInitialized)
            {
                for (int i = _items.Count - 1; i >= 0; i--)
                {
                    _items[i].Dispose();

                }
                _keys.Clear();
            }
        }

        internal void SetMarksFromParent(RangeItemMark[] marks) => _marks = marks;
        internal void SetVerticalFromParent(bool vertical) => Vertical = vertical;
        internal void SetReverseFromParent(bool reverse) => Reverse = reverse;

        internal double GetFirstEdgeBoundary(string id, RangeEdge fromHandle, RangeEdge attachedHandle)
        {
            if (AllowOverlapping)
            {
                if (_boundaries?[id].item?.HasAttachedEdgeWithGap ?? false)
                {
                    GetPulledEdgesValues(id, out double currentItemPulledEdge, out double attachedItemPulledEdge);
                    if (attachedItemPulledEdge < currentItemPulledEdge)
                    {
                        return currentItemPulledEdge - attachedItemPulledEdge;
                    }
                }
                return Min;
            }

            if (_isAtfterFirstRender && _boundaries[id].prevNeighbor != default)
            {
                if (fromHandle == attachedHandle)
                {
                    if (_boundaries[id].item.HasAttachedEdgeWithGap)
                    {
                        if (attachedHandle == RangeEdge.First)
                        {
                            return _boundaries[id].prevNeighbor.FirstValue + _boundaries[id].item.GapDistance;
                        }
                    }
                    else
                    {
                        if (attachedHandle == RangeEdge.First) //do not allow to exceed neighbor's edge
                        {
                            return _boundaries[id].prevNeighbor.FirstValue;
                        }
                    }
                }
                else
                {
                    //for single edge, all except the closes to the Min
                    return _boundaries[id].prevNeighbor.LastValue + _boundaryAdjust;
                }
            }
            return Min;
        }

        internal double GetLastEdgeBoundary(string id, RangeEdge fromHandle, RangeEdge attachedHandleNo)
        {
            if (AllowOverlapping)
            {
                if (_boundaries?[id].item?.HasAttachedEdgeWithGap ?? false)
                {
                    GetPulledEdgesValues(id, out double currentItemPulledEdge, out double attachedItemPulledEdge);
                    if (attachedItemPulledEdge > currentItemPulledEdge)
                    {
                        return Max - (attachedItemPulledEdge - currentItemPulledEdge);
                    }
                }
                return Max;
            }

            if (_isAtfterFirstRender && _boundaries[id].nextNeighbor != default)
            {
                if (fromHandle == attachedHandleNo)
                {
                    if (_boundaries[id].item.HasAttachedEdgeWithGap)
                    {
                        if (attachedHandleNo == RangeEdge.First)
                        {
                            return _boundaries[id].item.LastValue;
                        }
                        if (attachedHandleNo == RangeEdge.Last)
                        {
                            //in a gap situation, gap distance has to be accounted for
                            return _boundaries[id].nextNeighbor.LastValue - _boundaries[id].item.GapDistance;
                        }
                    }
                    else
                    {
                        if (attachedHandleNo == RangeEdge.First)
                        {
                            return _boundaries[id].nextNeighbor.FirstValue;
                        }
                        else
                        {
                            //used only when range is dragged but 2 neighboring
                            //edges are attached & first range is dragged
                            return _boundaries[id].nextNeighbor.LastValue;
                        }
                    }
                }
                else
                {
                    //for single edge, all except the closes to the Max
                    return _boundaries[id].nextNeighbor.FirstValue - _boundaryAdjust;

                }
            }
            if (attachedHandleNo > 0) //because this is with attached, it's max is its own current Last
            {
                if (!_boundaries[id].item.IsRangeDragged)
                {
                    return _boundaries[id].item.LastValue;
                }
            }
            return Max;
        }

        private void GetPulledEdgesValues(string id, out double currentItemPulledEdge, out double attachedItemPulledEdge)
        {
            var currentItem = _boundaries[id].item;
            currentItemPulledEdge = GetPulledEdgeValue(currentItem);
            attachedItemPulledEdge = GetPulledEdgeValue(currentItem.AttachedItem);
        }

        private static double GetPulledEdgeValue(RangeItem item)
        {
            if (item.AttachedHandleNo == RangeEdge.First)
            {
                return item.FirstValue;
            }
            else
            {
                return item.LastValue;
            }
        }
        internal RangeItem GetNextNeighbour(string id)
        {
            if (_isAtfterFirstRender && _boundaries?[id].nextNeighbor != default)
            {
                return _boundaries[id].nextNeighbor;
            }
            return default;
        }

        internal RangeItem GetPrevNeighbour(string id)
        {
            if (_isAtfterFirstRender && _boundaries?[id].prevNeighbor != default)
            {
                return _boundaries[id].prevNeighbor;
            }
            return default;
        }

        internal void SortRangeItems()
        {
            if (!_isAtfterFirstRender || _items.Count == 0)
            {
                return;
            }
            _items.Sort((s1, s2) =>
                {
                    var firstItemCompare = s1.Value.Item1.CompareTo(s2.Value.Item1);
                    if (firstItemCompare == 0)
                    {
                        return s1.Value.Item2.CompareTo(s2.Value.Item2);
                    }
                    return firstItemCompare;
                });

            _boundaries = new();
            RangeItem prevNeighbor = default;
            string previousId = "";
            (RangeItem prevNeighbor, RangeItem nextNeighbor, RangeItem item) previousItem;
            foreach (var item in _items)
            {
                if (previousId != string.Empty)
                {
                    previousItem = _boundaries[previousId];
                    previousItem.nextNeighbor = item;
                    _boundaries[previousId] = previousItem;

                }
                previousId = item.Id;

                _boundaries.Add(item.Id, (prevNeighbor, default, item));
                prevNeighbor = item;

            }

            previousItem = _boundaries[previousId];
            previousItem.nextNeighbor = default;
            _boundaries[previousId] = previousItem;
        }

        internal void GetAttachedInOrder(out RangeItem first, out RangeItem last)
        {
            if (ItemRequestingAttach.AttachedHandleNo != ItemRespondingToAttach.AttachedHandleNo //same edges - no way to overlap
                && AllowOverlapping)
            {
                if (ItemRequestingAttach.AttachedHandleNo == RangeEdge.First)
                {
                    last = ItemRequestingAttach;
                    first = ItemRespondingToAttach;
                }
                else
                {
                    last = ItemRespondingToAttach;
                    first = ItemRequestingAttach;
                }
            }
            else
            {
                if (ItemRequestingAttach.AttachedHandleNo == RangeEdge.First)
                {
                    first = ItemRequestingAttach;
                    last = ItemRespondingToAttach;
                }
                else
                {
                    first = ItemRespondingToAttach;
                    last = ItemRequestingAttach;
                }
            }
        }

        internal double GetNearestStep(double value)
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

        private string SetMarkPosition(double key)
        {
            if (Vertical)
            {
                if (Reverse)
                {
                    return GetVerticalCoordinate(1 - (key - Min) / MinMaxDelta);
                }
                return GetVerticalCoordinate((key - Min) / MinMaxDelta);
            }
            if (Reverse)
            {
                return Formatter.ToPercentWithoutBlank(1 - ((key - Min) / MinMaxDelta));

            }
            return Formatter.ToPercentWithoutBlank((key - Min) / MinMaxDelta);
        }

        private string GetRangeFullSize()
        {
            if (!Oversized)
            {
                return "";
            }
            else
            {
                return $"{_sizeType}: {(Max - Min) / (VisibleMax - VisibleMin) * 100}%;";
            }
        }

        internal void SetRangeItemFocus(RangeItem item, bool isFocused)
        {
            if (_focusedItem is not null)
            {
                if (isFocused && _focusedItem.Id == item.Id)
                {
                    return;
                }
                _focusedItem.SetFocus(false);
            }
            if (isFocused)
            {
                _focusedItem = item;
            }
            else
            {
                _focusedItem = null;
            }
        }

        /// <summary>
        /// When MultiRangeSlider is Vertical, special calculations are made, 
        /// there is a visual issue: Min and Max position are overflowing, so when an edge is set
        /// in the Min/Max, half of the edge is not visible due to overflowing set to hidden.
        /// 
        /// Current solution: make track smaller by a <see cref="VerticalTrackAdjust">number of pixels</see>.
        /// As a result, a relative calculation has to be performed to evaluate edge positions. 
        /// </summary>
        /// <param name="nominalPercentage">The percentage calculated for a point as it would be 
        /// used without compensating for visual issue.
        /// </param>
        /// <returns>css calc formula</returns>
        /// <see cref="GetVerticalTrackSize"/>
        internal static string GetVerticalCoordinate(double nominalPercentage)
        {
            var skew = GetVerticalSkew(nominalPercentage);
            return $"calc({Formatter.ToPercentWithoutBlank(nominalPercentage)} - ({skew} * {VerticalTrackAdjust}px))";
        }

        /// <summary>
        /// When MultiRangeSlider is Vertical, special calculations are made, 
        /// there is a visual issue: Min and Max position are overflowing, so when an edge is set
        /// in the Min/Max, half of the edge is not visible due to overflowing set to hidden.
        /// 
        /// Calculates the percentage of <see cref="VerticalTrackAdjust">number of pixels</see>.
        /// It will be applied to css calc formula.
        /// </summary>
        /// <param name="nominalPercentage">The percentage calculated for a point as it would be 
        /// used without compensating for visual issue.
        /// 
        /// It is a "pendulum algorithm" (don't know if such algorithm exists and if yes is this the correct name). 
        /// The logic here is that:
        /// 1. Rail is a range from 0% to 100%. 
        /// 2. Adjustment has to go from -100% at rail position = 0% to +100% at rail position = 100%. 
        /// 3. So if calculated from 0% => -100%, 1% => -98%, 2% => -96%, ... 50% => 0%, ..., 99% => 98%, 100% => 100%
        /// </param>
        /// <returns>percentage as fraction</returns>
        private static double GetVerticalSkew(double nominalPercentage)
        {
            double skew;
            if (nominalPercentage < 50)
            {
                skew = (2d * nominalPercentage) - 1d;
            }
            else
            {
                skew = (nominalPercentage - 0.5d) * 2d;
            }

            return skew;
        }

        /// <summary>
        /// When MultiRangeSlider is Vertical, special calculations are made, 
        /// there is a visual issue: Min and Max position are overflowing, so when an edge is set
        /// in the Min/Max, half of the edge is not visible due to overflowing set to hidden.
        /// 
        /// Current solution: make track smaller by a <see cref="VerticalTrackAdjust">number of pixels</see>.
        /// As a result, a relative calculation has to be performed to evaluate track size. 
        /// </summary>
        /// <param name="firstHandlePercentage">The percentage calculated for the first edge as it would be 
        /// used without compensating for visual issue.
        /// </param>
        /// <param name="secondHandlePercentage">The percentage calculated for the last edge it would be 
        /// used without compensating for visual issue.
        /// </param>/// 
        /// <returns>css calc formula</returns>
        /// <see cref="GetVerticalCoordinate"/>
        internal static string GetVerticalTrackSize(double firstHandlePercentage, double secondHandlePercentage)
        {
            var skewFirst = GetVerticalSkew(firstHandlePercentage);
            var skewLast = GetVerticalSkew(secondHandlePercentage);

            return $"calc(({Formatter.ToPercentWithoutBlank(secondHandlePercentage)} - ({skewLast} * {VerticalTrackAdjust}px)) " +
                   $"- ({Formatter.ToPercentWithoutBlank(firstHandlePercentage)} - ({skewFirst} * {VerticalTrackAdjust}px)))";
        }

        private string GetBasePosition() => Vertical ? "bottom" : "left";

        /// <summary>
        /// Will set focus to <see cref="RangeItem"/>. If multiple <see cref="RangeItem"/>
        /// have the same values set as passed, then first matching will receive focus.
        /// If another <see cref="RangeItem"/> is currently focused, it will loose its focus.
        /// </summary>
        /// <param name="value">Value of the <see cref="RangeItem"/></param>
        public void Focus((double, double) value)
        {
            var rangeItem = _items.Where(r => r.Value.Equals(value)).FirstOrDefault();
            if (rangeItem is not null)
            {
                rangeItem.SetFocus(true);
            }
        }

        /// <summary>
        /// Removes focus. 
        /// </summary>
        public void Blur()
        {
            SetRangeItemFocus(_focusedItem, false);
        }

    }
}
