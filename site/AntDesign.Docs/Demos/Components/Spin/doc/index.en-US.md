---
category: Components
type: Feedback
title: Spin
cover: https://gw.alipayobjects.com/zos/alicdn/LBcJqCPRv/Spin.svg
---

A spinner for displaying loading state of a page or a section.

## When To Use

When part of the page is waiting for asynchronous data or during a rendering process, an appropriate loading animation can effectively alleviate users' inquietude.

## API

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| Delay | specifies a delay in milliseconds for loading state (prevent flush) | number (milliseconds) | - |
| Indicator | React node of the spinning indicator | ReactNode | - |
| Size | size of Spin, options: `small`, `default` and `large` | string | `default` |
| Spinning | whether Spin is spinning | boolean | true |
| Tip | customize description content when Spin has children | string | - |
| WrapperClassName | className of wrapper when Spin has children | string | - |

### Static Method

- `Spin.setDefaultIndicator(indicator: ReactNode)`

  You can define default spin element globally.
