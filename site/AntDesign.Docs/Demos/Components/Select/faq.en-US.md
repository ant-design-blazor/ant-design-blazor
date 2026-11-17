### Why can search show two options with the same text in `tags` mode?

This can happen when an option's displayed label differs from its value. Make the label and value consistent, or customize `FilterExpression` to search the field users expect.

### How can I control whether the dropdown closes after clicking content from `PopupRender`?

Control the popup with `Open` and `OnOpenChange`. This is useful when custom popup content needs to keep the Select open or close it after an application action.

### How can I keep the dropdown open when clicking inside custom popup content?

Prevent the mouse down from taking focus away from the Select. For a custom Razor fragment, use Blazor's `@onmousedown:preventDefault` and `@onmousedown:stopPropagation` modifiers on the element that should not close the popup.

### Why can custom option content break virtual scrolling?

The virtual list assumes options have a consistent row height. Custom templates that change the row height can make scrolling and active-option positioning inaccurate. Keep option heights consistent, or set `Virtual="false"` when the list is small and needs variable-height content. `PopupContainerMaxHeight` controls the popup's maximum height.

### Why can an accessibility checker report missing `aria-*` attributes?

Some accessibility attributes and the auxiliary listbox node are created while the Select is being operated. Open the dropdown and run the check again. Set `AriaLabel` or `aria-labelledby` through `AdditionalAttributes` when the Select needs an explicit accessible name. Set `Virtual="false"` when a screen reader must navigate the actual option elements instead of the virtual list's accessibility proxy.

### Why does clicking a custom tag's close icon open the dropdown?

The Select listens to mouse down on its tags to manage focus. Use the `TagRender` component's `PreventMouseDownDefault` and `StopMouseDownPropagation` parameters on the custom tag so its close action does not trigger the Select.
