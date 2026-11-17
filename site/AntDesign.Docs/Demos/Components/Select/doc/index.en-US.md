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
| Bordered | **Obsolete:** legacy border toggle retained for backward compatibility. Use `Variant` to choose the selector style. | bool | true |  |
| BoundaryAdjustMode | `Dropdown` adjustment strategy (when for example browser resize is happening)         | TriggerBoundaryAdjustMode    | TriggerBoundaryAdjustMode.InView         |
| CustomTagLabelToValue | Converts custom tag (a string) to TItemValue type. | Func<string, TItemValue> | (label) => <br/>    (TItemValue)TypeDescriptor<br/>    .GetConverter(typeof(TItemValue))<br/>    .ConvertFromInvariantString(label) |  |
| DataSource | The datasource for this component. | IEnumerable&lt;TItem> | - |  |
| Options | Select options. React-compatible alias for `DataSource`. | IEnumerable&lt;TItem> | - | 6.0.0 |
| DataSourceEqualityComparer | EqualityComparer that will be used during DataSource change detection. If no comparer set, default comparer will be used that is going to compare only equiality of label & value properties. | IEqualityComparer&lt;TItem> | - |  |
| DefaultActiveFirstOption | Activates the first item that is not deactivated.  | bool | false |  |
| DefaultOpen | Whether the dropdown is open on the first render. Use `Open` to control it after initialization. | bool | false | 6.0.0 |
| DefaultValue | When `Mode = default` - The value is used during initialization and when pressing the Reset button within Forms. | TItemValue | - |  |
| DefaultValues | When `Mode = multiple` \| `tags` -  The values are used during initialization and when pressing the Reset button within Forms. | IEnumerable&lt;TItemValues> | - |  |
| Disabled | Whether the Select component is disabled. | bool | false |  |
| DisabledName | The name of the property to be used as a disabled indicator. | string |  |  |
| DisabledPredicate | Specifies predicate for disabled options |  -  |  -  |
| DropdownMatchSelectWidth | Controls dropdown width: `true` matches the Select trigger; `false` fits the widest option while keeping the trigger width as a minimum; a string sets an explicit width (e.g. `256px`). | OneOf<bool, string> | true |  |
| PopupMatchSelectWidth | Controls whether the popup matches the Select width. React-compatible alias for `DropdownMatchSelectWidth`. | OneOf<bool, string> | true | 6.0.0 |
| DropdownMaxWidth | Will not allow dropdown width to grow above stated in here value (eg. "768px"). | string | "auto" |  |
| DropdownRender | Customize dropdown content. | Renderfragment | - |  |
| PopupRender | Customize dropdown content. React-compatible alias for `DropdownRender`. | RenderFragment&lt;RenderFragment> | - | 6.0.0 |
| SearchDebounceMilliseconds | Delays the processing of the search input event until the user has stopped typing for a predetermined amount of time | int        |  250         |
| EnableSearch | Enables search in single mode. Multiple and tags modes are searchable by default, matching Ant Design v6. | bool | false |  |
| ShowSearch | Enables search with a boolean or config object. Config supports `OptionFilterProp` (string or string[]) and `FilterOption`; when omitted, search follows the Select mode defaults. | OneOf&lt;bool?, SelectShowSearchOptions> | mode-based | 6.0.0 |
| FilterExpression | Customize the logic to filter when searching. | Func<SelectOptionItem<TItemValue, TItem>, string, bool> | (item, searchValue) => item.Label.Contains(searchValue, StringComparison.InvariantCultureIgnoreCase) |  |
| FilterOption | Whether to filter options by the search text. Set to false to keep all options visible; use `FilterExpression` to customize matching. | bool? | true | 6.0.0 |
| FieldNames | Maps custom option fields for `Label`, `Value`, nested `Options`, and group `GroupLabel`. | SelectFieldNames | `{ label: "label", value: "value", options: "options", groupLabel: "label" }` | 6.0.0 |
| OptionFilterProp | **Obsolete:** legacy top-level alias; configure this inside `ShowSearch` to match React v6.1. Multiple fields use OR matching. | OneOf&lt;string, string[]> | value | 6.0.0 |
| FilterSort | Sorts filtered options. The third argument is the current search text. | Func&lt;SelectOptionItem&lt;TItemValue, TItem&gt;, SelectOptionItem&lt;TItemValue, TItem&gt;, string, int&gt; | - | 6.0.0 |
| GroupName | The name of the property to be used as a group indicator. If the value is set, the entries are displayed in groups. Use additional `SortByGroup` and `SortByLabel`. | string | - |  |
| HideSelected | Hides the selected items when they are selected. | bool | false |  |
| IgnoreItemChanges | Is used to increase the speed. If you expect changes to the label name, group name or disabled indicator, disable this property. | bool | true |  |
| ItemTemplate | Is used to customize the item style. | RenderFragment&lt;TItem> |  |  |
| OptionRender | Customize how each dropdown option is rendered. The template receives the option's source data item. | RenderFragment&lt;TItem> | - | 6.0.0 |
| LabelInValue | Whether to embed label in value, turn the format of value from `TItemValue` to string (JSON) e.g. { "value": `TItemValue`, "label": "`Label value`" } | bool | false |  |
| LabelName | The name of the property to be used for the label. | string |  |  |
| LabelTemplate | Is used to customize the label style. | RenderFragment&lt;TItem> |  |  |
| LabelRender | Customize the currently selected label. React-compatible name for `LabelTemplate`; the template receives the selected source item. | RenderFragment&lt;TItem> | - | 6.0.0 |
| LabelProperty | Specifies the label property in the option object. | Func<TItem, string> | - |
| Loading | Show loading indicator. You have to write the loading logic on your own. | bool | false |  |
| ListboxStyle | custom listbox styles | string | display: flex; flex-direction: column; |  |
| ListHeight | Configures the maximum height of the option list. React-compatible alias for `PopupContainerMaxHeight`. | int? | 256 | 6.0.0 |
| MaxTagCount | Max tag count to show. responsive will cost render performance. | int | `ResponsiveTag.Responsive` | - |  |
| MaxCount | Maximum number of options that can be selected in multiple or tags mode. | int? | - | 6.0.0 |  |
| Status | Set validation status to `error` or `warning`; inherits from `FormItem` when omitted. | string | - |  |
| MaxTagPlaceholder | Placeholder for hidden tags. If used with `ResponsiveTag.Responsive`, implement your own handling logic. | RenderFragment<IEnumerable<TItem>>> | - |  |
| MaxTagTextLength | Max tag text length (number of characters) to show. | int | - |  |
| MenuItemSelectedIcon | Custom icon shown for selected options in multiple or tags mode. | RenderFragment | Check icon | 6.0.0 |
| Mode | Set mode of Select - `default` \| `multiple` \| `tags` | string | default |  |
| NotFoundContent | Specify content to show when no result matches. | RenderFragment | `Not Found` |  |
| OnBlur | Called when blur. | Action | - |  |
| OnClear | React-compatible callback invoked after the clear action. | EventCallback | - | 6.0.0 |
| OnClearSelected | Called when the user clears the selection. | Action | - |  |
| OnCreateCustomTag | Called when custom tag is created. | Action | - |  |
| OnDataSourceChanged | Called when the datasource changes. | Action | - |  |
| OnDeselect | Called when an option is deselected in multiple or tags mode. Receives the option item. | EventCallback&lt;SelectOptionItem&lt;TItemValue, TItem>> | - | 6.0.0 |
| OnDropdownVisibleChange | Called when the dropdown visibility changes. | Action | - |  |
| OnOpenChange | Called when the dropdown open state changes. React-compatible callback name. | EventCallback&lt;bool> | - | 6.0.0 |
| OnFocus | Called when focus. | Action | - |  |
| OnInputKeyDown | Called when a key is pressed while the Select input is focused. | EventCallback&lt;KeyboardEventArgs> | - | 6.0.0 |
| OnMouseEnter | Called when mouse enter. | Action | - |  |
| OnMouseLeave | Called when mouse leave. | Action | - |  |
| OnPopupScroll | Called when the dropdown popup scrolls. | EventCallback&lt;EventArgs> | - | 6.0.0 |
| OnSearch | Callback function that is fired when input changed. | Action&lt;string> | - |  |
| SearchValue | Controlled search text. Bind with `SearchValueChanged` or `@bind-SearchValue` to synchronize the input and filtering state. | string | - | 6.0.0 |
| SearchValueChanged | Called when the controlled search text changes. | EventCallback&lt;string> | - | 6.0.0 |
| OnSelect | Called when an option is selected. Receives the option item, including its value, label, and source data. | EventCallback&lt;SelectOptionItem&lt;TItemValue, TItem>> | - | 6.0.0 |
| OnSelectedItemChanged | Called when the selected item changes. | Action&lt;TItem> | - |  |
| OnSelectedItemsChanged | Called when the selected items changes. | Action&lt;IEnumerable&lt;TItem>> | - |  |
| Open | Controlled open state of dropdown. | bool | false |  |
| Placeholder | Placeholder of select. | string | - |  |
| PopupContainerMaxHeight | The maximum height of the popup container. | string | `256px` |  |
| RemoveIcon | Custom icon used to remove a selected tag. | RenderFragment | Close icon | 6.0.0 |
| PopupContainerSelector | Use this to fix overlay problems e.g. #area | string | body |  |
| PrefixIcon | The custom prefix icon. For mode `multiple` and `tags` visible only when no data selected. | RenderFragment | - |  |
| Prefix | The custom prefix content. React-compatible alias for `PrefixIcon`. | RenderFragment | - | 6.0.0 |
| SelectOptions | Used for rendering select options manually. | RenderFragment | - |  |
| ShowArrowIcon | Whether to show the drop-down arrow | bool | true |  |
| ShowArrow | Whether to show the dropdown arrow. React-compatible alias for `ShowArrowIcon`. | bool | true | 6.0.0 |
| Virtual | Whether to use virtual scrolling. React-compatible alias for `EnableVirtualization`. | bool | true | 6.0.0 |
| ShowSearchIcon | Whether show search input in single mode. | bool | true |  |
| Size | The size of the component. `small` \| `default` \| `large` | string | default |  |
| SortByGroup | Sort items by group name. `None` \| `Ascending` \| `Descending` | SortDirection | SortDirection.None |  |
| SortByLabel | Sort items by label value. `None` \| `Ascending` \| `Descending` | SortDirection | SortDirection.None |  |
| Style | Set CSS style. | string | `width: 100%` |  |
| SuffixIcon | The custom suffix icon. | RenderFragment | - |  |
| TokenSeparators | Define what characters will be treated as token separators for newly created tags. Useful when creating new tags using only keyboard. | char[] | - |  |
| Tokenize | Optional callback that maps search text to committed values. Applies to multiple and tags modes; return an empty sequence until the input is ready to commit. | Func&lt;string, IEnumerable&lt;string&gt;&gt; | - |  |
| Value | Get or set the selected value. | TItemValue | - |  |
| Values | Get or set the selected values. | IEnumerable&lt;TItemValues> | - |  |
| ValueChanged | Used for the two-way binding. | EventCallback&lt;TItemValue> | - |  |
| ValuesChanged | Used for the two-way binding. | EventCallback&lt;IEnumerable&lt;TItemValue>> | - |  |
| ValueName | The name of the property to be used for the value. | string | - |  |
| ValueOnClear | When Clear button is pressed, Value will be set to whatever is set in ValueOnClear. | TItemValue | - | 0.11 |
| ValueProperty | Specifies the value property in the option object. | Func<TItem, TItemValue> | - |
| Variant | Selector appearance: `Outlined`, `Filled`, `Borderless`, or `Underlined`. Defaults to the legacy `Bordered` value when omitted. | `InputVariant?` | - | 6.0.0 |

### SelectOption props

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Class | The additional class to option                  | string          | -      |      |
| Disabled  | Disable this option                                 | Boolean        | false  |      |
| Label     | Label of Select after selecting this Option | string         | -      |      |
| Value     | Value of Select after selecting this Option | TItemValue          | -      |      |
| Item      | Item of the option                          | TItem          |  -  |    |

## Design Token

Select component tokens are available through `ThemeConfig.Components["Select"]` and are emitted as CSS variables under `--ant-select-*`. They follow the React v6 Select token contract; changing them does not require application-level CSS overrides.

| Token | CSS variable | Description | Default |
| --- | --- | --- | --- |
| ActiveBorderColor | `--ant-select-active-border-color` | Border color while active | `colorPrimary` |
| ActiveOutlineColor | `--ant-select-active-outline-color` | Focus outline color while active | `controlOutline` |
| ClearBg | `--ant-select-clear-bg` | Clear button background | `colorBgContainer` |
| HoverBorderColor | `--ant-select-hover-border-color` | Border color on hover | `colorPrimaryHover` |
| MultipleItemBg | `--ant-select-multiple-item-bg` | Background of a multiple-selection tag | `colorFillSecondary` |
| MultipleItemBorderColor | `--ant-select-multiple-item-border-color` | Border color of a multiple-selection tag | `transparent` |
| MultipleItemBorderColorDisabled | `--ant-select-multiple-item-border-color-disabled` | Disabled tag border color | `transparent` |
| MultipleItemColorDisabled | `--ant-select-multiple-item-color-disabled` | Disabled tag text color | `colorTextDisabled` |
| MultipleItemHeight | `--ant-select-multiple-item-height` | Multiple tag height | min(`controlHeight - 2 × paddingXXS`, `controlHeight - 2 × lineWidth`) |
| MultipleItemHeightLG | `--ant-select-multiple-item-height-lg` | Large multiple tag height | min(`controlHeightLG - 2 × paddingXXS`, `controlHeightLG - 2 × lineWidth`) |
| MultipleItemHeightSM | `--ant-select-multiple-item-height-sm` | Small multiple tag height | min(`controlHeightSM - 2 × paddingXXS`, `controlHeightSM - 2 × lineWidth`) |
| MultipleSelectorBgDisabled | `--ant-select-multiple-selector-bg-disabled` | Disabled multiple selector background | `colorBgContainerDisabled` |
| OptionActiveBg | `--ant-select-option-active-bg` | Active option background | `controlItemBgHover` |
| OptionFontSize | `--ant-select-option-font-size` | Option font size | `fontSize` |
| OptionHeight | `--ant-select-option-height` | Option height | `controlHeight` |
| OptionLineHeight | `--ant-select-option-line-height` | Option line height | `lineHeight` |
| OptionPadding | `--ant-select-option-padding` | Option padding | `(controlHeight - fontSize × lineHeight) / 2` vertically; `controlPaddingHorizontal` horizontally |
| OptionSelectedBg | `--ant-select-option-selected-bg` | Selected option background | `controlItemBgActive` |
| OptionSelectedColor | `--ant-select-option-selected-color` | Selected option text color | `colorText` |
| OptionSelectedFontWeight | `--ant-select-option-selected-font-weight` | Selected option font weight | `fontWeightStrong` |
| SelectorBg | `--ant-select-selector-bg` | Selector background | `colorBgContainer` |
| ShowArrowPaddingInlineEnd | `--ant-select-show-arrow-padding-inline-end` | Inline-end padding when the arrow is shown | `ceil(fontSize × 1.25)` |
| SingleItemHeightLG | `--ant-select-single-item-height-lg` | Selected item height in large size | `controlHeightLG` |
| ZIndexPopup | `--ant-select-z-index-popup` | Dropdown z-index | `zIndexPopupBase + 50` |
