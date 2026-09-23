using System;
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
        JSInterop.SetupVoid("AntDesign.interop.domManipulationHelper.setDomAttribute", _ => true);
    }

    [Fact]
    public void Button_exposes_v6_root_icon_and_content_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Button>(parameters =>
        {
            parameters.Add(x => x.Icon, "search");
            parameters.AddChildContent("Save");
        });

        Assert.Contains("ant-btn", cut.Markup, StringComparison.Ordinal);
        Assert.Contains("ant-btn-icon", cut.Markup, StringComparison.Ordinal);
        Assert.Contains("ant-btn-content", cut.Markup, StringComparison.Ordinal);
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
                .AddChildContent<AntDesign.BreadcrumbItem>(item => item.Add(x => x.ChildContent, "Home")));

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
        Assert.NotNull(cut.Find(".ant-collapse-header"));
        Assert.NotNull(cut.Find(".ant-collapse-header-text"));
        Assert.NotNull(cut.Find(".ant-collapse-expand-icon"));
        Assert.NotNull(cut.Find(".ant-collapse-content-box"));
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
            parameters.AddChildContent("Navigation"));

        Assert.NotNull(cut.Find(".ant-layout-sider"));
        Assert.NotNull(cut.Find(".ant-layout-sider-children"));
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
    public void Switch_exposes_v6_root_indicator_and_content_slots()
    {
        var cut = Context.RenderComponent<AntDesign.Switch>(parameters =>
            parameters.Add(x => x.CheckedChildren, "ON")
                .Add(x => x.UnCheckedChildren, "OFF"));

        Assert.NotNull(cut.Find(".ant-switch"));
        Assert.NotNull(cut.Find(".ant-switch-indicator"));
        Assert.NotNull(cut.Find(".ant-switch-content"));
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
        Assert.Equal(2, cut.FindAll(".ant-splitter-panel").Count);
        Assert.NotNull(cut.Find(".ant-splitter-bar-dragger"));
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
    public void Calendar_exposes_v6_root_header_body_content_and_item_slots()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<AntDesign.JsInterop.DomRect>(AntDesign.JSInteropConstants.GetBoundingClientRect, _ => true)
            .SetResult(new AntDesign.JsInterop.DomRect { Width = 120 });

        var cut = Context.RenderComponent<AntDesign.Calendar>();

        Assert.NotNull(cut.Find(".ant-picker-calendar"));
        Assert.NotNull(cut.Find(".ant-picker-calendar-header"));
        Assert.NotNull(cut.Find(".ant-picker-body"));
        Assert.NotNull(cut.Find(".ant-picker-content"));
        Assert.NotNull(cut.Find(".ant-picker-cell"));
        Assert.NotNull(cut.Find(".ant-picker-calendar-date-content"));
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

        Assert.NotNull(cut.Find(".ant-input-number"));
        Assert.NotNull(cut.Find(".ant-input-number-actions"));
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
            .Add(x => x.ShowText, true));

        Assert.NotNull(cut.Find(".ant-color-picker-trigger"));
        Assert.NotNull(cut.Find(".ant-color-picker-color-block"));
        Assert.Contains("#ff0000", cut.Find(".ant-color-picker-color-block-inner").GetAttribute("style"), StringComparison.OrdinalIgnoreCase);
        Assert.Equal("#ff0000", cut.Find(".ant-color-picker-trigger-text").TextContent);

        await cut.Find(".ant-color-picker-color-block").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
        Assert.NotNull(cut.Find(".ant-popover.ant-color-picker"));
        Assert.NotNull(cut.Find(".ant-color-picker-inner-content"));
        Assert.NotNull(cut.Find("input[type=color]"));
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
            .Add(x => x.Size, 128));

        Assert.NotNull(cut.Find(".ant-qrcode"));
        Assert.NotNull(cut.Find(".ant-qrcode svg"));
        Assert.Contains("<path", cut.Find(".ant-qrcode svg").InnerHtml, StringComparison.Ordinal);
        Assert.Contains("width:128px", cut.Find(".ant-qrcode").GetAttribute("style"), StringComparison.Ordinal);

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
        Assert.NotNull(cut.Find(".ant-popover-container[role='tooltip']"));
        Assert.NotNull(cut.Find(".ant-popconfirm-message-icon"));
        Assert.Equal("Confirm action", cut.Find(".ant-popconfirm-title").TextContent.Trim());
        Assert.NotNull(cut.Find(".ant-popconfirm-buttons"));
    }
}
