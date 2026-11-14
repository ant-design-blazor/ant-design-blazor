---
category: Components
type: Feedback
title: Drawer
cover: https://gw.alipayobjects.com/zos/alicdn/7z8NJQhFb/Drawer.svg
---

A panel which slides in from the edge of the screen.

## When To Use

A Drawer is a panel that is typically overlaid on top of a page and slides in from the side.
It contains a set of information or actions.  
Since the user can interact with the Drawer without leaving the current page,
tasks can be achieved more efficiently within thesame context.

- Use a Form to create or edit a set of information.
- Processing subtasks. When subtasks are too heavy for a Popover and we still want to keep the subtasks in the context of the main task,
  Drawer comes very handy.
- When the same Form is needed in multiple places.

## API

### Drawer

| Property      | Description                                                                                    | Type           | Default Value |
| ------------- | ---------------------------------------------------------------------------------------------- | -------------- | ------------- |
| Title         | the title for drawer                                                                           | string or slot | -             |
| BodyStyle     | Style of the drawer content part                                                               | object         | -             |
| HeaderStyle   | Style of the header content part                                                               | object         | -             |
| Closable      | Whether a close (x) button is visible on top right of the Drawer dialog or not.                | boolean        | true          |
| ChildContent  | Subcomponent                                                                                   | RenderFragment | -             |
| Content       | Content for Drawer. When given with ChildContent this is shown above ChildContent.             | OneOf<RenderFragment, string> | -             |
| MaskClosable  | Clicking on the mask (area outside the Drawer) will close the Drawer or not.                   | boolean        | true          |
| Mask          | Whether to show mask or not. When true the area around the Drawer will darken when Visible.    | boolean        | true          |
| Placement     | The placement of the Drawer, option could be `left` , `top`,`right`,`bottom`                   | string         | `right`       |
| Width         | Width of the Drawer. Only used when placement is 'left' or 'right'.                            | int            | 256           |
| Height        | Height of the Drawer. Only used when Placement is 'top' or 'bottom'                            | int            | 256           |
| ZIndex        | The z-index of the Drawer.                                                                     | int            | 1000          |
| OffsetX       | The the X coordinate offset(px). Only used when placement is `'left'` or `'right'`.            | int            | 0             |
| OffsetY       | The the Y coordinate offset(px). Only used when placement is `'top'` or `'bottom'`.            | int            | 0             |
| Visible       | Whether the Drawer dialog is visible or not.                                                   | boolean        | -             |
| Keyboard      | Whether support press esc to close. **Not currently implemented**                              | boolean        | true          |
| Handler       | Content that renders as a sibling to the content of the Drawer                                 | RenderFragment | -             |
| OnClose       | Specify a callback that will be called when a user clicks mask, close button or Cancel button. | EventCallback  | -             |
| OnOpen        | Specify a callback that will be called after drawer rendered                                   | Func<Task>     | -             |
| MaskStyle     | Style for Drawer's mask element.                                                               | string         | -             |
| BodyStyle     | Style of the drawer content part                                                               | string         | -             |
| WrapClassName | The class name of the container of the Drawer dialog.                                          | string         | -             |

### DrawerService

| Property    | Description               | Type                            | Return Value   |
| ----------- | ------------------------- | ------------------------------- | -------------- |
| CreateAsync | create and open an Drawer | `DrawerConfig`                  | `DrawerRef`    |
| CreateAsync | create and open an Drawer | `DrawerConfig` , TContentParams | `DrawerRef<R>` |

### DrawerOptions

| Property          | Description                                                                     | Type                                     | Default Value |
| ----------------- | ------------------------------------------------------------------------------- | ---------------------------------------- | ------------- |
| Content           | The drawer body content.                                                        | `OneOf<RenderFragment, string>`          | -             |
| ContentParams     | The component inputs the param / The Template context.                          | `D`                                      | -             |
| Closable          | Whether a close (x) button is visible on top right of the Drawer dialog or not. | `boolean`                                | `true`        |
| MaskClosable      | Clicking on the mask (area outside the Drawer) to close the Drawer or not.      | `boolean`                                | `true`        |
| Mask              | Whether to show mask or not.                                                    | `boolean`                                | `true`        |
| CloseOnNavigation | Whether to close the drawer when the navigation history changes                 | `boolean`                                | `true`        |
| Keyboard          | Whether to support keyboard esc off                                             | `boolean`                                | `true`        |
| MaskStyle         | Style for Drawer's mask element.                                                | `string`                                 | `{}`          |
| BodyStyle         | Body style for Drawer body element. Such as height, padding etc.                | `string`                                 | `{}`          |
| HeaderStyle       | Header style for Drawer header element. Such as height, padding etc.            | `string`                                 | `{}`          |
| Title             | The title for Drawer.                                                           | `OneOf<RenderFragment, string>`          | -             |
| Width             | Width of the Drawer dialog.                                                     | `int`                                    | `256`         |
| Height            | Height of the Drawer dialog, only when placement is `'top'` or `'bottom'`.      | `int`                                    | `256`         |
| WrapClassName     | The class name of the container of the Drawer dialog.                           | `string`                                 | -             |
| ZIndex            | The `z-index` of the Drawer.                                                    | `int`                                    | `1000`        |
| Placement         | The placement of the Drawer.                                                    | `'top' \| 'right' \| 'bottom' \| 'left'` | `'right'`     |
| OffsetX           | The the X coordinate offset(px), only when placement is `'left'` or `'right'`.  | `int`                                    | `0`           |
| OffsetY           | The the Y coordinate offset(px), only when placement is `'top'` or `'bottom'`.  | `int`                                    | `0`           |

### DrawerRef

| Property   | Description       | Type |
| ---------- | ----------------- | ---- |
| CloseAsync | close the drawer. |      |
| OpenAsync  | open the drawer.  |      |
