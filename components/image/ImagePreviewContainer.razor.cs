using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ImagePreviewContainer
    {
        [Inject] private ImageService ImageService { get; set; }

        private readonly IList<string> _previewList = new List<string>();

        protected override void OnInitialized()
        {
            ImageService.ImagePreviewOpened += HandlePreviewOpen;
            ImageService.ImagePreviewClosed += HandlePreviewClose;

            base.OnInitialized();
        }

        private void HandlePreviewOpen(string url)
        {
            if (!_previewList.Contains(url))
            {
                _previewList.Add(url);
            }

            StateHasChanged();
        }

        private void HandlePreviewClose(string url)
        {
            if (_previewList.Contains(url))
            {
                _previewList.Remove(url);
            }

            StateHasChanged();
        }

        protected override void Dispose(bool disposing)
        {
            ImageService.ImagePreviewOpened -= HandlePreviewOpen;
            ImageService.ImagePreviewClosed -= HandlePreviewClose;

            base.Dispose(disposing);
        }
    }
}
