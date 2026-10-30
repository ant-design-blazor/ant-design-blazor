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

Generated components share `Class`, `Style`, `Spin`, `Rotate`, `Role`, `AriaLabel`, and `AdditionalAttributes`. Generated two-tone components accept `TwoToneColor` as a `string[]`: one primary color, or a primary/secondary pair. Without a color override, the SVG keeps its original colors. The API table below describes the name-based `Icon` adapter.

When a primary color is supplied alone, generated icons calculate the secondary color with `AntDesign.Colors.ColorPalette.Generate(primary)[0]`; the `Icon` adapter does the same with `TwoToneColor`. To specify both colors, pass a two-item array to a generated icon, or set `TwoToneColor` and `TwoToneSecondaryColor` on `Icon`. The independent C# library ports Ant v6's `@ant-design/colors` algorithm and is also used by default/dark theme tokens, preset palettes, neutral colors and shadows. It generates all ten light swatches or all ten dark swatches, not an icon-only approximation. This works on the initial render and during server prerendering, without JavaScript. For example, `#52c41a` produces `#f6ffed`. `IconColorPalette.GetSecondaryColor` and `IconService.GetSecondaryColor` remain compatibility wrappers.

The five examples corresponding to React v6 are Basic, Two-tone icon and colorful icon, Custom Icon, Use iconfont.cn, and Multiple resources from iconfont.cn. Basic and two-tone examples use the generated `AntDesign.Icons` components directly. Blazor also provides a searchable categorized icon gallery and a separate dynamic two-tone color example.

## API

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Alt | Alternative text for the icon | string | - |
| AriaLabel | Value of the `aria-label` attribute | string | - |
| Class | Additional CSS class | string | - |
| Component | Custom icon content rendered in place of the generated SVG | RenderFragment | - |
| ComponentTemplate | Custom component template receiving SVG attributes; pass its context to a generated icon's `AdditionalAttributes` | `RenderFragment<IReadOnlyDictionary<string, object>>` | - |
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
| TwoToneColor | Optional primary color; when set, calculates the secondary color unless explicitly provided, and selects the TwoTone theme | string | - |
| TwoToneSecondaryColor | Optional explicit secondary color; requires no palette calculation | string | - |
| Type | Name of the generated icon to render | string | - |
| Width | SVG width | string | `1em` |

### IconFont

`IconFont` inherits the display parameters of `Icon` and renders a symbol reference. It can use resources already loaded by the application, or load them through `ScriptUrl` after the interactive render.

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Type | Symbol name, updated when the parameter changes | string | - |
| ScriptUrl | One JavaScript resource or an ordered array; loaded and cached like React's `createFromIconfontCN` | `OneOf<string, string[]>` | - |

## Design Token

Icon has no component-specific Design Tokens in React v6 or Blazor. Size and color inherit from CSS `font-size` and `color` (or the SVG `Width`, `Height`, and `Fill` parameters); use the global theme tokens when those inherited values should change across the application.

## FAQ

### Can icons change the order of component styles?

React v6 documents a CSS-layer issue involving runtime style injection from `@ant-design/icons` together with static `message`, `Modal`, or `notification` methods. That workaround is React-specific: Blazor's generated `AntDesign.Icons` components render SVG markup and do not use React's CSS-in-JS layer injection. If styles are missing in Blazor, verify that the Ant Design Blazor stylesheet and its configured theme assets are loaded; do not add a React icon component as a workaround.
