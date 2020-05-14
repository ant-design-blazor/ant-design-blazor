using System.Threading.Tasks;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntBlazor.Internal
{
    public partial class Overlay : AntDomComponentBase
    {
        [CascadingParameter(Name = "Trigger")]
        public OverlayTrigger Trigger { get; set; }

        [Parameter]
        public EventCallback OnOverlayMouseEnter { get; set; }

        [Parameter]
        public EventCallback OnOverlayMouseLeave { get; set; }

        private bool _hasAddOverlayToBody = false;
        private bool _isPreventHide = false;
        private bool _isChildDropdownShow = false;
        private bool _mouseInOverlay = false;

        private bool _isOverlayFirstRender = true;
        private bool _isWaitForOverlayFirstRender = false;

        private bool _preVisible = false;
        private bool _isOverlayShow = false;

        private int? _overlayLeft = null;
        private int? _overlayTop = null;

        private string _dropdownStyle = "";
        private string _overlayCls = "";

        private const int OVERLAY_TOP_OFFSET = 4;

        protected override async Task OnParametersSetAsync()
        {
            if (!_isOverlayShow && Trigger.Visible && !_preVisible)
            {
                await Show(_overlayLeft, _overlayTop);
            }
            else if (_isOverlayShow && !Trigger.Visible && _preVisible)
            {
                await Hide();
            }

            _preVisible = Trigger.Visible;
            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsInvokeAsync(JSInteropConstants.addClsToFirstChild, Ref, $"{Trigger.PrefixCls}-trigger");

                if (Trigger.Disabled)
                {
                    await JsInvokeAsync(JSInteropConstants.addClsToFirstChild, Ref, $"disabled");
                }
            }

            if (_isWaitForOverlayFirstRender && _isOverlayFirstRender)
            {
                _isOverlayFirstRender = false;

                await Show(_overlayLeft, _overlayTop);

                _isWaitForOverlayFirstRender = false;
                _overlayLeft = null;
                _overlayTop = null;
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected override void Dispose(bool disposing)
        {
            if (_hasAddOverlayToBody)
            {
                JsInvokeAsync(JSInteropConstants.delElementFrom, Ref, Trigger.PopupContainerSelector);
            }

            base.Dispose(disposing);
        }

        public async Task Show(int? overlayLeft = null, int? overlayTop = null)
        {
            if (_isOverlayShow || Trigger.Disabled)
            {
                return;
            }

            _overlayLeft = overlayLeft;
            _overlayTop = overlayTop;

            if (_isOverlayFirstRender)
            {
                _isWaitForOverlayFirstRender = true;

                StateHasChanged();
                return;
            }

            _isOverlayShow = true;

            await AddOverlayToBody();

            Element trigger = await JsInvokeAsync<Element>(JSInteropConstants.getFirstChildDomInfo, Trigger.Ref);
            Element overlayElement = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, Ref);
            Element containerElement = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, Trigger.PopupContainerSelector);

            int left = GetOverlayLeft(trigger, overlayElement, containerElement);
            int top = GetOverlayTop(trigger, overlayElement, containerElement);

            _dropdownStyle = $"left: {left}px;top: {top}px;";

            _overlayCls = $"slide-{Trigger.Placement.SlideName}-enter slide-{Trigger.Placement.SlideName}-enter-active slide-{Trigger.Placement.SlideName}";

            await Trigger.OnVisibleChange.InvokeAsync(true);

            StateHasChanged();
        }

        public async Task Hide(bool force = false)
        {
            if (!_isOverlayShow)
            {
                return;
            }

            await Task.Delay(100);

            if (!force && !IsContainTrigger(DropdownTrigger.Click) && (_isPreventHide || _mouseInOverlay || _isChildDropdownShow))
            {
                return;
            }

            _isOverlayFirstRender = true;
            _isWaitForOverlayFirstRender = false;

            _overlayCls = $"slide-{Trigger.Placement.SlideName}-leave slide-{Trigger.Placement.SlideName}-leave-active slide-{Trigger.Placement.SlideName}";

            await Trigger.OnVisibleChange.InvokeAsync(false);

            StateHasChanged();

            // wait for leave animation
            await Task.Delay(200);
            _isOverlayShow = false;

            StateHasChanged();
        }

        public void PreventHide(bool prevent)
        {
            _isPreventHide = prevent;
        }

        public void UpdateChildState(bool isChildDropdownShow)
        {
            _isChildDropdownShow = isChildDropdownShow;
        }

        public bool IsPopup()
        {
            return _isOverlayShow;
        }

        private async Task AddOverlayToBody()
        {
            if (!_hasAddOverlayToBody)
            {
                await JsInvokeAsync(JSInteropConstants.addElementTo, Ref, Trigger.PopupContainerSelector);

                _hasAddOverlayToBody = true;
            }
        }

        private int GetOverlayTop(Element trigger, Element overlay, Element containerElement)
        {
            int top = 0;

            int triggerTop = trigger.absoluteTop - containerElement.absoluteTop;
            int triggerHeight = trigger.clientHeight != 0 ? trigger.clientHeight : trigger.offsetHeight;

            // contextMenu
            if (_overlayTop != null)
            {
                triggerTop += (int)_overlayTop;
                triggerHeight = 0;
            }

            if (Trigger.Placement.Name.IsIn(DropdownPlacement.Left.Name, DropdownPlacement.Right.Name))
            {
                top = triggerTop;
            }
            else if (Trigger.Placement.Name == DropdownPlacement.Right.Name)
            {
                top = triggerTop + triggerHeight + OVERLAY_TOP_OFFSET;
            }
            else if (Trigger.Placement.SlideName == DropdownPlacement.BottomLeft.SlideName)
            {
                top = triggerTop + triggerHeight + OVERLAY_TOP_OFFSET;
            }
            else if (Trigger.Placement.SlideName == DropdownPlacement.TopLeft.SlideName)
            {
                top = triggerTop - overlay.clientHeight - OVERLAY_TOP_OFFSET;
            }

            return top;
        }

        private int GetOverlayLeft(Element trigger, Element overlay, Element containerElement)
        {
            int left = 0;
            int triggerLeft = trigger.absoluteLeft - containerElement.absoluteLeft;
            int triggerWidth = trigger.clientWidth;

            // contextMenu
            if (_overlayLeft != null)
            {
                triggerLeft += (int)_overlayLeft;
                triggerWidth = 0;
            }

            if (Trigger.Placement.Name == DropdownPlacement.Left.Name)
            {
                left = triggerLeft - triggerWidth;
            }
            else if (Trigger.Placement.Name == DropdownPlacement.Right.Name)
            {
                left = triggerLeft + triggerWidth;
            }
            else if (Trigger.Placement.Name.IsIn(DropdownPlacement.BottomLeft.Name, DropdownPlacement.TopLeft.Name))
            {
                left = triggerLeft;
            }
            else if (Trigger.Placement.Name.IsIn(DropdownPlacement.BottomCenter.Name, DropdownPlacement.TopCenter.Name))
            {
                left = triggerLeft + triggerWidth / 2 - overlay.clientWidth / 2;
            }
            else if (Trigger.Placement.Name.IsIn(DropdownPlacement.BottomRight.Name, DropdownPlacement.TopRight.Name))
            {
                left = triggerLeft + triggerWidth - overlay.clientWidth;
            }

            return left;
        }

        private bool IsContainTrigger(DropdownTrigger triggerType)
        {
            foreach (DropdownTrigger trigger in Trigger.Trigger)
            {
                if (trigger == triggerType)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
