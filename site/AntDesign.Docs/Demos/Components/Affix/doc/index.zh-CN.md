---
category: Components
type: 导航
title: Affix
subtitle: 固钉
cover: https://gw.alipayobjects.com/zos/alicdn/tX6-md4H6/Affix.svg
---

将页面元素钉在可视范围。

## 何时使用

- 当内容区域比较长，需要滚动页面时，这部分内容对应的操作或者导航需要在滚动范围内始终展现。常用于侧边菜单和按钮组合。
- 页面可视范围过小时，慎用此功能以免遮挡页面内容。


## API

时间轴

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| OffsetBottom | 距离窗口底部达到指定偏移量后触发 | uint?         | -         |
| OffsetTop   | 距离窗口顶部达到指定偏移量后触发| uint?         |   0  |
| TargetSelector | 设置 Affix 需要监听其滚动事件的元素，值为 CSS 选择器 | string         |-       |
| ChildContent | 附加内容｜ RenderFragment | -         |
| OnChange | 固定状态改变时触发的回调函数| EventCallback&lt;bool>  | -  |

注意：`Affix` 内的元素不要使用绝对定位，如需要绝对定位的效果，可以直接设置 `Affix` 为绝对定位：


## FAQ
`Affix` 使用 `target` 绑定容器时，元素会跑到容器外。
