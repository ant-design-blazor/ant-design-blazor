using System.Linq;
using AntDesign.Icons;
using Bunit;
using Xunit;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using AntDesign.Docs.Services;
using AntDesign.Extensions.Localization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using IconDemos = AntDesign.Docs.Demos.Components.Icon.demo;

namespace AntDesign.Tests.Docs;

public sealed class IconDemoParityTests : DocumentationTestBase
{
    [Theory]
    [InlineData("Icon", "scriptUrl", "components-icon-demo-scriptUrl")]
    [InlineData("Icon", "iconfont", "components-icon-demo-iconfont")]
    [InlineData("Select", "TrackItemChanges", "components-select-demo-TrackItemChanges")]
    public void Demo_and_contents_links_share_one_case_preserving_ID(string component, string demo, string expected)
        => Assert.Equal(expected, AntDesign.Docs.Shared.CodeBox.GetDemoId(component, demo));

    [Fact]
    public void Iconfont_demos_use_real_Space_items_brand_color_and_component_managed_resources()
    {
        JSInterop.SetupVoid(JSInteropConstants.CreateIconFromfontCN, _ => true).SetVoidResult();
        var single = RenderComponent<IconDemos.Iconfont>();
        var multiple = RenderComponent<IconDemos.ScriptUrl>();
        Assert.Equal(3, single.FindAll(".ant-space > .ant-space-item").Count);
        Assert.Equal(4, multiple.FindAll(".ant-space > .ant-space-item").Count);
        Assert.Empty(single.FindAll("script"));
        Assert.Empty(multiple.FindAll("script"));
        Assert.Equal("color: rgb(24, 119, 242);", single.FindAll(".anticon")[1].GetAttribute("style"));
        Assert.All(multiple.FindComponents<IconFont>(), font => Assert.Equal(2, font.Instance.ScriptUrl.AsT1.Length));
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("zh-CN")]
    public async Task Compiled_metadata_registers_the_extra_and_updated_sources_and_API(string language)
    {
        var locale = new Mock<ILocalizationService>();
        locale.SetupGet(x => x.CurrentCulture).Returns(CultureInfo.GetCultureInfo(language));
        var service = new DemoService(locale.Object, new HttpClient(), Services.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>());
        var page = await service.GetComponentAsync("components/icon");
        var extra = Assert.Single(page.DemoList.Where(d => d.Name == "two-tone-color"));
        Assert.Equal(typeof(IconDemos.TwoToneColor), service.GetShowCase(extra.Type));
        Assert.Contains("HomeOutlined", page.DemoList.Single(d => d.Name == "basic").Code);
        Assert.Contains("ComponentTemplate", page.ApiDoc);
        Assert.Contains("ScriptUrl", page.ApiDoc);
        Assert.Contains("AdditionalAttributes", page.Desc);
    }

    [Fact]
    public void Basic_uses_the_upstream_six_icons_in_Space_including_fill_spin_and_rotation()
    {
        var cut = RenderComponent<IconDemos.Basic>();
        var items = cut.FindAll(".ant-space > .ant-space-item");
        Assert.Equal(6, items.Count);
        Assert.Equal(new[] { "home", "setting", "smile", "sync", "smile", "loading" },
            items.Select(x => x.QuerySelector(".anticon")!.GetAttribute("aria-label")));
        Assert.Single(cut.FindComponents<SettingFilled>());
        Assert.NotNull(items[3].QuerySelector(".anticon-sync.anticon-spin"));
        Assert.Contains("rotate(180deg)", items[4].QuerySelector("svg")!.GetAttribute("style"));
        Assert.Null(items[4].QuerySelector(".anticon")!.GetAttribute("style"));
        Assert.NotNull(items[5].QuerySelector(".anticon-loading.anticon-spin"));
        Assert.Equal("64 64 896 896", items[0].QuerySelector("svg")!.GetAttribute("viewBox"));
    }

    [Fact]
    public void Two_tone_uses_normal_size_and_the_upstream_primary_and_secondary_colors()
    {
        var cut = RenderComponent<IconDemos.Twotone>();
        Assert.Equal(3, cut.FindAll(".ant-space > .ant-space-item").Count);
        Assert.Empty(cut.FindAll("button"));
        Assert.All(cut.FindAll("svg"), svg => Assert.Equal("1em", svg.GetAttribute("width")));
        Assert.Equal(new[] { "#1677ff", "#e6f4ff", "#1677ff" },
            cut.FindAll(".anticon-smile path").Select(x => x.GetAttribute("fill")));
        Assert.Equal(new[] { "#eb2f96", "#fff0f6" },
            cut.FindAll(".anticon-heart path").Select(x => x.GetAttribute("fill")));
        Assert.Equal(new[] { "#52c41a", "#f6ffed", "#52c41a" },
            cut.FindAll(".anticon-check-circle path").Select(x => x.GetAttribute("fill")));
    }

    [Fact]
    public void Custom_has_accessible_custom_SVGs_and_both_wrapped_and_direct_Home_icons()
    {
        var cut = RenderComponent<IconDemos.Custom>();
        var items = cut.FindAll(".ant-space > .ant-space-item");
        Assert.Equal(4, items.Count);
        Assert.Equal("heart icon", items[0].QuerySelector("title")!.TextContent);
        Assert.Equal("Panda icon", items[1].QuerySelector("title")!.TextContent);
        Assert.NotNull(items[2].QuerySelector(".anticon > .anticon-home"));
        Assert.Equal("true", items[2].QuerySelector(".anticon > .anticon-home")!.GetAttribute("aria-hidden"));
        Assert.Equal("1em", items[2].QuerySelector(".anticon > .anticon-home")!.GetAttribute("width"));
        Assert.NotNull(items[3].QuerySelector(".anticon-home"));
        Assert.Empty(cut.FindAll("[className],[p-id]"));
    }

    [Fact]
    public void Blazor_extra_retains_dynamic_color_changes_without_changing_the_React_demo()
    {
        var cut = RenderComponent<IconDemos.TwoToneColor>();
        var initial = cut.Find(".anticon-check-circle").InnerHtml;
        cut.Find("button").Click();
        Assert.NotEqual(initial, cut.Find(".anticon-check-circle").InnerHtml);
        Assert.Contains("#f759ab", cut.Find(".anticon-check-circle").InnerHtml);
        var icons = cut.FindAll(".anticon-check-circle");
        Assert.Equal(2, icons.Count);
        Assert.All(icons, icon =>
        {
            Assert.Contains("#f759ab", icon.InnerHtml);
            Assert.Contains("#fff0f5", icon.InnerHtml);
        });
    }
}
