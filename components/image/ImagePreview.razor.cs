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

        private void HandleClose()
        {
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
    }
}
