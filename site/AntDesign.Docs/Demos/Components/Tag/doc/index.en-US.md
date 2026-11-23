---
category: Components
type: Data Display
title: Tag
cover: https://gw.alipayobjects.com/zos/alicdn/cH1BOLfxC/Tag.svg
---

Tag for categorizing or marking items.

## When To Use

- Use tags to label items by dimension or property.
- Use tags to group or categorize related items.

## API

### Tag

| Parameter | Description | Type | Default |
| --- | --- | --- | --- |
| Bordered | Deprecated compatibility alias; use `Variant`. | bool? | - |
| Checkable | Blazor convenience for rendering a checkable tag. React exposes this as `Tag.CheckableTag`. | bool | false |
| Checked | Whether a checkable tag is selected. | bool | false |
| CheckedChanged | Callback when a checkable tag changes state. | EventCallback<bool> | - |
| ChildContent | Tag content. | RenderFragment | - |
| Class | CSS class on the root. | string | - |
| ClassNames | Semantic classes for `root`, `icon`, `content`, and `close`. | IReadOnlyDictionary<string, string> | - |
| CloseIcon | Custom close icon content; enables the close control when `Closable` is omitted. | RenderFragment | - |
| Closable | Whether to show the close control. An explicit `false` suppresses it even when `CloseIcon` is set. | bool? | false |
| Color | Preset color, status color, or custom CSS color. | OneOf<TagColor, string> | TagColor.Default |
| Disabled | Disables tag interactions and applies disabled styling. | bool | false |
| Href | URL that renders the root as an anchor. | string | - |
| Icon | Icon name rendered before the content. | string | - |
| IconTemplate | Custom icon content rendered before the content. | RenderFragment | - |
| IconTheme | Theme for the named `Icon`. | IconThemeType | Outline |
| OnClick | Callback when the tag is clicked. | EventCallback | - |
| OnKeyDown | Callback for key presses on the root. | EventCallback<KeyboardEventArgs> | - |
| OnClose | Callback invoked before the tag is hidden. | EventCallback<MouseEventArgs> | - |
| OnClosing | Blazor callback that can cancel closing through `CloseEventArgs.Cancel`. | EventCallback<CloseEventArgs<MouseEventArgs>> | - |
| RootClassName | Additional class on the tag root. React `rootClassName` equivalent. | string | - |
| Styles | Semantic inline styles for `root`, `icon`, `content`, and `close`. | IReadOnlyDictionary<string, string> | - |
| Style | Inline style on the root. | string | - |
| Target | Browsing context when `Href` is set. | string | - |
| Variant | Visual style: `Filled`, `Solid`, or `Outlined`. | TagVariant? | Filled |
| Visible | Initial visibility; closing also hides the existing root using `ant-tag-hidden`. Blazor-specific. | bool | true |

`Disabled`, `Href`, `Target`, `CloseIcon`, `Bordered`, and `RootClassName` map to the corresponding React Tag behavior. `OnClosing` and `Visible` are Blazor-specific extensions.

React v6 also provides `Disabled` and `Customize close` examples. The matching Blazor demos are included below.

### CheckableTagGroup

| Parameter | Description | Type | Default |
| --- | --- | --- | --- |
| Options | Values or option objects containing `Value`, `Label`, and optional item `Class` / `Style`. | IReadOnlyList<CheckableTagOption> | - |
| Multiple | Enables multiple selection. | bool | false |
| DefaultValue | Initial selection for uncontrolled single or multiple selection. | OneOf<string, IReadOnlyList<string>> | - |
| Value | Selected value in single-selection mode. | string | - |
| ValueChanged | Callback for the selected single value. | EventCallback<string> | - |
| Values | Selected values in multiple-selection mode. | IReadOnlyList<string> | - |
| ValuesChanged | Callback for selected values. | EventCallback<IReadOnlyList<string>> | - |
| Disabled | Disables every option. | bool | false |
| RootClassName | Additional class on the group root. | string | - |
| ClassNames | Semantic classes for `root` and `item`. | IReadOnlyDictionary<string, string> | - |
| Styles | Semantic styles for `root` and `item`. | IReadOnlyDictionary<string, string> | - |

`CheckableTagGroup` is a Blazor convenience component. React v6 provides `CheckableTag` as a controlled checkbox-like tag. The group supports controlled `Value` / `Values` and uncontrolled `DefaultValue`; its string-only option model is Blazor-specific.
