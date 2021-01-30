using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class ImageRef
    {
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
            _imageService.CloseImage(this);
        }
    }
}
