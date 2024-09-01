// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
