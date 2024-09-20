// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace AntDesign
{
    public class ImageRef
    {
        public event Action OnClosed;

        internal string ImageSrc => _showingImageSrc;

        internal int CurrentIndex => _currentIndex;

        internal int ImageCount => _images.Count;

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
