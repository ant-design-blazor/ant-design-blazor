---
category: Components
type: 通用
title: FloatButton
subtitle: 悬浮按钮
description: 悬浮于页面上方的按钮。
cover: https://mdn.alipayobjects.com/huamei_7uahnr/afts/img/A*tXAoQqyr-ioAAAAAAAAAAAAADrJ8AQ/original
coverDark: https://mdn.alipayobjects.com/huamei_7uahnr/afts/img/A*hSAwR7cnabwAAAAAAAAAAAAADrJ8AQ/original
cols: 2
---

## 何时使用 {#when-to-use}

- 用于网站上的全局功能；
- 无论浏览到何处都可以看见的按钮。

## 代码演示 {#examples}

<!-- prettier-ignore -->
<code src="./demo/Basic.razor" iframe="360">基本</code>
<code src="./demo/Type.razor" iframe="360">类型</code>
<code src="./demo/Shape.razor" iframe="360">形状</code>
<code src="./demo/Content.razor" iframe="360">描述</code>
<code src="./demo/Tooltip.razor" iframe="360">含有气泡卡片的悬浮按钮</code>
<code src="./demo/Group.razor" iframe="360">浮动按钮组</code>
<code src="./demo/GroupMenu.razor" iframe="360">菜单模式</code>
<code src="./demo/Controlled.razor" iframe="360">受控模式</code>
<code src="./demo/Placement.razor" iframe="380" version="5.21.0">弹出方向</code>
<code src="./demo/Draggable.razor" iframe="380">可拖拽</code>
<code src="./demo/BackTop.razor" iframe="360">回到顶部</code>
<code src="./demo/ProgressRing.razor" iframe="360" version="6.6.0">滚动进度</code>
<code src="./demo/Badge.razor" iframe="360">徽标数</code>
<code src="./demo/BadgeDebug.razor" iframe="360" debug>调试小圆点使用</code>
<code src="./demo/StyleClass.razor" iframe="360" version="6.0.0">自定义语义结构的样式和类</code>
<code src="./demo/RenderPanel.razor" debug>\_InternalPanelDoNotUseOrYouWillBeFired</code>

## API

通用属性参考：[通用属性](/docs/react/common-props)

> 自 `antd@5.0.0` 版本开始提供该组件。

### 共同的 API

| 参数 | 说明 | 类型 | 默认值 | 版本 | [全局配置](/components/config-provider-cn#component-config) |
| --- | --- | --- | --- | --- | --- |
| icon | 自定义图标 | ReactNode | - |  | FloatButton: ×，BackTop: 5.27.0 |
| classNames | 用于自定义组件内部各语义化结构的 class，支持对象或函数 | Record<[SemanticDOM](#semantic-dom), string> \| (info: { props })=> Record<[SemanticDOM](#semantic-dom), string> | - |  | 6.0.0 |
| content | 文字及其它内容 | ReactNode | - |  | × |
| ~~description~~ | 请使用 `content` 代替 | ReactNode | - |  | × |
| tooltip | 气泡卡片的内容 | ReactNode \| [TooltipProps](/components/tooltip-cn#api) | - | TooltipProps: 5.25.0 | × |
| type | 设置按钮类型 | `default` \| `primary` | `default` |  | × |
| shape | 设置按钮形状 | `circle` \| `square` | `circle` |  | × |
| styles | 用于自定义组件内部各语义化结构的行内 style，支持对象或函数 | Record<[SemanticDOM](#semantic-dom), CSSProperties> \| (info: { props })=> Record<[SemanticDOM](#semantic-dom), CSSProperties> | - |  | 6.0.0 |
| onClick | 点击按钮时的回调 | (event) => void | - |  | × |
| href | 点击跳转的地址，指定此属性 button 的行为和 a 链接一致 | string | - |  | × |
| target | 相当于 a 标签的 target 属性，href 存在时生效 | string | - |  | × |
| htmlType | 设置 `button` 原生的 `type` 值，可选值请参考 [HTML 标准](https://developer.mozilla.org/zh-CN/docs/Web/HTML/Element/button#type) | `submit` \| `reset` \| `button` | `button` | 5.21.0 | × |
| badge | 带徽标数字的悬浮按钮（不支持 `status` 以及相关属性） | [BadgeProps](/components/badge-cn#api) | - | 5.4.0 | × |
| disabled | 按钮是否禁用 | boolean | - | 6.4.0 | × |

### FloatButton.Group

| 参数 | 说明 | 类型 | 默认值 | 版本 | [全局配置](/components/config-provider-cn#component-config) |
| --- | --- | --- | --- | --- | --- |
| shape | 设置包含的 FloatButton 按钮形状 | `circle` \| `square` | `circle` |  | × |
| classNames | 自定义按钮组 Semantic DOM 节点 class | `IDictionary<string, string>` | - | 6.0.0 | × |
| styles | 自定义按钮组 Semantic DOM 节点内联样式 | `IDictionary<string, string>` | - | 6.0.0 | × |
| trigger | 触发方式（有触发方式为菜单模式） | `click` \| `hover` | - |  | × |
| open | 受控展开，需配合 trigger 一起使用 | boolean | - |  | × |
| closeIcon | 自定义关闭按钮 | React.ReactNode | `<CloseOutlined />` |  | 5.16.0 |
| placement | 自定义菜单弹出位置 | `top` \| `left` \| `right` \| `bottom` | `top` | 5.21.0 | × |
| onOpenChange | 展开收起时的回调，需配合 trigger 一起使用 | (open: boolean) => void | - |  | × |
| onClick | 点击按钮时的回调（仅在菜单模式中有效） | (event) => void | - | 5.3.0 | × |

### FloatButton.BackTop

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| duration | 回到顶部所需时间（ms）；开启“减弱动态效果”（`prefers-reduced-motion: reduce`）时不生效 | number | 450 |  |
| showProgress | 在 BackTop 按钮边缘展示当前滚动进度环 | boolean | false | 6.6.0 |
| target | 设置需要监听其滚动事件的元素 | () => HTMLElement | () => window |  |
| visibilityHeight | 滚动高度达到此参数值才出现 BackTop | number | 400 |  |
| onClick | 点击按钮的回调函数 | () => void | - |  |

## Semantic DOM

### FloatButton

<code src="./demo/_semantic.razor" simplify="true"></code>

### FloatButton.Group

<code src="./demo/_semantic_group.razor" simplify="true"></code>

## 主题变量（Design Token）{#design-token}

<ComponentTokenTable component="FloatButton"></ComponentTokenTable>
