---
category: Components
subtitle: 选择器
type: 数据录入
title: Select
cover: https://gw.alipayobjects.com/zos/alicdn/_0XzgOis7/Select.svg
---

下拉选择器。

## 何时使用

- 弹出一个下拉菜单给用户选择操作，用于代替原生的选择器，或者需要一个更优雅的多选器时。
- 当选项少时（少于 5 项），建议直接将选项平铺，使用 [Radio](/components/radio/) 是更好的选择。

## API


### Select props

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| AllowClear | 支持清除. Has no effect if Value type default is also in the list of options, unless used with `ValueOnClear`. | bool | false |  |
| AutoClearSearchValue | 是否在选中项后清空搜索框 | bool | true |  |
| Bordered | 是否有边框 | bool | true |  |
| BoundaryAdjustMode | `Dropdown` adjustment strategy (when for example browser resize is happening)         | TriggerBoundaryAdjustMode    | TriggerBoundaryAdjustMode.InView         |
| CustomTagLabelToValue | Converts custom tag (a string) to TItemValue type. | Func<string, TItemValue> | (label) => <br/>    (TItemValue)TypeDescriptor<br/>    .GetConverter(typeof(TItemValue))<br/>    .ConvertFromInvariantString(label) |  |
| DataSource | The datasource for this component. | IEnumerable&lt;TItem> | - |  |
| DataSourceEqualityComparer | EqualityComparer that will be used during DataSource change detection. If no comparer set, default comparer will be used that is going to compare only equiality of label & value properties. | IEqualityComparer&lt;TItem> | - |  |
| DefaultActiveFirstOption | 是否默认高亮第一个选项  | bool | false |  |
| DefaultValue | When `Mode = default` - The value is used during initialization and when pressing the Reset button within Forms. | TItemValue | - |  |
| DefaultValues | When `Mode = multiple` \| `tags` -  The values are used during initialization and when pressing the Reset button within Forms. | IEnumerable&lt;TItemValues> | - |  |
| Disabled | 是否禁用 | bool | false |  |
| DisabledName | The name of the property to be used as a disabled indicator. | string |  |  |
| DropdownMatchSelectWidth |  Will match drowdown width: <br/>- for boolean: `true` - with widest item in the dropdown list <br/> - for string: with value (e.g.: `256px`). | OneOf<bool, string> | true |  |
| DropdownMaxWidth | Will not allow dropdown width to grow above stated in here value (eg. "768px"). | string | "auto" |  |
| DropdownRender | 自定义下拉框内容 | Renderfragment | - |  |
| EnableSearch | Indicates whether the search function is active or not. Always `true` for mode `tags`. | bool | false |  |
| GroupName | The name of the property to be used as a group indicator. If the value is set, the entries are displayed in groups. Use additional `SortByGroup` and `SortByLabel`. | string | - |  |
| HideSelected | Hides the selected items when they are selected. | bool | false |  |
| IgnoreItemChanges | Is used to increase the speed. If you expect changes to the label name, group name or disabled indicator, disable this property. | bool | true |  |
| ItemTemplate | Is used to customize the item style. | RenderFragment&lt;TItem> |  |  |
| LabelInValue | Whether to embed label in value, turn the format of value from `TItemValue` to string (JSON) e.g. { "value": `TItemValue`, "label": "`Label value`" } | bool | false |  |
| LabelName | The name of the property to be used for the label. | string |  |  |
| LabelTemplate | Is used to customize the label style. | RenderFragment&lt;TItem> |  |  |
| Loading | Show loading indicator. You have to write the loading logic on your own. | bool | false |  |
| MaxTagCount | 最多显示多少个 tag，响应式模式会对性能产生损耗 | int | `ResponsiveTag.Responsive` | - |  |
| MaxTagPlaceholder | 隐藏 tag 时显示的内容. If used with `ResponsiveTag.Responsive`, implement your own handling logic. | RenderFragment<IEnumerable<TItem>>> | - |  |
| MaxTagTextLength | 最大显示的 tag 文本长度. | int | - |  |
| Mode | 设置 Select 的模式为多选或标签 - `default` \| `multiple` \| `tags` | string | default |  |
| NotFoundContent | 当下拉列表为空时显示的内容 | RenderFragment | `Not Found` |  |
| OnBlur | 失去焦点时回调 | Action | - |  |
| OnClearSelected | Called when the user clears the selection. | Action | - |  |
| OnCreateCustomTag | Called when custom tag is created. | Action | - |  |
| OnDataSourceChanged | Called when the datasource changes. | Action | - |  |
| OnDropdownVisibleChange | 展开下拉菜单的回调 | Action | - |  |
| OnFocus | 获得焦点时回调 | Action | - |  |
| OnMouseEnter | 鼠标移入时回调 | Action | - |  |
| OnMouseLeave | 鼠标移出时回调 | Action | - |  |
| OnSearch | 文本框值变化时回调 | Action&lt;string> | - |  |
| OnSelectedItemChanged | Called when the selected item changes. | Action&lt;TItem> | - |  |
| OnSelectedItemsChanged | Called when the selected items changes. | Action&lt;IEnumerable&lt;TItem>> | - |  |
| Open | Controlled open state of dropdown. | bool | false |  |
| Placeholder | 选择框默认文本 | string | - |  |
| PopupContainerMaxHeight | The maximum height of the popup container. | string | `256px` |  |
| PopupContainerSelector | Use this to fix overlay problems e.g. #area | string | body |  |
| PrefixIcon | The custom prefix icon. For mode `multiple` and `tags` visible only when no data selected. | RenderFragment | - |  |
| SelectOptions | Used for rendering select options manually. | RenderFragment | - |  |
| ShowArrowIcon | 是否显示下拉小箭头 | bool | true |  |
| ShowSearchIcon | 使单选模式可搜索 | bool | true |  |
| Size | The size of the component. `small` \| `default` \| `large` | string | default |  |
| SortByGroup | Sort items by group name. `None` \| `Ascending` \| `Descending` | SortDirection | SortDirection.None |  |
| SortByLabel | Sort items by label value. `None` \| `Ascending` \| `Descending` | SortDirection | SortDirection.None |  |
| Style | Set CSS style. | string | `width: 100%` |  |
| SuffixIcon | 自定义的选择框后缀图标 | RenderFragment | - |  |
| TokenSeparators | Define what characters will be treated as token separators for newly created tags. Useful when creating new tags using only keyboard. | char[] | - |  |
| Value | Get or set the selected value. | TItemValue | - |  |
| Values | Get or set the selected values. | IEnumerable&lt;TItemValues> | - |  |
| ValueChanged | Used for the two-way binding. | EventCallback&lt;TItemValue> | - |  |
| ValuesChanged | Used for the two-way binding. | EventCallback&lt;IEnumerable&lt;TItemValue>> | - |  |
| ValueName | The name of the property to be used for the value. | string | - |  |
| ValueOnClear | When Clear button is pressed, Value will be set to whatever is set in ValueOnClear. | TItemValue | - | 0.11 |

### SelectOption props

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Class | Option 器类名                  | string          | -      |      |
| Disabled  | 是否禁用                                 | Boolean        | false  |      |
| Label     | Label of Select after selecting this Option | string         | -      |      |
| Value     | Value of Select after selecting this Option | TItemValue          | -      |      |