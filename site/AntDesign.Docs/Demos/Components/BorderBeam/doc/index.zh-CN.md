---
category: Components
group: Other
type: Other
title: BorderBeam
subtitle: 动态边框光束
description: 沿容器边框移动的装饰光束。
---

## 何时使用

BorderBeam 可为功能卡片或重点面板添加装饰性的移动高光。它仅用于装饰，不能替代焦点、校验或状态提示。

## 示例

<code src="./demo/Basic.razor">基础用法</code>
<code src="./demo/Hover.razor">悬停显示</code>
<code src="./demo/Count.razor">多个光束</code>
<code src="./demo/CustomContainer.razor">自定义容器</code>
<code src="./demo/CustomizedColor.razor">渐变</code>
<code src="./demo/Duration.razor">持续时间</code>
<code src="./demo/Size.razor">尺寸</code>
<code src="./demo/LineWidth.razor">线宽</code>
<code src="./demo/NonUniformRadius.razor" debug>不规则圆角</code>
<code src="./demo/ComponentToken.razor" debug>组件 Token</code>

## API

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Count | 均匀分布的光束数量 | int | 1 |
| Color | 光束纯色或渐变色阶，尾部会渐隐为透明 | `OneOf<string, IReadOnlyList<BorderBeamGradientStop>>` | - |
| Duration | 光束绕行一圈的秒数 | double? | 6 |
| LineWidth | 光束线宽，例如 `2px` | string | 1px |
| Outset | 相对容器边框向外扩展的距离 | string | 根据容器边框计算 |
| Size | 可见光束线段的长度 | string | 100px |

将 `<BorderBeam />` 放在需要装饰的定位容器中。容器需要定位上下文，通常设置 `position: relative`。

```razor
<div style="position:relative; overflow:hidden; border:1px solid #eee; border-radius:12px; padding:24px">
    <BorderBeam />
    <h3>功能面板</h3>
    <p>装饰性边框高光。</p>
</div>
```

## 常见问题

### 启用减少动态效果时 BorderBeam 如何表现？

浏览器请求减少动态效果时，装饰光束会隐藏。

### Color 中的百分比是什么意思？

Blazor 的 `Color` 参数可接收 CSS 颜色字符串或 `BorderBeamGradientStop` 列表。每个色阶包含 CSS `Color` 和 0 到 100 的 `Percent`，最后一个色阶之后会渐隐为透明。

### 为什么 BorderBeam 没有显示？

请将组件放在要装饰的 DOM 容器内部，并为容器设置定位上下文，通常是 `position: relative`。
