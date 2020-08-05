---
category: Components
type: 数据展示
title: Tag
subtitle: 标签
cover: https://gw.alipayobjects.com/zos/alicdn/cH1BOLfxC/Tag.svg
---

进行标记和分类的小标签。

## 何时使用

- 用于标记事物的属性和维度。
- 进行分类。


## API

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Mode |方式选择 `default`, `closable`, `checkable`     | string         |
| Closable | 值标签是否可以关闭| boolean         |-       |
| Checked | 标签是否关闭所对应的值| boolean         |-       |
| CheckedChange | 点击标签时触发的回调| function(e)         |-       |
| Color | 预设标签色 | string   | -         |
| OnClose | 关闭时的回调     | function(e)         | -         |
| Visible | 是否显示标签 | boolean         |
| Icon | 设置图标  | string        | -         |


