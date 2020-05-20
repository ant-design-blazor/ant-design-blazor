using System;
using System.Threading.Tasks;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntBlazor.Internal
{
    public partial class Overlay : AntDomComponentBase
    {
        [CascadingParameter(Name = "Trigger")]
        public OverlayTrigger Trigger { get; set; }

        [CascadingParameter(Name = "ParentTrigger")]
        public OverlayTrigger ParentTrigger { get; set; }

        [Parameter]
        public string OverlayChildPrefixCls { get; set; } = "";

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback OnOverlayMouseEnter { get; set; }

        [Parameter]
        public EventCallback OnOverlayMouseLeave { get; set; }

        [CascadingParameter(Name = "ArrowPointAtCenter")]
        public bool ArrowPointAtCenter { get; set; }

        [Parameter]
        public int HideMillisecondsDelay { get; set; } = 100;
        [Parameter]
        public int WaitForHideAnimMilliseconds { get; set; } = 200;

        private bool _hasAddOverlayToBody = false;
        private bool _isPreventHide = false;
        private bool _isChildOverlayShow = false;
        private bool _mouseInOverlay = false;

        private bool _isOverlayFirstRender = true;
        private bool _isWaitForOverlayFirstRender = false;

        private bool _preVisible = false;
        private bool _isOverlayShow = false;
        private bool _isOverlayHiding = false;

        private int? _overlayLeft = null;
        private int? _overlayTop = null;

        private string _overlayStyle = "";
        private string _overlayCls = "";

        private const int OVERLAY_OFFSET = 4;

        private const int ARROW_WIDTH = 5;
        private const int HORIZONTAL_ARROW_SHIFT = 16;
        private const int VERTICAL_ARROW_SHIFT = 8;

        protected override async Task OnParametersSetAsync()
        {
            if (!_isOverlayShow && Trigger.Visible && !_preVisible)
            {
                await Show(_overlayLeft, _overlayTop);
            }
            else if (_isOverlayShow && !Trigger.Visible && _preVisible)
            {
                await Hide(true);
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
                _ = InvokeAsync(async () =>
                {
                    await Task.Delay(100);
                    await JsInvokeAsync(JSInteropConstants.delElementFrom, Ref, Trigger.PopupContainerSelector);
                });
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
            _isOverlayHiding = false;

            await UpdateParentOverlayState(true);

            await AddOverlayToBody();

            Element trigger = await JsInvokeAsync<Element>(JSInteropConstants.getFirstChildDomInfo, Trigger.Ref);
            Element overlayElement = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, Ref);
            Element containerElement = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, Trigger.PopupContainerSelector);

            int left = GetOverlayLeft(trigger, overlayElement, containerElement);
            int top = GetOverlayTop(trigger, overlayElement, containerElement);

            _overlayStyle = $"left: {left}px;top: {top}px;{GetTransformOrigin()}";

            _overlayCls = Trigger.GetOverlayEnterClass();

            await Trigger.OnVisibleChange.InvokeAsync(true);

            StateHasChanged();
        }

        public async Task Hide(bool force = false)
        {
            if (!_isOverlayShow)
            {
                return;
            }

            await Task.Delay(HideMillisecondsDelay);

            if (!force && !IsContainTrigger(TriggerType.Click) && (_isPreventHide || _mouseInOverlay || _isChildOverlayShow))
            {
                return;
            }

            _isOverlayFirstRender = true;
            _isWaitForOverlayFirstRender = false;
            _isOverlayHiding = true;

            _overlayCls = Trigger.GetOverlayLeaveClass();

            await Trigger.OnOverlayHiding.InvokeAsync(true);

            await UpdateParentOverlayState(false);

            StateHasChanged();

            // wait for leave animation
            await Task.Delay(WaitForHideAnimMilliseconds);
            _isOverlayShow = false;
            _isOverlayHiding = false;

            await Trigger.OnVisibleChange.InvokeAsync(false);

            StateHasChanged();
        }

        public void PreventHide(bool prevent)
        {
            _isPreventHide = prevent;
        }

        /// <summary>
        /// set if there any child overlay show or hide
        /// overlay would not hide if any child is showing
        /// </summary>
        /// <param name="isChildOverlayShow"></param>
        public void UpdateChildState(bool isChildOverlayShow)
        {
            _isChildOverlayShow = isChildOverlayShow;
        }

        public bool IsPopup()
        {
            return _isOverlayShow;
        }

        /// <summary>
        /// when overlay is complete hide, IsPopup return true
        /// when overlay is hiding(playing hide animation), IsPopup return false, IsHiding return true.
        /// </summary>
        /// <returns></returns>
        public bool IsHiding()
        {
            return _isOverlayHiding;
        }

        private async Task AddOverlayToBody()
        {
            if (!_hasAddOverlayToBody)
            {
                await JsInvokeAsync(JSInteropConstants.addElementTo, Ref, Trigger.PopupContainerSelector);

                _hasAddOverlayToBody = true;
            }
        }

        protected virtual int GetOverlayTop(Element trigger, Element overlay, Element containerElement)
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

            if (Trigger.Placement.IsIn(PlacementType.Left, PlacementType.Right))
            {
                top = triggerTop + triggerHeight / 2 - overlay.clientHeight / 2;
            }
            else if (Trigger.Placement.IsIn(PlacementType.LeftTop, PlacementType.RightTop))
            {
                top = ArrowPointAtCenter ?
                      triggerTop - (VERTICAL_ARROW_SHIFT + ARROW_WIDTH) :
                      triggerTop;
            }
            else if (Trigger.Placement.IsIn(PlacementType.LeftBottom, PlacementType.RightBottom))
            {
                top = ArrowPointAtCenter ?
                      triggerTop - overlay.clientHeight + triggerHeight + (VERTICAL_ARROW_SHIFT + ARROW_WIDTH) :
                      triggerTop - overlay.clientHeight + triggerHeight;
            }
            else if (Trigger.Placement.IsIn(PlacementType.BottomLeft, PlacementType.BottomCenter, PlacementType.BottomRight))
            {
                top = triggerTop + triggerHeight + OVERLAY_OFFSET;
            }
            else if (Trigger.Placement.IsIn(PlacementType.TopLeft, PlacementType.TopCenter, PlacementType.TopRight))
            {
                top = triggerTop - overlay.clientHeight - OVERLAY_OFFSET;
            }

            return top;
        }

        protected virtual int GetOverlayLeft(Element trigger, Element overlay, Element containerElement)
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

            if (Trigger.Placement.IsIn(PlacementType.Left, PlacementType.LeftTop, PlacementType.LeftBottom))
            {
                left = triggerLeft - overlay.clientWidth - OVERLAY_OFFSET;
            }
            else if (Trigger.Placement.IsIn(PlacementType.Right, PlacementType.RightTop, PlacementType.RightBottom))
            {
                left = triggerLeft + triggerWidth + OVERLAY_OFFSET;
            }
            else if (Trigger.Placement.IsIn(PlacementType.BottomLeft, PlacementType.TopLeft))
            {
                left = ArrowPointAtCenter ?
                       triggerLeft + triggerWidth / 2 - (HORIZONTAL_ARROW_SHIFT + ARROW_WIDTH) :
                       triggerLeft;
            }
            else if (Trigger.Placement.IsIn(PlacementType.BottomCenter, PlacementType.TopCenter))
            {
                left = ArrowPointAtCenter ?
                       triggerLeft + triggerWidth / 2 - overlay.clientWidth + (HORIZONTAL_ARROW_SHIFT + ARROW_WIDTH) :
                       triggerLeft + triggerWidth / 2 - overlay.clientWidth / 2;
            }
            else if (Trigger.Placement.IsIn(PlacementType.BottomRight, PlacementType.TopRight))
            {
                left = triggerLeft + triggerWidth - overlay.clientWidth;
            }

            return left;
        }

        private string GetTransformOrigin()
        {
            string x = "";
            string y = "";

            if (Trigger.Placement.Equals(PlacementType.LeftTop))
            {
                x = "100%";
                y = "33%";
            }
            else if (Trigger.Placement.Equals(PlacementType.Left))
            {
                x = "100%";
                y = "50%";
            }
            else if (Trigger.Placement.Equals(PlacementType.LeftBottom))
            {
                x = "100%";
                y = "66%";
            }
            else if (Trigger.Placement.Equals(PlacementType.RightTop))
            {
                x = "0%";
                y = "33%";
            }
            else if (Trigger.Placement.Equals(PlacementType.Right))
            {
                x = "0%";
                y = "50%";
            }
            else if (Trigger.Placement.Equals(PlacementType.RightBottom))
            {
                x = "0%";
                y = "66%";
            }
            else if (Trigger.Placement.Equals(PlacementType.TopLeft))
            {
                x = "33%";
                y = "100%";
            }
            else if (Trigger.Placement.Equals(PlacementType.TopCenter))
            {
                x = "50%";
                y = "100%";
            }
            else if (Trigger.Placement.Equals(PlacementType.TopRight))
            {
                x = "66%";
                y = "100%";
            }
            else if (Trigger.Placement.Equals(PlacementType.BottomLeft))
            {
                x = "33%";
                y = "100%";
            }
            else if (Trigger.Placement.Equals(PlacementType.TopCenter))
            {
                x = "50%";
                y = "100%";
            }
            else if (Trigger.Placement.Equals(PlacementType.TopRight))
            {
                x = "66%";
                y = "100%";
            }

            if (string.IsNullOrEmpty(x) && string.IsNullOrEmpty(y))
            {
                return "";
            }

            return $"transform-origin: {x} {y}";
        }

        private bool IsContainTrigger(TriggerType triggerType)
        {
            foreach (TriggerType trigger in Trigger.Trigger)
            {
                if (trigger == triggerType)
                {
                    return true;
                }
            }

            return false;
        }

        private async Task UpdateParentOverlayState(bool visible)
        {
            if (ParentTrigger == null)
            {
                return;
            }

            ParentTrigger.GetOverlayComponent().UpdateChildState(visible);

            if (!visible)
            {
                await ParentTrigger.Hide();
            }
        }

        protected string GetOverlayCls()
        {
            string overlayCls;

            if (!_isOverlayShow && !_isWaitForOverlayFirstRender)
            {
                overlayCls = Trigger.GetOverlayHiddenClass();
            }
            else
            {
                overlayCls = _overlayCls;
            }

            return overlayCls;
        }

        protected string GetDisplayStyle()
        {
            string display = _isOverlayShow ? "display: inline-flex;" : "visibility: hidden;";

            if (!_isOverlayShow && !_isWaitForOverlayFirstRender)
            {
                display = "";
            }

            return display;
        }
    }
}
