---
category: Components
type: Data Display
title: Popover
cover: https://gw.alipayobjects.com/zos/alicdn/1PNL1p_cO/Popover.svg
---

The floating card popped by clicking or hovering.

## When To Use

A simple popup menu to provide extra information or operations.

Comparing with `Tooltip`, besides information `Popover` card can also provide action elements like links and buttons.

## Two types

There are 2 rendering approaches for `Popover`:  
1. Wraps child element (content of the `Popover`) with a `<div>` (default approach).
2. Child element is not wrapped with anything. This approach requires usage of `<Unbound>` tag inside `<Popover>` and depending on the child element type (please refer to the first example):
   - html tag: has to have its `@ref` set to `@context.Current` 
   - `Ant Design Blazor` component: has to have its `RefBack` attribute set to `@context`.

## API

| Param   | Description         | Type                               | Default value | Version |
| ------- | ------------------- | ---------------------------------- | ------------- | ------- |
| content | Content of the card | string\|ReactNode\|() => ReactNode | -             |         |
| title   | Title of the card   | string\|ReactNode\|() => ReactNode | -             |         |

Consult [Tooltip's documentation](/components/tooltip/#API) to find more APIs.

## Note

Please ensure that the child node of `Popover` accepts `onMouseEnter`, `onMouseLeave`, `onFocus`, `onClick` events.
