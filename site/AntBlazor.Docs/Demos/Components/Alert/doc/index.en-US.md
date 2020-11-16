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

时间轴

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| AfterClose | Called when close animation is finished | function()         | -         |
| Banner   | Whether to show as banner | bool         | false    |
| Closable | Whether Alert can be closed | bool        | false       |
| CloseText | Close text to show         | string         |
| Description | Additional content of Alert | string  | -  |
| Icon | Custom icon, effective when showIcon is true | RenderFragment  | -  |
| Message | Content of Aler | string  | -  |
| ShowIcon | Whether to show icon | bool  | -  |
| Type | Type of Alert styles, options: success, info, warning, error | string  | Warning  |
| OnClose | Callback when Alert is closed | function()  | -  |
| ChildContent | Additional Content | RenderFragment  | -  |
