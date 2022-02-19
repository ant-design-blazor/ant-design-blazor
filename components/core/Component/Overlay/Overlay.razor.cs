﻿using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Core.JsInterop.Modules.Components;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Internal
{
    public sealed partial class Overlay : AntDomComponentBase
    {
        [CascadingParameter(Name = "ArrowPointAtCenter")]
        public bool ArrowPointAtCenter { get; set; }

        /// <summary>
        /// Used in nested overlays (for example menu -> submenu) when
        /// trigger is another overlay.
        /// </summary>
        [CascadingParameter(Name = "ParentTrigger")]
        public OverlayTrigger ParentTrigger { get; set; }

        /// <summary>
        /// Component that will trigger the overlay to show.
        /// </summary>
        [CascadingParameter(Name = "Trigger")]
        public OverlayTrigger Trigger { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback OnOverlayMouseEnter { get; set; }

        [Parameter]
        public EventCallback OnOverlayMouseLeave { get; set; }

        [Parameter]
        public EventCallback OnOverlayMouseUp { get; set; }

        [Parameter]
        public EventCallback OnShow { get; set; }

        [Parameter]
        public EventCallback OnHide { get; set; }

        [Parameter]
        public string OverlayChildPrefixCls { get; set; } = "";


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

        /// <summary>
        /// By default Overlay does not render its content if Overlay hasn't been
        /// activated (shown at least once). Setting HiddenMode = true will 
        /// go through rendering process.
        /// Use case: Select component, when using <see cref="SimpleSelectOption"/> or <see cref="SelectOption{TItemValue, TItem}"/>
        /// needs HiddenMode = true, so the select options are initialized and 
        /// potential defaults can be rendered properly.
        /// </summary>
        [Parameter]
        public bool HiddenMode { get; set; } = false;

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        private bool _hasAddOverlayToBody = false;
        private bool _isPreventHide = false;
        private bool _isChildOverlayShow = false;
        private bool _mouseInOverlay = false;

        private bool _isOverlayFirstRender = true;
        private bool _isWaitForOverlayFirstRender = false;

        private bool _preVisible = false;
        private bool _isOverlayDuringShowing = false;
        private bool _isOverlayShow = false;
        private bool _isOverlayHiding = false;
        private bool _lastDisabledState = false;

        private int? _overlayLeft = null;
        private int? _overlayTop = null;

        //if this style needs to be changed, also change 
        //the removal of that style in js interop overlay.ts class (in constructor)
        private string _overlayStyle = "display: none;"; //initial value prevents from screen flickering when adding overlay to dom; it will be overwritten immediately 
        private string _overlayCls = "";

        private bool _shouldRender = true;

        protected override bool ShouldRender()
        {
            if (_shouldRender)
                return base.ShouldRender();
            _shouldRender = true;
            return false;
        }

        protected override void OnInitialized()
        {
            _overlayCls = Trigger.GetOverlayHiddenClass();
            base.OnInitialized();
        }

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
                DomEventListener.AddShared<JsonElement>("window", "beforeunload", Reloading);
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
                //await Show(_overlayLeft, _overlayTop);

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
                    await JsInvokeAsync(JSInteropConstants.OverlayComponentHelper.DeleteOverlayFromContainer, Ref.Id);
                });
            }
            DomEventListener.Dispose();
            base.Dispose(disposing);
        }

        internal async Task Show(int? overlayLeft = null, int? overlayTop = null)
        {
            if (_isOverlayShow || Trigger.Disabled)
            {
                return;
            }
            _isOverlayDuringShowing = true;

            if (_isOverlayFirstRender)
            {
                Trigger.SetShouldRender(false);
                await Task.Yield();
            }

            _overlayLeft = overlayLeft;
            _overlayTop = overlayTop;

            if (_isOverlayFirstRender)
            {
                _isWaitForOverlayFirstRender = true;

                await InvokeAsync(StateHasChanged);
            }

            await UpdateParentOverlayState(true);

            await AddOverlayToBody(overlayLeft, overlayTop);
            _isOverlayShow = true;
            _isOverlayDuringShowing = false;
            _isOverlayHiding = false;

            _overlayCls = Trigger.GetOverlayEnterClass();
            await Trigger.OnVisibleChange.InvokeAsync(true);

            await InvokeAsync(StateHasChanged);

            if (OnShow.HasDelegate)
                OnShow.InvokeAsync(null);
        }

        internal async Task Hide(bool force = false)
        {
            if (_isOverlayDuringShowing)
            {
                //If Show() method is processing, wait up to 1000 ms
                //for it to end processing
                await WaitFor(() => _isOverlayShow);
            }
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

            if (OnHide.HasDelegate)
                OnHide.InvokeAsync(null);
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
        private OverlayPosition _position;

        private void Reloading(JsonElement jsonElement) => _isReloading = true;

        private int _recurenceGuard = 0;
        private async Task AddOverlayToBody(int? overlayLeft = null, int? overlayTop = null)
        {
            if (!_hasAddOverlayToBody)
            {
                bool triggerIsWrappedInDiv = Trigger.Unbound is null;
                _recurenceGuard++;

                //In ServerSide it may happen that trigger element reference has not yet been retrieved.                    
                if (!(await WaitFor(() => Trigger.Ref.Id is not null)))
                {
                    //Place where Error Boundary could be utilized
                    throw new ArgumentNullException("Trigger.Ref.Id cannot be null when attaching overlay to it.");
                }
                if (!(await WaitFor(() => Ref.Id is not null)))
                {
                    Debug.WriteLine("Overlay.Ref.Id is null. Adding overlay stopped.");
                    return;
                }

                _position = await JsInvokeAsync<OverlayPosition>(JSInteropConstants.OverlayComponentHelper.AddOverlayToContainer,
                    Ref.Id, Ref, Trigger.Ref, Trigger.Placement, Trigger.PopupContainerSelector,
                    Trigger.BoundaryAdjustMode, triggerIsWrappedInDiv, Trigger.PrefixCls,
                    VerticalOffset, HorizontalOffset, ArrowPointAtCenter, overlayTop, overlayLeft);
                if (_position is null && _recurenceGuard <= 10) //up to 10 attempts
                {
                    //Console.WriteLine($"Failed to add overlay to the container. Container: {Trigger.PopupContainerSelector}, trigger: {Trigger.Ref.Id}, overlay: {Ref.Id}. Awaiting and rerunning.");
                    await Task.Delay(10);
                    await AddOverlayToBody(overlayLeft, overlayTop);
                }
                else
                {
                    _hasAddOverlayToBody = true;
                    _overlayStyle = _position.PositionCss + GetTransformOrigin();
                    if (_position.Placement != Trigger.Placement)
                    {
                        Trigger.ChangePlacementForShow(PlacementType.Create(_position.Placement));
                    }
                }
            }
            else
            {
                await UpdatePosition(overlayLeft, overlayTop);
            }
            _recurenceGuard = 0;
        }

        /// <summary>
        /// Will probe a check predicate every given milliseconds until predicate is true or until
        /// runs out of number of probings.
        /// </summary>
        /// <param name="check">A predicate that will be run every time after waitTimeInMilisecondsPerProbing will pass.</param>
        /// <param name="probings">Maximum number of probings. After this number is reached, the method finishes.</param>
        /// <param name="waitTimeInMilisecondsPerProbing">How long to wait between each probing.</param>
        /// <returns>Task</returns>
        private async Task<bool> WaitFor(Func<bool> check, int probings = 100, int waitTimeInMilisecondsPerProbing = 10)
        {
            if (!check())
            {
                for (int i = 0; i < probings; i++)
                {
                    await Task.Delay(waitTimeInMilisecondsPerProbing);
                    if (check())
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

        private string GetTransformOrigin()
        {
            return $"transform-origin: {Trigger.GetPlacementType().TranformOrigin}";
        }

        private bool IsContainTrigger(TriggerType triggerType)
        {
            foreach (TriggerType trigger in Trigger.GetTriggerType())
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
                _overlayCls = Trigger.GetOverlayEnterClass();
            }
            else
            {
                overlayCls = _overlayCls;
            }

            return overlayCls;
        }

        private string GetDisplayStyle()
        {
            if (!_isOverlayShow && !_isWaitForOverlayFirstRender)
                return "";

            if (_isOverlayShow && _hasAddOverlayToBody)
                return "display: inline-flex;";

            if (_hasAddOverlayToBody)
                return "visibility: hidden;";

            return "display: inline-flex; visibility: hidden;";
        }

        internal async Task UpdatePosition(int? overlayLeft = null, int? overlayTop = null)
        {
            bool triggerIsWrappedInDiv = Trigger.Unbound is null;

            _position = await JsInvokeAsync<OverlayPosition>(JSInteropConstants.OverlayComponentHelper.UpdateOverlayPosition,
                Ref.Id, Ref, Trigger.Ref, Trigger.Placement, Trigger.PopupContainerSelector,
                Trigger.BoundaryAdjustMode, triggerIsWrappedInDiv, Trigger.PrefixCls,
                VerticalOffset, HorizontalOffset, ArrowPointAtCenter, overlayTop, overlayLeft);
            if (_position is not null)
            {
                if (_position.Placement != Trigger.Placement)
                {
                    Trigger.ChangePlacementForShow(PlacementType.Create(_position.Placement));
                }
                _overlayStyle = _position.PositionCss + GetTransformOrigin();
            }
        }

        private int ChangeOverlayLeftToRight(int left, HtmlElement overlay, HtmlElement container)
        {
            return container.ClientWidth - left - overlay.OffsetWidth;
        }
    }
}
