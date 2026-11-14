---
category: Components
type: Data Display
title: Collapse
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/IxH16B9RD/Collapse.svg
---

A content area which can be collapsed and expanded.

## When To Use

- Can be used to group or hide complex regions to keep the page clean.
- `Accordion` is a special kind of `Collapse`, which allows only one panel to be expanded at a time.

## API

### Collapse

| Property | Description | Type | Default |
| -------------------- | ------------------------ | ------------------- | ------- |
| `Accordion`          | Accordion mode           | `boolean`           | `false` |
| `Bordered`           | Set border style         | `boolean`           | `true`  |
| `ExpandIconPosition` | Set expand icon position | `'left' \| 'right'` | `left`  |
| `Animation`          | Whether enable the animation | `boolean` | `false` |

### Panel

| Property | Description | Type | Default |
| -------------- | ------------------------------------------- | -------------------------- | ------- |
| `Disabled`     | If `true`, panel cannot be opened or closed | `boolean`                  | `false` |
| `Header`       | Title of the panel                          | `string \| RenderFragment` | -       |
| `ExpandedIcon` | Customize an icon for toggle                | `string \| RenderFragment` | -       |
| `Extra`        | Extra element in the corner                 | `string \| RenderFragment` | -       |
| `ShowArrow`    | Display arrow or not                        | `boolean`                  | `true`  |
| `Active`       | Active status of panel, double binding      | `boolean`                  | -       |
| `ActiveChange` | Callback function of the active status      | `EventCallback<boolean>`   | -       |

### Methods

| Name    | Description  |
| ------- | ------------ |
| Activate(string[] activeKeys)  | Activate the panels by their keys |
| Deactivate(string[] inactiveKeys) | Deactivate the panels by their keys   |