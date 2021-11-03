---
category: Components
subtitle: 标签页
type: 数据展示
title: Tabs
cols: 1
cover: https://gw.alipayobjects.com/zos/antfincdn/lkI2hNEDr2V/Tabs.svg
---

选项卡切换组件。

## 何时使用

提供平级的区域将大块内容进行收纳和展现，保持界面整洁。

Ant Design 依次提供了三级选项卡，分别用于不同的场景。

- 卡片式的页签，提供可关闭的样式，常用于容器顶部。
- 既可用于容器顶部，也可用于容器内部，是最通用的 Tabs。
- [RadioButton](/components/radio/#components-radio-demo-radiobutton) 可作为更次级的页签来使用。

## API

### Tabs

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| ActiveKey | 当前激活 tab 面板的 key，可双向绑定 | string | 无 |
| Animated | 是否使用动画切换 Tabs，在 `tabPosition=top|bottom` 时有效 | bool | false |
| DefaultActiveKey | 初始化选中面板的 key，如果没有设置 activeKey | string | 第一个面板 |
| HideAdd | 是否隐藏加号图标，在 `type="editable-card"` 时有效 | boolean | false |
| Size | 大小，提供 `large` `default` 和 `small` 三种大小 | string | 'default' |
| TabBarExtraContent | tab bar 上额外的元素 | RenderFargment | 无 |
| TabBarExtraContentLeft |  tab bar 上额外的元素(左边) | RenderFargment | - |
| TabBarExtraContentRight | tab bar 上额外的元素(右边) | RenderFargment | - |
| TabBarGutter | tabs 之间的间隙 | number | 无 |
| TabBarStyle | tab bar 的样式对象 | object | - |
| TabPosition | 页签位置，可选值有 `top` `right` `bottom` `left` | string | 'top' |
| Type | 页签的基本样式，可选 `line`、`card` `editable-card` 类型 | string | 'line' |
| OnChange | 切换面板的回调 | Function(activeKey) {} | 无 |
| OnEdit | 新增和删除页签的回调，在 `type="editable-card"` 时有效 | (targetKey, action): void | 无 |
| OnClose | 删除页签的回调，在 `type="editable-card"` 时有效 | EventCallback(targetKey) {} | - |
| OnNextClick | next 按钮被点击的回调 | Function | 无 |
| OnPrevClick | prev 按钮被点击的回调 | Function | 无 |
| OnTabClick | tab 被点击的回调 | Function | 无 |
| Draggable | 使标签可拖拽调整顺序 | bool | false |

### Tabs.TabPane

| 参数        | 说明                      | 类型              | 默认值 |
| ----------- | ------------------------- | ----------------- | ------ |
| ForceRender | 被隐藏时是否渲染 DOM 结构 | boolean           | false  |
| Key         | 对应 activeKey            | string            | 无     |
| Tab         | 选项卡头显示文字          | string | 无     |
| TabTemplate | 选项卡头显示文字模板       | RenderFargment | 无     |
| TabContextMenu | Template for customer context menu | RenderFargment | 无 |