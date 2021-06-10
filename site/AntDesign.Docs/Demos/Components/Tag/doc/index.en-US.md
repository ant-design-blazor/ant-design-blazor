---
category: Components
type: Data Display
title: Tag
cover: https://gw.alipayobjects.com/zos/alicdn/cH1BOLfxC/Tag.svg
---

Tag for categorizing or markup.

## When To Use

- It can be used to tag by dimension or property.
- When categorizing.

## API

| Property | Description | Type | Default Value | Version | 
| --- | --- | --- | --- | --- |
| Animate | Whether the `Tag` will be animated when it appears or disappears. | boolean         | false       | 0.9 
| ChildContent | Contents of the `Tag`| RenderFragment  |-       |
| Closable | Whether the `Tag` can be closed| boolean         | false       |
| Checkable | Whether the `Tag` can be checked | boolean         | false       |
| Checked | 	Checked status of `Tag` | boolean         |false       |
| CheckedChange | Callback executed when `Tag` is checked/unchecked| Action<bool>         |-       |
| Color | Color of the `Tag` | string   | -         |
| Icon | Set the icon of the `Tag`  | string        | -         |
| OnClose | Callback executed when the `Tag` is closed     | Action<MouseEventArgs>        | -         |
| OnClosing | Callback executed when the `Tag` is being closed. Closing can be canceled here.     | Action<CloseEventArgs<MouseEventArgs>>        | -         |
| OnClick | Callback executed when the `Tag` is clicked (excluding closing button) | Action | -         |
| Visible | Whether the `Tag` is closed or not | boolean         | true         |