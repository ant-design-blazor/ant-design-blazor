---
category: Components
type: Other
title: BorderBeam
subtitle: 边框流光
description: 为容器边框提供持续流动的装饰性高亮效果。
cover: https://mdn.alipayobjects.com/huamei_7uahnr/afts/img/A*uae3QbkNCm8AAAAAAAAAAAAADrJ8AQ/original
coverDark: https://mdn.alipayobjects.com/huamei_7uahnr/afts/img/A*VcjGQLSrYdcAAAAAAAAAAAAADrJ8AQ/original
cols: 2
---

## 何时使用

- 需要强化某个容器的视觉关注度，但又不希望引入业务状态语义时。
- 适合登录面板、推荐卡片、AI 模块、重点 CTA 区域等场景。
- 它是装饰性效果，不应替代焦点态、校验态或业务状态边框。

## API

### BorderBeam

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| ChildContent | 装饰内容 | `RenderFragment` | - |
| Color | 流光颜色配置，支持单色字符串或渐变停靠点数组 | `OneOf<string, BorderBeamGradient[]>` | - |
| Count | 流光数量 | `double` | 1 |
| Duration | 流光完成一圈动画的时间，单位秒 | `double` | 6 |
| LineWidth | 流光线宽，数字类型按像素处理 | `object` | `1px` |
| Outset | 流光层相对容器边缘的外扩距离 | `object` | - |
| Size | 流光可见段的尺寸，数字类型按像素处理 | `object` | `100px` |

## FAQ

### 开启减少动态效果后会怎样？

当命中 `prefers-reduced-motion: reduce` 时，组件会隐藏 beam 效果。

### `Color` 中的 `Percent` 表示什么？

`Percent` 表示渐变停靠点的输入位置，取值范围为 `0 ~ 100`。组件会将这些停靠点映射到可见 beam 段内，并为尾部透明过渡保留空间。

### 为什么 BorderBeam 没有效果？

`BorderBeam` 会将 beam 层插入到第一个子元素内部。请确保子内容最终渲染为实际的 HTML 容器，并为它提供定位上下文，通常设置 `position: relative` 即可。

### 如何让流光边框跟随容器圆角？

组件会在初始化时读取宿主元素的计算后 `border-radius`。对于不规则圆角，可以直接在宿主元素上设置类似 `20px 20px 0 0` 的多值圆角，并使用 `Outset="0"`。
