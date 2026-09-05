// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using Xunit;

namespace AntDesign.Tests.Tabs
{
    public partial class TabsTests : AntDesignTestBase
    {
        public TabsTests()
        {
            JSInterop.Setup<Dictionary<string, HtmlElement>>(JSInteropConstants.GetElementsDomInfo, _ => true)
                .SetResult(new Dictionary<string, HtmlElement>());
            JSInterop.SetupVoid(JSInteropConstants.StyleHelper.AddClsToFirstChild, _ => true).SetVoidResult();
            JSInterop.SetupVoid(JSInteropConstants.StyleHelper.AddCls, _ => true).SetVoidResult();
            JSInterop.SetupVoid("AntDesign.interop.touchHelper.initializeTouch", _ => true).SetVoidResult();
            JSInterop.SetupVoid("AntDesign.interop.touchHelper.dispose", _ => true).SetVoidResult();
        }

        private IRenderedComponent<AntDesign.Tabs> CreateTabs(Action<RenderTreeBuilder> childContent)
        {
            var jsRuntime = new Mock<IJSRuntime>();
            jsRuntime.Setup(u => u.InvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, It.IsAny<object[]>()))
                .ReturnsAsync(new HtmlElement());
            jsRuntime.Setup(u => u.InvokeAsync<Dictionary<string, HtmlElement>>(JSInteropConstants.GetElementsDomInfo, It.IsAny<object[]>()))
                .ReturnsAsync(new Dictionary<string, HtmlElement>()
                {
                    ["1-nav-list"] = new HtmlElement() { ClientWidth = 45, ClientHeight = 45, },
                    ["1-nav-wrapper"] = new HtmlElement() { ClientWidth = 45, ClientHeight = 45, },
                    ["rc-tabs-2-tab-2"] = new HtmlElement() { ClientWidth = 45, ClientHeight = 45, },
                });

            Context.Services.AddScoped(_ => jsRuntime.Object);

            var cut = Context.RenderComponent<AntDesign.Tabs>(tabs => tabs
                .Add(x => x.DefaultActiveKey, "2")
                .Add(x => x.Id, "1")
                .Add(b => b.ChildContent, b => childContent(b))
            );

            return cut;
        }

        private static RenderFragment CreateTabPanel(string key, Action<ComponentParameterCollectionBuilder<TabPane>>? configure = null)
        {
            var tabPane1Builder = new ComponentParameterCollectionBuilder<TabPane>()
                .Add(x => x.Key, key)
                .Add(x => x.Id, key)
                .Add(x => x.Tab, $"Tab {key}")
                .Add(x => x.ChildContent, $"Content {key}".ToRenderFragment());

            configure?.Invoke(tabPane1Builder);

            return tabPane1Builder.Build().ToRenderFragment<TabPane>();
        }

        [Fact]
        public void Should_render_the_all_panes_when_ForceRender_is_true()
        {
            var cut = CreateTabs(p =>
            {
                var tabPane1 = CreateTabPanel("1", x => x.Add(y => y.ForceRender, true));
                var tabPane2 = CreateTabPanel("2", x => x.Add(y => y.ForceRender, true));

                tabPane1(p);
                tabPane2(p);
            });

            var renderedPanes = cut.FindAll(".ant-tabs-tabpane");

            Assert.Equal(2, renderedPanes.Count);
        }

        [Fact]
        public void Should_handle_missing_dictionary_keys_gracefully()
        {
            // Arrange: Setup JS runtime to return incomplete dictionary (missing required keys)
            var jsRuntime = new Mock<IJSRuntime>();
            jsRuntime.Setup(u => u.InvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, It.IsAny<object[]>()))
                .ReturnsAsync(new HtmlElement());

            // Return dictionary without the required nav-list and nav-wrapper keys
            jsRuntime.Setup(u => u.InvokeAsync<Dictionary<string, HtmlElement>>(JSInteropConstants.GetElementsDomInfo, It.IsAny<object[]>()))
                .ReturnsAsync(new Dictionary<string, HtmlElement>()
                {
                    ["some-other-key"] = new HtmlElement() { ClientWidth = 45, ClientHeight = 45, },
                    // Missing: "1-nav-list" and "1-nav-wrapper" keys
                });

            Context.Services.AddScoped(_ => jsRuntime.Object);

            // Act: This should not throw an exception even with missing keys
            var cut = Context.RenderComponent<AntDesign.Tabs>(tabs => tabs
                .Add(x => x.DefaultActiveKey, "2")
                .Add(x => x.Id, "1")
                .Add(b => b.ChildContent, b =>
                {
                    var tabPane1 = CreateTabPanel("1");
                    var tabPane2 = CreateTabPanel("2");
                    tabPane1(b);
                    tabPane2(b);
                })
            );

            // Assert: Component should render successfully without throwing
            Assert.NotNull(cut);
            var tabsElement = cut.Find(".ant-tabs");
            Assert.NotNull(tabsElement);

            // Verify that tabs are still rendered despite missing dictionary keys
            var tabButtons = cut.FindAll(".ant-tabs-tab");
            Assert.Equal(2, tabButtons.Count);
        }

        [Fact]
        public void Should_handle_null_dictionary_gracefully()
        {
            // Arrange: Setup JS runtime to return null dictionary
            var jsRuntime = new Mock<IJSRuntime>();
            jsRuntime.Setup(u => u.InvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, It.IsAny<object[]>()))
                .ReturnsAsync(new HtmlElement());

            // Return null dictionary
            jsRuntime.Setup(u => u.InvokeAsync<Dictionary<string, HtmlElement>>(JSInteropConstants.GetElementsDomInfo, It.IsAny<object[]>()))
                .ReturnsAsync((Dictionary<string, HtmlElement>)null);

            Context.Services.AddScoped(_ => jsRuntime.Object);

            // Act: This should not throw an exception even with null dictionary
            var cut = Context.RenderComponent<AntDesign.Tabs>(tabs => tabs
                .Add(x => x.DefaultActiveKey, "2")
                .Add(x => x.Id, "1")
                .Add(b => b.ChildContent, b =>
                {
                    var tabPane1 = CreateTabPanel("1");
                    var tabPane2 = CreateTabPanel("2");
                    tabPane1(b);
                    tabPane2(b);
                })
            );

            // Assert: Component should render successfully without throwing
            Assert.NotNull(cut);
            var tabsElement = cut.Find(".ant-tabs");
            Assert.NotNull(tabsElement);
        }

        [Fact]
        public void Should_support_go_next_previous_and_keyboard_navigation_with_clamping()
        {
            var activeKey = "2";
            var cut = Context.RenderComponent<AntDesign.Tabs>(tabs => tabs
                .Add(x => x.ActiveKey, activeKey)
                .Add(x => x.ActiveKeyChanged, EventCallback.Factory.Create<string>(this, value => activeKey = value))
                .Add(x => x.Id, "nav-tabs")
                .Add(x => x.ChildContent, builder =>
                {
                    builder.AddContent(0, CreateTabPanel("1"));
                    builder.AddContent(1, CreateTabPanel("2"));
                    builder.AddContent(2, CreateTabPanel("3"));
                }));

            cut.InvokeAsync(() => cut.Instance.GoTo(-10));
            activeKey.Should().Be("1");

            cut.InvokeAsync(() => cut.Instance.Next());
            activeKey.Should().Be("2");

            cut.InvokeAsync(() => cut.Instance.GoTo(99));
            activeKey.Should().Be("3");

            cut.InvokeAsync(() => cut.Instance.Previous());
            activeKey.Should().Be("2");

            var tabPanes = cut.FindComponents<TabPane>();
            cut.InvokeAsync(() => cut.Instance.HandleKeydown(new KeyboardEventArgs { Code = "ArrowRight" }, tabPanes[1].Instance));
            activeKey.Should().Be("3");

            cut.InvokeAsync(() => cut.Instance.HandleKeydown(new KeyboardEventArgs { Code = "ArrowLeft" }, tabPanes[2].Instance));
            activeKey.Should().Be("2");
        }

        [Fact]
        public void Should_handle_swipe_directions_for_horizontal_and_vertical_tabs()
        {
            var activeKey = "1";
            var cut = Context.RenderComponent<AntDesign.Tabs>(tabs => tabs
                .Add(x => x.ActiveKey, activeKey)
                .Add(x => x.ActiveKeyChanged, EventCallback.Factory.Create<string>(this, value => activeKey = value))
                .Add(x => x.Id, "swipe-tabs")
                .Add(x => x.EnableSwipe, true)
                .Add(x => x.TabPosition, TabPosition.Left)
                .Add(x => x.ChildContent, builder =>
                {
                    builder.AddContent(0, CreateTabPanel("1"));
                    builder.AddContent(1, CreateTabPanel("2"));
                    builder.AddContent(2, CreateTabPanel("3"));
                }));

            cut.InvokeAsync(() => cut.Instance.HandleSwipe("left"));
            activeKey.Should().Be("1");

            cut.InvokeAsync(() => cut.Instance.HandleSwipe("up"));
            activeKey.Should().Be("2");

            cut.InvokeAsync(() => cut.Instance.HandleSwipe("down"));
            activeKey.Should().Be("1");

            cut.SetParametersAndRender(parameters => parameters
                .Add(x => x.EnableSwipe, true)
                .Add(x => x.TabPosition, TabPosition.Top));

            cut.InvokeAsync(() => cut.Instance.HandleSwipe("right"));
            activeKey.Should().Be("1");

            cut.InvokeAsync(() => cut.Instance.HandleSwipe("left"));
            activeKey.Should().Be("2");
        }

        [Fact]
        public async Task Should_ignore_disabled_tabs_and_dispose_cleanly()
        {
            var activeKey = "1";
            var cut = Context.RenderComponent<AntDesign.Tabs>(tabs => tabs
                .Add(x => x.ActiveKey, activeKey)
                .Add(x => x.ActiveKeyChanged, EventCallback.Factory.Create<string>(this, value => activeKey = value))
                .Add(x => x.Id, "dispose-tabs")
                .Add(x => x.ChildContent, builder =>
                {
                    builder.AddContent(0, CreateTabPanel("1"));
                    builder.AddContent(1, CreateTabPanel("2", x => x.Add(y => y.Disabled, true)));
                    builder.AddContent(2, CreateTabPanel("3"));
                }));

            cut.Instance.ActivatePane("2");
            activeKey.Should().Be("1");

            await cut.Instance.DisposeAsync();
            Assert.True(true);
        }
    }
}
