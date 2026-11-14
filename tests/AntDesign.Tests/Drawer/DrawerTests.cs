// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace AntDesign.Tests.Drawer
{
    public class DrawerTests : AntDesignTestBase
    {
        public DrawerTests() : base()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
        }

        [Theory]
        [InlineData(DrawerPlacement.Left, "width:256px;transform:translateX(-100%);", "height:100%")]
        [InlineData(DrawerPlacement.Right, "width:256px;transform:translateX(100%);", "height:100%")]
        [InlineData(DrawerPlacement.Top, "height:256px;transform:translateY(-100%);", "")]
        [InlineData(DrawerPlacement.Bottom, "height:256px;transform:translateY(100%);", "")]
        public void ItShouldRenderWrapperProperly(DrawerPlacement placement, string contentWrapperStyle, string bodyWrapperStyle)
        {
            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters.Add(x => x.Placement, placement));

            systemUnderTest.MarkupMatches($@"<div class=""ant-drawer ant-drawer-{placement.ToString().ToLowerInvariant()}"" style=""z-index:-9999;"" id:ignore>
                <div class=""ant-drawer-content-wrapper"" style=""{contentWrapperStyle}"" id:ignore>
                    <div class=""ant-drawer-content"">
                        <div class=""ant-drawer-wrapper-body"" style=""{bodyWrapperStyle}"">
                            <div class=""ant-drawer-header-no-title"">
                                <button type=""button"" aria-label=""Close"" class=""ant-drawer-close"">
                                    <span role=""img"" class=""anticon anticon-close"" id:ignore>
                                        <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor""
                                            style=""pointer-events: none;"" xmlns=""http://www.w3.org/2000/svg"" class=""icon""
                                            viewBox=""0 0 1024 1024"">
                                            <path
                                                d=""M563.8 512l262.5-312.9c4.4-5.2.7-13.1-6.1-13.1h-79.8c-4.7 0-9.2 2.1-12.3 5.7L511.6 449.8 295.1 191.7c-3-3.6-7.5-5.7-12.3-5.7H203c-6.8 0-10.5 7.9-6.1 13.1L459.4 512 196.9 824.9A7.95 7.95 0 0 0 203 838h79.8c4.7 0 9.2-2.1 12.3-5.7l216.5-258.1 216.5 258.1c3 3.6 7.5 5.7 12.3 5.7h79.8c6.8 0 10.5-7.9 6.1-13.1L563.8 512z"">
                                            </path>
                                        </svg>
                                    </span>
                                </button>
                            </div>
                            <div class=""ant-drawer-body"">
                            </div>
                        </div>
                    </div>
                </div>
            </div>");
        }

        [Theory]
        [InlineData(DrawerPlacement.Top, DrawerPlacement.Left, "width:256px;transform:translateX(-100%);", "height:100%")]
        [InlineData(DrawerPlacement.Top, DrawerPlacement.Right, "width:256px;transform:translateX(100%);", "height:100%")]
        [InlineData(DrawerPlacement.Left, DrawerPlacement.Top, "height:256px;transform:translateY(-100%);", "")]
        [InlineData(DrawerPlacement.Left, DrawerPlacement.Bottom, "height:256px;transform:translateY(100%);", "")]
        public void ItShouldChangePlacementAfterRender(DrawerPlacement initialPlacement, DrawerPlacement placement, string contentWrapperStyle, string bodyWrapperStyle)
        {
            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters.Add(x => x.Placement, initialPlacement));

            systemUnderTest.SetParametersAndRender(parameters => parameters.Add(x => x.Placement, placement));

            systemUnderTest.MarkupMatches($@"<div class=""ant-drawer ant-drawer-{placement.ToString().ToLowerInvariant()}"" style=""z-index:-9999;"" id:ignore>
                <div class=""ant-drawer-content-wrapper"" style=""{contentWrapperStyle}"" id:ignore>
                    <div class=""ant-drawer-content"">
                        <div class=""ant-drawer-wrapper-body"" style=""{bodyWrapperStyle}"">
                            <div class=""ant-drawer-header-no-title"">
                                <button type=""button"" aria-label=""Close"" class=""ant-drawer-close"">
                                    <span role=""img"" class=""anticon anticon-close"" id:ignore>
                                        <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor""
                                            style=""pointer-events: none;"" xmlns=""http://www.w3.org/2000/svg"" class=""icon""
                                            viewBox=""0 0 1024 1024"">
                                            <path
                                                d=""M563.8 512l262.5-312.9c4.4-5.2.7-13.1-6.1-13.1h-79.8c-4.7 0-9.2 2.1-12.3 5.7L511.6 449.8 295.1 191.7c-3-3.6-7.5-5.7-12.3-5.7H203c-6.8 0-10.5 7.9-6.1 13.1L459.4 512 196.9 824.9A7.95 7.95 0 0 0 203 838h79.8c4.7 0 9.2-2.1 12.3-5.7l216.5-258.1 216.5 258.1c3 3.6 7.5 5.7 12.3 5.7h79.8c6.8 0 10.5-7.9 6.1-13.1L563.8 512z"">
                                            </path>
                                        </svg>
                                    </span>
                                </button>
                            </div>
                            <div class=""ant-drawer-body"">
                            </div>
                        </div>
                    </div>
                </div>
            </div>");
        }

        [Fact]
        public void ItShouldRenderTitle()
        {
            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters.Add(x => x.Title, "Test Title"));

            systemUnderTest.Find(".ant-drawer-title").MarkupMatches(@"<div class=""ant-drawer-title"">Test Title</div>");
        }

        [Fact]
        public void ItShouldRenderTitleRenderFragment()
        {
            RenderFragment fragment = builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(1, "Testing");
                builder.CloseElement();
            };

            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters.Add(x => x.Title, fragment));

            systemUnderTest.Find(".ant-drawer-title").MarkupMatches(@"<div class=""ant-drawer-title""><span>Testing</span></div>");
        }

        [Fact]
        public void ItShouldNotRenderMaskWhenTurnedOff()
        {
            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters.Add(x => x.Mask, false));

            systemUnderTest.FindAll(".ant-drawer-mask").Should().BeEmpty();
        }

        [Fact]
        public void ItShouldNotRenderCloseButtonWhenTurnedOff()
        {
            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters.Add(x => x.Closable, false));

            systemUnderTest.FindAll(".ant-drawer-close").Should().BeEmpty();
        }

        [Fact]
        public void ItShouldRenderContentString()
        {
            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters.Add(x => x.Content, "Test Llama"));

            systemUnderTest.Find(".ant-drawer-body").MarkupMatches(@"<div class=""ant-drawer-body"">Test Llama</div>");
        }

        [Fact]
        public void ItShouldRenderContentRenderFragment()
        {
            RenderFragment fragment = builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(1, "Testing");
                builder.CloseElement();
            };

            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters.Add(x => x.Content, fragment));

            systemUnderTest.Find(".ant-drawer-body").MarkupMatches(@"<div class=""ant-drawer-body"">
                <span>Testing</span>
            </div>");
        }

        [Fact]
        public void ItShouldRenderChildContent()
        {
            RenderFragment fragment = builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(1, "Testing");
                builder.CloseElement();
            };

            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters.Add(x => x.ChildContent, fragment));

            systemUnderTest.Find(".ant-drawer-body").MarkupMatches(@"<div class=""ant-drawer-body"">
                <span>Testing</span>
            </div>");
        }

        [Fact]
        public void ItShouldRenderChildContentAndContentParameterString()
        {
            RenderFragment fragment = builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(1, "Testing");
                builder.CloseElement();
            };

            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters
                .Add(x => x.ChildContent, fragment)
                .Add(x => x.Content, "First Content"));

            systemUnderTest.Find(".ant-drawer-body").MarkupMatches(@"<div class=""ant-drawer-body"">
                First Content
                <span>Testing</span>
            </div>");
        }

        [Fact]
        public void ItShouldRenderChildContentAndContentParameterRenderFragment()
        {
            RenderFragment childContent = builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(1, "Testing");
                builder.CloseElement();
            };

            RenderFragment fragment = builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(1, "First Content Woo!");
                builder.CloseElement();
            };

            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters
                .Add(x => x.ChildContent, childContent)
                .Add(x => x.Content, fragment));

            systemUnderTest.Find(".ant-drawer-body").MarkupMatches(@"<div class=""ant-drawer-body"">
                <span>First Content Woo!</span>
                <span>Testing</span>
            </div>");
        }

        [Fact]
        public void ItShouldRenderGivenZIndexWhenVisible()
        {
            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters
                .Add(x => x.ZIndex, 4000)
                .Add(x => x.Visible, true));

            systemUnderTest.Find(".ant-drawer").Attributes.Single(x => x.Name == "style").Value.Trim().Should().Contain("z-index: 4000;");
        }

        [Fact]
        public void ItShouldRenderZIndexBelowEverythingWhenNotVisible()
        {
            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters
                .Add(x => x.ZIndex, 4000)
                .Add(x => x.Visible, false));

            systemUnderTest.MarkupMatches(@"
                <div class=""ant-drawer ant-drawer-right"" style=""z-index:-9999;"" id:ignore diff:ignoreChildren></div>");
        }

        //[Theory]
        //[InlineData("right", "transition:transform 0.3s cubic-bezier(0.78, 0.14, 0.15, 0.86) 0s width 0s cubic-bezier(0.78, 0.14, 0.15, 0.86) 0.3s;")]
        //[InlineData("left", "transition:transform 0.3s cubic-bezier(0.78, 0.14, 0.15, 0.86) 0s width 0s cubic-bezier(0.78, 0.14, 0.15, 0.86) 0.3s;")]
        //[InlineData("top", "transition:transform 0.3s cubic-bezier(0.78, 0.14, 0.15, 0.86) 0s height 0s cubic-bezier(0.78, 0.14, 0.15, 0.86) 0.3s;")]
        //[InlineData("bottom", "transition:transform 0.3s cubic-bezier(0.78, 0.14, 0.15, 0.86) 0s height 0s cubic-bezier(0.78, 0.14, 0.15, 0.86) 0.3s;")]
        //public void ItShouldRenderProperAnimationStyleWhenVisible(string placement, string expectedStyle)
        //{
        //    var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters
        //        .Add(x => x.Placement, placement)
        //        .Add(x => x.Visible, true));

        //    var resultingStyle = systemUnderTest.Find(".ant-drawer").Attributes.Single(x => x.Name == "style").Value.Trim();
        //    resultingStyle.Should().Contain(expectedStyle);
        //}

        [Theory]
        [InlineData(DrawerPlacement.Right, 20, "transform: translateX(-20px)")]
        [InlineData(DrawerPlacement.Left, 25, "transform: translateX(25px)")]
        public void ItShouldRenderProperOffsetXStyleWhenVisible(DrawerPlacement placement, int offset, string expectedStyle)
        {
            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters
                .Add(x => x.Placement, placement)
                .Add(x => x.Visible, true)
                .Add(x => x.OffsetX, offset));

            var resultingStyle = systemUnderTest.Find(".ant-drawer").Attributes.Single(x => x.Name == "style").Value.Trim();
            resultingStyle.Should().Contain(expectedStyle);
        }

        [Theory]
        [InlineData(DrawerPlacement.Top, 20, "transform: translateY(20px)")]
        [InlineData(DrawerPlacement.Bottom, 25, "transform: translateY(-25px)")]
        public void ItShouldRenderProperOffsetYStyleWhenVisible(DrawerPlacement placement, int offset, string expectedStyle)
        {
            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters
                .Add(x => x.Placement, placement)
                .Add(x => x.Visible, true)
                .Add(x => x.OffsetY, offset));

            var resultingStyle = systemUnderTest.Find(".ant-drawer").Attributes.Single(x => x.Name == "style").Value.Trim();
            resultingStyle.Should().Contain(expectedStyle);
        }

        [Fact]
        public void ItShouldCallCloseWhenClickingMaskByDefault()
        {
            var closed = false;
            var onClose = () =>
            {
                closed = true;
            };
            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters
                .Add(x => x.OnClose, onClose)
                .Add(x => x.Visible, true)
                .Add(x => x.MaskClosable, true));

            systemUnderTest.InvokeAsync(() => systemUnderTest.Find(".ant-drawer-mask").TriggerEvent("onclick", new MouseEventArgs()));

            systemUnderTest.WaitForAssertion(() => closed.Should().BeTrue());
        }

        [Fact]
        public void ItShouldNotCallCloseWhenClickingMaskWhenCloseableDisabled()
        {
            var closed = false;
            var onClose = () =>
            {
                closed = true;
            };
            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters
                .Add(x => x.OnClose, onClose)
                .Add(x => x.Visible, true)
                .Add(x => x.MaskClosable, false));

            systemUnderTest.InvokeAsync(() => systemUnderTest.Find(".ant-drawer-mask").TriggerEvent("onclick", new MouseEventArgs()));

            systemUnderTest.WaitForAssertion(() => closed.Should().BeFalse());
        }

        [Fact]
        public void ItShouldNotCallCloseWhenClickingCloseButtonByDefault()
        {
            var closed = false;
            var onClose = () =>
            {
                closed = true;
            };
            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters
                .Add(x => x.OnClose, onClose));

            systemUnderTest.InvokeAsync(() => systemUnderTest.Find(".ant-drawer-close").TriggerEvent("onclick", new MouseEventArgs()));

            systemUnderTest.WaitForAssertion(() => closed.Should().BeTrue());
        }

        [Fact]
        public void ItShouldNotCallOnCloseWhenVisibleChangedToFalseAfterRender()
        {
            var closed = false;
            var onClose = () =>
            {
                closed = true;
            };
            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters
                .Add(x => x.Placement, DrawerPlacement.Right)
                .Add(x => x.Visible, true));

            systemUnderTest.SetParametersAndRender(parameters => parameters.Add(x => x.Visible, false));

            closed.Should().BeFalse();
        }

        [Fact]
        public void ItShouldCallOnOpenWhenVisibleChangedToTrueAfterRender()
        {
            var opened = false;
            var onOpen = () =>
            {
                opened = true;

                return Task.CompletedTask;
            };

            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters
                .Add(x => x.Placement, DrawerPlacement.Right)
                .Add(x => x.Visible, false)
                .Add(x => x.OnOpen, onOpen));

            systemUnderTest.SetParametersAndRender(parameters => parameters.Add(x => x.Visible, true));

            opened.Should().BeTrue();
        }

        [Fact]
        public void ItShouldCloseWhenVisibleChangedToFalseAfterRender()
        {
            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters
                .Add(x => x.Placement, DrawerPlacement.Right)
                .Add(x => x.Visible, true));

            systemUnderTest.SetParametersAndRender(parameters => parameters.Add(x => x.Visible, false));
        }

        [Fact]
        public void ItShouldRenderHandlerNextToContentWhenProvided()
        {
            RenderFragment fragment = builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(1, "Handler Stuff");
                builder.CloseElement();
            };

            var systemUnderTest = RenderComponent<AntDesign.Drawer>(parameters => parameters.Add(x => x.Handler, fragment));

            systemUnderTest.MarkupMatches(@"<div diff:ignoreAttributes>
                <div class=""ant-drawer-content-wrapper"" diff:ignoreAttributes>
                    <div class=""ant-drawer-content"" diff:ignoreChildren></div>
                    <span>Handler Stuff</span>
                </div>
            </div>");
        }
    }
}
