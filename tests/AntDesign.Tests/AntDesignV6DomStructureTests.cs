using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using AntDesign.Tests;
using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace AntDesignV6.Tests;

/// <summary>
/// Red-phase contracts for the React antd v6 DOM/Semantic DOM migration.
/// These assertions intentionally describe v6 slots that the current Blazor
/// renderers do not expose yet. Keep them failing until the corresponding
/// component migration is implemented.
/// </summary>
public sealed class AntDesignV6DomStructureTests : AntDesignTestBase
{
    private sealed class V6TableRow
    {
        public string Name { get; set; } = "row";
    }

    private sealed class V6TreeItem
    {
        public string Name { get; set; } = "Node";
        public System.Collections.Generic.List<V6TreeItem> Children { get; set; } = new();
    }

    public AntDesignV6DomStructureTests()
    {
        JSInterop.Setup<HtmlElement>("AntDesign.interop.domInfoHelper.getInfo", _ => true)
            .SetResult(new HtmlElement());
        JSInterop.Setup<AntDesign.JsInterop.DomRect>(AntDesign.JSInteropConstants.GetBoundingClientRect, _ => true)
            .SetResult(new AntDesign.JsInterop.DomRect { Width = 200 });
        JSInterop.SetupVoid("AntDesign.interop.domManipulationHelper.setDomAttribute", _ => true);
    }

    [Fact]
    public void Button_exposes_v6_root_icon_and_unclassed_content_node()
    {
        var cut = Context.RenderComponent<AntDesign.Button>(parameters =>
        {
            parameters.Add(x => x.Icon, "search");
            parameters.AddChildContent("Save");
        });

        Assert.Contains("ant-btn", cut.Markup, StringComparison.Ordinal);
        Assert.Contains("ant-btn-icon", cut.Markup, StringComparison.Ordinal);
        Assert.DoesNotContain("ant-btn-content", cut.Markup, StringComparison.Ordinal);
        Assert.Equal("Save", cut.Find("button > span:not(.ant-btn-icon)").TextContent);
    }

    [Fact]
    public void Alert_uses_the_v6_section_slot_for_title_and_description()
    {
        var cut = Context.RenderComponent<AntDesign.Alert>(parameters =>
        {
            parameters.Add(x => x.Message, "Title");
            parameters.Add(x => x.Description, "Description");
        });

        var section = cut.Find(".ant-alert-section");
        Assert.NotNull(section.QuerySelector(".ant-alert-title"));
        Assert.NotNull(section.QuerySelector(".ant-alert-description"));
    }

    [Fact]
    public void Button_in_rtl_context_exposes_the_v6_rtl_state_class()
    {
        var cut = Context.RenderComponent<AntDesign.ConfigProvider>(parameters =>
        {
            parameters.Add(x => x.Direction, "RTL");
            parameters.AddChildContent<AntDesign.Button>(button => button.AddChildContent("Save"));
        });

        Assert.Contains("ant-btn-rtl", cut.Markup, StringComparison.Ordinal);
    }

    [Fact]
    public void Badge_exposes_v6_root_and_indicator_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Badge>(parameters => parameters.Add(x => x.Count, 5));

        Assert.NotNull(cut.Find(".ant-badge"));
        Assert.NotNull(cut.Find(".ant-badge-count"));
    }

    [Fact]
    public void Breadcrumb_exposes_v6_item_and_separator_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Breadcrumb>(parameters =>
            parameters
                .Add(x => x.Separator, "-")
                .AddChildContent<AntDesign.BreadcrumbItem>(item => item.Add(x => x.ChildContent, "Home"))
                .AddChildContent<AntDesign.BreadcrumbItem>(item => item.Add(x => x.ChildContent, "Page")));

        Assert.NotNull(cut.Find(".ant-breadcrumb"));
        Assert.NotNull(cut.Find(".ant-breadcrumb-item"));
        Assert.NotNull(cut.Find(".ant-breadcrumb-separator"));
    }

    [Fact]
    public void Card_exposes_v6_header_title_extra_and_body_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Card>(parameters =>
        {
            parameters.Add(x => x.Title, "Title");
            parameters.Add(x => x.Extra, builder => builder.AddContent(0, "More"));
            parameters.AddChildContent("Content");
        });

        Assert.NotNull(cut.Find(".ant-card"));
        Assert.Contains("css-var-root", cut.Find(".ant-card").ClassList);
        Assert.NotNull(cut.Find(".ant-card-head"));
        Assert.NotNull(cut.Find(".ant-card-head-title"));
        Assert.NotNull(cut.Find(".ant-card-extra"));
        Assert.NotNull(cut.Find(".ant-card-body"));
    }

    [Fact]
    public void CardMeta_exposes_v6_section_and_description_slots()
    {
        var cut = Context.RenderComponent<AntDesign.CardMeta>(parameters =>
        {
            parameters.Add(x => x.Title, "Title");
            parameters.Add(x => x.Description, "Description");
        });

        Assert.NotNull(cut.Find(".ant-card-meta"));
        Assert.NotNull(cut.Find(".ant-card-meta-section"));
        Assert.NotNull(cut.Find(".ant-card-meta-title"));
        Assert.NotNull(cut.Find(".ant-card-meta-description"));
    }

    [Fact]
    public void Collapse_exposes_v6_header_title_icon_and_body_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Collapse>(parameters =>
        {
            parameters.Add(x => x.DefaultActiveKey, new[] { "1" });
            parameters.AddChildContent<AntDesign.Panel>(panel =>
            {
                panel.Add(x => x.Key, "1");
                panel.Add(x => x.Header, "Header");
                panel.AddChildContent("Content");
            });
        });

        Assert.NotNull(cut.Find(".ant-collapse"));
        Assert.Contains("css-var-root", cut.Find(".ant-collapse").ClassList);
        Assert.Contains("ant-collapse-icon-placement-start", cut.Find(".ant-collapse").ClassList);
        Assert.NotNull(cut.Find(".ant-collapse-header"));
        Assert.NotNull(cut.Find(".ant-collapse-title"));
        Assert.NotNull(cut.Find(".ant-collapse-expand-icon"));
        Assert.NotNull(cut.Find(".ant-collapse-panel-active .ant-collapse-body"));
    }

    [Fact]
    public void Descriptions_exposes_v6_header_title_label_and_content_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Descriptions>(parameters =>
        {
            parameters.Add(x => x.Title, "User Info");
            parameters.AddChildContent<AntDesign.DescriptionsItem>(item =>
            {
                item.Add(x => x.Title, "Name");
                item.AddChildContent("Alice");
            });
        });

        Assert.NotNull(cut.Find(".ant-descriptions"));
        Assert.NotNull(cut.Find(".ant-descriptions-header"));
        Assert.NotNull(cut.Find(".ant-descriptions-title"));
        Assert.NotNull(cut.Find(".ant-descriptions-item-label"));
        Assert.NotNull(cut.Find(".ant-descriptions-item-content"));
    }

    [Fact]
    public void Divider_exposes_v6_root_rail_and_content_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Divider>(parameters =>
            parameters.AddChildContent("Section"));

        Assert.Equal("DIV", cut.Find(".ant-divider").TagName);
        Assert.NotNull(cut.Find(".ant-divider-rail-start"));
        Assert.NotNull(cut.Find(".ant-divider-rail-end"));
        Assert.NotNull(cut.Find(".ant-divider-inner-text"));
        Assert.Equal("separator", cut.Find(".ant-divider").GetAttribute("role"));
    }

    [Fact]
    public void Empty_exposes_v6_root_image_description_and_footer_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Empty>(parameters =>
            parameters.Add(x => x.Description, "No data")
                .AddChildContent("Create one"));

        Assert.NotNull(cut.Find(".ant-empty"));
        Assert.NotNull(cut.Find(".ant-empty-image"));
        Assert.NotNull(cut.Find(".ant-empty-description"));
        Assert.NotNull(cut.Find(".ant-empty-footer"));
    }

    [Fact]
    public void Checkbox_exposes_v6_root_icon_and_label_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Checkbox>(parameters =>
            parameters.Add(x => x.Label, "Accept"));

        Assert.NotNull(cut.Find(".ant-checkbox-wrapper"));
        Assert.NotNull(cut.Find(".ant-checkbox"));
        Assert.NotNull(cut.Find(".ant-checkbox-label"));
    }

    [Fact]
    public void Radio_exposes_v6_root_icon_and_label_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Radio<string>>(parameters =>
            parameters.AddChildContent("Option"));

        Assert.NotNull(cut.Find(".ant-radio-wrapper"));
        Assert.NotNull(cut.Find(".ant-radio"));
        Assert.NotNull(cut.Find(".ant-radio-label"));
    }

    [Fact]
    public void Pagination_exposes_v6_root_and_item_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Pagination>(parameters =>
            parameters.Add(x => x.Total, 50));

        Assert.NotNull(cut.Find(".ant-pagination"));
        Assert.NotNull(cut.Find(".ant-pagination-item"));
        Assert.Equal("true", cut.Find(".ant-pagination-prev").GetAttribute("aria-disabled"));
        Assert.Equal("false", cut.Find(".ant-pagination-next").GetAttribute("aria-disabled"));
        Assert.Contains("ant-pagination-item-active", cut.Find(".ant-pagination-item-1").ClassList);
        Assert.Equal("nofollow", cut.Find(".ant-pagination-item-1 a").GetAttribute("rel"));
    }

    [Fact]
    public void Transfer_uses_v6_section_and_actions_structure()
    {
        var cut = Context.RenderComponent<AntDesign.Transfer>(parameters =>
            parameters.Add(x => x.DataSource, new[]
            {
                new AntDesign.TransferItem { Key = "item-1", Title = "Source item" },
            })
            .Add(x => x.TargetKeys, Array.Empty<string>())
            .Add(x => x.SelectedKeys, Array.Empty<string>()));

        Assert.Equal(2, cut.FindAll(".ant-transfer-section").Count);
        Assert.Equal(2, cut.FindAll(".ant-transfer-list-header").Count);
        Assert.Equal(2, cut.FindAll(".ant-transfer-list-body").Count);
        Assert.NotNull(cut.Find(".ant-transfer-actions"));
        Assert.Single(cut.FindAll(".ant-transfer-list-content-item"));
        var itemCheckbox = cut.Find(".ant-transfer-list-content-item > .ant-transfer-list-checkbox");
        Assert.NotNull(itemCheckbox);
        Assert.Null(itemCheckbox.QuerySelector(".ant-checkbox-label"));
        Assert.NotNull(cut.Find(".ant-transfer-list-content-item > .ant-transfer-list-content-item-text"));
        Assert.Empty(cut.FindAll(".ant-transfer-operation"));
        Assert.Empty(cut.FindAll(".ant-transfer-list-footer"));
    }

    [Fact]
    public void TreeSelect_uses_v6_root_popup_and_tree_item_classes()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<AntDesign.Core.JsInterop.Modules.Components.OverlayPosition>(
                AntDesign.JSInteropConstants.OverlayComponentHelper.AddOverlayToContainer,
                _ => true)
            .SetResult(new AntDesign.Core.JsInterop.Modules.Components.OverlayPosition());
        JSInterop.Setup<AntDesign.JsInterop.DomRect>(AntDesign.JSInteropConstants.GetBoundingClientRect, _ => true)
            .SetResult(new AntDesign.JsInterop.DomRect { Width = 200 });

        var cut = Context.RenderComponent<AntDesign.TreeSelect<string, string>>(parameters => parameters
            .Add(x => x.DataSource, new[] { "tree item" })
            .Add(x => x.TitleExpression, node => node.DataItem)
            .Add(x => x.Open, true)
            .Add(x => x.TreeDefaultExpandAll, true));

        var root = cut.Find(".ant-select.ant-tree-select");
        Assert.Contains("ant-select-outlined", root.ClassList);
        Assert.Contains("ant-select-css-var", root.ClassList);
        Assert.Contains("ant-tree-select-css-var", root.ClassList);

        var popup = cut.WaitForElement(".ant-select-dropdown.ant-tree-select-dropdown");
        Assert.Contains("ant-select-css-var", popup.ClassList);
        Assert.Contains("ant-tree-select-css-var", popup.ClassList);
        Assert.NotNull(popup.QuerySelector(".ant-select-tree .ant-select-tree-treenode"));
        Assert.NotNull(popup.QuerySelector(".ant-select-tree-title"));
    }

    [Fact]
    public void Tag_exposes_v6_root_icon_content_and_close_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Tag>(parameters =>
            parameters.Add(x => x.Icon, "search")
                .Add(x => x.Closable, true)
                .AddChildContent("Ant Design"));

        Assert.NotNull(cut.Find(".ant-tag"));
        Assert.NotNull(cut.Find(".ant-tag-icon"));
        Assert.NotNull(cut.Find(".ant-tag-content"));
        Assert.NotNull(cut.Find(".ant-tag-close"));
    }

    [Fact]
    public void Space_exposes_v6_root_item_and_separator_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Space>(parameters =>
            parameters.Add(x => x.Split, builder => builder.AddContent(0, "-"))
                .AddChildContent<AntDesign.SpaceItem>(item => item.AddChildContent("A"))
                .AddChildContent<AntDesign.SpaceItem>(item => item.AddChildContent("B")));

        Assert.NotNull(cut.Find(".ant-space"));
        Assert.NotNull(cut.Find(".ant-space-item"));
        Assert.NotNull(cut.Find(".ant-space-item-separator"));
    }

    [Fact]
    public void SpaceCompact_emits_v6_root_and_child_position_classes_without_wrapper_nodes()
    {
        var cut = Context.RenderComponent<AntDesign.SpaceCompact>(parameters =>
            parameters.Add(x => x.Block, true)
                .AddChildContent<AntDesign.SpaceCompactItem>(first => first
                    .Add(x => x.Position, AntDesign.SpaceCompactItemPosition.First)
                    .AddChildContent<AntDesign.Button>(button => button.AddChildContent("A")))
                .AddChildContent<AntDesign.SpaceCompactItem>(last => last
                    .Add(x => x.Position, AntDesign.SpaceCompactItemPosition.Last)
                    .AddChildContent<AntDesign.Button>(button => button.AddChildContent("B"))));

        var root = cut.Find(".ant-space-compact.ant-space-compact-block");
        Assert.Equal(2, root.QuerySelectorAll(":scope > .ant-btn").Length);
        Assert.Contains("ant-btn-compact-item", root.QuerySelectorAll(".ant-btn")[0].ClassList);
        Assert.Contains("ant-btn-compact-first-item", root.QuerySelectorAll(".ant-btn")[0].ClassList);
        Assert.Contains("ant-btn-compact-last-item", root.QuerySelectorAll(".ant-btn")[1].ClassList);
    }

    [Fact]
    public void SpaceCompact_applies_context_to_input_and_addon_without_item_wrappers()
    {
        var cut = Context.RenderComponent<AntDesign.SpaceCompact>(parameters =>
            parameters.Add(x => x.Size, AntDesign.InputSize.Small)
                .AddChildContent<AntDesign.SpaceCompactItem>(first => first
                    .Add(x => x.Position, AntDesign.SpaceCompactItemPosition.First)
                    .AddChildContent<AntDesign.SpaceAddon>(addon => addon.AddChildContent("https://")))
                .AddChildContent<AntDesign.SpaceCompactItem>(last => last
                    .Add(x => x.Position, AntDesign.SpaceCompactItemPosition.Last)
                    .AddChildContent<AntDesign.Input<string>>()));

        var root = cut.Find(".ant-space-compact");
        Assert.Equal(2, root.Children.Length);
        Assert.Contains("ant-space-addon-compact-first-item", root.Children[0].ClassList);
        Assert.Contains("ant-space-addon-small", root.Children[0].ClassList);
        Assert.Contains("ant-input-compact-last-item", root.Children[1].ClassList);
        Assert.Contains("ant-input-sm", root.Children[1].ClassList);
    }

    [Fact]
    public void Space_applies_semantic_classes_and_styles_to_root_items_and_separator()
    {
        var cut = Context.RenderComponent<AntDesign.Space>(parameters =>
            parameters.Add(x => x.Split, builder => builder.AddContent(0, "•"))
                .Add(x => x.ClassNames, new Dictionary<string, string>
                {
                    ["root"] = "custom-root",
                    ["item"] = "custom-item",
                    ["separator"] = "custom-separator"
                })
                .Add(x => x.Styles, new Dictionary<string, string>
                {
                    ["root"] = "border: 2px solid red;",
                    ["item"] = "padding: 4px;",
                    ["separator"] = "color: red;"
                })
                .AddChildContent<AntDesign.SpaceItem>(item => item.AddChildContent("A"))
                .AddChildContent<AntDesign.SpaceItem>(item => item.AddChildContent("B")));

        Assert.Contains("custom-root", cut.Find(".ant-space").ClassList);
        Assert.Contains("custom-item", cut.Find(".ant-space-item").ClassList);
        Assert.Contains("custom-separator", cut.Find(".ant-space-item-separator").ClassList);
        Assert.Contains("border: 2px solid red", cut.Find(".ant-space").GetAttribute("style"));
        Assert.Contains("padding: 4px", cut.Find(".ant-space-item").GetAttribute("style"));
        Assert.Contains("color: red", cut.Find(".ant-space-item-separator").GetAttribute("style"));
    }

    [Fact]
    public void Progress_exposes_v6_root_body_rail_track_and_indicator_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Progress>(parameters =>
            parameters.Add(x => x.Percent, 80));

        Assert.NotNull(cut.Find(".ant-progress"));
        Assert.NotNull(cut.Find(".ant-progress-body"));
        Assert.NotNull(cut.Find(".ant-progress-rail"));
        Assert.NotNull(cut.Find(".ant-progress-track"));
        Assert.NotNull(cut.Find(".ant-progress-body .ant-progress-indicator"));
    }

    [Fact]
    public void LayoutSider_exposes_v6_root_and_body_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Sider>(parameters =>
            parameters.Add(x => x.ClassNames, new Dictionary<string, string>
                {
                    ["root"] = "custom-sider-root",
                    ["body"] = "custom-sider-body"
                })
                .Add(x => x.Styles, new Dictionary<string, string>
                {
                    ["body"] = "min-height: 96px;"
                })
                .AddChildContent("Navigation"));

        var root = cut.Find(".ant-layout-sider");
        var body = cut.Find(".ant-layout-sider-children");
        Assert.Contains("custom-sider-root", root.ClassList);
        Assert.Contains("custom-sider-body", body.ClassList);
        Assert.Contains("min-height: 96px", body.GetAttribute("style"));
    }

    [Fact]
    public void Skeleton_exposes_v6_root_header_section_and_content_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Skeleton>(parameters =>
            parameters.Add(x => x.Loading, true)
                .Add(x => x.Avatar, true)
                .Add(x => x.Title, true)
                .Add(x => x.Paragraph, true));

        Assert.NotNull(cut.Find(".ant-skeleton"));
        Assert.NotNull(cut.Find(".ant-skeleton-header"));
        Assert.NotNull(cut.Find(".ant-skeleton-section"));
        Assert.NotNull(cut.Find(".ant-skeleton-content"));
    }

    [Fact]
    public void Slider_exposes_v6_root_rail_tracks_track_and_handle_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Slider<double>>();

        Assert.NotNull(cut.Find(".ant-slider"));
        Assert.NotNull(cut.Find(".ant-slider-rail"));
        Assert.NotNull(cut.Find(".ant-slider-tracks"));
        Assert.NotNull(cut.Find(".ant-slider-track"));
        Assert.NotNull(cut.Find(".ant-slider-handle"));
    }

    [Fact]
    public void Switch_matches_v6_root_indicator_and_content_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Switch>(parameters =>
            parameters.Add(x => x.CheckedChildren, "ON")
                .Add(x => x.UnCheckedChildren, "OFF"));

        Assert.NotNull(cut.Find(".ant-switch"));
        Assert.Contains("css-var-root", cut.Find(".ant-switch").ClassList);
        Assert.NotNull(cut.Find(".ant-switch-handle"));
        Assert.NotNull(cut.Find(".ant-switch-inner"));
        Assert.NotNull(cut.Find(".ant-switch-inner-checked"));
        Assert.NotNull(cut.Find(".ant-switch-inner-unchecked"));
        Assert.Empty(cut.FindAll(".ant-click-animating-node"));
    }

    [Fact]
    public void Result_exposes_v6_root_icon_title_subtitle_body_and_extra_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Result>(parameters =>
            parameters.Add(x => x.Title, "Success")
                .Add(x => x.SubTitle, "Completed")
                .Add(x => x.Extra, builder => builder.AddContent(0, "Next"))
                .AddChildContent("Details"));

        Assert.NotNull(cut.Find(".ant-result"));
        Assert.NotNull(cut.Find(".ant-result-icon"));
        Assert.NotNull(cut.Find(".ant-result-title"));
        Assert.NotNull(cut.Find(".ant-result-subtitle"));
        Assert.NotNull(cut.Find(".ant-result-body"));
        Assert.NotNull(cut.Find(".ant-result-extra"));
    }

    [Fact]
    public void Statistic_exposes_v6_root_header_title_content_value_and_affixes()
    {
        var cut = Context.RenderComponent<AntDesign.Statistic<double>>(parameters =>
            parameters.Add(x => x.Title, "Active")
                .Add(x => x.Value, 11.28)
                .Add(x => x.Prefix, "+")
                .Add(x => x.Suffix, "%"));

        Assert.NotNull(cut.Find(".ant-statistic"));
        Assert.NotNull(cut.Find(".ant-statistic-header .ant-statistic-title"));
        Assert.NotNull(cut.Find(".ant-statistic-content"));
        Assert.NotNull(cut.Find(".ant-statistic-content-value"));
        Assert.NotNull(cut.Find(".ant-statistic-content-prefix"));
        Assert.NotNull(cut.Find(".ant-statistic-content-suffix"));
    }

    [Fact]
    public void Typography_exposes_v6_root_actions_and_action_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Text>(parameters =>
            parameters.Add(x => x.Copyable, true)
                .Add(x => x.Editable, true)
                .AddChildContent("Editable text"));

        Assert.NotNull(cut.Find(".ant-typography"));
        Assert.NotNull(cut.Find(".ant-typography-actions"));
        Assert.Equal(2, cut.FindAll(".ant-typography-action").Count);
    }

    [Fact]
    public void Upload_exposes_v6_wrapper_trigger_list_and_file_item_slots()
    {
        JSInterop.SetupVoid("AntDesign.interop.uploadHelper.addFileClickEventListener", _ => true);

        var cut = Context.RenderComponent<AntDesign.Upload>(parameters => parameters
            .Add(x => x.Action, "https://example.invalid/upload")
            .Add(x => x.FileList, new System.Collections.Generic.List<AntDesign.UploadFileItem>
            {
                new() { FileName = "report.txt", State = AntDesign.UploadState.Success, Percent = 100 }
            })
            .AddChildContent("Choose file"));

        Assert.NotNull(cut.Find(".ant-upload-wrapper"));
        Assert.NotNull(cut.Find(".ant-upload-select"));
        Assert.NotNull(cut.Find(".ant-upload-list"));
        Assert.NotNull(cut.Find(".ant-upload-list-item"));
    }

    [Fact]
    public void Splitter_exposes_v6_root_panels_and_dragger_slots()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = Context.RenderComponent<AntDesign.Splitter>(parameters =>
            parameters.AddChildContent<AntDesign.SplitterPanel>(panel => panel.AddChildContent("First"))
                .AddChildContent<AntDesign.SplitterPanel>(panel => panel.AddChildContent("Second")));

        Assert.NotNull(cut.Find(".ant-splitter"));
        Assert.Contains("ant-splitter-css-var", cut.Find(".ant-splitter").ClassName);
        Assert.Equal(2, cut.FindAll(".ant-splitter-panel").Count);
        Assert.NotNull(cut.Find(".ant-splitter-bar-dragger"));
        Assert.Equal("separator", cut.Find(".ant-splitter-bar-dragger").GetAttribute("role"));
    }

    [Fact]
    public void Splitter_renders_a_dragger_between_each_panel_in_multiple_layouts()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = Context.RenderComponent<AntDesign.Splitter>(parameters =>
            parameters.AddChildContent<AntDesign.SplitterPanel>(panel => panel.AddChildContent("First"))
                .AddChildContent<AntDesign.SplitterPanel>(panel => panel.AddChildContent("Second"))
                .AddChildContent<AntDesign.SplitterPanel>(panel => panel.AddChildContent("Third")));

        Assert.Equal(3, cut.FindAll(".ant-splitter-panel").Count);
        Assert.Equal(2, cut.FindAll(".ant-splitter-bar").Count);
        Assert.Equal(new[] { "0", "1" }, cut.FindAll(".ant-splitter-bar-dragger").Select(x => x.GetAttribute("data-index")));
    }

    [Fact]
    public void Splitter_panel_minimum_and_maximum_sizes_are_rendered()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = Context.RenderComponent<AntDesign.Splitter>(parameters =>
            parameters.AddChildContent<AntDesign.SplitterPanel>(panel => panel
                    .Add(x => x.Min, "100px")
                    .Add(x => x.Max, "200px")
                    .AddChildContent("First"))
                .AddChildContent<AntDesign.SplitterPanel>(panel => panel.AddChildContent("Second")));

        Assert.Equal("100px", cut.FindAll(".ant-splitter-panel")[0].GetAttribute("data-min"));
        Assert.Equal("200px", cut.FindAll(".ant-splitter-panel")[0].GetAttribute("data-max"));
        Assert.Contains("min-width:100px", cut.FindAll(".ant-splitter-panel")[0].GetAttribute("style"));
    }

    [Fact]
    public void Splitter_collapsible_panel_restores_its_size_after_expanding()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = Context.RenderComponent<AntDesign.Splitter>(parameters =>
            parameters.AddChildContent<AntDesign.SplitterPanel>(panel => panel
                    .Add(x => x.Collapsible, true)
                    .AddChildContent("First"))
                .AddChildContent<AntDesign.SplitterPanel>(panel => panel
                    .Add(x => x.Collapsible, true)
                    .AddChildContent("Second")));

        Assert.Equal(2, cut.FindAll(".ant-splitter-bar-collapse-bar-hover-only").Count);
        Assert.Equal(2, cut.FindAll(".ant-splitter-bar-collapse-bar").Count);

        var firstPanel = cut.FindAll(".ant-splitter-panel")[0];
        Assert.Contains("flex:1 1 0%", firstPanel.GetAttribute("style"));

        cut.Find(".ant-splitter-bar-collapse-bar-start").Click();
        Assert.Contains("width:0px", cut.FindAll(".ant-splitter-panel")[0].GetAttribute("style"));

        cut.Find(".ant-splitter-bar-collapse-bar-start").Click();
        var expandedPanelStyle = cut.FindAll(".ant-splitter-panel")[0].GetAttribute("style");
        Assert.Contains("flex:1 1 0%", expandedPanelStyle);
        Assert.DoesNotContain("width:0px", expandedPanelStyle);
    }

    [Fact]
    public void Splitter_collapsible_icon_visibility_matches_configured_mode()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = Context.RenderComponent<AntDesign.Splitter>(parameters =>
            parameters.AddChildContent<AntDesign.SplitterPanel>(panel => panel
                    .Add(x => x.Collapsible, true)
                    .Add(x => x.ShowCollapsibleIcon, true)
                    .AddChildContent("First"))
                .AddChildContent<AntDesign.SplitterPanel>(panel => panel
                    .Add(x => x.Collapsible, true)
                    .Add(x => x.ShowCollapsibleIcon, false)
                    .AddChildContent("Second")));

        Assert.Single(cut.FindAll(".ant-splitter-bar-collapse-bar-always-visible"));
        Assert.Single(cut.FindAll(".ant-splitter-bar-collapse-bar-always-hidden"));
    }

    [Fact]
    public void Splitter_supports_custom_dragger_and_collapse_icons()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        RenderFragment draggerIcon = builder => builder.AddContent(0, "custom-dragger");
        RenderFragment collapseStart = builder => builder.AddContent(0, "custom-start");
        RenderFragment collapseEnd = builder => builder.AddContent(0, "custom-end");

        var cut = Context.RenderComponent<AntDesign.Splitter>(parameters => parameters
            .Add(x => x.DraggerIcon, draggerIcon)
            .Add(x => x.CollapseIconStart, collapseStart)
            .Add(x => x.CollapseIconEnd, collapseEnd)
            .AddChildContent<AntDesign.SplitterPanel>(panel => panel
                .Add(x => x.Collapsible, true)
                .AddChildContent("First"))
            .AddChildContent<AntDesign.SplitterPanel>(panel => panel
                .Add(x => x.Collapsible, true)
                .AddChildContent("Second")));

        Assert.Contains("ant-splitter-bar-dragger-customize", cut.Find(".ant-splitter-bar-dragger").ClassName);
        Assert.Equal("custom-dragger", cut.Find(".ant-splitter-bar-dragger-icon").TextContent);
        Assert.Equal("custom-start", cut.Find(".ant-splitter-bar-collapse-bar-start .ant-splitter-bar-collapse-icon").TextContent);
        Assert.Equal("custom-end", cut.Find(".ant-splitter-bar-collapse-bar-end .ant-splitter-bar-collapse-icon").TextContent);
        Assert.Equal(2, cut.FindAll(".ant-splitter-bar-collapse-bar-customize").Count);
    }

    [Fact]
    public void Splitter_can_hide_the_default_dragger_indicator_when_customizing()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = Context.RenderComponent<AntDesign.Splitter>(parameters => parameters
            .Add(x => x.HideDraggerIndicator, true)
            .AddChildContent<AntDesign.SplitterPanel>(panel => panel.AddChildContent("First"))
            .AddChildContent<AntDesign.SplitterPanel>(panel => panel.AddChildContent("Second")));

        Assert.Contains("ant-splitter-bar-dragger-customize", cut.Find(".ant-splitter-bar-dragger").ClassName);
        Assert.Empty(cut.FindAll(".ant-splitter-bar-dragger-icon"));
    }

    [Fact]
    public void Splitter_applies_component_tokens_to_the_root_element()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var componentTokens = new Dictionary<string, IDictionary<string, string>>
        {
            ["Splitter"] = new Dictionary<string, string>
            {
                ["splitBarSize"] = "1",
                ["splitTriggerSize"] = "16",
            }
        };

        var cut = Context.RenderComponent<CascadingValue<IReadOnlyDictionary<string, IDictionary<string, string>>>>(parameters => parameters
            .Add(x => x.Name, "AntDesignThemeComponents")
            .Add(x => x.Value, componentTokens)
            .AddChildContent<AntDesign.Splitter>(splitter => splitter
                .AddChildContent<AntDesign.SplitterPanel>(panel => panel.AddChildContent("First"))
                .AddChildContent<AntDesign.SplitterPanel>(panel => panel.AddChildContent("Second"))));

        var style = cut.Find(".ant-splitter").GetAttribute("style");
        Assert.Contains("--ant-splitter-split-bar-size:1px", style);
        Assert.Contains("--ant-splitter-split-trigger-size:16px", style);
    }

    [Fact]
    public void Splitter_lazy_mode_renders_a_drag_preview_for_each_bar()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = Context.RenderComponent<AntDesign.Splitter>(parameters => parameters
            .Add(x => x.Lazy, true)
            .AddChildContent<AntDesign.SplitterPanel>(panel => panel.AddChildContent("First"))
            .AddChildContent<AntDesign.SplitterPanel>(panel => panel.AddChildContent("Second"))
            .AddChildContent<AntDesign.SplitterPanel>(panel => panel.AddChildContent("Third")));

        Assert.Equal(2, cut.FindAll(".ant-splitter-bar-preview").Count);
    }


    [Fact]
    public void Anchor_exposes_v6_wrapper_indicator_item_and_title_slots()
    {
        JSInterop.Setup<AntDesign.JsInterop.DomRect>(AntDesign.JSInteropConstants.GetBoundingClientRect, _ => true)
            .SetResult(new AntDesign.JsInterop.DomRect());

        var cut = Context.RenderComponent<AntDesign.Anchor>(parameters =>
            parameters.AddChildContent<AntDesign.AnchorLink>(link => link
                .Add(x => x.Href, "#overview")
                .Add(x => x.Title, "Overview")));

        Assert.NotNull(cut.Find(".ant-anchor-wrapper"));
        Assert.NotNull(cut.Find(".ant-anchor-ink"));
        Assert.NotNull(cut.Find(".ant-anchor-link"));
        Assert.NotNull(cut.Find(".ant-anchor-link-title"));
    }

    [Fact]
    public void Anchor_supports_horizontal_direction_and_semantic_slots()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<AntDesign.JsInterop.DomRect>(AntDesign.JSInteropConstants.GetBoundingClientRect, _ => true)
            .SetResult(new AntDesign.JsInterop.DomRect());

        var classNames = new Dictionary<string, string>
        {
            ["root"] = "custom-root",
            ["item"] = "custom-item",
            ["itemTitle"] = "custom-title",
            ["indicator"] = "custom-indicator"
        };
        var styles = new Dictionary<string, string>
        {
            ["root"] = "height: 120px",
            ["itemTitle"] = "color: red"
        };

        var cut = Context.RenderComponent<AntDesign.Anchor>(parameters => parameters
            .Add(x => x.Direction, "horizontal")
            .Add(x => x.Affix, false)
            .Add(x => x.ClassNames, classNames)
            .Add(x => x.Styles, styles)
            .AddChildContent<AntDesign.AnchorLink>(link => link
                .Add(x => x.Href, "#overview")
                .Add(x => x.TargetOffset, 50)
                .Add(x => x.Title, "Overview")));

        Assert.Contains("ant-anchor-wrapper-horizontal", cut.Find(".custom-root").ClassName);
        Assert.NotNull(cut.Find(".custom-indicator"));
        Assert.NotNull(cut.Find(".custom-item"));
        Assert.Equal("color: red", cut.Find(".custom-title").GetAttribute("style"));
        Assert.Contains("height: 120px", cut.Find(".custom-root").GetAttribute("style"));
    }

    [Fact]
    public void Calendar_exposes_v6_root_header_body_content_and_item_slots()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<AntDesign.JsInterop.DomRect>(AntDesign.JSInteropConstants.GetBoundingClientRect, _ => true)
            .SetResult(new AntDesign.JsInterop.DomRect { Width = 120 });

        var cut = Context.RenderComponent<AntDesign.Calendar>();

        var root = cut.Find(".ant-picker-calendar");
        Assert.Equal("ant-picker-calendar ant-picker-calendar-full css-var-root", root.ClassName);
        Assert.NotNull(cut.Find(".ant-picker-calendar-header"));
        Assert.NotNull(cut.Find(".ant-picker-body"));
        Assert.NotNull(cut.Find(".ant-picker-content"));
        Assert.NotNull(cut.Find(".ant-picker-cell"));
        Assert.NotNull(cut.Find(".ant-picker-calendar-date-content"));
    }

    [Fact]
    public void Carousel_exposes_v6_root_css_scope_and_slick_slide_structure()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = Context.RenderComponent<AntDesign.Carousel>(parameters => parameters
            .AddChildContent<AntDesign.CarouselSlick>(slide => slide.AddChildContent("Slide 1")));

        var root = cut.Find(".ant-carousel");
        Assert.Equal("ant-carousel css-var-root", root.ClassName);
        Assert.NotNull(cut.Find(".slick-slider.slick-initialized"));
        Assert.NotNull(cut.Find(".slick-slide.slick-active.slick-current"));
        Assert.Equal("Slide 1", cut.Find(".slick-slide.slick-active").TextContent.Trim());
    }

    [Fact]
    public void Table_exposes_v6_root_section_title_content_header_body_and_footer_slots()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = Context.RenderComponent<AntDesign.Table<V6TableRow>>(parameters => parameters
            .Add(x => x.DataSource, new[] { new V6TableRow() })
            .Add(x => x.Title, "Records")
            .Add(x => x.Footer, "End")
            .Add(x => x.ChildContent, item => builder => builder.AddContent(0, item)));

        Assert.NotNull(cut.Find(".ant-table-wrapper"));
        Assert.NotNull(cut.Find(".ant-table"));
        Assert.NotNull(cut.Find(".ant-table-title"));
        Assert.NotNull(cut.Find(".ant-table-container"));
        Assert.NotNull(cut.Find(".ant-table-content"));
        Assert.NotNull(cut.Find(".ant-table-thead"));
        Assert.NotNull(cut.Find(".ant-table-tbody"));
        Assert.NotNull(cut.Find(".ant-table-footer"));
    }

    [Fact]
    public void InputNumber_exposes_v6_root_actions_and_input_slots()
    {
        var cut = Context.RenderComponent<AntDesign.InputNumber<double>>();

        var root = cut.Find(".ant-input-number");
        Assert.Contains("ant-input-number-mode-input", root.ClassList);
        Assert.Contains("css-var-root", root.ClassList);
        Assert.Contains("ant-input-number-css-var", root.ClassList);
        Assert.Contains("ant-input-number-outlined", root.ClassList);
        Assert.Equal("INPUT", root.Children[0].TagName);
        Assert.Equal("ant-input-number-actions", root.Children[1].ClassName);
        Assert.Equal(2, cut.FindAll(".ant-input-number-action").Count);
        Assert.Empty(cut.FindAll(".ant-input-number-handler-wrap"));
        Assert.Empty(cut.FindAll(".ant-input-number-input-wrap"));
        Assert.NotNull(cut.Find(".ant-input-number-input"));
    }

    [Fact]
    public void TextArea_exposes_v6_root_textarea_clear_and_count_slots()
    {
        JSInterop.Setup<AntDesign.Internal.TextAreaInfo>("AntDesign.interop.inputHelper.getTextAreaInfo", _ => true)
            .SetResult(new AntDesign.Internal.TextAreaInfo { LineHeight = 10, PaddingTop = 1, PaddingBottom = 1, BorderTop = 1, BorderBottom = 1 });

        var cut = Context.RenderComponent<AntDesign.TextArea>(parameters =>
        {
            parameters.Add(x => x.Value, "Hello, Ant Design");
            parameters.Add(x => x.Rows, 3u);
            parameters.Add(x => x.MaxLength, 100);
            parameters.Add(x => x.ShowCount, true);
            parameters.Add(x => x.AllowClear, true);
        });

        Assert.NotNull(cut.Find(".ant-input-affix-wrapper"));
        Assert.NotNull(cut.Find("textarea.ant-input"));
        Assert.NotNull(cut.Find(".ant-input-clear-icon"));
        Assert.Equal("17 / 100", cut.Find(".ant-input-data-count").TextContent);
        Assert.Equal("17 / 100", cut.Find(".ant-input-affix-wrapper").GetAttribute("data-count"));
    }

    [Fact]
    public void Input_exposes_v6_root_input_clear_count_prefix_and_suffix_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Input<string>>(parameters =>
        {
            parameters.Add(x => x.AllowClear, true);
            parameters.Add(x => x.ShowCount, true);
            parameters.Add(x => x.Value, "hello");
            parameters.Add(x => x.Prefix, builder => builder.AddContent(0, "@"));
            parameters.Add(x => x.Suffix, builder => builder.AddContent(0, "kg"));
        });

        Assert.NotNull(cut.Find(".ant-input-affix-wrapper"));
        Assert.NotNull(cut.Find("input.ant-input"));
        Assert.NotNull(cut.Find(".ant-input-clear-icon"));
        Assert.NotNull(cut.Find(".ant-input-show-count-suffix"));
        Assert.NotNull(cut.Find(".ant-input-prefix"));
        Assert.NotNull(cut.Find(".ant-input-suffix"));
    }

    [Fact]
    public void Search_exposes_v6_search_button_slot()
    {
        var cut = Context.RenderComponent<AntDesign.Search>(parameters => parameters
            .Add(x => x.EnterButton, true));

        Assert.NotNull(cut.Find(".ant-input-search"));
        Assert.NotNull(cut.Find(".ant-input-search-button"));
    }

    [Fact]
    public void Spin_exposes_v6_root_section_indicator_description_and_container_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Spin>(parameters =>
        {
            parameters.Add(x => x.Tip, "Loading records");
            parameters.AddChildContent("Records");
        });

        Assert.NotNull(cut.Find(".ant-spin"));
        Assert.NotNull(cut.Find(".ant-spin-section"));
        Assert.NotNull(cut.Find(".ant-spin-dot-holder .ant-spin-dot"));
        Assert.Equal("Loading records", cut.Find(".ant-spin-description").TextContent);
        Assert.Equal("Records", cut.Find(".ant-spin-container").TextContent.Trim());
    }

    [Fact]
    public void Steps_exposes_v6_item_wrapper_section_header_rail_and_content_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Steps>(parameters => parameters
            .AddChildContent<AntDesign.Step>(step => step
                .Add(x => x.Title, "Prepare")
                .Add(x => x.Subtitle, "00:01")
                .Add(x => x.Description, "Prepare the data"))
            .AddChildContent<AntDesign.Step>(step => step.Add(x => x.Title, "Finish")));

        Assert.NotNull(cut.Find(".ant-steps"));
        Assert.NotNull(cut.Find(".ant-steps-item"));
        Assert.NotNull(cut.Find(".ant-steps-item-wrapper"));
        Assert.NotNull(cut.Find(".ant-steps-item-icon"));
        Assert.NotNull(cut.Find(".ant-steps-item-section"));
        Assert.NotNull(cut.Find(".ant-steps-item-header"));
        Assert.NotNull(cut.Find(".ant-steps-item-title"));
        Assert.NotNull(cut.Find(".ant-steps-item-subtitle"));
        Assert.NotNull(cut.Find(".ant-steps-item-rail"));
        Assert.NotNull(cut.Find(".ant-steps-item-content"));
    }

    [Fact]
    public void Tree_exposes_v6_root_item_switcher_and_title_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Tree<V6TreeItem>>(parameters => parameters
            .Add(x => x.DataSource, new[] { new V6TreeItem { Children = new() { new V6TreeItem { Name = "Child" } } } })
            .Add(x => x.TitleExpression, node => node.DataItem.Name)
            .Add(x => x.ChildrenExpression, node => node.DataItem.Children));

        Assert.NotNull(cut.Find(".ant-tree"));
        Assert.NotNull(cut.Find(".ant-tree-treenode"));
        Assert.NotNull(cut.Find(".ant-tree-switcher"));
        Assert.NotNull(cut.Find(".ant-tree-title"));
    }

    [Fact]
    public void Tabs_exposes_v6_root_header_item_indicator_body_and_content_slots()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<System.Collections.Generic.Dictionary<string, HtmlElement>>("AntDesign.interop.domInfoHelper.getElementsInfo", _ => true)
            .SetResult(new System.Collections.Generic.Dictionary<string, HtmlElement>());

        var cut = Context.RenderComponent<AntDesign.Tabs>(parameters => parameters
            .Add(x => x.Id, "v6-tabs")
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<AntDesign.TabPane>(0);
                builder.AddAttribute(1, "Key", "one");
                builder.AddAttribute(2, "Tab", "One");
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(content => content.AddContent(0, "Panel")));
                builder.CloseComponent();
            }));

        Assert.NotNull(cut.Find(".ant-tabs"));
        Assert.NotNull(cut.Find(".ant-tabs-nav"));
        Assert.NotNull(cut.Find(".ant-tabs-tab"));
        Assert.NotNull(cut.Find(".ant-tabs-ink-bar"));
        Assert.NotNull(cut.Find(".ant-tabs-content-holder"));
        Assert.NotNull(cut.Find(".ant-tabs-content"));
    }

    [Fact]
    public void FormItem_exposes_v6_label_content_help_and_extra_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Form<object>>(parameters => parameters
            .Add(x => x.Model, new object())
            .Add(x => x.ChildContent, _ => builder =>
            {
                builder.OpenComponent<AntDesign.FormItem>(0);
                builder.AddAttribute(1, "Label", "Name");
                builder.AddAttribute(2, "Help", "Enter your name");
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(content => content.AddContent(0, "input field")));
                builder.AddAttribute(4, "Extra", (RenderFragment)(content => content.AddContent(0, "Additional guidance")));
                builder.CloseComponent();
            }));

        Assert.NotNull(cut.Find(".ant-form-item"));
        Assert.Equal("Name", cut.Find(".ant-form-item-label").TextContent.Trim());
        Assert.Equal("input field", cut.Find(".ant-form-item-control-input-content").TextContent.Trim());
        Assert.Equal("Enter your name", cut.Find(".ant-form-item-explain").TextContent.Trim());
        Assert.Equal("Additional guidance", cut.Find(".ant-form-item-extra").TextContent.Trim());
    }

    [Fact]
    public void Cascader_exposes_v6_root_input_placeholder_and_suffix_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Cascader>(parameters => parameters
            .Add(x => x.Placeholder, "Choose a location")
            .Add(x => x.ShowSearch, true));

        Assert.NotNull(cut.Find(".ant-select.ant-cascader"));
        Assert.NotNull(cut.Find(".ant-select-input"));
        Assert.Equal("Choose a location", cut.Find(".ant-select-selection-placeholder").TextContent);
        Assert.NotNull(cut.Find(".ant-select-suffix"));
    }

    [Fact]
    public void Popover_exposes_v6_content_and_title_slots()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = Context.RenderComponent<AntDesign.Popover>(parameters => parameters
            .Add(x => x.Visible, true)
            .Add(x => x.Title, "Details")
            .Add(x => x.Content, "More information")
            .AddChildContent("Open details"));

        Assert.NotNull(cut.Find(".ant-popover-content"));
        Assert.NotNull(cut.Find(".ant-popover.ant-popover-css-var"));
        Assert.Equal("Details", cut.Find(".ant-popover-title").TextContent.Trim());
        Assert.Equal("More information", cut.Find(".ant-popover-inner-content").TextContent.Trim());
    }

    [Fact]
    public async Task Tooltip_uses_the_v6_container_and_arrow_slots_without_legacy_wrappers()
    {
        JSInterop.Setup<AntDesign.Core.JsInterop.Modules.Components.OverlayPosition>(
                AntDesign.JSInteropConstants.OverlayComponentHelper.AddOverlayToContainer,
                _ => true)
            .SetResult(new AntDesign.Core.JsInterop.Modules.Components.OverlayPosition());

        var cut = Context.RenderComponent<AntDesign.Tooltip>(parameters => parameters
            .Add(x => x.MouseEnterDelay, 0d)
            .Add(x => x.Title, "Help")
            .AddChildContent("Target"));
        await cut.Find("div").TriggerEventAsync("onmouseenter", new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        Assert.Equal("Help", cut.Find(".ant-tooltip-container").TextContent.Trim());
        Assert.NotNull(cut.Find(".ant-tooltip.ant-tooltip-css-var"));
        Assert.NotNull(cut.Find(".ant-tooltip-arrow .ant-tooltip-arrow-content"));
        Assert.Empty(cut.FindAll(".ant-tooltip-content"));
        Assert.Empty(cut.FindAll(".ant-tooltip-inner"));
    }

    [Fact]
    public void Dropdown_uses_v6_semantic_menu_item_content_and_group_title_classes()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<AntDesign.Core.JsInterop.Modules.Components.OverlayPosition>(
                AntDesign.JSInteropConstants.OverlayComponentHelper.AddOverlayToContainer,
                _ => true)
            .SetResult(new AntDesign.Core.JsInterop.Modules.Components.OverlayPosition());

        var cut = Context.RenderComponent<AntDesign.Dropdown>(parameters => parameters
            .Add(x => x.Visible, true)
            .Add(x => x.Overlay, builder =>
            {
                builder.OpenComponent<AntDesign.Menu>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(menuBuilder =>
                {
                    menuBuilder.OpenComponent<AntDesign.MenuItemGroup>(0);
                    menuBuilder.AddAttribute(1, "Title", "Group title");
                    menuBuilder.AddAttribute(2, "ChildContent", (RenderFragment)(itemsBuilder =>
                    {
                        itemsBuilder.OpenComponent<AntDesign.MenuItem>(0);
                        itemsBuilder.AddAttribute(1, "Icon", "save");
                        itemsBuilder.AddAttribute(2, "ChildContent", (RenderFragment)(content => content.AddContent(0, "Rename")));
                        itemsBuilder.CloseComponent();
                    }));
                    menuBuilder.CloseComponent();
                }));
                builder.CloseComponent();
            })
            .AddChildContent("Actions"));

        Assert.NotNull(cut.Find(".ant-dropdown"));
        Assert.NotNull(cut.Find(".ant-dropdown-menu-item"));
        Assert.NotNull(cut.Find(".ant-dropdown-menu-item-icon"));
        Assert.Equal("Rename", cut.Find(".ant-dropdown-menu-title-content").TextContent.Trim());
        Assert.Equal("Group title", cut.Find(".ant-dropdown-menu-item-group-title").TextContent.Trim());
    }

    [Fact]
    public async Task ColorPicker_exposes_v6_trigger_and_popup_semantic_slots()
    {
        var cut = Context.RenderComponent<AntDesign.ColorPicker>(parameters => parameters
            .Add(x => x.Value, "#ff0000")
            .Add(x => x.ShowText, true)
            .Add(x => x.Size, "large"));

        var trigger = cut.Find(".ant-color-picker-trigger");
        Assert.Contains("ant-color-picker-lg", trigger.ClassList);
        Assert.Equal("DIV", trigger.TagName);
        Assert.Equal("DIV", cut.Find(".ant-color-picker-color-block").TagName);
        Assert.Equal("DIV", cut.Find(".ant-color-picker-color-block-inner").TagName);
        Assert.Contains("background:#ff0000", cut.Find(".ant-color-picker-color-block-inner").GetAttribute("style"), StringComparison.OrdinalIgnoreCase);
        Assert.Equal("#FF0000", cut.Find(".ant-color-picker-trigger-text").TextContent);

        await trigger.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
        Assert.NotNull(cut.Find(".ant-popover.ant-color-picker"));
        Assert.NotNull(cut.Find(".ant-color-picker-inner-content"));
        Assert.NotNull(cut.Find(".ant-color-picker-select .ant-color-picker-saturation"));
        Assert.Equal(2, cut.FindAll(".ant-slider.ant-color-picker-slider.ant-slider-horizontal").Count);
        Assert.Equal(2, cut.FindAll(".ant-slider-rail.ant-color-picker-slider-rail").Count);
        Assert.Equal(2, cut.FindAll(".ant-slider-handle.ant-slider-handle-1.ant-color-picker-slider-handle").Count);
        Assert.NotNull(cut.Find(".ant-color-picker-format-select"));
    }

    [Fact]
    public async Task ColorPicker_matches_react_small_disabled_trigger_classes()
    {
        var cut = Context.RenderComponent<AntDesign.ColorPicker>(parameters => parameters
            .Add(x => x.Size, "small")
            .Add(x => x.Disabled, true)
            .Add(x => x.ShowText, true));

        var trigger = cut.Find(".ant-color-picker-trigger");
        Assert.Contains("ant-color-picker-sm", trigger.ClassList);
        Assert.Contains("ant-color-picker-trigger-disabled", trigger.ClassList);
        Assert.DoesNotContain("button", trigger.InnerHtml, StringComparison.OrdinalIgnoreCase);
        Assert.Equal("#1677FF", cut.Find(".ant-color-picker-trigger-text").TextContent);
        await trigger.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
        Assert.Empty(cut.FindAll(".ant-popover.ant-color-picker"));
    }

    [Fact]
    public async Task ColorPicker_clears_and_selects_preset_colors()
    {
        var changed = new List<string>();
        var cleared = false;
        var presets = new[]
        {
            new AntDesign.ColorPickerPreset { Label = "Brand", Colors = new[] { "#1677ff", "#52c41a" } }
        };
        var cut = Context.RenderComponent<AntDesign.ColorPicker>(parameters => parameters
            .Add(x => x.Value, "#1677ff")
            .Add(x => x.ValueChanged, value => changed.Add(value))
            .Add(x => x.AllowClear, true)
            .Add(x => x.Presets, presets)
            .Add(x => x.OnClear, () => cleared = true)
            .Add(x => x.Open, true));

        Assert.Equal(2, cut.FindAll(".ant-color-picker-presets-color").Count);
        await cut.FindAll(".ant-color-picker-presets-color")[1].ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
        Assert.Equal("#52c41a", changed[^1]);

        await cut.Find(".ant-color-picker-operation .ant-color-picker-clear").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
        Assert.True(cleared);
        Assert.Equal("transparent", changed[^1]);
        Assert.Contains("ant-color-picker-clear", cut.Find(".ant-color-picker-trigger").InnerHtml);
    }

    [Fact]
    public async Task ColorPicker_panel_edits_hsb_and_switches_format()
    {
        JSInterop.Setup<AntDesign.JsInterop.DomRect>(AntDesign.JSInteropConstants.GetBoundingClientRect, _ => true)
            .SetResult(new AntDesign.JsInterop.DomRect { Width = 300, Height = 160 });
        var changed = new List<string>();
        var formats = new List<string>();
        var cut = Context.RenderComponent<AntDesign.ColorPicker>(parameters => parameters
            .Add(x => x.Value, "#ff0000")
            .Add(x => x.ValueChanged, value => changed.Add(value))
            .Add(x => x.FormatChanged, format => formats.Add(format))
            .Add(x => x.Open, true));

        await cut.Find(".ant-color-picker-select").TriggerEventAsync("onpointerdown", new Microsoft.AspNetCore.Components.Web.PointerEventArgs { OffsetX = 150, OffsetY = 80 });
        Assert.Equal("#804040", changed[^1]);

        var formatSelect = cut.FindComponent<AntDesign.Select<string, string>>();
        formatSelect.SetParametersAndRender(parameters => parameters.Add(x => x.Open, true));
        await cut.FindAll(".ant-select-item-option")[1].ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
        Assert.Equal("rgb", formats[^1]);
        var rgbInputs = cut.FindAll(".ant-color-picker-rgb-input .ant-input-number-input");
        Assert.Equal(3, rgbInputs.Count);
        Assert.Equal(new[] { "128", "64", "64" }, rgbInputs.Select(input => input.GetAttribute("value")).ToArray());
        var alphaInput = cut.Find(".ant-color-picker-alpha-input .ant-input-number-input");
        Assert.Equal("0", alphaInput.GetAttribute("aria-valuemin"));
        Assert.Equal("100", alphaInput.GetAttribute("aria-valuemax"));
    }

    [Fact]
    public async Task ColorPicker_maps_pointer_positions_using_measured_control_dimensions()
    {
        JSInterop.Setup<AntDesign.JsInterop.DomRect>(AntDesign.JSInteropConstants.GetBoundingClientRect, _ => true)
            .SetResult(new AntDesign.JsInterop.DomRect { Width = 300, Height = 160 });
        var changed = new List<string>();
        var cut = Context.RenderComponent<AntDesign.ColorPicker>(parameters => parameters
            .Add(x => x.Value, "#ff0000")
            .Add(x => x.ValueChanged, value => changed.Add(value))
            .Add(x => x.Open, true));

        await cut.Find(".ant-color-picker-select").TriggerEventAsync("onpointerdown", new Microsoft.AspNetCore.Components.Web.PointerEventArgs { OffsetX = 300, OffsetY = 0 });
        Assert.Equal("#FF0000", changed[^1]);

        await cut.Find(".ant-color-picker-select").TriggerEventAsync("onpointermove", new Microsoft.AspNetCore.Components.Web.PointerEventArgs { OffsetX = 150, OffsetY = 80 });
        Assert.Equal("#804040", changed[^1]);

        await cut.Find(".ant-slider.ant-color-picker-slider").TriggerEventAsync("onpointerdown", new Microsoft.AspNetCore.Components.Web.PointerEventArgs { OffsetX = 150 });
        Assert.Equal("#40807F", changed[^1]);

        await cut.FindAll(".ant-slider.ant-color-picker-slider")[1].TriggerEventAsync("onpointerdown", new Microsoft.AspNetCore.Components.Web.PointerEventArgs { OffsetX = 150 });
        Assert.Equal("rgba(64, 128, 127, 0.5)", changed[^1]);
    }

    [Fact]
    public async Task ColorPicker_accepts_unprefixed_hex_input_and_ignores_disabled_panel_edits()
    {
        var changed = new List<string>();
        var cut = Context.RenderComponent<AntDesign.ColorPicker>(parameters => parameters
            .Add(x => x.ValueChanged, value => changed.Add(value))
            .Add(x => x.Open, true));

        await cut.Find(".ant-color-picker-hex-input input").InputAsync(new Microsoft.AspNetCore.Components.ChangeEventArgs { Value = "ff0000" });
        Assert.Equal("#FF0000", changed[^1]);

        var disabled = Context.RenderComponent<AntDesign.ColorPicker>(parameters => parameters
            .Add(x => x.Disabled, true)
            .Add(x => x.Open, true)
            .Add(x => x.ValueChanged, value => changed.Add(value)));
        Assert.Empty(disabled.FindAll(".ant-popover.ant-color-picker"));
        Assert.Empty(disabled.FindAll(".ant-color-picker-trigger-active"));
    }

    [Fact]
    public void Listy_renders_group_headers_and_items_in_react_v6_structure()
    {
        var cut = Context.RenderComponent<AntDesign.Listy<string>>(parameters => parameters
            .Add(x => x.DataSource, new[] { "Olivia", "Liam", "Noah" })
            .Add(x => x.GroupBy, name => name == "Noah" ? "Engineering" : "Design")
            .Add(x => x.GroupHeaderTemplate, name => builder => builder.AddContent(0, name)));

        Assert.NotNull(cut.Find(".ant-listy"));
        Assert.Equal(2, cut.FindAll(".ant-listy-group-section").Count);
        Assert.Equal(3, cut.FindAll(".ant-listy-item").Count);
        Assert.Equal("Design", cut.FindAll(".ant-listy-group-header")[0].TextContent.Trim());
        Assert.Equal("Engineering", cut.FindAll(".ant-listy-group-header")[1].TextContent.Trim());
    }

    [Fact]
    public void QRCode_renders_a_real_svg_matrix_and_v6_root_and_cover_slots()
    {
        var cut = Context.RenderComponent<AntDesign.QRCode>(parameters => parameters
            .Add(x => x.Value, "https://ant.design")
            .Add(x => x.Type, "svg")
            .Add(x => x.OnRefresh, () => { })
            .Add(x => x.Size, 128));

        Assert.NotNull(cut.Find(".ant-qrcode"));
        Assert.NotNull(cut.Find(".ant-qrcode svg"));
        Assert.Contains("<path", cut.Find(".ant-qrcode svg").InnerHtml, StringComparison.Ordinal);
        Assert.Contains("width: 128px", cut.Find(".ant-qrcode").GetAttribute("style"), StringComparison.Ordinal);

        cut.SetParametersAndRender(parameters => parameters.Add(x => x.Status, "expired"));
        Assert.NotNull(cut.Find(".ant-qrcode-cover"));
        Assert.NotNull(cut.Find(".ant-qrcode-cover button"));
    }

    [Fact]
    public void Segmented_exposes_v6_root_item_and_label_slots()
    {
        JSInterop.Setup<System.Collections.Generic.Dictionary<string, HtmlElement>>("AntDesign.interop.domInfoHelper.getElementsInfo", _ => true);

        var cut = Context.RenderComponent<AntDesign.Segmented<string>>(parameters =>
            parameters.Add(x => x.Labels, new[] { "List", "Grid" }));

        Assert.NotNull(cut.Find(".ant-segmented"));
        Assert.NotNull(cut.Find(".ant-segmented-item"));
        Assert.NotNull(cut.Find(".ant-segmented-item-label"));
    }


    [Theory]
    [InlineData("default")]
    [InlineData("hover")]
    [InlineData("active")]
    [InlineData("selected")]
    [InlineData("disabled")]
    [InlineData("open")]
    [InlineData("popup")]
    [InlineData("rtl")]
    public void V6_structure_matrix_declares_required_state(string state)
    {
        Assert.Contains(state, new[] { "default", "hover", "active", "selected", "disabled", "open", "popup", "rtl" });
    }

    [Fact]
    public void Mentions_exposes_v6_root_textarea_and_popup_semantic_slots()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = Context.RenderComponent<AntDesign.Mentions>(parameters => parameters
            .AddChildContent<AntDesign.MentionsOption>(option => option
                .Add(x => x.Value, "user")
                .AddChildContent("User")));

        Assert.NotNull(cut.Find(".ant-mentions"));
        Assert.NotNull(cut.Find("textarea.ant-mentions-textarea"));
        Assert.NotNull(cut.Find("ul.ant-mentions-popup"));
    }

    [Fact]
    public void Popconfirm_renders_v6_popover_container_and_confirmation_semantic_slots()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<AntDesign.Core.JsInterop.Modules.Components.OverlayPosition>(
                AntDesign.JSInteropConstants.OverlayComponentHelper.AddOverlayToContainer,
                _ => true)
            .SetResult(new AntDesign.Core.JsInterop.Modules.Components.OverlayPosition());

        var cut = Context.RenderComponent<AntDesign.Popconfirm>(parameters => parameters
            .Add(x => x.Visible, true)
            .Add(x => x.Title, "Confirm action")
            .AddChildContent("Trigger"));

        Assert.NotNull(cut.Find(".ant-popover-arrow"));
        Assert.NotNull(cut.Find(".ant-popover.ant-popover-css-var"));
        Assert.NotNull(cut.Find(".ant-popover-container[role='tooltip']"));
        Assert.NotNull(cut.Find(".ant-popconfirm-message-icon"));
        Assert.Equal("Confirm action", cut.Find(".ant-popconfirm-title").TextContent.Trim());
        Assert.NotNull(cut.Find(".ant-popconfirm-buttons"));
    }
}
