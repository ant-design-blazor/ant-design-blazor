---
category: Components
type: 数据录入
title: Cascader
subtitle: 级联选择
cover: https://gw.alipayobjects.com/zos/alicdn/UdS8y8xyZ/Cascader.svg
---

级联选择框。

## 何时使用

- 需要从一组相关联的数据集合进行选择，例如省市区，公司层级，事物分类等。
- 从一个较大的数据集合中进行选择时，用多级分类进行分隔，方便选择。
- 比起 Select 组件，可以在同一个浮层中完成选择，有较好的体验。


## API

Cascader

| 参数                 |  说明	                                          |  类型               |  默认值  |  
| -------------------- | ---------------------------------------------------- | --------------------- | --------- |  
| AllowClear           |  是否支持清除                                      |  bool               |  true	      |
| BoundaryAdjustMode   |  边界自动调整的模式，默认在可视范围调整        | TriggerBoundaryAdjustMode    | TriggerBoundaryAdjustMode.InView         |
| ChangeOnSelect       |  当此项为 true 时，点选每级菜单选项值都会发生变化  |  bool               |  false	      |
| Multiple             |  启用复选框多选                                      |  bool               |  false       |
| Values               |  多选模式下已选叶子节点的值                          |  IEnumerable<string> |  -          |
| ShowCheckedStrategy  |  控制已选标签显示父节点、子节点或全部节点            |  CascaderCheckedStrategy | ShowParent |
| Style                |  自定义类名                                        |  string             |  -	          |
| DefaultValue         |  默认的选中项                                      |  string             |  -	          |
| ExpandTrigger        |  次级菜单的展开方式，可选 'click' 和 'hover'       |  string             |  'click'	  |
| Options	           |  可选项数据源                                      |  IList<AntCheckbox> |  -	          |
| Placeholder          |  输入框占位文本                                    |  string             |  '请选择'	  |
| Placement            |  浮层预设位置                                      | `BottomLeft` \| `BottomRight` \| `TopLeft` \| `TopRight` |  -  |
| Size                 |  输入框大小，可选 'large','middle' 'small'         |  string           |  无	          |
| OnChange             |  选择完成后的回调 (List<CascaderNode>, string, string) => void  |   -   |  -            |


CascaderNode

| 参数             |  说明                    |  类型          | 默认值    |
| ---------------- | ------------------------ | -------------- | --------- |
| Label            |  节点名称                |  string        |  -        |
| Value            |  节点值                  |  string        |  -        |
| Disabled         |  节点是否禁用            |  bool          |  false    |
| DisableCheckbox  |  仅禁用节点复选框，不禁用选项 | bool | false |
| HasChildren      |  是否存在子节点(只读)    |  bool          |  -        |
| Children         |  子节点                  |  AntCheckbox[] |  -        |

## Semantic DOM

通过 `ClassNames` 和 `Styles` 设置语义节点，支持 `root`、`prefix`、`suffix`、
`input`、`content`、`placeholder`、`clear`、`popup.root`、`popup.list` 和
`popup.listItem`。弹出列表插槽会应用到每一级级联菜单。
