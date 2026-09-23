using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AntDesign.Docs.Services;

internal static class SemanticDomGeneratedCatalog
{
    internal static readonly IReadOnlyDictionary<string, IReadOnlyList<SemanticDomNode>> Nodes =
        new ReadOnlyDictionary<string, IReadOnlyList<SemanticDomNode>>(
            new Dictionary<string, IReadOnlyList<SemanticDomNode>>(StringComparer.OrdinalIgnoreCase)
            {
                ["affix"] = Array.Empty<SemanticDomNode>(),
                ["alert"] = new[]
                {
                    new SemanticDomNode("actions", ".ant-alert-actions", "alert actions Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("close", ".ant-alert-close", "alert close Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("description", ".ant-alert-description", "alert description Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("icon", ".ant-alert-icon", "alert icon Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-alert", "alert root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("section", ".ant-alert-section", "alert section Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("title", ".ant-alert-title", "alert title Semantic DOM 节点", "6.0.0"),
                },
                ["anchor"] = new[]
                {
                    new SemanticDomNode("indicator", ".ant-anchor-ink", "anchor indicator Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("item", ".ant-anchor-link", "anchor item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemTitle", ".ant-anchor-link-title", "anchor itemTitle Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-anchor-wrapper", "anchor root Semantic DOM 节点", "6.0.0"),
                },
                ["auto-complete"] = new[]
                {
                    new SemanticDomNode("clear", ".ant-auto-complete-clear", "auto-complete clear Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("content", ".ant-auto-complete-content", "auto-complete content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("input", ".ant-auto-complete-input", "auto-complete input Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("placeholder", ".ant-auto-complete-placeholder", "auto-complete placeholder Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("popup", ".ant-auto-complete-popup", "auto-complete popup Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("prefix", ".ant-auto-complete-prefix", "auto-complete prefix Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-auto-complete", "auto-complete root Semantic DOM 节点", "6.0.0"),
                },
                ["avatar"] = Array.Empty<SemanticDomNode>(),
                ["back-top"] = Array.Empty<SemanticDomNode>(),
                ["badge"] = new[]
                {
                    new SemanticDomNode("content", ".ant-badge-content", "badge content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("indicator", ".ant-badge-indicator", "badge indicator Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-badge", "badge root Semantic DOM 节点", "6.0.0"),
                },
                ["breadcrumb"] = new[]
                {
                    new SemanticDomNode("item", ".ant-breadcrumb-item", "breadcrumb item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-breadcrumb", "breadcrumb root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("separator", ".ant-breadcrumb-separator", "breadcrumb separator Semantic DOM 节点", "6.0.0"),
                },
                ["button"] = new[]
                {
                    new SemanticDomNode("content", ".ant-button-content", "button content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("icon", ".ant-button-icon", "button icon Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-button", "button root Semantic DOM 节点", "6.0.0"),
                },
                ["calendar"] = new[]
                {
                    new SemanticDomNode("body", ".ant-picker-body", "calendar body Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("content", ".ant-picker-content", "calendar content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("header", ".ant-picker-calendar-header", "calendar header Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("item", ".ant-picker-cell", "calendar item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemContent", ".ant-picker-calendar-date-content", "calendar itemContent Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-picker-calendar", "calendar root Semantic DOM 节点", "6.0.0"),
                },
                ["card"] = new[]
                {
                    new SemanticDomNode("actions", ".ant-card-actions", "card actions Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("avatar", ".ant-card-avatar", "card avatar Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("body", ".ant-card-body", "card body Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("cover", ".ant-card-cover", "card cover Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("description", ".ant-card-description", "card description Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("extra", ".ant-card-extra", "card extra Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("header", ".ant-card-header", "card header Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-card", "card root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("section", ".ant-card-section", "card section Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("title", ".ant-card-title", "card title Semantic DOM 节点", "6.0.0"),
                },
                ["carousel"] = Array.Empty<SemanticDomNode>(),
                ["cascader"] = new[]
                {
                    new SemanticDomNode("content", ".ant-select-content, .ant-select-selection-item", "cascader content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("input", ".ant-select-input, .ant-select-selection-search-input", "cascader input Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("item", ".ant-cascader-menu-item", "cascader item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemContent", ".ant-cascader-menu-item-content", "cascader itemContent Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemRemove", ".ant-select-selection-item-remove", "cascader itemRemove Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("placeholder", ".ant-select-selection-placeholder", "cascader placeholder Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("popup", ".ant-cascader-dropdown", "cascader popup Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("prefix", ".ant-select-prefix", "cascader prefix Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-select.ant-cascader", "cascader root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("suffix", ".ant-select-suffix", "cascader suffix Semantic DOM 节点", "6.0.0"),
                },
                ["checkbox"] = new[]
                {
                    new SemanticDomNode("icon", ".ant-checkbox-icon", "checkbox icon Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("label", ".ant-checkbox-label", "checkbox label Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-checkbox", "checkbox root Semantic DOM 节点", "6.0.0"),
                },
                ["collapse"] = new[]
                {
                    new SemanticDomNode("body", ".ant-collapse-body", "collapse body Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("header", ".ant-collapse-header", "collapse header Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("icon", ".ant-collapse-icon", "collapse icon Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-collapse", "collapse root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("title", ".ant-collapse-title", "collapse title Semantic DOM 节点", "6.0.0"),
                },
                ["color-picker"] = new[]
                {
                    new SemanticDomNode("body", ".ant-color-picker-color-block", "color-picker body Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("content", ".ant-color-picker-color-block-inner", "color-picker content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("description", ".ant-color-picker-trigger-text", "color-picker description Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("popup", ".ant-popover.ant-color-picker", "color-picker popup Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("popupOverlayInner", ".ant-color-picker-inner-content", "color-picker popupOverlayInner Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-color-picker-trigger", "color-picker root Semantic DOM 节点", "6.0.0"),
                },
                ["config-provider"] = Array.Empty<SemanticDomNode>(),
                ["date-picker"] = new[]
                {
                    new SemanticDomNode("body", ".ant-date-picker-body", "date-picker body Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("container", ".ant-date-picker-container", "date-picker container Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("content", ".ant-date-picker-content", "date-picker content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("footer", ".ant-date-picker-footer", "date-picker footer Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("header", ".ant-date-picker-header", "date-picker header Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("input", ".ant-date-picker-input", "date-picker input Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("item", ".ant-date-picker-item", "date-picker item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("popup", ".ant-date-picker-popup", "date-picker popup Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("prefix", ".ant-date-picker-prefix", "date-picker prefix Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-date-picker", "date-picker root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("suffix", ".ant-date-picker-suffix", "date-picker suffix Semantic DOM 节点", "6.0.0"),
                },
                ["descriptions"] = new[]
                {
                    new SemanticDomNode("content", ".ant-descriptions-content", "descriptions content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("extra", ".ant-descriptions-extra", "descriptions extra Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("header", ".ant-descriptions-header", "descriptions header Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("label", ".ant-descriptions-label", "descriptions label Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-descriptions", "descriptions root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("title", ".ant-descriptions-title", "descriptions title Semantic DOM 节点", "6.0.0"),
                },
                ["divider"] = new[]
                {
                    new SemanticDomNode("content", ".ant-divider-content", "divider content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("rail", ".ant-divider-rail", "divider rail Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-divider", "divider root Semantic DOM 节点", "6.0.0"),
                },
                ["drawer"] = new[]
                {
                    new SemanticDomNode("body", ".ant-drawer-body", "drawer body Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("close", ".ant-drawer-close", "drawer close Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("content", ".ant-drawer-content", "drawer content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("dragger", ".ant-drawer-dragger", "drawer dragger Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("extra", ".ant-drawer-extra", "drawer extra Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("footer", ".ant-drawer-footer", "drawer footer Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("header", ".ant-drawer-header", "drawer header Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("mask", ".ant-drawer-mask", "drawer mask Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-drawer", "drawer root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("section", ".ant-drawer-section", "drawer section Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("title", ".ant-drawer-title", "drawer title Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("wrapper", ".ant-drawer-wrapper", "drawer wrapper Semantic DOM 节点", "6.0.0"),
                },
                ["dropdown"] = new[]
                {
                    new SemanticDomNode("item", ".ant-dropdown-item", "dropdown item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemContent", ".ant-dropdown-item-content", "dropdown itemContent Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemIcon", ".ant-dropdown-item-icon", "dropdown itemIcon Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemTitle", ".ant-dropdown-item-title", "dropdown itemTitle Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-dropdown", "dropdown root Semantic DOM 节点", "6.0.0"),
                },
                ["empty"] = new[]
                {
                    new SemanticDomNode("description", ".ant-empty-description", "empty description Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("footer", ".ant-empty-footer", "empty footer Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("image", ".ant-empty-image", "empty image Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-empty", "empty root Semantic DOM 节点", "6.0.0"),
                },
                ["flex"] = Array.Empty<SemanticDomNode>(),
                ["float-button"] = new[]
                {
                    new SemanticDomNode("item", ".ant-float-button-item", "float-button item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemContent", ".ant-float-button-item-content", "float-button itemContent Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemIcon", ".ant-float-button-item-icon", "float-button itemIcon Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("list", ".ant-float-button-list", "float-button list Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-float-button", "float-button root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("trigger", ".ant-float-button-trigger", "float-button trigger Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("triggerContent", ".ant-float-button-trigger-content", "float-button triggerContent Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("triggerIcon", ".ant-float-button-trigger-icon", "float-button triggerIcon Semantic DOM 节点", "6.0.0"),
                },
                ["form"] = new[]
                {
                    new SemanticDomNode("content", ".ant-form-item-control-input-content", "form content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("extra", ".ant-form-item-extra", "form extra Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("help", ".ant-form-item-explain", "form help Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("helpItem", ".ant-form-item-explain-connected", "form helpItem Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("label", ".ant-form-item-label", "form label Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-form", "form root Semantic DOM 节点", "6.0.0"),
                },
                ["grid"] = Array.Empty<SemanticDomNode>(),
                ["image"] = new[]
                {
                    new SemanticDomNode("actions", ".ant-image-actions", "image actions Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("body", ".ant-image-body", "image body Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("close", ".ant-image-close", "image close Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("cover", ".ant-image-cover", "image cover Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("footer", ".ant-image-footer", "image footer Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("image", ".ant-image-image", "image image Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("mask", ".ant-image-mask", "image mask Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("placeholder", ".ant-image-placeholder", "image placeholder Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("popup", ".ant-image-popup", "image popup Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("progress", ".ant-image-progress", "image progress Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-image", "image root Semantic DOM 节点", "6.0.0"),
                },
                ["input"] = new[]
                {
                    new SemanticDomNode("button", ".ant-input-search-button", "input button Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("clear", ".ant-input-clear-icon", "input clear Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("count", ".ant-input-data-count, .ant-input-show-count-suffix", "input count Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("input", ".ant-input", "input input Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("prefix", ".ant-input-prefix", "input prefix Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-input-affix-wrapper, .ant-input-group-wrapper, input.ant-input", "input root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("separator", ".ant-input-group-addon", "input separator Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("suffix", ".ant-input-suffix", "input suffix Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("textarea", "textarea.ant-input", "input textarea Semantic DOM 节点", "6.0.0"),
                },
                ["input-number"] = new[]
                {
                    new SemanticDomNode("actions", ".ant-input-number-actions", "input-number actions Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("input", ".ant-input-number-input", "input-number input Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("prefix", ".ant-input-number-prefix", "input-number prefix Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-input-number", "input-number root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("suffix", ".ant-input-number-suffix", "input-number suffix Semantic DOM 节点", "6.0.0"),
                },
                ["layout"] = new[]
                {
                    new SemanticDomNode("body", ".ant-layout-sider-children", "layout body Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-layout-sider", "layout root Semantic DOM 节点", "6.0.0"),
                },
                ["list"] = Array.Empty<SemanticDomNode>(),
                ["listy"] = new[]
                {
                    new SemanticDomNode("groupHeader", ".ant-listy-group-header", "listy groupHeader Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("item", ".ant-listy-item", "listy item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-listy", "listy root Semantic DOM 节点", "6.0.0"),
                },
                ["masonry"] = new[]
                {
                    new SemanticDomNode("item", ".ant-masonry-item", "masonry item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-masonry", "masonry root Semantic DOM 节点", "6.0.0"),
                },
                ["mentions"] = new[]
                {
                    new SemanticDomNode("popup", ".ant-mentions-popup", "mentions popup Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-mentions", "mentions root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("suffix", ".ant-mentions-suffix", "mentions suffix Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("textarea", ".ant-mentions-textarea", "mentions textarea Semantic DOM 节点", "6.0.0"),
                },
                ["menu"] = Array.Empty<SemanticDomNode>(),
                ["message"] = new[]
                {
                    new SemanticDomNode("icon", ".ant-message-icon", "message icon Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("list", ".ant-message-list", "message list Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("listContent", ".ant-message-list-content", "message listContent Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-message", "message root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("title", ".ant-message-title", "message title Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("wrapper", ".ant-message-wrapper", "message wrapper Semantic DOM 节点", "6.0.0"),
                },
                ["modal"] = new[]
                {
                    new SemanticDomNode("body", ".ant-modal-body", "modal body Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("close", ".ant-modal-close", "modal close Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("container", ".ant-modal-container", "modal container Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("footer", ".ant-modal-footer", "modal footer Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("header", ".ant-modal-header", "modal header Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("mask", ".ant-modal-mask", "modal mask Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-modal", "modal root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("title", ".ant-modal-title", "modal title Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("wrapper", ".ant-modal-wrapper", "modal wrapper Semantic DOM 节点", "6.0.0"),
                },
                ["notification"] = new[]
                {
                    new SemanticDomNode("actions", ".ant-notification-actions", "notification actions Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("close", ".ant-notification-close", "notification close Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("description", ".ant-notification-description", "notification description Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("icon", ".ant-notification-icon", "notification icon Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("list", ".ant-notification-list", "notification list Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("listContent", ".ant-notification-list-content", "notification listContent Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("progress", ".ant-notification-progress", "notification progress Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-notification", "notification root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("section", ".ant-notification-section", "notification section Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("title", ".ant-notification-title", "notification title Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("wrapper", ".ant-notification-wrapper", "notification wrapper Semantic DOM 节点", "6.0.0"),
                },
                ["pagination"] = new[]
                {
                    new SemanticDomNode("item", ".ant-pagination-item", "pagination item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-pagination", "pagination root Semantic DOM 节点", "6.0.0"),
                },
                ["popconfirm"] = Array.Empty<SemanticDomNode>(),
                ["popover"] = new[]
                {
                    new SemanticDomNode("content", ".ant-popover-content", "popover content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("title", ".ant-popover-title", "popover title Semantic DOM 节点", "6.0.0"),
                },
                ["progress"] = new[]
                {
                    new SemanticDomNode("body", ".ant-progress-body", "progress body Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("indicator", ".ant-progress-indicator", "progress indicator Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("rail", ".ant-progress-rail", "progress rail Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-progress", "progress root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("track", ".ant-progress-track", "progress track Semantic DOM 节点", "6.0.0"),
                },
                ["qr-code"] = new[]
                {
                    new SemanticDomNode("cover", ".ant-qr-code-cover", "qr-code cover Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-qr-code", "qr-code root Semantic DOM 节点", "6.0.0"),
                },
                ["radio"] = new[]
                {
                    new SemanticDomNode("icon", ".ant-radio-icon", "radio icon Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("label", ".ant-radio-label", "radio label Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-radio", "radio root Semantic DOM 节点", "6.0.0"),
                },
                ["rate"] = Array.Empty<SemanticDomNode>(),
                ["result"] = new[]
                {
                    new SemanticDomNode("body", ".ant-result-body", "result body Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("extra", ".ant-result-extra", "result extra Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("icon", ".ant-result-icon", "result icon Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-result", "result root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("subTitle", ".ant-result-subtitle", "result subTitle Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("title", ".ant-result-title", "result title Semantic DOM 节点", "6.0.0"),
                },
                ["segmented"] = new[]
                {
                    new SemanticDomNode("icon", ".ant-segmented-item-icon", "segmented icon Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("item", ".ant-segmented-item", "segmented item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("label", ".ant-segmented-item-label", "segmented label Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-segmented", "segmented root Semantic DOM 节点", "6.0.0"),
                },
                ["select"] = new[]
                {
                    new SemanticDomNode("clear", ".ant-select-clear", "select clear Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("content", ".ant-select-content", "select content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("input", ".ant-select-input", "select input Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("item", ".ant-select-item", "select item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemContent", ".ant-select-item-content", "select itemContent Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemRemove", ".ant-select-item-remove", "select itemRemove Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("list", ".ant-select-list", "select list Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("listItem", ".ant-select-list-item", "select listItem Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("placeholder", ".ant-select-placeholder", "select placeholder Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("popup", ".ant-select-popup", "select popup Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("prefix", ".ant-select-prefix", "select prefix Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-select", "select root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("suffix", ".ant-select-suffix", "select suffix Semantic DOM 节点", "6.0.0"),
                },
                ["skeleton"] = new[]
                {
                    new SemanticDomNode("avatar", ".ant-skeleton-avatar", "skeleton avatar Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("content", ".ant-skeleton-content", "skeleton content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("header", ".ant-skeleton-header", "skeleton header Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("paragraph", ".ant-skeleton-paragraph", "skeleton paragraph Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-skeleton", "skeleton root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("section", ".ant-skeleton-section", "skeleton section Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("title", ".ant-skeleton-title", "skeleton title Semantic DOM 节点", "6.0.0"),
                },
                ["slider"] = new[]
                {
                    new SemanticDomNode("handle", ".ant-slider-handle", "slider handle Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("rail", ".ant-slider-rail", "slider rail Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-slider", "slider root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("track", ".ant-slider-track", "slider track Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("tracks", ".ant-slider-tracks", "slider tracks Semantic DOM 节点", "6.0.0"),
                },
                ["space"] = new[]
                {
                    new SemanticDomNode("item", ".ant-space-item", "space item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-space", "space root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("separator", ".ant-space-item-separator", "space separator Semantic DOM 节点", "6.0.0"),
                },
                ["spin"] = new[]
                {
                    new SemanticDomNode("container", ".ant-spin-container", "spin container Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("description", ".ant-spin-description", "spin description Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("indicator", ".ant-spin-dot-holder", "spin indicator Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("mask", ".ant-spin", "spin mask Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-spin", "spin root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("section", ".ant-spin-section", "spin section Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("tip", ".ant-spin-description", "spin tip Semantic DOM 节点（兼容别名）", "6.0.0"),
                },
                ["splitter"] = new[]
                {
                    new SemanticDomNode("dragger", ".ant-splitter-bar-dragger", "splitter dragger Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("panel", ".ant-splitter-panel", "splitter panel Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-splitter", "splitter root Semantic DOM 节点", "6.0.0"),
                },
                ["statistic"] = new[]
                {
                    new SemanticDomNode("content", ".ant-statistic-content", "statistic content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("header", ".ant-statistic-header", "statistic header Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("prefix", ".ant-statistic-content-prefix", "statistic prefix Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-statistic", "statistic root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("suffix", ".ant-statistic-content-suffix", "statistic suffix Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("title", ".ant-statistic-title", "statistic title Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("value", ".ant-statistic-content-value", "statistic value Semantic DOM 节点", "6.0.0"),
                },
                ["steps"] = new[]
                {
                    new SemanticDomNode("item", ".ant-steps-item", "steps item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemContent", ".ant-steps-item-content", "steps itemContent Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemHeader", ".ant-steps-item-header", "steps itemHeader Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemIcon", ".ant-steps-item-icon", "steps itemIcon Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemRail", ".ant-steps-item-rail", "steps itemRail Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemSection", ".ant-steps-item-section", "steps itemSection Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemSubtitle", ".ant-steps-item-subtitle", "steps itemSubtitle Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemTitle", ".ant-steps-item-title", "steps itemTitle Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemWrapper", ".ant-steps-item-wrapper", "steps itemWrapper Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-steps", "steps root Semantic DOM 节点", "6.0.0"),
                },
                ["switch"] = new[]
                {
                    new SemanticDomNode("content", ".ant-switch-content", "switch content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("indicator", ".ant-switch-indicator", "switch indicator Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-switch", "switch root Semantic DOM 节点", "6.0.0"),
                },
                ["table"] = new[]
                {
                    new SemanticDomNode("body", ".ant-table-body", "table body Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("content", ".ant-table-content", "table content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("footer", ".ant-table-footer", "table footer Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("header", ".ant-table-header", "table header Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("pagination", ".ant-table-pagination", "table pagination Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-table", "table root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("section", ".ant-table-section", "table section Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("title", ".ant-table-title", "table title Semantic DOM 节点", "6.0.0"),
                },
                ["tabs"] = new[]
                {
                    new SemanticDomNode("body", ".ant-tabs-content-holder", "tabs body Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("content", ".ant-tabs-content", "tabs content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("header", ".ant-tabs-nav", "tabs header Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("indicator", ".ant-tabs-ink-bar", "tabs indicator Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("item", ".ant-tabs-tab", "tabs item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("popup", ".ant-tabs-dropdown-menu-root", "tabs popup Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("remove", ".ant-tabs-tab-remove", "tabs remove Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-tabs", "tabs root Semantic DOM 节点", "6.0.0"),
                },
                ["tag"] = new[]
                {
                    new SemanticDomNode("close", ".ant-tag-close", "tag close Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("content", ".ant-tag-content", "tag content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("icon", ".ant-tag-icon", "tag icon Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("item", ".ant-tag-item", "tag item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-tag", "tag root Semantic DOM 节点", "6.0.0"),
                },
                ["time-picker"] = new[]
                {
                    new SemanticDomNode("container", ".ant-time-picker-container", "time-picker container Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("content", ".ant-time-picker-content", "time-picker content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("footer", ".ant-time-picker-footer", "time-picker footer Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("input", ".ant-time-picker-input", "time-picker input Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("item", ".ant-time-picker-item", "time-picker item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("popup", ".ant-time-picker-popup", "time-picker popup Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("prefix", ".ant-time-picker-prefix", "time-picker prefix Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-time-picker", "time-picker root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("suffix", ".ant-time-picker-suffix", "time-picker suffix Semantic DOM 节点", "6.0.0"),
                },
                ["timeline"] = Array.Empty<SemanticDomNode>(),
                ["tooltip"] = new[]
                {
                    new SemanticDomNode("arrow", ".ant-tooltip-arrow", "tooltip arrow Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("container", ".ant-tooltip-container", "tooltip container Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-tooltip", "tooltip root Semantic DOM 节点", "6.0.0"),
                },
                ["tour"] = new[]
                {
                    new SemanticDomNode("actions", ".ant-tour-actions", "tour actions Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("close", ".ant-tour-close", "tour close Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("cover", ".ant-tour-cover", "tour cover Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("description", ".ant-tour-description", "tour description Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("footer", ".ant-tour-footer", "tour footer Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("header", ".ant-tour-header", "tour header Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("indicator", ".ant-tour-indicator", "tour indicator Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("indicators", ".ant-tour-indicators", "tour indicators Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("mask", ".ant-tour-mask", "tour mask Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-tour", "tour root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("section", ".ant-tour-section", "tour section Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("title", ".ant-tour-title", "tour title Semantic DOM 节点", "6.0.0"),
                },
                ["transfer"] = new[]
                {
                    new SemanticDomNode("actions", ".ant-transfer-actions", "transfer actions Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("body", ".ant-transfer-body", "transfer body Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("footer", ".ant-transfer-footer", "transfer footer Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("header", ".ant-transfer-header", "transfer header Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("item", ".ant-transfer-item", "transfer item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemContent", ".ant-transfer-item-content", "transfer itemContent Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemIcon", ".ant-transfer-item-icon", "transfer itemIcon Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("list", ".ant-transfer-list", "transfer list Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-transfer", "transfer root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("section", ".ant-transfer-section", "transfer section Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("source", ".ant-transfer-source", "transfer source Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("target", ".ant-transfer-target", "transfer target Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("title", ".ant-transfer-title", "transfer title Semantic DOM 节点", "6.0.0"),
                },
                ["tree"] = new[]
                {
                    new SemanticDomNode("item", ".ant-tree-treenode", "tree item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemIcon", ".ant-tree-iconEle", "tree itemIcon Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemSwitcher", ".ant-tree-switcher", "tree itemSwitcher Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemTitle", ".ant-tree-title", "tree itemTitle Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-tree", "tree root Semantic DOM 节点", "6.0.0"),
                },
                ["tree-select"] = new[]
                {
                    new SemanticDomNode("content", ".ant-tree-select-content", "tree-select content Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("input", ".ant-tree-select-input", "tree-select input Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("item", ".ant-tree-select-item", "tree-select item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemContent", ".ant-tree-select-item-content", "tree-select itemContent Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemRemove", ".ant-tree-select-item-remove", "tree-select itemRemove Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemSwitcher", ".ant-tree-select-item-switcher", "tree-select itemSwitcher Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("itemTitle", ".ant-tree-select-item-title", "tree-select itemTitle Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("placeholder", ".ant-tree-select-placeholder", "tree-select placeholder Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("popup", ".ant-tree-select-popup", "tree-select popup Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("prefix", ".ant-tree-select-prefix", "tree-select prefix Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-tree-select", "tree-select root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("suffix", ".ant-tree-select-suffix", "tree-select suffix Semantic DOM 节点", "6.0.0"),
                },
                ["typography"] = new[]
                {
                    new SemanticDomNode("action", ".ant-typography-action", "typography action Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("actions", ".ant-typography-actions", "typography actions Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-typography", "typography root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("textarea", ".ant-typography-textarea", "typography textarea Semantic DOM 节点", "6.0.0"),
                },
                ["upload"] = new[]
                {
                    new SemanticDomNode("item", ".ant-upload-list-item", "upload item Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("list", ".ant-upload-list", "upload list Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("root", ".ant-upload-wrapper", "upload root Semantic DOM 节点", "6.0.0"),
                    new SemanticDomNode("trigger", ".ant-upload-select", "upload trigger Semantic DOM 节点", "6.0.0"),
                },
            });
}
