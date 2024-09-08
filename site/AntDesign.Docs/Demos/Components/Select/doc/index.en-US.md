---
category: Components
type: Data Entry
title: Select
cover: https://gw.alipayobjects.com/zos/alicdn/_0XzgOis7/Select.svg
---

Select component to select value from options.

## When To Use

- A dropdown menu for displaying choices - an elegant alternative to the native `<Select>` element.
- Utilizing [Radio](/components/radio/) is recommended when there are fewer total options (less than 5).

## API


### Select props

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| AccessKey | The [accesskey](https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/accesskey) global attribute.  | string | |
| AllowClear | Show clear button. Has no effect if Value type default is also in the list of options, unless used with `ValueOnClear`. | bool | false |  |
| AutoClearSearchValue | Whether the current search will be cleared on selecting an item. | bool | true |  |
| AutoFocus | get focus when component mounted                             | boolean        | false         |
| Bordered | Toggle the border style. | bool | true |  |
| BoundaryAdjustMode | `Dropdown` adjustment strategy (when for example browser resize is happening)         | TriggerBoundaryAdjustMode    | TriggerBoundaryAdjustMode.InView         |
| CustomTagLabelToValue | Converts custom tag (a string) to TItemValue type. | Func<string, TItemValue> | (label) => <br/>    (TItemValue)TypeDescriptor<br/>    .GetConverter(typeof(TItemValue))<br/>    .ConvertFromInvariantString(label) |  |
| DataSource | The datasource for this component. | IEnumerable&lt;TItem> | - |  |
| DataSourceEqualityComparer | EqualityComparer that will be used during DataSource change detection. If no comparer set, default comparer will be used that is going to compare only equiality of label & value properties. | IEqualityComparer&lt;TItem> | - |  |
| DefaultActiveFirstOption | Activates the first item that is not deactivated.  | bool | false |  |
| DefaultValue | When `Mode = default` - The value is used during initialization and when pressing the Reset button within Forms. | TItemValue | - |  |
| DefaultValues | When `Mode = multiple` \| `tags` -  The values are used during initialization and when pressing the Reset button within Forms. | IEnumerable&lt;TItemValues> | - |  |
| Disabled | Whether the Select component is disabled. | bool | false |  |
| DisabledName | The name of the property to be used as a disabled indicator. | string |  |  |
| DisabledPredicate | Specifies predicate for disabled options |  -  |  -  |
| DropdownMatchSelectWidth |  Will match drowdown width: <br/>- for boolean: `true` - with widest item in the dropdown list <br/> - for string: with value (e.g.: `256px`). | OneOf<bool, string> | true |  |
| DropdownMaxWidth | Will not allow dropdown width to grow above stated in here value (eg. "768px"). | string | "auto" |  |
| DropdownRender | Customize dropdown content. | Renderfragment | - |  |
| SearchDebounceMilliseconds | Delays the processing of the search input event until the user has stopped typing for a predetermined amount of time | int        |  250         |
| EnableSearch | Indicates whether the search function is active or not. Always `true` for mode `tags`. | bool | false |  |
| FilterExpression | Customize the logic to filter when searching. | Func<SelectOptionItem<TItemValue, TItem>, string, bool> | (item, searchValue) => item.Label.Contains(searchValue, StringComparison.InvariantCultureIgnoreCase) |  |
| GroupName | The name of the property to be used as a group indicator. If the value is set, the entries are displayed in groups. Use additional `SortByGroup` and `SortByLabel`. | string | - |  |
| HideSelected | Hides the selected items when they are selected. | bool | false |  |
| IgnoreItemChanges | Is used to increase the speed. If you expect changes to the label name, group name or disabled indicator, disable this property. | bool | true |  |
| ItemTemplate | Is used to customize the item style. | RenderFragment&lt;TItem> |  |  |
| LabelInValue | Whether to embed label in value, turn the format of value from `TItemValue` to string (JSON) e.g. { "value": `TItemValue`, "label": "`Label value`" } | bool | false |  |
| LabelName | The name of the property to be used for the label. | string |  |  |
| LabelTemplate | Is used to customize the label style. | RenderFragment&lt;TItem> |  |  |
| LabelProperty | Specifies the label property in the option object. | Func<TItem, string> | - |
| Loading | Show loading indicator. You have to write the loading logic on your own. | bool | false |  |
| ListboxStyle | custom listbox styles | string | display: flex; flex-direction: column; |  |
| MaxTagCount | Max tag count to show. responsive will cost render performance. | int | `ResponsiveTag.Responsive` | - |  |
| MaxTagPlaceholder | Placeholder for hidden tags. If used with `ResponsiveTag.Responsive`, implement your own handling logic. | RenderFragment<IEnumerable<TItem>>> | - |  |
| MaxTagTextLength | Max tag text length (number of characters) to show. | int | - |  |
| Mode | Set mode of Select - `default` \| `multiple` \| `tags` | string | default |  |
| NotFoundContent | Specify content to show when no result matches. | RenderFragment | `Not Found` |  |
| OnBlur | Called when blur. | Action | - |  |
| OnClearSelected | Called when the user clears the selection. | Action | - |  |
| OnCreateCustomTag | Called when custom tag is created. | Action | - |  |
| OnDataSourceChanged | Called when the datasource changes. | Action | - |  |
| OnDropdownVisibleChange | Called when the dropdown visibility changes. | Action | - |  |
| OnFocus | Called when focus. | Action | - |  |
| OnMouseEnter | Called when mouse enter. | Action | - |  |
| OnMouseLeave | Called when mouse leave. | Action | - |  |
| OnSearch | Callback function that is fired when input changed. | Action&lt;string> | - |  |
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
| TokenSeparators | Define what characters will be treated as token separators for newly created tags. Useful when creating new tags using only keyboard. | char[] | - |  |
| Value | Get or set the selected value. | TItemValue | - |  |
| Values | Get or set the selected values. | IEnumerable&lt;TItemValues> | - |  |
| ValueChanged | Used for the two-way binding. | EventCallback&lt;TItemValue> | - |  |
| ValuesChanged | Used for the two-way binding. | EventCallback&lt;IEnumerable&lt;TItemValue>> | - |  |
| ValueName | The name of the property to be used for the value. | string | - |  |
| ValueOnClear | When Clear button is pressed, Value will be set to whatever is set in ValueOnClear. | TItemValue | - | 0.11 |
| ValueProperty | Specifies the value property in the option object. | Func<TItem, TItemValue> | - |

### SelectOption props

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Class | The additional class to option                  | string          | -      |      |
| Disabled  | Disable this option                                 | Boolean        | false  |      |
| Label     | Label of Select after selecting this Option | string         | -      |      |
| Value     | Value of Select after selecting this Option | TItemValue          | -      |      |
| Item      | Item of the option                          | TItem          |  -  |    |