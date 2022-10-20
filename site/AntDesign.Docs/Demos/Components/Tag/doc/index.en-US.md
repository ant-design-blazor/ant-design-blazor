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
| Checkable | Whether the `Tag` can be checked | boolean         | false       |
| Checked | 	Checked status of `Tag` | boolean         |false       |
| CheckedChange | Callback executed when `Tag` is checked/unchecked| Action<bool>         |-       |
| ChildContent | Contents of the `Tag`| RenderFragment  |-       |
| Class | Any css class that will be added to tag. Use case: adding animation. | string   | -  | 0.9 
| Closable | Whether the `Tag` can be closed| boolean         | false       |
| Color | Color of the `Tag` | string   | "default"         |
| Icon | Set the icon of the `Tag`  | string        | -         |
| OnClick | Callback executed when the `Tag` is clicked (excluding closing button) | Action | -         |
| OnClose | Callback executed when the `Tag` is closed     | Action<MouseEventArgs>        | -         |
| OnClosing | Callback executed when the `Tag` is being closed. Closing can be canceled here.     | Action<CloseEventArgs<MouseEventArgs>>        | -         |
| Visible | Whether the `Tag` is closed or not | boolean         | true         |