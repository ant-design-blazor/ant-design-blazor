// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using AntDesign.JsInterop;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using Xunit;

namespace AntDesign.Tests.Tabs
{
    public partial class TabsTests : AntDesignTestBase
    {
        private IRenderedComponent<AntDesign.Tabs> CreateTabs(Action<RenderTreeBuilder> childContent)
        {
            var jsRuntime = new Mock<IJSRuntime>();
            jsRuntime.Setup(u => u.InvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, It.IsAny<object[]>()))
                .ReturnsAsync(new HtmlElement());

            Context.Services.AddScoped(_ => jsRuntime.Object);

            var cut = Context.RenderComponent<AntDesign.Tabs>(tabs => tabs
                .Add(x => x.DefaultActiveKey, "2")
                .Add(b => b.ChildContent, b => childContent(b))
            );

            return cut;
        }

        private static RenderFragment CreateTabPanel(string key, Action<ComponentParameterCollectionBuilder<TabPane>>? configure = null)
        {
            var tabPane1Builder = new ComponentParameterCollectionBuilder<TabPane>()
                .Add(x => x.Key, key)
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
    }
}
