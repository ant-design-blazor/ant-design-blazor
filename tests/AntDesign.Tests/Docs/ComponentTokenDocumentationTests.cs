using System.Linq;
using AntDesign.Docs.Components;
using AntDesign.Docs.Services;
using AntDesign.Theming;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class ComponentTokenDocumentationTests : AntDesignTestBase
{
    [Fact]
    public void Qrcode_renders_the_global_tokens_it_uses_in_both_locales()
    {
        var cut = RenderComponent<ComponentTokenDocumentation>(parameters => parameters.Add(x => x.Component, "qr-code"));
        Assert.Equal(12, cut.FindAll(".global-token-table tbody tr").Count);
        Assert.Contains("rgba(0, 0, 0, 0.88)", cut.Markup);
        Assert.Contains("Default text color", cut.Markup);

        cut.SetParametersAndRender(parameters => parameters.Add(x => x.Language, "zh-CN"));
        Assert.Equal(12, cut.FindAll(".global-token-table tbody tr").Count);
        Assert.Contains("最深的文本色", cut.Find(".global-token-table").TextContent);
        Assert.Equal(ComponentTokenCatalog.GetGlobal("QRCode").Count, ComponentTokenCatalog.GetGlobal("qr-code").Count);
    }

    [Fact]
    public void Every_documented_global_token_has_a_runtime_default()
    {
        var defaults = new ThemeConfig().GetResolvedTokenValues();
        var names = GeneratedGlobalTokens.All.Select(value => value.Name).Distinct();
        Assert.All(names, name => Assert.True(defaults.TryGetValue(name, out var value) && !string.IsNullOrWhiteSpace(value), $"Missing runtime default for {name}"));
    }
}
