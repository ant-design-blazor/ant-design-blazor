// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Bunit;
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
        private IRenderedComponent<AntDesign.Tabs> CreateTabs(Action<RenderTreeBuilder> childContent, bool draggable = false)
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
                .Add(x => x.Draggable, draggable)
                .Add(b => b.ChildContent, b => childContent(b))
            );

            return cut;
        }

        [Fact]
        public async Task Dragging_a_tab_keeps_its_panel_content_attached_to_its_key()
        {
            var cut = CreateTabs(p =>
            {
                CreateTabPanel("1")(p);
                CreateTabPanel("2")(p);
                CreateTabPanel("3")(p);
            }, draggable: true);

            cut.FindAll(".ant-tabs-tab").Single(x => x.TextContent.Trim() == "Tab 3").Click();
            var panes = cut.FindComponents<TabPane>();
            var source = panes.Single(x => x.Instance.Key == "3").Instance;
            var target = panes.Single(x => x.Instance.Key == "1").Instance;
            await cut.InvokeAsync(() =>
            {
                cut.Instance.HandleDragStart(new DragEventArgs { DataTransfer = new DataTransfer() }, source);
                cut.Instance.HandleDrop(target);
            });

            var activePane = cut.Find(".ant-tabs-tabpane-active");
            Assert.Equal("rc-tabs-3-panel-3", activePane.Id);
            Assert.Equal("Content 3", activePane.TextContent.Trim());

            cut.FindAll(".ant-tabs-tab").Single(x => x.TextContent.Trim() == "Tab 1").Click();

            activePane = cut.Find(".ant-tabs-tabpane-active");
            Assert.Equal("rc-tabs-1-panel-1", activePane.Id);
            Assert.Equal("Content 1", activePane.TextContent.Trim());
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
    }
}
