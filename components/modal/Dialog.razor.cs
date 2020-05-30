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
        [Parameter]
        public DialogOptions Config { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback OnClosed { get; set; }

        [Parameter]
        public bool Visible { get; set; }

        private bool _hasDestroy = true;

        private string GetStyle()
        {
            if (string.IsNullOrWhiteSpace(Style))
            {
                return Config.GetWidth();
            }
            else
            {
                return $"{Style};{Config.GetWidth()}";
            }
        }


        private ElementReference _element;

        /// <summary>
        /// destory
        /// </summary>
        /// <returns></returns>
        private async Task RemoveFromContainer()
        {
            await JsInvokeAsync(JSInteropConstants.destoryDialog, _element, Config.GetContainer);
        }

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
                await OnCloserClick(e);
            }
        }

        private async Task OnCloserClick(MouseEventArgs e)
        {
            await Close(e);
            await Config.OnCancel.InvokeAsync(e);
        }

        private readonly string _sentinelStart = Guid.NewGuid().ToString();
        private readonly string _sentinelEnd = Guid.NewGuid().ToString();

        private async Task OnKeyDown(KeyboardEventArgs e)
        {
            if (Config.Keyboard && e.Key == "Escape")
            {
                await Close(null);
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

        /// <summary>
        /// close modal, hide or Destroy
        /// </summary>
        /// <returns></returns>
        private async Task Close(MouseEventArgs e)
        {
            if (_hasDestroy)
            {
                return;
            }

            if (Config.DestroyOnClose || Config.ForceRender)
            {
                _hasDestroy = true;
                await JsInvokeAsync(JSInteropConstants.hideModal, _element);
                await Task.Delay(250);
                await RemoveFromContainer();
            }
            else
            {
                await JsInvokeAsync(JSInteropConstants.hideModal, _element);
            }

            if (Config.OnClosed.HasDelegate)
            {
                await Config.OnClosed.InvokeAsync(null);
            }
        }

        private async Task ShowModal()
        {
            await JsInvokeAsync(JSInteropConstants.showModal, _element);
        }

        private async Task AppendToContainer()
        {
            await JsInvokeAsync(JSInteropConstants.addElementTo, _element, Config.GetContainer);
        }

        #region override

        protected override async Task OnAfterRenderAsync(bool isFirst)
        {
            if (Visible)
            {
                if (_hasDestroy)
                {
                    await AppendToContainer();
                    _hasDestroy = false;
                }
                await ShowModal();
            }
            else
            {
                await Close(null);
            }

            await base.OnAfterRenderAsync(isFirst);
        }

        protected override void Dispose(bool disposing)
        {
            _ = RemoveFromContainer();
            base.Dispose(disposing);
        }

        #endregion
    }
}
