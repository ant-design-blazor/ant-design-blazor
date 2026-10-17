// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Bunit;
using AntDesign.JsInterop;
using Xunit;

namespace AntDesign.Tests.Image
{
    public class ImageRenderTests : AntDesignTestBase
    {
        [Fact]
        public void PreviewableImageMatchesReactV6RootSemantics()
        {
            var image = RenderComponent<AntDesign.Image>(parameters => parameters
                .Add(x => x.Src, "https://example.com/image.png"));

            var root = image.Find(".ant-image");
            Assert.Equal("button", root.GetAttribute("role"));
            Assert.Equal("0", root.GetAttribute("tabindex"));
            Assert.Contains("css-var-root", root.ClassList);
            Assert.Contains("ant-image-css-var", root.ClassList);
            Assert.NotNull(image.Find(".ant-image-cover.ant-image-cover-center"));
        }

        [Fact]
        public void NonPreviewableImageDoesNotExposeButtonSemanticsOrCover()
        {
            var image = RenderComponent<AntDesign.Image>(parameters => parameters
                .Add(x => x.Src, "https://example.com/image.png")
                .Add(x => x.Preview, false));

            var root = image.Find(".ant-image");
            Assert.Null(root.GetAttribute("role"));
            Assert.Null(root.GetAttribute("tabindex"));
            Assert.Empty(image.FindAll(".ant-image-cover"));
        }

        [Fact]
        public void OpeningPreviewRendersReactV6PopupSemanticStructure()
        {
            JSInterop.SetupVoid(JSInteropConstants.ImgDragAndDrop, _ => true);
            var previews = RenderComponent<ImagePreviewContainer>();
            var image = RenderComponent<AntDesign.Image>(parameters => parameters
                .Add(x => x.Src, "https://example.com/image.png"));

            image.Find(".ant-image-cover").Click();

            var popup = previews.Find(".ant-image-preview[role='dialog'][aria-modal='true']");
            Assert.NotNull(popup.QuerySelector(".ant-image-preview-mask"));
            Assert.NotNull(popup.QuerySelector(".ant-image-preview-body > .ant-image-preview-img"));
            Assert.NotNull(popup.QuerySelector(".ant-image-preview-close"));
            Assert.NotNull(popup.QuerySelector(".ant-image-preview-footer .ant-image-preview-actions"));
        }

        [Fact]
        public void PreviewToolbarKeepsZoomAndFlipControlsFunctional()
        {
            JSInterop.SetupVoid(JSInteropConstants.ImgDragAndDrop, _ => true);
            var previews = RenderComponent<ImagePreviewContainer>();
            var image = RenderComponent<AntDesign.Image>(parameters => parameters
                .Add(x => x.Src, "https://example.com/image.png"));

            image.Find(".ant-image-cover").Click();
            var popup = previews.Find(".ant-image-preview");
            var previewImage = popup.QuerySelector(".ant-image-preview-img");
            Assert.Contains("scale3d(1, 1, 1)", previewImage!.GetAttribute("style"));

            popup.QuerySelector("[aria-label='zoomIn']")!.Click();
            Assert.Contains("scale3d(2, 2, 1)", previews.Find(".ant-image-preview-img").GetAttribute("style"));

            popup = previews.Find(".ant-image-preview");
            popup.QuerySelector("[aria-label='flipX']")!.Click();
            Assert.Contains("scaleX(-1)", previews.Find(".ant-image-preview-img").GetAttribute("style"));
        }

        [Fact]
        public void IndeterminatePlaceholderProgressMatchesReactV6ProgressMarkup()
        {
            var image = RenderComponent<AntDesign.Image>(parameters => parameters
                .Add(x => x.PlaceholderProgress, true));

            var progress = image.Find(".ant-image.ant-image-progress-wrapper");
            Assert.Equal("true", progress.GetAttribute("aria-busy"));
            Assert.Equal("status", progress.QuerySelector("[role='status']")?.GetAttribute("role"));
            Assert.NotNull(progress.QuerySelector(".ant-image-progress-ink-1"));
            Assert.NotNull(progress.QuerySelector(".ant-image-progress-ink-2"));
            Assert.NotNull(progress.QuerySelector(".ant-image-progress-content"));
        }

        [Fact]
        public void DeterminatePlaceholderProgressExposesClampedAccessibleValue()
        {
            var image = RenderComponent<AntDesign.Image>(parameters => parameters
                .Add(x => x.ProgressPercent, 52.7));

            var progress = image.Find(".ant-image.ant-image-progress-wrapper");
            Assert.Equal("progressbar", progress.GetAttribute("role"));
            Assert.Equal("53", progress.GetAttribute("aria-valuenow"));
            Assert.Equal("53%", progress.GetAttribute("aria-label"));
            Assert.Equal("--progress-percent:53%", progress.QuerySelector(".ant-image-progress-rail")?.GetAttribute("style"));
            Assert.Equal("53%", progress.QuerySelector(".ant-image-progress-indicator")?.TextContent);
        }
    }
}
