using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Internal
{
    public sealed partial class Overlay : AntDomComponentBase
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

        [Parameter]
        public Action OnShow { get; set; }

        [Parameter]
        public Action OnHide { get; set; }

        [CascadingParameter(Name = "ArrowPointAtCenter")]
        public bool ArrowPointAtCenter { get; set; }

        [Parameter]
        public int HideMillisecondsDelay { get; set; } = 100;

        [Parameter]
        public int WaitForHideAnimMilliseconds { get; set; } = 200;

        /// <summary>
        /// vertical offset between Trigger and Overlay, default is 4
        /// </summary>
        [Parameter]
        public int VerticalOffset { get; set; } = 4;

        /// <summary>
        /// horizontal offset between Trigger and Overlay, default is 4
        /// </summary>
        [Parameter]
        public int HorizontalOffset { get; set; } = 4;

        [Parameter]
        public bool HiddenMode { get; set; } = false;

        [Inject]
        private DomEventService DomEventService { get; set; }

        private bool _hasAddOverlayToBody = false;
        private bool _isPreventHide = false;
        private bool _isChildOverlayShow = false;
        private bool _mouseInOverlay = false;

        private bool _isOverlayFirstRender = true;
        private bool _isWaitForOverlayFirstRender = false;

        private bool _preVisible = false;
        private bool _isOverlayShow = false;
        private bool _isOverlayHiding = false;
        private bool _lastDisabledState = false;

        private int? _overlayLeft = null;
        private int? _overlayTop = null;

        private string _overlayStyle = "";
        private string _overlayCls = "";

        private const int ARROW_SIZE = 13;
        private const int HORIZONTAL_ARROW_SHIFT = 13;
        private const int VERTICAL_ARROW_SHIFT = 5;

        private int _overlayClientWidth = 0;

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
                DomEventService.AddEventListener("window", "beforeunload", Reloading, false);
            }

            if (_lastDisabledState != Trigger.Disabled)
            {
                if (Ref.Id != null)
                {
                    if (Trigger.Disabled)
                    {
                        await JsInvokeAsync(JSInteropConstants.AddClsToFirstChild, Ref, $"disabled");
                    }
                    else
                    {
                        await JsInvokeAsync(JSInteropConstants.RemoveClsFromFirstChild, Ref, $"disabled");
                    }
                }
                _lastDisabledState = Trigger.Disabled;
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
            if (_hasAddOverlayToBody && !_isReloading)
            {
                _ = InvokeAsync(async () =>
                {
                    await Task.Delay(100);
                    await JsInvokeAsync(JSInteropConstants.DelElementFrom, Ref, Trigger.PopupContainerSelector);
                });
            }
            DomEventService.RemoveEventListerner<JsonElement>("window", "beforeunload", Reloading);
            base.Dispose(disposing);
        }

        internal async Task Show(int? overlayLeft = null, int? overlayTop = null)
        {
            if (_isOverlayShow || Trigger.Disabled)
            {
                return;
            }
            await Task.Yield();

            HtmlElement trigger;
            if (Trigger.ChildContent != null)
            {
                trigger = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetFirstChildDomInfo, Trigger.Ref);
                // fix bug in submenu: Overlay show when OvelayTrigger is not rendered complete.
                // I try to render Overlay when OvelayTrigger’s OnAfterRender is called, but is still get negative absoluteLeft
                // This may be a bad solution, but for now I can only do it this way.
                while (trigger.AbsoluteLeft <= 0 && trigger.ClientWidth <= 0)
                {
                    await Task.Delay(50);
                    trigger = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetFirstChildDomInfo, Trigger.Ref);
                }
            }
            else //(Trigger.Unbound != null)
            {
                trigger = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, Trigger.Ref);
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

            HtmlElement overlayElement = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, Ref);
            HtmlElement containerElement = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, Trigger.PopupContainerSelector);

            int left = await GetOverlayLeftWithBoundaryAdjust(trigger, overlayElement, containerElement);
            int top = await GetOverlayTopWithBoundaryAdjust(trigger, overlayElement, containerElement);

            int zIndex = await JsInvokeAsync<int>(JSInteropConstants.GetMaxZIndex);

            _overlayStyle = $"z-index:{zIndex};left: {left}px;top: {top}px;{GetTransformOrigin()}";

            _overlayCls = Trigger.GetOverlayEnterClass();

            await Trigger.OnVisibleChange.InvokeAsync(true);

            StateHasChanged();

            OnShow?.Invoke();
        }

        internal async Task Hide(bool force = false)
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

            OnHide?.Invoke();
        }

        internal void PreventHide(bool prevent)
        {
            _isPreventHide = prevent;
        }

        /// <summary>
        /// set if there any child overlay show or hide
        /// overlay would not hide if any child is showing
        /// </summary>
        /// <param name="isChildOverlayShow"></param>
        internal void UpdateChildState(bool isChildOverlayShow)
        {
            _isChildOverlayShow = isChildOverlayShow;
        }

        internal bool IsPopup()
        {
            return _isOverlayShow;
        }

        /// <summary>
        /// when overlay is complete hide, IsPopup return true
        /// when overlay is hiding(playing hide animation), IsPopup return false, IsHiding return true.
        /// </summary>
        /// <returns></returns>
        internal bool IsHiding()
        {
            return _isOverlayHiding;
        }

        /// <summary>
        /// Indicates that a page is being refreshed
        /// </summary>
        private bool _isReloading;

        private void Reloading(JsonElement jsonElement) => _isReloading = true;

        private async Task AddOverlayToBody()
        {
            if (!_hasAddOverlayToBody)
            {
                await JsInvokeAsync(JSInteropConstants.AddElementTo, Ref, Trigger.PopupContainerSelector);

                _hasAddOverlayToBody = true;
            }
        }

        private async Task<int> GetOverlayTopWithBoundaryAdjust(HtmlElement trigger, HtmlElement overlay, HtmlElement containerElement)
        {
            int top = GetOverlayTop(trigger, overlay, containerElement);

            /*
             * 边界检测和方向调整
             * boundary detection and orientation adjustment
             */
            return await BoundaryAdjust(trigger, overlay, containerElement, top, "top");
        }

        private async Task<int> GetOverlayLeftWithBoundaryAdjust(HtmlElement trigger, HtmlElement overlay, HtmlElement containerElement)
        {
            int left = GetOverlayLeft(trigger, overlay, containerElement);

            /*
             * 边界检测和方向调整
             * boundary detection and orientation adjustment
             */
            return await BoundaryAdjust(trigger, overlay, containerElement, left, "left");
        }

        private int GetOverlayTop(HtmlElement trigger, HtmlElement overlay, HtmlElement containerElement)
        {
            int top = 0;

            int triggerTop = trigger.AbsoluteTop - containerElement.AbsoluteTop;
            int triggerHeight = trigger.ClientHeight != 0 ? trigger.ClientHeight : trigger.OffsetHeight;

            // contextMenu
            if (_overlayTop != null)
            {
                triggerTop += (int)_overlayTop;
                triggerHeight = 0;
            }

            if (Trigger.Placement.IsIn(PlacementType.Left, PlacementType.Right))
            {
                top = triggerTop + triggerHeight / 2 - overlay.ClientHeight / 2;
            }
            else if (Trigger.Placement.IsIn(PlacementType.LeftTop, PlacementType.RightTop))
            {
                top = triggerTop;

                if (ArrowPointAtCenter)
                {
                    top += -VERTICAL_ARROW_SHIFT - ARROW_SIZE / 2 + triggerHeight / 2;
                }
            }
            else if (Trigger.Placement.IsIn(PlacementType.LeftBottom, PlacementType.RightBottom))
            {
                top = triggerTop - overlay.ClientHeight + triggerHeight;

                if (ArrowPointAtCenter)
                {
                    top += VERTICAL_ARROW_SHIFT + ARROW_SIZE / 2 - triggerHeight / 2;
                }
            }
            else if (Trigger.Placement.IsIn(PlacementType.BottomLeft, PlacementType.BottomCenter, PlacementType.Bottom, PlacementType.BottomRight))
            {
                top = triggerTop + triggerHeight + VerticalOffset;
            }
            else if (Trigger.Placement.IsIn(PlacementType.TopLeft, PlacementType.TopCenter, PlacementType.Top, PlacementType.TopRight))
            {
                top = triggerTop - overlay.ClientHeight - VerticalOffset;
            }

            return top;
        }

        private int GetOverlayLeft(HtmlElement trigger, HtmlElement overlay, HtmlElement containerElement)
        {
            int left = 0;

            int triggerLeft = trigger.AbsoluteLeft - containerElement.AbsoluteLeft;
            int triggerWidth = trigger.ClientWidth != 0 ? trigger.ClientWidth : trigger.OffsetWidth;

            if (overlay.ClientWidth > 0)
            {
                _overlayClientWidth = overlay.ClientWidth;
            }

            // contextMenu
            if (_overlayLeft != null)
            {
                triggerLeft += (int)_overlayLeft;
                triggerWidth = 0;
            }

            if (Trigger.Placement.IsIn(PlacementType.Left, PlacementType.LeftTop, PlacementType.LeftBottom))
            {
                left = triggerLeft - _overlayClientWidth - HorizontalOffset;
            }
            else if (Trigger.Placement.IsIn(PlacementType.Right, PlacementType.RightTop, PlacementType.RightBottom))
            {
                left = triggerLeft + triggerWidth + HorizontalOffset;
            }
            else if (Trigger.Placement.IsIn(PlacementType.BottomLeft, PlacementType.TopLeft))
            {
                left = triggerLeft;

                if (ArrowPointAtCenter)
                {
                    left += -HORIZONTAL_ARROW_SHIFT - ARROW_SIZE / 2 + triggerWidth / 2;
                }
            }
            else if (Trigger.Placement.IsIn(PlacementType.BottomCenter, PlacementType.Bottom, PlacementType.TopCenter, PlacementType.Top))
            {
                left = triggerLeft + triggerWidth / 2 - _overlayClientWidth / 2;
            }
            else if (Trigger.Placement.IsIn(PlacementType.BottomRight, PlacementType.TopRight))
            {
                left = triggerLeft + triggerWidth - _overlayClientWidth;

                if (ArrowPointAtCenter)
                {
                    left += HORIZONTAL_ARROW_SHIFT + ARROW_SIZE / 2 - triggerWidth / 2;
                }
            }

            return left;
        }

        /*
         * 边界检测和方向调整
         * boundary detection and orientation adjustment
         */

        private async Task<int> BoundaryAdjust(
            HtmlElement trigger, HtmlElement overlay, HtmlElement containerElement,
            int curPos, string direction)
        {
            if (Trigger.BoundaryAdjustMode == TriggerBoundaryAdjustMode.None)
            {
                return curPos;
            }

            int overlaySize = direction == "top" ? overlay.ClientHeight : overlay.ClientWidth;
            int boundarySize = await GetWindowBoundarySize(direction, containerElement);

            if (Trigger.Trigger.Contains(TriggerType.ContextMenu))
            {
                if (overlaySize + curPos > boundarySize)
                {
                    curPos -= overlaySize;
                }
                else if (curPos < 0)
                {
                    curPos += overlaySize;
                }

                return curPos;
            }

            int overlayPosWithSize = GetOverlayPosWithSize(curPos, overlaySize);

            if (overlayPosWithSize > boundarySize || overlayPosWithSize < 0)
            {
                // 翻转位置/reverse placement
                Trigger.ChangePlacementForShow(Trigger.Placement.GetReverseType());

                int reversePos = direction switch
                {
                    "top" => GetOverlayTop(trigger, overlay, containerElement),
                    _ => GetOverlayLeft(trigger, overlay, containerElement)
                };

                curPos = reversePos;
            }

            /*
                TODO： 翻转位置后仍然超出了边界/still outof boundary range after reverse placement
             */
            //overlayPosWithSize = GetOverlayPosWithSize(curPos, overlaySize);
            //if (overlayPosWithSize > boundarySize)
            //{
            //    curPos = GetOverlayBoundaryPos(overlaySize, boundarySize);
            //}
            //else if (overlayPosWithSize < 0)
            //{
            //    curPos -= overlayPosWithSize;
            //}

            return curPos;
        }

        private int GetOverlayPosWithSize(int overlayPos, int overlaySize)
        {
            PlacementDirection placementDirection = Trigger.Placement.GetDirection();

            if (placementDirection.IsIn(PlacementDirection.Top, PlacementDirection.Left))
            {
                return overlayPos - overlaySize;
            }
            else
            {
                return overlayPos + overlaySize;
            }
        }

        private int GetOverlayBoundaryPos(int overlaySize, int boundarySize)
        {
            PlacementDirection placementDirection = Trigger.Placement.GetDirection();

            if (placementDirection.IsIn(PlacementDirection.Top, PlacementDirection.Left))
            {
                return 0;
            }
            else
            {
                return boundarySize - overlaySize;
            }
        }

        private async Task<int> GetWindowBoundarySize(string direction, HtmlElement containerElement)
        {
            if (Trigger.BoundaryAdjustMode == TriggerBoundaryAdjustMode.InScroll)
            {
                return direction switch
                {
                    "top" => containerElement.ScrollHeight,
                    _ => containerElement.ScrollWidth
                };
            }

            Window windowElement = await JsInvokeAsync<Window>(JSInteropConstants.GetWindow, Ref);
            JsonElement scrollInfo = await JsInvokeAsync<JsonElement>(JSInteropConstants.GetScroll);
            int windowScrollX = (int)scrollInfo.GetProperty("x").GetDouble();
            int windowScrollY = (int)scrollInfo.GetProperty("y").GetDouble();

            return direction switch
            {
                "top" => (int)windowElement.innerHeight + windowScrollY,
                _ => (int)windowElement.innerWidth + windowScrollX
            };
        }

        private string GetTransformOrigin()
        {
            return $"transform-origin: {Trigger.Placement.TranformOrigin}";
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

        private string GetOverlayCls()
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

        private string GetDisplayStyle()
        {
            string display = _isOverlayShow ? "display: inline-flex;" : "visibility: hidden;";

            if (!_isOverlayShow && !_isWaitForOverlayFirstRender)
            {
                display = "";
            }

            return display;
        }

        internal async Task UpdatePosition(int? overlayLeft = null, int? overlayTop = null)
        {
            HtmlElement trigger;
            if (Trigger.ChildContent != null)
            {
                trigger = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetFirstChildDomInfo, Trigger.Ref);
            }
            else //(Trigger.Unbound != null)
            {
                trigger = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, Trigger.Ref);
            }

            _overlayLeft = overlayLeft;
            _overlayTop = overlayTop;

            HtmlElement overlayElement = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, Ref);
            HtmlElement containerElement = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, Trigger.PopupContainerSelector);

            int left = await GetOverlayLeftWithBoundaryAdjust(trigger, overlayElement, containerElement);
            int top = await GetOverlayTopWithBoundaryAdjust(trigger, overlayElement, containerElement);

            int zIndex = await JsInvokeAsync<int>(JSInteropConstants.GetMaxZIndex);

            _overlayStyle = $"z-index:{zIndex};left: {left}px;top: {top}px;{GetTransformOrigin()}";

            StateHasChanged();
        }
    }
}
