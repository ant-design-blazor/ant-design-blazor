// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using AntDesign.JsInterop;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;
using Xunit;

namespace AntDesign.Tests.Avatar
{
    public class AvatarTests : AntDesignTestBase
    {
        private void ItShouldSetSizeProperly(OneOf<AvatarSize, string> size, string expectedClass, string expectedStyle)
        {
            var systemUnderTest = RenderComponent<AntDesign.Avatar>(parameters => parameters
                .Add(x => x.Size, size)
                .Add(x => x.Icon, "user"));

            systemUnderTest.MarkupMatches(@$"<span class=""ant-avatar ant-avatar-icon {expectedClass}"" style=""{expectedStyle}"" id:ignore>
                <span role=""img"" class="" anticon anticon-user"" id:ignore>
                    <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor"" style=""pointer-events: none;"" xmlns=""http://www.w3.org/2000/svg"" class=""icon"" viewBox=""0 0 1024 1024"">
                        <path d=""M858.5 763.6a374 374 0 0 0-80.6-119.5 375.63 375.63 0 0 0-119.5-80.6c-.4-.2-.8-.3-1.2-.5C719.5 518 760 444.7 760 362c0-137-111-248-248-248S264 225 264 362c0 82.7 40.5 156 102.8 201.1-.4.2-.8.3-1.2.5-44.8 18.9-85 46-119.5 80.6a375.63 375.63 0 0 0-80.6 119.5A371.7 371.7 0 0 0 136 901.8a8 8 0 0 0 8 8.2h60c4.4 0 7.9-3.5 8-7.8 2-77.2 33-149.5 87.8-204.3 56.7-56.7 132-87.9 212.2-87.9s155.5 31.2 212.2 87.9C779 752.7 810 825 812 902.2c.1 4.4 3.6 7.8 8 7.8h60a8 8 0 0 0 8-8.2c-1-47.8-10.9-94.3-29.5-138.2zM512 534c-45.9 0-89.1-17.9-121.6-50.4S340 407.9 340 362c0-45.9 17.9-89.1 50.4-121.6S466.1 190 512 190s89.1 17.9 121.6 50.4S684 316.1 684 362c0 45.9-17.9 89.1-50.4 121.6S557.9 534 512 534z"">
                        </path>
                    </svg>
                </span>
            </span>");
        }

        [Theory]
        [InlineData(AvatarSize.Small, "ant-avatar-sm", "")]
        [InlineData(AvatarSize.Large, "ant-avatar-lg", "")]
        public void ItShouldSetSizeProperlyWithAvatarSize(AvatarSize size, string expectedClass, string expectedStyle)
            => ItShouldSetSizeProperly(size, expectedClass, expectedStyle);

        [Theory]
        [InlineData("64.5", "", "width:64.5px;height:64.5px;line-height:64.5px;font-size:calc(64.5px / 2);")]
        public void ItShouldSetSizeProperlyWithString(string size, string expectedClass, string expectedStyle)
            => ItShouldSetSizeProperly(size, expectedClass, expectedStyle);

        [Theory]
        [InlineData(AvatarShape.Square, "ant-avatar-square")]
        [InlineData(AvatarShape.Circle, "ant-avatar-circle")]
        [InlineData(null, "")]
        public void ItShouldProperlyStyleShapes(AvatarShape? shape, string expectedClass)
        {
            var systemUnderTest = RenderComponent<AntDesign.Avatar>(parameters => parameters
                .Add(x => x.Shape, shape)
                .Add(x => x.Icon, "user"));

            systemUnderTest.MarkupMatches(@$"<span class=""ant-avatar ant-avatar-icon {expectedClass}"" style="""" id:ignore>
                <span role=""img"" class="" anticon anticon-user"" id:ignore>
                    <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor"" style=""pointer-events: none;"" xmlns=""http://www.w3.org/2000/svg"" class=""icon"" viewBox=""0 0 1024 1024"">
                        <path d=""M858.5 763.6a374 374 0 0 0-80.6-119.5 375.63 375.63 0 0 0-119.5-80.6c-.4-.2-.8-.3-1.2-.5C719.5 518 760 444.7 760 362c0-137-111-248-248-248S264 225 264 362c0 82.7 40.5 156 102.8 201.1-.4.2-.8.3-1.2.5-44.8 18.9-85 46-119.5 80.6a375.63 375.63 0 0 0-80.6 119.5A371.7 371.7 0 0 0 136 901.8a8 8 0 0 0 8 8.2h60c4.4 0 7.9-3.5 8-7.8 2-77.2 33-149.5 87.8-204.3 56.7-56.7 132-87.9 212.2-87.9s155.5 31.2 212.2 87.9C779 752.7 810 825 812 902.2c.1 4.4 3.6 7.8 8 7.8h60a8 8 0 0 0 8-8.2c-1-47.8-10.9-94.3-29.5-138.2zM512 534c-45.9 0-89.1-17.9-121.6-50.4S340 407.9 340 362c0-45.9 17.9-89.1 50.4-121.6S466.1 190 512 190s89.1 17.9 121.6 50.4S684 316.1 684 362c0 45.9-17.9 89.1-50.4 121.6S557.9 534 512 534z"">
                        </path>
                    </svg>
                </span>
            </span>");
        }

        [Theory]
        [InlineData(200, 64, 0.28)]
        [InlineData(56, 64, 1)]
        [InlineData(10, 64, 1)]
        [InlineData(57, 64, 0.9824561403508771929824561404)]
        public void ItShouldRenderAndScaleTextProperly(int textWidth, decimal avatarWidth, double expectedScale)
        {
            JSInterop
                .Setup<HtmlElement>("AntDesign.interop.domInfoHelper.getInfo", _ => true)
                .SetResult(new HtmlElement
                {
                    OffsetWidth = textWidth
                });

            JSInterop
                .Setup<DomRect>("AntDesign.interop.domInfoHelper.getBoundingClientRect", _ => true)
                .SetResult(new DomRect
                {
                    Width = avatarWidth
                });

            var systemUnderTest = RenderComponent<AntDesign.Avatar>(parameters => parameters
                .Add(x => x.Text, "KR"));

            systemUnderTest.MarkupMatches(@$"<span class=""ant-avatar"" style="""" id:ignore>
                <span class=""ant-avatar-string"" style=""transform: scale({expectedScale.ToString(CultureInfo.InvariantCulture)}) translateX(-50%);"">KR</span>
            </span>");
        }

        [Fact]
        public void ItShouldRenderAndScaleTextProperlyWhenUnableToGetJsInfo()
        {
            JSInterop
                .Setup<HtmlElement>("AntDesign.interop.domInfoHelper.getInfo", _ => true)
                .SetResult(null);

            JSInterop
                .Setup<DomRect>("AntDesign.interop.domInfoHelper.getBoundingClientRect", _ => true)
                .SetResult(null);

            var systemUnderTest = RenderComponent<AntDesign.Avatar>(parameters => parameters
                .Add(x => x.Text, "KR"));

            systemUnderTest.MarkupMatches(@$"<span class=""ant-avatar"" style="""" id:ignore>
                <span class=""ant-avatar-string"" style=""transform: scale(1) translateX(-50%);"">KR</span>
            </span>");
        }

        [Theory]
        [InlineData(200, 64, 0.28)]
        [InlineData(56, 64, 1)]
        [InlineData(10, 64, 1)]
        [InlineData(57, 64, 0.9824561403508771929824561404)]
        public void ItShouldRenderAndScaleChildContentProperly(int textWidth, decimal avatarWidth, double expectedScale)
        {
            JSInterop
                .Setup<HtmlElement>("AntDesign.interop.domInfoHelper.getInfo", _ => true)
                .SetResult(new HtmlElement
                {
                    OffsetWidth = textWidth
                });

            JSInterop
                .Setup<DomRect>("AntDesign.interop.domInfoHelper.getBoundingClientRect", _ => true)
                .SetResult(new DomRect
                {
                    Width = avatarWidth
                });

            RenderFragment fragment = builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(0, "Text");

                builder.CloseElement();
            };

            var systemUnderTest = RenderComponent<AntDesign.Avatar>(parameters => parameters
                .Add(x => x.ChildContent, fragment));

            systemUnderTest.MarkupMatches(@$"<span class=""ant-avatar"" style="""" id:ignore>
                <span class=""ant-avatar-string"" style=""transform: scale({expectedScale.ToString(CultureInfo.InvariantCulture)}) translateX(-50%);"">
                    <span>Text</span>
                </span>
            </span>");
        }

        [Fact]
        public void ItShouldCallOnErrorWhenImageLoadErrors()
        {
            var calledOnError = false;

            var systemUnderTest = RenderComponent<AntDesign.Avatar>(parameters => parameters
                .Add(x => x.Size, AvatarSize.Default)
                .Add(x => x.Src, "InvalidImage")
                .Add(x => x.OnError, () => calledOnError = true)
                .Add(x => x.Icon, "user"));

            systemUnderTest.Find("img").TriggerEvent("onerror", new ErrorEventArgs());

            systemUnderTest.WaitForAssertion(() => calledOnError.Should().BeTrue());
        }

        [Fact]
        public void ItShouldRenderIconWhenImageErrorsAndGivenIcon()
        {
            var systemUnderTest = RenderComponent<AntDesign.Avatar>(parameters => parameters
                .Add(x => x.Size, AvatarSize.Default)
                .Add(x => x.Src, "InvalidImage")
                .Add(x => x.Icon, "user")
                .Add(x => x.Text, "Won't be used"));

            systemUnderTest.Find("img").TriggerEvent("onerror", new ErrorEventArgs());

            systemUnderTest.WaitForAssertion(() => systemUnderTest.MarkupMatches(@$"<span class=""ant-avatar ant-avatar-icon"" style="""" id:ignore>
                <span role=""img"" class="" anticon anticon-user"" id:ignore>
                    <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor"" style=""pointer-events: none;"" xmlns=""http://www.w3.org/2000/svg"" class=""icon"" viewBox=""0 0 1024 1024"">
                        <path d=""M858.5 763.6a374 374 0 0 0-80.6-119.5 375.63 375.63 0 0 0-119.5-80.6c-.4-.2-.8-.3-1.2-.5C719.5 518 760 444.7 760 362c0-137-111-248-248-248S264 225 264 362c0 82.7 40.5 156 102.8 201.1-.4.2-.8.3-1.2.5-44.8 18.9-85 46-119.5 80.6a375.63 375.63 0 0 0-80.6 119.5A371.7 371.7 0 0 0 136 901.8a8 8 0 0 0 8 8.2h60c4.4 0 7.9-3.5 8-7.8 2-77.2 33-149.5 87.8-204.3 56.7-56.7 132-87.9 212.2-87.9s155.5 31.2 212.2 87.9C779 752.7 810 825 812 902.2c.1 4.4 3.6 7.8 8 7.8h60a8 8 0 0 0 8-8.2c-1-47.8-10.9-94.3-29.5-138.2zM512 534c-45.9 0-89.1-17.9-121.6-50.4S340 407.9 340 362c0-45.9 17.9-89.1 50.4-121.6S466.1 190 512 190s89.1 17.9 121.6 50.4S684 316.1 684 362c0 45.9-17.9 89.1-50.4 121.6S557.9 534 512 534z"">
                        </path>
                    </svg>
                </span>
            </span>"));
        }

        [Fact]
        public void ItShouldRenderTextWhenImageErrorsAndGivenTextButNotIcon()
        {
            JSInterop
                .Setup<HtmlElement>("AntDesign.interop.domInfoHelper.getInfo", _ => true)
                .SetResult(new HtmlElement());

            JSInterop
                .Setup<DomRect>("AntDesign.interop.domInfoHelper.getBoundingClientRect", _ => true)
                .SetResult(new DomRect());

            var systemUnderTest = RenderComponent<AntDesign.Avatar>(parameters => parameters
                .Add(x => x.Size, AvatarSize.Default)
                .Add(x => x.Src, "InvalidImage")
                .Add(x => x.Text, "USER"));

            systemUnderTest.Find("img").TriggerEvent("onerror", new ErrorEventArgs());

            systemUnderTest.WaitForAssertion(() => systemUnderTest.Find(".ant-avatar-string")
                .MarkupMatches(@$"<span class=""ant-avatar-string"" style:ignore>
                    USER
                </span>"));
        }
    }
}
