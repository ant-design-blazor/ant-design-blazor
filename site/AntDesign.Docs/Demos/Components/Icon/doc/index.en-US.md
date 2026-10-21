---
category: Components
type: General
title: Icon
cover: https://gw.alipayobjects.com/zos/alicdn/rrwbSt3FQ/Icon.svg
---

Semantic vector graphics.

## How to use

Install `AntDesign` as usual. Its NuGet dependency supplies the generated v6 icon catalog from `AntDesign.Icons`; no separate SVG files or icon-font stylesheet need to be copied into the application.

Set `Type` to an icon name and `Theme` to `Outline`, `Fill`, or `TwoTone`. The searchable icon gallery below is generated from the package catalog and its category metadata.

## Examples

The five examples corresponding to React v6 are Basic, Two-tone icon and colorful icon, Custom Icon, Use iconfont.cn, and Multiple resources from iconfont.cn. Blazor also provides a searchable categorized icon gallery.

## API

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Alt | Alternative text for the icon | string | - |
| AriaLabel | Value of the `aria-label` attribute | string | - |
| Class | Additional CSS class | string | - |
| Component | Custom icon content rendered in place of the generated SVG | RenderFragment | - |
| Fill | SVG fill value | string | `currentColor` |
| Height | SVG height | string | `1em` |
| IconFont | Icon-font symbol name, used by `IconFont` | string | - |
| OnClick | Click callback | `EventCallback<MouseEventArgs>` | - |
| Role | SVG wrapper role; set to an empty string to hide it from assistive technology | string | `img` |
| Rotate | Rotate by n degrees | int | `0` |
| Spin | Rotate the icon with animation | bool | false |
| StopPropagation | Stop click event propagation | bool | false |
| Style | Inline CSS styles, such as `font-size` and `color` | string | - |
| TabIndex | `tabindex` value for the wrapper | string | - |
| Theme | Icon theme | `IconThemeType` | `Outline` |
| TwoToneColor | Primary color for a two-tone icon; setting it also selects the TwoTone theme | string | - |
| Type | Name of the generated icon to render | string | - |
| Width | SVG width | string | `1em` |

## Design Token

Icon has no component-specific Design Tokens in React v6 or Blazor. Size and color inherit from CSS `font-size` and `color` (or the SVG `Width`, `Height`, and `Fill` parameters); use the global theme tokens when those inherited values should change across the application.
