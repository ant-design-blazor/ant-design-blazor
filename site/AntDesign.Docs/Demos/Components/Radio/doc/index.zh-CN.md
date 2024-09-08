---
category: Components
type: 数据录入
title: Radio
subtitle: 单选框
cover: https://gw.alipayobjects.com/zos/alicdn/8cYb5seNB/Radio.svg
---

单选框。

## 何时使用

-用于在多个备选项中选中单个状态。
-和 `Select` 的区别是，`Radio` 所有选项默认可见，方便用户在比较中选择，因此选项不宜过多。



## API
Radio/Radio.Button

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| AutoFocus | 自动获取焦点                               | boolean        | -         |
| Checked            | 指定当前是否选中           | boolean         |
| DefaultChecked            | 初始是否选中          | boolean         |-       |
| Disabled |	禁用 Radio        | string        | -         |
| RadioButton | 设置为 TRUE 以将radio风格设置为按钮组 | bool | false |
| Value              | 根据 value 进行比较，判断是否选中        | string        | -        |

RadioGroup

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| ButtonStyle            | RadioButton 的风格样式，目前有描边和填色两种风格          | `outline`,`solid`         |-       |
| Disabled |禁选所有子单选器       | string        | -         |
| Value              | 用于设置当前选中的值        | string        | -        |
| Name            | RadioGroup 下所有 input[type="radio"] 的 name 属性          | string         |-       |
| Size |	大小，只对按钮样式生效        | string        | -         |
| onChange              | 选项变化时的回调函数       | function(e)        | -        |
