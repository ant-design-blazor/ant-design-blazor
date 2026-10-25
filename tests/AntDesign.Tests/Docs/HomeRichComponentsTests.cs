using System.Linq;
using AntDesign.Docs.Components;
using AntDesign.JsInterop;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class HomeRichComponentsTests : DocumentationTestBase
{
    public HomeRichComponentsTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
            .SetResult(new DomRect { Width = 358, Height = 395 });
        JSInterop.Setup<HtmlElement>(JSInteropConstants.GetFirstChildDomInfo, _ => true)
            .SetResult(new HtmlElement { ClientWidth = 358, ClientHeight = 395 });
        JSInterop.Setup<System.Text.Json.JsonElement>(JSInteropConstants.GetScroll, _ => true)
            .SetResult(System.Text.Json.JsonDocument.Parse("{\"x\":0,\"y\":0}").RootElement.Clone());
        JSInterop.Setup<Window>(JSInteropConstants.GetWindow, _ => true).SetResult(new Window { InnerWidth = 1440, InnerHeight = 1000 });
    }

    [Fact]
    public void Desktop_contains_the_five_real_components_and_the_inline_tour_is_interactive()
    {
        var cut = Context.RenderComponent<HomeRichComponents>();
        Assert.Contains("@media(width < 768px)", cut.Find("style").TextContent);
        Assert.Contains("[data-theme=dark] .home-rich-card", cut.Find("style").TextContent);
        Assert.DoesNotContain("100vw", cut.Find("style").TextContent);
        Assert.Equal(new[] { "DatePicker", "Tour", "FloatButton", "Splitter", "Masonry" },
            cut.FindAll(".home-rich-card").Select(card => card.GetAttribute("data-component")));
        Assert.NotNull(cut.Find(".ant-picker-dropdown-pure .ant-picker-date-panel"));
        Assert.Equal(4, cut.FindAll(".ant-picker-presets li").Count);
        Assert.NotEmpty(cut.FindAll(".ant-tour-indicator"));
        Assert.NotNull(cut.Find(".ant-splitter-vertical"));
        Assert.Equal(9, cut.FindAll(".ant-masonry-item").Count);
        var tour = cut.FindComponent<TourPanel>();
        Assert.Empty(tour.Find(".ant-tour-indicators").ChildNodes.Where(node => node.NodeType == AngleSharp.Dom.NodeType.Text));
        Assert.Empty(tour.Find(".ant-tour-actions").ChildNodes.Where(node => node.NodeType == AngleSharp.Dom.NodeType.Text && node.TextContent.Length > 0));
        tour.Find(".ant-tour-next-btn").Click();
        Assert.Equal(4, tour.Instance.Current);
        tour.Find(".ant-tour-prev-btn").Click();
        Assert.Equal(3, tour.Instance.Current);
        tour.Find(".ant-tour-close").Click();
        Assert.Empty(cut.FindAll(".ant-tour-pure"));
        Assert.DoesNotContain(JSInterop.Invocations, call => call.Identifier == JSInteropConstants.DisableBodyScroll);
    }

    [Fact]
    public void Mobile_uses_the_real_five_slide_carousel_and_omits_date_presets()
    {
        JSInterop.Setup<Window>(JSInteropConstants.GetWindow, _ => true).SetResult(new Window { InnerWidth = 390, InnerHeight = 844 });
        var cut = Context.RenderComponent<HomeRichComponents>();
        cut.WaitForAssertion(() => Assert.NotNull(cut.Find(".home-rich-mobile .ant-carousel")));
        Assert.Equal(5, cut.FindAll(".slick-dots li").Count);
        Assert.Empty(cut.FindAll(".ant-picker-presets"));
        Assert.Equal(5, cut.FindComponent<Carousel>().FindComponents<CarouselSlick>().Count);
    }

    [Fact]
    public void Inline_date_panel_updates_selection_without_a_trigger_or_portal()
    {
        var selected = new System.DateTime?(new System.DateTime(2025, 11, 22));
        var cut = RenderComponent<DatePickerPanel<System.DateTime?>>(parameters => parameters
            .Add(x => x.Value, selected).Add(x => x.ShowToday, false).Add(x => x.ShowInput, true)
            .Add(x => x.ValueChanged, value => selected = value));
        cut.FindAll(".ant-picker-cell-in-view").Single(cell => cell.TextContent.Trim() == "23").Click();
        Assert.Equal(23, selected?.Day);
        Assert.Equal("2025-11-23", cut.Find("input[readonly]").GetAttribute("value"));
        Assert.Contains("inset:auto", cut.Find(".ant-picker-dropdown-pure").GetAttribute("style"));
        Assert.Empty(cut.FindAll(".ant-picker-today-btn"));
        Assert.DoesNotContain(JSInterop.Invocations, call => call.Identifier == JSInteropConstants.OverlayComponentHelper.AddOverlayToContainer);
    }
}
