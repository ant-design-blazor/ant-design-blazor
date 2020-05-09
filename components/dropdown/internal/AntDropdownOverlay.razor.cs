using System;
using System.Threading.Tasks;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntBlazor.Internal
{
    public partial class AntDropdownOverlay : AntDomComponentBase
    {
        [CascadingParameter]
        public AntDropdown Dropdown { get; set; }

        [Parameter]
        public EventCallback OnOverlayMouseEnter { get; set; }

        [Parameter]
        public EventCallback OnOverlayMouseLeave { get; set; }

        private bool _hasAddOverlayToBody = false;
        private bool _isPreventHide = false;
        private bool _mouseInOverlay = false;

        private bool _isOverlayFirstRender = true;
        private bool _isWaitForOverlayFirstRender = false;

        private int? _overlayLeft = null;
        private int? _overlayTop = null;

        private string _dropdownStyle = "";
        private string _overlayCls = "";

        private const int OVERLAY_TOP_OFFSET = 4;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsInvokeAsync(JSInteropConstants.addClsToFirstChild, Ref, $"{Dropdown.PrefixCls}-trigger");

                if (Dropdown.Disabled)
                {
                    await JsInvokeAsync(JSInteropConstants.addClsToFirstChild, Ref, $"disabled");
                }

                if (Dropdown.Visible)
                {
                    await Show(_overlayLeft, _overlayTop);

                    _overlayLeft = null;
                    _overlayTop = null;
                }
            }
            else
            {
                if (_isWaitForOverlayFirstRender && _isOverlayFirstRender)
                {
                    _isOverlayFirstRender = false;

                    await Show(_overlayLeft, _overlayTop);

                    _isWaitForOverlayFirstRender = false;
                    _overlayLeft = null;
                    _overlayTop = null;
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_hasAddOverlayToBody)
            {
                if (!string.IsNullOrEmpty(Dropdown.PopupContainerSelector))
                {
                    JsInvokeAsync(JSInteropConstants.delElementFrom, Ref, Dropdown.PopupContainerSelector);
                }
                else
                {
                    JsInvokeAsync(JSInteropConstants.delElementFromBody, Ref);
                }
            }
        }

        public async Task Show(int? overlayLeft = null, int? overlayTop = null)
        {
            if (Dropdown.Visible || Dropdown.Disabled)
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

            Dropdown.Visible = true;

            await AddOverlayToBody();

            Element trigger = await JsInvokeAsync<Element>(JSInteropConstants.getFirstChildDomInfo, Dropdown.Ref);
            Element overlayElement = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, Ref);
            Element containerElement = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, Dropdown.PopupContainerSelector);

            int left = GetOverlayLeft(trigger, overlayElement, containerElement);
            int top = GetOverlayTop(trigger, overlayElement, containerElement);

            _dropdownStyle = $"left: {left}px;top: {top}px;";

            _overlayCls = $"slide-{Dropdown.Placement.SlideName}-enter slide-{Dropdown.Placement.SlideName}-enter-active slide-{Dropdown.Placement.SlideName}";

            await Dropdown.OnVisibleChange.InvokeAsync(true);

            StateHasChanged();
        }

        public async Task Hide(bool force = false)
        {
            if (!Dropdown.Visible)
            {
                return;
            }

            await Task.Delay(100);

            if (!force && !IsContainTrigger(AntDropdownTrigger.Click) && (_isPreventHide || _mouseInOverlay))
            {
                return;
            }

            _isOverlayFirstRender = true;
            _isWaitForOverlayFirstRender = false;

            _overlayCls = $"slide-{Dropdown.Placement.SlideName}-leave slide-{Dropdown.Placement.SlideName}-leave-active slide-{Dropdown.Placement.SlideName}";

            await Dropdown.OnVisibleChange.InvokeAsync(false);

            StateHasChanged();

            // wait for leave animation
            await Task.Delay(200);
            Dropdown.Visible = false;

            StateHasChanged();
        }

        public void PreventHide(bool prevent)
        {
            _isPreventHide = prevent;
        }

        public bool IsPopup()
        {
            return Dropdown.Visible;
        }

        private async Task AddOverlayToBody()
        {
            if (!_hasAddOverlayToBody)
            {
                await JsInvokeAsync(JSInteropConstants.addElementTo, Ref, Dropdown.PopupContainerSelector);

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

            if (Dropdown.Placement.SlideName == AntDropdownPlacement.BottomLeft.SlideName)
            {
                top = triggerTop + triggerHeight + OVERLAY_TOP_OFFSET;
            }

            if (Dropdown.Placement.SlideName == AntDropdownPlacement.TopLeft.SlideName)
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

            if (Dropdown.Placement.Name.IsIn(AntDropdownPlacement.BottomLeft.Name, AntDropdownPlacement.TopLeft.Name))
            {
                left = triggerLeft;
            }
            if (Dropdown.Placement.Name.IsIn(AntDropdownPlacement.BottomCenter.Name, AntDropdownPlacement.TopCenter.Name))
            {
                left = triggerLeft + triggerWidth / 2 - overlay.clientWidth / 2;
            }
            if (Dropdown.Placement.Name.IsIn(AntDropdownPlacement.BottomRight.Name, AntDropdownPlacement.TopRight.Name))
            {
                left = triggerLeft + triggerWidth - overlay.clientWidth;
            }

            return left;
        }

        private bool IsContainTrigger(AntDropdownTrigger triggerType)
        {
            foreach (AntDropdownTrigger trigger in Dropdown.Trigger)
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
