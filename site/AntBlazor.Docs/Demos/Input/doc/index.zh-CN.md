---
category: Components
type: 数据录入
title: Input
subtitle: 输入框
cover: https://gw.alipayobjects.com/zos/alicdn/xS9YEJhfe/Input.svg
---

通过鼠标或键盘输入内容，是最基础的表单域的包装。

## 何时使用

- 需要用户输入表单域内容时。
- 提供组合型输入框，带搜索的输入框，还可以进行大小选择。


## API

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| AddOnBefore | 带标签的 input，设置前置标签                               | RenderFragment        | -         |
| AddOnAfter            | 带标签的 input，设置后置标签           | RenderFragment         |
| ChildContent            |子组件           | RenderFragment         |-       |
| Size |抽屉元素之间的子组件  `default`, `large`, `small`        | string        | -         |
| Placeholder              |提供可描述输入字段预期值的提示信息        | string        | -        |
| DefaultValue |输入框默认内容                              | string        | -         |
| MaxLength |最大长度        | int         |
| Disabled |是否禁用状态，默认为 false                               | string        | -         |
| AllowClear |可以点击清除图标删除内容                               | boolean        | false         |
| Prefix | 带有前缀图标的 input                               | RenderFragment        | -        |
| Suffix | 带有后缀图标的 input                               | RenderFragment        | -         |
| Type            |声明 input 类型，同原生 input 标签的 type 属性，见：MDN(请直接使用 Input.TextArea 代替 type="textarea")。         | string  | -         |
| OnChange |输入框内容变化时的回调                                | function(e)        | -        |
| OnPressEnter | 按下回车的回调                              | function(e)        | -         |
| OnInput |输入时的回调                               | function(e)        | -         |

