---
category: Components
subtitle: 瀑布流
type: 布局
title: Masonry
cover: https://mdn.alipayobjects.com/huamei_iwk9zp/afts/img/A*cELTRrM5HpAAAAAAOGAAAAgAegCCAQ/original
coverDark: https://mdn.alipayobjects.com/huamei_iwk9zp/afts/img/A*2CxJRYJmfbIAAAAAPqAAAAgAegCCAQ/original
cols: 1
tag: 6.0.0
---

瀑布流布局组件，用于展示不同高度的内容。

## 何时使用

- 展示不规则高度的图片或卡片时
- 需要按照列数均匀分布内容时
- 需要响应式调整列数时

## 代码演示

## API

### Masonry

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| ClassNames | 用于自定义组件内部各语义化结构的 class，支持对象或函数 | Record<[SemanticDOM](#semantic-dom), string> \| (info: { props })=> Record<[SemanticDOM](#semantic-dom), string> | - | 6.0.0 |
| Columns | 列数，可以是固定值或响应式配置 | `number \| { xs?: number; sm?: number; md?: number }` | `3` |  |
| Fresh | 是否持续监听子项尺寸变化 | `boolean` | `false` |  |
| Gutter | 间距，可以是固定值、响应式配置或水平垂直间距配置 | [Gap](#gap) \| \[[Gap](#gap), [Gap](#gap)\] | `0` |  |
| Items | 瀑布流项 | [MasonryItem](#masonryitem)[] | - |  |
| ItemRender | 自定义项渲染 | `(item: MasonryItem) => React.ReactNode` | - |  |
| Styles | 语义化结构 style，支持对象和函数形式 | Record<[SemanticDOM](#semantic-dom), CSSProperties> \| ((info: { props }) => Record<[SemanticDOM](#semantic-dom), CSSProperties>) | - | 6.0.0 |
| OnLayoutChange | 列排序回调 | `({ key: React.Key; column: number }[]) => void` | - |  |

### MasonryItem

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| ChildContent | 自定义展示内容，相对 `itemRender` 具有更高优先级 | `React.ReactNode` | - |
| Column | 自定义所在列 | `number` | - |
| Data | 自定义存储数据 | `T` | - |
| Height | 高度 | `number` | - |
| Key | 唯一标识 | `string` \| `number` | - |

### Gap

Gap 是项之间的间距，可以是固定值，也可以是响应式配置。

```ts
type Gap = undefined | number | Partial<Record<'xs' | 'sm' | 'md' | 'lg' | 'xl' | 'xxl', number>>;
```

## Semantic DOM

| 语义化结构 | 说明 |
| --- | --- |
| root | 根元素，设置相对定位、flex布局和瀑布流容器样式 |
| item | 条目元素，设置绝对定位、宽度计算、过渡动画和瀑布流项目样式 |

## Design Token

Masonry 没有组件特定的 Design Token。
