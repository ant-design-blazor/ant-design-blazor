using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace AntDesign
{
    public partial class Dialog
    {
        private const string IdPrefix = "Ant-Design-";

        [Parameter]
        public DialogOptions Config { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Visible { get; set; }

        private string _maskAnimation = "";
        private string _modalAnimation = "";
        private string _maskHideClsName = "";

        private bool _hasShow;
        private bool _hasDestroy = true;
        private string _wrapStyle = "";
        private bool _disableBodyScroll;
        private bool _doDragable = false;


        /// <summary>
        /// dialog root container
        /// </summary>
        private ElementReference _element;

        private ElementReference _dialogHeader;

        private ElementReference _modal;

        private bool _isFirstRender = true;

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
        private bool _dialogMouseDown = false;

        private void OnDialogMouseDown()
        {
            _dialogMouseDown = true;
        }

        private async Task OnMaskClick(MouseEventArgs e)
        {
            if (Config.MaskClosable
                && !_dialogMouseDown)
            {
                await CloseAsync();
            }
            _dialogMouseDown = false;
        }

        #endregion

        #region keyboard control

        /// <summary>
        /// TAB keyboard control
        /// </summary>
        private readonly string _sentinelStart = IdPrefix + Guid.NewGuid().ToString();
        private readonly string _sentinelEnd = IdPrefix + Guid.NewGuid().ToString();

        public string SentinelStart => _sentinelStart;

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


        private async Task OnCloserClick(MouseEventArgs e)
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

        private async Task Show()
        {
            if (_hasShow)
            {
                return;
            }

            if (Visible)
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
                _maskAnimation = ModalAnimation.MaskEnter;
                _modalAnimation = ModalAnimation.ModalEnter;

                // wait for animation, "antZoomIn" animation take 0.3s
                // see: @animation-duration-slow: 0.3s;
                await Task.Delay(300);

                _hasShow = true;

                StateHasChanged();
            }

        }

        public async Task Hide()
        {
            if (!_hasShow)
            {
                return;
            }

            if (!Visible)
            {
                _maskAnimation = ModalAnimation.MaskLeave;
                _modalAnimation = ModalAnimation.ModalLeave;
                await Task.Delay(200);
                _wrapStyle = "display: none;";
                _maskHideClsName = "ant-modal-mask-hidden";
                _hasShow = false;
                StateHasChanged();
                if (Config.OnClosed != null)
                {
                    await Config.OnClosed.Invoke();
                }
            }

        }

        public bool IsShow()
        {
            return _hasShow;
        }

        #endregion

        private string GetMaskClsName()
        {
            string clsName = _maskHideClsName;
            clsName += _maskAnimation;
            return clsName;
        }

        private string GetModalClsName()
        {
            string clsName = Config.ClassName;

            return clsName + _modalAnimation;
        }

        #region override

        protected override async Task OnParametersSetAsync()
        {
            //Reduce one rendering when showing and not destroyed
            if (Visible)
            {
                if (!_hasDestroy)
                {
                    await Show();
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

        protected override async Task OnAfterRenderAsync(bool isFirst)
        {
            _isFirstRender = isFirst;

            if (Visible)
            {
                if (_hasDestroy)
                {
                    await AppendToContainer();
                    _hasDestroy = false;
                    Show();
                    StateHasChanged();
                }

                if (!_disableBodyScroll)
                {
                    _disableBodyScroll = true;
                    await JsInvokeAsync(JSInteropConstants.DisableBodyScroll);
                }

                if (Config.Draggable && !_doDragable)
                {
                    _doDragable = true;
                    await JsInvokeAsync(JSInteropConstants.EnableDraggable, _dialogHeader, _modal, Config.DragInViewport);
                }
            }
            else
            {
                if (_disableBodyScroll)
                {
                    _disableBodyScroll = false;
                    await Task.Delay(250);
                    await JsInvokeAsync(JSInteropConstants.EnableBodyScroll);
                }

                if (Config.Draggable && _doDragable)
                {
                    _doDragable = false;
                    await JsInvokeAsync(JSInteropConstants.DisableDraggable, _dialogHeader);
                }
            }
            await base.OnAfterRenderAsync(isFirst);
        }
        #endregion
    }
}
