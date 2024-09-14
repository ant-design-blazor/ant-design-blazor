---
category: Components
type: Feedback
title: Alert
cover: https://gw.alipayobjects.com/zos/alicdn/8emPa3fjl/Alert.svg
---

Alert component for feedback.

## When To Use

- When you need to show alert messages to users.
- When you need a persistent static container which is closable by user actions.

## API

### Alert Props

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| AfterClose | Called when close animation is finished | EventCallback&lt;MouseEventArgs> | - |  |
| Banner | Whether to show as banner | Banner | false |  |
| Closable | Whether Alert can be closed | bool | false |  |
| CloseText | Close text to show | string | - |  |
| Description | Additional content of Alert | string | - |  |
| Icon | Custom icon, effective when `ShowIcon` is `true` | RenderFragment | - |  |
| Message | Content of Alert | string | - |  |
| MessageTemplate | Template for message | RenderFragment | - | |
| ShowIcon | Whether to show icon | bool | false |  |
| Type | Type of Alert styles, options: `success` \| `info` \| `warning` \| `error` | string | warning |  |
| OnClose | Callback when Alert is closed | EventCallback&lt;MouseEventArgs> | - |  |
| ChildContent | Additional Content shown like description| RenderFragment | - |  |
