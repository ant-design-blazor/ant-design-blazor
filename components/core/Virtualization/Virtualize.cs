﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NET5_0 || NET_6_0
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace AntDesign
{
    /// <summary>
    /// Provides functionality for rendering a virtualized list of items.
    /// </summary>
    /// <typeparam name="TItem">The <c>context</c> type for the items being rendered.</typeparam>
    public class Virtualize<TItem> : ComponentBase, IVirtualizeJsCallbacks, IAsyncDisposable
    {
        private VirtualizeJsInterop? _jsInterop;

        private ElementReference _spacerBefore;

        private ElementReference _spacerAfter;

        private int _itemsBefore;

        private int _visibleItemCapacity;

        private int _itemCount;

        private int _loadedItemsStartIndex;

        private int _lastRenderedItemCount;

        private int _lastRenderedPlaceholderCount;

        private float _itemSize;

        private IEnumerable<TItem>? _loadedItems;

        private CancellationTokenSource? _refreshCts;

        private Exception? _refreshException;

        private ItemsProviderDelegate<TItem> _itemsProvider = default!;

        private RenderFragment<TItem>? _itemTemplate;

        private RenderFragment<PlaceholderContext>? _placeholder;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        /// <summary>
        /// Gets or sets the item template for the list.
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the item template for the list.
        /// </summary>
        [Parameter]
        public RenderFragment<TItem>? ItemContent { get; set; }

        /// <summary>
        /// Gets or sets the template for items that have not yet been loaded in memory.
        /// </summary>
        [Parameter]
        public RenderFragment<PlaceholderContext>? Placeholder { get; set; }

        /// <summary>
        /// Gets the size of each item in pixels. Defaults to 50px.
        /// </summary>
        [Parameter]
        public float ItemSize { get; set; } = 50f;

        /// <summary>
        /// Gets or sets the function providing items to the list.
        /// </summary>
        [Parameter]
        public ItemsProviderDelegate<TItem>? ItemsProvider { get; set; }

        /// <summary>
        /// Gets or sets the fixed item source.
        /// </summary>
        [Parameter]
        public ICollection<TItem>? Items { get; set; }

        /// <summary>
        /// Gets or sets a value that determines how many additional items will be rendered
        /// before and after the visible region. This help to reduce the frequency of rendering
        /// during scrolling. However, higher values mean that more elements will be present
        /// in the page.
        /// </summary>
        [Parameter]
        public int OverscanCount { get; set; } = 3;

        [Parameter]
        public string SpacerElement { get; set; } = "div";

        [Parameter]
        public float? ContainerSize { get; set; }

        /// <summary>
        /// Instructs the component to re-request data from its <see cref="ItemsProvider"/>.
        /// This is useful if external data may have changed. There is no need to call this
        /// when using <see cref="Items"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
        public async Task RefreshDataAsync()
        {
            // We don't auto-render after this operation because in the typical use case, the
            // host component calls this from one of its lifecycle methods, and will naturally
            // re-render afterwards anyway. It's not desirable to re-render twice.
            await RefreshDataCoreAsync(renderOnSuccess: false);
        }

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            if (ItemSize <= 0)
            {
                throw new InvalidOperationException(
                    $"{GetType()} requires a positive value for parameter '{nameof(ItemSize)}'.");
            }

            if (_itemSize <= 0)
            {
                _itemSize = ItemSize;
            }

            if (ItemsProvider != null)
            {
                if (Items != null)
                {
                    throw new InvalidOperationException(
                        $"{GetType()} can only accept one item source from its parameters. " +
                        $"Do not supply both '{nameof(Items)}' and '{nameof(ItemsProvider)}'.");
                }

                _itemsProvider = ItemsProvider;
            }
            else if (Items != null)
            {
                _itemsProvider = DefaultItemsProvider;

                // When we have a fixed set of in-memory data, it doesn't cost anything to
                // re-query it on each cycle, so do that. This means the developer can add/remove
                // items in the collection and see the UI update without having to call RefreshDataAsync.
                var refreshTask = RefreshDataCoreAsync(renderOnSuccess: false);

                // We know it's synchronous and has its own error handling
                Debug.Assert(refreshTask.IsCompletedSuccessfully);
            }
            else
            {
                throw new InvalidOperationException(
                    $"{GetType()} requires either the '{nameof(Items)}' or '{nameof(ItemsProvider)}' parameters to be specified " +
                    $"and non-null.");
            }

            _itemTemplate = ItemContent ?? ChildContent;
            _placeholder = Placeholder ?? DefaultPlaceholder;
        }

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsInterop = new VirtualizeJsInterop(this, JSRuntime);
                await _jsInterop.InitializeAsync(_spacerBefore, _spacerAfter);
            }
        }

        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (_refreshException != null)
            {
                var oldRefreshException = _refreshException;
                _refreshException = null;

                throw oldRefreshException;
            }

            builder.OpenElement(0, SpacerElement);
            builder.AddAttribute(1, "style", GetSpacerStyle(_itemsBefore));
            builder.AddElementReferenceCapture(2, elementReference => _spacerBefore = elementReference);
            builder.CloseElement();

            var lastItemIndex = Math.Min(_itemsBefore + _visibleItemCapacity, _itemCount);
            var renderIndex = _itemsBefore;
            var placeholdersBeforeCount = Math.Min(_loadedItemsStartIndex, lastItemIndex);

            builder.OpenRegion(3);

            // Render placeholders before the loaded items.
            for (; renderIndex < placeholdersBeforeCount; renderIndex++)
            {
                // This is a rare case where it's valid for the sequence number to be programmatically incremented.
                // This is only true because we know for certain that no other content will be alongside it.
                builder.AddContent(renderIndex, _placeholder, new PlaceholderContext(renderIndex, _itemSize));
            }

            builder.CloseRegion();

            _lastRenderedItemCount = 0;

            // Render the loaded items.
            if (_loadedItems != null && _itemTemplate != null)
            {
                var itemsToShow = _loadedItems
                    .Skip(_itemsBefore - _loadedItemsStartIndex)
                    .Take(lastItemIndex - _loadedItemsStartIndex);

                builder.OpenRegion(4);

                foreach (var item in itemsToShow)
                {
                    _itemTemplate(item)(builder);
                    _lastRenderedItemCount++;
                }

                renderIndex += _lastRenderedItemCount;

                builder.CloseRegion();
            }

            _lastRenderedPlaceholderCount = Math.Max(0, lastItemIndex - _itemsBefore - _lastRenderedItemCount);

            builder.OpenRegion(5);

            // Render the placeholders after the loaded items.
            for (; renderIndex < lastItemIndex; renderIndex++)
            {
                builder.AddContent(renderIndex, _placeholder, new PlaceholderContext(renderIndex, _itemSize));
            }

            builder.CloseRegion();

            var itemsAfter = Math.Max(0, _itemCount - _visibleItemCapacity - _itemsBefore);

            builder.OpenElement(6, SpacerElement);
            builder.AddAttribute(7, "style", GetSpacerStyle(itemsAfter));
            builder.AddElementReferenceCapture(8, elementReference => _spacerAfter = elementReference);

            builder.CloseElement();
        }

        private string GetSpacerStyle(int itemsInSpacer)
            => $"height: {(itemsInSpacer * _itemSize).ToString(CultureInfo.InvariantCulture)}px;";

        void IVirtualizeJsCallbacks.OnBeforeSpacerVisible(float spacerSize, float spacerSeparation, float containerSize)
        {
            if (ContainerSize != null) containerSize = ContainerSize.Value;
            CalcualteItemDistribution(spacerSize, spacerSeparation, containerSize, out var itemsBefore, out var visibleItemCapacity);

            // Since we know the before spacer is now visible, we absolutely have to slide the window up
            // by at least one element. If we're not doing that, the previous item size info we had must
            // have been wrong, so just move along by one in that case to trigger an update and apply the
            // new size info.
            if (itemsBefore == _itemsBefore && itemsBefore > 0)
            {
                itemsBefore--;
            }

            UpdateItemDistribution(itemsBefore, visibleItemCapacity);
        }

        void IVirtualizeJsCallbacks.OnAfterSpacerVisible(float spacerSize, float spacerSeparation, float containerSize)
        {
            CalcualteItemDistribution(spacerSize, spacerSeparation, containerSize, out var itemsAfter, out var visibleItemCapacity);

            var itemsBefore = Math.Max(0, _itemCount - itemsAfter - visibleItemCapacity);

            // Since we know the after spacer is now visible, we absolutely have to slide the window down
            // by at least one element. If we're not doing that, the previous item size info we had must
            // have been wrong, so just move along by one in that case to trigger an update and apply the
            // new size info.
            if (itemsBefore == _itemsBefore && itemsBefore < _itemCount - visibleItemCapacity)
            {
                itemsBefore++;
            }

            UpdateItemDistribution(itemsBefore, visibleItemCapacity);
        }

        private void CalcualteItemDistribution(
            float spacerSize,
            float spacerSeparation,
            float containerSize,
            out int itemsInSpacer,
            out int visibleItemCapacity)
        {
            if (_lastRenderedItemCount > 0)
            {
                _itemSize = (spacerSeparation - (_lastRenderedPlaceholderCount * _itemSize)) / _lastRenderedItemCount;
            }

            if (_itemSize <= 0)
            {
                // At this point, something unusual has occurred, likely due to misuse of this component.
                // Reset the calculated item size to the user-provided item size.
                _itemSize = ItemSize;
            }

            itemsInSpacer = Math.Max(0, (int)Math.Floor(spacerSize / _itemSize) - OverscanCount);
            visibleItemCapacity = (int)Math.Ceiling(containerSize / _itemSize) + 2 * OverscanCount;
        }

        private void UpdateItemDistribution(int itemsBefore, int visibleItemCapacity)
        {
            if (itemsBefore != _itemsBefore || visibleItemCapacity != _visibleItemCapacity)
            {
                _itemsBefore = itemsBefore;
                _visibleItemCapacity = visibleItemCapacity;
                var refreshTask = RefreshDataCoreAsync(renderOnSuccess: true);

                if (!refreshTask.IsCompleted)
                {
                    StateHasChanged();
                }
            }
        }

        private async ValueTask RefreshDataCoreAsync(bool renderOnSuccess)
        {
            _refreshCts?.Cancel();
            CancellationToken cancellationToken;

            if (_itemsProvider == DefaultItemsProvider)
            {
                // If we're using the DefaultItemsProvider (because the developer supplied a fixed
                // Items collection) we know it will complete synchronously, and there's no point
                // instantiating a new CancellationTokenSource
                _refreshCts = null;
                cancellationToken = CancellationToken.None;
            }
            else
            {
                _refreshCts = new CancellationTokenSource();
                cancellationToken = _refreshCts.Token;
            }

            var request = new ItemsProviderRequest(_itemsBefore, _visibleItemCapacity, cancellationToken);

            try
            {
                var result = await _itemsProvider(request);

                // Only apply result if the task was not canceled.
                if (!cancellationToken.IsCancellationRequested)
                {
                    _itemCount = result.TotalItemCount;
                    _loadedItems = result.Items;
                    _loadedItemsStartIndex = request.StartIndex;

                    if (renderOnSuccess)
                    {
                        StateHasChanged();
                    }
                }
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException oce && oce.CancellationToken == cancellationToken)
                {
                    // No-op; we canceled the operation, so it's fine to suppress this exception.
                }
                else
                {
                    // Cache this exception so the renderer can throw it.
                    _refreshException = e;

                    // Re-render the component to throw the exception.
                    StateHasChanged();
                }
            }
        }

        private ValueTask<ItemsProviderResult<TItem>> DefaultItemsProvider(ItemsProviderRequest request)
        {
            return ValueTask.FromResult(new ItemsProviderResult<TItem>(
                Items!.Skip(request.StartIndex).Take(request.Count),
                Items!.Count));
        }

        private RenderFragment DefaultPlaceholder(PlaceholderContext context) => (builder) =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "style", $"height: {_itemSize.ToString(CultureInfo.InvariantCulture)}px;");
            builder.CloseElement();
        };

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            _refreshCts?.Cancel();

            if (_jsInterop != null)
            {
                await _jsInterop.DisposeAsync();
            }
        }
    }
}
#endif
