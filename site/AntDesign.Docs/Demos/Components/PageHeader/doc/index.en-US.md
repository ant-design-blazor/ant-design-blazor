---
category: Components
type: Navigation
title: PageHeader
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/6bKE0Cq0R/PageHeader.svg
---

A header with common actions and design elements built in.

## When To Use

PageHeader can be used to highlight the page topic, display important information about the page, and carry the action items related to the current page (including page-level operations, inter-page navigation, etc.) It can also be used as inter-page navigation.

## API

### PageHeader
| 参数 | 说明 | 类型 | 默认值 | 全局配置 |
| --- | --- | --- | --- | --- |
| `Ghost` | Make background transparent | `boolean` | `true` | - |
| `Title` | Title string | `string \| RenderFragment` | - | - |
| `Subtitle` | subTitle string | `string \| RenderFragment` | - | - |
| `BackIcon` | Custom back icon | `bool \| string \| RenderFragment` | - | - |
| `OnBack` | Back icon click event | `EventCallback` | default Call history.back| - |

### Page Header 组成部分
| 元素 | 说明 |
| ----- | ----------- | ---- | ------------- |
| `PageHeaderTitle` | Title section |
| `PageHeaderSubtitle` | Subtitle section，`Subtitle` has high priority |
| `PageHeaderContent` | Content section |
| `PageHeaderFooter` | Footer section |
| `PageHeaderTags` |  Tags container after the title |
| `PageHeaderExtra` | Operating area, at the end of the line of the title line |
| `PageHeaderBreadcrumb` | Breadcrumb section |
| `PageHeaderAvatar` | Avatar section |