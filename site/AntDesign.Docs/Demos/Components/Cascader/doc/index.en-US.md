---
category: Components
type: Data Entry
title: Cascader
cover: https://gw.alipayobjects.com/zos/alicdn/UdS8y8xyZ/Cascader.svg
---

Cascade selection box.

## When To Use

- When you need to select from a set of associated data set. Such as province/city/district, company level, things classification.
- When selecting from a large data set, with multi-stage classification separated for easy selection.
- Chooses cascade items in one float layer for better user experience.


## API

Cascader

| 参数                 |  说明	                                          |  类型               |  默认值  |  
| -------------------- | ---------------------------------------------------- | --------------------- | --------- |  
| AllowClear           |  whether allow clear                                      |  bool               |  true	      |
| BoundaryAdjustMode | `Dropdown` adjustment strategy (when for example browser resize is happening)         | TriggerBoundaryAdjustMode    | TriggerBoundaryAdjustMode.InView         |
| ChangeOnSelect       |  change value on each selection if set to true, see above demo for details  |  bool               |  false	      |
| Style                |  additional css class                                       |  string             |  -	          |
| DefaultValue         |  initial selected value                                      |  string             |  -	          |
| ExpandTrigger        |  expand current item when click or hover, one of 'click' 'hover'       |  string             |  'click'	  |
| Options	           |  data options of cascade                                    |  IList<AntCheckbox> |  -	          |
| Placeholder          |  input placeholder                                   |  string             |  'Please Select'	  |
| Size                 |  input size, one of 'large','middle' 'small'             |  string           |  无	          |
| OnChange             |  callback when finishing cascader select (List<CascaderNode>, string, string) => void  |   -   |  -            |


CascaderNode

| 参数             |  说明                    |  类型          | 默认值    |
| ---------------- | ------------------------ | -------------- | --------- |
| Label            |  Label                |  string        |  -        |
| Value            |  Value                   |  string        |  -        |
| Disabled         |  Disabled            |  bool          |  false    |
| HasChildren      |  HasChildren    |  bool          |  -        |
| Children         |  Children                 |  AntCheckbox[] |  -        |
