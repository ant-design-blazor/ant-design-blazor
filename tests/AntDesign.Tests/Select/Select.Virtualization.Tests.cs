// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Core.JsInterop.Modules.Components;
using AntDesign.Select.Internal;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Xunit;

namespace AntDesign.Tests.Select
{
    public class SelectVirtualizationTests : AntDesignTestBase
    {
        public SelectVirtualizationTests()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            JSInterop.Setup<AntDesign.JsInterop.DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
                .SetResult(new AntDesign.JsInterop.DomRect());
            JSInterop.Setup<OverlayPosition>("AntDesign.interop.overlayHelper.addOverlayToContainer", _ => true)
                .SetResult(new OverlayPosition());
        }

        [Theory]
        [InlineData(SelectMode.Default, "Material 1998")]
        [InlineData(SelectMode.Multiple, "Material 1998")]
        [InlineData(SelectMode.Default, "中文材料")]
        public async Task SearchUpdatesVirtualizedItems(SelectMode mode, string search)
        {
            var items = Enumerable.Range(1, 2000)
                .Select(i => i == 1999 ? "中文材料 1999" : $"Material {i:D4}").ToArray();
            var cut = RenderComponent<AntDesign.Select<string, string>>(parameters => parameters
                .Add(p => p.DataSource, items)
                .Add(p => p.EnableSearch, true)
                .Add(p => p.EnableVirtualization, true)
                .Add(p => p.SearchDebounceMilliseconds, 0)
                .Add(p => p.Mode, mode));

            Virtualize<SelectOptionItem<string, string>> Virtualizer() =>
                cut.FindComponent<Virtualize<SelectOptionItem<string, string>>>().Instance;

            Virtualizer().Items.Should().HaveCount(2000);
            await cut.Find("input[type=search]").InputAsync(new ChangeEventArgs { Value = search });
            cut.WaitForAssertion(() => Virtualizer().Items!.Select(x => x.Label)
                .Should().Equal(items.Where(x => x.Contains(search, StringComparison.InvariantCultureIgnoreCase))));

            await cut.Find("input[type=search]").InputAsync(new ChangeEventArgs { Value = "no match" });
            cut.WaitForAssertion(() => cut.FindAll(".ant-select-item-empty").Should().ContainSingle());

            await cut.Find("input[type=search]").InputAsync(new ChangeEventArgs { Value = "" });
            cut.WaitForAssertion(() => Virtualizer().Items.Should().HaveCount(2000));
        }
    }
}
