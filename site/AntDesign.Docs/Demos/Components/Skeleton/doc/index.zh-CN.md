---
category: Components
subtitle: 骨架屏
type: 反馈
title: Skeleton
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/KpcciCJgv/Skeleton.svg
---

在需要等待加载内容的位置提供一个占位图形组合。

## 何时使用

- 网络较慢，需要长时间等待加载处理的情况下。
- 图文信息内容较多的列表/卡片中。
- 只适合用在第一次加载数据的场景。
- 可以被 Spin 完全代替，但是在可用的场景下可以比 Spin 提供更好的视觉效果和用户体验

## API

### Skeleton

| 属性             | 说明                                                                       | 类型                                     | 默认值  |
| ---------------- | -------------------------------------------------------------------------- | ---------------------------------------- | ------- |
| `Active`         | 是否展示动画效果                                                           | `boolean`                                | `false` |
| `Avatar`         | 是否显示头像占位图                                                         | `boolean`                                | `false` |
| `AvatarSize`     | 设置头像占位图的大小                                                       | `int \| 'large' \| 'small' \| 'default'` | -       |
| `AvatarShape`    | 指定头像的形状                                                             | `'circle' \| 'square'`                   | -       |
| `Loading`        | 为 `true` 时，显示占位图。反之则直接展示子组件                             | `boolean`                                | -       |
| `Paragraph`      | 是否显示段落占位图                                                         | `boolean`                                | `true`  |
| `ParagraphRows`  | 设置段落占位图的行数                                                       | `int`                                    | -       |
| `ParagraphWidth` | 设置标题占位图的宽度，若为数组时则为对应的每行宽度，反之则是最后一行的宽度 | `int \| string \| Array<int \| string>`  | -       |
| `Title`          | 是否显示标题占位图                                                         | `boolean`                                | `true`  |
| `TitleWidth`     | 设置标题占位图的宽度                                                       | `int \| string`                          | -       |


### SkeletonElement Type="button"

| 属性     | 说明             | 类型                               | 默认值      |
| -------- | ---------------- | ---------------------------------- | ----------- |
| `Active` | 是否展示动画效果 | `boolean`                          | `false`     |
| `Size`   | 大小             | `'large' \| 'small' \| 'default'`  | `'default'` |
| `Shape`  | 形状             | `'circle' \| 'round' \| 'default'` | `'default'` |

### SkeletonElement Type="avatar"

| 属性     | 说明             | 类型                                     | 默认值      |
| -------- | ---------------- | ---------------------------------------- | ----------- |
| `Active` | 是否展示动画效果 | `boolean`                                | `false`     |
| `Size`   | 大小             | `int \| 'large' \| 'small' \| 'default'` | `'default'` |
| `Shape`  | 形状             | `'circle' \| 'square'`                   | `'square'`  |

### SkeletonElement Type="input"

| 属性     | 说明             | 类型                              | 默认值      |
| -------- | ---------------- | --------------------------------- | ----------- |
| `Active` | 是否展示动画效果 | `boolean`                         | `false`     |
| `Size`   | 大小             | `'large' \| 'small' \| 'default'` | `'default'` |