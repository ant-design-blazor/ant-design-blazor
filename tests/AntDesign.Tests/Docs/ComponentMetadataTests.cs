using System;
using System.Linq;
using AntDesign.Docs.Shared;
using Bunit;
using Microsoft.JSInterop;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class ComponentMetadataTests : DocumentationTestBase
{
    [Theory]
    [InlineData("en-US", "Import", "GitHub", "Docs", "Edit this page", "Changelog")]
    [InlineData("zh-CN", "使用", "反馈", "文档", "编辑此页", "更新日志")]
    public void Metadata_has_three_localized_rows_with_Blazor_links(string language, string import, string source, string docs, string edit, string changelog)
    {
        var cut = RenderComponent<ComponentMetadata>(p => p
            .Add(x => x.ComponentName, "Select").Add(x => x.ComponentKey, "select").Add(x => x.Language, language));
        cut.WaitForAssertion(() => Assert.Equal(3, cut.FindAll(".ant-descriptions-row").Count));
        Assert.Equal(new[] { import, source, docs }, cut.FindAll(".ant-descriptions-item-label").Select(x => x.TextContent.Trim()));
        Assert.Equal("@using AntDesign", cut.Find("button").TextContent);
        var links = cut.FindAll("a");
        Assert.Equal(6, links.Count);
        Assert.Equal("https://github.com/ant-design-blazor/ant-design-blazor/tree/master/components/select", links[0].GetAttribute("href"));
        Assert.EndsWith("/issues/new/choose", links[1].GetAttribute("href"));
        Assert.Contains("is:issue is:open Select in:title", Uri.UnescapeDataString(links[2].GetAttribute("href")!));
        Assert.Equal(edit, links[3].TextContent.Trim());
        Assert.EndsWith($"/Select/doc/index.{language}.md", links[3].GetAttribute("href"));
        Assert.Equal("LLMs.md", links[4].TextContent.Trim());
        Assert.Equal($"{language}/components/select.md", links[4].GetAttribute("href"));
        Assert.Equal(changelog, links[5].TextContent.Trim());
        Assert.EndsWith($"/CHANGELOG.{language}.md", links[5].GetAttribute("href"));
        Assert.All(links, link => Assert.Equal("noopener noreferrer", link.GetAttribute("rel")));
        Assert.Single(cut.FindAll(".ant-divider"));
    }

    [Theory]
    [InlineData("Select", "en-US", "@using AntDesign", "Copied")]
    [InlineData("Icon", "zh-CN", "@using AntDesign.Icons", "已复制")]
    public void Import_is_keyboard_accessible_and_copies_the_Blazor_namespace(string name, string language, string import, string copied)
    {
        var clipboard = JSInterop.SetupVoid("navigator.clipboard.writeText", import).SetVoidResult();
        var cut = RenderComponent<ComponentMetadata>(p => p
            .Add(x => x.ComponentName, name).Add(x => x.ComponentKey, name.ToLowerInvariant()).Add(x => x.Language, language));
        Assert.Equal("button", cut.Find("button").GetAttribute("type"));
        cut.Find("button").Click();
        cut.WaitForAssertion(() => Assert.Equal(copied, cut.Find("[role=status]").TextContent));
        Assert.Single(clipboard.Invocations);
        Assert.Equal(copied, cut.Find("button").GetAttribute("aria-label"));
        cut.SetParametersAndRender(p => p.Add(x => x.ComponentName, "Button"));
        Assert.Equal(string.Empty, cut.Find("[role=status]").TextContent);
    }

    [Fact]
    public void A_component_without_Markdown_does_not_get_broken_edit_or_LLM_links()
    {
        var cut = RenderComponent<ComponentMetadata>(p => p.Add(x => x.ComponentName, "App").Add(x => x.ComponentKey, "app"));
        cut.WaitForAssertion(() => Assert.Equal(3, cut.FindAll(".ant-descriptions-row").Count));
        Assert.Equal(4, cut.FindAll("a").Count);
        Assert.DoesNotContain("LLMs.md", cut.Markup);
        Assert.DoesNotContain("/edit/master/", cut.Markup);
        Assert.Contains("Changelog", cut.Markup);
    }

    [Fact]
    public void Clipboard_rejection_does_not_report_success_or_break_the_page()
    {
        JSInterop.SetupVoid("navigator.clipboard.writeText", "@using AntDesign")
            .SetException(new JSException("Clipboard unavailable"));
        var cut = RenderComponent<ComponentMetadata>(p => p.Add(x => x.ComponentName, "Select").Add(x => x.ComponentKey, "select"));
        cut.Find("button").Click();
        cut.WaitForAssertion(() => Assert.Equal("Copy failed", cut.Find("[role=status]").TextContent));
    }

    [Theory]
    [InlineData(null, "", "")]
    [InlineData("<p>Intro</p>", "<p>Intro</p>", "")]
    [InlineData("<p>Intro</p>\n<h2>Usage</h2><ul><li>Item</li></ul>", "<p>Intro</p>\n", "<h2>Usage</h2><ul><li>Item</li></ul>")]
    [InlineData("<p>&lt;h2&gt; is escaped</p><h3 id=\"api\">API</h3>", "<p>&lt;h2&gt; is escaped</p>", "<h3 id=\"api\">API</h3>")]
    public void Introduction_split_preserves_all_compiled_content(string html, string introduction, string remainder)
    {
        var sections = ComponentMetadata.SplitDescription(html);
        Assert.Equal(introduction, sections.Introduction);
        Assert.Equal(remainder, sections.Remainder);
        Assert.Equal(html ?? string.Empty, sections.Introduction + sections.Remainder);
    }
}
