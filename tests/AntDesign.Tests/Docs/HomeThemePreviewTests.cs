using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.Docs.Components;
using AntDesign.Docs.Services;
using AntDesign.Theming;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class HomeThemePreviewTests : DocumentationTestBase
{
    public HomeThemePreviewTests()
    {
        Context.JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<AntDesign.JsInterop.DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
            .SetResult(new AntDesign.JsInterop.DomRect { Width = 300, Height = 32 });
        Context.Services.AddSingleton(new HttpClient(new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.ServiceUnavailable))));
    }
    [Fact]
    public void Renders_all_themes_and_switches_view_and_theme()
    {
        var cut = Context.RenderComponent<HomeThemePreview>();

        var swatches = cut.FindAll("button.home-theme-swatch");
        Assert.Equal(13, swatches.Count);
        Assert.Equal(
            GeneratedPreviewThemes.All.Select(theme => theme.Name),
            swatches.Select(item => item.GetAttribute("aria-label")));
        Assert.Equal("true", swatches[0].GetAttribute("aria-selected"));
        Assert.All(swatches.Skip(1), swatch => Assert.Equal("false", swatch.GetAttribute("aria-selected")));
        Assert.NotNull(cut.Find(".home-theme-components"));
        Assert.Contains("@media(max-width:600px)", cut.Find("style").TextContent);
        Assert.Contains("@media(width < 768px)", cut.Find("#home-theme-preview-style").TextContent);
        Assert.DoesNotContain("100vw", cut.Find("#home-theme-preview-style").TextContent);
        Assert.Contains("overflow-x:auto", cut.Find("#home-theme-preview-style").TextContent);
        Assert.Contains("flex-shrink:0", cut.Find("#home-theme-preview-style").TextContent);

        cut.FindAll(".home-theme-tabs .ant-segmented-item-label")[1].Click();
        Assert.Empty(cut.FindAll(".home-theme-components"));
        Assert.NotEmpty(cut.FindAll(".home-theme-dashboard"));

        var lightTabsClass = cut.Find(".home-theme-tabs").GetAttribute("class");
        var lightTabItemClass = cut.Find(".home-theme-tabs .ant-segmented-item").GetAttribute("class");
        cut.Find("button[title='Dark']").Click();

        cut.WaitForAssertion(
            () => Assert.NotEqual(lightTabsClass, cut.Find(".home-theme-tabs").GetAttribute("class")),
            TimeSpan.FromSeconds(10));
        Assert.Contains("--home-theme-primary:#1668dc", cut.Find("#home-theme-preview-style").TextContent);
        Assert.Equal("true", cut.Find("button[title='Dark']").GetAttribute("aria-selected"));
        Assert.Equal("false", cut.Find("button[title='Ant Design']").GetAttribute("aria-selected"));

        cut.Find("button[title='Geek']").Click();
        var darkBackgroundCss = cut.Find("#home-theme-preview-style").TextContent;
        Assert.Contains("rgba(255,255,255,.14)", darkBackgroundCss);
        Assert.Contains("rgba(255,255,255,.78)", darkBackgroundCss);
        Assert.NotEqual(lightTabItemClass, cut.Find(".home-theme-tabs .ant-segmented-item").GetAttribute("class"));
    }

    [Fact]
    public void Theme_preview_matches_react_group_structure_and_background_styles()
    {
        var cut = Context.RenderComponent<HomeThemePreview>();

        var previewRoot = cut.Find(".home-theme-preview");
        Assert.Equal("DIV", previewRoot.TagName);
        Assert.Null(previewRoot.GetAttribute("aria-hidden"));
        Assert.NotNull(cut.Find(".home-theme-preview > .adb-home-theme-preview-group-container"));
        Assert.NotNull(cut.Find(".home-theme-preview > .adb-home-theme-preview-group-mask > .adb-home-theme-preview-typography-wrapper"));
        Assert.NotNull(cut.Find(".adb-home-theme-preview-group-children > .adb-home-theme-preview-theme-container > .adb-home-theme-preview-wrapper"));
        Assert.Equal("font-weight:900", cut.Find(".adb-home-theme-preview-heading h1").GetAttribute("style"));
        var generatedCss = cut.Find("#home-theme-preview-style").TextContent;
        Assert.Contains("background-position:50% 0%", generatedCss);
        Assert.DoesNotContain("background-color:var(--home-theme-bg)", generatedCss);
        Assert.Contains("position:relative;z-index:1;padding-block:var(--ant-margin-far-sm, 80px)", generatedCss);
    }

    [Fact]
    public void Dark_background_theme_uses_white_group_title_and_description()
    {
        var cut = Context.RenderComponent<HomeThemePreview>();
        var lightHeadingClass = cut.Find(".home-theme-heading").GetAttribute("class");

        cut.Find("button[title='Geek']").Click();

        Assert.Equal("true", cut.Find("button[title='Geek']").GetAttribute("aria-selected"));
        Assert.NotEqual(lightHeadingClass, cut.Find(".home-theme-heading").GetAttribute("class"));
        Assert.Contains("color:#fff", cut.Find("#home-theme-preview-style").TextContent);
    }

    [Fact]
    public void Exposes_keyboard_and_screen_reader_theme_controls()
    {
        var cut = Context.RenderComponent<HomeThemePreview>();

        Assert.Equal(2, cut.FindAll(".home-theme-tabs input[type=radio]").Count);
        Assert.NotNull(cut.Find("[role='tablist'][aria-label='Theme']"));
        Assert.All(cut.FindAll("button.home-theme-swatch"), swatch =>
        {
            Assert.NotEmpty(swatch.GetAttribute("title") ?? string.Empty);
            Assert.NotEmpty(swatch.GetAttribute("aria-label") ?? string.Empty);
            Assert.Equal("tab", swatch.GetAttribute("role"));
        });
    }

    [Fact]
    public void Contributor_avatars_match_react_preview_accessibility_and_dimensions()
    {
        var cut = Context.RenderComponent<HomeThemePreview>();
        var avatars = cut.FindAll(".home-theme-contributors .ant-avatar");
        var images = cut.FindAll(".home-theme-contributors .ant-avatar img");

        Assert.Equal(7, avatars.Count);
        Assert.Equal(new[]
        {
            "https://avatars.githubusercontent.com/u/507615?v=4",
            "https://avatars.githubusercontent.com/u/5378891?v=4",
            "https://avatars.githubusercontent.com/u/49217418?v=4",
            "https://avatars.githubusercontent.com/u/117748716?v=4",
            "https://avatars.githubusercontent.com/u/59312002?v=4",
            "https://avatars.githubusercontent.com/u/82765353?v=4",
        }, images.Select(image => image.GetAttribute("src")));
        Assert.All(avatars, avatar =>
        {
            Assert.Contains("width:46px;height:46px;line-height:46px;", avatar.GetAttribute("style"));
            Assert.Equal("false", avatar.GetAttribute("draggable"));
        });
        Assert.All(images, image =>
            Assert.Equal("Contributor: Ant Design contributor", image.GetAttribute("alt")));
        Assert.All(avatars.Take(6), avatar =>
            Assert.Equal("Contributor: Ant Design contributor", avatar.GetAttribute("aria-label")));
        Assert.Equal("+5", avatars[^1].TextContent.Trim());
        Assert.Contains("We've sent a code to a****@gmail.com", cut.Find(".home-theme-verify").TextContent);
    }

    [Fact]
    public void Contributor_avatars_fetch_react_contributor_data_and_exclude_bots()
    {
        const string response = """
            [
              {"avatar_url":"https://avatars.githubusercontent.com/u/1?v=4","login":"alice","type":"User"},
              {"avatar_url":"https://avatars.githubusercontent.com/u/2?v=4","login":"github-actions[bot]","type":"Bot"},
              {"avatar_url":"https://avatars.githubusercontent.com/u/3?v=4","login":"renovate-helper","type":"User"},
              {"avatar_url":"https://avatars.githubusercontent.com/u/4?v=4","login":"bob","type":"User"},
              {"avatar_url":"https://avatars.githubusercontent.com/u/5?v=4","login":"carol","type":"User"},
              {"avatar_url":"https://avatars.githubusercontent.com/u/6?v=4","login":"dave","type":"User"},
              {"avatar_url":"https://avatars.githubusercontent.com/u/7?v=4","login":"erin","type":"User"},
              {"avatar_url":"https://avatars.githubusercontent.com/u/8?v=4","login":"frank","type":"User"},
              {"avatar_url":"https://avatars.githubusercontent.com/u/9?v=4","login":"grace","type":"User"}
            ]
            """;
        var requestUri = string.Empty;
        var userAgent = string.Empty;
        var accept = string.Empty;
        Context.Services.AddSingleton(new HttpClient(new StubHttpMessageHandler(request =>
        {
            requestUri = request.RequestUri?.ToString() ?? string.Empty;
            userAgent = request.Headers.UserAgent.ToString();
            accept = request.Headers.Accept.ToString();
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(response, Encoding.UTF8, "application/json")
            };
        })));

        var cut = Context.RenderComponent<HomeThemePreview>();
        var avatars = cut.FindAll(".home-theme-contributors .ant-avatar");
        var contributorAvatars = avatars.Take(6).ToArray();

        Assert.Equal("https://api.github.com/repos/ant-design/ant-design/contributors?per_page=100", requestUri);
        Assert.Equal("ant-design-blazor-docs", userAgent);
        Assert.Equal("application/vnd.github+json", accept);
        Assert.Equal(7, avatars.Count);
        Assert.Equal(6, contributorAvatars.Length);
        Assert.DoesNotContain(contributorAvatars, avatar => avatar.GetAttribute("aria-label")?.Contains("github-actions", System.StringComparison.OrdinalIgnoreCase) == true);
        Assert.DoesNotContain(contributorAvatars, avatar => avatar.GetAttribute("aria-label")?.Contains("renovate", System.StringComparison.OrdinalIgnoreCase) == true);
        Assert.All(contributorAvatars, avatar =>
        {
            Assert.Contains(new[] { "alice", "bob", "carol", "dave", "erin", "frank", "grace" }, login =>
                avatar.GetAttribute("aria-label") == $"Contributor: {login}");
            Assert.Contains("width:46px;height:46px;line-height:46px;", avatar.GetAttribute("style"));
            Assert.Equal("false", avatar.GetAttribute("draggable"));
        });
    }

    [Fact]
    public void Every_theme_uses_its_generated_global_and_component_configuration()
    {
        var cut = Context.RenderComponent<HomeThemePreview>();
        foreach (var definition in GeneratedPreviewThemes.All)
        {
            cut.Find($"button[data-theme-key='{definition.Key}']").Click();
            var provider = cut.FindComponent<ConfigProvider>().Instance;
            Assert.False(provider.ThemeConfig.Inherit);
            Assert.Equal(definition.Key, provider.ThemeConfig.Key);
            Assert.Equal(definition.Algorithm, provider.ResolvedAlgorithm);
            Assert.Equal(definition.Tokens.Count, provider.ThemeConfig.GetExplicitTokenValues().Count);
            Assert.Equal(definition.Components.Count, provider.ThemeConfig.Components.Count);
            foreach (var component in definition.Components)
                foreach (var token in component.Value)
                    Assert.Equal(token.Value, provider.ThemeConfig.Components[component.Key][token.Key]);
        }
        Assert.NotEmpty(cut.FindAll("svg title").Where(title => title.TextContent == "Serene Icon"));
        Assert.All(cut.FindComponents<AntDesign.Progress>().Where(p => p.Instance.Type == ProgressType.Line), p => Assert.True(p.Instance.ShowInfo));
        Assert.Empty(cut.FindAll(".home-theme-progress-list > span"));
        Assert.NotNull(cut.Find(".ant-space-compact"));
        Assert.Equal(2, cut.FindAll(".home-theme-compact button").Count);
        Assert.NotNull(cut.Find(".adb-home-theme-preview-main-card > .ant-border-beam[aria-hidden=true]"));
        Assert.NotNull(cut.Find(".home-theme-segments .anticon-message"));
        Assert.NotNull(cut.Find(".home-theme-segments .anticon-mail"));
    }

    [Fact]
    public void Progress_uses_the_v6_rail_and_inline_circle_hides_its_label()
    {
        var cut = Context.RenderComponent<HomeThemePreview>();
        Assert.All(cut.FindAll(".home-theme-progress-list .ant-progress-rail"), rail => Assert.Contains("height:8px", rail.GetAttribute("style")));
        Assert.Empty(cut.FindAll(".ant-progress-inline-circle .ant-progress-text"));
        Assert.Equal("15", cut.Find(".ant-progress-inline-circle .ant-progress-circle-rail").GetAttribute("stroke-width"));
    }

    [Fact]
    public void Copied_theme_contains_complete_CSharp_configuration()
    {
        var config = PreviewThemeConfigFactory.Create("mui");
        config.Inherit = false;
        var code = PreviewThemeCodeFormatter.Format(config);
        Assert.Contains("var theme = new ThemeConfig", code);
        Assert.Contains("[\"colorPrimary\"] = \"#1976d2\"", code);
        Assert.Contains("[\"Button\"] = new Dictionary<string, string>", code);
        Assert.Contains("[\"paddingInline\"] = \"16\"", code);
        Assert.Contains("Inherit = false", code);
        var tree = Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(code);
        Assert.Empty(tree.GetDiagnostics().Where(d => d.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Error));
    }

    private sealed class StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> respond) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(respond(request));
    }
}
