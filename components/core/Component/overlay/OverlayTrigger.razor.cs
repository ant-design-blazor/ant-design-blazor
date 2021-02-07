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

        [Parameter]
        public string PopupContainerSelector { get; set; } = "body";

        [Parameter]
        public string PlacementCls { get; set; }

        [Parameter]
        public string OverlayEnterCls { get; set; }

        [Parameter]
        public string OverlayLeaveCls { get; set; }

        [Parameter]
        public string OverlayHiddenCls { get; set; }

        [Parameter]
        public string OverlayClassName { get; set; }

        [Parameter]
        public string OverlayStyle { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool Visible { get; set; } = false;

        public void SetVisible(bool visible) => Visible = visible;

        /// <summary>
        /// 自动关闭功能和Visible参数同时生效
        /// Both auto-off and Visible control close
        /// </summary>
        [Parameter]
        public bool ComplexAutoCloseAndVisible { get; set; } = false;

        [Parameter]
        public bool IsButton { get; set; } = false;

        [Parameter]
        public bool InlineFlexMode { get; set; } = false;

        [Parameter]
        public bool HiddenMode { get; set; } = false;

        [Parameter]
        public TriggerType[] Trigger { get; set; } = new TriggerType[] { TriggerType.Hover };

        /*
         * 通过参数赋值的placement，不应该通过其它方式赋值
         * Placement set by Parameter, should not be change by other way
         */
        private PlacementType _paramPlacement = PlacementType.BottomLeft;

        /*
         * 当前的placement，某些情况下可能会被Overlay组件修改（通过ChangePlacementForShow函数）
         * Current placement, would change by overlay in some cases(via ChangePlacementForShow function)
         */
        private PlacementType _placement = PlacementType.BottomLeft;
        [Parameter]
        public PlacementType Placement
        {
            get
            {
                return _placement;
            }
            set
            {
                _placement = value;
                _paramPlacement = value;
            }
        }

        [Parameter] public Action OnMouseEnter { get; set; }

        [Parameter] public Action OnMouseLeave { get; set; }

        [Parameter]
        public EventCallback<bool> OnVisibleChange { get; set; }

        [Parameter]
        public EventCallback<bool> OnOverlayHiding { get; set; }

        [Parameter]
        public RenderFragment Overlay { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RenderFragment<ForwardRef> Unbound { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        [Inject]
        private DomEventService DomEventService { get; set; }

        private bool _mouseInTrigger = false;
        private bool _mouseInOverlay = false;

        protected Overlay _overlay = null;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                DomEventService.AddEventListener("document", "mouseup", OnMouseUp, false);
                DomEventService.AddEventListener("window", "resize", OnWindowResize, false);
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

        private void OnUnboundMouseEnter(JsonElement jsonElement) => OnTriggerMouseEnter();
        private void OnUnboundMouseLeave(JsonElement jsonElement) => OnTriggerMouseLeave();
        private void OnUnboundFocusIn(JsonElement jsonElement) => OnTriggerFocusIn();
        private void OnUnboundFocusOut(JsonElement jsonElement) => OnTriggerFocusOut();

        private async void OnUnboundClick(JsonElement jsonElement)
        {
            var eventArgs = JsonSerializer.Deserialize<MouseEventArgs>(jsonElement.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await OnClickDiv(eventArgs);
        }
        private async void OnContextMenu(JsonElement jsonElement)
        {
            var eventArgs = JsonSerializer.Deserialize<MouseEventArgs>(jsonElement.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await OnTriggerContextmenu(eventArgs);
        }

        protected override void Dispose(bool disposing)
        {
            DomEventService.RemoveEventListerner<JsonElement>("document", "mouseup", OnMouseUp);
            DomEventService.RemoveEventListerner<JsonElement>("window", "resize", OnMouseUp);
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
            if (_mouseInOverlay == false && _mouseInTrigger == false)
            {
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

        protected virtual void OnOverlayShow() { }
        protected virtual void OnOverlayHide() { }

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
            return $"slide-{Placement.SlideName}-enter slide-{Placement.SlideName}-enter-active slide-{Placement.SlideName}";
        }

        internal virtual string GetOverlayLeaveClass()
        {
            if (!string.IsNullOrEmpty(OverlayLeaveCls))
            {
                return OverlayLeaveCls;
            }
            return $"slide-{Placement.SlideName}-leave slide-{Placement.SlideName}-leave-active slide-{Placement.SlideName}";
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

        internal async Task<Element> GetTriggerDomInfo()
        {
            return await JsInvokeAsync<Element>(JSInteropConstants.GetFirstChildDomInfo, Ref);
        }

        public async Task Close()
        {
            await _overlay.Hide(true);
        }

        public bool IsOverlayShow()
        {
            return _overlay != null ? _overlay.IsPopup() : false;
        }
    }
}
