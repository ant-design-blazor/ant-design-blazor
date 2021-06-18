using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign.Internal
{
    public partial class OverlayTrigger : AntDomComponentBase
    {
        [CascadingParameter(Name = "PrefixCls")]
        public string PrefixCls { get; set; } = "ant-dropdown";

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
        /// Overlay adjustment strategy (when for example browser resize is happening)
        /// </summary>
        [Parameter]
        public TriggerBoundaryAdjustMode BoundaryAdjustMode { get; set; } = TriggerBoundaryAdjustMode.InView;

        /// <summary>
        /// Trigger (link, button, etc) 
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// 自动关闭功能和Visible参数同时生效
        /// Both auto-off and Visible control close
        /// </summary>
        [Parameter]
        public bool ComplexAutoCloseAndVisible { get; set; } = false;

        /// <summary>
        /// Whether the trigger is disabled.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Property forwarded to Overlay component. Consult the Overlay
        /// property for more detailed explanation.
        /// </summary>
        [Parameter]
        public bool HiddenMode { get; set; } = false;

        /// <summary>
        /// (not used in Unbound) Sets wrapping div style to `display: inline-flex;`.
        /// </summary>
        [Parameter]
        public bool InlineFlexMode { get; set; } = false;

        /// <summary>
        /// Behave like a button: when clicked invoke OnClick 
        /// (unless OnClickDiv is overriden and does not call base).
        /// </summary>
        [Parameter]
        public bool IsButton { get; set; } = false;

        /// <summary>
        /// Callback when triggger is clicked
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        /// <summary>
        ///  Callback - equivalent of OnMouseUp event on the trigger trigger.
        /// </summary>
        [Parameter]
        public EventCallback OnMaskClick { get; set; }

        /// <summary>
        /// Callback when mouse enters trigger boundaries.
        /// </summary>
        [Parameter] public Action OnMouseEnter { get; set; }

        /// <summary>
        /// Callback when mouse leaves trigger boundaries.
        /// </summary>
        [Parameter] public Action OnMouseLeave { get; set; }

        /// <summary>
        /// Callback when overlay is hiding.
        /// </summary>
        [Parameter]
        public EventCallback<bool> OnOverlayHiding { get; set; }

        /// <summary>
        /// Callback when overlay visibility is changing. 
        /// </summary>
        [Parameter]
        public EventCallback<bool> OnVisibleChange { get; set; }

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

        /*
         * 当前的placement，某些情况下可能会被Overlay组件修改（通过ChangePlacementForShow函数）
         * Current placement, would change by overlay in some cases(via ChangePlacementForShow function)
         */
        private PlacementType _placement = PlacementType.BottomLeft;

        /*
         * 通过参数赋值的placement，不应该通过其它方式赋值
         * Placement set by Parameter, should not be change by other way
         */
        private PlacementType _paramPlacement = PlacementType.BottomLeft;

        [Parameter]
        public PlacementType Placement
        {
            get
            {
                return RTL ? _placement.GetRTLPlacement() : _placement;
            }
            set
            {
                _placement = value;
                _paramPlacement = value;
            }
        }

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
        [Parameter]
        public string PopupContainerSelector { get; set; } = "body";

        /// <summary>
        /// Trigger mode. Could be multiple by passing an array.
        /// </summary>
        [Parameter]
        public TriggerType[] Trigger { get; set; } = new TriggerType[] { TriggerType.Hover };

        /// <summary>
        /// Manually set reference to triggering element. 
        /// </summary>
        [Parameter]
        public ElementReference TriggerReference
        {
            get => Ref;
            set => Ref = value;
        }

        /// <summary>
        /// ChildElement with ElementReference set to avoid wrapping div. 
        /// </summary>
        [Parameter]
        public RenderFragment<ForwardRef> Unbound { get; set; }

        /// <summary>
        /// Toggles overlay viability.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = false;

        [Inject]
        protected DomEventService DomEventService { get; set; }

        private bool _mouseInTrigger = false;
        private bool _mouseInOverlay = false;
        private bool _mouseUpInOverlay = false;

        protected Overlay _overlay = null;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                DomEventService.AddEventListener("document", "mouseup", OnMouseUp, false);
                DomEventService.AddEventListener("window", "resize", OnWindowResize, false);
                DomEventService.AddEventListener("document", "scroll", OnWindowScroll, false);
            }

            base.OnAfterRender(firstRender);
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && Unbound != null)
            {
                Ref = RefBack.Current;
                DomEventService.AddEventListener(Ref, "click", OnUnboundClick, true);
                DomEventService.AddEventListener(Ref, "mouseover", OnUnboundMouseEnter, true);
                DomEventService.AddEventListener(Ref, "mouseout", OnUnboundMouseLeave, true);
                DomEventService.AddEventListener(Ref, "focusin", OnUnboundFocusIn, true);
                DomEventService.AddEventListener(Ref, "focusout", OnUnboundFocusOut, true);
                DomEventService.AddEventListener(Ref, "contextmenu", OnContextMenu, true, true);
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        protected void OnUnboundMouseEnter(JsonElement jsonElement) => OnTriggerMouseEnter();

        protected void OnUnboundMouseLeave(JsonElement jsonElement) => OnTriggerMouseLeave();

        protected void OnUnboundFocusIn(JsonElement jsonElement) => OnTriggerFocusIn();

        protected void OnUnboundFocusOut(JsonElement jsonElement) => OnTriggerFocusOut();

        protected async void OnUnboundClick(JsonElement jsonElement)
        {
            var eventArgs = JsonSerializer.Deserialize<MouseEventArgs>(jsonElement.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            await OnClickDiv(eventArgs);
        }

        protected async void OnContextMenu(JsonElement jsonElement)
        {
            var eventArgs = JsonSerializer.Deserialize<MouseEventArgs>(jsonElement.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await OnTriggerContextmenu(eventArgs);
        }

        protected override void Dispose(bool disposing)
        {
            DomEventService.RemoveEventListerner<JsonElement>("document", "mouseup", OnMouseUp);
            DomEventService.RemoveEventListerner<JsonElement>("window", "resize", OnWindowResize);
            DomEventService.RemoveEventListerner<JsonElement>("document", "scroll", OnWindowScroll);

            if (Unbound != null)
            {
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "click", OnUnboundClick);
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "mouseover", OnUnboundMouseEnter);
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "mouseout", OnUnboundMouseLeave);
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "focusin", OnUnboundFocusIn);
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "focusout", OnUnboundFocusOut);
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "contextmenu", OnContextMenu);
            }
            base.Dispose(disposing);
        }

        protected virtual async Task OnTriggerMouseEnter()
        {
            _mouseInTrigger = true;

            if (_overlay != null && IsContainTrigger(TriggerType.Hover))
            {
                _overlay.PreventHide(true);

                await Show();
            }

            OnMouseEnter?.Invoke();
        }

        protected virtual async Task OnTriggerMouseLeave()
        {
            _mouseInTrigger = false;

            if (_overlay != null && IsContainTrigger(TriggerType.Hover))
            {
                _overlay.PreventHide(_mouseInOverlay);

                await Hide();
            }

            OnMouseLeave?.Invoke();
        }

        protected virtual async Task OnTriggerFocusIn()
        {
            _mouseInTrigger = true;

            if (_overlay != null && IsContainTrigger(TriggerType.Focus))
            {
                _overlay.PreventHide(true);

                await Show();
            }
        }

        protected virtual async Task OnTriggerFocusOut()
        {
            _mouseInTrigger = false;

            if (_overlay != null && IsContainTrigger(TriggerType.Focus))
            {
                _overlay.PreventHide(_mouseInOverlay);

                await Hide();
            }
        }

        protected virtual void OnOverlayMouseEnter()
        {
            _mouseInOverlay = true;

            if (_overlay != null && IsContainTrigger(TriggerType.Hover))
            {
                _overlay.PreventHide(true);
            }
        }

        protected virtual async Task OnOverlayMouseLeave()
        {
            _mouseInOverlay = false;

            if (_overlay != null && IsContainTrigger(TriggerType.Hover))
            {
                _overlay.PreventHide(_mouseInTrigger);

                await Hide();
            }
        }

        protected virtual void OnOverlayMouseUp()
        {
            _mouseUpInOverlay = true;
        }

        /// <summary>
        /// Handle the trigger click.
        /// </summary>
        /// <param name="args">MouseEventArgs</param>
        /// <returns></returns>
        public virtual async Task OnClickDiv(MouseEventArgs args)
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
            if (IsContainTrigger(TriggerType.Click))
            {
                if (_overlay.IsPopup())
                {
                    await Hide();
                }
                else
                {
                    await Show();
                }
            }
            else if (IsContainTrigger(TriggerType.ContextMenu) && _overlay.IsPopup())
            {
                await Hide();
            }
        }

        protected virtual async Task OnTriggerContextmenu(MouseEventArgs args)
        {
            if (IsContainTrigger(TriggerType.ContextMenu))
            {
                int offsetX = 10;
                int offsetY = 10;
#if NET5_0
                // offsetX/offsetY were only supported in Net5
                offsetX = (int)args.OffsetX;
                offsetY = (int)args.OffsetY;
#endif

                await Hide();
                await Show(offsetX, offsetY);
            }
        }

        protected virtual void OnMouseUp(JsonElement element)
        {
            if (_mouseUpInOverlay)
            {
                _mouseUpInOverlay = false;
                return;
            }

            if (_mouseInTrigger == false)
            {
                if (OnMaskClick.HasDelegate)
                {
                    OnMaskClick.InvokeAsync(null);
                }

                Hide();
            }
        }

        protected async void OnWindowResize(JsonElement element)
        {
            RestorePlacement();

            if (IsOverlayShow())
            {
                await GetOverlayComponent().UpdatePosition();
            }
        }

        protected void OnWindowScroll(JsonElement element)
        {
            RestorePlacement();
        }

        protected virtual bool IsContainTrigger(TriggerType triggerType)
        {
            return Trigger.Contains(triggerType);
        }

        protected virtual async Task OverlayVisibleChange(bool visible)
        {
            await OnVisibleChange.InvokeAsync(visible);
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
            _placement = placement;
        }

        internal void RestorePlacement()
        {
            _placement = _paramPlacement;
        }

        internal virtual string GetPlacementClass()
        {
            if (!string.IsNullOrEmpty(PlacementCls))
            {
                return PlacementCls;
            }
            return $"{PrefixCls}-placement-{Placement.Name}";
        }

        internal virtual string GetOverlayEnterClass()
        {
            if (!string.IsNullOrEmpty(OverlayEnterCls))
            {
                return OverlayEnterCls;
            }
            return $"ant-slide-{Placement.SlideName}-enter ant-slide-{Placement.SlideName}-enter-active ant-slide-{Placement.SlideName}";
        }

        internal virtual string GetOverlayLeaveClass()
        {
            if (!string.IsNullOrEmpty(OverlayLeaveCls))
            {
                return OverlayLeaveCls;
            }
            return $"ant-slide-{Placement.SlideName}-leave ant-slide-{Placement.SlideName}-leave-active ant-slide-{Placement.SlideName}";
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

        /// <summary>
        /// Toggle overlay visibility.
        /// </summary>
        /// <param name="visible">boolean: visibility true/false</param>
        public void SetVisible(bool visible) => Visible = visible;
    }
}
