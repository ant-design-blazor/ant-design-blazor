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
| OffsetBottom | Offset from the bottom of the viewport (in pixels) | int         | -         |
| OffsetTop   | Offset from the top of the viewport (in pixels)| int         |- |
| Target | Specifies the scrollable area DOM node | RenderFragment         |-       |
| OnChange |Callback for when Affix state is changed| Function()  | -  |

Note: Children of `Affix` must not have the property `position: absolute`, but you can set `position: absolute` on `Affix` itself:
