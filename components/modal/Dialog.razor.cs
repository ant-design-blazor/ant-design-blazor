using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace AntDesign
{
    public partial class Dialog
    {
        private const string IdPrefix = "Ant-Design-";

        [Parameter] public DialogOptions Config { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public bool Visible { get; set; }

        private string _maskAnimation = "";
        private string _modalAnimation = "";
        private string _maskHideClsName = "";

        private bool _hasShow;
        private bool _hasDestroy = true;
        private string _wrapStyle = "";
        private bool _disableBodyScroll;

        /// <summary>
        /// dialog root container
        /// </summary>
        private ElementReference _element;

        /// <summary>
        /// ant-modal style
        /// </summary>
        /// <returns></returns>
        private string GetStyle()
        {
            var style = $"{Config.GetWidth()};";

            if (!string.IsNullOrWhiteSpace(Style))
            {
                style += Style + ";";
            }

            return style;
        }

        /// <summary>
        ///  append To body
        /// </summary>
        /// <returns></returns>
        private async Task AppendToContainer()
        {
            await JsInvokeAsync(JSInteropConstants.addElementTo, _element, Config.GetContainer);
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

        private async Task OnMaskMouseUp()
        {
            if (Config.MaskClosable && _dialogMouseDown)
            {
                await Task.Delay(50);
                _dialogMouseDown = false;
            }
        }

        private async Task OnMaskClick(MouseEventArgs e)
        {
            if (Config.MaskClosable
                && !_dialogMouseDown)
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
                    var activeElement = await JsInvokeAsync<string>(JSInteropConstants.getActiveElement, _sentinelEnd);
                    if (e.ShiftKey)
                    {
                        if (activeElement == _sentinelStart)
                        {
                            await JsInvokeAsync(JSInteropConstants.focus, "#" + _sentinelEnd);
                        }
                    }
                    else if (activeElement == _sentinelEnd)
                    {
                        await JsInvokeAsync(JSInteropConstants.focus, "#" + _sentinelStart);
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

        private void Show()
        {
            if (_hasShow)
            {
                return;
            }

            if (Visible)
            {
                _wrapStyle = "";
                _maskHideClsName = "";
                _maskAnimation = ModalAnimation.MaskEnter;
                _modalAnimation = ModalAnimation.ModalEnter;

                _hasShow = true;
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

        protected override async Task OnAfterRenderAsync(bool isFirst)
        {
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
                    await JsInvokeAsync(JSInteropConstants.disableBodyScroll);
                }
            }
            else
            {
                if (_disableBodyScroll)
                {
                    _disableBodyScroll = false;
                    await Task.Delay(250);
                    await JsInvokeAsync(JSInteropConstants.enableModalBodyScroll);
                }
            }

            await base.OnAfterRenderAsync(isFirst);
        }

        #endregion
    }
}
