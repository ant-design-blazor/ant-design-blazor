// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class ImageRef
    {
        public event Action OnClosed;

        internal string ImageSrc => _showingImageSrc;

        internal int CurrentIndex => _currentIndex;

        internal int ImageCount => _images.Count;

        internal bool PreviewMaskVisible => _images[_currentIndex].PreviewMaskVisible;

        internal bool PreviewMaskBlur => _images[_currentIndex].PreviewMaskBlur;

        internal RenderFragment<ImagePreviewActionsRenderContext> ActionsRender => _images[_currentIndex].ActionsRender;

        internal RenderFragment<ImagePreviewImageRenderContext> ImageRender => _images[_currentIndex].ImageRender;

        internal string ImageAlt => _images[_currentIndex].Alt;

        internal string ImageWidth => _images[_currentIndex].Width;

        internal string ImageHeight => _images[_currentIndex].Height;

        internal string SemanticClass(string slot, string baseClass) => _images[_currentIndex].SemanticClass(slot, baseClass);

        internal string SemanticStyle(string slot, string baseStyle = null) => _images[_currentIndex].SemanticStyle(slot, baseStyle);

        private IList<Image> _images;
        private string _showingImageSrc;
        private ImageService _imageService;
        private int _currentIndex;

        public ImageRef(IList<Image> images, ImageService imageService)
        {
            _images = images;
            _imageService = imageService;
        }

        public void SwitchTo(int index)
        {
            if (index < 0 || index >= _images.Count)
            {
                return;
            }

            _currentIndex = index;
            _showingImageSrc = _images[index].PreviewSrc;
        }

        public void Close()
        {
            OnClosed?.Invoke();
            _imageService.CloseImage(this);
        }
    }
}
