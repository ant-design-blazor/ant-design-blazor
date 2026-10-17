---
category: Components
type: Layout
title: Divider
cover: https://gw.alipayobjects.com/zos/alicdn/5swjECahe/Divider.svg
---

A divider line separates different content.

## When To Use

- Divide sections of article.
- Divide inline text and links such as the operation column of table.


## API



| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| Dashed   | whether line is dashed| boolean         |false|
| Orientation | position of title inside divider `left`, `right`, `center` | string         |-       |
| Style |style object of container| CSS properties  | -  |
| Type |direction type of divider| string | -  |
| Plain |divider text show as plain style| boolean  | -  |
| Size |Spacing size for horizontal dividers: `small`, `middle`, or `large`| string | - |
| ClassNames |Semantic CSS classes keyed by `root`, `rail`, and `content`| `IReadOnlyDictionary<string, string>` | - |
| Styles |Semantic inline CSS keyed by `root`, `rail`, and `content`| `IReadOnlyDictionary<string, string>` | - |

## Semantic DOM

| Slot | Class |
| --- | --- |
| root | `.ant-divider` |
| rail | `.ant-divider-rail` |
| content | `.ant-divider-inner-text` |

## Design Token

Divider tokens can be customized through `ConfigProvider.ThemeConfig.Components["Divider"]`, including `textPaddingInline`, `orientationMargin`, and `verticalMarginInline`.
