---
category: Components
type: Data Display
title: Tooltip
cover: https://gw.alipayobjects.com/zos/alicdn/Vyyeu8jq2/Tooltp.svg
---

A simple text popup tip.

## When To Use

- The tip is shown on mouse enter, and is hidden on mouse leave. The `Tooltip` doesn't support complex text or operations.
- To provide an explanation of a `button/text/operation`. It's often used instead of the html `title` attribute.

## Two types

There are 2 rendering approaches for `Tooltip`:  
1. Wraps child element (content of the `Tooltip`) with a `<div>` (default approach).
2. Child element is not wrapped with anything. This approach requires usage of `<Unbound>` tag inside `<Tooltip>` and depending on the child element type (please refer to the first example):
   - html tag: has to have its `@ref` set to `@context.Current` 
   - `Ant Design Blazor` component: has to have its `RefBack` attribute set to `@context`.

## API

| Property | Description                   | Type                               | Default |
| -------- | ----------------------------- | ---------------------------------- | ------- |
| Title    | The text shown in the tooltip | string | string.Empty |
| TitleTemplate | The content shown in the tooltip | RenderFragment | - |

### Common API

The following APIs are shared by Tooltip, Popconfirm, Popover.

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| ArrowPointAtCenter | Whether the arrow is pointed at the center of target | bool | `false` |  |
| AutoAdjustOverflow | Whether to adjust popup placement automatically when popup is off screen | bool | `true` |  |
| DefaultVisible | Whether the floating tooltip card is visible by default | bool | `false` |  |
| PopupContainerSelector | The DOM container of the tip, the default behavior is to create a `div` element in `body` | string (css selector) | -                   |  |
| MouseEnterDelay | Delay in seconds, before tooltip is shown on mouse enter | doube | 0.1 |  |
| MouseLeaveDelay | Delay in seconds, before tooltip is hidden on mouse leave | doube | 0.1 |  |
| OverlayClassName | Class name of the tooltip card | string | - |  |
| OverlayStyle | Style of the tooltip card | string | - |  |
| Placement | The position of the tooltip relative to the target, which can be one of `Top` `Left` `Right` `Bottom` `TopLeft` `TopRight` `BottomLeft` `BottomRight` `LeftTop` `LeftBottom` `RightTop` `RightBottom` | PlacementType | `PlacementType.Top` |  |
| Trigger | Tooltip trigger mode. Could be multiple by passing an array | TriggerType[] | `TriggerType.Hover` |  |
| Visible | Whether the floating tooltip card is visible or not | bool | `false` |  |
| OnVisibleChange | Callback executed when visibility of the tooltip card is changed | EventCallback<bool>   | - |  |

## Note

Please ensure that the child node of `Tooltip` accepts `onMouseEnter`, `onMouseLeave`, `onFocus`, `onClick` events.
