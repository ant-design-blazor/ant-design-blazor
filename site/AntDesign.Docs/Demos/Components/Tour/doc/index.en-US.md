---
category: Components
subtitle: Guided tour
type: Data Display
title: Tour
cover: https://gw.alipayobjects.com/zos/alicdn/RT_USzA48/DatePicker.svg
---

A popup guide that walks users through the features of a page step by step.

## When To Use

Use Tour when a page contains new features or a workflow that benefits from an introduction.

## Examples

Basic, non-modal, placement, custom mask, custom indicator, highlighted area, custom actions, and custom styles are migrated from the React Tour demos.

## API

### Tour

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Steps | Steps in the tour | `IReadOnlyList<TourStep>` | `[]` |
| Open | Whether the tour is visible | `bool` | `false` |
| Current | Current step, zero-based | `int` | `0` |
| Mask | Whether to show the mask | `bool` | `true` |
| MaskColor / MaskStyle | Mask color and style | `string` | `rgba(0,0,0,0.5)` / - |
| MaskClosable | Whether clicking the mask closes the tour | `bool` | `true` |
| Keyboard | Whether Esc closes the tour | `bool` | `true` |
| DisabledInteraction | Disable interaction with the target | `bool` | `false` |
| Type | Tour appearance: `Default` or `Primary` | `TourType` | `Default` |
| GapOffset / GapOffsetX / GapOffsetY / GapRadius | Gap and corner radius of the highlighted area; use one shared gap or separate horizontal and vertical gaps | `int` / `int?` | `6` / - / - / `2` |
| IndicatorsRender | Custom step indicator | `RenderFragment<(int Current, int Total)>` | - |
| ActionsRender | Custom actions; `Origin` contains the default actions | `RenderFragment<TourActionsRenderContext>` | - |
| ZIndex | Tour z-index | `int` | `1070` |
| OnChange | Callback when the step changes | `EventCallback<int>` | - |
| OnClose | Callback when the tour closes | `EventCallback` | - |
| OnFinish | Callback when the tour finishes | `EventCallback` | - |

### TourStep

| Property | Description | Type |
| --- | --- | --- |
| Target | Target element `ElementReference` or CSS selector string such as `#save-button`; use it together with `TargetSelector` as alternatives | `ElementReference` / `string` |
| TargetSelector | CSS selector of the target element, for example `#save-button`; takes precedence over `Target` | `string` |
| Title | Step title | `RenderFragment` |
| Description | Step description | `RenderFragment` |
| Cover | Cover content | `RenderFragment` |
| Placement | Popup placement | `Placement` |
| Mask | Mask setting for this step | `bool?` |
| MaskColor / MaskStyle | Mask color and style for this step | `string` |
| Closable | Whether to show the close button | `bool?` |
| NextText / PreviousText / FinishText | Action button labels | `string` |
