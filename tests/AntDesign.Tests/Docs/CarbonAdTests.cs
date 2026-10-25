using System.Threading.Tasks;
using AntDesign.Docs.Shared;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class CarbonAdTests : DocumentationTestBase
{
    [Fact]
    public void Component_overview_keeps_ad_outside_title_and_metadata_with_one_trailing_divider()
    {
        var module = JSInterop.SetupModule("./_content/AntDesign.Docs/js/carbon-ad.js");
        module.SetupVoid("mount", _ => true).SetVoidResult();
        var cut = RenderComponent<ComponentOverview>(p => p.AddChildContent("<h1>Select</h1><section class=docs-component-meta>Metadata</section>"));
        Assert.NotNull(cut.Find(".docs-component-overview-layout > .docs-component-overview-content > h1"));
        Assert.NotNull(cut.Find(".docs-component-overview-layout > .docs-component-overview-ad"));
        Assert.Empty(cut.FindAll("h1 .docs-carbon-ad,.docs-component-meta .docs-carbon-ad"));
        Assert.Single(cut.FindAll(".ant-divider"));
        Assert.Single(module.Invocations["mount"]);
    }

    [Fact]
    public void Ad_mounts_only_once_and_never_renders_a_vendor_script_as_Razor_markup()
    {
        var module = JSInterop.SetupModule("./_content/AntDesign.Docs/js/carbon-ad.js");
        module.SetupVoid("mount", _ => true).SetVoidResult();
        var cut = RenderComponent<CarbonAd>(p => p.Add(x => x.Class, "page-ad"));
        cut.Render();
        cut.Render();
        Assert.NotNull(cut.Find(".docs-carbon-ad.page-ad"));
        Assert.Empty(cut.FindAll("script,#carbonads"));
        Assert.Single(module.Invocations["mount"]);
    }

    [Fact]
    public async Task Ad_releases_its_host_only_once_on_disposal()
    {
        var module = JSInterop.SetupModule("./_content/AntDesign.Docs/js/carbon-ad.js");
        module.SetupVoid("mount", _ => true).SetVoidResult();
        module.SetupVoid("unmount", _ => true).SetVoidResult();
        var cut = RenderComponent<CarbonAd>();
        await cut.Instance.DisposeAsync();
        await cut.Instance.DisposeAsync();
        Assert.Single(module.Invocations["unmount"]);
    }
}
