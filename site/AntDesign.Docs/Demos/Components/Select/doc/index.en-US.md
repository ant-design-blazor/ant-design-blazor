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

### Select Properties

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| AllowClear | Show clear button. | bool | false |  |
| AutoClearSearchValue | Whether the current search will be cleared on selecting an item. | bool | true |  |
| Bordered | Toggle the border style. | bool | true |  |
| DataSource | The datasource for this component. | IEnumerable&lt;TItem> | - |  |
| DefaultActiveFirstOption | Activates the first item that is not deactivated.  | bool | false |  |
| DefaultValue | `default` - The value is used during initialization and when pressing the Rest button within Forms. | TItemValue | - |  |
| DefaultValues | `multiple` \| `tags` -  The values are used during initialization and when pressing the Rest button within Forms. | IEnumerable&lt;TItemValues> | - |  |
| Disabled | Whether disabled select. | bool | false |  |
| DisabledName | The name of the property to be used as a disabled indicator. | string |  |  |
| DropdownRender | Customize dropdown content. | Renderfragment | - |  |
| EnableSearch | Indicates whether the search function is active or not. Always `true` for mode `tags`. | bool | false |  |
| GroupName | The name of the property to be used as a group indicator. If the value is set, the entries are displayed in groups. Use additional `SortByGroup` and `SortByLabel`. | string | - |  |
| HideSelected | Hides the selected items when they are selected. | bool | false |  |
| IgnoreItemChanges | Is used to increase the speed. If you expect changes to the label name, group name or disabled indicator, disable this property. | bool | true |  |
| ItemTemplate | Is used to customize the item style. | RenderFragment&lt;TItem> |  |  |
| LabelInValue | Whether to embed label in value, turn the format of value from `TItemValue` to string (JSON) e.g. { "value": `TItemValue`, "label": "`Label value`" } | bool | false |  |
| LabelName | The name of the property to be used for the label. | string |  |  |
| LabelTemplate | Is used to customize the label style. | RenderFragment&lt;TItem> |  |  |
| Loading | Show loading indicator. You have to write the loading logic by your own. | bool | false |  |
| MaxTagCount | Max tag count to show. responsive will cost render performance. | int | `ResponsiveTag.Responsive` | - |  |
| MaxTagPlaceholder | Placeholder for not showing tags. If used with `ResponsiveTag.Responsive`, implement your own handling logic. | RenderFragment<IEnumerable<TItem>>> | - |  |
| MaxTagTextLength | Max tag text length (number of characters) to show. | int | - |  |
| Mode | Set mode of Select - `default` \| `multiple` \| `tags` | string | default |  |
| NotFoundContent | Specify content to show when no result matches. | RenderFragment | `Not Found` |  |
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
| PopupContainerGrowToMatchWidestItem | Will allow the popup container to grow to match widest item. | bool | false |  |
| PopupContainerMaxWidth  | The maximum width of the popup container. | string | `auto` |  |
| PrefixIcon | The custom prefix icon. For mode `multiple` and `tags` visible only when no data selected. | RenderFragment | - |  |
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

> Note that if you find that the dropdown menu follows the page scroll, or if you need to trigger `Select` in another popup layer, try using `GetPopupContainer={triggerNode => triggerNode.parentElement}` to fix the dropdown popup render node in the trigger's parent element.

### Select Methods

| Method    | Description     | Version |
| ------- | -------- | ---- |
| Blur()  | Stop focusing on the `Select` control. |      |
| Focus() | Start focusing on the `Select` control. |      |

### SelectOption Properties

| Property      | Description                                     | Type           | Default | Version |
| --------- | --------------------------------------- | -------------- | ------ | ---- |
| Disabled  | Is the `SelectOption` being disabled.                                 | bool        | false  |      |
| Title     | Title of the `select` after being selected to this `SelectOption`.  | string         | -      |      |
| Value     | Filtered based on this property by default.                  | string          | -      |      |
| ClassName | Class name of the `SelectOption`                     | string          | -      |      |

### SelectOptGroup Properties

| Property  | Description | Type                  | Default | Version |
| ----- | ---- | --------------------- | ------ | ---- |
| Key   |      | string                | -      |      |
| Label | The name of the group. | string                | null     |      |

## FAQ

### What if I click on the `dropdownRender` content and then the floating layer closes?

Go to the description in [dropdownRender example](#components-select-demo-custom-dropdown-menu).

### What if the custom SelectOption style causes a scrolling exception?

This is because the default option height for virtual scrolling is `32px`, if your option height is less than this value then you need to adjust it via the `ListItemHeight` property, which is used to set the scroll container height.

```razor
<Select ListItemHeight="10" ListHeight="250" />
```

Note: `ListItemHeight` and `ListHeight` are internal properties, do not modify the values if not necessary.
