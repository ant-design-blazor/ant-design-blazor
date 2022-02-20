---
category: Components
type: General
title: Icon
cover: https://gw.alipayobjects.com/zos/alicdn/rrwbSt3FQ/Icon.svg
---

Semantic vector graphics. Before use icons。

## List of icons



## API

Common Icon

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| Alt | Alternative text for the icon | string | - |
| ClassName | className of Icon |string         | -         |
| Style   | Style properties of icon, like fontSize and color| Css propertities         |
| Spin | Rotate icon with animation | boolean         |-       |
| Rotate |Rotate by n degrees (not working in IE9)| int  | -  |
| TwoToneColor |Only supports the two-tone icon. Specify the primary color.| string  | -  |

We still have three different themes for icons, icon component name is the icon name suffixed by the theme name.

Custom Icon

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| Style   | Style properties of icon, like fontSize and color| CSSProperties         |
| Spin | Rotate icon with animation | boolean         |-       |
| Rotate | Rotate degrees (not working in IE9)| int  | -  |
| Component |The component used for the root node.|   | -  |