using Bunit;
using Xunit;

namespace AntDesign.Tests.Button
{
    public class ButtonTests : AntDesignTestBase
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
                <button class=""ant-btn ant-btn-default"" id:ignore type=""button"" ant-click-animating-without-extra-node=""false"">Save</button>
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
        public void Renders_buttons_of_different_types(string type)
        {
            var cut = Context.RenderComponent<AntDesign.Button>(p =>
                p.Add(x => x.Type, type )
            );

            cut.MarkupMatches($@"
                <button class=""ant-btn ant-btn-{type.ToLower()}"" id:ignore type=""button"" ant-click-animating-without-extra-node=""false""></button>
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
    }
}
