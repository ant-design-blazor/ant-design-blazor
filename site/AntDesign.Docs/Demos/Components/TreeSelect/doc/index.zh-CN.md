---
category: Components
subtitle: 树选择
type: 数据录入
title: TreeSelect
cover: https://gw.alipayobjects.com/zos/alicdn/Ax4DA0njr/TreeSelect.svg
---

树型选择控件。

## 何时使用

类似 Select 的选择控件，可选择的数据结构是一个树形结构时，可以使用 TreeSelect，例如公司层级、学科系统、分类目录等等。

## API

### Select props

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| AccessKey | 指定 [accesskey](https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/accesskey) 全局属性。  | string | |
| AllowClear | 支持清除. Has no effect if Value type default is also in the list of options, unless used with `ValueOnClear`. | bool | false |  |
| AutoClearSearchValue | 是否在选中项后清空搜索框 | bool | true |  |
| BoundaryAdjustMode | `Dropdown` 调整策略（例如当浏览器调整大小时）        | TriggerBoundaryAdjustMode    | TriggerBoundaryAdjustMode.InView         |
| CustomTagLabelToValue | 将自定义标签（字符串）转换为 TItemValue 类型 | Func<string, TItemValue> | (label) => <br/>    (TItemValue)TypeDescriptor<br/>    .GetConverter(typeof(TItemValue))<br/>    .ConvertFromInvariantString(label) |  |
| DefaultValues | 当`Mode = multiple` \| `tags` - 在初始化期间和在表单中按下重置按钮时使用这些值. | IEnumerable&lt;TItemValues> | - |  |
| Disabled | 是否禁用 | bool | false |  |
| DropdownMatchSelectWidth |  将匹配下拉宽度： <br/>- for boolean: `true` - 下拉列表中最宽的项目 <br/> - for string: with value (e.g.: `256px`). | OneOf<bool, string> | true |  |
| DropdownMaxWidth | 不允许下拉菜单的宽度超过此处的值（例如“768px”）. | string | "auto" |  |
| DropdownRender | 自定义下拉框内容 | Renderfragment | - |  |
| SearchDebounceMilliseconds |推迟对搜索输入事件的处理，直到用户停止输入一个预定的时间。 | int        | 250    |
| EnableSearch | 指示搜索功能是否处于活动状态。 对于Mode = `tags` 始终为 `true`。 | bool | false |  |
| HideSelected | 是否隐藏选中项. | bool | false |  |
| ItemValue    | 指定 Item 中作为选项 Value 的属性，也可以指定 Item 本身。(替代 ValueName）| Func<TITem,TItemValue> |  -  |
| ItemLabel    | 指定 Item 中作为选项 Label 的属性。(替代 LabelName）| Func<TITem,string> |  -  |
| LabelInValue | 是否在 value 中嵌入标签，将 value 的格式从 `TItemValue` 转换为 string (JSON) e.g. { "value": `TItemValue`, "label": "`标签值`" } | bool | false |  |
| LabelName | 用于标签的属性名称. | string |  |  |
| LabelTemplate | 用于自定义标签样式. | RenderFragment&lt;TItem> |  |  |
| Loading | 显示加载指示器。 必须编写加载逻辑. | bool | false |  |
| MaxTagCount | 最多显示多少个 tag，响应式模式会对性能产生损耗 | int | `ResponsiveTag.Responsive` | - |  |
| MaxTagTextLength | 最大显示的 tag 文本长度. | int | - |  |
| Mode | 设置 Select 的模式为多选或标签 - `default` \| `multiple` \| `tags` | string | default |  |
| OnClearSelected | 当用户清除选择时调用. | Action | - |  |
| OnFocus | 获得焦点时回调 | Action | - |  |
| OnMouseEnter | 鼠标移入时回调 | Action | - |  |
| OnMouseLeave | 鼠标移出时回调 | Action | - |  |
| OnSearch | 文本框值变化时回调 | EventCallback&lt;string> | - |  |
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
| Value |获取或设置选定的值。 | TItemValue | - |  |
| Values | 获取或设置选定的值(多选). | IEnumerable&lt;TItemValues> | - |  |
| ValueChanged | 用于双向绑定. | EventCallback&lt;TItemValue> | - |  |
| ValuesChanged | 用于双向绑定(多选). | EventCallback&lt;IEnumerable&lt;TItemValue>> | - |  |
| ValueName | 用于值的属性的名称. | string | - |  |
| ValueOnClear | 按下清除按钮时，值将设置为 ValueOnClear 中设置的值. | TItemValue | - | 0.11 |

### Tree props

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| ShowExpand | 显示展开图标 | boolean | true |  |
| ShowTreeLine | 显示连接线 | boolean | false |  |
| ShowIcon | 显示节点图标 | boolean | false |  |
| ShowLeafIcon | 显示子叶图标（如果 ShowLeafIcon 未赋值, 会等于 `ShowLine` 的值） | boolean | false |  |
| Multiple  |  允许选择多个树节点 | boolean | false  |   |
| TreeCheckable |  是否可勾选节点 | boolean  | false  |   |
| TreeCheckStrictly | checkable 状态下节点选择完全受控（父子节点选中状态不再关联） | boolean  | false  |   |
| ShowCheckedStrategy | 配置 treeCheckable 时，定义选中项回填的方式。ShowAll: 显示所有选中节点(包括父节点)。ShowParent: 只显示父节点(当父节点下所有子节点都选中时)。 默认只显示子节点 | TreeCheckedStrategy  | ShowChild  |   |
| CheckOnClickNode |  点击节点标题选中或取消选中节点 | boolean  | true  |   |
| SearchExpression  | 自定义搜索匹配方法  |  Func\<TreeNode\<TItem\>, bool\> | null  |   |
| MatchedStyle  | 搜索匹配关键字高亮样式 | string  | null  |   |
| MatchedClass  | 搜索匹配关键字高亮样式 | string  | null  |   |
| DataSource | 数据源 | List  |  null  |   |
| TitleExpression  |  指定一个方法，该表达式返回节点的文本。 | Func  |   |   |
| KeyExpression |  指定一个返回节点Key的方法。 |  Func |   |   |
| IconExpression  |  指定一个返回节点名称的方法。 | Func  |   |   |
| IsLeafExpression  | 返回一个值是否是页节点  | Func  |   |   |
| ChildrenExpression  | 返回子节点的方法  | Func  |   |   |
| DisabledExpression  |  指定一个返回禁用节点的方法 | Func  |   |   |
| CheckableExpression  |  指定一个返回可勾选节点的方法 | Func  |   |   |
| TreeDefaultExpandAll  |  默认展开所有节点 |  boolean  | false  |   |
| TreeDefaultExpandParent  |  默认展开顶级父节点 | boolean  | false  |   |
| TreeDefaultExpandedKeys  |  默认展开的节点 | string[]  | null |   |
| ExpandedKeys  |  （受控）展开指定的树节点 | string[]  |  null  |   |
| ExpandOnClickNode |  点击节点标题展开或收缩节点 | boolean  | false  |   |

### Tree EventCallback

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| OnNodeLoadDelayAsync  |  异步加载时回调，方法异步使用 async  | EventCallback  |   |   |
| OnTreeNodeSelect  |  选择节点时触发  | EventCallback  |   |   |

### Tree RenderFragment

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| TitleTemplate |  自定义标题模板 | RenderFragment  |   |   |
| TitleIconTemplate | 自定义标题Icon  |  RenderFragment |   |   |

### TreeNode props

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Key  |  节点key  | string  |   |   |
| Disabled |  是否禁用 | string  |   |   |
| Checkable | 是否可勾选  |  boolean |  true |   |
| Checked | 是否勾选（支持双向绑定）  |  boolean |  false |   |
| DisableCheckbox | 禁用复选框  |  boolean |  false |   |
| Selected | 是否选中（支持双向绑定）  |  boolean |  false |   |
| Expanded | 是否展开（支持双向绑定）  |  boolean |  false |   |
| Title | 标题  |  string |  false |   |
| TitleTemplate | 标题模板 | RenderFragment | null |  |
| Icon | 标题前图标  |  string |  false |   |
| IconTemplate | 标题前图标模板 | RenderFragment | null |  |
| DataItem | 数据项  |  Type |  |   | 
| SwitcherIcon | 该节点的展开图标 | string | null | |
| SwitcherIconTemplate | 该节点的展开图标 | RenderFragment | null | |


