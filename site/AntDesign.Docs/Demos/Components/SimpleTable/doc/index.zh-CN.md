---
category: Components
cols: 1
type: 数据展示
title: SimpleTable
subtitle: 简单表格
cover: https://gw.alipayobjects.com/zos/alicdn/f-SbcX2Lx/Table.svg
---

只保留 `Table` 中简单的样式，使用户可灵活使用`<thead>`、`<tbody>`,`<tr>`、`<th>`、`<td>` 等 HTML 元素进行构建。

## 何时使用

- 当需要展示大量数据量、或者 `Table` 的功能不能满足时。

## 如何使用

自行使用`<thead>`、`<tbody>`,`<tr>`、`<th>`、`<td>` 等原生标签来构建表头和表格的结构。

## API

| 参数             | 说明             | 类型                         | 默认值 |
| ---------------- | ---------------- | ---------------------------- | ------ |
| Loading | 表格是否加载中 | bool | false |
| Title | 表格标题 | string | - |
| TitleTemplate | 标题模板 | RenderFragment | - |
| Footer | 表格尾部 | string | - |
| FooterTemplate | 表格尾部模板 | RenderFragment | - |
| Bordered | 是否展示外边框和列边框 | bool | false |
