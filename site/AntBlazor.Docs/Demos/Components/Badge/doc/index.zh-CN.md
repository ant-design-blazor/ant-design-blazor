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
| Color | 自定义小圆点的颜色 | string | - |  |
| Count | 展示的数字，大于 overflowCount 时显示为 `${overflowCount}+`，为 0 时隐藏 | ReactNode |  |  |
| Dot | 不展示数字，只有一个小红点 | boolean | false |  |
| Offset | 设置状态点的位置偏移，格式为 `[x, y]` | `[number, number]` | - |  |
| OverflowCount | 展示封顶的数字值 | number | 99 |  |
| ShowZero | 当数值为 0 时，是否展示 Badge | boolean | false |  |
| Status | 设置 Badge 为状态点 | `success` \| `processing` \| `default` \| `error` \| `warning` | '' |  |
| Text | 在设置了 `status` 的前提下有效，设置状态点的文本 | string | '' |  |
| Title | 设置鼠标放在状态点上时显示的文字 | string | `count` |  |

### Badge.Ribbon

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Color | 自定义缎带的颜色 | string | - |  |
| Placement | 缎带的位置 | `start` \| `end` | `end` |  |
| Text | 缎带中填入的内容 | ReactNode | - |  |
