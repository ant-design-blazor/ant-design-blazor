---
category: Components
subtitle: 徽标数
type: 数据展示
title: Badge
cover: https://gw.alipayobjects.com/zos/antfincdn/6%26GF9WHwvY/Badge.svg
---

图标右上角的圆形徽标数字。

## 何时使用

一般出现在通知图标或头像的右上角，用于显示需要处理的消息条数，通过醒目视觉形式吸引用户处理。

## API

### Badge

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Color | 自定义徽章状态点颜色。使用此参数将使徽章成为状态点。 | string | - |  |
| Count | 徽章中显示的编号 | int |  |  |
| CountTemplate | 代替计数显示的模板 | RenderFragment |  |  |
| Dot | 不展示数字，只有一个小红点 | boolean | false |  |
| Offset | 设置状态点的位置偏移，格式为 `[x, y]` | `[number, number]` | - |  |
| OverflowCount | 展示封顶的数字值 | number | 99 |  |
| ShowZero | 当数值为 0 时，是否展示 Badge | boolean | false |  |
| Status | 将徽章点设置为状态颜色。使用此参数将使徽章成为状态点。 | `success` \| `processing` \| `default` \| `error` \| `warning` | '' |  |
| Size | 在设置了 `count` 的前提下有效，设置小圆点的大小 | `default` \| `small` | - |  |
| Text | 状态点旁边的显示文本 | string | '' |  |
| Title | 设置鼠标放在状态点上时显示的文字 | string | `count` |  |

### Badge.Ribbon

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Color | 自定义缎带的颜色 | string | - |  |
| Placement | 缎带的位置 | `start` \| `end` | `end` |  |
| Text | 缎带中填入的内容 | ReactNode | - |  |
