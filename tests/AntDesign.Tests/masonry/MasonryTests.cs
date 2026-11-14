// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using AntDesign;
using AntDesign.Core.JsInterop.ObservableApi;
using Bunit;
using Microsoft.AspNetCore.Components;
using Moq;
using Xunit;

namespace AntDesign.Tests.Masonry
{
    public class MasonryTests : AntDesignTestBase
    {
        public MasonryTests()
        {
            JSInterop
                .Setup<AntDesign.JsInterop.DomRect[]>(JSInteropConstants.GetBoundingClientRects, _ => true)
                .SetResult(new[]
                {
                    new AntDesign.JsInterop.DomRect { Height = 100 },
                    new AntDesign.JsInterop.DomRect { Height = 100 },
                    new AntDesign.JsInterop.DomRect { Height = 100 },
                });
        }

        [Fact]
        public void RendersReactCompatibleMasonryStructure()
        {
            var items = new List<MasonryItem<int>>
            {
                new() { Key = "first", Data = 1 },
                new() { Key = "second", Data = 2 },
                new() { Key = "third", Data = 3 },
            };

            var cut = RenderComponent<AntDesign.Masonry<int>>(parameters => parameters
                .Add(x => x.Columns, 3)
                .Add(x => x.Gutter, 16)
                .Add(x => x.Items, items)
                .Add(x => x.ItemRender, item => builder => builder.AddContent(0, item.Data)));

            var root = cut.Find(".ant-masonry");
            Assert.Equal(3, root.Children.Length);
            Assert.All(root.Children, item => Assert.Equal("ant-masonry-item", item.ClassName));
            Assert.Contains("position: absolute", root.Children[0].GetAttribute("style"));
            Assert.Contains("inset-inline-start", root.Children[1].GetAttribute("style"));
            MockedDomEventListener.Verify(listener => listener.AddResizeObserver(
                It.IsAny<ElementReference>(),
                It.IsAny<Action<List<ResizeObserverEntry>>>()), Times.Once);
        }

        [Fact]
        public void DoesNotEnableResponsiveInteropByDefault()
        {
            var items = new[] { new MasonryItem<int> { Key = "item", Data = 1 } };

            RenderComponent<AntDesign.Masonry<int>>(parameters => parameters
                .Add(x => x.Items, items)
                .Add(x => x.ItemRender, item => builder => builder.AddContent(0, item.Data)));

            MockedDomEventListener.Verify(listener => listener.AddResizeObserver(
                It.IsAny<ElementReference>(),
                It.IsAny<Action<List<ResizeObserverEntry>>>()), Times.Never);
        }

        [Fact]
        public void ItemChildContentTakesPrecedenceOverItemRender()
        {
            var items = new[]
            {
                new MasonryItem<int>
                {
                    Key = "item",
                    Data = 1,
                    ChildContent = builder => builder.AddContent(0, "child"),
                },
            };

            var cut = RenderComponent<AntDesign.Masonry<int>>(parameters => parameters
                .Add(x => x.Items, items)
                .Add(x => x.ItemRender, _ => builder => builder.AddContent(0, "render")));

            Assert.Equal("child", cut.Find(".ant-masonry-item").TextContent.Trim());
        }

        [Fact]
        public void RemovesItemStateAndObserverWhenItemViewIsRemoved()
        {
            var first = new MasonryItem<int> { Key = "first", Data = 1 };
            var second = new MasonryItem<int> { Key = "second", Data = 2 };
            var items = new List<MasonryItem<int>> { first, second };

            var cut = RenderComponent<AntDesign.Masonry<int>>(parameters => parameters
                .Add(x => x.Columns, 2)
                .Add(x => x.Items, items)
                .Add(x => x.Fresh, true)
                .Add(x => x.ItemRender, item => builder => builder.AddContent(0, item.Data)));

            MockedDomEventListener.Invocations.Clear();
            items = new List<MasonryItem<int>> { second };
            cut.SetParametersAndRender(parameters => parameters.Add(x => x.Items, items));

            MockedDomEventListener.Verify(listener => listener.DisposeResizeObserver(
                It.IsAny<ElementReference>()), Times.Once);
            Assert.Single(cut.FindAll(".ant-masonry-item"));
        }
    }
}
