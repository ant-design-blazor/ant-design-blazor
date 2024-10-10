// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /**
    <summary>
    <para>Previewable image.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>When you need to display pictures.</item>
        <item>Display when loading a large image or fault tolerant handling when loading fail.</item>
    </list>
    </summary>
    <seealso cref="ImagePreviewGroup"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/antfincdn/D1dXz9PZqa/image.svg", Columns = 2, Title = "Image", SubTitle = "图片")]
    public partial class Image : AntDomComponentBase
    {
        /// <summary>
        /// Alternative text for image
        /// </summary>
        [Parameter]
        public string Alt { get; set; }

        /// <summary>
        /// Fallback if image fails to load
        /// </summary>
        [Parameter]
        public string Fallback { get; set; }

        /// <summary>
        /// Height of image
        /// </summary>
        [Parameter]
        public string Height { get; set; }

        /// <summary>
        /// Width of image
        /// </summary>
        [Parameter]
        public string Width { get; set; }

        /// <summary>
        /// Loading placeholder
        /// </summary>
        [Parameter]
        public RenderFragment Placeholder { get; set; }

        /// <summary>
        /// Enable or disable preview functionality
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool Preview { get; set; } = true;

        /// <summary>
        /// If the preview is visible or not
        /// </summary>
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

        /// <summary>
        /// Image source
        /// </summary>
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

        /// <summary>
        /// Preview image source
        /// </summary>
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

        /// <summary>
        /// Callback executed when <see cref="PreviewVisible"/> changes
        /// </summary>
        [Parameter]
        public EventCallback<bool> PreviewVisibleChanged { get; set; }

        /// <summary>
        /// Callback executed on image click
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        /// <summary>
        /// Locale data for component
        /// </summary>
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

            var prefixCls = "ant-image";
            var hashId = UseStyle(prefixCls, ImageStyle.UseComponentStyle);
            ClassMapper.Add(prefixCls)
                .Add(hashId)
                .If($"{prefixCls}-error", () => _isError)
                ;

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

        private void ShowPreview()
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

        private void OnPreviewClose()
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
