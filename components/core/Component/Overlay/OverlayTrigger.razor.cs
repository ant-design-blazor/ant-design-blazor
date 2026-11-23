// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.Core.Helpers;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace AntDesign.Internal
{
    public partial class OverlayTrigger : AntDomComponentBase
    {
        internal virtual string GetOverlayAdditionalClass() => string.Empty;

        internal virtual string GetOverlayAdditionalStyle() => string.Empty;

        // Custom triggers can render the anchor itself rather than a wrapper.
        internal virtual bool IsTriggerWrappedInDiv => Unbound is null;

        [CascadingParameter(Name = "PrefixCls")]
        [Parameter]
        public string PrefixCls { get; set; } = "ant-dropdown";

        /// <summary>
        /// Optional direction override for components that expose a local
        /// direction setting. When omitted, the ConfigProvider direction is used.
        /// </summary>
        [Parameter]
        public string Direction { get; set; }

        internal bool IsRTL => string.IsNullOrWhiteSpace(Direction)
            ? RTL
            : string.Equals(Direction, "rtl", StringComparison.OrdinalIgnoreCase);

        /// <summary>CSS transition class prefix for the popup motion.</summary>
        [Parameter]
        public string TransitionName { get; set; }

        /// <summary>
        /// Optional animation class configuration for the overlay. When omitted,
        /// the placement-aware default slide animation is used.
        /// </summary>
        [Parameter]
        public OverlayAnimationOptions OverlayAnimation { get; set; }

        /// <summary>Delay in seconds before opening on hover.</summary>
        [Parameter]
        public double MouseEnterDelay { get; set; }

        /// <summary>Delay in seconds before closing after hover ends.</summary>
        [Parameter]
        public double MouseLeaveDelay { get; set; }

        private string _popupContainerSelectorFromCascade = "";

        [CascadingParameter(Name = "PopupContainerSelector")]
        public string PopupContainerSelectorFromCascade
        {
            get
            {
                return _popupContainerSelectorFromCascade;
            }
            set
            {
                _popupContainerSelectorFromCascade = value;
                PopupContainerSelector = value;
            }
        }

        /// <summary>
        /// Overlay adjustment strategy (when for example browser resize is happening). Check
        /// enum for details.
        /// </summary>
        /// <default value="TriggerBoundaryAdjustMode.InView" />
        [Parameter]
        public TriggerBoundaryAdjustMode BoundaryAdjustMode { get; set; } = TriggerBoundaryAdjustMode.InView;

        /// <summary>
        /// Trigger (link, button, etc)
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Whether need both auto-off and Visible control closing.
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool ComplexAutoCloseAndVisible { get; set; } = false;

        /// <summary>
        /// Whether the trigger is disabled.
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Property forwarded to Overlay component. Consult the Overlay
        /// property for more detailed explanation.
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool HiddenMode { get; set; } = false;

        /// <summary>
        /// Whether the popup content is removed from the DOM after it closes.
        /// </summary>
        [Parameter]
        public bool DestroyOnHidden { get; set; }

        /// <summary>Use the popup's CSS token for its stacking level.</summary>
        [Parameter] public bool UseCssZIndex { get; set; }

        /// <summary>Display mode forwarded to Overlay; hidden popups remain hidden.</summary>
        [Parameter] public string OverlayDisplayStyle { get; set; }


        /// <summary>
        /// (not used in Unbound) Sets wrapping div style to `display: inline-flex;`.
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool InlineFlexMode { get; set; } = false;

        /// <summary>
        /// Behave like a button: when clicked invoke OnClick
        /// (unless OnClickDiv is overriden and does not call base).
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool IsButton { get; set; } = false;

        /// <summary>
        /// Callback when triggger is clicked
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        /// <summary>Descendant controls whose clicks must not toggle an unbound trigger.</summary>
        [Parameter] public string ClickIgnoreSelector { get; set; }

        /// <summary>
        ///  Callback - equivalent of OnMouseUp event on the trigger trigger.
        /// </summary>
        [Parameter]
        public EventCallback OnMaskClick { get; set; }

        /// <summary>
        /// Whether an outside click closes the popup.
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool CloseOnOutsideClick { get; set; } = true;

        /// <summary>
        /// Callback when mouse enters trigger boundaries.
        /// </summary>
        [Parameter]
        public EventCallback OnMouseEnter { get; set; }

        /// <summary>
        /// Callback when mouse leaves trigger boundaries.
        /// </summary>
        [Parameter]
        public EventCallback OnMouseLeave { get; set; }

        /// <summary>
        /// Callback when overlay is hiding.
        /// </summary>
        [Parameter]
        public EventCallback<bool> OnOverlayHiding { get; set; }

        /// <summary>
        /// Callback when overlay visibility is changing.
        /// </summary>
        [Obsolete("Use VisibleChanged instead")]
        [Parameter]
        public EventCallback<bool> OnVisibleChange { get; set; }

        /// <summary>
        /// Callback invoked after the popup has been positioned but before it is revealed.
        /// Components can synchronize trigger content with the opening state before the
        /// popup's enter motion becomes visible.
        /// </summary>
        [Parameter]
        public EventCallback<bool> OnBeforeVisibleChange { get; set; }

        /// <summary>
        /// Overlay content (what will be rendered after trigger is activated)
        /// </summary>
        [Parameter]
        public RenderFragment Overlay { get; set; }

        /// <summary>
        /// Overlay container custom css class.
        /// </summary>
        [Parameter]
        public string OverlayClassName { get; set; }

        /// <summary>
        /// Css class added to overlay when overlay is shown.
        /// </summary>
        [Parameter]
        public string OverlayEnterCls { get; set; }

        /// <summary>
        /// Css class added to overlay when overlay is hidden.
        /// </summary>
        [Parameter]
        public string OverlayHiddenCls { get; set; }

        /// <summary>
        /// Css class added to overlay when overlay is hiding.
        /// </summary>
        [Parameter]
        public string OverlayLeaveCls { get; set; }

        /// <summary>
        /// Css style that will be added to overlay div.
        /// </summary>
        [Parameter]
        public string OverlayStyle { get; set; }

        /// <summary>
        /// Opens a pre-rendered overlay on pointer down before a Blazor Server
        /// event round trip completes. Intended for controls such as Select
        /// whose popup is already present in the DOM in HiddenMode.
        /// </summary>
        [Parameter]
        public bool ImmediateOpenOnPointerDown { get; set; }

        /// <summary>Width strategy applied before an immediate-open popup is positioned and revealed.</summary>
        [Parameter]
        public string ImmediateOpenWidthMode { get; set; }

        /// <summary>
        /// Width strategy used before an overlay becomes visible. Defaults to
        /// matching the trigger; set to <c>content</c> to size to the content
        /// while keeping at least the trigger width, <c>content-only</c> to
        /// size to content without a trigger-width minimum, or <c>custom</c> to use
        /// <see cref="PopupWidth"/>.
        /// </summary>
        [Parameter]
        public string PopupWidthMode { get; set; } = "match-trigger";

        /// <summary>Explicit width used when <see cref="PopupWidthMode"/> is <c>custom</c>.</summary>
        [Parameter]
        public string PopupWidth { get; set; }

        /// <summary>Optional maximum width applied while measuring the popup.</summary>
        [Parameter]
        public string PopupMaxWidth { get; set; }

        internal string EffectivePopupWidthMode => string.IsNullOrWhiteSpace(ImmediateOpenWidthMode)
            ? PopupWidthMode
            : ImmediateOpenWidthMode;

        internal string EffectivePopupWidth => string.IsNullOrWhiteSpace(ImmediateOpenWidth)
            ? PopupWidth
            : ImmediateOpenWidth;

        internal string EffectivePopupMaxWidth => string.IsNullOrWhiteSpace(ImmediateOpenMaxWidth)
            ? PopupMaxWidth
            : ImmediateOpenMaxWidth;

        /// <summary>Explicit width used when <see cref="ImmediateOpenWidthMode"/> is <c>custom</c>.</summary>
        [Parameter]
        public string ImmediateOpenWidth { get; set; }

        /// <summary>Optional maximum width applied while measuring an immediate-open popup.</summary>
        [Parameter]
        public string ImmediateOpenMaxWidth { get; set; }

        /*
         * 当前的placement，某些情况下可能会被Overlay组件修改（通过ChangePlacementForShow函数）
         * Current placement, would change by overlay in some cases(via ChangePlacementForShow function)
         */
        private PlacementType _actualPlacement = PlacementType.BottomLeft;

        /// <summary>
        /// The position of the Dropdown overlay relative to the target. 
        /// Can be: Top, Left, Right, Bottom, TopLeft, TopRight, BottomLeft, BottomRight, LeftTop, LeftBottom, RightTop, RightBottom
        /// </summary>
        /// <default value="Placement.BottomLeft" />
        [Parameter]
        public Placement Placement { get; set; } = Placement.BottomLeft;

        private Placement? _placementWithRTL = null;
        private Placement? _lastRequestedPlacement;

        internal Placement PlacementWithRTL => (Placement)_placementWithRTL;

        internal PlacementType GetPlacementType() => _actualPlacement;

        /// <summary>
        /// Override default placement class which is based on `Placement` parameter.
        /// </summary>
        [Parameter]
        public string PlacementCls { get; set; }

        /// <summary>
        /// Define what is going to be the container of the overlay.
        /// Example use case: when overlay has to be contained in a
        /// scrollable area.
        /// </summary>
        /// <default value="body" />
        [Parameter]
        public string PopupContainerSelector { get; set; } = "body";

        /// <summary>
        /// Select the popup container from the trigger element, matching React's
        /// getPopupContainer callback. Use PopupContainerSelector when a CSS
        /// selector is more convenient.
        /// </summary>
        [Parameter]
        public Func<ElementReference, ElementReference> GetPopupContainer { get; set; }

        private ElementReference? _popupContainerElement;
        internal object EffectivePopupContainer => _popupContainerElement.HasValue
            ? _popupContainerElement.Value
            : PopupContainerSelector;

        /// <summary>
        /// Trigger mode. Could be multiple by passing an array.
        /// </summary>
        /// <default value="TriggerType.Hover" />
        [Parameter]
        public Trigger[] Trigger //TODO: this should probably be a flag not an array
        {
            get
            {
                return [.. _trigger.Select(t => t.Trigger)];
            }
            set
            {
                _trigger = [.. value.Select(TriggerType.Create)];
            }
        }

        /// <summary>
        /// The trigger element CSS class.
        /// </summary>
        /// <default value="ant-dropdown-trigger" />
        [Parameter]
        public string TriggerCls { get; set; }

        /// <summary>
        /// Manually set reference to triggering element.
        /// </summary>
        [Parameter]
        public ElementReference TriggerReference
        {
            get => Ref;
            set => Ref = value;
        }

        internal TriggerType[] GetTriggerType() => _trigger;

        /// <summary>
        /// ChildElement with ElementReference set to avoid wrapping div.
        /// </summary>
        [Parameter]
        public RenderFragment<ForwardRef> Unbound { get; set; }

        /// <summary>
        /// Toggles overlay viability.
        /// </summary>
        /// <default value="false" />
        private bool _visible;

        [Parameter]
        public bool Visible
        {
            get => _visible;
            set => _visible = value;
        }

        /// <summary>
        /// Whether the overlay is open. This is the canonical state parameter;
        /// <see cref="Visible"/> remains available for compatibility.
        /// </summary>
        [Parameter]
        public bool Open
        {
            get => _visible;
            set => _visible = value;
        }

        /// <summary>
        /// Callback when visibility is changed.
        /// </summary>
        [Parameter]
        public EventCallback<bool> VisibleChanged { get; set; }

        /// <summary>
        /// Callback invoked when <see cref="Open"/> changes.
        /// </summary>
        [Parameter]
        public EventCallback<bool> OpenChanged { get; set; }

        /// <summary>
        /// Callback invoked when the overlay open state changes.
        /// </summary>
        [Parameter]
        public EventCallback<bool> OnOpenChange { get; set; }

        internal bool HasVisibilityChangeCallback => VisibleChanged.HasDelegate || OpenChanged.HasDelegate || OnOpenChange.HasDelegate || OnVisibleChange.HasDelegate;

        [Inject]
        protected IDomEventListener DomEventListener { get; set; }

        private bool _mouseInTrigger = false;
        private bool _mouseInOverlay = false;
        private int _overlayMouseEnterCount = 0;

        protected Overlay _overlay = null;
        private TriggerType[] _trigger = [TriggerType.Hover];
        private bool _shouldRender = true;
        private Placement? _lastRenderedPlacement;
        private CancellationTokenSource _mouseEnterDelayCancellation;
        private CancellationTokenSource _mouseLeaveDelayCancellation;

        internal void SetShouldRender(bool shouldRender) => _shouldRender = shouldRender;

        public OverlayTrigger()
        {
            ClassMapper.Add("antblazor-overlay-trigger-wrapper");
        }

        protected override bool ShouldRender()
        {
            if (_lastRenderedPlacement != Placement)
            {
                _lastRenderedPlacement = Placement;
                _shouldRender = true;
                return base.ShouldRender();
            }

            if (_shouldRender)
                return base.ShouldRender();
            _shouldRender = true;
            return false;
        }

        internal bool CheckPlacementChanged()
        {
            var newPlacement = IsRTL ? Placement.GetRTLPlacement() : Placement;
            if (_placementWithRTL != newPlacement)
            {
                _placementWithRTL = newPlacement;
                return true;
            }
            return false;
        }

        protected override void OnParametersSet()
        {
            var requestedPlacement = IsRTL ? Placement.GetRTLPlacement() : Placement;
            if (_lastRequestedPlacement != requestedPlacement)
            {
                _lastRequestedPlacement = requestedPlacement;
                _placementWithRTL = requestedPlacement;
                _actualPlacement = PlacementType.Create(requestedPlacement);
            }

            base.OnParametersSet();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            CheckPlacementChanged();
            _actualPlacement = PlacementType.Create(PlacementWithRTL);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                DomEventListener.AddShared<JsonElement>("document", "mouseup", OnMouseUp);
                // Some input paths (touch and synthetic activation in
                // particular) do not produce a document mouseup. The click
                // listener covers those while the bounds checks below keep
                // clicks inside the trigger or popup from dismissing it.
                DomEventListener.AddShared<JsonElement>("document", "click", OnMouseUp);
            }

            base.OnAfterRender(firstRender);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var isUnbound = Unbound != null || (ChildContent == null && Ref.Id != null);

                if (isUnbound)
                {
                    // A component can supply a more precise anchor than the
                    // element rendered by its Unbound fragment. Keep that
                    // explicit reference; otherwise use the Unbound element.
                    if (Ref.Id is null)
                    {
                        Ref = RefBack.Current;
                    }

                    DomEventListener.AddExclusive<JsonElement>(Ref, "click", OnUnboundClick, ignoreSelector: ClickIgnoreSelector);
                    DomEventListener.AddExclusive<JsonElement>(Ref, "mouseover", OnUnboundMouseEnter);
                    DomEventListener.AddExclusive<JsonElement>(Ref, "mouseout", OnUnboundMouseLeave);
                    DomEventListener.AddExclusive<JsonElement>(Ref, "focusin", OnUnboundFocusIn);
                    DomEventListener.AddExclusive<JsonElement>(Ref, "focusout", OnUnboundFocusOut);

                    if (IsContainTrigger(TriggerType.ContextMenu))
                    {
                        DomEventListener.AddExclusive<JsonElement>(Ref, "contextmenu", OnContextMenu, true);
                    }
                }

                if (!string.IsNullOrWhiteSpace(TriggerCls))
                {
                    if (isUnbound)
                    {
                        await Js.InvokeVoidAsync(JSInteropConstants.StyleHelper.AddCls, Ref, TriggerCls);
                    }
                    else if (ChildContent != null)
                    {
                        await Js.InvokeVoidAsync(JSInteropConstants.StyleHelper.AddClsToFirstChild, Ref, TriggerCls);
                    }
                }
            }

            if (firstRender && ImmediateOpenOnPointerDown)
            {
                await _overlay.BindImmediateOpenAsync(ClickIgnoreSelector,
                    EffectivePopupWidthMode, EffectivePopupWidth, EffectivePopupMaxWidth);
            }

            await base.OnAfterRenderAsync(firstRender);

            if (firstRender && GetPopupContainer != null)
            {
                var container = GetPopupContainer(Ref);
                if (container.Id != null)
                {
                    _popupContainerElement = container;
                    StateHasChanged();
                }
            }
        }

        protected Task OnUnboundMouseEnter(JsonElement jsonElement) => OnTriggerMouseEnter();

        protected Task OnUnboundMouseLeave(JsonElement jsonElement) => OnTriggerMouseLeave();

        protected Task OnUnboundFocusIn(JsonElement jsonElement) => OnTriggerFocusIn();

        protected Task OnUnboundFocusOut(JsonElement jsonElement) => OnTriggerFocusOut();

        protected async Task OnUnboundClick(JsonElement jsonElement)
        {
            var eventArgs = JsonSerializer.Deserialize<MouseEventArgs>(jsonElement.ToString(), JsonSerializerHelper.DefaultOptions);
            await OnClickDiv(eventArgs);
        }

        protected async Task OnContextMenu(JsonElement jsonElement)
        {
            var eventArgs = JsonSerializer.Deserialize<MouseEventArgs>(jsonElement.ToString(), JsonSerializerHelper.DefaultOptions);

            await OnTriggerContextmenu(eventArgs);
        }

        protected override void Dispose(bool disposing)
        {
            CancelMouseDelay(ref _mouseEnterDelayCancellation);
            CancelMouseDelay(ref _mouseLeaveDelayCancellation);
            DomEventListener?.Dispose();

            base.Dispose(disposing);
        }

        protected virtual async Task OnTriggerMouseEnter()
        {
            _mouseInTrigger = true;
            CancelMouseDelay(ref _mouseLeaveDelayCancellation);

            if (_overlay != null && IsContainTrigger(TriggerType.Hover))
            {
                await ShowAfterMouseDelay();
            }
            else
            {
                _shouldRender = false;
            }

            if (OnMouseEnter.HasDelegate)
                _ = OnMouseEnter.InvokeAsync(null);
        }

        protected virtual async Task OnTriggerMouseLeave()
        {
            _mouseInTrigger = false;
            CancelMouseDelay(ref _mouseEnterDelayCancellation);

            if (_overlay != null && IsContainTrigger(TriggerType.Hover) && IsOverlayShow())
            {
                await HideAfterMouseDelay();
            }
            else
            {
                _shouldRender = false;
            }

            if (OnMouseLeave.HasDelegate)
                OnMouseLeave.InvokeAsync(null);
        }

        protected virtual async Task OnTriggerFocusIn()
        {
            _mouseInTrigger = true;

            if (_overlay != null && IsContainTrigger(TriggerType.Focus))
            {
                await Show();
            }
        }

        protected virtual async Task OnTriggerFocusOut()
        {
            _mouseInTrigger = false;

            if (_overlay != null && IsContainTrigger(TriggerType.Focus))
            {
                await Hide();
            }
        }

        protected virtual void OnOverlayMouseEnter()
        {
            CancelMouseDelay(ref _mouseLeaveDelayCancellation);
            _overlayMouseEnterCount++;
            _mouseInOverlay = true;

            if (_overlay != null && IsContainTrigger(TriggerType.Hover))
            {
                _overlay.SetMouseInOverlay(true);
            }
        }

        protected virtual async Task OnOverlayMouseLeave()
        {
            _overlayMouseEnterCount--;

            // Only hide when all enter events have been matched with leave events.
            // This guards against spurious mouseleave events fired by the browser
            // (e.g., during CSS transitions or in Chromium 149+) that would otherwise
            // cause the overlay to disappear while the cursor is still over it.
            if (_overlayMouseEnterCount > 0)
            {
                _mouseInOverlay = true;
                return;
            }

            _mouseInOverlay = false;

            if (_overlay != null && IsContainTrigger(TriggerType.Hover))
            {
                _overlay.SetMouseInOverlay(_mouseInTrigger);
                await HideAfterMouseDelay();
            }
        }

        protected virtual void OnOverlayMouseUp()
        {
            // The popup stops mouseup propagation, so a document listener cannot
            // consume a corresponding marker. Outside clicks are checked by bounds.
        }

        private async Task ShowAfterMouseDelay()
        {
            CancelMouseDelay(ref _mouseEnterDelayCancellation);
            if (MouseEnterDelay > 0)
            {
                var cancellation = new CancellationTokenSource();
                _mouseEnterDelayCancellation = cancellation;
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(MouseEnterDelay), cancellation.Token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                if (cancellation.IsCancellationRequested || !_mouseInTrigger) return;
            }
            await Show();
        }

        private async Task HideAfterMouseDelay()
        {
            CancelMouseDelay(ref _mouseLeaveDelayCancellation);
            if (MouseLeaveDelay > 0)
            {
                var cancellation = new CancellationTokenSource();
                _mouseLeaveDelayCancellation = cancellation;
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(MouseLeaveDelay), cancellation.Token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                if (cancellation.IsCancellationRequested || _mouseInTrigger || _mouseInOverlay) return;
            }
            await Hide();
        }

        private static void CancelMouseDelay(ref CancellationTokenSource cancellation)
        {
            if (cancellation == null) return;
            cancellation.Cancel();
            cancellation.Dispose();
            cancellation = null;
        }

        /// <summary>
        /// Handle the trigger click.
        /// </summary>
        /// <param name="args">MouseEventArgs</param>
        /// <returns></returns>
        protected virtual async Task OnClickDiv(MouseEventArgs args)
        {
            if (!IsButton)
            {
                await OnTriggerClick();
            }
            else
            {
                await OnClick.InvokeAsync(args);
            }
        }

        protected virtual async Task OnTriggerClick()
        {
            //_mouseInTrigger might have been set by a different event,
            //so track that to know if should be reset at the end of this method
            bool resetMouseInTrigger = !_mouseInTrigger;
            _mouseInTrigger = true;
            if (IsContainTrigger(TriggerType.Click))
            {
                if (_overlay.IsPopup())
                {
                    _overlay.PreventHide(false);
                    await Hide();
                }
                else
                {
                    await Show();
                }
            }
            else if (IsContainTrigger(TriggerType.ContextMenu) && _overlay.IsPopup())
            {
                _overlay.PreventHide(false);
                await Hide();
            }
            if (resetMouseInTrigger)
            {
                _mouseInTrigger = false;
            }
        }

        protected virtual async Task OnTriggerContextmenu(MouseEventArgs args)
        {
            if (IsContainTrigger(TriggerType.ContextMenu))
            {
                int offsetX = 10;
                int offsetY = 10;
#if NET5_0_OR_GREATER
                // offsetX/offsetY were only supported in Net5 or grater
                offsetX = (int)args.OffsetX;
                offsetY = (int)args.OffsetY;
#endif

                await Hide();
                await Show(offsetX, offsetY);
            }
        }

        /// <summary>
        /// on document mouse up
        /// </summary>
        /// <param name="element"></param>
        protected virtual async Task OnMouseUp(JsonElement element)
        {
            if (element.TryGetProperty("__antImmediateOpenTriggerClick", out var immediateOpenTriggerClick)
                && immediateOpenTriggerClick.ValueKind == JsonValueKind.True)
            {
                return;
            }

            if (!IsOverlayShow())
            {
                return;
            }

            // Click-triggered overlays (including Select) must close on an
            // outside pointer release even if the asynchronous mouseenter /
            // mouseleave callbacks have left _mouseInTrigger stale. Use live
            // bounds for both the trigger and popup as the source of truth.
            if (_mouseInTrigger == false || IsContainTrigger(TriggerType.Click))
            {
                // Document mouseup and the trigger click are dispatched as
                // separate Blazor Server events. Under latency, the click can
                // open the popup before this mouseup callback arrives, making
                // an in-trigger mouseup look like an outside click. Check the
                // original pointer coordinates against the trigger before
                // closing; this also keeps click-open overlays stable when the
                // mouseenter callback arrives later than the click callback.
                if (element.TryGetProperty("clientX", out var clientX)
                    && element.TryGetProperty("clientY", out var clientY))
                {
                    var pointerX = clientX.GetDouble();
                    var pointerY = clientY.GetDouble();
                    if (await IsPointerWithinAsync(Ref, pointerX, pointerY)
                        || (_overlay != null && await IsPointerWithinAsync(_overlay.Ref, pointerX, pointerY)))
                    {
                        return;
                    }
                }

                if (!CloseOnOutsideClick)
                {
                    return;
                }

                if (OnMaskClick.HasDelegate)
                {
                    _ = OnMaskClick.InvokeAsync(null);
                }
                _overlay?.PreventHide(false);
                _ = Hide();
            }
        }

        private async Task<bool> IsPointerWithinAsync(ElementReference element, double x, double y)
        {
            if (element.Id == null)
            {
                return false;
            }

            var rect = await Js.InvokeAsync<JsonElement>(JSInteropConstants.GetBoundingClientRect, element);
            return rect.TryGetProperty("left", out var left)
                && rect.TryGetProperty("right", out var right)
                && rect.TryGetProperty("top", out var top)
                && rect.TryGetProperty("bottom", out var bottom)
                && x >= left.GetDouble()
                && x <= right.GetDouble()
                && y >= top.GetDouble()
                && y <= bottom.GetDouble();
        }

        protected virtual bool IsContainTrigger(TriggerType triggerType)
        {
            return _trigger.Contains(triggerType);
        }


        protected virtual async Task OverlayHiding(bool visible)
        {
            await OnOverlayHiding.InvokeAsync(visible);
        }

        protected virtual void OnOverlayShow()
        {
        }

        protected virtual void OnOverlayHide()
        {
        }

        internal void ChangePlacementForShow(PlacementType placement)
        {
            _actualPlacement = placement;
        }

        internal virtual string GetArrowClass() => "";

        internal virtual string GetPlacementClass()
        {
            if (!string.IsNullOrEmpty(PlacementCls))
            {
                return PlacementCls;
            }
            return $"{PrefixCls}-placement-{_actualPlacement.Name}";
        }

        internal virtual string GetOverlayEnterClass()
        {
            var configuredEnterClass = OverlayAnimation?.GetEnterClass(_actualPlacement.Placement);
            if (!string.IsNullOrWhiteSpace(configuredEnterClass))
            {
                return configuredEnterClass;
            }

            if (!string.IsNullOrEmpty(OverlayEnterCls))
            {
                return OverlayEnterCls;
            }
            var transitionName = string.IsNullOrWhiteSpace(TransitionName)
                ? $"ant-slide-{_actualPlacement.SlideName}"
                : TransitionName;
            return $"{transitionName}-enter {transitionName}-enter-active {transitionName}";
        }

        internal virtual string GetOverlayLeaveClass()
        {
            if (!string.IsNullOrEmpty(OverlayLeaveCls))
            {
                return OverlayLeaveCls;
            }

            var configuredLeaveClass = OverlayAnimation?.GetLeaveClass(_actualPlacement.Placement);
            if (!string.IsNullOrWhiteSpace(configuredLeaveClass))
            {
                return configuredLeaveClass;
            }

            var leavePlacement = _actualPlacement.GetReverseType();
            var transitionName = string.IsNullOrWhiteSpace(TransitionName)
                ? $"ant-slide-{leavePlacement.SlideName}"
                : TransitionName;
            return $"{transitionName}-leave {transitionName}-leave-active {transitionName}";
        }

        internal virtual string GetOverlayHiddenClass()
        {
            if (!string.IsNullOrEmpty(OverlayHiddenCls))
            {
                return OverlayHiddenCls;
            }
            return $"{PrefixCls}-hidden";
        }

        internal virtual async Task Show(int? overlayLeft = null, int? overlayTop = null)
        {
            if (!_mouseInTrigger && IsContainTrigger(TriggerType.Hover) && !Visible)
            {
                return;
            }

            // Reset the enter/leave counter for a new overlay session
            _overlayMouseEnterCount = 0;

            await _overlay.Show(overlayLeft, overlayTop);
        }

        internal virtual async Task Hide(bool force = false)
        {
            if (Visible && !ComplexAutoCloseAndVisible && !force)
            {
                return;
            }

            await _overlay.Hide(force);
        }

        internal Overlay GetOverlayComponent()
        {
            return _overlay;
        }

        internal async Task<HtmlElement> GetTriggerDomInfo()
        {
            return await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetFirstChildDomInfo, Ref);
        }

        /// <summary>
        /// Will hide the overlay.
        /// </summary>
        /// <returns></returns>
        public async Task Close()
        {
            await _overlay.Hide(true);
        }

        /// <summary>
        /// Checks if overlay is currently in visible state.
        /// </summary>
        /// <returns></returns>
        public bool IsOverlayShow()
        {
            return _overlay != null ? _overlay.IsPopup() : false;
        }

        internal bool IsOverlayOpening()
        {
            return _overlay?.IsOpening() ?? false;
        }

        /// <summary>
        /// Toggle overlay visibility.
        /// </summary>
        /// <param name="visible">boolean: visibility true/false</param>
        internal void SetVisible(bool visible) => _visible = visible;
    }
}
