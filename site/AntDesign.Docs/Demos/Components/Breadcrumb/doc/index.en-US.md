---
category: Components
type: Navigation
title: Breadcrumb
cover: https://gw.alipayobjects.com/zos/alicdn/9Ltop8JwH/Breadcrumb.svg
---

A breadcrumb displays the current location within a hierarchy. It allows going back to states higher up in the hierarchy.

## When To Use

- When the system has more than two layers in a hierarchy.
- When you need to inform the user of where they are.
- When the user may need to navigate back to a higher level.


## API


Breadcrumb

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| AutoGenerate | Auto generate navigation from Menu data | bool | - | 
| ChildContent | Custom item renderer | int   | -         |
| Separator | Custom separator | RenderFragment  | -  |


BreadcrumbItem

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| Href | Target of hyperlink | string         | -         |
| Overlay   | The dropdown menu | RenderFragment         |-         |
| OnClick | Set the handler to handle click event | EventCallback<MouseEventArgs>         |-       |
| ChildContent | Title template | RenderFragment  | -  |

