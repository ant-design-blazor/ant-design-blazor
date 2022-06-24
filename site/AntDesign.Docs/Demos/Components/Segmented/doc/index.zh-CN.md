---
category: Components
subtitle: 分段控制器
type: 数据展示
title: Segmented
cover: https://gw.alipayobjects.com/zos/bmw-prod/a3ff040f-24ba-43e0-92e9-c845df1612ad.svg
---

分段控制器。自 `v0.12.0` 版本开始提供该组件。

## 何时使用

- 用于展示多个选项并允许用户选择其中单个选项；
- 当切换选中选项时，关联区域的内容会发生变化。

## API

> 自 `v0.12.0` 版本开始提供该组件。

### Segmented

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Block | 将宽度调整为父元素宽度的选项 | boolean | false |  |
| DefaultValue | 默认选中的值 | TValue |  |  |
| Disabled | 是否禁用 | boolean | false |  |
| Labels | 使用字符串同时作为 label 和 Value | string[] |  |  |
| OnChange | 选项变化时的回调函数 | EventCallback<TValue> |  |  |
| Options | 数据化配置选项内容 | TValue[] | [] |  |
| Size | 控件尺寸 | `large` \| `middle` \| `small` | - |  |
| Value | 当前选中的值 | TValue |  |  |
