---
category: Components
type: 数据录入
title: AutoComplete
subtitle: 自动完成
cover: https://gw.alipayobjects.com/zos/alicdn/qtJm4yt45/AutoComplete.svg
---

输入框自动完成功能。

## 何时使用

需要自动完成时。

## API

### AutoComplete

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Backfill | 使用键盘选择选项的时候把选中项回填到输入框中 | boolean | false |
| BoundaryAdjustMode | `Dropdown` adjustment strategy (when for example browser resize is happening)         | TriggerBoundaryAdjustMode    | TriggerBoundaryAdjustMode.InView         |
| Options | 自动完成的数据源 | AutocompleteDataSource | - |
| Disabled | 是否禁用 | bool | - |
| Placeholder | 占位符文本 | string | - |
| DefaultActiveFirstOption | 是否默认高亮第一个选项。 | boolean | true |
| Width | 自定义宽度单位 px | number | auto |
| OverlayClassName | 下拉根元素的类名称 | string | - |
| OverlayStyle | 下拉根元素的样式 | object | - |
| CompareWith | 对比，用于两个对象比较是否相同 | (o1: object, o2: object) => bool | (o1: object, o2: object) => o1===o2 |
| PopupContainerSelector | 菜单渲染父节点的选择器。默认渲染到 body 上，如果你遇到菜单滚动定位问题，试试修改为滚动的区域，并相对其定位。 | string | 'body' |
| Placeholder | 输入框提示 | string |  |
| AutoCompleteOptions | 列表对象集合 | list<AutoCompleteOption> |  |
| OptionDataItems | 绑定列表数据项格式的数据源 | AutoCompleteDataItem |  |
| OnSelectionChange | 选择项发生变化时，调用此函数 | function（）=>AutoCompleteOption |  |
| OnActiveChange | 当控件状态改变时，调用此函数 | function（）=>AutoCompleteOption |  |
| OnInput | 当控件输入时，调用此函数 | function（）=>ChangeEventArgs |  |
| OnPanelVisibleChange | 当面板显示状态是否改变 | function（）=>bool |  |
| ChildContent | 附加内容 | RenderFragment |  |
| OptionTemplate | 选项模板 | RenderFragment=>AutoCompleteDataItem |  |
| OptionFormat | 格式化选项，可以自定义显示格式 | RenderFragment=>AutoCompleteDataItem |  |
| OverlayTemplate | 所有选项模板 | RenderFragment |  |
| FilterExpression | 过滤表达式 | function(option, value)=>AutoCompleteDataItem |  |
| AllowFilter | 是否允许过滤 | bool | true |
| ShowPanel | 是否操作时显示面板 | bool | false |

### AutoCompleteOption

| 属性 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Value | 绑定到触发元素 ngModel 的值 | object | - |
| Label | 填入触发元素显示的值 | string | - |
| Disabled | 禁用选项 | boolean | false |

