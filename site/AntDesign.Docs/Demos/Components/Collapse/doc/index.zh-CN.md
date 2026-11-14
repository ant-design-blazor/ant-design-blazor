---
category: Components
type: 数据展示
title: Collapse
subtitle: 折叠面板
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/IxH16B9RD/Collapse.svg
---

可以折叠/展开的内容区域。

## 何时使用

- 对复杂区域进行分组和隐藏，保持页面的整洁。
- `手风琴` 是一种特殊的折叠面板，只允许单个内容区域展开。

## API

### Collapse

| 参数                   | 说明                  | 类型                | 默认值  |
| ---------------------- | --------------------- | ------------------- | ------- |
| `Accordion`          | 是否每次只打开一个tab | `boolean`           | `false` |
| `Bordered`           | 是否有边框            | `boolean`           | `true`  |
| `ExpandIconPosition` | 设置图标位置          | `'left' \| 'right'` | `left`  |
| `Animation`          | 是否开启动画          | `boolean` | `false` |

### Panel

| 参数           | 说明                                       | 类型                       | 默认值  |
| -------------- | ------------------------------------------ | -------------------------- | ------- |
| `Disabled`     | 禁用后的面板展开与否将无法通过用户交互改变 | `boolean`                  | `false` |
| `Header`       | 面板头内容                                 | `string \| RenderFragment` | -       |
| `ExpandedIcon` | 自定义切换图标                             | `string \| RenderFragment` | -       |
| `Extra`        | 自定义渲染每个面板右上角的内容             | `string \| RenderFragment` | -       |
| `ShowArrow`    | 是否展示箭头                               | `boolean`                  | `true`  |
| `Active`       | 面板是否展开，可双向绑定                   | `boolean`                  | -       |
| `ActiveChange` | 面板展开回调                               | `EventCallback<boolean>`   | -       |

### 方法

| 名称    | 说明  |
| ------- | ------------ |
| Activate(string[] activeKeys)  | 展开指定 key 对应的 Panel |
| Deactivate(string[] inactiveKeys) | 关闭指定 key 对应的 Panel  |