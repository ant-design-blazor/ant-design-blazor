using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace AntDesign.Tests.Badge
{
    public class RibbonTests : AntDesignTestBase
    {
        [Fact(DisplayName = "Works with empty params")]
        public void TestRibbonEmpty()
        {
            var cut = Context.RenderComponent<BadgeRibbon>();
            cut.MarkupMatches(@"
                <div class=""ant-ribbon-wrapper""> 
                    <div class=""ant-ribbon ant-ribbon-placement-end"">
                    <span class=""ant-ribbon-text""></span>
                        <div class=""ant-ribbon-corner""></div>
                    </div>
                </div>
            ");
        }

        [Fact(DisplayName = "Works with `start` & `end` placement")]
        public void TestRibbonPlacement()
        {
            var cut = Context.RenderComponent<BadgeRibbon>(p =>
                {
                    p.Add(x => x.Placement, "start");
                    p.AddChildContent("<div />");
                }
            );
            cut.MarkupMatches(@"
                <div class=""ant-ribbon-wrapper""> 
                    <div /> 
                    <div class=""ant-ribbon ant-ribbon-placement-start"">
                    <span class=""ant-ribbon-text""></span>
                    <div class=""ant-ribbon-corner"">
                        </div>
                    </div>
                </div>
            ");

            cut = Context.RenderComponent<BadgeRibbon>(p =>
                {
                    p.Add(x => x.Placement, "end");
                    p.AddChildContent("<div />");
                }
            );
            cut.MarkupMatches(@"
                <div class=""ant-ribbon-wrapper""> 
                    <div /> 
                    <div class=""ant-ribbon ant-ribbon-placement-end"">
                    <span class=""ant-ribbon-text""></span>
                    <div class=""ant-ribbon-corner"">
                        </div>
                    </div>
                </div>
            ");
        }

        [Fact(DisplayName = "Works with preset color")]
        public void TestRibbonPresetColor()
        {
            var cut = Context.RenderComponent<BadgeRibbon>(p =>
                {
                    p.Add(x => x.Color, "green");
                    p.AddChildContent("<div />");
                }
            );
            cut.MarkupMatches(@"
                <div class=""ant-ribbon-wrapper""> 
                    <div /> 
                    <div class=""ant-ribbon ant-ribbon-placement-end ant-ribbon-color-green"">
                    <span class=""ant-ribbon-text""></span>
                    <div class=""ant-ribbon-corner"">
                        </div>
                    </div>
                </div>
            ");
        }

        [Fact(DisplayName = "Works with preset color")]
        public void TestRibbonCustomColor()
        {
            var color = "#888";

            var cut = Context.RenderComponent<BadgeRibbon>(p =>
                {
                    p.Add(x => x.Color, color);
                    p.AddChildContent("<div />");
                }
            );

            cut.MarkupMatches($@"
                <div class=""ant-ribbon-wrapper""> 
                    <div /> 
                    <div class=""ant-ribbon ant-ribbon-placement-end"" style=""background: {color}"">
                    <span class=""ant-ribbon-text""></span>
                    <div class=""ant-ribbon-corner"" style=""color: {color}"">
                        </div>
                    </div>
                </div>
            ");
        }

        [Fact(DisplayName = "Works with string")]
        public void TestRibbonText()
        {
            var cut = Context.RenderComponent<BadgeRibbon>(p =>
                {
                    p.Add(x => x.Text, "unicorn");
                    p.AddChildContent("<div />");
                }
            );

            cut.MarkupMatches(@"
                <div class=""ant-ribbon-wrapper""> 
                    <div /> 
                    <div class=""ant-ribbon ant-ribbon-placement-end"">
                    <span class=""ant-ribbon-text"">unicorn</span>                        
                        <div class=""ant-ribbon-corner"">
                            </div>
                        </div>
                </div>
            ");
        }

        [Fact(DisplayName = "Works with RenderFragment")]
        public void TestRibbonRenderFragment()
        {
            RenderFragment fragment = builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddAttribute(0, "class", "cool");
                builder.AddContent(0, "Hello");

                builder.CloseElement();
            };
            var cut = Context.RenderComponent<BadgeRibbon>(p =>
                {
                    p.Add(x => x.TextTemplate, fragment);
                    p.AddChildContent("<div />");
                }
            );

            cut.MarkupMatches(@"
                <div class=""ant-ribbon-wrapper""> 
                    <div /> 
                    <div class=""ant-ribbon ant-ribbon-placement-end"">
                        <span class=""ant-ribbon-text"">
                            <span class=""cool"">Hello</span>
                        </span>
                        <div class=""ant-ribbon-corner"">
                        </div>
                    </div>
                </div>
            ");
        }
    }
}
