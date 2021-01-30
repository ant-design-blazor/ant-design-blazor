using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ImagePreviewGroup
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject]
        private ImageService ImageService { get; set; }

        internal IList<Image> Images => _images;

        private IList<Image> _images;

        public void AddImage(Image image)
        {
            _images ??= new List<Image>();
            _images.Add(image);
        }

        public void Remove(Image image)
        {
            _images.Remove(image);
        }
    }
}
