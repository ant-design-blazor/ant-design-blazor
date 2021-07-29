﻿---
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
| Closable | Whether the Tag can be closed| boolean         |-       |
| Checkable | Whether the Tag can be checked | boolean         |-       |
| Checked | 	Checked status of Tag| boolean         |-       |
| CheckedChange | Callback executed when Tag is checked/unchecked| function(e)         |-       |
| Color | Color of the Tag | string   | -         |
| PresetColor | The Preset Color of the Tag | PresetColor   | -         |
| OnClose | Callback executed when tag is closed     | function(e)        | -         |
| Visible | Whether the Tag is closed or not | boolean         | true         |
| Icon | Set the icon of tag  | string        | -         |