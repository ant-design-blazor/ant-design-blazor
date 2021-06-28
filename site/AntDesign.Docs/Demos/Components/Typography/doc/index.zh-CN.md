---
category: Components
type: 通用
title: Typography
subtitle: 排版
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/GOM1KQ24O/Typography.svg
---

文本的基本格式。

## 何时使用

- 当需要展示标题、段落、列表内容时使用，如文章/博客/日志的文本样式。
- 当需要一列基于文本的基础操作时，如拷贝/省略/可编辑。


## API

Typography.Text

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Code | 添加代码样式 | boolean         | false         |
| Copyable | 	是否可拷贝，为对象时可设置复制文本以回调函数 | boolean         | false         |
| Delete | 	添加删除线样式 | boolean         | false         |
| Disabled | 禁用文本 | boolean         | false         |
| Editable | 是否可编辑，为对象时可对编辑进行控制 | boolean         | false         |
| Ellipsis | 设置自动溢出省略，需要设置元素宽度| boolean         | false         |
| Mark | 添加标记样式 | boolean         | false         |
| Keyboard | 添加键盘样式 | boolean         | false         
| Underline | 添加下划线样式 | boolean         | false         |
| OnChange | 当用户提交编辑内容时触发 | boolean         | false         |
| Strong | 是否加粗 | boolean         | false         |
| Type | 文本类型 `secondary` `warning` `danger` | string         | -         |

Typography.Title

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Copyable | 	是否可拷贝，为对象时可设置复制文本以回调函数 | boolean         | false         |
| Delete | 	添加删除线样式 | boolean         | false         |
| Disabled | 禁用文本 | boolean         | false         |
| Editable | 是否可编辑，为对象时可对编辑进行控制 | boolean         | false         |
| Ellipsis | 设置自动溢出省略，需要设置元素宽度| boolean         | false         |
| Mark | 添加标记样式 | boolean         | false         |
| Level | 重要程度，相当于 h1、h2、h3、h4 | int         | 1         
| Underline | 添加下划线样式 | boolean         | false         |
| OnChange | 当用户提交编辑内容时触发 | boolean         | false         |
| Type | 文本类型 `secondary` `warning` `danger` | string         | -         |

Typography.Paragraph

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Copyable | 	是否可拷贝，为对象时可设置复制文本以回调函数 | boolean         | false         |
| Delete | 	添加删除线样式 | boolean         | false         |
| Disabled | 禁用文本 | boolean         | false         |
| Editable | 是否可编辑，为对象时可对编辑进行控制 | boolean         | false         |
| Ellipsis | 设置自动溢出省略，需要设置元素宽度| boolean         | false         |
| Mark | 添加标记样式 | boolean         | false         |
| Underline | 添加下划线样式 | boolean         | false         |
| OnChange | 当用户提交编辑内容时触发 | boolean         | false         |
| Strong | 是否加粗 | boolean         | false         |
| Type | 文本类型 `secondary` `warning` `danger` | string         | -         |
