---
category: Components
subtitle: 评分
type: 数据录入
title: Rate
cover: https://gw.alipayobjects.com/zos/alicdn/R5uiIWmxe/Rate.svg
---

评分组件。

## 何时使用

- 对评价进行展示。
- 对事物进行快速的评级操作。

## API

| 属性 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| AllowClear | 是否允许再次点击后清除 | boolean | true |
| AllowHalf | 是否允许半选 | boolean | false |
| AutoFocus | 自动获取焦点 | boolean | false |
| Character | 自定义字符 | ReactNode | [<StarFilled /\>](/components/icon-cn/) |
| ClassName | 自定义样式类名 | string |  |
| Count | star 总数 | int | 5 |
| DefaultValue | 默认值 | int | 0 |
| Disabled | 只读，无法进行交互 | boolean | false |
| Style | 自定义样式对象 | CSSProperties |  |
| Tooltips | 自定义每项的提示信息 | string\[] |  |
| Value | 当前数，受控值 | number |  |
| OnBlur | 失去焦点时的回调 | Function() |  |
| OnChange | 选择时的回调 | Function(value: number) |  |
| OnFocus | 获取焦点时的回调 | Function() |  |
| OnHoverChange | 鼠标经过时数值变化的回调 | Function(value: number) |  |
| OnKeyDown | 按键回调 | Function(event) |  |

## 方法

| 名称    | 描述     |
| ------- | -------- |
| blur()  | 移除焦点 |
| focus() | 获取焦点 |
