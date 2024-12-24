---
category: Components
type: Data Display
title: Tabs
cols: 1
cover: https://gw.alipayobjects.com/zos/antfincdn/lkI2hNEDr2V/Tabs.svg
---

Tabs make it easy to switch between different views.

### When To Use

Ant Design has 3 types of Tabs for different situations.

- Card Tabs: for managing too many closeable views.
- Normal Tabs: for EventCallbackal aspects of a page.
- [RadioButton](/components/radio/#components-radio-demo-radiobutton): for secondary tabs.


## Supports keyboard navigation

Keyboard navigation is available when the focus is on the tab.

- <kbd>ArrowLeft</kbd>: navigate to previous tab
- <kbd>ArrowRight</kbd>: navigate to next tab
- <kbd>ArrowUp</kbd>: navigate to first tab
- <kbd>ArrowDown</kbd>: navigate to last tab
- <kbd>Enter</kbd>: navigate to currently focused tab

## API

### Tabs

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| ActiveKey | Current TabPane's key£¬support two-way binding. | string | - |
| Animated | Whether to change tabs with animation. Only works while `TabPosition="TabPosition.Top"\|"TabPosition.Bottom"` | boolean | `false` |
| DefaultActiveKey | Initial active TabPane's key, if `ActiveKey` is not set. | string | - |
| HideAdd | Hide plus icon or not. Only works while `Type="TabType.EditableCard"` | boolean | `false` |
| Size | Preset tab bar size | TabSize | `TabSize.Default` |
| TabBarExtraContent | Extra content in tab bar | RenderFargment | - |
| TabBarExtraContentLeft | Extra content in the left of tab bar | RenderFargment | - |
| TabBarExtraContentRight | Extra content in the right of tab bar | RenderFargment | - |
| TabBarGutter | The gap between tabs | number | - |
| TabBarStyle | Tab bar style object | object | - |
| TabPosition | Position of tabs | TabPosition | `TabPosition.Top` |
| Type | Basic style of tabs | TabType | `TabType.Line` |
| OnChange | Callback executed when active tab is changed | EventCallback(activeKey) {} | - |
| OnEdit | Callback executed when tab is added or removed. Only works while `Type="TabType.EditableCard"` | `Func<string, string, Task<bool>>` | - |
| OnClose | Callback executed when tab is removed. Only works while `Type="TabType.EditableCard"` | EventCallback(targetKey) {} | - |
| OnNextClick | Callback executed when next button is clicked | EventCallback | - |
| OnPrevClick | Callback executed when prev button is clicked | EventCallback | - |
| OnTabClick | Callback executed when tab is clicked | EventCallback(key: string, event: MouseEvent) | - |
| Draggable | Make tabs draggable | bool | false | 

### Tabs.TabPane

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| ForceRender | Forced render of content in tabs, not lazy render after clicking on tabs | boolean | false |
| Key | TabPane's key | string | - |
| Tab | Show text in TabPane's head | string | - |
| TabTemplate | Template of TabPane's head | RenderFargment | - |
| TabContextMenu | Template for customer context menu | RenderFargment<TabPane> | - |
