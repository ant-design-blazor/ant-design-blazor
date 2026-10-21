// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Bunit;
using FluentAssertions;
using Xunit;

namespace AntDesign.Tests.Drawer
{
    public class DrawerSemanticTests : AntDesignTestBase
    {
        public DrawerSemanticTests()
        {
            JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        }

        [Fact]
        public void Applies_semantic_classes_and_styles_to_drawer_slots()
        {
            var classNames = new Dictionary<string, string>
            {
                ["root"] = "semantic-root",
                ["mask"] = "semantic-mask",
                ["wrapper"] = "semantic-wrapper",
                ["section"] = "semantic-section",
                ["header"] = "semantic-header",
                ["close"] = "semantic-close",
                ["title"] = "semantic-title",
                ["extra"] = "semantic-extra",
                ["body"] = "semantic-body",
                ["footer"] = "semantic-footer",
                ["dragger"] = "semantic-dragger"
            };
            var styles = new Dictionary<string, string>
            {
                ["root"] = "--semantic-root: 1;",
                ["mask"] = "--semantic-mask: 1;",
                ["wrapper"] = "--semantic-wrapper: 1;",
                ["section"] = "--semantic-section: 1;",
                ["header"] = "--semantic-header: 1;",
                ["close"] = "--semantic-close: 1;",
                ["title"] = "--semantic-title: 1;",
                ["extra"] = "--semantic-extra: 1;",
                ["body"] = "--semantic-body: 1;",
                ["footer"] = "--semantic-footer: 1;",
                ["dragger"] = "--semantic-dragger: 1;"
            };

            var drawer = RenderComponent<AntDesign.Drawer>(parameters => parameters
                .Add(x => x.Visible, true)
                .Add(x => x.Title, "Title")
                .Add(x => x.Extra, "Extra")
                .Add(x => x.Footer, "Footer")
                .Add(x => x.Resizable, true)
                .Add(x => x.ClassNames, classNames)
                .Add(x => x.Styles, styles));

            AssertSlot(drawer, ".ant-drawer", "semantic-root", "--semantic-root: 1");
            AssertSlot(drawer, ".ant-drawer-mask", "semantic-mask", "--semantic-mask: 1");
            AssertSlot(drawer, ".ant-drawer-content-wrapper", "semantic-wrapper", "--semantic-wrapper: 1");
            AssertSlot(drawer, ".ant-drawer-section", "semantic-section", "--semantic-section: 1");
            AssertSlot(drawer, ".ant-drawer-header", "semantic-header", "--semantic-header: 1");
            AssertSlot(drawer, ".ant-drawer-close", "semantic-close", "--semantic-close: 1");
            AssertSlot(drawer, ".ant-drawer-title", "semantic-title", "--semantic-title: 1");
            AssertSlot(drawer, ".ant-drawer-extra", "semantic-extra", "--semantic-extra: 1");
            AssertSlot(drawer, ".ant-drawer-body", "semantic-body", "--semantic-body: 1");
            AssertSlot(drawer, ".ant-drawer-footer", "semantic-footer", "--semantic-footer: 1");
            AssertSlot(drawer, ".ant-drawer-resizable-dragger", "semantic-dragger", "--semantic-dragger: 1");
        }

        [Fact]
        public void Supports_deprecated_content_slot_as_section_fallback()
        {
            var classNames = new Dictionary<string, string> { ["content"] = "legacy-content" };
            var styles = new Dictionary<string, string> { ["content"] = "--legacy-content: 1;" };

            var drawer = RenderComponent<AntDesign.Drawer>(parameters => parameters
                .Add(x => x.ClassNames, classNames)
                .Add(x => x.Styles, styles));

            AssertSlot(drawer, ".ant-drawer-section", "legacy-content", "--legacy-content: 1");
        }

        private static void AssertSlot(Bunit.IRenderedComponent<AntDesign.Drawer> drawer, string selector, string className, string style)
        {
            var element = drawer.Find(selector);
            element.ClassList.Should().Contain(className);
            element.GetAttribute("style").Should().Contain(style);
        }
    }
}
