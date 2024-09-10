---
category: Components
type: Feedback
title: Popconfirm
cover: https://gw.alipayobjects.com/zos/alicdn/fjMCD9xRq/Popconfirm.svg
---

A simple and compact confirmation dialog of an action.

## When To Use

A simple and compact dialog used for asking for user confirmation.

The difference with the `confirm` modal dialog is that it's more lightweight than the static popped full-screen confirm modal.

## Two types

There are 2 rendering approaches for `Popconfirm`:  
1. Wraps child element (content of the `Popconfirm`) with a `<div>` (default approach).
2. Child element is not wrapped with anything. This approach requires usage of `<Unbound>` tag inside `<Popconfirm>` and depending on the child element type (please refer to the first example):
   - html tag: has to have its `@ref` set to `@context.Current` 
   - `Ant Design Blazor` component: has to have its `RefBack` attribute set to `@context`.
   
## API

| Param | Description | Type | Default value |
| --- | --- | --- | --- |
| cancelText | text of the Cancel button | string | `Cancel` |
| okText | text of the Confirm button | string | `OK` |
| okType | Button `type` of the Confirm button | string | `primary` |
| okButtonProps | The ok button props | [ButtonProps](/components/button/#API) | - |
| cancelButtonProps | The cancel button props | [ButtonProps](/components/button/#API) | - |
| title | title of the confirmation box | string\|ReactNode\|() => ReactNode | - |
| onCancel | callback of cancel | function(e) | - |
| onConfirm | callback of confirmation | function(e) | - |
| icon | customize icon of confirmation | ReactNode | `<ExclamationCircle />` |
| disabled | is show popconfirm when click its childrenNode | boolean | false |

Consult [Tooltip's documentation](/components/tooltip/#API) to find more APIs.

## Note

Please ensure that the child node of `Popconfirm` accepts `onMouseEnter`, `onMouseLeave`, `onFocus`, `onClick` events.
