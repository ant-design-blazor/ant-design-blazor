// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Bunit;
using AntDesign.JsInterop;
using System;
using System.Collections.Generic;
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
        public void CustomThumbnailCoverSupportsV6Placement()
        {
            var cover = new Microsoft.AspNetCore.Components.RenderFragment(builder => builder.AddContent(0, "Custom cover"));
            var image = RenderComponent<AntDesign.Image>(parameters => parameters
                .Add(x => x.Src, "https://example.com/image.png")
                .Add(x => x.PreviewCover, cover)
                .Add(x => x.PreviewCoverPlacement, ImageCoverPlacement.Top));

            image.Find("img").TriggerEvent("onload", new EventArgs());
            Assert.Contains("ant-image-cover-top", image.Find(".ant-image-cover").ClassList);
            Assert.Contains("Custom cover", image.Markup);
        }

        [Fact]
        public void PreviewMaskSupportsHiddenAndBlurredStates()
        {
            JSInterop.SetupVoid(JSInteropConstants.ImgDragAndDrop, _ => true);
            var previews = RenderComponent<ImagePreviewContainer>();
            var image = RenderComponent<AntDesign.Image>(parameters => parameters
                .Add(x => x.Src, "https://example.com/image.png")
                .Add(x => x.PreviewMaskVisible, false)
                .Add(x => x.PreviewMaskBlur, true));

            image.Find(".ant-image-cover").Click();

            var mask = previews.Find(".ant-image-preview-mask");
            Assert.Contains("ant-image-preview-mask-hidden", mask.ClassList);
            Assert.Contains("ant-image-preview-mask-blur", mask.ClassList);
        }

        [Fact]
        public void ProgressRenderReceivesNormalizedPercentAndDefaultProgressNodes()
        {
            var image = RenderComponent<AntDesign.Image>(parameters => parameters
                .Add(x => x.ProgressPercent, 62.7)
                .Add(x => x.ProgressRender, context => builder =>
                {
                    builder.OpenElement(0, "section");
                    builder.AddAttribute(1, "class", "custom-image-progress");
                    builder.AddContent(2, $"Custom {context.Percent}%");
                    builder.AddContent(3, context.DefaultProgress);
                    builder.CloseElement();
                }));

            var customProgress = image.Find(".custom-image-progress");
            Assert.Contains("Custom 63%", customProgress.TextContent);
            Assert.Equal("63%", customProgress.QuerySelector(".ant-image-progress-indicator")?.TextContent);
            Assert.Equal("--progress-percent:63%", customProgress.QuerySelector(".ant-image-progress-rail")?.GetAttribute("style"));
        }

        [Fact]
        public void ProgressRenderRunsForIndeterminateProgressWithoutDefaultBar()
        {
            var renderCalled = false;
            var progressPercent = -1;
            var hasPercent = true;
            var image = RenderComponent<AntDesign.Image>(parameters => parameters
                .Add(x => x.PlaceholderProgress, true)
                .Add(x => x.ProgressRender, context =>
                {
                    renderCalled = true;
                    progressPercent = context.Percent;
                    hasPercent = context.HasPercent;
                    return builder => builder.AddContent(0, "Waiting for image");
                }));

            Assert.True(renderCalled);
            Assert.Equal(0, progressPercent);
            Assert.False(hasPercent);
            Assert.Equal("true", image.Find(".ant-image-progress-wrapper").GetAttribute("aria-busy"));
            Assert.Contains("Waiting for image", image.Markup);
            Assert.Empty(image.FindAll(".ant-image-progress-rail"));
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
        public void SemanticClassNamesAndStylesApplyToThumbnailAndPopupParts()
        {
            JSInterop.SetupVoid(JSInteropConstants.ImgDragAndDrop, _ => true);
            var previews = RenderComponent<ImagePreviewContainer>();
            var image = RenderComponent<AntDesign.Image>(parameters => parameters
                .Add(x => x.Src, "https://example.com/image.png")
                .Add(x => x.ClassNames, new Dictionary<string, string>
                {
                    ["root"] = "custom-image-root",
                    ["image"] = "custom-image-element",
                    ["cover"] = "custom-image-cover",
                    ["popup.root"] = "custom-preview-root",
                    ["popup.mask"] = "custom-preview-mask",
                    ["popup.body"] = "custom-preview-body",
                    ["popup.footer"] = "custom-preview-footer",
                    ["popup.actions"] = "custom-preview-actions",
                    ["popup.close"] = "custom-preview-close"
                })
                .Add(x => x.Styles, new Dictionary<string, string>
                {
                    ["root"] = "border: 1px solid red",
                    ["image"] = "object-fit: cover",
                    ["cover"] = "color: red",
                    ["popup.mask"] = "background: rgba(0,0,0,.5)",
                    ["popup.actions"] = "gap: 4px"
                }));

            Assert.Contains("custom-image-root", image.Find(".ant-image").ClassList);
            Assert.Contains("border: 1px solid red", image.Find(".ant-image").GetAttribute("style"));
            Assert.Contains("custom-image-element", image.Find("img.ant-image-img").ClassList);
            Assert.Contains("object-fit: cover", image.Find("img.ant-image-img").GetAttribute("style"));
            Assert.Contains("custom-image-cover", image.Find(".ant-image-cover").ClassList);

            image.Find(".ant-image-cover").Click();
            var popup = previews.Find(".custom-preview-root");
            Assert.Contains("custom-preview-mask", popup.QuerySelector(".ant-image-preview-mask")!.ClassList);
            Assert.Contains("background: rgba(0,0,0,.5)", popup.QuerySelector(".ant-image-preview-mask")!.GetAttribute("style"));
            Assert.Contains("custom-preview-body", popup.QuerySelector(".ant-image-preview-body")!.ClassList);
            Assert.Contains("custom-preview-footer", popup.QuerySelector(".ant-image-preview-footer")!.ClassList);
            Assert.Contains("custom-preview-actions", popup.QuerySelector(".ant-image-preview-actions")!.ClassList);
            Assert.Contains("gap: 4px", popup.QuerySelector(".ant-image-preview-actions")!.GetAttribute("style"));
            Assert.Contains("custom-preview-close", popup.QuerySelector(".ant-image-preview-close")!.ClassList);
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
        public void ActionsRenderCanExtendDefaultToolbarAndInvokePreviewActions()
        {
            JSInterop.SetupVoid(JSInteropConstants.ImgDragAndDrop, _ => true);
            var previews = RenderComponent<ImagePreviewContainer>();
            var actionsRender = new Microsoft.AspNetCore.Components.RenderFragment<ImagePreviewActionsRenderContext>(context => builder =>
            {
                builder.OpenElement(0, "button");
                builder.AddAttribute(1, "aria-label", "custom-zoom");
                builder.AddAttribute(2, "onclick", context.OnZoomIn);
                builder.AddContent(3, $"Custom zoom {context.Scale}; image {context.Current}/{context.Total}");
                builder.CloseElement();
                builder.AddContent(4, context.OriginalNode);
            });
            var image = RenderComponent<AntDesign.Image>(parameters => parameters
                .Add(x => x.Src, "https://example.com/image.png")
                .Add(x => x.ActionsRender, actionsRender));

            image.Find(".ant-image-cover").Click();
            var actions = previews.Find(".ant-image-preview-actions");
            Assert.Contains("Custom zoom 1; image 1/1", actions.TextContent);
            Assert.NotNull(actions.QuerySelector("[aria-label='zoomIn']"));

            actions.QuerySelector("[aria-label='custom-zoom']")!.Click();
            Assert.Contains("scale3d(2, 2, 1)", previews.Find(".ant-image-preview-img").GetAttribute("style"));
        }

        [Fact]
        public void ImageRenderCanWrapDefaultImageAndReceivesCurrentTransformInfo()
        {
            JSInterop.SetupVoid(JSInteropConstants.ImgDragAndDrop, _ => true);
            var previews = RenderComponent<ImagePreviewContainer>();
            var imageRender = new Microsoft.AspNetCore.Components.RenderFragment<ImagePreviewImageRenderContext>(context => builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "custom-preview-image-render");
                builder.AddContent(2, context.OriginalNode);
                builder.OpenElement(3, "span");
                builder.AddAttribute(4, "class", "custom-preview-transform");
                builder.AddContent(5, $"{context.Alt}; scale {context.Scale}");
                builder.CloseElement();
                builder.CloseElement();
            });
            var image = RenderComponent<AntDesign.Image>(parameters => parameters
                .Add(x => x.Src, "https://example.com/image.png")
                .Add(x => x.Alt, "Preview alt")
                .Add(x => x.ImageRender, imageRender));

            image.Find(".ant-image-cover").Click();
            Assert.Contains("Preview alt; scale 1", previews.Find(".custom-preview-transform").TextContent);
            Assert.NotNull(previews.Find(".custom-preview-image-render img.ant-image-preview-img"));

            previews.Find("[aria-label='zoomIn']").Click();
            Assert.Contains("Preview alt; scale 2", previews.Find(".custom-preview-transform").TextContent);
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
                .Add(x => x.ProgressPercent, 52.7)
                .Add(x => x.ClassNames, new Dictionary<string, string>
                {
                    ["placeholder.progress.root"] = "custom-progress-root",
                    ["placeholder.progress.content"] = "custom-progress-content",
                    ["placeholder.progress.rail"] = "custom-progress-rail",
                    ["placeholder.progress.indicator"] = "custom-progress-indicator"
                })
                .Add(x => x.Styles, new Dictionary<string, string>
                {
                    ["placeholder.progress.root"] = "max-width: 320px",
                    ["placeholder.progress.content"] = "color: rebeccapurple",
                    ["placeholder.progress.rail"] = "border-radius: 4px",
                    ["placeholder.progress.indicator"] = "font-weight: 600"
                }));

            var progress = image.Find(".custom-progress-root.ant-image-progress-wrapper");
            Assert.Equal("progressbar", progress.GetAttribute("role"));
            Assert.Equal("53", progress.GetAttribute("aria-valuenow"));
            Assert.Equal("53%", progress.GetAttribute("aria-label"));
            Assert.Contains("max-width: 320px", progress.GetAttribute("style"));
            Assert.Contains("custom-progress-content", progress.QuerySelector(".ant-image-progress-content")!.ClassList);
            Assert.Contains("color: rebeccapurple", progress.QuerySelector(".ant-image-progress-content")!.GetAttribute("style"));
            Assert.Contains("custom-progress-rail", progress.QuerySelector(".ant-image-progress-rail")!.ClassList);
            Assert.Contains("border-radius: 4px", progress.QuerySelector(".ant-image-progress-rail")!.GetAttribute("style"));
            Assert.Contains("--progress-percent:53%", progress.QuerySelector(".ant-image-progress-rail")!.GetAttribute("style"));
            Assert.Contains("custom-progress-indicator", progress.QuerySelector(".ant-image-progress-indicator")!.ClassList);
            Assert.Contains("font-weight: 600", progress.QuerySelector(".ant-image-progress-indicator")!.GetAttribute("style"));
            Assert.Equal("53%", progress.QuerySelector(".ant-image-progress-indicator")?.TextContent);
        }
    }
}
