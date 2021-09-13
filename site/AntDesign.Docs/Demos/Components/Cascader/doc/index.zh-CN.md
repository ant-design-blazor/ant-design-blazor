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
| BoundaryAdjustMode | `Dropdown` adjustment strategy (when for example browser resize is happening)         | TriggerBoundaryAdjustMode    | TriggerBoundaryAdjustMode.InView         |
| ChangeOnSelect       |  当此项为 true 时，点选每级菜单选项值都会发生变化  |  bool               |  false	      |
| Style                |  自定义类名                                        |  string             |  -	          |
| DefaultValue         |  默认的选中项                                      |  string             |  -	          |
| ExpandTrigger        |  次级菜单的展开方式，可选 'click' 和 'hover'       |  string             |  'click'	  |
| Options	           |  可选项数据源                                      |  IList<AntCheckbox> |  -	          |
| Placeholder          |  输入框占位文本                                    |  string             |  '请选择'	  |
| Size                 |  输入框大小，可选 'large','middle' 'small'         |  string           |  无	          |
| OnChange             |  选择完成后的回调 (List<CascaderNode>, string, string) => void  |   -   |  -            |


CascaderNode

| 参数             |  说明                    |  类型          | 默认值    |
| ---------------- | ------------------------ | -------------- | --------- |
| Label            |  节点名称                |  string        |  -        |
| Value            |  节点值                  |  string        |  -        |
| Disabled         |  节点是否禁用            |  bool          |  false    |
| HasChildren      |  是否存在子节点(只读)    |  bool          |  -        |
| Children         |  子节点                  |  AntCheckbox[] |  -        |
