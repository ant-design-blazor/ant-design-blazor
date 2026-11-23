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
- 如果需要用户自由输入，或在输入框中键入并选择建议项，请使用 [AutoComplete](/components/auto-complete/)。

## API


### Select props

以下先列出 React v6 Select 对应的 API。Blazor 参数使用 PascalCase；单选和多选分别通过 `ValueChanged` 与 `ValuesChanged` 提供 `onChange` 行为。

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| AllowClear | 使用布尔值显示清除按钮，或传入 `SelectAllowClearOptions` 配置对象，对应 React 的 `allowClear`。 | OneOf&lt;bool, SelectAllowClearOptions> | false | 6.7.0 |
| AutoComplete | 设置输入框的 HTML `autocomplete` 属性。默认值为 `new-password`，与 React v6.7.0 一致。 | string | `new-password` | 6.7.0 |
| ClearIcon | 旧版顶层自定义图标参数；配置对象中优先使用 `AllowClear.ClearIcon`。 | RenderFragment | 内置清除图标 | 6.0.0 |
| AutoClearSearchValue | 是否在选中项后清空搜索框 | bool | true |  |
| Bordered | **已弃用：**为兼容旧代码而保留。请使用 `Variant` 设置选择器外观。 | bool | true |  |
| ClassNames | 自定义 Select 语义节点的类名。弹出层等嵌套节点使用 `popup.root` 这样的点分键。 | IReadOnlyDictionary&lt;string, string> | - | 6.0.0 |
| ClassNamesFunc | 根据当前 Select 实例计算语义类名，是 React 函数形式 `classNames` 在 Blazor 中的回调写法；设置后优先于 `ClassNames`。 | Func&lt;Select&lt;TItemValue, TItem>, IReadOnlyDictionary&lt;string, string>> | - | 6.0.0 |
| DefaultActiveFirstOption | 下拉菜单打开时是否默认激活第一个未禁用选项。 | bool | true |  |
| DefaultOpen | 首次渲染时是否打开下拉菜单。初始化后可使用 `Open` 控制开合状态。 | bool | false | 6.0.0 |
| DefaultValue | 当 `Mode = default` - 该值在初始化期间和在 Forms 中按下 Reset 按钮时使用. | TItemValue | - |  |
| Direction | 此 Select 的文本方向，优先级高于从 `ConfigProvider` 继承的方向。 | string | 继承 |  |
| Disabled | 是否禁用 | bool | false |  |
| DropdownClassName | **已弃用：**旧版弹出层类名。请使用与 React `popupClassName` 对应的 `PopupClassName`。 | string | - |  |
| DropdownMatchSelectWidth | 控制下拉宽度：`true` 与 Select 触发器同宽；`false` 适配最宽选项、宽度不小于触发器，并关闭虚拟滚动；数字表示像素最小宽度；字符串可指定 CSS 宽度（例如 `256px`）。 | OneOf<bool, double, string> | true |  |
| PopupClassName | 应用于弹出层的额外类名，对应 React 的 `popupClassName`。 | string | - | 6.0.0 |
| PopupMatchSelectWidth | 按 React v6 语义控制弹层宽度：`true` 与触发器同宽；`false` 按内容适配并关闭虚拟滚动；数字表示像素最小宽度。字符串 CSS 宽度作为 Blazor 扩展保留。 | OneOf<bool, double, string> | true | 6.0.0 |
| DropdownRender | 自定义下拉框内容 | Renderfragment | - |  |
| PopupRender | 自定义下拉框内容，与 React 的 `popupRender` 对应，为 `DropdownRender` 的兼容别名。 | RenderFragment&lt;RenderFragment> | - | 6.0.0 |
| DropdownStyle | **已弃用：**旧版弹出层内联样式，对应 React 的 `dropdownStyle`。 | string | - |  |
| FieldNames | 将自定义选项字段映射到 `Label`、`Value`、嵌套 `Options` 和分组 `GroupLabel`；可选的 `Title`、`GroupClassName`、`GroupTitle` 用于映射选项或分组的原生属性。 | SelectFieldNames | `{ label: "label", value: "value", options: "options", groupLabel: "label" }` | 6.0.0 |
| FilterOption | 旧版顶层布尔过滤开关。请在 `ShowSearch` 中配置布尔值或 `(searchValue, option) => bool`；谓词形式可通过 `SelectShowSearchOptions<TItemValue, TItem>` 使用。 | bool? | true | 6.0.0 |
| FilterExpression | 自定义搜索时过滤的逻辑。 | Func<SelectOptionItem<TItemValue, TItem>, string, bool> | (item, searchValue) => item.Label.Contains(searchValue, StringComparison.InvariantCultureIgnoreCase) |  |
| FilterSort | 对过滤后的选项排序，第三个参数是当前搜索文本。 | Func&lt;SelectOptionItem&lt;TItemValue, TItem&gt;, SelectOptionItem&lt;TItemValue, TItem&gt;, string, int&gt; | - | 6.0.0 |
| GetPopupContainer | 根据触发器元素选择弹出层容器，对应 React 的 `getPopupContainer`。也可使用 CSS 选择器形式的 `PopupContainerSelector`。 | Func&lt;ElementReference, ElementReference> | - | 6.0.0 |
| LabelInValue | 是否在 value 中嵌入标签，将 value 的格式从 `TItemValue` 转换为 string (JSON) e.g. { "value": `TItemValue`, "label": "`标签值`" } | bool | false |  |
| ListHeight | 设置选项列表的最大高度，与 React `listHeight` 同名；别名对应 `PopupContainerMaxHeight`。 | int? | 256 | 6.0.0 |
| Loading | 显示加载指示器。 必须编写加载逻辑. | bool | false |  |
| LoadingIcon | 自定义下拉加载图标；未设置时显示内置图标。 | RenderFragment | 内置加载图标 | 6.0.0 |
| MaxCount | 多选或标签模式下可选择的最大数量 | int? | - | 6.0.0 |  |
| MaxTagCount | 最多显示的标签数量。设置为 `ResponsiveTag.Responsive` 时，会根据宽度折叠标签；响应式测量会增加性能开销。 | `OneOf<int, ResponsiveTag>` | -（不限制） |  |
| MaxTagPlaceholder | 隐藏 tag 时显示的内容.如果与 ResponsiveTag.Responsive 一起使用，请实现处理逻辑。 | RenderFragment<IEnumerable<TItem>>> | - |  |
| MaxTagTextLength | 最大显示的 tag 文本长度. | int | - |  |
| MenuItemSelectedIcon | 多选或标签模式下，已选选项显示的自定义图标。 | RenderFragment | 勾选图标 | 6.0.0 |
| Mode | 设置 Select 的模式为多选或标签 - `default` \| `multiple` \| `tags` | string | default |  |
| NotFoundContent | 当下拉列表为空时显示的内容 | RenderFragment | `Not Found` |  |
| Open | 下拉菜单的打开状态. | bool | false |  |
| OptionFilterProp | **已弃用：**旧的顶层别名；请放到 `ShowSearch` 配置中以匹配 React v6.1。多个字段按 OR 方式匹配。 | OneOf&lt;string, string[]> | value | 6.0.0 |
| OptionLabelProp | **已弃用：**与 React 旧参数名称兼容，对应设置选中标签字段的 `LabelName`。 | string | - |  |
| Options | 选择项数据源，与 React 的 `options` 同名对应，为 `DataSource` 的兼容别名。 | IEnumerable&lt;TItem> | - | 6.0.0 |
| OptionRender | 自定义下拉选项，匹配 React 的 `optionRender(option, { index })`。模板接收 `SelectOptionRenderInfo<TItemValue, TItem>`，其中包含选项元数据、源数据项，以及当前可见排序列表中的从零开始的索引。 | RenderFragment&lt;SelectOptionRenderInfo&lt;TItemValue, TItem>> | - | 6.0.0 |
| Placeholder | 选择框默认文本 | string | - |  |
| Placement | 弹出层的位置。 | Placement | `BottomLeft` |  |
| Prefix | 自定义前缀内容，与 React `prefix` 同名，对应 `PrefixIcon`。 | RenderFragment | - | 6.0.0 |
| RemoveIcon | 自定义已选标签的移除图标。 | RenderFragment | 关闭图标 | 6.0.0 |
| SearchValue | 受控搜索文本。通过 `SearchValueChanged` 或 `@bind-SearchValue` 同步输入框和筛选状态。 | string | - | 6.0.0 |
| ShowArrow | 是否显示下拉箭头，与 React 的 `showArrow` 对应，为 `ShowArrowIcon` 的兼容别名。 | bool | true | 6.0.0 |
| ShowSearch | 使用布尔值启用搜索，或传入配置对象。配置支持 `AutoClearSearchValue`、`FilterOption`、`OptionFilterProp`、`SearchValue`、`OnSearch` 和 `SearchIcon`；使用 `SelectShowSearchOptions<TItemValue, TItem>` 可配置强类型过滤谓词和 `FilterSort`。 | OneOf&lt;bool?, SelectShowSearchOptions> | 按模式决定 | 6.0.0 |
| Size | Select 输入框大小。Blazor 的 `Default` 对应 React 的 `medium`。 | `InputSize` | `Default`（对应 React 的 `medium`） |  |
| Status | 设置 `error` 或 `warning` 校验状态；未设置时继承 `FormItem` 的校验状态 | string | - |  |
| Styles | 自定义 Select 语义节点的内联样式。弹出层等嵌套节点使用 `popup.root` 这样的点分键。 | IReadOnlyDictionary&lt;string, string> | - | 6.0.0 |
| StylesFunc | 根据当前 Select 实例计算语义内联样式，是 React 函数形式 `styles` 在 Blazor 中的回调写法；设置后优先于 `Styles`。 | Func&lt;Select&lt;TItemValue, TItem>, IReadOnlyDictionary&lt;string, string>> | - | 6.0.0 |
| Suffix | 自定义后缀内容，与 React v6.7 的 `suffix` 参数名称一致，对应 `SuffixIcon`。 | RenderFragment | 内置箭头或搜索指示图标 | 6.7.0 |
| SuffixIcon | **旧版别名：**自定义后缀内容。使用 `Suffix` 与 React v6.7 保持一致。 | RenderFragment | - |  |
| TagRender | 自定义 `multiple` 或 `tags` 模式下的已选标签。模板上下文为 `SelectTagRenderInfo<TItemValue, TItem>`。 | RenderFragment&lt;SelectTagRenderInfo&lt;TItemValue, TItem>> | - | 6.0.0 |
| LabelRender | 自定义当前选中项的标签。与 React 参数名称对应；模板上下文为选项源数据项。 | RenderFragment&lt;TItem> | - | 6.0.0 |
| TokenSeparators | 创建标签时用于分词的字符，或将当前输入映射为已提交值的函数，与 React v6 一致。 | `OneOf<char[], Func<string, IEnumerable<string>>>` | - | 6.5.0 |
| Value |获取或设置选定的值。 | TItemValue | - |  |
| Variant | 选择器外观：`Outlined`、`Borderless`、`Filled` 或 `Underlined`。默认通过旧版 `Bordered` 值使用 `Outlined`。 | `InputVariant?` | `Outlined`（实际默认值） | 6.0.0 |
| Virtual | 是否启用虚拟滚动，与 React 的 `virtual` 对应，为 `EnableVirtualization` 的兼容别名。 | bool | true | 6.0.0 |
| OnActive | 弹出层打开时，高亮选项变化后传入该选项的值。 | EventCallback&lt;TItemValue> | - | 6.0.0 |
| OnBlur | Select 输入框失去焦点时调用，并接收焦点事件。 | EventCallback&lt;FocusEventArgs> | - |  |
| ValueChanged | 选中值变化时调用；单选模式下对应 React 的 `onChange`，也用于双向绑定。 | EventCallback&lt;TItemValue> | - |  |
| ValuesChanged | 选中值集合变化时调用；多选或标签模式下对应 React 的 `onChange`，也用于双向绑定。 | EventCallback&lt;IEnumerable&lt;TItemValue>> | - |  |
| OnClear | 清除操作完成后调用，与 React `onClear` 对应。 | EventCallback | - | 6.0.0 |
| OnDeselect | 多选或标签模式下取消选择选项时调用，参数为选项对象。 | EventCallback&lt;SelectOptionItem&lt;TItemValue, TItem>> | - | 6.0.0 |
| ~~OnDropdownVisibleChange~~ | 已弃用。下拉菜单可见状态变化时调用，请使用 `OnOpenChange`。 | Action&lt;bool> | - |  |
| OnOpenChange | 下拉菜单开合状态变化时的回调，与 React 的 `onOpenChange` 同名对应。 | EventCallback&lt;bool> | - | 6.0.0 |
| OnFocus | Select 输入框获得焦点时调用，并接收焦点事件。 | EventCallback&lt;FocusEventArgs> | - |  |
| OnInputKeyDown | Select 输入框获得焦点时按键调用。 | EventCallback&lt;KeyboardEventArgs> | - | 6.0.0 |
| OnPopupScroll | 下拉弹层滚动时调用。 | EventCallback&lt;EventArgs> | - | 6.0.0 |
| ~~OnSearch~~ | 已弃用的顶层搜索回调。请改在 `ShowSearch` 中配置 `OnSearch`。 | Action&lt;string> | - |  |
| OnSelect | 选择选项时调用，参数为包含值、标签和源数据的选项对象。 | EventCallback&lt;SelectOptionItem&lt;TItemValue, TItem>> | - | 6.0.0 |

#### AllowClear 配置

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| ClearIcon | 自定义清除图标。 | RenderFragment | 内置清除图标 |
| Disabled | 为 true 时禁用清除操作。 | bool? | false |
| Label | 清除按钮的辅助技术名称。 | string | `Clear` |
| TabIndex | 可选的清除按钮键盘顺序。原生按钮默认可通过键盘聚焦。 | int? | — |

#### ShowSearch 配置

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| AutoClearSearchValue | 多选或标签模式下，选中后是否清空搜索文本。 | bool? | true |
| FilterOption | `SelectShowSearchOptions` 中使用布尔开关；`SelectShowSearchOptions<TItemValue, TItem>` 中使用 `(searchValue, option) => bool` 谓词。强类型 option 上下文包含 `Value`、`Label` 和源数据 `Item`；泛型配置的布尔开关使用 `FilterOptionEnabled`。 | bool? 或 `Func<string, SelectOptionItem<TItemValue, TItem>, bool>` | true |
| FilterOptionEnabled | 使用 `SelectShowSearchOptions<TItemValue, TItem>` 谓词时，用于启用或关闭选项过滤的布尔开关。 | bool? | true |
| OptionFilterProp | 用于匹配的选项字段；多个字段按 OR 方式匹配。 | OneOf&lt;string, string[]> | `value` |
| SearchValue | 受控搜索文本。 | string | - |
| OnSearch | 搜索输入变化时调用。 | Action&lt;string> | - |
| SearchIcon | 搜索时显示的自定义后缀图标。 | RenderFragment | 内置搜索图标 |
| FilterSort | 对过滤结果排序。仅泛型配置 `SelectShowSearchOptions<TItemValue, TItem>` 提供此项，并保留选项源类型。 | Func&lt;SelectOptionItem&lt;TItemValue, TItem&gt;, SelectOptionItem&lt;TItemValue, TItem&gt;, string, int&gt; | - |

<details class="docs-blazor-api"><summary>Blazor 专属及旧版参数 (42)</summary>

以下参数用于扩展或保留 Blazor API，可与上方兼容 React 的参数同时使用。

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| 参数 | 说明 | 类型 | 默认值 | 版本 |
| AccessKey | 指定 [accesskey](https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/accesskey) 全局属性。  | string | |
| BoundaryAdjustMode | `Dropdown` 调整策略（例如当浏览器调整大小时）        | TriggerBoundaryAdjustMode    | TriggerBoundaryAdjustMode.InView         |
| CustomTagLabelToValue | 将自定义标签（字符串）转换为 TItemValue 类型 | Func<string, TItemValue> | (label) => <br/>    (TItemValue)TypeDescriptor<br/>    .GetConverter(typeof(TItemValue))<br/>    .ConvertFromInvariantString(label) |  |
| DataSource | 此组件的数据源 | IEnumerable&lt;TItem> | - |  |
| DataSourceEqualityComparer | 将在 DataSource 更改检测期间使用的 EqualityComparer。 如果没有设置比较器，将使用默认比较器，它将仅比较标签和值属性的相等性. | IEqualityComparer&lt;TItem> | - |  |
| DefaultValues | 初始选中值，也用于重置 Form 时恢复选项。 | IEnumerable&lt;TItemValue> | - |  |
| DisabledName | 用作禁用指示器的属性名称. | string |  |  |
| DisabledPredicate | 指定禁用选项的判断条件 |  -  |  -  |
| DropdownMaxWidth | 不允许下拉菜单的宽度超过此处的值（例如“768px”）. | string | "auto" |  |
| SearchDebounceMilliseconds |推迟对搜索输入事件的处理，直到用户停止输入一个预定的时间。 | int        | 250    |
| EnableSearch | 为单选启用搜索。多选和标签模式默认可搜索，与 Ant Design v6 保持一致。 | bool | false |  |
| GroupName | 用作组指示符的属性的名称。 如果设置了该值，则条目将按组显示。 使用额外的 `SortByGroup` 和 `SortByLabel`. | string | - |  |
| HideSelected | 是否隐藏选中项. | bool | false |  |
| IgnoreItemChanges |用于提高速度。 如果希望更改标签名称、组名称或禁用指示器，请禁用此属性。 | bool | true |  |
| ItemTemplate | 用于自定义Item样式. | RenderFragment&lt;TItem> |  |  |
| LabelName | 用于标签的属性名称. | string |  |  |
| LabelTemplate | 用于自定义标签样式. | RenderFragment&lt;TItem> |  |  |
| LabelProperty | 指定 option 对象的 label属性 | `Func<TItem, string>` | - |
| ListboxStyle | 自定义下拉列表样式 | string | display: flex; flex-direction: column; |  |
| OnClearSelected | 当用户清除选择时调用. | Action | - |  |
| OnCreateCustomTag | 创建自定义标签时调用. | Action | - |  |
| OnDataSourceChanged | 数据源更改时调用. | Action | - |  |
| OnMouseEnter | 鼠标移入时回调 | Action | - |  |
| OnMouseLeave | 鼠标移出时回调 | Action | - |  |
| SearchValueChanged | 搜索文本变化时触发，可用于受控绑定。 | EventCallback&lt;string> | - | 6.0.0 |
| OnSelectedItemChanged | 当所选Item更改时调用. | Action&lt;TItem> | - |  |
| OnSelectedItemsChanged | 当所选Items(多选)更改时调用. | Action&lt;IEnumerable&lt;TItem>> | - |  |
| PopupContainerMaxHeight | 弹出容器的最大高度. | string | `256px` |  |
| PopupContainerSelector | 使用它来修复覆盖问题，例如 #area | string | body |  |
| PrefixIcon | 自定义前缀图标。 对于模式 `multiple` 和 `tags` 仅在未选择数据时可见. | RenderFragment | - |  |
| SelectOptions | 用于手动渲染选择Option. | RenderFragment | - |  |
| ShowArrowIcon | 是否显示下拉小箭头 | bool | true |  |
| ShowSearchIcon | 使单选模式可搜索 | bool | true |  |
| SortByGroup | 按组名排序项目. `None` \| `Ascending` \| `Descending` | SortDirection | SortDirection.None |  |
| SortByLabel | 按标签名对项目进行排序. `None` \| `Ascending` \| `Descending` | SortDirection | SortDirection.None |  |
| Style | 设置样式. | string | `width: 100%` |  |
| Tokenize | 可选回调，将搜索文本转换为已提交的值。仅适用于多选和标签模式；输入尚未完成时返回空序列。 | Func&lt;string, IEnumerable&lt;string&gt;&gt; | - |  |
| Values | 获取或设置 `multiple` 或 `tags` 模式下的选中值。 | IEnumerable&lt;TItemValue> | - |  |
| ValueName | 用于值的属性的名称. | string | - |  |
| ValueOnClear | 按下清除按钮时，值将设置为 ValueOnClear 中设置的值. | TItemValue | - | 0.11 |
| ValueProperty | 指定 option 对象的 value 属性. | `Func<TItem, TItemValue>` | - |

</details>

### Select 方法

通过组件引用调用以下方法，对应 React Select 的 `blur()` 和 `focus()`。

| 方法 | 说明 | .NET 返回类型 |
| --- | --- | --- |
| Blur() | 移除 Select 输入框的焦点。 | Task |
| Focus() | 聚焦 Select 输入框。 | Task |

### 选项分组

React v6 使用 `OptGroup` 包含嵌套的 `Option`。Blazor 则通过选项数据表达分组：

| React v6 | Blazor 对应方式 |
| --- | --- |
| `OptGroup.label` | 嵌套数据使用 `FieldNames.GroupLabel`，平铺数据使用 `GroupName` |
| `OptGroup.className` | 可选字段 `FieldNames.GroupClassName` |
| `OptGroup.title` | 可选字段 `FieldNames.GroupTitle` |
| `OptGroup` 中的选项 | 嵌套数据使用 `FieldNames.Options` |
| `Option` 的 value 和 label | `FieldNames.Value` 和 `FieldNames.Label` |

嵌套数据会从分组对象读取 class 和 title；平铺数据则从每个分组中最先渲染的选项读取。平铺数据用法见[选项分组示例](#components-select-demo-optgroup)，嵌套数据见[字段名示例](#components-select-demo-fieldnames)。

### SelectOption props

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Class | Option 类名                  | string          | -      |      |
| Disabled  | 是否禁用                     | Boolean        | false  |      |
| Label     | 选择此选项后选择的标签内容    | string         | -      |      |
| Title     | 渲染在选项上的原生 `title` 属性。 | string | - | 6.0.0 |
| Value     | 选择此选项后的 Select 值    | TItemValue          | -      |      |
| Item      | option 对象                | TItem          |  -  |    |

## 常见问题

### 为什么 `tags` 模式搜索会出现重复选项？

当选项的 `label` 与 `value` 不同时可能出现这种情况。将 `ShowSearch.OptionFilterProp` 设为 `label`，即可按显示文本筛选。

### 如何控制点击 `PopupRender` 的内容时是否关闭下拉菜单？

Select 失去焦点时通常会关闭下拉菜单。若要由应用决定关闭时机，请使用受控参数 `Open` 和 `OnOpenChange` 回调。

### 如何在操作自定义弹出内容时保持下拉菜单打开？

阻止自定义内容的鼠标按下事件将焦点从 Select 移走。在 Razor 的 `PopupRender` 内容上使用 `@onmousedown:preventDefault`；必要时还可使用 `@onmousedown:stopPropagation`。

### 为什么自定义选项高度会导致虚拟滚动跳动？

虚拟滚动会按固定行高估算位置。请让自定义选项保持一致高度；如果行高不同，可设置 `Virtual="false"`。`ListHeight` 用于控制弹出列表的最大高度。

### 为什么无障碍检查会报告缺少 `aria-*` 属性？

Select 的部分无障碍关联只会在控件激活时创建。检查前请先打开控件；如果输入框需要可访问名称，请提供 `aria-label` 或 `aria-labelledby`。虚拟滚动只把长列表的一部分渲染进 DOM；若辅助技术需要遍历全部选项，可设置 `Virtual="false"`。

### 为什么点击自定义标签的关闭按钮会打开下拉菜单？

鼠标按下事件可能在标签关闭前冒泡到 Select 触发器。使用 Tag 的 `PreventMouseDownDefault` 和 `StopMouseDownPropagation` 参数，或在 `TagRender` 中实现相同行为，避免关闭操作打开弹层。
