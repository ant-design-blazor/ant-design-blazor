using System.Text.Json;
using System.Threading.Tasks;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor.Internal
{
    public partial class OverlayTrigger : AntDomComponentBase
    {
        [CascadingParameter]
        public string PrefixCls { get; set; } = "ant-dropdown";

        [Parameter]
        public string PopupContainerSelector { get; set; }

        [Parameter]
        public string OverlayClassName { get; set; }

        [Parameter]
        public string OverlayStyle { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool Visible { get; set; } = false;

        [Parameter]
        public bool IsButton { get; set; } = false;

        [Parameter]
        public DropdownTrigger[] Trigger { get; set; } = new DropdownTrigger[] { DropdownTrigger.Hover };

        [Parameter]
        public DropdownPlacement Placement { get; set; } = DropdownPlacement.BottomLeft;

        [Parameter]
        public EventCallback<bool> OnVisibleChange { get; set; }

        [Parameter]
        public RenderFragment Overlay { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        [Inject]
        private DomEventService DomEventService { get; set; }

        private bool _mouseInTrigger = false;
        private bool _mouseInOverlay = false;

        protected Overlay _overlay = null;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            DomEventService.AddEventListener("app", "mouseup", OnMouseUp);
        }

        protected virtual async Task OnTriggerMouseEnter()
        {
            _mouseInTrigger = true;

            if (_overlay != null && IsContainTrigger(TriggerType.Hover))
            {
                _overlay.PreventHide(true);

                await _overlay.Show();
            }
        }

        protected virtual async Task OnTriggerMouseLeave()
        {
            _mouseInTrigger = false;

            if (_overlay != null && IsContainTrigger(TriggerType.Hover))
            {
                _overlay.PreventHide(_mouseInOverlay);

                await _overlay.Hide();
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

                await _overlay.Hide();
            }
        }

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
            if (IsContainTrigger(TriggerType.Click))
            {
                if (_overlay.IsPopup())
                {
                    await _overlay.Hide();
                }
                else
                {
                    await _overlay.Show();
                }
            }
        }

        protected virtual async Task OnTriggerContextmenu(MouseEventArgs args)
        {
            if (IsContainTrigger(TriggerType.ContextMenu))
            {
                // TODO：MouseEventArgs will support offsetX/offsetY in the future
                int offsetX = 10;
                int offsetY = 10;

                await _overlay.Hide();
                await _overlay.Show(offsetX, offsetY);
            }
        }

        protected virtual void OnMouseUp(JsonElement element)
        {
            if (_mouseInOverlay == false && _mouseInTrigger == false)
            {
                _overlay.Hide();
            }
        }

        protected virtual bool IsContainTrigger(TriggerType triggerType)
        {
            foreach (TriggerType trigger in Trigger)
            {
                if (trigger == triggerType)
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual string GetPlacementClass()
        {
            return $"{PrefixCls}-placement-{Placement.Name}";
        }

        protected virtual string GetOverlayEnterClass()
        {
            return $"slide-{Placement.SlideName}-enter slide-{Placement.SlideName}-enter-active slide-{Placement.SlideName}";
        }

        protected virtual string GetOverlayLeaveClass()
        {
            return $"slide-{Placement.SlideName}-leave slide-{Placement.SlideName}-leave-active slide-{Placement.SlideName}";
        }

        public async Task Show()
        {
            await _overlay.Show();
        }

        public async Task Hide()
        {
            await _overlay.Hide();
        }

        public Overlay GetOverlayComponent()
        {
            return _overlay;
        }

        public string PlacementCls { get { return GetPlacementClass(); } }
        public string OverlayEnterCls { get { return GetOverlayEnterClass(); } }
        public string OverlayLeaveCls { get { return GetOverlayLeaveClass(); } }

    }
}
