// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ImagePreviewContainer
    {
        [Inject] private ImageService ImageService { get; set; }

        private readonly IList<ImageRef> _previewList = new List<ImageRef>();

        protected override void OnInitialized()
        {
            ImageService.ImagePreviewOpened += HandlePreviewOpen;
            ImageService.ImagePreviewClosed += HandlePreviewClose;

            base.OnInitialized();
        }

        private void HandlePreviewOpen(ImageRef imageRef)
        {
            if (!_previewList.Contains(imageRef))
            {
                _previewList.Add(imageRef);
            }

            StateHasChanged();
        }

        private void HandlePreviewClose(ImageRef imageRef)
        {
            if (_previewList.Contains(imageRef))
            {
                _previewList.Remove(imageRef);
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
