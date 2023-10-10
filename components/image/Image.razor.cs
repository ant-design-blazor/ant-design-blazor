using System.Collections.Generic;
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
        public bool PreviewVisible
        {
            get => _previewVisible;
            set
            {
                if (_previewVisible != value)
                {
                    _previewVisible = value;

                    if (_previewVisible)
                    {
                        ShowPreview();
                    }
                }
            }
        }

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

                    if (!_isPreviewSrcSet)
                    {
                        _previewSrc = _src;
                    }
                }
            }
        }

        [Parameter]
        public string PreviewSrc
        {
            get => _previewSrc;
            set
            {
                _previewSrc = value;
                _isPreviewSrcSet = true;
            }
        }

        [Parameter]
        public EventCallback<bool> PreviewVisibleChanged { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        [Parameter]
        public ImageLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Image;

        [CascadingParameter]
        private ImagePreviewGroup Group { get; set; }

        [Inject]
        private ImageService ImageService { get; set; }

        private bool _isError;
        private string _wrapperStyle;
        private bool _loaded;
        private string _src;
        private ImageRef _imageRef;
        private bool _isPreviewSrcSet;
        private string _previewSrc;

        private bool _previewVisible = true;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!string.IsNullOrWhiteSpace(Height))
            {
                _wrapperStyle += $"height:{(CssSizeLength)Height};";
                Style += $"height:{(CssSizeLength)Height};";
            }

            if (!string.IsNullOrWhiteSpace(Width))
            {
                _wrapperStyle += $"width:{(CssSizeLength)Width};";
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

        private void OnMaskClick(MouseEventArgs e)
        {
            if (_previewVisible)
            {
                ShowPreview();
            }

            if (OnClick.HasDelegate)
            {
                OnClick.InvokeAsync(e);
            }
        }

        public void ShowPreview()
        {
            var images = Group?.Images ?? new List<Image>() { this };
            var index = images.IndexOf(this);

            _imageRef = ImageService.OpenImages(images);
            _imageRef.OnClosed += OnPreviewClose;
            _imageRef.SwitchTo(index);

            if (PreviewVisibleChanged.HasDelegate)
            {
                PreviewVisibleChanged.InvokeAsync(true);
            }
        }

        public void OnPreviewClose()
        {
            if (PreviewVisibleChanged.HasDelegate)
            {
                PreviewVisibleChanged.InvokeAsync(false);
            }
        }

        protected override void Dispose(bool disposing)
        {
            Group?.Remove(this);
            base.Dispose(disposing);
        }
    }
}
