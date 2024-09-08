---
category: Components
type: Data Entry

title: TreeSelect
cover: https://gw.alipayobjects.com/zos/alicdn/Ax4DA0njr/TreeSelect.svg
---

Tree selection control.

## When To Use

`TreeSelect` is similar to `Select`, but the values are provided in a tree like structure. Any data whose entries are defined in a hierarchical manner is fit to use this control. Examples of such case may include a corporate hierarchy, a directory structure, and so on.

## API

### Select props

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| AccessKey | The [accesskey](https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/accesskey) global attribute.  | string | |
| AllowClear | Show clear button. Has no effect if Value type default is also in the list of options, unless used with `ValueOnClear`. | bool | false |  |
| AutoClearSearchValue | Whether the current search will be cleared on selecting an item. | bool | true |  |
| BoundaryAdjustMode | `Dropdown` adjustment strategy (when for example browser resize is happening)         | TriggerBoundaryAdjustMode    | TriggerBoundaryAdjustMode.InView         |
| CustomTagLabelToValue | Converts custom tag (a string) to TItemValue type. | Func<string, TItemValue> | (label) => <br/>    (TItemValue)TypeDescriptor<br/>    .GetConverter(typeof(TItemValue))<br/>    .ConvertFromInvariantString(label) |  |
| DefaultValues | When `Mode = multiple` \| `tags` -  The values are used during initialization and when pressing the Reset button within Forms. | IEnumerable&lt;TItemValues> | - |  |
| Disabled | Whether the Select component is disabled. | bool | false |  |
| DropdownMatchSelectWidth |  Will match drowdown width: <br/>- for boolean: `true` - with widest item in the dropdown list <br/> - for string: with value (e.g.: `256px`). | OneOf<bool, string> | true |  |
| DropdownMaxWidth | Will not allow dropdown width to grow above stated in here value (eg. "768px"). | string | "auto" |  |
| DropdownRender | Customize dropdown content. | Renderfragment | - |  |
| SearchDebounceMilliseconds | Delays the processing of the search input event until the user has stopped typing for a predetermined amount of time | int        |  250         |
| EnableSearch | Indicates whether the search function is active or not. Always `true` for mode `tags`. | bool | false |  |
| HideSelected | Hides the selected items when they are selected. | bool | false |  |
| ItemValue    | Specify the property in Item that is the Value of the option, can also specify the Item itself. (Used instead of ValueName) | Func<TITem,TItemValue> |  -  |
| ItemLabel    | Specifies the property in Item that is the option Label. (Used instead of LabelName)| Func<TITem,string> |  -  |
| LabelInValue | Whether to embed label in value, turn the format of value from `TItemValue` to string (JSON) e.g. { "value": `TItemValue`, "label": "`Label value`" } | bool | false |  |
| LabelName | The name of the property to be used for the label. | string |  |  |
| LabelTemplate | Is used to customize the label style. | RenderFragment&lt;TItem> |  |  |
| Loading | Show loading indicator. You have to write the loading logic on your own. | bool | false |  |
| MaxTagCount | Max tag count to show. responsive will cost render performance. | int | `ResponsiveTag.Responsive` | - |  |
| MaxTagTextLength | Max tag text length (number of characters) to show. | int | - |  |
| Mode | Set mode of Select - `default` \| `multiple` \| `tags` | string | default |  |
| OnClearSelected | Called when the user clears the selection. | Action | - |  |
| OnFocus | Called when focus. | Action | - |  |
| OnMouseEnter | Called when mouse enter. | Action | - |  |
| OnMouseLeave | Called when mouse leave. | Action | - |  |
| OnSearch | Callback function that is fired when input changed. | EventCallback&lt;string> | - |  |
| OnSelectedItemChanged | Called when the selected item changes. | Action&lt;TItem> | - |  |
| OnSelectedItemsChanged | Called when the selected items changes. | Action&lt;IEnumerable&lt;TItem>> | - |  |
| Open | Controlled open state of dropdown. | bool | false |  |
| Placeholder | Placeholder of select. | string | - |  |
| PopupContainerMaxHeight | The maximum height of the popup container. | string | `256px` |  |
| PopupContainerSelector | Use this to fix overlay problems e.g. #area | string | body |  |
| PrefixIcon | The custom prefix icon. For mode `multiple` and `tags` visible only when no data selected. | RenderFragment | - |  |
| SelectOptions | Used for rendering select options manually. | RenderFragment | - |  |
| ShowArrowIcon | Whether to show the drop-down arrow | bool | true |  |
| ShowSearchIcon | Whether show search input in single mode. | bool | true |  |
| Size | The size of the component. `small` \| `default` \| `large` | string | default |  |
| SortByGroup | Sort items by group name. `None` \| `Ascending` \| `Descending` | SortDirection | SortDirection.None |  |
| SortByLabel | Sort items by label value. `None` \| `Ascending` \| `Descending` | SortDirection | SortDirection.None |  |
| Style | Set CSS style. | string | `width: 100%` |  |
| SuffixIcon | The custom suffix icon. | RenderFragment | - |  |
| Value | Get or set the selected value. | TItemValue | - |  |
| Values | Get or set the selected values. | IEnumerable&lt;TItemValues> | - |  |
| ValueChanged | Used for the two-way binding. | EventCallback&lt;TItemValue> | - |  |
| ValuesChanged | Used for the two-way binding. | EventCallback&lt;IEnumerable&lt;TItemValue>> | - |  |
| ValueName | The name of the property to be used for the value. | string | - |  |
| ValueOnClear | When Clear button is pressed, Value will be set to whatever is set in ValueOnClear. | TItemValue | - | 0.11 |

### Tree props

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| ShowExpand | Shows an expansion icon before the node | boolean | true |  |
| ShowTreeLine | Shows a connecting line | boolean | false |  |
| ShowIcon | show treeNode icon icon | boolean | false |  |
| ShowLeafIcon | Displays the cotyledon icon | boolean | false |  |
| Multiple  |  Allows selecting multiple treeNodes | boolean | false  |   |
| TreeCheckable | Whether can be checked | boolean  | false  |   |
| TreeCheckStrictly | Check treeNode precisely; parent treeNode and children treeNodes are not associated | boolean  | false  |   |
| ShowCheckedStrategy | The way show selected item in box when treeCheckable set. Default: just show child nodes. ShowAll: show all checked treeNodes (include parent treeNode). ShowParent: show checked treeNodes (just show parent treeNode) | TreeCheckedStrategy  | ShowChild  |   |
| CheckOnClickNode |  Click title to check or uncheck the node | boolean  | true  |   |
| SearchExpression  | Customized matching expression  |  Func\<TreeNode\<TItem\>, bool\> | null  |   |
| MatchedStyle  | Search for matching text styles | string  | null  |   |
| MatchedClass | The  class name of matching text | string | null |  |
| DataSource | bing datasource | List  |  null  |   |
| TitleExpression  |  Specifies a method that returns the text of the node. | Func  |   |   |
| KeyExpression |  Specifies a method that returns the key of the node. |  Func |   |   |
| IconExpression  |  Specifies a method to return the node icon. | Func  |   |   |
| IsLeafExpression  | Specifies a method that returns whether the expression is a leaf node.  | Func  |   |   |
| ChildrenExpression  | Specifies a method  to return a child node  | Func  |   |   |
| DisabledExpression  |  Specifies a method to return a disabled node | Func  |   |   |
| CheckableExpression  |  Specifies a method to return a checkable node | Func  |   |   |
| TreeDefaultExpandAll  |  All tree nodes are expanded by default |  boolean  | false  |   |
| TreeDefaultExpandParent  |  The parent node is expanded by default | boolean  | false  |   |
| TreeDefaultExpandedKeys  |  Expand the specified tree nodes by default | string[]  | null |   |
| ExpandedKeys  |  (Controlled) expands the specified tree node | string[]  |  null  |   |
| ExpandOnClickNode |  Click title to expand or collapse the node | boolean  | false  |   |

### Tree EventCallback

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| OnNodeLoadDelayAsync  |   Lazy load callbacks. You must use async and the return type is Task, otherwise you may experience load lag and display problems | EventCallback  |   |   |
| OnTreeNodeSelect  |  Triggered when selecting a node  | EventCallback  |   |   |

### Tree RenderFragment

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| TitleTemplate |  Customize the header template | RenderFragment  |   |   |
| TitleIconTemplate | Customize the icon templates  |  RenderFragment |   |   |

### TreeNode props

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Key  |  key  | string  |   |   |
| Disabled |  disabled | string  |   |   |
| Checkable | Checkable  |  boolean |  true |   |
| Checked | checked (Supports two-way binding)  |  boolean |  false |   |
| DisableCheckbox |   |  boolean |  false |   |
| Selected | selected (Supports two-way binding)  |  boolean |  false |   |
| Expanded | expanded (Supports two-way binding)  |  boolean |  false |   |
| Title | title  |  string |  false |   |
| TitleTemplate | title template | RenderFragment | null |  |
| Icon | icon  |  string |  false |   |
| IconTemplate | icon template | RenderFragment | null |  |
| DataItem | dataitem |  Type |  |   | 
| SwitcherIcon | Customize node toggle icon, the value is Icon types  | string | null | |
| SwitcherIconTemplate | SwitcherIcon template | RenderFragment | null | |
