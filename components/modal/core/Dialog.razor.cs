using System;
using System.Collections.Generic;
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

        /// <summary>
        /// default Dialog close icon
        /// </summary>
        internal static readonly RenderFragment DefaultCloseIcon = (builder) =>
        {
            builder.OpenComponent<Icon>(0);
            builder.AddAttribute(1, "Type", "close");
            builder.AddAttribute(2, "Theme", "outline");
            builder.CloseComponent();
        };

        /// <summary>
        /// default modal footer
        /// </summary>
        internal static readonly RenderFragment DefaultFooter = (builder) =>
        {
            builder.OpenComponent<ModalFooter>(0);
            builder.CloseComponent();
        };


        #region Parameters

#pragma warning disable 1591

        [Parameter]
        public DialogOptions Config { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Visible { get; set; }

#pragma warning restore 1591

        #endregion

        [CascadingParameter(Name = "DialogWrapperId")]
        public string DialogWrapperId { get; set; } = "";

        private string _maskAnimationClsName = "";
        private string _modalAnimationClsName = "";
        private string _maskHideClsName = "";
        private string _wrapStyle = "";

        private bool _hasShow;
        private bool _hasDestroy = true;
        private bool _disableBodyScroll;
        private bool _doDraggable;

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
                var style = $"{Config.GetWidth()};";

                if (Config.Draggable)
                {
                    string left = $"margin: 0; padding-bottom:0;";
                    style += left;
                }
                _modalStyle = style;
            }

            if (!string.IsNullOrWhiteSpace(Style))
            {
                return _modalStyle + Style + ";";
            }
            return _modalStyle;
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

        #endregion

        /// <summary>
        ///  append To body
        /// </summary>
        /// <returns></returns>
        private async Task AppendToContainer()
        {
            await JsInvokeAsync(JSInteropConstants.AddElementTo, _element, Config.GetContainer);
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

        private async Task OnMaskMouseUp()
        {
            if (Config.MaskClosable && _dialogMouseDown)
            {
                await Task.Delay(50);
                _dialogMouseDown = false;
            }
        }

        private async Task OnMaskClick()
        {
            if (Config.MaskClosable && !_dialogMouseDown)
            {
                await CloseAsync();
            }
        }

        #endregion

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

        #endregion

        /// <summary>
        /// closer(X) click event
        /// </summary>
        /// <returns></returns>
        private async Task OnCloserClick()
        {
            await CloseAsync();
        }

        private async Task CloseAsync()
        {
            if (_hasDestroy)
            {
                return;
            }
            if (Config.OnCancel != null)
            {
                await Config.OnCancel.Invoke(null);
            }
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
                if (Config.Draggable)
                {
                    _wrapStyle = "display:flex;justify-content: center;";
                    if (Config.Centered)
                    {
                        _wrapStyle += "align-items: center;";
                    }
                    else
                    {
                        _wrapStyle += "align-items: flex-start;";
                    }
                }
                else
                {
                    _wrapStyle = "";
                }
                _maskHideClsName = "";
                _maskAnimationClsName = ModalAnimation.MaskEnter;
                _modalAnimationClsName = ModalAnimation.ModalEnter;
                _hasShow = true;
            }
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

        #endregion

        #region build element's class name

        private string GetMaskClsName()
        {
            var clsName = _maskHideClsName;
            clsName += _maskAnimationClsName;
            return clsName;
        }

        private string GetModalClsName()
        {
            var clsName = Config.ClassName;
            return clsName + _modalAnimationClsName;
        }

        #endregion

        #region override

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task OnParametersSetAsync()
        {
            //Reduce one rendering when showing and not destroyed
            if (Visible)
            {
                if (!_hasDestroy)
                {
                    Show();
                    //await InvokeStateHasChangedAsync();
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

                if (!_disableBodyScroll)
                {
                    _disableBodyScroll = true;
                    await JsInvokeAsync(JSInteropConstants.DisableBodyScroll);
                }

                // enable drag and drop
                if (Config.Draggable && !_doDraggable)
                {
                    _doDraggable = true;
                    await JsInvokeAsync(JSInteropConstants.EnableDraggable, _dialogHeader, _modal, Config.DragInViewport);
                }
            }
            else
            {
                // enable body scroll
                if (_disableBodyScroll)
                {
                    _disableBodyScroll = false;
                    await Task.Delay(250);
                    await JsInvokeAsync(JSInteropConstants.EnableBodyScroll);
                }
                // disable drag and drop
                if (Config.Draggable && _doDraggable)
                {
                    _doDraggable = false;
                    await JsInvokeAsync(JSInteropConstants.DisableDraggable, _dialogHeader);
                }
            }

            await base.OnAfterRenderAsync(isFirst);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            // enable body scroll
            if (_disableBodyScroll)
            {
                _disableBodyScroll = false;
                _ = Task.Delay(250);
                _ = JsInvokeAsync(JSInteropConstants.EnableBodyScroll);
            }
        }
    }
}
