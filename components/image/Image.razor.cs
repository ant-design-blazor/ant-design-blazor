// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
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
        /// Render an indeterminate image loading progress placeholder.
        /// </summary>
        [Parameter]
        public bool PlaceholderProgress { get; set; }

        /// <summary>
        /// Render a determinate loading progress placeholder. Values are rounded and clamped to 0-100.
        /// </summary>
        [Parameter]
        public double? ProgressPercent { get; set; }

        /// <summary>Customize the determinate progress placeholder, with access to the normalized percentage and default progress UI.</summary>
        [Parameter]
        public RenderFragment<ImageProgressRenderContext> ProgressRender { get; set; }

        /// <summary>
        /// Enable or disable preview functionality
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool Preview { get; set; } = true;

        /// <summary>Whether the image preview popup mask is visible.</summary>
        [Parameter]
        public bool PreviewMaskVisible { get; set; } = true;

        /// <summary>Whether to blur the background through the image preview popup mask.</summary>
        [Parameter]
        public bool PreviewMaskBlur { get; set; }

        /// <summary>Custom content rendered on the image thumbnail's preview cover.</summary>
        [Parameter]
        public RenderFragment PreviewCover { get; set; }

        /// <summary>Position of <see cref="PreviewCover"/> on the image thumbnail.</summary>
        [Parameter]
        public ImageCoverPlacement PreviewCoverPlacement { get; set; } = ImageCoverPlacement.Center;

        /// <summary>CSS classes keyed by Image semantic DOM parts.</summary>
        [Parameter]
        public IReadOnlyDictionary<string, string> ClassNames { get; set; }

        /// <summary>Inline CSS declarations keyed by Image semantic DOM parts.</summary>
        [Parameter]
        public IReadOnlyDictionary<string, string> Styles { get; set; }

        /// <summary>Custom content for the preview toolbar action group.</summary>
        [Parameter]
        public RenderFragment<ImagePreviewActionsRenderContext> ActionsRender { get; set; }

        /// <summary>Custom rendering for the preview image content.</summary>
        [Parameter]
        public RenderFragment<ImagePreviewImageRenderContext> ImageRender { get; set; }

        private string PreviewCoverPlacementClass => PreviewCoverPlacement switch
        {
            ImageCoverPlacement.Top => "ant-image-cover-top",
            ImageCoverPlacement.Bottom => "ant-image-cover-bottom",
            _ => "ant-image-cover-center"
        };

        internal string SemanticClass(string slot, string baseClass)
        {
            if (ClassNames == null || !ClassNames.TryGetValue(slot, out var customClass) || string.IsNullOrWhiteSpace(customClass))
                return baseClass;
            return string.IsNullOrWhiteSpace(baseClass) ? customClass : $"{baseClass} {customClass}";
        }

        internal string SemanticStyle(string slot, string baseStyle = null)
        {
            var semanticStyle = Styles != null && Styles.TryGetValue(slot, out var value) ? value : null;
            if (string.IsNullOrWhiteSpace(baseStyle)) return semanticStyle;
            if (string.IsNullOrWhiteSpace(semanticStyle)) return baseStyle;
            return $"{baseStyle.Trim().TrimEnd(';')}; {semanticStyle}";
        }

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

        private bool IsProgressPlaceholder => PlaceholderProgress || ProgressPercent.HasValue;

        private bool HasProgressPercent => ProgressPercent.HasValue && !double.IsNaN(ProgressPercent.Value) && !double.IsInfinity(ProgressPercent.Value);

        private int ProgressPercentValue => HasProgressPercent
            ? (int)Math.Max(0, Math.Min(100, Math.Floor(ProgressPercent.Value + 0.5)))
            : 0;

        private string ProgressPercentText => ProgressPercentValue.ToString(CultureInfo.InvariantCulture);

        private RenderFragment DefaultProgressContent => builder =>
        {
            if (!HasProgressPercent)
                return;

            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", SemanticClass("placeholder.progress.rail", "ant-image-progress-rail"));
            builder.AddAttribute(2, "style", SemanticStyle("placeholder.progress.rail", $"--progress-percent:{ProgressPercentText}%"));
            builder.CloseElement();
            builder.OpenElement(3, "div");
            builder.AddAttribute(4, "class", SemanticClass("placeholder.progress.indicator", "ant-image-progress-indicator"));
            builder.AddAttribute(5, "style", SemanticStyle("placeholder.progress.indicator"));
            builder.AddContent(6, $"{ProgressPercentText}%");
            builder.CloseElement();
        };

        private RenderFragment GetProgressContent()
        {
            var defaultProgress = DefaultProgressContent;
            if (ProgressRender == null)
                return defaultProgress;

            return ProgressRender(new ImageProgressRenderContext
            {
                Percent = ProgressPercentValue,
                HasPercent = HasProgressPercent,
                DefaultProgress = defaultProgress
            });
        }

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

            ClassMapper.Add("ant-image css-var-root ant-image-css-var")
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
