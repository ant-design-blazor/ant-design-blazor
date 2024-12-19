// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Bunit;
using Xunit;

namespace AntDesign.Tests.Button
{
    public partial class ButtonTests : AntDesignTestBase
    {
        [Fact]
        public void Renders_an_empty_button()
        {
            var cut = Context.RenderComponent<AntDesign.Button>();
            cut.MarkupMatches(@"
                <button class=""ant-btn ant-btn-default"" id:ignore type=""button"" ant-click-animating-without-extra-node=""false""></button>
            ");
        }

        [Fact]
        public void Renders_a_button_with_contents()
        {
            var cut = Context.RenderComponent<AntDesign.Button>(p =>
                p.AddChildContent("Save")
            );

            cut.MarkupMatches(@"
                <button class=""ant-btn ant-btn-default"" id:ignore type=""button"" ant-click-animating-without-extra-node=""false""><span>Save</span></button>
            ");
        }
        [Fact]
        public void Renders_a_button_with_contents_with_arialabel()
        {
            var cut = Context.RenderComponent<AntDesign.Button>(p =>
            {
                p.AddChildContent("Save");
                p.Add(x => x.AriaLabel, "Save");
            });

            cut.MarkupMatches(@"
                <button class=""ant-btn ant-btn-default"" id:ignore type=""button"" ant-click-animating-without-extra-node=""false"" aria-label=""Save""><span>Save</span></button>
            ");
        }

        [Fact]
        public void Renders_a_disabled_the_button()
        {
            var cut = Context.RenderComponent<AntDesign.Button>(p =>
                p.Add(x => x.Disabled, true)
            );

            cut.MarkupMatches(@"
                <button class=""ant-btn ant-btn-default"" id:ignore type=""button"" ant-click-animating-without-extra-node=""false"" disabled></button>
            ");
        }

        [Theory]
        [InlineData(ButtonType.Dashed)]
        [InlineData(ButtonType.Default)]
        [InlineData(ButtonType.Primary)]
        [InlineData(ButtonType.Link)]
        public void Renders_buttons_of_different_types(ButtonType type)
        {
            var cut = Context.RenderComponent<AntDesign.Button>(p =>
                p.Add(x => x.Type, type)
            );

            cut.MarkupMatches($@"
                <button class=""ant-btn ant-btn-{type.ToString().ToLower()}"" id:ignore type=""button"" ant-click-animating-without-extra-node=""false""></button>
            ");
        }

        [Fact]
        public void Should_fire_OnClick_when_clicked()
        {
            var clicked = false;

            var cut = Context.RenderComponent<AntDesign.Button>(p =>
                p.Add(x => x.OnClick, args => clicked = true)
            );

            var buttonEl = cut.Find("button");
            buttonEl.Click();

            Assert.True(clicked);
        }

        [Fact]
        public void Renders_loading_icon()
        {
            var cut = Context.RenderComponent<AntDesign.Button>(p =>
                p.Add(x => x.Loading, true)
            );

            cut.MarkupMatches(@"
                <button class=""ant-btn ant-btn-default ant-btn-loading"" id:ignore type=""button"" ant-click-animating-without-extra-node=""false"">
                  <span diff:ignore></span>
                </button>
            ");
        }


        [Fact]
        public void Renders_when_type_is_changed()
        {
            var cut = Context.RenderComponent<AntDesign.Button>(p =>
                p.Add(x => x.Type, ButtonType.Default)
            );

            cut.SetParametersAndRender(p =>
                p.Add(x => x.Type, ButtonType.Dashed)
            );

            cut.MarkupMatches($@"
                <button class=""ant-btn ant-btn-dashed"" id:ignore type=""button"" ant-click-animating-without-extra-node=""false""></button>
            ");
        }
    }
}
