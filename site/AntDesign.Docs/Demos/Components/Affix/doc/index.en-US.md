---
category: Components
type: Navigation
title: Affix
cover: https://gw.alipayobjects.com/zos/alicdn/tX6-md4H6/Affix.svg
---

Wrap Affix around another component to make it stick the viewport.

## When To Use

- On longer web pages, its helpful for some content to stick to the viewport. This is common for menus and actions.
- Please note that Affix should not cover other content on the page, especially when the size of the viewport is small.


## API



| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| OffsetBottom | Offset from the bottom of the viewport (in pixels) | uint?         | -         |
| OffsetTop   | Offset from the top of the viewport (in pixels)| uint?         | 0 |
| TargetSelector | The CSS selector that specifies the scrollable area DOM node | string      |-       |
| ChildContent | Additional Content | RenderFragment         |-       |
| OnChange |Callback for when Affix state is changed| EventCallback&lt;bool>  | -  |

Note: Children of `Affix` must not have the property `position: absolute`, but you can set `position: absolute` on `Affix` itself:
