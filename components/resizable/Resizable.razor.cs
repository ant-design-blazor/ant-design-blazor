using System.Reactive;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace AntDesign
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Resizable : AntDomComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public double Width { get; set; } = 400;

        [Parameter]
        public double Height { get; set; } = 200;

        private bool _resizing = false;

        private ResizeHandleEvent _currentEvent;

        private ResizableDomRect _domRect = new ResizableDomRect();

        private string GetStyle => AsStyle();

        private void SetClass()
        {
            var prefixCls = "ant-resizable";
            ClassMapper.Add($"{prefixCls}")
                .If($"{prefixCls}-resizing", () => _resizing)
                .If($"{prefixCls}-disabled", () => Disabled)
                 ;
        }

        private string AsStyle()
        {
            return $"position:relative;width:{this.Width}px;height:{Height}px";
        }

        private async void StartResize()
        {
            // 给window添加鼠标事件
            if (this._resizing)
            {
                await this.JsInvokeAsync(JSInteropConstants.StartResize, DotNetObjectReference.Create(this));
                this._domRect = await this.JsInvokeAsync<ResizableDomRect>(JSInteropConstants.GetBoundingClientRect, this.Ref);
            }
        }

        private void Resize(double clientX, double clientY)
        {
            MouseEventArgs handleEvent = this._currentEvent?.MouseEvent;
            switch (this._currentEvent?.Direction)
            {
                case "bottomRight":
                    this.Width = clientX - this._domRect.left;
                    this.Height = clientY - this._domRect.top;
                    break;
                case "bottomLeft":
                    this.Width = this._domRect.width + handleEvent.ClientX - clientX;
                    this.Height = clientY - this._domRect.top;
                    break;
                case "topRight":
                    this.Width = clientX - _domRect.left;
                    this.Height = _domRect.height + handleEvent.ClientY - clientY;
                    break;
                case "topLeft":
                    this.Width = _domRect.width + handleEvent.ClientX - clientX;
                    this.Height = _domRect.height + handleEvent.ClientY - clientY;
                    break;
                case "top":
                    this.Height = _domRect.height + handleEvent.ClientY - clientY;
                    break;
                case "right":
                    this.Width = (double)(clientX - this._domRect.left);
                    break;
                case "bottom":
                    this.Height = clientY - _domRect.top;
                    break;
                case "left":
                    this.Width = _domRect.width + handleEvent.ClientX - clientX;
                    break;
            }
            this.InvokeStateHasChanged();
        }

        private async void EndResize()
        {
            if (this._resizing)
            {
                this._resizing = false;
                await this.JsInvokeAsync(JSInteropConstants.EndResize);
            }
        }

        private async void SetPosition()
        {
            await JsInvokeAsync(JSInteropConstants.SetCss, this.Ref, "position", "relative");
        }

        private async void SetCursor()
        {
            switch (this._currentEvent?.Direction)
            {
                case "left":
                case "right":
                    await this.JsInvokeAsync(JSInteropConstants.SetBodyStyle, "cursor", "ew-resize");
                    break;
                case "top":
                case "bottom":
                    await this.JsInvokeAsync(JSInteropConstants.SetBodyStyle, "cursor", "ns-resize");
                    break;
                case "topLeft":
                case "bottomRight":
                    await this.JsInvokeAsync(JSInteropConstants.SetBodyStyle, "cursor", "nwse-resize");
                    break;
                case "topRight":
                case "bottomLeft":
                    await this.JsInvokeAsync(JSInteropConstants.SetBodyStyle, "cursor", "nesw-resize");
                    break;
            }
            await this.JsInvokeAsync(JSInteropConstants.SetBodyStyle, "user-select", "none");
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClass();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                ResizableService.MouseDown.Subscribe(Observer.Create<ResizeHandleEvent>(ret =>
                {
                    this._currentEvent = ret;
                    this.SetCursor();
                    this._resizing = true;
                    this.StartResize();
                }));
            }
        }

        private async Task OnMouseEnter()
        {
            await InvokeAsync(() =>
            {
                ResizableService.MouseEntered.OnNext(true);
            });
        }

        private async Task OnMouseLeave()
        {
            await InvokeAsync(() =>
            {
                ResizableService.MouseEntered.OnNext(false);
            });
        }

        [JSInvokable("ClientMouseUp")]
        public async void OnMouseUp()
        {
            await InvokeAsync(() =>
            {
                this.EndResize();
            });
        }

        [JSInvokable("ClientMouseMove")]
        public async void OnMouseMove(MouseEventArgs e)
        {
            await InvokeAsync(() =>
            {
                this.Resize(e.ClientX, e.ClientY);
            });
        }
    }
}
