---
category: Components
type: Data Entry
title: Select
cover: https://gw.alipayobjects.com/zos/alicdn/_0XzgOis7/Select.svg
---

A dropdown menu for displaying choices.

## When To Use

- A dropdown menu for displaying choices - an elegant alternative to the native `<Select>` element.
- Utilizing [Radio](/components/radio/) is recommended when there are fewer total options (less than 5).
- Use [AutoComplete](/components/auto-complete/) when users should be able to type freely or choose a suggestion from an input box.

## API


### Select props

React v6 Select APIs are listed first. Blazor uses PascalCase parameter names; `ValueChanged` and `ValuesChanged` provide the `onChange` behavior for single and multiple values.

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| AllowClear | Show the clear button with a boolean, or configure it with `SelectAllowClearOptions`, matching React's `allowClear` object. | OneOf&lt;bool, SelectAllowClearOptions> | false | 6.7.0 |
| AutoComplete | Sets the input's HTML `autocomplete` attribute. Defaults to `new-password`, matching React v6.7.0. | string | `new-password` | 6.7.0 |
| ClearIcon | Legacy top-level custom icon. Prefer `AllowClear.ClearIcon` in the config object. | RenderFragment | Built-in clear icon | 6.0.0 |
| AutoClearSearchValue | Whether the current search will be cleared on selecting an item. | bool | true |  |
| Bordered | **Obsolete:** legacy border toggle retained for backward compatibility. Use `Variant` to choose the selector style. | bool | true |  |
| ClassNames | Customize classes for semantic parts of the Select component. Use dotted keys such as `popup.root` for nested popup parts. | IReadOnlyDictionary&lt;string, string> | - | 6.0.0 |
| ClassNamesFunc | Compute semantic classes from the current Select instance. Blazor callback form of React's function-valued `classNames`; takes precedence over `ClassNames`. | Func&lt;Select&lt;TItemValue, TItem>, IReadOnlyDictionary&lt;string, string>> | - | 6.0.0 |
| DefaultActiveFirstOption | Whether to activate the first enabled option when the dropdown opens. | bool | true |  |
| DefaultOpen | Whether the dropdown is open on the first render. Use `Open` to control it after initialization. | bool | false | 6.0.0 |
| DefaultValue | When `Mode = default` - The value is used during initialization and when pressing the Reset button within Forms. | TItemValue | - |  |
| Direction | Text direction for this Select. Overrides the direction inherited from `ConfigProvider`. | string | inherited |  |
| Disabled | Whether the Select component is disabled. | bool | false |  |
| DropdownClassName | **Obsolete:** legacy popup class name. Use `PopupClassName`, matching React's `popupClassName`. | string | - |  |
| DropdownMatchSelectWidth | Controls dropdown width: `true` matches the Select trigger; `false` fits the widest option, keeps the trigger width as a minimum, and disables virtual scrolling; a number sets a minimum width in pixels; a string sets an explicit CSS width (e.g. `256px`). | OneOf<bool, double, string> | true |  |
| PopupClassName | Additional class applied to the popup, matching React's `popupClassName`. | string | - | 6.0.0 |
| PopupMatchSelectWidth | Controls popup width using React v6 semantics: `true` matches the trigger, `false` fits content and disables virtual scrolling, and a number sets a pixel minimum width. A string CSS width remains available as a Blazor extension. | OneOf<bool, double, string> | true | 6.0.0 |
| DropdownRender | Customize dropdown content. | Renderfragment | - |  |
| PopupRender | Customize dropdown content. React-compatible alias for `DropdownRender`. | RenderFragment&lt;RenderFragment> | - | 6.0.0 |
| DropdownStyle | **Obsolete:** legacy inline style for the popup, matching React's `dropdownStyle`. | string | - |  |
| FieldNames | Maps custom option fields for `Label`, `Value`, nested `Options`, and group `GroupLabel`; optional `Title`, `GroupClassName`, and `GroupTitle` map native option/group attributes. | SelectFieldNames | `{ label: "label", value: "value", options: "options", groupLabel: "label" }` | 6.0.0 |
| FilterOption | Legacy top-level boolean filter toggle. Configure a boolean or `(searchValue, option) => bool` inside `ShowSearch`; the predicate is available on `SelectShowSearchOptions<TItemValue, TItem>`. | bool? | true | 6.0.0 |
| FilterExpression | Customize the logic to filter when searching. | Func<SelectOptionItem<TItemValue, TItem>, string, bool> | (item, searchValue) => item.Label.Contains(searchValue, StringComparison.InvariantCultureIgnoreCase) |  |
| FilterSort | Sorts filtered options. The third argument is the current search text. | Func&lt;SelectOptionItem&lt;TItemValue, TItem&gt;, SelectOptionItem&lt;TItemValue, TItem&gt;, string, int&gt; | - | 6.0.0 |
| GetPopupContainer | Select the popup container from the trigger element, matching React's `getPopupContainer`. Use `PopupContainerSelector` when a CSS selector is more convenient. | Func&lt;ElementReference, ElementReference> | - | 6.0.0 |
| LabelInValue | Whether to embed label in value, turn the format of value from `TItemValue` to string (JSON) e.g. { "value": `TItemValue`, "label": "`Label value`" } | bool | false |  |
| ListHeight | Configures the maximum height of the option list. React-compatible alias for `PopupContainerMaxHeight`. | int? | 256 | 6.0.0 |
| Loading | Show loading indicator. You have to write the loading logic on your own. | bool | false |  |
| LoadingIcon | Custom loading icon; the built-in icon is shown when omitted. | RenderFragment | Built-in loading icon | 6.0.0 |
| MaxCount | Maximum number of options that can be selected in multiple or tags mode. | int? | - | 6.0.0 |  |
| MaxTagCount | Maximum number of tags to show. Use `ResponsiveTag.Responsive` to collapse tags based on width; responsive measurement has a performance cost. | `OneOf<int, ResponsiveTag>` | - (unlimited) |  |
| MaxTagPlaceholder | Placeholder for hidden tags. If used with `ResponsiveTag.Responsive`, implement your own handling logic. | RenderFragment<IEnumerable<TItem>>> | - |  |
| MaxTagTextLength | Max tag text length (number of characters) to show. | int | - |  |
| MenuItemSelectedIcon | Custom icon shown for selected options in multiple or tags mode. | RenderFragment | Check icon | 6.0.0 |
| Mode | Set mode of Select - `default` \| `multiple` \| `tags` | string | default |  |
| NotFoundContent | Specify content to show when no result matches. | RenderFragment | `Not Found` |  |
| Open | Controlled open state of dropdown. | bool | false |  |
| OptionFilterProp | **Obsolete:** legacy top-level alias; configure this inside `ShowSearch` to match React v6.1. Multiple fields use OR matching. | OneOf&lt;string, string[]> | value | 6.0.0 |
| OptionLabelProp | **Obsolete:** React-compatible legacy name for `LabelName`, the option field used as the selected label. | string | - |  |
| Options | Select options. React-compatible alias for `DataSource`. | IEnumerable&lt;TItem> | - | 6.0.0 |
| OptionRender | Customize each dropdown option, matching React's `optionRender(option, { index })`. The template receives `SelectOptionRenderInfo<TItemValue, TItem>` with the option metadata/source item and its zero-based index in the visible, sorted list. | RenderFragment&lt;SelectOptionRenderInfo&lt;TItemValue, TItem>> | - | 6.0.0 |
| Placeholder | Placeholder of select. | string | - |  |
| Placement | Placement of the popup. | Placement | `BottomLeft` |  |
| Prefix | The custom prefix content. React-compatible alias for `PrefixIcon`. | RenderFragment | - | 6.0.0 |
| RemoveIcon | Custom icon used to remove a selected tag. | RenderFragment | Close icon | 6.0.0 |
| SearchValue | Controlled search text. Bind with `SearchValueChanged` or `@bind-SearchValue` to synchronize the input and filtering state. | string | - | 6.0.0 |
| ShowArrow | Whether to show the dropdown arrow. React-compatible alias for `ShowArrowIcon`. | bool | true | 6.0.0 |
| ShowSearch | Enables search with a boolean or config object. The config supports `AutoClearSearchValue`, `FilterOption`, `OptionFilterProp`, `SearchValue`, `OnSearch`, and `SearchIcon`; use `SelectShowSearchOptions<TItemValue, TItem>` for a typed filter predicate and `FilterSort`. | OneOf&lt;bool?, SelectShowSearchOptions> | mode-based | 6.0.0 |
| Size | Select input size. Blazor `Default` corresponds to React `medium`. | `InputSize` | `Default` (React `medium`) |  |
| Status | Set validation status to `error` or `warning`; inherits from `FormItem` when omitted. | string | - |  |
| Styles | Customize inline styles for semantic parts of the Select component. Use dotted keys such as `popup.root` for nested popup parts. | IReadOnlyDictionary&lt;string, string> | - | 6.0.0 |
| StylesFunc | Compute semantic inline styles from the current Select instance. Blazor callback form of React's function-valued `styles`; takes precedence over `Styles`. | Func&lt;Select&lt;TItemValue, TItem>, IReadOnlyDictionary&lt;string, string>> | - | 6.0.0 |
| Suffix | Custom suffix content. React-compatible name for `SuffixIcon`. | RenderFragment | Built-in arrow or search indicator | 6.7.0 |
| SuffixIcon | **Legacy alias:** custom suffix content. Use `Suffix` to match React v6.7. | RenderFragment | - |  |
| TagRender | Customize a selected tag in `multiple` or `tags` mode. The template receives `SelectTagRenderInfo<TItemValue, TItem>`. | RenderFragment&lt;SelectTagRenderInfo&lt;TItemValue, TItem>> | - | 6.0.0 |
| LabelRender | Customize the currently selected label. React-compatible name for `LabelTemplate`; the template receives the selected source item. | RenderFragment&lt;TItem> | - | 6.0.0 |
| TokenSeparators | Characters used to split tags, or a function that maps the current input to committed values, matching React v6. | `OneOf<char[], Func<string, IEnumerable<string>>>` | - | 6.5.0 |
| Value | Get or set the selected value. | TItemValue | - |  |
| Variant | Selector appearance: `Outlined`, `Borderless`, `Filled`, or `Underlined`. Defaults to `Outlined` through the legacy `Bordered` value. | `InputVariant?` | `Outlined` (effective default) | 6.0.0 |
| Virtual | Whether to use virtual scrolling. React-compatible alias for `EnableVirtualization`. | bool | true | 6.0.0 |
| OnActive | Called with the active option value when the highlighted option changes while the popup is open. | EventCallback&lt;TItemValue> | - | 6.0.0 |
| OnBlur | Called when the Select input loses focus; receives the focus event. | EventCallback&lt;FocusEventArgs> | - |  |
| ValueChanged | Called when the selected value changes; equivalent to React's `onChange` in single mode. Also used for two-way binding. | EventCallback&lt;TItemValue> | - |  |
| ValuesChanged | Called when selected values change; equivalent to React's `onChange` in multiple or tags mode. Also used for two-way binding. | EventCallback&lt;IEnumerable&lt;TItemValue>> | - |  |
| OnClear | React-compatible callback invoked after the clear action. | EventCallback | - | 6.0.0 |
| OnDeselect | Called when an option is deselected in multiple or tags mode. Receives the option item. | EventCallback&lt;SelectOptionItem&lt;TItemValue, TItem>> | - | 6.0.0 |
| ~~OnDropdownVisibleChange~~ | Deprecated. Called when the dropdown visibility changes; use `OnOpenChange` instead. | Action&lt;bool> | - |  |
| OnOpenChange | Called when the dropdown open state changes. React-compatible callback name. | EventCallback&lt;bool> | - | 6.0.0 |
| OnFocus | Called when the Select input receives focus; receives the focus event. | EventCallback&lt;FocusEventArgs> | - |  |
| OnInputKeyDown | Called when a key is pressed while the Select input is focused. | EventCallback&lt;KeyboardEventArgs> | - | 6.0.0 |
| OnPopupScroll | Called when the dropdown popup scrolls. | EventCallback&lt;EventArgs> | - | 6.0.0 |
| ~~OnSearch~~ | Deprecated top-level callback for search input changes. Configure `OnSearch` inside `ShowSearch` instead. | Action&lt;string> | - |  |
| OnSelect | Called when an option is selected. Receives the option item, including its value, label, and source data. | EventCallback&lt;SelectOptionItem&lt;TItemValue, TItem>> | - | 6.0.0 |

#### AllowClear config

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| ClearIcon | Custom clear icon. | RenderFragment | Built-in clear icon |
| Disabled | Disables the clear action when true. | bool? | false |
| Label | Accessible name of the clear button. | string | `Clear` |
| TabIndex | Optional explicit tab order of the clear button. The native button is focusable by default. | int? | — |

#### ShowSearch config

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| AutoClearSearchValue | Whether to clear search text after selecting an option in multiple or tags mode. | bool? | true |
| FilterOption | Boolean toggle on `SelectShowSearchOptions`; on `SelectShowSearchOptions<TItemValue, TItem>`, a `(searchValue, option) => bool` predicate. The typed option context exposes `Value`, `Label`, and source `Item`. Use `FilterOptionEnabled` for the toggle with typed options. | bool? or `Func<string, SelectOptionItem<TItemValue, TItem>, bool>` | true |
| FilterOptionEnabled | Boolean filter toggle for `SelectShowSearchOptions<TItemValue, TItem>` when its `FilterOption` predicate is also configured. | bool? | true |
| OptionFilterProp | Option field or fields used for matching; multiple fields use OR matching. | OneOf&lt;string, string[]> | `value` |
| SearchValue | Controlled search text. | string | - |
| OnSearch | Called when the search input changes. | Action&lt;string> | - |
| SearchIcon | Custom suffix icon shown while searching. | RenderFragment | Built-in search icon |
| FilterSort | Sorts filtered items. Available on `SelectShowSearchOptions<TItemValue, TItem>`; the generic type preserves the option item's type. | Func&lt;SelectOptionItem&lt;TItemValue, TItem&gt;, SelectOptionItem&lt;TItemValue, TItem&gt;, string, int&gt; | - |

<details class="docs-blazor-api"><summary>Blazor-specific and legacy parameters (42)</summary>

The following parameters extend or preserve the Blazor API. They remain available alongside the React-compatible names above.

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| AccessKey | The [accesskey](https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/accesskey) global attribute.  | string | |
| AutoFocus | get focus when component mounted                             | boolean        | false         |
| BoundaryAdjustMode | `Dropdown` adjustment strategy (when for example browser resize is happening)         | TriggerBoundaryAdjustMode    | TriggerBoundaryAdjustMode.InView         |
| CustomTagLabelToValue | Converts custom tag (a string) to TItemValue type. | Func<string, TItemValue> | (label) => <br/>    (TItemValue)TypeDescriptor<br/>    .GetConverter(typeof(TItemValue))<br/>    .ConvertFromInvariantString(label) |  |
| DataSource | The datasource for this component. | IEnumerable&lt;TItem> | - |  |
| DataSourceEqualityComparer | EqualityComparer that will be used during DataSource change detection. If no comparer set, default comparer will be used that is going to compare only equiality of label & value properties. | IEqualityComparer&lt;TItem> | - |  |
| DefaultValues | Initial selected values, also used when resetting a Form. | IEnumerable&lt;TItemValue> | - |  |
| DisabledName | The name of the property to be used as a disabled indicator. | string |  |  |
| DisabledPredicate | Specifies predicate for disabled options |  -  |  -  |
| DropdownMaxWidth | Will not allow dropdown width to grow above stated in here value (eg. "768px"). | string | "auto" |  |
| SearchDebounceMilliseconds | Delays the processing of the search input event until the user has stopped typing for a predetermined amount of time | int        |  250         |
| EnableSearch | Enables search in single mode. Multiple and tags modes are searchable by default, matching Ant Design v6. | bool | false |  |
| GroupName | The name of the property to be used as a group indicator. If the value is set, the entries are displayed in groups. Use additional `SortByGroup` and `SortByLabel`. | string | - |  |
| HideSelected | Hides the selected items when they are selected. | bool | false |  |
| IgnoreItemChanges | Is used to increase the speed. If you expect changes to the label name, group name or disabled indicator, disable this property. | bool | true |  |
| ItemTemplate | Is used to customize the item style. | RenderFragment&lt;TItem> |  |  |
| LabelName | The name of the property to be used for the label. | string |  |  |
| LabelTemplate | Is used to customize the label style. | RenderFragment&lt;TItem> |  |  |
| LabelProperty | Specifies the label property in the option object. | Func<TItem, string> | - |
| ListboxStyle | custom listbox styles | string | display: flex; flex-direction: column; |  |
| OnClearSelected | Called when the user clears the selection. | Action | - |  |
| OnCreateCustomTag | Called when custom tag is created. | Action | - |  |
| OnDataSourceChanged | Called when the datasource changes. | Action | - |  |
| OnMouseEnter | Called when mouse enter. | Action | - |  |
| OnMouseLeave | Called when mouse leave. | Action | - |  |
| SearchValueChanged | Called when the controlled search text changes. | EventCallback&lt;string> | - | 6.0.0 |
| OnSelectedItemChanged | Called when the selected item changes. | Action&lt;TItem> | - |  |
| OnSelectedItemsChanged | Called when the selected items changes. | Action&lt;IEnumerable&lt;TItem>> | - |  |
| PopupContainerMaxHeight | The maximum height of the popup container. | string | `256px` |  |
| PopupContainerSelector | Use this to fix overlay problems e.g. #area | string | body |  |
| PrefixIcon | The custom prefix icon. For mode `multiple` and `tags` visible only when no data selected. | RenderFragment | - |  |
| SelectOptions | Used for rendering select options manually. | RenderFragment | - |  |
| ShowArrowIcon | Whether to show the drop-down arrow | bool | true |  |
| ShowSearchIcon | Whether show search input in single mode. | bool | true |  |
| SortByGroup | Sort items by group name. `None` \| `Ascending` \| `Descending` | SortDirection | SortDirection.None |  |
| SortByLabel | Sort items by label value. `None` \| `Ascending` \| `Descending` | SortDirection | SortDirection.None |  |
| Style | Set CSS style. | string | `width: 100%` |  |
| Tokenize | Legacy callback alias for function-valued `TokenSeparators`. Applies to multiple and tags modes; return an empty sequence until the input is ready to commit. | Func&lt;string, IEnumerable&lt;string&gt;&gt; | - |  |
| Values | Get or set the selected values in `multiple` or `tags` mode. | IEnumerable&lt;TItemValue> | - |  |
| ValueName | The name of the property to be used for the value. | string | - |  |
| ValueOnClear | When Clear button is pressed, Value will be set to whatever is set in ValueOnClear. | TItemValue | - | 0.11 |
| ValueProperty | Specifies the value property in the option object. | Func<TItem, TItemValue> | - |

</details>

### Select methods

Call these methods through a component reference. They correspond to React Select's `blur()` and `focus()` methods.

| Method | Description | .NET return type |
| --- | --- | --- |
| Blur() | Removes focus from the Select input. | Task |
| Focus() | Focuses the Select input. | Task |

### Option groups

React v6 uses `OptGroup` with nested `Option` elements. Blazor represents groups in the option data instead:

| React v6 | Blazor equivalent |
| --- | --- |
| `OptGroup.label` | `FieldNames.GroupLabel` for nested data, or `GroupName` for a flat data source |
| `OptGroup.className` | Optional `FieldNames.GroupClassName` source field |
| `OptGroup.title` | Optional `FieldNames.GroupTitle` source field |
| `OptGroup` options | `FieldNames.Options` for nested data |
| `Option` value and label | `FieldNames.Value` and `FieldNames.Label` |

For nested data, group class and title are read from the group record. For flat data, they are read from the first option rendered in each group. See the [Option Group example](#components-select-demo-optgroup) for flat data, or the [Field Names example](#components-select-demo-fieldnames) for nested groups.

### SelectOption props

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Class | The additional class to option                  | string          | -      |      |
| Disabled  | Disable this option                                 | Boolean        | false  |      |
| Label     | Label of Select after selecting this Option | string         | -      |      |
| Title     | Native `title` attribute rendered on the option. | string | - | 6.0.0 |
| Value     | Value of Select after selecting this Option | TItemValue          | -      |      |
| Item      | Item of the option                          | TItem          |  -  |    |

## FAQ

### Why does search show duplicate options in `tags` mode?

This can happen when an option's `label` differs from its `value`. Set `ShowSearch.OptionFilterProp` to `label` so filtering uses the displayed text.

### How can I control whether clicking content from `PopupRender` closes the dropdown?

The popup normally closes when Select loses focus. Use the controlled `Open` parameter and `OnOpenChange` callback when the application needs to decide when it closes.

### How do I keep the dropdown open while interacting with custom popup content?

Prevent the custom content's mouse-down from moving focus away from Select. In Razor, use `@onmousedown:preventDefault` (and, when needed, `@onmousedown:stopPropagation`) on the content rendered by `PopupRender`.

### Why can custom option heights make virtual scrolling jump?

Virtual scrolling estimates a fixed row height. Keep custom option rows at a consistent height, or set `Virtual="false"` when their heights vary. `ListHeight` controls the popup list's maximum height.

### Why does an accessibility audit report missing `aria-*` attributes?

Select creates some accessibility relationships only while it is active. Open the control before inspecting it, and provide an `aria-label` or `aria-labelledby` when the input needs an accessible name. Virtual scrolling renders only part of a long list; set `Virtual="false"` when assistive technology must traverse every option in the DOM.

### Why does clicking a custom tag's close control open the dropdown?

The mouse-down can bubble to the Select trigger before the tag closes. Use the tag's `PreventMouseDownDefault` and `StopMouseDownPropagation` options (or apply the equivalent behavior in `TagRender`) so the close action does not open the popup.
