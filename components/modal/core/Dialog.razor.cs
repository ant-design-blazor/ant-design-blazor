// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /// <summary>
    /// Dialog only control component show or hide,
    /// Elements are not removed from the DOM tree
    /// </summary>
    public partial class Dialog
    {
        private const string IdPrefix = "Ant-Design-";

        #region Parameters

#pragma warning disable 1591

        [Parameter]
        public DialogOptions Config { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RenderFragment AdditionalContent { get; set; }

        [Parameter]
        public bool Visible { get; set; }

        [Parameter]
        public EventCallback<bool> VisibleChanged { get; set; }

#pragma warning restore 1591

        #endregion Parameters

        [CascadingParameter(Name = "DialogWrapperId")]
        public string DialogWrapperId { get; set; } = "";

        [Inject] private NavigationManager NavigationManager { get; set; }

        private string _maskAnimationClsName = "";
        private string _modalAnimationClsName = "";
        private string _maskHideClsName = "ant-modal-mask-hidden";
        private string _wrapStyle = "display: none;";

        private bool _hasShow;
        private bool _hasDestroy = true;
        private bool _disableBodyScroll;
        private bool _doDraggable;
        private string _prevUrl = "";

        private bool _resizing;

#pragma warning disable 649

        /// <summary>
        /// dialog root container
        /// </summary>
        private ElementReference _element;

        private ElementReference _dialogHeader;

        private ElementReference _modal;
#pragma warning restore 649

        #region ant-modal style

        private string _modalStyle = null;

        /// <summary>
        /// ant-modal style
        /// </summary>
        /// <returns></returns>
        private string GetStyle()
        {
            if (_modalStyle == null)
            {
                _modalStyle = CalcModalStyle();
            }
            return _modalStyle;
        }

        private string CalcModalStyle()
        {
            string style;
            if (Status == ModalStatus.Default)
            {
                style = $"{Config.GetWidth()};";
                if (Config.Draggable)
                {
                    style += "margin: 0; padding-bottom:0;";
                }
            }
            else
            {
                style = "margin: 0; padding: 0 ; top: 0;";
            }

            if (string.IsNullOrWhiteSpace(Style))
            {
                return style;
            }
            return style + Style + ";";
        }

        private string GetBodyStyle()
        {
            var style = Config.BodyStyle;

            if (!string.IsNullOrWhiteSpace(Config.MaxBodyHeight))
            {
                var maxBodyHeight = (CssSizeLength)(Config.MaxBodyHeight);
                style += $";max-height:{maxBodyHeight.ToString()};overflow-y:scroll;";
            }

            return style;
        }

        /// <summary>
        /// if Modal is draggable, reset the position style similar with the first show
        /// </summary>
        internal async Task TryResetModalStyle()
        {
            if (Config.Draggable)
            {
                await JsInvokeAsync(JSInteropConstants.ResetModalPosition, _dialogHeader);
            }
        }

        #endregion ant-modal style

        /// <summary>
        ///  append To body
        /// </summary>
        /// <returns></returns>
        private async Task AppendToContainer()
        {
            await JsInvokeAsync(JSInteropConstants.AddElementTo, _element, "body");
        }

        #region mask and dialog click event

        /// <summary>
        /// check is dialog click
        /// </summary>
        private bool _dialogMouseDown;

        private void OnDialogMouseDown()
        {
            _dialogMouseDown = true;
        }

        private void OnMaskMouseDown()
        {
            _dialogMouseDown = false;
        }

        private async Task OnMaskMouseUp()
        {
            if (Config.MaskClosable && !_dialogMouseDown)
            {
                await CloseAsync();
            }
            _dialogMouseDown = false;
        }

        #endregion mask and dialog click event

        #region keyboard control

        /// <summary>
        /// TAB keyboard control
        /// </summary>
        private readonly string _sentinelStart = IdPrefix + Guid.NewGuid().ToString();

        private readonly string _sentinelEnd = IdPrefix + Guid.NewGuid().ToString();

        /// <summary>
        /// Tab start control id
        /// </summary>
        public string SentinelStart => _sentinelStart;

        /// <summary>
        /// Listening for Tab and ESC key events
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task OnKeyDown(KeyboardEventArgs e)
        {
            if (Config.Keyboard && e.Key == "Escape")
            {
                await CloseAsync();
                return;
            }
            if (Visible)
            {
                if (e.Key == "Tab")
                {
                    var activeElement = await JsInvokeAsync<string>(JSInteropConstants.GetActiveElement, _sentinelEnd);
                    if (e.ShiftKey)
                    {
                        if (activeElement == _sentinelStart)
                        {
                            await JsInvokeAsync(JSInteropConstants.Focus, "#" + _sentinelEnd);
                        }
                    }
                    else if (activeElement == _sentinelEnd)
                    {
                        await JsInvokeAsync(JSInteropConstants.Focus, "#" + _sentinelStart);
                    }
                }
            }
        }

        #endregion keyboard control

        /// <summary>
        /// closer(X) click event
        /// </summary>
        /// <returns></returns>
        internal async Task OnCloserClick()
        {
            await CloseAsync();
        }

        private async Task CloseAsync()
        {
            if (_hasDestroy)
            {
                return;
            }
            if (VisibleChanged.HasDelegate)
            {
                await VisibleChanged.InvokeAsync(false);
            }
            if (Config.OnCancel != null)
            {
                await Config.OnCancel.Invoke(null);
            }
        }

        public ModalStatus Status { get; private set; } = ModalStatus.Default;

        /// <summary>
        /// closer(X) click event
        /// </summary>
        /// <returns></returns>
        internal Task OnMaxBtnClick()
        {
            if (Status == ModalStatus.Default)
            {
                _resizing = false;
                SetModalStatus(ModalStatus.Max);
            }
            else
            {
                _resizing = Config.Resizable;
                SetModalStatus(ModalStatus.Default);
            }
            return Task.CompletedTask;
        }

        private void SetModalStatus(ModalStatus modalStatus)
        {
            Status = modalStatus;
            _wrapStyle = CalcWrapStyle();
            _modalStyle = CalcModalStyle();
        }

        #region control show and hide class name and style

        /// <summary>
        /// show dialog through animation
        /// </summary>
        /// <returns></returns>
        private void Show()
        {
            if (!_hasShow && Visible)
            {
                _hasShow = true;
                _resizing = Config.Resizable;
                _wrapStyle = CalcWrapStyle();
                _maskHideClsName = "";
                _maskAnimationClsName = ModalAnimation.MaskEnter;
                _modalAnimationClsName = ModalAnimation.ModalEnter;
            }
        }

        private string CalcWrapStyle()
        {
            string style = "";
            if (Status == ModalStatus.Default && Config.Draggable)
            {
                style = "display:flex;justify-content: center;";
                if (Config.Centered)
                {
                    style += "align-items: center;";
                }
                else
                {
                    style += "align-items: flex-start;";
                }
            }

            if (!Config.Mask)
            {
                style += "pointer-events:none;";
            }

            return style;
        }

        /// <summary>
        /// clear ant-model enter class, which will disable user-select.
        /// more details see style/mixins/modal-mask.less
        /// </summary>
        /// <returns></returns>
        internal async Task CleanShowAnimationAsync()
        {
            var animationTimeMs = 300;
            await Task.Delay(animationTimeMs);
            _maskAnimationClsName = "";
            _modalAnimationClsName = "";
            await InvokeStateHasChangedAsync();
        }

        /// <summary>
        /// Hide Dialog through animation
        /// </summary>
        /// <returns></returns>
        internal async Task Hide()
        {
            if (!_hasShow)
            {
                return;
            }

            if (!Visible)
            {
                _hasShow = false;

                if (VisibleChanged.HasDelegate)
                {
                    await VisibleChanged.InvokeAsync(false);
                }

                _maskAnimationClsName = ModalAnimation.MaskLeave;
                _modalAnimationClsName = ModalAnimation.ModalLeave;
                await Task.Delay(200);
                _wrapStyle = "display: none;";
                _maskHideClsName = "ant-modal-mask-hidden";

                await InvokeStateHasChangedAsync();
                if (Config.OnClosed != null)
                {
                    await Config.OnClosed.Invoke();
                }
            }
        }

        /// <summary>
        /// Determine whether Dialog is displayed
        /// </summary>
        /// <returns></returns>
        public bool IsShow()
        {
            return _hasShow;
        }

        #endregion control show and hide class name and style

        #region build element's class name

        private string GetMaskClsName()
        {
            var clsName = _maskHideClsName;
            clsName += _maskAnimationClsName;
            return clsName;
        }

        private string GetModalClsName()
        {
            var clsList = new List<string>()
            {
                Config.ClassName,
                _modalAnimationClsName,
                Status == ModalStatus.Max ? "ant-modal-max" : "",
                Config.Resizable ? "ant-modal-resizable":"",
                Class
            };

            return string.Join(" ", clsList.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        #endregion build element's class name

        #region override

        protected override void OnInitialized()
        {
            _prevUrl = NavigationManager.Uri;

            base.OnInitialized();
        }

        private bool _hasRendered = false;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override async Task OnParametersSetAsync()
        {
            //Reduce one rendering when showing and not destroyed
            if (Visible)
            {
                if (!_hasRendered && Config.DefaultMaximized)
                {
                    _hasRendered = true;
                    SetModalStatus(ModalStatus.Max);
                }
                if (!_hasDestroy)
                {
                    Show();
                }
                else
                {
                    _wrapStyle = "display: none;";
                    _maskHideClsName = "ant-modal-mask-hidden";
                }
            }
            else
            {
                await Hide();
            }
            await base.OnParametersSetAsync();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="isFirst"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool isFirst)
        {
            if (Visible)
            {
                // disable body scroll
                if (_hasDestroy)
                {
                    _hasDestroy = false;
                    await AppendToContainer();
                    Show();
                    await InvokeStateHasChangedAsync();
                }

                // enable drag and drop
                if (Status != ModalStatus.Max && Config.Draggable && !_doDraggable)
                {
                    _doDraggable = true;
                    await JsInvokeAsync(JSInteropConstants.EnableDraggable, _dialogHeader, _modal, Config.DragInViewport);
                }
            }
            else if (!_hasRendered && Config.ForceRender)
            {
                _hasRendered = true;
                await AppendToContainer();
            }
            else
            {
                // disable drag and drop
                if (Status != ModalStatus.Max && Config.Draggable && _doDraggable)
                {
                    _doDraggable = false;
                    await JsInvokeAsync(JSInteropConstants.DisableDraggable, _dialogHeader);
                }
            }

            await base.OnAfterRenderAsync(isFirst);
        }

        #endregion override

        protected override void Dispose(bool disposing)
        {

            _ = TryResetModalStyle();
            // When a modal is defined in template, it will be destroyed when the page is navigated to different page component.
            // but sometime the opened modal dom would not be removed from body, and sometime it would...
            // so we remove it manually, and set a delay to avoid the dom being removed before the page is navigated.
            if (!Config.CreateByService && !Config.DestroyOnClose && _prevUrl != NavigationManager.Uri)
            {
                _ = JsInvokeAsync(JSInteropConstants.DelElementFrom, "#" + Id, "body", 1000);
            }
        }
    }
}
