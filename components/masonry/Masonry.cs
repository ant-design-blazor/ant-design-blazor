// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Core.JsInterop.ObservableApi;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using OneOf;

namespace AntDesign
{
    using MasonryGutter = OneOf<int, Dictionary<string, int>, (int, int), (Dictionary<string, int>, int), (int, Dictionary<string, int>), (Dictionary<string, int>, Dictionary<string, int>)>;
    using MasonryColumns = OneOf<int, Dictionary<string, int>>;

    /**
    <summary>
    <para>A masonry layout component for displaying content with different heights.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>When displaying images or cards with irregular heights</item>
        <item>When content needs to be evenly distributed in columns</item>
        <item>When column count needs to be responsive</item>
    </list>
    </summary>
    <seealso cref="MasonryItem{T}" />
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Layout, "https://mdn.alipayobjects.com/huamei_iwk9zp/afts/img/A*cELTRrM5HpAAAAAAOGAAAAgAegCCAQ/original", Columns = 1, Title = "Masonry", SubTitle = "瀑布流")]
    public partial class Masonry<T> : AntDomComponentBase, IMasonryItemViewParent
    {
        private static readonly BreakpointType[] ResponsiveBreakpoints =
        [
            BreakpointType.Xxl,
            BreakpointType.Xl,
            BreakpointType.Lg,
            BreakpointType.Md,
            BreakpointType.Sm,
            BreakpointType.Xs,
        ];

        private readonly Dictionary<object, double> _itemHeights = new();
        private readonly Dictionary<object, ElementReference> _itemRefs = new();
        private readonly Dictionary<object, ElementReference> _pendingObserverDisposals = new();
        private readonly HashSet<object> _observedItems = new();
        private readonly Dictionary<object, int> _lastLayout = new();

        private List<MasonryItem<T>> _items = [];
        private Dictionary<object, MasonryPosition> _positions = new();
        private int _columnCount = 3;
        private int _horizontalGutter;
        private int _verticalGutter;
        private double _totalHeight;
        private BreakpointType _breakpoint = BreakpointType.Xs;
        private bool _measureRequested;
        private bool _firstRender;
        private bool _responsiveEnabled;
        private bool _resizeObserverRegistered;

        /// <summary>
        /// Customize class for each semantic structure inside the component. Supports object or function.
        /// </summary>
        [Parameter]
        public IDictionary<string, string> ClassNames { get; set; }

        /// <summary>
        /// Customize inline style for each semantic structure inside the component. Supports object or function.
        /// </summary>
        [Parameter]
        public IDictionary<string, string> Styles { get; set; }

        /// <summary>
        /// Spacing, can be a fixed value, responsive configuration, or a configuration for horizontal and vertical spacing
        /// </summary>
        [Parameter]
        public MasonryGutter Gutter { get; set; } = 0;

        /// <summary>
        /// Masonry items
        /// </summary>
        [Parameter]
        public IEnumerable<MasonryItem<T>> Items { get; set; }

        /// <summary>
        /// Custom item rendering function
        /// </summary>
        [Parameter]
        public RenderFragment<MasonryItemRenderContext<T>> ItemRender { get; set; }

        /// <summary>
        /// Number of columns, can be a fixed value or a responsive configuration
        /// </summary>
        [Parameter]
        public MasonryColumns Columns { get; set; } = 3;

        /// <summary>
        /// Callback for column sorting changes
        /// </summary>
        [Parameter]
        public EventCallback<IEnumerable<MasonryLayoutItem>> OnLayoutChange { get; set; }

        /// <summary>
        /// Whether to continuously monitor the size changes of child items
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Fresh { get; set; }

        [Inject]
        private DomEventService DomEventService { get; set; }

        private IDomEventListener DomEventListener { get; set; }

        protected string PrefixCls => "ant-masonry";

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            _responsiveEnabled = false;
            foreach (var parameter in parameters)
            {
                if (parameter.Name == nameof(Columns) || parameter.Name == nameof(Gutter))
                {
                    _responsiveEnabled = true;
                    break;
                }
            }

            await base.SetParametersAsync(parameters);
        }

        protected override void OnInitialized()
        {
            DomEventListener = DomEventService.CreateDomEventListerner();

            ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-rtl", () => RTL);

            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            _items = (Items ?? Enumerable.Empty<MasonryItem<T>>()).ToList();

            var validKeys = new HashSet<object>(_items.Select((item, index) => GetItemKey(item, index)));
            var staleKeys = _itemRefs.Keys
                .Concat(_itemHeights.Keys)
                .Concat(_observedItems)
                .Where(key => !validKeys.Contains(key))
                .Distinct()
                .ToList();
            foreach (var key in staleKeys)
            {
                if (_observedItems.Contains(key) && _itemRefs.TryGetValue(key, out var element))
                {
                    _pendingObserverDisposals[key] = element;
                }

                _itemHeights.Remove(key);
                _itemRefs.Remove(key);
                _observedItems.Remove(key);
                _lastLayout.Remove(key);
            }

            _columnCount = Math.Max(1, ResolveColumns());
            (_horizontalGutter, _verticalGutter) = ResolveGutters();
            RecalculatePositions();
            _measureRequested = true;

            base.OnParametersSet();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (_responsiveEnabled && !_resizeObserverRegistered)
            {
                _resizeObserverRegistered = true;
                await DomEventListener.AddResizeObserver(Ref, OnContainerResize);
                var dimensions = await JsInvokeAsync<Window>(JSInteropConstants.GetWindow);
                SetBreakpoint(GetBreakpoint(dimensions.InnerWidth));
            }
            else if (!_responsiveEnabled && _resizeObserverRegistered)
            {
                _resizeObserverRegistered = false;
                await DomEventListener.DisposeResizeObserver(Ref);
            }

            if (firstRender)
            {
                _firstRender = true;
            }

            await DisposeRemovedItemObserversAsync();

            if (!Fresh)
            {
                await DisposeAllItemObserversAsync();
            }

            if (_firstRender && Fresh)
            {
                await ObserveItemsAsync();
            }

            if (_measureRequested)
            {
                _measureRequested = false;
                await CollectItemSizesAsync();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task CollectItemSizesAsync()
        {
            var changed = false;
            var itemRefs = _items
                .Select((item, index) => (Key: GetItemKey(item, index), Item: item))
                .Where(item => _itemRefs.ContainsKey(item.Key))
                .Select(item => (item.Key, Element: _itemRefs[item.Key]))
                .ToList();

            if (itemRefs.Count > 0)
            {
                var rects = await JsInvokeAsync<DomRect[]>(
                    JSInteropConstants.GetBoundingClientRects,
                    itemRefs.Select(item => item.Element).ToArray());

                for (var index = 0; index < itemRefs.Count; index++)
                {
                    var key = itemRefs[index].Key;
                    var height = (double)(rects?.ElementAtOrDefault(index)?.Height ?? 0);
                    if (!_itemHeights.TryGetValue(key, out var previous) || Math.Abs(previous - height) > 0.01)
                    {
                        _itemHeights[key] = height;
                        changed = true;
                    }
                }
            }

            if (changed)
            {
                RecalculatePositions();
                await NotifyLayoutChangeAsync();
                InvokeStateHasChanged();
            }
            else
            {
                await NotifyLayoutChangeAsync();
            }
        }

        private async Task ObserveItemsAsync()
        {
            foreach (var (key, element) in _itemRefs.ToList())
            {
                if (_observedItems.Add(key))
                {
                    await DomEventListener.AddResizeObserver(element, OnItemResize);
                }
            }
        }

        private async Task DisposeRemovedItemObserversAsync()
        {
            foreach (var element in _pendingObserverDisposals.Values)
            {
                await DomEventListener.DisposeResizeObserver(element);
            }

            _pendingObserverDisposals.Clear();
        }

        private async Task DisposeAllItemObserversAsync()
        {
            foreach (var element in _itemRefs
                .Where(item => _observedItems.Contains(item.Key))
                .Select(item => item.Value)
                .ToList())
            {
                await DomEventListener.DisposeResizeObserver(element);
            }

            _observedItems.Clear();
        }

        private void OnItemResize(List<ResizeObserverEntry> _)
        {
            _measureRequested = true;
            InvokeStateHasChanged();
        }

        private void OnContainerResize(List<ResizeObserverEntry> entries)
        {
            var width = entries.FirstOrDefault()?.ContentRect?.Width;
            if (width.HasValue)
            {
                SetBreakpoint(GetBreakpoint(width.Value));
            }

            _measureRequested = true;
            InvokeStateHasChanged();
        }

        private void SetBreakpoint(BreakpointType breakpoint)
        {
            if (_breakpoint == breakpoint)
            {
                return;
            }

            _breakpoint = breakpoint;
            _columnCount = Math.Max(1, ResolveColumns());
            (_horizontalGutter, _verticalGutter) = ResolveGutters();
            RecalculatePositions();
            _measureRequested = true;
            InvokeStateHasChanged();
        }

        private int ResolveColumns()
        {
            return Columns.Match(
                number => number,
                responsive => ResolveResponsiveValue(responsive, 1));
        }

        private (int Horizontal, int Vertical) ResolveGutters()
        {
            return Gutter.Match(
                number => (number, number),
                responsive => (ResolveResponsiveValue(responsive, 0), ResolveResponsiveValue(responsive, 0)),
                tuple => tuple,
                tuple => (ResolveResponsiveValue(tuple.Item1, 0), tuple.Item2),
                tuple => (tuple.Item1, ResolveResponsiveValue(tuple.Item2, 0)),
                tuple => (ResolveResponsiveValue(tuple.Item1, 0), ResolveResponsiveValue(tuple.Item2, 0)));
        }

        private int ResolveResponsiveValue(Dictionary<string, int> values, int fallback)
        {
            if (values == null)
            {
                return fallback;
            }

            foreach (var breakpoint in ResponsiveBreakpoints)
            {
                if (IsBreakpointActive(breakpoint) && values.TryGetValue(breakpoint.ToString(), out var value))
                {
                    return value;
                }
            }

            return fallback;
        }

        private bool IsBreakpointActive(BreakpointType breakpoint)
        {
            return breakpoint == BreakpointType.Xs || (int)_breakpoint >= (int)breakpoint;
        }

        private static BreakpointType GetBreakpoint(decimal width)
        {
            if (width >= (int)BreakpointType.Xxl) return BreakpointType.Xxl;
            if (width >= (int)BreakpointType.Xl) return BreakpointType.Xl;
            if (width >= (int)BreakpointType.Lg) return BreakpointType.Lg;
            if (width >= (int)BreakpointType.Md) return BreakpointType.Md;
            if (width >= (int)BreakpointType.Sm) return BreakpointType.Sm;
            return BreakpointType.Xs;
        }

        private void RecalculatePositions()
        {
            var columnHeights = new double[_columnCount];
            var positions = new Dictionary<object, MasonryPosition>();

            for (var index = 0; index < _items.Count; index++)
            {
                var item = _items[index];
                var key = GetItemKey(item, index);
                var column = item.Column ?? Array.IndexOf(columnHeights, columnHeights.Min());
                column = Math.Min(Math.Max(column, 0), _columnCount - 1);
                var top = columnHeights[column];
                positions[key] = new MasonryPosition(column, top);
                columnHeights[column] += _itemHeights.TryGetValue(key, out var height) ? height + _verticalGutter : _verticalGutter;
            }

            _positions = positions;
            _totalHeight = Math.Max(0, columnHeights.Length == 0 ? 0 : columnHeights.Max() - _verticalGutter);
        }

        private async Task NotifyLayoutChangeAsync()
        {
            if (!OnLayoutChange.HasDelegate || _positions.Count != _items.Count)
            {
                return;
            }

            var changed = _positions.Any(pair => !_lastLayout.TryGetValue(pair.Key, out var column) || column != pair.Value.Column)
                || _lastLayout.Count != _positions.Count;
            if (!changed)
            {
                return;
            }

            _lastLayout.Clear();
            foreach (var pair in _positions)
            {
                _lastLayout[pair.Key] = pair.Value.Column;
            }

            await OnLayoutChange.InvokeAsync(_positions.Select(pair => new MasonryLayoutItem
            {
                Key = pair.Key,
                Column = pair.Value.Column,
            }));
        }

        private object GetItemKey(MasonryItem<T> item, int index) => item.Key ?? index;

        internal void SetItemRef(object key, ElementReference element) => _itemRefs[key] = element;

        void IMasonryItemViewParent.SetItemRef(object key, ElementReference element) => SetItemRef(key, element);

        internal void OnItemViewDisposed(object key)
        {
            if (IsDisposed)
            {
                return;
            }

            if (_itemRefs.Remove(key, out var element) && _observedItems.Contains(key))
            {
                _pendingObserverDisposals[key] = element;
            }

            _itemHeights.Remove(key);
            _observedItems.Remove(key);
            _lastLayout.Remove(key);
            _positions.Remove(key);
            _measureRequested = true;
            RecalculatePositions();
            InvokeStateHasChanged();
        }

        void IMasonryItemViewParent.OnItemViewDisposed(object key) => OnItemViewDisposed(key);

        private string GetRootStyle() => $"height: {_totalHeight.ToString(CultureInfo.InvariantCulture)}px; {GetSemanticStyle("root")} {Style}";

        private string GetItemStyle(object key)
        {
            var position = _positions.TryGetValue(key, out var value) ? value : new MasonryPosition(0, 0);
            return $"{GetSemanticStyle("item")} --ant-masonry-item-width: calc((100% + {_horizontalGutter}px) / {_columnCount}); inset-inline-start: calc(var(--ant-masonry-item-width) * {position.Column}); width: calc(var(--ant-masonry-item-width) - {_horizontalGutter}px); top: {position.Top.ToString(CultureInfo.InvariantCulture)}px; position: absolute;";
        }

        private string GetItemClass(object key) => $"{PrefixCls}-item {GetSemanticClass("item")}".Trim();

        private string GetRootClass() => $"{ClassMapper.Class} {GetSemanticClass("root")}".Trim();

        private string GetSemanticClass(string semanticName) => ClassNames != null && ClassNames.TryGetValue(semanticName, out var value) ? value : string.Empty;

        private string GetSemanticStyle(string semanticName) => Styles != null && Styles.TryGetValue(semanticName, out var value) ? value.TrimEnd(';') + ";" : string.Empty;

        private RenderFragment RenderItem(MasonryItem<T> item, int index)
        {
            var key = GetItemKey(item, index);
            var column = _positions.TryGetValue(key, out var position) ? position.Column : 0;
            return item.ChildContent ?? ItemRender?.Invoke(new MasonryItemRenderContext<T>(item, index, column));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DomEventListener?.Dispose();
            }

            base.Dispose(disposing);
        }

        private readonly record struct MasonryPosition(int Column, double Top);
    }
}
