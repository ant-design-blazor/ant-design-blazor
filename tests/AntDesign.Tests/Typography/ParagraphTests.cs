// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Bunit;
using FluentAssertions;
using Xunit;

namespace AntDesign.Tests.Typography
{
    public class ParagraphTests : AntDesignTestBase
    {
        [Fact]
        public void ItShouldRenderBasicProperly()
        {
            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"
                <div class=""ant-typography"">
                    Something
                </div>");
        }

        [Theory]
        [InlineData(TextElementType.Secondary, "ant-typography-secondary")]
        [InlineData(TextElementType.Warning, "ant-typography-warning")]
        [InlineData(TextElementType.Danger, "ant-typography-danger")]
        public void ItShouldRenderTypesProperly(TextElementType type, string expectedClass)
        {
            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .Add(x => x.Type, type)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<div class=""ant-typography {expectedClass}"">
                Something
            </div>");
        }

        [Fact]
        public void ItShouldRenderCustomStyleOnDiv()
        {
            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .Add(x => x.Style, "color: red")
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<div class=""ant-typography"" style=""color: red"">
                Something
            </div>");
        }

        [Fact]
        public void EllipsisConfig_clamps_rows_and_expand_button_toggles_full_text()
        {
            var expandedCount = 0;
            var config = new TypographyEllipsisConfig
            {
                Rows = 2,
                Expandable = true,
                OnExpand = () => expandedCount++
            };
            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .Add(x => x.EllipsisConfig, config)
                .AddChildContent("A long paragraph that should be clamped to two rows."));

            systemUnderTest.Find(".ant-typography").ClassList.Should().Contain("ant-typography-ellipsis-multiple-line");
            systemUnderTest.Find(".ant-typography").GetAttribute("style").Should().Contain("-webkit-line-clamp: 2");
            systemUnderTest.Find(".ant-typography-expand").TextContent.Should().Be("more");

            systemUnderTest.Find(".ant-typography-expand").Click();

            systemUnderTest.FindAll(".ant-typography-collapse").Should().BeEmpty();
            systemUnderTest.Find(".ant-typography").GetAttribute("style").Should().BeNullOrEmpty();
            systemUnderTest.Find(".ant-typography").ClassList.Should().NotContain("ant-typography-ellipsis-multiple-line");
            expandedCount.Should().Be(1);
        }

        [Fact]
        public void Ellipsis_supports_controlled_expansion_and_collapsing()
        {
            var config = new TypographyEllipsisConfig { Rows = 2, Expandable = true, Collapsible = true, Expanded = false };
            config.OnExpandedChange = Microsoft.AspNetCore.Components.EventCallback.Factory.Create<bool>(this, expanded => config.Expanded = expanded);
            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .Add(x => x.EllipsisConfig, config)
                .AddChildContent("A long paragraph with controlled expandable content."));

            systemUnderTest.Find(".ant-typography-expand").Click();

            config.Expanded.Should().BeTrue();
            systemUnderTest.Find(".ant-typography-collapse").TextContent.Should().Be("less");
            systemUnderTest.Find(".ant-typography").ClassList.Should().NotContain("ant-typography-ellipsis-multiple-line");

            systemUnderTest.Find(".ant-typography-collapse").Click();

            config.Expanded.Should().BeFalse();
            systemUnderTest.Find(".ant-typography-expand").TextContent.Should().Be("more");
            systemUnderTest.Find(".ant-typography").ClassList.Should().Contain("ant-typography-ellipsis-multiple-line");
        }

        [Fact]
        public void Ellipsis_suffix_renders_at_the_clamped_paragraph_end_and_title_preserves_full_text()
        {
            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .Add(x => x.EllipsisConfig, new TypographyEllipsisConfig { Rows = 2, Suffix = "— Author" })
                .Add(x => x.Title, "Full text — Author")
                .AddChildContent("A long paragraph that should be clamped while retaining an attribution."));

            systemUnderTest.Find(".ant-typography").GetAttribute("style").Should().Contain("position: relative");
            systemUnderTest.Find(".ant-typography").GetAttribute("title").Should().Be("Full text — Author");
            systemUnderTest.Find(".ant-typography-ellipsis-suffix").GetAttribute("style").Should().Contain("position: absolute");
            systemUnderTest.Find(".ant-typography-ellipsis-suffix").TextContent.Should().Be("— Author");
        }

        [Fact]
        public void Editable_supports_start_enter_commit_and_escape_cancel()
        {
            JSInterop.Setup<AntDesign.Internal.TextAreaInfo>(JSInteropConstants.InputComponentHelper.RegisterResizeTextArea, _ => true);
            var started = 0;
            var changed = 0;
            var cancelled = 0;
            var ended = 0;
            var config = new TypographyEditableConfig
            {
                Text = "Original",
                OnStart = () => started++,
                OnChange = _ => changed++,
                OnCancel = () => cancelled++,
                OnEnd = () => ended++
            };
            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .Add(x => x.Editable, true)
                .Add(x => x.EditConfig, config)
                .AddChildContent(builder => builder.AddContent(0, config.Text)));

            systemUnderTest.Find(".ant-typography-edit").Click();
            systemUnderTest.Find("textarea").Input("Discard this draft");
            systemUnderTest.Find("textarea").KeyUp(new Microsoft.AspNetCore.Components.Web.KeyboardEventArgs { Key = "Escape" });

            config.Text.Should().Be("Original");
            started.Should().Be(1);
            cancelled.Should().Be(1);
            changed.Should().Be(0);

            systemUnderTest.Find(".ant-typography-edit").Click();
            systemUnderTest.Find("textarea").Input("  Saved draft  ");
            systemUnderTest.Find("textarea").KeyUp(new Microsoft.AspNetCore.Components.Web.KeyboardEventArgs { Key = "Enter" });

            ended.Should().Be(1);
            config.Text.Should().Be("Saved draft");
            started.Should().Be(2);
            changed.Should().Be(1);
            systemUnderTest.Find(".ant-typography").TextContent.Should().Contain("Saved draft");
        }

        [Fact]
        public void ItShouldRenderMark()
        {
            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .Add(x => x.Mark, true)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<div class:ignore>
                <mark>Something</mark>
            </div>");
        }

        [Fact]
        public void ItShouldRenderDelete()
        {
            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .Add(x => x.Delete, true)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<div class:ignore>
                <del>Something</del>
            </div>");
        }

        [Fact]
        public void ItShouldRenderUnderline()
        {
            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .Add(x => x.Underline, true)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<div class:ignore>
                <u>Something</u>
            </div>");
        }

        [Fact]
        public void ItShouldRenderCode()
        {
            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .Add(x => x.Code, true)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<div class:ignore>
                <code>Something</code>
            </div>");
        }

        [Fact]
        public void ItShouldRenderKeyboard()
        {
            var systemUnderTest = RenderComponent<Text>(parameters => parameters
                .Add(x => x.Keyboard, true)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<span class:ignore>
                <kbd>Something</kbd>
            </span>");
        }

        [Fact]
        public void ItShouldRenderStrong()
        {
            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .Add(x => x.Strong, true)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<div class:ignore>
                <strong>Something</strong>
            </div>");
        }

        [Fact]
        public void ItShouldRenderDisabled()
        {
            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .Add(x => x.Disabled, true)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<div class=""ant-typography ant-typography-disabled"">
                Something
            </div>");
        }

        [Fact]
        public void ItShouldRenderCopyIconWhenCopyable()
        {
            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .Add(x => x.Copyable, true)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches(
                $@"<div class=""ant-typography"">
                    Something
                    <div  style=""border: 0px; background: transparent; padding: 0px; line-height: inherit; display: inline-block;"" class=""ant-typography-copy"" tabindex=""0"" role=""button"">
                        <span role=""img"" class=""anticon anticon-copy"" id:ignore >
                          <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor"" style=""pointer-events: none;"" xmlns=""http://www.w3.org/2000/svg"" class=""icon"" viewBox=""0 0 1024 1024"">
                            <path d=""M832 64H296c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8h496v688c0 4.4 3.6 8 8 8h56c4.4 0 8-3.6 8-8V96c0-17.7-14.3-32-32-32zM704 192H192c-17.7 0-32 14.3-32 32v530.7c0 8.5 3.4 16.6 9.4 22.6l173.3 173.3c2.2 2.2 4.7 4 7.4 5.5v1.9h4.2c3.5 1.3 7.2 2 11 2H704c17.7 0 32-14.3 32-32V224c0-17.7-14.3-32-32-32zM350 856.2L263.9 770H350v86.2zM664 888H414V746c0-22.1-17.9-40-40-40H232V264h432v624z""></path>
                          </svg>
                        </span>
                    </div>
                </div>");
        }

        #region Copy functionality

        [Fact]
        public void ItShouldCopyElementByDefault_WhenCopyIconLinkClicked()
        {
            JSInterop.Setup<object>(JSInteropConstants.CopyElement, _ => true).SetResult(new { });

            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .Add(x => x.Copyable, true)
                .AddChildContent("Something"));

            systemUnderTest.Find(".ant-typography-copy").Click();

            JSInterop.VerifyInvoke(JSInteropConstants.CopyElement, "Copy JS was not called");
        }

        [Fact]
        public void ItShouldCopyTextWhenCopyConfigSet_WhenCopyIconLinkClicked()
        {
            var copyConfig = new TypographyCopyableConfig
            {
                Text = "Something to copy"
            };

            JSInterop.Setup<object>(JSInteropConstants.Copy, _ => true).SetResult(new { });

            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .Add(x => x.CopyConfig, copyConfig)
                .Add(x => x.Copyable, true)
                .AddChildContent("Something"));

            systemUnderTest.Find(".ant-typography-copy").Click();

            JSInterop.VerifyInvoke(JSInteropConstants.Copy, "Copy JS was not called");
        }

        [Fact]
        public void ItShouldCallCopyConfigOnCopy_WhenCopyIconLinkClicked()
        {
            var methodCalled = false;

            var copyConfig = new TypographyCopyableConfig
            {
                OnCopy = () => methodCalled = true,
                Text = "Something to copy"
            };

            JSInterop.Setup<object>(JSInteropConstants.Copy, _ => true).SetResult(new { });

            var systemUnderTest = RenderComponent<Paragraph>(parameters => parameters
                .Add(x => x.CopyConfig, copyConfig)
                .Add(x => x.Copyable, true)
                .AddChildContent("Something"));

            systemUnderTest.Find(".ant-typography-copy").Click();

            methodCalled.Should().BeTrue();
        }

        #endregion Copy functionality
    }
}
