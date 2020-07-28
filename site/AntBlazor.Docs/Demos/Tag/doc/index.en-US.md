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

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| Mode |mode select `default`, `closable`, `checkable`     | string         |
| Closable | Whether the Tag can be closed| boolean         |-       |
| Checked | 	Checked status of Tag| boolean         |-       |
| CheckedChange | Callback executed when Tag is checked/unchecked| function(e)         |-       |
| Color | Color of the Tag | string   | -         |
| OnClose | Callback executed when tag is closed     | function(e)        | -         |
| Visible | Whether the Tag is closed or not | boolean         | true         |
| Icon | Set the icon of tag  | string        | -         |