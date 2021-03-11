---
category: Components
type: Other
title: BackTop
cover: https://gw.alipayobjects.com/zos/alicdn/tJZ5jbTwX/BackTop.svg
---

`BackTop` makes it easy to go back to the top of the page.

## When To Use

- When the page content is very long.
- When you need to go back to the top very frequently in order to view the contents.


## API

BackTop

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| TargetSelector | 	specifies the scrollable area dom selector | string    | -         |
| VisibilityHeight   | the `BackTop` button will not show until the scroll height reaches this value| int         |-    |
| OnClick | a callback function, which can be executed when you click the button | function         |-       |
| ChildContent | Custom Content | `RenderFragment`         | -       |
| Visible | Display the button | bool         | -       |