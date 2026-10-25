---
category: Components
type: 通用
title: Icon
subtitle: 图标
cover: https://gw.alipayobjects.com/zos/alicdn/rrwbSt3FQ/Icon.svg
---

语义化的矢量图形。

## 如何使用

按常规方式安装 `AntDesign` 即可。NuGet 依赖会提供由 `AntDesign.Icons` 生成的 v6 图标目录；应用无需另行复制 SVG 文件或安装图标字体样式表。

通过 `Type` 指定图标名称，`Theme` 设置为 `Outline`、`Fill` 或 `TwoTone`。下方可搜索的图标库直接读取包中的图标目录和分类元数据。

生成式组件提供 `Class`、`Style`、`Spin`、`Rotate`、`Role`、`AriaLabel` 和 `AdditionalAttributes`。生成式双色组件的 `TwoToneColor` 类型为 `string[]`，可传一个主题色，或主题色与次要颜色两个值。下方 API 表介绍按名称使用的 `Icon` 适配组件。

只传主色时，生成式组件与 `Icon` 适配组件都使用 `AntDesign.Colors.ColorPalette.Generate(primary)[0]`。这个独立 C# 库迁移自 Ant v6 的 `@ant-design/colors`，默认/暗色主题 Token、预设色板、中性色和阴影也使用它；生成完整十档明暗色板，而不是 Icon 专用的近似算法。无需 JavaScript，首次渲染和服务端预渲染即可得到正确颜色。例如 `#52c41a` 对应 `#f6ffed`。`IconColorPalette.GetSecondaryColor` 和 `IconService.GetSecondaryColor` 保留为兼容包装。

与 React v6 对应的五个示例为：基本用法、多色图标、自定义图标、使用 iconfont.cn、使用多个 iconfont.cn 资源。基本用法和双色示例直接使用 `AntDesign.Icons` 的生成式组件。Blazor 另外提供可搜索、按分类分组的完整图标库，以及独立的动态双色改色示例。

## API

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Alt | 图标备用文本 | string | - |
| AriaLabel | `aria-label` 属性值 | string | - |
| Class | 附加 CSS 类名 | string | - |
| Component | 自定义图标内容，用于替代生成的 SVG | RenderFragment | - |
| ComponentTemplate | 接收 SVG 属性的自定义组件模板；可将上下文传给生成式图标的 `AdditionalAttributes` | `RenderFragment<IReadOnlyDictionary<string, object>>` | - |
| Fill | SVG 填充色 | string | `currentColor` |
| Height | SVG 高度 | string | `1em` |
| IconFont | 图标字体 symbol 名称，供 `IconFont` 使用 | string | - |
| OnClick | 点击回调 | `EventCallback<MouseEventArgs>` | - |
| Role | SVG 包装元素的 role；设为空字符串可从辅助技术中隐藏 | string | `img` |
| Rotate | 图标旋转角度 | int | `0` |
| Spin | 是否显示旋转动画 | bool | false |
| StopPropagation | 是否阻止点击事件冒泡 | bool | false |
| Style | 行内样式，例如 `font-size` 和 `color` | string | - |
| TabIndex | 包装元素的 `tabindex` 值 | string | - |
| Theme | 图标主题 | `IconThemeType` | `Outline` |
| TwoToneColor | 双主题图标的主色；设置后也会自动选择 TwoTone 主题 | string | - |
| Type | 要渲染的生成图标名称 | string | - |
| Width | SVG 宽度 | string | `1em` |

### IconFont

`IconFont` 继承 `Icon` 的显示参数并引用 SVG symbol。可使用应用已加载的资源，也可通过 `ScriptUrl` 在交互式渲染后加载。

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Type | Symbol 名称，参数变化时同步更新 | string | - |
| ScriptUrl | 单个 JS 资源地址或有序数组，按 React 的 `createFromIconfontCN` 规则加载和缓存 | `OneOf<string, string[]>` | - |

## 主题变量（Design Token）

React v6 和 Blazor 的 Icon 均无独立组件 Design Token。图标大小和颜色继承 CSS 的 `font-size` 与 `color`（也可通过 `Width`、`Height`、`Fill` 设置 SVG 属性）；若需全局调整这些继承值，请使用全局主题 token。
