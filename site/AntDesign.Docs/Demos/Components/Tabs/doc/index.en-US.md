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

## API

### Tabs

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| ActiveKey | Current TabPane's key£¬support two-way binding. | string | - |
| Animated | Whether to change tabs with animation. Only works while `tabPosition="top"\|"bottom"` | boolean | `false` |
| DefaultActiveKey | Initial active TabPane's key, if `activeKey` is not set. | string | - |
| HideAdd | Hide plus icon or not. Only works while `type="editable-card"` | boolean | `false` |
| Size | preset tab bar size | `large` \| `default` \| `small` | `default` |
| TabBarExtraContent | Extra content in tab bar | RenderFargment | - |
| TabBarExtraContentLeft | Extra content in the left of tab bar | RenderFargment | - |
| TabBarExtraContentRight | Extra content in the right of tab bar | RenderFargment | - |
| TabBarGutter | The gap between tabs | number | - |
| TabBarStyle | Tab bar style object | object | - |
| TabPosition | Position of tabs | `top` \| `right` \| `bottom` \| `left` | `top` |
| Type | Basic style of tabs | `line` \| `card` \| `editable-card` | `line` |
| OnChange | Callback executed when active tab is changed | EventCallback(activeKey) {} | - |
| OnEdit | Callback executed when tab is added or removed. Only works while `type="editable-card"` | (targetKey, action): void | - |
| OnClose | Callback executed when tab is removed. Only works while `type="editable-card"` | EventCallback(targetKey) {} | - |
| OnNextClick | Callback executed when next button is clicked | EventCallback | - |
| OnPrevClick | Callback executed when prev button is clicked | EventCallback | - |
| OnTabClick | Callback executed when tab is clicked | EventCallback(key: string, event: MouseEvent) | - |
| Draggable | make tabs draggable. | bool | false | 

### Tabs.TabPane

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| ForceRender | Forced render of content in tabs, not lazy render after clicking on tabs | boolean | false |
| Key | TabPane's key | string | - |
| Tab | Show text in TabPane's head | string | - |
| TabTemplate | Template of TabPane's head | RenderFargment | - |
| TabContextMenu | Template for customer context menu | RenderFargment<TabPane> | - |
