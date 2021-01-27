using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ImagePreview
    {
        [Parameter]
        public string ImageUrl { get; set; }

        [Inject]
        private ImageService ImageService { get; set; }

        private int _zoomOutTimes = 1;
        private int _rotateTimes;
        private bool _visible = true;

        private async Task HandleClose()
        {
            _visible = false;
            StateHasChanged();
            // Blocking DOM removal
            await Task.Delay(200);
            ImageService.CloseImage(ImageUrl);
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
    }
}
