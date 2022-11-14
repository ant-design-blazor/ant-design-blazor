---
category: Components
type: General
title: Button
cover: https://gw.alipayobjects.com/zos/alicdn/fNUKzY1sk/Button.svg
---

To trigger an operation.

## When To Use

A button means an operation (or a series of operations). Clicking a button will trigger corresponding business logic.

In Ant Design we provide 4 types of button.

- Primary button: indicate the main action, one primary button at most in one section.
- Default button: indicate a series of actions without priority.
- Dashed button: used for adding action commonly.
- Link button: used for external links.

And 4 other properties additionally.

- `danger`: used for actions of risk, like deletion or authorization.
- `ghost`: used in situations with complex background, home pages usually.
- `disabled`: when actions is not available.
- `loading`: add loading spinner in button, avoiding multiple submits too.

## API



| Property | Description | Type | Default Value | Version 
| --- | --- | --- | --- |
| AriaLabel | Sets the aria-label attribute         | string    | null         |
| Block | Option to fit button width to its parent width         | bool    | false         | 
| ChildContent | Content of the button.   | RenderFragment    | -         |
| Danger | Set the danger status of button | bool    | false         | 
| Disabled | Whether the `Button` is disabled.         | bool    | false     |
| Ghost | Make background transparent and invert text and border colors | bool    | false         | 
| HtmlType | Set the original html `type` of `button`, see: [MDN]('https://developer.mozilla.org/en-US/docs/Web/HTML/Element/button#attr-type')         | string    | `button` |
| Icon | Set the icon component of button | string | -         | 
| Loading | Set the loading status of button        | bool    | false         | 
| OnClick | Callback when `Button` is clicked          | Action    | -         |
| OnClickStopPropagation | Do not propagate events when button is clicked. | bool    | false    |
| Search | Adds class `ant-input-search-button` to the button.   | bool | false         |
| Shape | Can set button shape: `circle` &#124; `round` or `null` (default, which is rectangle).    | string    | null |
| Size | Set the size of `Button`.         | AntSizeLDSType    | `AntSizeLDSType.Default`         | 
| Type | Type of the button.         | ButtonType | `ButtonType.Default` |
| NoSpanWrap | Remove `<span>` from button content, if you want to provide rich content        | bool | false |


### DownloadButton

| Property | Description | Type | Default Value | Version 
| --- | --- | --- | --- |
| Url | The download url of a file    | string    | null         |
| FileName | name of the file         | string    |         | 