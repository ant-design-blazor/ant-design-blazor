## API

### Tour

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Open | Whether the tour is open | `bool` | `false` |
| Steps | Array of tour steps | `List<TourStep>` | `[]` |
| Current | Current step index | `int` | `0` |
| Type | Tour type, affects background and text color | `TourType` | `TourType.Default` |
| Mask | Whether to show mask | `bool` | `true` |
| MaskStyle | Mask style | `string` | - |
| ZIndex | z-index of the tour | `int` | `1001` |
| CloseIcon | Custom close icon | `RenderFragment` | - |
| IndicatorsRender | Custom indicator render | `RenderFragment<(int current, int total)>` | - |
| OnChange | Callback when step changes | `EventCallback<int>` | - |
| OnClose | Callback when tour closes | `EventCallback` | - |
| OnFinish | Callback when tour finishes | `EventCallback` | - |

### TourStep

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Title | Title of the step | `string` | - |
| Description | Description text | `string` | - |
| DescriptionTemplate | Custom description content | `RenderFragment` | - |
| Cover | Cover image or content | `RenderFragment` | - |
| Target | Target element, null for center placement | `Func<ElementReference?>` | - |
| Placement | Placement of tour panel relative to target | `Placement` | `Placement.Bottom` |
| Arrow | Whether to show arrow | `bool` | `true` |
| Mask | Whether to enable mask | `bool?` | - |
| MaskStyle | Mask style | `string` | - |
| Type | Type, affects background and text color | `TourType` | `TourType.Default` |
| Closable | Whether to show close button | `bool` | `true` |
| PrevButtonProps | Previous button properties | `TourButtonProps` | - |
| NextButtonProps | Next button properties | `TourButtonProps` | - |

### TourType

| Value | Description |
| --- | --- |
| Default | Default style |
| Primary | Primary color style |

### TourButtonProps

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Text | Button text | `string` | - |
| OnClick | Click event handler | `EventCallback<MouseEventArgs>` | - |
| Disabled | Whether button is disabled | `bool` | `false` |
| Class | Custom CSS class | `string` | - |
| Style | Custom inline style | `string` | - |
