---
category: Components
type: Data Entry
title: AutoComplete
cover: https://gw.alipayobjects.com/zos/alicdn/qtJm4yt45/AutoComplete.svg
---

Autocomplete function of input field.

## When To Use

When there is a need for autocomplete functionality.

## API

### AutoComplete

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Backfill | backfill selected item the input when using keyboard | bool | false |
| BoundaryAdjustMode | `Dropdown` adjustment strategy (when for example browser resize is happening)         | TriggerBoundaryAdjustMode    | TriggerBoundaryAdjustMode.InView         |
| Options | Data source for autocomplete | AutocompleteDataSource | - |
| Disabled | Set disabled | bool | - |
| Placeholder | Placeholder text | string | - |
| DefaultActiveFirstOption | Whether active first option by default | boolean | true |
| Width | Custom width, unit px | int | auto |
| OverlayClassName | Class name of the dropdown root element | string | - |
| OverlayStyle | Style of the dropdown root element | `object` | - |
| CompareWith | Contrast, used to compare whether two objects are the same  | (o1: object, o2: object) => bool | (o1: object, o2: object) => o1===o2 |
| PopupContainerSelector | The selector of the container for dropdown element. | string | body |
| Placeholder | The placeholder of input | string |  |
| AutoCompleteOptions | Data source for autocomplete | list<AutoCompleteOption> |  |
| OptionDataItems | Data source for Options binding | AutoCompleteDataItem |  |
| OnSelectionChange | callback when a option is selected | function（）=>AutoCompleteOption |  |
| OnActiveChange | callback when active change | function（）=>AutoCompleteOption |  |
| OnInput | callback function,when input change | function（）=>ChangeEventArgs |  |
| OnPanelVisibleChange | callback function when panel visible is changed | function（）=>bool |  |
| ChildContent | Additional Content | RenderFragment |  |
| OptionTemplate | option template | RenderFragment=>AutoCompleteDataItem |  |
| OptionFormat | Format options,customize the display format | `RenderFragment=>AutoCompleteDataItem |  |
| OverlayTemplate | All options template | RenderFragment |  |
| FilterExpression | Filter expression | function(option, value)=>AutoCompleteDataItem |  |
| AllowFilter | is filtering allow data | bool | true |
| ShowPanel | where optioning, display panel | bool | false |

### AutoCompleteOption

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Value | bind ngModel of the trigger element | object | - |
| Label | display value of the trigger element | string | - |
| Disabled | disabled option | boolean | false |

