---
category: Components
type: General
title: Icon
cover: https://gw.alipayobjects.com/zos/alicdn/rrwbSt3FQ/Icon.svg
cols: 1
---

Semantic vector graphics.

## How to use {#how-to-use}

Before using icons, install the `AntDesign` NuGet package:

```bash
dotnet add package AntDesign
```

> **Tip:** The package includes the generated v6 SVG icon catalog from `AntDesign.Icons`; no separate icon package, SVG files, or icon-font stylesheet is required.

Set `Type` to an icon name and `Theme` to `Outline`, `Fill`, or `TwoTone`. The searchable gallery below lists the available icons.

## API

### Common Icon {#common-icon}

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Class | Additional CSS class | string | - |
| Rotate | Rotate by n degrees | int | `0` |
| Spin | Whether to rotate the icon with animation | bool | false |
| Style | Inline CSS styles, such as `font-size` and `color` | string | - |
| TwoToneColor | Optional primary color or primary/secondary color pair; when set, selects the TwoTone theme and calculates the secondary color if only the primary is specified | `OneOf<string, string[]>` | - |

The generated icon names use the same theme suffixes as React: `Outlined`, `Filled`, and `TwoTone`. Use a named component when the icon is known at compile time; use `Icon` with `Type` and `Theme` when selecting an icon at runtime.

```razor
@using AntDesign.Icons

<Space>
    <StarOutlined />
    <StarFilled />
    <StarTwoTone TwoToneColor="#eb2f96" />
</Space>
```

<details class="docs-blazor-api"><summary>Blazor-specific and inherited parameters</summary>

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Alt | Alternative text for the icon | string | - |
| AriaLabel | Value of the `aria-label` attribute | string | - |
| Id | ID applied to the icon wrapper | string | - |
| AdditionalAttributes | Additional HTML attributes applied to the icon wrapper, such as `data-*` attributes | `IReadOnlyDictionary<string, object>` | - |
| Fill | SVG fill value | string | `currentColor` |
| Height | SVG height | string | `1em` |
| OnClick | Click callback | `EventCallback<MouseEventArgs>` | - |
| OnKeyDown | Key down callback for interactive icons | `EventCallback<KeyboardEventArgs>` | - |
| Role | SVG wrapper role; set to an empty string to hide it from assistive technology | string | `img` |
| StopPropagation | Stop click event propagation | bool | false |
| TabIndex | `tabindex` value for the wrapper | string | - |
| Theme | Icon theme | `IconThemeType` | `Outline` |
| TwoToneSecondaryColor | Optional explicit secondary color; requires no palette calculation | string | - |
| Type | Name of the generated icon to render | string | - |
| Width | SVG width | string | `1em` |

</details>

### Custom Icon {#custom-icon}

Use `Component` to render a custom `RenderFragment` inside the icon wrapper. Use `ComponentTemplate` when the custom SVG component needs the `width`, `height`, `fill`, `focusable`, and `aria-hidden` attributes supplied by `Icon`.

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Component | Custom icon content rendered in place of the generated SVG | RenderFragment | - |
| Rotate | Rotate by n degrees | int | `0` |
| Spin | Whether to rotate the icon with animation | bool | false |
| Style | Inline CSS styles, such as `font-size` and `color` | string | - |

```razor
<Icon Component="HeartSvg" />

@code {
    private RenderFragment HeartSvg => @<svg viewBox="0 0 1024 1024" fill="currentColor">
        <path d="M512 900 150 540C-20 360 110 120 330 120c80 0 140 40 182 95 42-55 102-95 182-95 220 0 350 240 180 420L512 900z" />
    </svg>;
}
```

### About SVG icons {#about-svg-icons}

The icon catalog renders inline SVG instead of relying on an icon font. This provides:

- Offline rendering without a CDN-hosted font file or locally deployed font assets.
- Sharp output at different display sizes.
- Colors that inherit from `currentColor` or can be overridden with `Style` and `TwoToneColor`.
- Custom SVG markup through `Component` or `ComponentTemplate`.

See the [SVG icon discussion](https://github.com/ant-design/ant-design/issues/10353) for the React icon migration background. Blazor uses generated Razor components and inline SVG, so the React-specific webpack/Vite loaders and CSS-in-JS layer setup are not required. The `AntDesign.Icons` NuGet package supplies the generated catalog; use `Icon.Type` for a runtime-selected name or a named icon component for a compile-time reference.

Both forms accept inline styles for size and color:

```razor
@using AntDesign.Icons

<MessageOutlined Style="font-size: 16px; color: #08c;" />
<Icon Type="message" Theme="IconThemeType.Outline" Style="font-size: 16px; color: #08c;" />
```

### Set TwoTone Color {#set-two-tone-color}

Generated components share `Class`, `Style`, `Spin`, `Rotate`, `Role`, `AriaLabel`, and `AdditionalAttributes`. `TwoToneColor` accepts a primary color or a primary/secondary pair, matching React's `twoToneColor`. With one color, the secondary color is calculated by `AntDesign.Colors.ColorPalette.Generate(primary)[0]`; without an override, the SVG keeps its original colors. The same Ant v6 color algorithm powers theme palettes. For example, `#52c41a` produces `#f6ffed`. `IconColorPalette.GetSecondaryColor` and `IconService.GetSecondaryColor` remain compatibility wrappers.

```razor
@using AntDesign.Icons

<SmileTwoTone TwoToneColor="#eb2f96" />
<HeartTwoTone TwoToneColor='@(new[] { "#1677ff", "#e6f4ff" })' />
<Icon Type="heart" TwoToneColor='@(new[] { "#1677ff", "#e6f4ff" })' />
```

### Custom Font Icon {#custom-font-icon}

`IconFont` inherits the display parameters of `Icon` and renders a symbol reference. It can use resources already loaded by the application, or load them through `ScriptUrl` after the interactive render.

Create a project on [iconfont.cn](https://www.iconfont.cn/), select the symbols the application needs, and generate its JavaScript resource. Use each symbol name as `Type`:

```razor
<IconFont Type="icon-example" ScriptUrl="//at.alicdn.com/t/font_example.js" />
```

`ScriptUrl` also accepts an ordered array of resources. The helper loads and caches each URL and associates the sprite symbols with the `IconFont` instance.

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| IconFont | Icon-font symbol name, used by `IconFont` | string | - |
| Type | Symbol name, updated when the parameter changes | string | - |
| ScriptUrl | One JavaScript resource or an ordered array; loaded and cached like React's `createFromIconfontCN` | `OneOf<string, string[]>` | - |
| ExtraCommonProps | Additional attributes applied to the SVG symbol, such as `aria-label` or `data-*` | `IDictionary<string, object>` | - |

### Custom SVG Icon {#custom-svg-icon}

React's `component` prop accepts an SVG React component. In Blazor, use `Component` for inline/custom SVG markup or `ComponentTemplate` when a child component should receive the SVG attributes from `Icon`. No webpack or Vite SVG loader configuration is needed.

The common React SVG properties map to these Blazor parameters:

| React property | Blazor equivalent |
| --- | --- |
| `className` | `Class` |
| `fill` | `Fill` |
| `height` | `Height` |
| `style` | `Style` |
| `width` | `Width` |

Use `ComponentTemplate` to forward those attributes to a generated icon:

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| ComponentTemplate | Custom component template receiving SVG attributes; pass its context to a generated icon's `AdditionalAttributes` | `RenderFragment<IReadOnlyDictionary<string, object>>` | - |

```razor
<Icon>
    <ComponentTemplate Context="attributes">
        <HomeOutlined AdditionalAttributes="attributes" />
    </ComponentTemplate>
</Icon>
```

## FAQ

### Can icons change the order of component styles?

React v6 documents a CSS-layer issue involving runtime style injection from `@ant-design/icons` together with static `message`, `Modal`, or `notification` methods. That workaround is React-specific: Blazor's generated `AntDesign.Icons` components render SVG markup and do not use React's CSS-in-JS layer injection. If styles are missing in Blazor, verify that the Ant Design Blazor stylesheet and its configured theme assets are loaded; do not add a React icon component as a workaround.
