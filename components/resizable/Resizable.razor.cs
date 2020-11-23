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

        private bool _resizing = false;

        private ResizeHandleEvent _currentEvent;

        [Inject]
        private DomEventService DomEventService { get; set; }

        private void SetClass()
        {
            var prefixCls = "ant-resizable";
            ClassMapper.Add($"{prefixCls}")
                .If($"{prefixCls}-resizing", () => _resizing)
                .If($"{prefixCls}-disabled", () => Disabled)
                 ;
        }

        private async void StartResize()
        {
            // 给window添加鼠标事件
            if (this._resizing)
            {
                await this.JsInvokeAsync(JSInteropConstants.StartResize, DotNetObjectReference.Create(this));
            }
        }

        private async void EndResize()
        {
            if (this._resizing)
            {
                this._resizing = false;
                // 还原widnow的鼠标样式
                await this.JsInvokeAsync(JSInteropConstants.SetBodyStyle, "user-select", "");
                await this.JsInvokeAsync(JSInteropConstants.SetBodyStyle, "cursor", "");
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
                this.SetPosition();
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
        private async void OnMouseUp()
        {
            await InvokeAsync(() =>
            {
                this.EndResize();
            });
        }

        [JSInvokable("ClientMouseMove")]
        private async void OnMouseMove(MouseEventArgs e)
        {
            await InvokeAsync(() =>
            {
                System.Console.WriteLine(e);
            });
        }
    }
}
