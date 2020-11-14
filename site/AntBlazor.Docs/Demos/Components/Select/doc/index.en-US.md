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
| AllowClear | Show clear button. | bool | false |  |
| AllowCustomTags | Wheter the user an create own tags. You have to implement your own logic to create tags (`CreateCustomTag`).  | bool | false |  |
| AutoClearSearchValue | Whether the current search will be cleared on selecting an item. | bool | true |  |
| Bordered | Toggle the border style. | bool | true |  |
| CreateCustomTag | Returns the string of the input field when the user press enter. | Action&lt;string> | - |  |
| DataSource | The datasource for this component. | IEnumerable&lt;TItem> | - |  |
| DefaultActiveFirstOption | Activates the first item that is not deactivated.  | bool | true |  |
| DefaultValue | `default` - The value is used during initialization and when pressing the Rest button within Forms. | TItemValue | - |  |
| DefaultValues | `multiple` \| `tags` -  The values are used during initialization and when pressing the Rest button within Forms. | IEnumerable&lt;TItemValues> | - |  |
| Disabled | Whether disabled select. | bool | false |  |
| DisabledName | The name of the property to be used as a disabled indicator. | string |  |  |
| DropdownRender | Customize dropdown content. | Renderfragment | - |  |
| EnableSearch | Indicates whether the search function is active or not. | bool | false |  |
| GroupName | The name of the property to be used as a group indicator. If the value is set, the entries are displayed in groups. Use additional `SortByGroup` and `SortByLabel`. | string | - |  |
| HideSelected | Hides the selected items when they are selected. | bool | false |  |
| IgnoreItemChanges | Is used to increase the speed. If you expect changes to the label name, group name or disabled indicator, disable this property. | bool | true |  |
| ItemTemplate | Is used to customize the item style. | RenderFragment&lt;TItem> |  |  |
| LabelName | The name of the property to be used for the label. | string |  |  |
| LabelTemplate | Is used to customize the label style. | RenderFragment&lt;TItem> |  |  |
| Loading | Show loading indicator. You have to write the loading logic by your own. | bool | false |  |
| Mode | Set mode of Select - `default` \| `multiple` \| `tags` | string | default |  |
| OnBlur | Called when blur. | Action | - |  |
| OnClearSelected | Called when the user clears the selection. | Action | - |  |
| OnDataSourceChanged | Called when the datasource changes. From `null` to `IEnumerable<TItem>`, from `IEnumerable<TItem>` to `IEnumerable<TItem>` or from `IEnumerable<TItem>` to `null`. | Action | - |  |
| OnDropdownVisibleChange | Called when the dropdown visibility changes. | Action&lt;bool> | - |  |
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