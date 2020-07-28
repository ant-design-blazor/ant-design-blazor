---
category: Components
type: 数据展示
title: Timeline
subtitle: 时间轴
cover: https://gw.alipayobjects.com/zos/antfincdn/vJmo00mmgR/Timeline.svg
---

垂直展示的时间流信息。

## 何时使用

- 当有一系列信息需按时间排列时，可正序和倒序。
- 需要有一条时间轴进行视觉上的串联时。




## API

时间轴

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Pending | 指定最后一个幽灵节点是否存在或内容 | RenderFragment         | -         |
| PendingDot   | 当最后一个幽灵节点存在時，指定其时间图点| RenderFragment         |
| Reverse | 节点排序 | boolean         |-       |
| Mode |通过设置 mode 可以改变时间轴和内容的相对位置，可选 `alternate` `left` `right` | string  | -  |

Timeline.Item
时间轴的每一个节点。

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Color | 指定圆圈颜色 blue, red, green, gray，或自定义的色值 | string,         | -         |
| Dot   | 自定义时间轴点| string         |
| Position | 自定义节点位置 `left`,`right`| string         |-       |
| Label |设置标签 | RenderFragment  | -  |
