---
type: Feedback
category: Components
title: Result
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/9nepwjaLa/Result.svg
---

Used to feed back the results of a series of operational tasks.

## When To Use

Use when important operations need to inform the user to process the results and the feedback is more complicated.

## API

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Title | title string | string\|RenderFragment | - |
| SubTitle | subTitle string | string\|RenderFragment | - |
| Status | result status,decide icons and colors | `success` \| `error` \| `info` \| `warning` \| `404` \| `403` \| `500` | `info` |
| Icon | custom back icon | string (`{type}-{theme}`) | - |
| Extra | operating area | RenderFragment | - |
