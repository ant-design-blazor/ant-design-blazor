// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using AntDesign.JsInterop;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace AntDesign.Tests.Alert
{
    public class AlertTests : AntDesignTestBase
    {
        public AlertTests() : base()
        {
            JSInterop
                .Setup<HtmlElement>("AntDesign.interop.domInfoHelper.getInfo", _ => true)
                .SetResult(new HtmlElement());
        }

        [Theory]
        [InlineData(AlertType.Warning, "ant-alert-warning")]
        [InlineData(AlertType.Success, "ant-alert-success")]
        [InlineData(AlertType.Error, "ant-alert-error")]
        [InlineData(AlertType.Info, "ant-alert-info")]
        public void ItShouldSetProperClassForTypeParameter(string type, string expectedClass)
        {
            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters.Add(x => x.Type, type));

            systemUnderTest.MarkupMatches($"<div data-show=\"true\" class=\" ant-alert {expectedClass} ant-alert-no-icon\" style=\"\" id:ignore>"
                + "<div class=\"ant-alert-content\"></div>"
                + "</div>");
        }

        [Theory]
        [InlineData(AlertType.Warning, MessageOnlyWarningIcon)]
        [InlineData(AlertType.Success, MessageOnlySuccessIcon)]
        [InlineData(AlertType.Error, MessageOnlyErrorIcon)]
        [InlineData(AlertType.Info, MessageOnlyInfoIcon)]
        public void ItShouldShowIconWithMessage(string type, string expectedMarkup)
        {
            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters
                .Add(x => x.Message, "Test")
                .Add(x => x.Type, type)
                .Add(x => x.ShowIcon, true));

            systemUnderTest.Find(".ant-alert-icon").MarkupMatches(expectedMarkup);
        }

        [Theory]
        [InlineData(AlertType.Warning, MessageAndDescriptionWarningIcon)]
        [InlineData(AlertType.Success, MessageAndDescriptionSuccessIcon)]
        [InlineData(AlertType.Error, MessageAndDescriptionErrorIcon)]
        [InlineData(AlertType.Info, MessageAndDescriptionInfoIcon)]
        public void ItShouldShowIconWithDescription(string type, string expectedMarkup)
        {
            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters
                .Add(x => x.Message, "Test")
                .Add(x => x.Description, "Test")
                .Add(x => x.Type, type)
                .Add(x => x.ShowIcon, true));

            systemUnderTest.Find(".ant-alert-icon").MarkupMatches(expectedMarkup);
        }

        [Fact]
        public void ItShouldShowCustomIcon()
        {
            RenderFragment fragment = builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(0, "Icon");

                builder.CloseElement();
            };

            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters
                .Add(x => x.Icon, fragment)
                .Add(x => x.ShowIcon, true));

            systemUnderTest.MarkupMatches(@"<div data-show=""true"" class="" ant-alert ant-alert-info"" style:ignore id:ignore>
                <div class=""ant-alert-icon"">
                    <span>Icon</span>
                </div>
                <div class=""ant-alert-content"">
                </div>
            </div>");
        }

        [Fact]
        public void ItShouldNotShowCustomIconWhenShowIconFalse()
        {
            RenderFragment fragment = builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(0, "Icon");

                builder.CloseElement();
            };

            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters
                .Add(x => x.Icon, fragment)
                .Add(x => x.ShowIcon, false));

            systemUnderTest.MarkupMatches(@"<div data-show=""true"" class="" ant-alert ant-alert-info ant-alert-no-icon"" style:ignore id:ignore>
                <div class=""ant-alert-content"">
                </div>
            </div>");
        }

        [Fact]
        public void ItShouldShowClose()
        {
            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters.Add(x => x.Closable, true));

            systemUnderTest.Find("button.ant-alert-close-icon").Should().NotBeNull();
        }

        [Fact]
        public void ItShouldShowCustomCloseText()
        {
            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters
                .Add(x => x.Closable, true)
                .Add(x => x.CloseText, "Close Please"));

            systemUnderTest.Find("button.ant-alert-close-icon").MarkupMatches("<button type=\"button\" class=\"ant-alert-close-icon\" tabindex=\"0\">"
                + "<span class=\"ant-alert-close-text\">Close Please</span>"
                + "</button>");
        }

        [Fact]
        public async Task ItShouldRemoveElementWhenCloseClicked()
        {
            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters.Add(x => x.Closable, true));

            await systemUnderTest.InvokeAsync(() => systemUnderTest.Find("button.ant-alert-close-icon").Click());

            systemUnderTest.WaitForAssertion(() => systemUnderTest.MarkupMatches(string.Empty));
        }

        [Fact]
        public async Task ItShouldCallOnCloseWhenCloseClicked()
        {
            var delegateExecuted = false;

            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters
                .Add(x => x.Closable, true)
                .Add(x => x.OnClose, () => delegateExecuted = true)
            );

            await systemUnderTest.InvokeAsync(() => systemUnderTest.Find("button.ant-alert-close-icon").Click());

            systemUnderTest.WaitForAssertion(() => delegateExecuted.Should().BeTrue());
        }

        [Fact]
        public async Task ItShouldCallAfterCloseWhenCloseClickedAndElementHides()
        {
            var delegateExecuted = false;

            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters
                .Add(x => x.Closable, true)
                .Add(x => x.AfterClose, () => delegateExecuted = true)
            );

            await systemUnderTest.InvokeAsync(() => systemUnderTest.Find("button.ant-alert-close-icon").Click());

            systemUnderTest.WaitForAssertion(() => systemUnderTest.MarkupMatches(string.Empty));

            systemUnderTest.WaitForAssertion(() => delegateExecuted.Should().BeTrue());
        }

        [Fact]
        public async Task ItShouldApplyClassesForSmoothCloseOnCloseClicked()
        {
            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters.Add(x => x.Closable, true));

            await systemUnderTest.InvokeAsync(() => systemUnderTest.Find("button.ant-alert-close-icon").Click());

            systemUnderTest.WaitForAssertion(() => systemUnderTest.Find("div.ant-alert-motion").Should().NotBeNull());

            systemUnderTest.WaitForAssertion(() => systemUnderTest.Find("div.ant-alert-motion-leave").Should().NotBeNull());

            systemUnderTest.WaitForAssertion(() => systemUnderTest.Find("div.ant-alert-motion-leave-active").Should().NotBeNull());
        }

        [Fact]
        public void ItShouldRenderMessage()
        {
            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters.Add(x => x.Message, "Test message"));

            systemUnderTest.MarkupMatches($"<div data-show=\"true\" class=\" ant-alert ant-alert-info ant-alert-no-icon\" style=\"\" id:ignore>"
                + "<div class=\"ant-alert-content\">"
                + "<div class=\"ant-alert-message\">Test message</div>"
                + "</div></div>");
        }

        [Fact]
        public void ItShouldRenderMessageTemplate()
        {
            RenderFragment fragment = builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(0, "Test Message");

                builder.CloseElement();
            };

            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters.Add(x => x.MessageTemplate, fragment));

            systemUnderTest.MarkupMatches($"<div data-show=\"true\" class=\" ant-alert ant-alert-info ant-alert-no-icon\" style=\"\" id:ignore>"
                + "<div class=\"ant-alert-content\">"
                + "<div class=\"ant-alert-message\"><span>Test Message</span></div>"
                + "</div></div>");
        }

        [Fact]
        public void ItShouldRenderChildContent()
        {
            RenderFragment fragment = builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(0, "Test Message");

                builder.CloseElement();
            };

            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters.Add(x => x.ChildContent, fragment));

            systemUnderTest.MarkupMatches($"<div data-show=\"true\" class=\" ant-alert ant-alert-info ant-alert-no-icon ant-alert-with-description\" style=\"\" id:ignore>"
                + "<div class=\"ant-alert-content\">"
                + "<div class=\"ant-alert-description\"><span>Test Message</span></div>"
                + "</div></div>");
        }

        [Fact]
        public void ItShouldRenderDescription()
        {
            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters
                .Add(x => x.Description, "Here is some more detail")
                .Add(x => x.Message, "Something happened"));

            systemUnderTest.MarkupMatches($"<div data-show=\"true\" class=\" ant-alert ant-alert-info ant-alert-no-icon ant-alert-with-description\" style=\"\" id:ignore>"
                + "<div class=\"ant-alert-content\">"
                + "<div class=\"ant-alert-message\">Something happened</div><div class=\"ant-alert-description\">Here is some more detail</div>"
                + "</div></div>");
        }

        [Fact]
        public void ItShouldRenderAsBannerWithDefaults()
        {
            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters.Add(x => x.Banner, true));

            systemUnderTest.MarkupMatches(@"<div data-show=""true"" class="" ant-alert ant-alert-warning ant-alert-banner"" style:ignore id:ignore>
                <span role=""img"" class=""ant-alert-icon anticon anticon-exclamation-circle"" id:ignore>
                    <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor"" style=""pointer-events: none;""
                        xmlns=""http://www.w3.org/2000/svg"" class=""icon"" viewBox=""0 0 1024 1024"">
                        <path
                            d=""M512 64C264.6 64 64 264.6 64 512s200.6 448 448 448 448-200.6 448-448S759.4 64 512 64zm-32 232c0-4.4 3.6-8 8-8h48c4.4 0 8 3.6 8 8v272c0 4.4-3.6 8-8 8h-48c-4.4 0-8-3.6-8-8V296zm32 440a48.01 48.01 0 0 1 0-96 48.01 48.01 0 0 1 0 96z"">
                        </path>
                    </svg>
                </span>
                <div class=""ant-alert-content"">
                </div>
            </div>");
        }

        [Fact]
        public void ItShouldRenderAsBannerWithNoIcon()
        {
            var systemUnderTest = RenderComponent<AntDesign.Alert>(parameters => parameters
                .Add(x => x.Banner, true)
                .Add(x => x.ShowIcon, false));

            systemUnderTest.MarkupMatches(@"<div data-show=""true"" class="" ant-alert ant-alert-warning ant-alert-no-icon ant-alert-banner"" style:ignore id:ignore>
                <div class=""ant-alert-content"">
                </div>
            </div>");
        }

        #region Test Data

        public const string MessageOnlySuccessIcon = @"<span role=""img"" class=""ant-alert-icon anticon anticon-check-circle"" id:ignore>
            <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor"" style=""pointer-events: none;"" xmlns=""http://www.w3.org/2000/svg"" class=""icon"" viewBox=""0 0 1024 1024"">
                <path d=""M512 64C264.6 64 64 264.6 64 512s200.6 448 448 448 448-200.6 448-448S759.4 64 512 64zm193.5 301.7l-210.6 292a31.8 31.8 0 0 1-51.7 0L318.5 484.9c-3.8-5.3 0-12.7 6.5-12.7h46.9c10.2 0 19.9 4.9 25.9 13.3l71.2 98.8 157.2-218c6-8.3 15.6-13.3 25.9-13.3H699c6.5 0 10.3 7.4 6.5 12.7z""></path>
            </svg>
        </span>";

        public const string MessageOnlyInfoIcon = @"<span role=""img"" class=""ant-alert-icon anticon anticon-info-circle"" id:ignore>
            <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor"" style=""pointer-events: none;"" xmlns=""http://www.w3.org/2000/svg"" class=""icon"" viewBox=""0 0 1024 1024"">
                 <path d=""M512 64C264.6 64 64 264.6 64 512s200.6 448 448 448 448-200.6 448-448S759.4 64 512 64zm32 664c0 4.4-3.6 8-8 8h-48c-4.4 0-8-3.6-8-8V456c0-4.4 3.6-8 8-8h48c4.4 0 8 3.6 8 8v272zm-32-344a48.01 48.01 0 0 1 0-96 48.01 48.01 0 0 1 0 96z""></path>
            </svg>
        </span>";

        public const string MessageOnlyWarningIcon = @"<span role=""img"" class=""ant-alert-icon anticon anticon-exclamation-circle"" id:ignore>
            <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor"" style=""pointer-events: none;"" xmlns=""http://www.w3.org/2000/svg"" class=""icon"" viewBox=""0 0 1024 1024"">
                <path d=""M512 64C264.6 64 64 264.6 64 512s200.6 448 448 448 448-200.6 448-448S759.4 64 512 64zm-32 232c0-4.4 3.6-8 8-8h48c4.4 0 8 3.6 8 8v272c0 4.4-3.6 8-8 8h-48c-4.4 0-8-3.6-8-8V296zm32 440a48.01 48.01 0 0 1 0-96 48.01 48.01 0 0 1 0 96z""></path>
            </svg>
        </span>";

        public const string MessageOnlyErrorIcon = @"<span role=""img"" class=""ant-alert-icon anticon anticon-close-circle"" id:ignore>
            <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor"" style=""pointer-events: none;"" xmlns=""http://www.w3.org/2000/svg"" class=""icon"" viewBox=""0 0 1024 1024"">
                <path d=""M512 64C264.6 64 64 264.6 64 512s200.6 448 448 448 448-200.6 448-448S759.4 64 512 64zm165.4 618.2l-66-.3L512 563.4l-99.3 118.4-66.1.3c-4.4 0-8-3.5-8-8 0-1.9.7-3.7 1.9-5.2l130.1-155L340.5 359a8.32 8.32 0 0 1-1.9-5.2c0-4.4 3.6-8 8-8l66.1.3L512 464.6l99.3-118.4 66-.3c4.4 0 8 3.5 8 8 0 1.9-.7 3.7-1.9 5.2L553.5 514l130 155c1.2 1.5 1.9 3.3 1.9 5.2 0 4.4-3.6 8-8 8z""></path>
            </svg>
        </span>";

        public const string MessageAndDescriptionSuccessIcon = @"<span role=""img"" class=""ant-alert-icon anticon anticon-check-circle"" id:ignore>
            <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor"" style=""pointer-events: none;""
                xmlns=""http://www.w3.org/2000/svg"" class=""icon"" viewBox=""0 0 1024 1024"">
                <path d=""M699 353h-46.9c-10.2 0-19.9 4.9-25.9 13.3L469 584.3l-71.2-98.8c-6-8.3-15.6-13.3-25.9-13.3H325c-6.5 0-10.3 7.4-6.5 12.7l124.6 172.8a31.8 31.8 0 0 0 51.7 0l210.6-292c3.9-5.3.1-12.7-6.4-12.7z""></path>
                <path d=""M512 64C264.6 64 64 264.6 64 512s200.6 448 448 448 448-200.6 448-448S759.4 64 512 64zm0 820c-205.4 0-372-166.6-372-372s166.6-372 372-372 372 166.6 372 372-166.6 372-372 372z""></path>
            </svg>
        </span>";

        public const string MessageAndDescriptionInfoIcon = @"<span role=""img"" class=""ant-alert-icon anticon anticon-info-circle"" id:ignore>
            <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor""
                style=""pointer-events: none;"" xmlns=""http://www.w3.org/2000/svg"" class=""icon"" viewBox=""0 0 1024 1024"">
                <path d=""M512 64C264.6 64 64 264.6 64 512s200.6 448 448 448 448-200.6 448-448S759.4 64 512 64zm0 820c-205.4 0-372-166.6-372-372s166.6-372 372-372 372 166.6 372 372-166.6 372-372 372z""></path>
                <path d=""M464 336a48 48 0 1 0 96 0 48 48 0 1 0-96 0zm72 112h-48c-4.4 0-8 3.6-8 8v272c0 4.4 3.6 8 8 8h48c4.4 0 8-3.6 8-8V456c0-4.4-3.6-8-8-8z""></path>
            </svg>
        </span>";

        public const string MessageAndDescriptionWarningIcon = @"<span role=""img"" class=""ant-alert-icon anticon anticon-exclamation-circle"" id:ignore>
            <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor"" style=""pointer-events: none;"" xmlns=""http://www.w3.org/2000/svg"" class=""icon"" viewBox=""0 0 1024 1024"">
                <path d=""M512 64C264.6 64 64 264.6 64 512s200.6 448 448 448 448-200.6 448-448S759.4 64 512 64zm0 820c-205.4 0-372-166.6-372-372s166.6-372 372-372 372 166.6 372 372-166.6 372-372 372z""></path>
                <path d=""M464 688a48 48 0 1 0 96 0 48 48 0 1 0-96 0zm24-112h48c4.4 0 8-3.6 8-8V296c0-4.4-3.6-8-8-8h-48c-4.4 0-8 3.6-8 8v272c0 4.4 3.6 8 8 8z""></path>
            </svg>
        </span>";

        public const string MessageAndDescriptionErrorIcon = @"<span role=""img"" class=""ant-alert-icon anticon anticon-close-circle"" id:ignore>
            <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor""
                style=""pointer-events: none;"" xmlns=""http://www.w3.org/2000/svg"" class=""icon"" viewBox=""0 0 1024 1024"">
                <path d=""M685.4 354.8c0-4.4-3.6-8-8-8l-66 .3L512 465.6l-99.3-118.4-66.1-.3c-4.4 0-8 3.5-8 8 0 1.9.7 3.7 1.9 5.2l130.1 155L340.5 670a8.32 8.32 0 0 0-1.9 5.2c0 4.4 3.6 8 8 8l66.1-.3L512 564.4l99.3 118.4 66 .3c4.4 0 8-3.5 8-8 0-1.9-.7-3.7-1.9-5.2L553.5 515l130.1-155c1.2-1.4 1.8-3.3 1.8-5.2z""></path>
                <path d=""M512 65C264.6 65 64 265.6 64 513s200.6 448 448 448 448-200.6 448-448S759.4 65 512 65zm0 820c-205.4 0-372-166.6-372-372s166.6-372 372-372 372 166.6 372 372-166.6 372-372 372z""></path>
            </svg>
        </span>";

        #endregion
    }
}
