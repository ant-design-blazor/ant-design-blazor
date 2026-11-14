---
category: Components
subtitle: 锚点
cols: 2
type: 其他
title: Anchor
cover: https://gw.alipayobjects.com/zos/alicdn/_1-C1JwsC/Anchor.svg
---

用于跳转到页面指定位置。

## 何时使用

需要展现当前页面上可供跳转的锚点链接，以及快速在锚点之间跳转。

## API

### Anchor Props

| 成员 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Affix | 固定模式 | boolean | true |  |
| Bounds | 锚点区域边界 | number | 5(px) |  |
| GetContainer | 指定滚动的容器 | () => HTMLElement | () => window |  |
| OffsetBottom | 距离窗口底部达到指定偏移量后触发 | number |  |  |
| OffsetTop | 距离窗口顶部达到指定偏移量后触发 | number |  |  |
| ShowInkInFixed | 固定模式是否显示小圆点 | boolean | false |  |
| OnClick | `click` 事件的 handler | Function(e: Event, link: Object) | - |  |
| GetCurrentAnchor | 自定义高亮的锚点 | () => string | - |  |
| TargetOffset | 锚点滚动偏移量，默认与 offsetTop 相同，[例子](#components-anchor-demo-targetOffset) | number | `offsetTop` |  |
| OnChange | 监听锚点链接改变 | (currentActiveLink: string) => void |  |  |
| Key | 当key发生改变时刷新链接列表 | string |  |  |
| ChildContent | 附加内容 | RenderFragment |  |  |

### Link Props

| 成员   | 说明                             | 类型              | 默认值 | 版本 |
| ------ | -------------------------------- | ----------------- | ------ | ---- |
| Href   | 锚点链接                         | string            |        |      |
| Title  | 文字内容                         | string\|ReactNode |        |      |
| Target | 该属性指定在何处显示链接的资源。 | string            |        |      |
