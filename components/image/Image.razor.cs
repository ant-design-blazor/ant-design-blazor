using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Image : AntDomComponentBase
    {
        [Parameter]
        public string Alt { get; set; }

        [Parameter]
        public string Fallback { get; set; }

        [Parameter]
        public string Height { get; set; }

        [Parameter]
        public string Width { get; set; }

        [Parameter]
        public RenderFragment Placeholder { get; set; }

        [Parameter]
        public bool Preview { get; set; } = true;

        [Parameter]
        public string Src
        {
            get => _src;
            set
            {
                if (_src != value)
                {
                    _loaded = false;
                    _isError = false;
                    _src = value;

                    if (string.IsNullOrWhiteSpace(PreviewSrc))
                    {
                        PreviewSrc = _src;
                    }
                }
            }
        }

        [Parameter]
        public string PreviewSrc { get; set; }

        [Parameter]
        public ImageLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Image;

        [CascadingParameter]
        private ImagePreviewGroup Group { get; set; }

        [Inject]
        private ImageService ImageService { get; set; }

        private bool _isError;

        private string _imgStyle;

        private bool _loaded;
        private string _src;
        private ImageRef _imageRef;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!string.IsNullOrWhiteSpace(Width))
            {
                Style = $"width:{(CssSizeLength)Width};" + Style;
            }

            if (!string.IsNullOrWhiteSpace(Height))
            {
                Style = $"height:{(CssSizeLength)Height};" + Style;
                _imgStyle = $"height:{(CssSizeLength)Height};" + _imgStyle;
            }

            if (string.IsNullOrWhiteSpace(PreviewSrc))
            {
                PreviewSrc = _src;
            }

            ClassMapper.Add("ant-image")
                .If("ant-image-error", () => _isError);

            Group?.AddImage(this);
        }

        private void HandleOnError()
        {
            Src = Fallback;

            _loaded = true;
            _isError = true;
        }

        private void HandleOnLoad()
        {
            _loaded = true;
        }

        private void HandleOnLoadStart()
        {
            _loaded = false;
        }

        private void OnPreview()
        {
            var images = Group?.Images ?? new List<Image>() { this };
            var index = images.IndexOf(this);

            _imageRef = ImageService.OpenImages(images);

            _imageRef.SwitchTo(index);
        }

        protected override void Dispose(bool disposing)
        {
            Group?.Remove(this);
            base.Dispose(disposing);
        }
    }
}
