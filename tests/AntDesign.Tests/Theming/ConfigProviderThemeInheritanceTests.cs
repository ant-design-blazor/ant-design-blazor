using System.Linq;
using Bunit;
using AntDesign.Theming;
using Xunit;

namespace AntDesign.Tests.Theming;

public sealed class ConfigProviderThemeInheritanceTests : AntDesignTestBase
{
    public ConfigProviderThemeInheritanceTests() => JSInterop.Mode = JSRuntimeMode.Loose;

    [Fact]
    public void Nested_dark_algorithm_does_not_inherit_derived_light_neutrals()
    {
        var cut = RenderComponent<ConfigProvider>(parameters => parameters
            .AddChildContent<ConfigProvider>(child => child
                .Add(x => x.ThemeConfig, new ThemeConfig { Algorithm = "dark" })
                .AddChildContent<AntDesign.Button>(button => button.AddChildContent("Dark"))));
        var style = cut.FindAll(".ant-theme")[1].GetAttribute("style");
        Assert.Contains("--ant-color-bg-container:#141414;", style);
        Assert.Contains("--ant-color-text:rgba(255, 255, 255, 0.85);", style);
        Assert.Contains("--ant-color-primary:#1668dc;", style);
        Assert.Contains("rgba(255, 255, 255, 0.016)", style);
    }

    [Fact]
    public void Three_levels_inherit_explicit_seed_alias_and_algorithm_without_rederiving_the_parent_palette()
    {
        var parentTheme = new ThemeConfig { Algorithm = "dark" };
        parentTheme.Token["colorPrimary"] = "#722ed1";
        parentTheme.Token["colorTextHeading"] = "#abcdef";
        var cut = RenderComponent<ConfigProvider>(parameters => parameters
            .Add(x => x.ThemeConfig, parentTheme)
            .AddChildContent<ConfigProvider>(child => child
                .AddChildContent<ConfigProvider>(grandchild => grandchild
                    .AddChildContent<AntDesign.Button>(button => button.AddChildContent("Inherited")))));

        var providers = cut.FindComponents<ConfigProvider>();
        Assert.Equal(2, providers.Count);
        Assert.All(providers, provider => {
            Assert.Equal("dark", provider.Instance.ResolvedAlgorithm);
            var style = provider.Find(".ant-theme").GetAttribute("style");
            Assert.Contains("--ant-color-primary:#642ab5;", style);
            Assert.Contains("--ant-color-bg-container:#141414;", style);
            Assert.Contains("--ant-color-text-heading:#abcdef;", style);
        });
    }

    [Fact]
    public void Explicit_child_default_algorithm_replaces_dark_while_retaining_the_parent_brand_seed()
    {
        var parent = new ThemeConfig { Algorithm = "dark" };
        parent.Token["colorPrimary"] = "#722ed1";
        var cut = RenderComponent<ConfigProvider>(parameters => parameters
            .Add(x => x.ThemeConfig, parent)
            .AddChildContent<ConfigProvider>(child => child.Add(x => x.ThemeConfig, new ThemeConfig { Algorithm = "default" })));
        var inner = cut.FindComponents<ConfigProvider>().Single();
        Assert.Equal("default", inner.Instance.ResolvedAlgorithm);
        Assert.Contains("--ant-color-primary:#722ed1;", inner.Find(".ant-theme").GetAttribute("style"));
        Assert.Contains("--ant-color-bg-container:#ffffff;", inner.Find(".ant-theme").GetAttribute("style"));
    }

    [Fact]
    public void Inherit_false_starts_a_fresh_default_theme_and_drops_parent_components()
    {
        var parent = new ThemeConfig { Algorithm = "dark" };
        parent.Token["colorPrimary"] = "#722ed1";
        parent.Components["Button"] = new System.Collections.Generic.Dictionary<string, string> { ["primaryColor"] = "#abcdef" };
        var cut = RenderComponent<ConfigProvider>(parameters => parameters
            .Add(x => x.ThemeConfig, parent)
            .AddChildContent<ConfigProvider>(child => child.Add(x => x.ThemeConfig, new ThemeConfig { Inherit = false })));
        var inner = cut.FindComponents<ConfigProvider>().Single();
        Assert.Equal("default", inner.Instance.ResolvedAlgorithm);
        var style = inner.Find(".ant-theme").GetAttribute("style");
        Assert.Contains("--ant-color-primary:#1677ff;", style);
        Assert.DoesNotContain("--ant-button-primary-color:#abcdef", style);
        Assert.DoesNotContain("colorPrimary", inner.Instance.ExplicitThemeTokens.Keys);
    }

    [Fact]
    public void Dynamic_wireframe_token_loads_the_same_styles_as_the_typed_seed()
    {
        var theme = PreviewThemeConfigFactory.Create("v4");
        var cut = RenderComponent<ConfigProvider>(parameters => parameters.Add(x => x.ThemeConfig, theme));
        Assert.NotNull(cut.Find(".ant-theme-wireframe"));
        Assert.Contains("pagination-wireframe.css", cut.FindAll("link").Last().GetAttribute("href"));
    }
}
