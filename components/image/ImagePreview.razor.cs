// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
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
        private bool _flipX;
        private bool _flipY;

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

        private void HandleFlipX()
        {
            _flipX = !_flipX;
        }

        private void HandleFlipY()
        {
            _flipY = !_flipY;
        }

        private void SwitchPrevious()
        {
            ImageRef.SwitchTo(ImageRef.CurrentIndex - 1);
        }

        private void SwitchNext()
        {
            ImageRef.SwitchTo(ImageRef.CurrentIndex + 1);
        }

        private async Task HandleKeyDown(KeyboardEventArgs args)
        {
            if (args.Key == "Escape")
            {
                await HandleClose();
            }
        }

        private async Task WeelHandZoom(WheelEventArgs wheelEventArgs)
        {
            if (wheelEventArgs.DeltaY < 0)
            {
                _zoomOutTimes += 0.1;
            }
            else if (_zoomOutTimes > 0.5)
            {
                _zoomOutTimes -= 0.1;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Js.InvokeVoidAsync(JSInteropConstants.ImgDragAndDrop, _previewImg);
            }
        }
    }
}
