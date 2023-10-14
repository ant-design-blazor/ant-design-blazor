using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using AntDesign.Core.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace AntDesign
{
    public partial class ImagePreview
    {
        [Parameter]
        public ImageRef ImageRef { get; set; }

        [Inject]
        protected IJSRuntime Js { get; set; }

        private ElementReference _previewImg;
        private double _zoomOutTimes = 1;
        private int _rotateTimes;
        private bool _visible = true;
        private string _left = "50%";
        private string _top = "50%";


        private async Task HandleClose()
        {
            _visible = false;
            StateHasChanged();
            // Blocking DOM removal
            await Task.Delay(200);

            ImageRef.Close();
        }

        private void HandleZoomIn()
        {
            _zoomOutTimes++;
        }

        private void HandleZoomOut()
        {
            if (_zoomOutTimes > 1)
            {
                _zoomOutTimes--;
            }
        }

        private void HandleRotateRight()
        {
            _rotateTimes++;
        }

        private void HandleRotateLeft()
        {
            _rotateTimes--;
        }

        private async Task WeelHandZoom(WheelEventArgs wheelEventArgs)
        {
            _left = await Js.InvokeAsync<string>(JSInteropConstants.GetStyle, _previewImg, "left");
            _top = await Js.InvokeAsync<string>(JSInteropConstants.GetStyle, _previewImg, "top");

            if (wheelEventArgs.DeltaY < 0)
            {
                _zoomOutTimes += 0.1;
            }
            else if (_zoomOutTimes > 0.5)
            {
                _zoomOutTimes -= 0.1;
            }
        }

        private DialogOptions GetDialogOptions()
        {
            return new DialogOptions()
            {
                PrefixCls = "ant-image-preview",
                Closable = false,
                Footer = null,
                MaskClosable = true,
                OnCancel = async (e) =>
                {
                    await HandleClose();
                }
            };
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                await Js.InvokeVoidAsync(JSInteropConstants.ImgDragAndDrop, _previewImg);
            }
        }
    }
}
