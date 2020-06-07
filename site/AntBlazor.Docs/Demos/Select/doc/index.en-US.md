---
category: Components
type: Data Entry
title: Select
---

Select component to select value from options.

## When To Use

- A dropdown menu for displaying choices - an elegant alternative to the native `<select>` element.
- Utilizing [Radio](/components/radio/) is recommended when there are fewer total options (less than 5).

## API

```jsx
<Select>
  <SelectOption Value="lucy">lucy</SelectOption>
</Select>
```

### Select props

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| AllowClear | Show clear button. | Boolean | false |  |
| AutoClearSearchValue | Whether the current search will be cleared on selecting an item. Only applies when `mode` is set to `multiple` or `tags`. | Boolean | true |  |
| AutoFocus | Get focus by default | Boolean | false |  |
| DefaultActiveFirstOption | Whether active first option by default | Boolean | true |  |
| DefaultValue | Initial selected option. | string\|IEnumerable&lt;string><br />LabeledValue\|IEnumerable&lt;LabeledValue> | - |  |
| Disabled | Whether disabled select | Boolean | false |  |
| DropdownClassName | className of dropdown menu | string | - |  |
| DropdownMatchSelectWidth | Determine whether the dropdown menu and the select input are the same width. Default set `min-width` same as input. `false` will disable virtual scroll | Boolean \| number | true |  |
| DropdownRender | Customize dropdown content | (menuNode: RenderFragment, props: Properties) => RenderFragment | - |  |
| DropdownStyle | style of dropdown menu | string | - |  |
| FilterOption | If true, filter options by input, if function, filter options against it. The function will receive two arguments, `inputValue` and `option`, if the function returns `true`, the option will be included in the filtered set; Otherwise, it will be excluded. | Boolean or function(inputValue, option) | true |  |
| GetPopupContainer | Parent Node which the selector should be rendered to. Default to `body`. When position issues happen, try to modify it into scrollable content and position it relative. [Example](https://codesandbox.io/s/4j168r7jw0) | function(triggerNode) | () => document.body |  |
| LabelInValue | whether to embed label in value, turn the format of value from `string` to `{key: string, label: RenderFragment}` | Boolean | false |  |
| ListHeight | Config popup height | number | 256 |  |
| MaxTagCount | Max tag count to show | number | - |  |
| MaxTagTextLength | Max tag text length to show | number | - |  |
| MaxTagPlaceholder | Placeholder for not showing tags | RenderFragment/function(omittedValues) | - |  |
| TagRender | Customize tag render | (props: Properties) => RenderFragment | - |  |
| Mode | Set mode of Select | `multiple` \| `tags` | - |  |
| NotFoundContent | Specify content to show when no result matches.. | RenderFragment | 'Not Found' |  |
| OptionFilterProp | Which prop value of option will be used for filter if filterOption is true | string | value |  |
| OptionLabelProp | Which prop value of option will render as content of select. [Example](https://codesandbox.io/s/antd-reproduction-template-tk678) | string | `value` for `combobox`, `children` for other modes |  |
| Placeholder | Placeholder of select | string\|RenderFragment | - |  |
| ShowArrow | Whether to show the drop-down arrow | Boolean | true |  |
| ShowSearch | Whether show search input in single mode. | Boolean | false |  |
| Size | Size of Select input. | `large` \| `middle` \| `small` |  |  |
| SuffixIcon | The custom suffix icon | RenderFragment | - |  |
| RemoveIcon | The custom remove icon | RenderFragment | - |  |
| ClearIcon | The custom clear icon | RenderFragment | - |  |
| MenuItemSelectedIcon | The custom menuItemSelected icon with multiple options | RenderFragment | - |  |
| TokenSeparators | Separator used to tokenize on tag/multiple mode | IEnumerable&lt;string> |  |  |
| Value | Current selected option. | string\|IEnumerable&lt;string><br />LabeledValue\|IEnumerable&lt;LabeledValue> | - |  |
| Virtual | Disable virtual scroll when set to `false` | Boolean | true | 4.1.0 |
| OnBlur | Called when blur | function | - |  |
| OnChange | Called when select an option or input value change, or value of input is changed in combobox mode | function(value, option:SelectOption/IEnumerable&lt;SelectOption>) | - |  |
| OnDeselect | Called when a option is deselected, param is the selected option's value. Only called for multiple or tags, effective in multiple or tags mode only. | function(string\|number\|LabeledValue) | - |  |
| OnFocus | Called when focus | function | - |  |
| OnInputKeyDown | Called when key pressed | function | - |  |
| OnMouseEnter | Called when mouse enter | function | - |  |
| OnMouseLeave | Called when mouse leave | function | - |  |
| OnPopupScroll | Called when dropdown scrolls | function | - |  |
| OnSearch | Callback function that is fired when input changed. | function(value: string) |  |  |
| OnSelect | Called when a option is selected, the params are option's value (or key) and option instance. | function(string\|LabeledValue, option:Option) | - |  |
| DefaultOpen | Initial open state of dropdown | Boolean | - |  |
| Open | Controlled open state of dropdown | Boolean | - |  |
| OnDropdownVisibleChange | Call when dropdown open | function(open) | - |  |
| Loading | indicate loading state | Boolean | false |  |
| Bordered | whether has border style | Boolean | true |  |

### Select Methods

| Name    | Description  | Version |
| ------- | ------------ | ------- |
| Blur()  | Remove focus |         |
| Focus() | Get focus    |         |

### SelectOption props

| Property  | Description                                      | Type           | Default | Version |
| --------- | ------------------------------------------------ | -------------- | ------- | ------- |
| Disabled  | Disable this SelectOption                        | Boolean        | false   |         |
| Title     | `title` of Select after select this SelectOption | string         | -       |         |
| Value     | default to filter with this property             | string         | -       |         |
| ClassName | additional class to SelectOption                 | string         | -       |         |

### OptGroup props

| Property | Description | Type                  | Default | Version |
| -------- | ----------- | --------------------- | ------- | ------- |
| Key      |             | string                | -       |         |
| Label    | Group label | string                | -       |         |

## FAQ

### The dropdown is closed when click `dropdownRender` area?

See the instruction in [dropdownRender example](#components-select-demo-custom-dropdown-menu).

### Why sometime customize SelectOption cause scroll break?

Virtual scroll internal set item height as `32px`. You need to adjust `ListItemHeight` when your option height is less and `ListHeight` config list container height:

```tsx
<Select ListItemHeight="10" ListHeight="250" />
```

Note: `ListItemHeight` and `ListHeight` are internal props. Please only modify when necessary.
