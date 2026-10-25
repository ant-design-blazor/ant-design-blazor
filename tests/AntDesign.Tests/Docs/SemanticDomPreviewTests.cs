using System.Linq;
using System.Threading.Tasks;
using AntDesign.Docs.Components;
using Bunit;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class SemanticDomPreviewTests : DocumentationTestBase
{
    [Fact]
    public async Task Highlights_real_dom_for_hover_focus_and_keyboard_activation()
    {
        var cut = Context.RenderComponent<SemanticDomPreview>(parameters => parameters
            .Add(x => x.Component, "button")
            .AddChildContent<AntDesign.Button>(button => button.AddChildContent("Save")));

        var root = cut.Find(".adb-semantic-preview-root");
        var items = cut.FindAll(".adb-semantic-preview-item");
        Assert.Equal(3, items.Count);
        Assert.Equal(new[] { "content", "icon", "root" }, items.Select(item => item.QuerySelector("strong")!.TextContent).OrderBy(name => name));
        Assert.Equal(".ant-btn", items.Single(item => item.QuerySelector("strong")!.TextContent == "root").QuerySelector("code")!.TextContent);
        Assert.Equal(".ant-btn-icon", items.Single(item => item.QuerySelector("strong")!.TextContent == "icon").QuerySelector("code")!.TextContent);
        Assert.Equal(".ant-btn > span:not(.ant-btn-icon)", items.Single(item => item.QuerySelector("strong")!.TextContent == "content").QuerySelector("code")!.TextContent);
        Assert.NotNull(cut.Find(".ant-btn"));
        Assert.Contains("@media (max-width: 767.99px)", cut.Find("style").TextContent);
        Assert.All(items, item =>
        {
            Assert.Equal("button", item.GetAttribute("role"));
            Assert.Equal("0", item.GetAttribute("tabindex"));
            Assert.Equal("false", item.GetAttribute("aria-pressed"));
        });

        AngleSharp.Dom.IElement Item(string name) => cut.FindAll(".adb-semantic-preview-item").Single(item => item.QuerySelector("strong")!.TextContent == name);
        await Item("root").TriggerEventAsync("onmouseenter", new MouseEventArgs());
        Assert.Equal("button-root", root.GetAttribute("data-active-semantic"));
        Assert.Contains("adb-semantic-preview-active", Item("root").ClassName);
        Assert.Equal("true", Item("root").GetAttribute("aria-pressed"));

        await Item("icon")
            .TriggerEventAsync("onfocus", new FocusEventArgs());
        Assert.Equal("button-icon", root.GetAttribute("data-active-semantic"));
        Assert.Equal("true", Item("icon").GetAttribute("aria-pressed"));

        await Item("icon")
            .TriggerEventAsync("onkeydown", new KeyboardEventArgs { Key = "Enter" });
        Assert.Equal("button-icon", root.GetAttribute("data-active-semantic"));

        await Item("content")
            .TriggerEventAsync("onkeydown", new KeyboardEventArgs { Key = " " });
        Assert.Equal("button-content", root.GetAttribute("data-active-semantic"));

        await Item("root")
            .TriggerEventAsync("onkeydown", new KeyboardEventArgs { Key = "Escape" });
        Assert.Equal("button-content", root.GetAttribute("data-active-semantic"));
    }

    [Fact]
    public void Exposes_keyboard_focus_contract_and_selector_mapping()
    {
        var cut = Context.RenderComponent<SemanticDomPreview>(parameters => parameters
            .Add(x => x.Component, "alert")
            .AddChildContent<AntDesign.Alert>(alert => alert.Add(x => x.Message, "Saved")));

        var items = cut.FindAll(".adb-semantic-preview-item");
        Assert.Equal(7, items.Count);
        Assert.All(items, item =>
        {
            Assert.Equal("button", item.GetAttribute("role"));
            Assert.Equal("0", item.GetAttribute("tabindex"));
            Assert.NotNull(item.QuerySelector("code"));
            Assert.Contains("v6.0.0", item.TextContent);
        });
        Assert.Equal(new[]
        {
            ".ant-alert", ".ant-alert-icon", ".ant-alert-section", ".ant-alert-title",
            ".ant-alert-description", ".ant-alert-actions", ".ant-alert-close-icon"
        }.OrderBy(selector => selector), items.Select(item => item.QuerySelector("code")!.TextContent).OrderBy(selector => selector));
        Assert.NotNull(cut.Find(".ant-alert"));
        Assert.NotNull(cut.Find(".ant-alert-section"));
    }

    [Theory]
    [InlineData("dropdown", "dropdown-root", ".ant-dropdown")]
    [InlineData("modal", "modal-root", ".ant-modal")]
    [InlineData("notification", "notification-root", ".ant-notification")]
    [InlineData("popconfirm", "popconfirm-root", ".ant-popover")]
    [InlineData("popover", "popover-root", ".ant-popover")]
    [InlineData("tooltip", "tooltip-root", ".ant-tooltip")]
    [InlineData("select", "select-popup.list", ".ant-select-dropdown-list")]
    [InlineData("select", "select-popup.listItem", ".ant-select-item")]
    public void Emits_component_scoped_global_highlight_rules_for_portals(string component, string activeKey, string portalSelector)
    {
        var cut = Context.RenderComponent<SemanticDomPreview>(parameters => parameters
            .Add(x => x.Component, component));

        var css = cut.Find("style").TextContent;
        var expectedSelector = $"body:has(.adb-semantic-preview-root[data-active-semantic=\"{activeKey}\"]) {portalSelector}";

        Assert.Contains(expectedSelector, css);
        Assert.Contains("outline:2px solid var(--ant-color-warning, #faad14)", css);
        Assert.DoesNotContain($"body {portalSelector}", css);
    }

    [Fact]
    public void Keeps_message_highlight_inside_its_semantic_preview()
    {
        var cut = Context.RenderComponent<SemanticDomPreview>(parameters => parameters
            .Add(x => x.Component, "message")
            .AddChildContent("message preview"));

        var css = cut.Find("style").TextContent;

        Assert.DoesNotContain("body:has(", css);
        Assert.Contains("[data-active-semantic=\"message-root\"] .ant-message", css);
        Assert.Contains("[data-active-semantic=\"message-list\"] .ant-message-list", css);
    }

    [Fact]
    public void Gives_masonry_a_stretching_preview_host_for_its_absolutely_positioned_items()
    {
        var cut = Context.RenderComponent<SemanticDomPreview>(parameters => parameters
            .Add(x => x.Component, "masonry"));

        var css = cut.Find("style").TextContent;
        var contentClass = cut.Find(".adb-semantic-preview-content").ClassList[0];

        Assert.Contains($".{contentClass}", css);
        Assert.Contains("flex-direction:column", css);
        Assert.Contains("align-items:stretch", css);
    }

    [Fact]
    public void Exposes_cascader_slots_and_highlights_its_portaled_popup()
    {
        var cut = Context.RenderComponent<SemanticDomPreview>(parameters => parameters
            .Add(x => x.Component, "cascader")
            .AddChildContent<CascaderSemanticPreview>());

        var items = cut.FindAll(".adb-semantic-preview-item");
        Assert.Equal(new[]
        {
            "clear", "content", "input", "item", "itemContent", "itemRemove", "placeholder", "popup.list", "popup.listItem", "popup.root", "prefix", "root", "suffix"
        }, items.Select(item => item.QuerySelector("strong")!.TextContent).ToArray());
        Assert.Equal(new[]
        {
            ".ant-select-clear", ".ant-select-content", ".ant-select-input",
            ".ant-select-selection-item", ".ant-select-selection-item-content", ".ant-select-selection-item-remove",
            ".ant-select-placeholder", ".ant-cascader-menu", ".ant-cascader-menu-item", ".ant-select-dropdown", ".ant-select-prefix",
            ".ant-select", ".ant-select-suffix"
        }, items.Select(item => item.QuerySelector("code")!.TextContent).ToArray());
        Assert.NotNull(cut.Find(".ant-select.ant-cascader"));
        Assert.Equal("prefix", cut.Find(".ant-select-prefix").TextContent.Trim());
        Assert.Equal("DIV", cut.Find(".ant-select > .ant-select-prefix").TagName);
        Assert.DoesNotContain("padding-left", cut.Find(".ant-select-content").GetAttribute("style") ?? "");
        Assert.NotNull(cut.Find(".ant-select-input"));
        Assert.NotNull(cut.Find(".ant-cascader-menu-item"));
        Assert.NotNull(cut.Find(".ant-cascader-menu-item-content"));
        Assert.NotNull(cut.Find(".ant-select-clear"));

        var css = cut.Find("style").TextContent;
        Assert.Contains("data-active-semantic=\"cascader-popup.root\"", css);
        Assert.Contains(".ant-select-dropdown", css);
    }

    [Fact]
    public void Does_not_emit_global_highlight_rules_for_non_portaled_components()
    {
        var cut = Context.RenderComponent<SemanticDomPreview>(parameters => parameters
            .Add(x => x.Component, "button"));

        Assert.DoesNotContain(":has(", cut.Find("style").TextContent);
    }

    [Theory]
    [InlineData("qrcode")]
    [InlineData("qr-code")]
    public void Qrcode_cover_highlight_stays_inside_the_clipped_root(string component)
    {
        var cut = Context.RenderComponent<SemanticDomPreview>(parameters => parameters
            .Add(x => x.Component, component));
        var css = cut.Find("style").TextContent;
        Assert.Contains(".ant-qrcode-cover{outline:2px solid", css);
        Assert.Contains("outline-offset:-2px;box-shadow:inset 0 0 0 2px", css);
        Assert.DoesNotContain("body:has(", css);
    }
}
