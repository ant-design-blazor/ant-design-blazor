using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class ImageService
    {
        public event Action<string> ImagePreviewOpened;

        public event Action<string> ImagePreviewClosed;

        public void OpenImage(string url)
        {
            ImagePreviewOpened?.Invoke(url);
        }

        public void CloseImage(string url)
        {
            ImagePreviewClosed?.Invoke(url);
        }
    }
}
