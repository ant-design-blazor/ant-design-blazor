---
category: Components
type: 导航
title: Steps
subtitle: 步骤条
cols: 1
cover: https://gw.alipayobjects.com/zos/antfincdn/UZYqMizXHaj/Steps.svg
---

引导用户按照流程完成任务的导航条。

## 何时使用

- 当任务复杂或者存在先后关系时，将其分解成一系列步骤，从而简化任务。


## API


Steps
The whole of the step bar.

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| ClassName | 步骤条类名 | string         | -         |
| Type   | 步骤条类型，有: `default`, `navigation`| string         |`default`       |
| Current | 指定当前步骤，从 0 开始记数。在子 Step 元素中，可以通过 status 属性覆盖状态 | int         |0       |
| Direction | 指定步骤条方向。目前支持水平和竖直两种方向 `horizontal` or `vertical`| string  | `horizontal`  |
| LabelPlacement | 指定标签放置位置，默认水平放图标右侧，可选 `vertical` 放图标下方| string  | `horizontal`  |
| ProgressDot | 点状步骤条，可以设置为一个 function，labelPlacement 将强制为 `vertical`| string  | `default`  |
| Size | 指定大小，目前支持普通（default）和迷你（small）| string  | `default`  |
| Status | 指定当前步骤的状态，可选 `wait` `process` `finish` `error`| string  | `process`  |
| Initial | 起始序号，从 0 开始记数| int  | -  |
| OnChange | 点击切换步骤时触发| function(e)  | -  |


Steps.Step
A single step in the step bar.

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Description | 步骤的详情描述，可选 | string         | -         |
| Icon | 步骤图标的类型，可选 | string         | -         |
| Status | 	指定状态。当不配置该属性时，会使用 Steps 的 current 来自动指定状态。可选： `wait` `process` `finish` `error` | string         | -         |
| Title | 标题 | string         | -         |
| SubTitle | 子标题 | string         | -         |
| Disabled | 禁用点击 | boolean         | false         |