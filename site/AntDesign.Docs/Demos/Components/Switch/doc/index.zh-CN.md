---
category: Components
type: 数据录入
title: Switch
subtitle: 开关
cover: https://gw.alipayobjects.com/zos/alicdn/zNdJQMhfm/Switch.svg
---

开关选择器。

## 何时使用

- 需要表示开关状态/两种状态之间的切换时；
- 和 `checkbox`的区别是，切换 `switch` 会直接触发状态改变，而 checkbox 一般用于状态标记，需要和提交操作配合。


## API

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| AutoFocus | 自动获取焦点                             | boolean        | false         |
| Checked            | 指定当前是否选中         | boolean         |
| CheckedChildren            | 选中时的内容           | RenderFragment         |-       |
| Control  | 是否完全由用户控制状态 | boolean | false |
| DefaultChecked |初始是否选中     | string        | -         |
| Disabled              | 	是否禁用       | string        | -        |
| Loading | 加载中的开关                             | boolean        | -         |
| Size | 开关大小，可选值：default small       | string         |
| UnCheckedChildren | 非选中时的内容                           | RenderFragment        | -         |
| OnChange |变化时回调函数                            | function(e)        | -         |
| OnClick |trigger when the is clicked. Very useful in combination with `Control` parameter | function(e)        | -         |

