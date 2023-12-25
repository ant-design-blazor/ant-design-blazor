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
| AccessKey | 指定 [accesskey](https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/accesskey) 全局属性。  | string | |
| AllowClear | 支持清除. Has no effect if Value type default is also in the list of options, unless used with `ValueOnClear`. | bool | false |  |
| AutoClearSearchValue | 是否在选中项后清空搜索框 | bool | true |  |
| Bordered | 是否有边框 | bool | true |  |
| BoundaryAdjustMode | `Dropdown` 调整策略（例如当浏览器调整大小时）        | TriggerBoundaryAdjustMode    | TriggerBoundaryAdjustMode.InView         |
| CustomTagLabelToValue | 将自定义标签（字符串）转换为 TItemValue 类型 | Func<string, TItemValue> | (label) => <br/>    (TItemValue)TypeDescriptor<br/>    .GetConverter(typeof(TItemValue))<br/>    .ConvertFromInvariantString(label) |  |
| DataSource | 此组件的数据源 | IEnumerable&lt;TItem> | - |  |
| DataSourceEqualityComparer | 将在 DataSource 更改检测期间使用的 EqualityComparer。 如果没有设置比较器，将使用默认比较器，它将仅比较标签和值属性的相等性. | IEqualityComparer&lt;TItem> | - |  |
| DefaultActiveFirstOption | 是否默认高亮第一个选项  | bool | false |  |
| DefaultValue | 当 `Mode = default` - 该值在初始化期间和在 Forms 中按下 Reset 按钮时使用. | TItemValue | - |  |
| DefaultValues | 当`Mode = multiple` \| `tags` - 在初始化期间和在表单中按下重置按钮时使用这些值. | IEnumerable&lt;TItemValues> | - |  |
| Disabled | 是否禁用 | bool | false |  |
| DisabledName | 用作禁用指示器的属性名称. | string |  |  |
| DisabledPredicate | 指定禁用选项的判断条件 |  -  |  -  |
| DropdownMatchSelectWidth |  将匹配下拉宽度： <br/>- for boolean: `true` - 下拉列表中最宽的项目 <br/> - for string: with value (e.g.: `256px`). | OneOf<bool, string> | true |  |
| DropdownMaxWidth | 不允许下拉菜单的宽度超过此处的值（例如“768px”）. | string | "auto" |  |
| DropdownRender | 自定义下拉框内容 | Renderfragment | - |  |
| SearchDebounceMilliseconds |推迟对搜索输入事件的处理，直到用户停止输入一个预定的时间。 | int        | 250    |
| EnableSearch | 指示搜索功能是否处于活动状态。 对于Mode = `tags` 始终为 `true`。 | bool | false |  |
| GroupName | 用作组指示符的属性的名称。 如果设置了该值，则条目将按组显示。 使用额外的 `SortByGroup` 和 `SortByLabel`. | string | - |  |
| HideSelected | 是否隐藏选中项. | bool | false |  |
| IgnoreItemChanges |用于提高速度。 如果希望更改标签名称、组名称或禁用指示器，请禁用此属性。 | bool | true |  |
| ItemTemplate | 用于自定义Item样式. | RenderFragment&lt;TItem> |  |  |
| LabelInValue | 是否在 value 中嵌入标签，将 value 的格式从 `TItemValue` 转换为 string (JSON) e.g. { "value": `TItemValue`, "label": "`标签值`" } | bool | false |  |
| LabelName | 用于标签的属性名称. | string |  |  |
| LabelTemplate | 用于自定义标签样式. | RenderFragment&lt;TItem> |  |  |
| LabelProperty | 指定 option 对象的 label属性 | `Func<TItem, string>` | - |
| Loading | 显示加载指示器。 必须编写加载逻辑. | bool | false |  |
| ListboxStyle | 自定义下拉列表样式 | string | display: flex; flex-direction: column; |  |
| MaxTagCount | 最多显示多少个 tag，响应式模式会对性能产生损耗 | int | `ResponsiveTag.Responsive` | - |  |
| MaxTagPlaceholder | 隐藏 tag 时显示的内容.如果与 ResponsiveTag.Responsive 一起使用，请实现处理逻辑。 | RenderFragment<IEnumerable<TItem>>> | - |  |
| MaxTagTextLength | 最大显示的 tag 文本长度. | int | - |  |
| Mode | 设置 Select 的模式为多选或标签 - `default` \| `multiple` \| `tags` | string | default |  |
| NotFoundContent | 当下拉列表为空时显示的内容 | RenderFragment | `Not Found` |  |
| OnBlur | 失去焦点时回调 | Action | - |  |
| OnClearSelected | 当用户清除选择时调用. | Action | - |  |
| OnCreateCustomTag | 创建自定义标签时调用. | Action | - |  |
| OnDataSourceChanged | 数据源更改时调用. | Action | - |  |
| OnDropdownVisibleChange | 展开下拉菜单的回调 | Action | - |  |
| OnFocus | 获得焦点时回调 | Action | - |  |
| OnMouseEnter | 鼠标移入时回调 | Action | - |  |
| OnMouseLeave | 鼠标移出时回调 | Action | - |  |
| OnSearch | 文本框值变化时回调 | Action&lt;string> | - |  |
| OnSelectedItemChanged | 当所选Item更改时调用. | Action&lt;TItem> | - |  |
| OnSelectedItemsChanged | 当所选Items(多选)更改时调用. | Action&lt;IEnumerable&lt;TItem>> | - |  |
| Open | 下拉菜单的打开状态. | bool | false |  |
| Placeholder | 选择框默认文本 | string | - |  |
| PopupContainerMaxHeight | 弹出容器的最大高度. | string | `256px` |  |
| PopupContainerSelector | 使用它来修复覆盖问题，例如 #area | string | body |  |
| PrefixIcon | 自定义前缀图标。 对于模式 `multiple` 和 `tags` 仅在未选择数据时可见. | RenderFragment | - |  |
| SelectOptions | 用于手动渲染选择Option. | RenderFragment | - |  |
| ShowArrowIcon | 是否显示下拉小箭头 | bool | true |  |
| ShowSearchIcon | 使单选模式可搜索 | bool | true |  |
| Size | 组件大小,可选: `small` \| `default` \| `large` | string | default |  |
| SortByGroup | 按组名排序项目. `None` \| `Ascending` \| `Descending` | SortDirection | SortDirection.None |  |
| SortByLabel | 按标签名对项目进行排序. `None` \| `Ascending` \| `Descending` | SortDirection | SortDirection.None |  |
| Style | 设置样式. | string | `width: 100%` |  |
| SuffixIcon | 自定义的选择框后缀图标 | RenderFragment | - |  |
| TokenSeparators | 定义哪些字符将被视为新创建标签的标记分隔符, 仅使用键盘创建新标签时很有用. | char[] | - |  |
| Value |获取或设置选定的值。 | TItemValue | - |  |
| Values | 获取或设置选定的值(多选). | IEnumerable&lt;TItemValues> | - |  |
| ValueChanged | 用于双向绑定. | EventCallback&lt;TItemValue> | - |  |
| ValuesChanged | 用于双向绑定(多选). | EventCallback&lt;IEnumerable&lt;TItemValue>> | - |  |
| ValueName | 用于值的属性的名称. | string | - |  |
| ValueOnClear | 按下清除按钮时，值将设置为 ValueOnClear 中设置的值. | TItemValue | - | 0.11 |
| ValueProperty | 指定 option 对象的 value 属性. | `Func<TItem, TItemValue>` | - |

### SelectOption props

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Class | Option 类名                  | string          | -      |      |
| Disabled  | 是否禁用                     | Boolean        | false  |      |
| Label     | 选择此选项后选择的标签内容    | string         | -      |      |
| Value     | 选择此选项后的 Select 值    | TItemValue          | -      |      |
| Item      | option 对象                | TItem          |  -  |    |