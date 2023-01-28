using System;
using System.Collections.Generic;
using System.Linq;

namespace AntDesign
{
    public class ImageService
    {
        public event Action<ImageRef> ImagePreviewOpened;

        public event Action<ImageRef> ImagePreviewClosed;

        public ImageRef OpenImages(IList<Image> images)
        {
            if (images?.Any() != true)
                return null;

            var imageRef = new ImageRef(images, this);

            ImagePreviewOpened?.Invoke(imageRef);

            return imageRef;
        }

        public void CloseImage(ImageRef iamgeRef)
        {
            ImagePreviewClosed?.Invoke(iamgeRef);
        }
    }
}
