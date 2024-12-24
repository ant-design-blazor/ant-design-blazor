// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Bunit;
using FluentAssertions;
using Xunit;

namespace AntDesign.Tests.Typography
{
    public class TextTests : AntDesignTestBase
    {
        [Fact]
        public void ItShouldRenderBasicProperly()
        {
            var systemUnderTest = RenderComponent<Text>(parameters => parameters
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<span class=""ant-typography"">Something</span>");
        }

        [Theory]
        [InlineData(TextElementType.Secondary, "ant-typography-secondary")]
        [InlineData(TextElementType.Warning, "ant-typography-warning")]
        [InlineData(TextElementType.Danger, "ant-typography-danger")]
        public void ItShouldRenderTypesProperly(TextElementType type, string expectedClass)
        {
            var systemUnderTest = RenderComponent<Text>(parameters => parameters
                .Add(x => x.Type, type)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<span class=""ant-typography {expectedClass}"">Something</span>");
        }

        [Fact]
        public void ItShouldRenderCustomStyle()
        {
            var systemUnderTest = RenderComponent<Text>(parameters => parameters
                .Add(x => x.Style, "color: red")
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<span class=""ant-typography"" style=""color: red"">Something</span>");
        }

        [Fact]
        public void ItShouldRenderMark()
        {
            var systemUnderTest = RenderComponent<Text>(parameters => parameters
                .Add(x => x.Mark, true)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<span class:ignore><mark>Something</mark></span>");
        }

        [Fact]
        public void ItShouldRenderDelete()
        {
            var systemUnderTest = RenderComponent<Text>(parameters => parameters
                .Add(x => x.Delete, true)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<span class:ignore><del>Something</del></span>");
        }

        [Fact]
        public void ItShouldRenderUnderline()
        {
            var systemUnderTest = RenderComponent<Text>(parameters => parameters
                .Add(x => x.Underline, true)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<span class:ignore><u>Something</u></span>");
        }

        [Fact]
        public void ItShouldRenderCode()
        {
            var systemUnderTest = RenderComponent<Text>(parameters => parameters
                .Add(x => x.Code, true)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<span class:ignore><code>Something</code></span>");
        }

        [Fact]
        public void ItShouldRenderKeyboard()
        {
            var systemUnderTest = RenderComponent<Text>(parameters => parameters
                .Add(x => x.Keyboard, true)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<span class:ignore><kbd>Something</kbd></span>");
        }

        [Fact]
        public void ItShouldRenderStrong()
        {
            var systemUnderTest = RenderComponent<Text>(parameters => parameters
                .Add(x => x.Strong, true)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<span class:ignore><strong>Something</strong></span>");
        }

        [Fact]
        public void ItShouldRenderDisabled()
        {
            var systemUnderTest = RenderComponent<Text>(parameters => parameters
                .Add(x => x.Disabled, true)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"<span class=""ant-typography ant-typography-disabled"">Something</span>");
        }

        [Fact]
        public void ItShouldRenderCopyIconWhenCopyable()
        {
            var systemUnderTest = RenderComponent<Text>(parameters => parameters
                .Add(x => x.Copyable, true)
                .AddChildContent("Something"));

            systemUnderTest.MarkupMatches($@"
                    <span class=""ant-typography"">Something<div  style=""border: 0px; background: transparent; padding: 0px; line-height: inherit; display: inline-block;"" class=""ant-typography-copy"" tabindex=""0"" role=""button"">
                        <span role=""img"" class=""anticon anticon-copy"" id:ignore >
                          <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor"" style=""pointer-events: none;"" xmlns=""http://www.w3.org/2000/svg"" class=""icon"" viewBox=""0 0 1024 1024"">
                            <path d=""M832 64H296c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8h496v688c0 4.4 3.6 8 8 8h56c4.4 0 8-3.6 8-8V96c0-17.7-14.3-32-32-32zM704 192H192c-17.7 0-32 14.3-32 32v530.7c0 8.5 3.4 16.6 9.4 22.6l173.3 173.3c2.2 2.2 4.7 4 7.4 5.5v1.9h4.2c3.5 1.3 7.2 2 11 2H704c17.7 0 32-14.3 32-32V224c0-17.7-14.3-32-32-32zM350 856.2L263.9 770H350v86.2zM664 888H414V746c0-22.1-17.9-40-40-40H232V264h432v624z""></path>
                          </svg>
                        </span>
                      </div>
                    </span>");
        }

        #region Copy functionality

        [Fact]
        public void ItShouldCopyElementByDefault_WhenCopyIconLinkClicked()
        {
            JSInterop.Setup<object>(JSInteropConstants.CopyElement, _ => true).SetResult(new { });

            var systemUnderTest = RenderComponent<Text>(parameters => parameters
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

            var systemUnderTest = RenderComponent<Text>(parameters => parameters
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

            var systemUnderTest = RenderComponent<Text>(parameters => parameters
                .Add(x => x.CopyConfig, copyConfig)
                .Add(x => x.Copyable, true)
                .AddChildContent("Something"));

            systemUnderTest.Find(".ant-typography-copy").Click();

            methodCalled.Should().BeTrue();
        }

        #endregion Copy functionality
    }
}
