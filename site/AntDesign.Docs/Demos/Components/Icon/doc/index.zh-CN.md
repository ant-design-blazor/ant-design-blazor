---
category: Components
type: 通用
title: Icon
subtitle: 图标
cover: https://gw.alipayobjects.com/zos/alicdn/rrwbSt3FQ/Icon.svg
cols: 1
---

语义化的矢量图形。

## 使用方法 {#how-to-use}

使用图标前，请先安装 `AntDesign` NuGet 包：

```bash
dotnet add package AntDesign
```

> **提示：** 包中包含由 `AntDesign.Icons` 生成的 v6 SVG 图标目录；无需额外安装图标包、复制 SVG 文件或配置图标字体样式表。

通过 `Type` 指定图标名称，`Theme` 设置为 `Outline`、`Fill` 或 `TwoTone`。下方可搜索图标库会展示可用图标。


## 设计师专属 {#designers-exclusive}

安装 [Kitchen Sketch 插件 💎](https://kitchen.alipay.com)，即可拖拽使用 Ant Design 和 Iconfont 图标，并关联自己的项目。

## API

### 通用图标 {#common-icon}

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Class | 附加 CSS 类名 | string | - |
| Rotate | 图标旋转角度 | int | `0` |
| Spin | 是否旋转图标并显示动画 | bool | false |
| Style | 行内样式，例如 `font-size` 和 `color` | string | - |
| TwoToneColor | 可选主色或主色/次色组合；设置后自动选择 TwoTone 主题，仅提供主色时自动计算次色 | `OneOf<string, string[]>` | - |

生成图标的名称使用与 React 相同的主题后缀：`Outlined`、`Filled` 和 `TwoTone`。编译时已知图标时可使用具名组件；需要在运行时选择图标时，可通过 `Icon` 的 `Type` 和 `Theme` 指定。

```razor
@using AntDesign.Icons

<Space>
    <StarOutlined />
    <StarFilled />
    <StarTwoTone TwoToneColor="#eb2f96" />
</Space>
```

<details class="docs-blazor-api"><summary>Blazor 特有及继承参数</summary>

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Alt | 图标备用文本 | string | - |
| AriaLabel | `aria-label` 属性值 | string | - |
| Id | 应用于图标包装元素的 ID | string | - |
| AdditionalAttributes | 应用于图标包装元素的额外 HTML 属性，例如 `data-*` 属性 | `IReadOnlyDictionary<string, object>` | - |
| Fill | SVG 填充色 | string | `currentColor` |
| Height | SVG 高度 | string | `1em` |
| OnClick | 点击回调 | `EventCallback<MouseEventArgs>` | - |
| OnKeyDown | 交互式图标的按键回调 | `EventCallback<KeyboardEventArgs>` | - |
| Role | SVG 包装元素的 role；设为空字符串可从辅助技术中隐藏 | string | `img` |
| StopPropagation | 是否阻止点击事件冒泡 | bool | false |
| TabIndex | 包装元素的 `tabindex` 值 | string | - |
| Theme | 图标主题 | `IconThemeType` | `Outline` |
| TwoToneSecondaryColor | 可选的显式次色；设置后不再计算次色 | string | - |
| Type | 要渲染的生成图标名称 | string | - |
| Width | SVG 宽度 | string | `1em` |

</details>

### 自定义图标 {#custom-icon}

使用 `Component` 在图标包装元素内渲染自定义 `RenderFragment`。如果自定义 SVG 组件需要 `Icon` 提供的 `width`、`height`、`fill`、`focusable` 和 `aria-hidden` 属性，请使用 `ComponentTemplate`。

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Component | 自定义图标内容，用于替代生成的 SVG | RenderFragment | - |
| Rotate | 图标旋转角度 | int | `0` |
| Spin | 是否旋转图标并显示动画 | bool | false |
| Style | 行内样式，例如 `font-size` 和 `color` | string | - |

```razor
<Icon Component="HeartSvg" />

@code {
    private RenderFragment HeartSvg => @<svg viewBox="0 0 1024 1024" fill="currentColor">
        <path d="M512 900 150 540C-20 360 110 120 330 120c80 0 140 40 182 95 42-55 102-95 182-95 220 0 350 240 180 420L512 900z" />
    </svg>;
}
```

### 关于 SVG 图标 {#about-svg-icons}

图标目录以内联 SVG 渲染，不依赖图标字体。这种方式具有以下特点：

- 无需 CDN 字体文件或本地部署字体资源，可离线渲染。
- 在不同显示尺寸下保持清晰。
- 颜色默认继承 `currentColor`，也可通过 `Style` 和 `TwoToneColor` 覆盖。
- 可通过 `Component` 或 `ComponentTemplate` 渲染自定义 SVG。

可参考 [SVG 图标讨论](https://github.com/ant-design/ant-design/issues/10353) 了解 React 图标迁移背景。Blazor 使用生成的 Razor 组件和内联 SVG，无需 React 专用的 webpack/Vite loader 或 CSS-in-JS 分层配置。`AntDesign.Icons` NuGet 包提供生成图标目录；运行时选择名称时使用 `Icon.Type`，编译时确定图标时使用具名图标组件。

两种用法都支持通过行内样式设置尺寸和颜色：

```razor
@using AntDesign.Icons

<MessageOutlined Style="font-size: 16px; color: #08c;" />
<Icon Type="message" Theme="IconThemeType.Outline" Style="font-size: 16px; color: #08c;" />
```

### 设置双色图标颜色 {#set-two-tone-color}

生成式组件提供 `Class`、`Style`、`Spin`、`Rotate`、`Role`、`AriaLabel` 和 `AdditionalAttributes`。双色图标的 `TwoToneColor` 可传入主色，或主色与次色组成的数组。只传主色时，通过 `AntDesign.Colors.ColorPalette.Generate(primary)[0]` 计算次色；不传颜色时保留 SVG 原色。主题色板也使用同一套 Ant v6 颜色算法。例如，`#52c41a` 对应 `#f6ffed`。`IconColorPalette.GetSecondaryColor` 和 `IconService.GetSecondaryColor` 保留为兼容包装。

```razor
@using AntDesign.Icons

<SmileTwoTone TwoToneColor="#eb2f96" />
<HeartTwoTone TwoToneColor='@(new[] { "#1677ff", "#e6f4ff" })' />
<Icon Type="heart" TwoToneColor='@(new[] { "#1677ff", "#e6f4ff" })' />
```

### 自定义字体图标 {#custom-font-icon}

`IconFont` 继承 `Icon` 的显示参数并引用 SVG symbol。可使用应用已加载的资源，也可通过 `ScriptUrl` 在交互式渲染后加载。

在 [iconfont.cn](https://www.iconfont.cn/) 创建项目、选择应用需要的 symbol 并生成 JavaScript 资源，再将各 symbol 名称作为 `Type` 使用：

```razor
<IconFont Type="icon-example" ScriptUrl="//at.alicdn.com/t/font_example.js" />
```

`ScriptUrl` 也支持按顺序传入多个资源。helper 会加载并缓存各 URL，并将 sprite symbol 关联到对应的 `IconFont` 实例。

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| IconFont | 图标字体 symbol 名称，供 `IconFont` 使用 | string | - |
| Type | Symbol 名称，参数变化时同步更新 | string | - |
| ScriptUrl | 单个 JS 资源地址或有序数组，按 React 的 `createFromIconfontCN` 规则加载和缓存 | `OneOf<string, string[]>` | - |
| ExtraCommonProps | 应用到 SVG symbol 的额外属性，例如 `aria-label` 或 `data-*` | `IDictionary<string, object>` | - |

### 自定义 SVG 图标 {#custom-svg-icon}

React 的 `component` 属性接收 SVG React 组件。在 Blazor 中，独立或行内 SVG 可使用 `Component`；如果子组件需要接收 `Icon` 转发的 SVG 属性，则使用 `ComponentTemplate`。无需配置 webpack 或 Vite SVG loader。

React 常用 SVG 属性与 Blazor 参数的对应关系如下：

| React 属性 | Blazor 对应参数 |
| --- | --- |
| `className` | `Class` |
| `fill` | `Fill` |
| `height` | `Height` |
| `style` | `Style` |
| `width` | `Width` |

可使用 `ComponentTemplate` 将这些属性转发给生成式图标：

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| ComponentTemplate | 接收 SVG 属性的自定义组件模板；可将上下文传给生成式图标的 `AdditionalAttributes` | `RenderFragment<IReadOnlyDictionary<string, object>>` | - |

```razor
<Icon>
    <ComponentTemplate Context="attributes">
        <HomeOutlined AdditionalAttributes="attributes" />
    </ComponentTemplate>
</Icon>
```

## 常见问题

### 图标会改变组件样式的加载顺序吗？

React v6 文档提到，`@ant-design/icons` 的运行时样式注入与 `@layer antd` 及 `message`、`Modal`、`notification` 静态方法组合时可能影响样式优先级。这是 React 专属问题：Blazor 的生成式 `AntDesign.Icons` 组件直接渲染 SVG，不使用 React 的 CSS-in-JS 分层注入。若 Blazor 页面样式缺失，请检查 Ant Design Blazor 样式表和配置的主题资源是否正确加载；不要通过添加 React 图标组件来规避。
