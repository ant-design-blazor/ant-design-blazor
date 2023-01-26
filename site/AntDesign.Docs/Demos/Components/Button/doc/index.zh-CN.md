---
category: Components
type: 通用
title: Button
subtitle: 按钮
cover: https://gw.alipayobjects.com/zos/alicdn/fNUKzY1sk/Button.svg
---

按钮用于开始一个即时操作。

## 何时使用

标记了一个（或封装一组）操作命令，响应用户点击行为，触发相应的业务逻辑。

在 Ant Design 中，我们有四种按钮。

- 主按钮：用于主行动点，一个操作区域只能有一个主按钮。
- 默认按钮：用于没有主次之分的一组行动点。
- 虚线按钮：常用于添加操作。
- 链接按钮：用于次要或外链的行动点。

以及四种状态属性与上面配合使用。

- 危险：删除/移动/修改权限等危险操作，一般需要二次确认。
- 幽灵：用于背景色比较复杂的地方，常用在首页/产品页等展示场景。
- 禁用：行动点不可用的时候，一般需要文案解释。
- 加载中：用于异步操作等待反馈的时候，也可以避免多次提交。

## API



| Property | Description | Type | Default Value | Version 
| --- | --- | --- | --- |
| AriaLabel | Sets the aria-label attribute         | string    | null         |
| Block | 将按钮宽度调整为其父宽度的选项 | bool    | false         | 
| ChildContent | Content of the button.   | RenderFragment    | -         |
| Danger | 设置危险按钮 | bool    | false         | 
| Disabled | 按钮失效状态         | bool    | false     |
| Ghost | 幽灵属性，使按钮背景透明 | bool    | false         | 
| HtmlType | 设置 button 原生的 type 值，可选值请参考 [HTML 标准]('https://developer.mozilla.org/en-US/docs/Web/HTML/Element/button#attr-type')         | string    | `button` |
| Icon | 设置按钮的图标组件 | string | -         | 
| Loading | 设置按钮载入状态        | bool    | false         | 
| OnClick | Callback when `Button` is clicked          | Action    | -         |
| OnClickStopPropagation | Do not propagate events when button is clicked. | bool    | false    |
| Search | Adds class `ant-input-search-button` to the button.   | bool | false         |
| Shape | Can set button shape: `circle` &#124; `round` or `null` (default, which is rectangle).    | string    | null |
| Size | 设置按钮大小        | AntSizeLDSType    | `AntSizeLDSType.Default`         | 
| Type | 设置按钮类型        | ButtonType | `ButtonType.Default` |
| NoSpanWrap | Remove `<span>` from button content, if you want to provide rich content        | bool | false |


### DownloadButton

| Property | Description | Type | Default Value | Version 
| --- | --- | --- | --- |
| Url | 文件下载地址     | string    | null         |
| FileName |  指定文件名        | string    | false         | 