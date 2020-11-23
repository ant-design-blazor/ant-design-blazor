using System;
using System.Text;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ResizeHandle : AntDomComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string Direction { get; set; } = "bottomRight";

        private bool _entered = false;

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.SetClass();
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
                ResizableService.MouseEntered.Subscribe(ret =>
                {
                    this._entered = ret;
                    this.InvokeStateHasChanged();
                });
            }
        }

        private void SetClass()
        {
            var prefixCls = "ant-resizable-handle";

            ClassMapper
                .Clear()
                .Add($"{prefixCls}")
                .If($"{prefixCls}-left", () => Direction == ResizeHandleDirection.Left)
                .If($"{prefixCls}-top", () => Direction == ResizeHandleDirection.Top)
                .If($"{prefixCls}-right", () => Direction == ResizeHandleDirection.Right)
                .If($"{prefixCls}-bottom", () => Direction == ResizeHandleDirection.Bottom)
                .If($"{prefixCls}-topRight", () => Direction == ResizeHandleDirection.TopRight)
                .If($"{prefixCls}-bottomRight", () => Direction == ResizeHandleDirection.BottomRight)
                .If($"{prefixCls}-bottomLeft", () => Direction == ResizeHandleDirection.BottomLeft)
                .If($"{prefixCls}-topLeft", () => Direction == ResizeHandleDirection.TopLeft)
                .If($"{prefixCls}-box-hover", () => _entered)
                ;
        }

        private async Task OnMouseDown(MouseEventArgs args)
        {
            await InvokeAsync(() =>
            {
                ResizableService.MouseDown.OnNext(new ResizeHandleEvent() { Direction = this.Direction, MouseEvent = args });
            });
        }
    }
}
