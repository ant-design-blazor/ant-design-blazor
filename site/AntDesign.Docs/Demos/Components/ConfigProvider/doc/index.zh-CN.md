---
category: Components
subtitle: 全局化配置
type: 其他
title: ConfigProvider
---

ConfigProvider 用于为组件树提供统一的方向、主题和表单配置。

## 何时使用

当应用需要统一配置 Ant Design 组件的主题、文字方向或全局行为时使用。

## API

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Direction | 文字方向 | string | -- |
| Theme | 全局主题 | GlobalTheme | -- |
| ThemeConfig | Design Token 与组件主题配置 | ThemeConfig | -- |
| Form | 全局表单配置 | FormConfig | -- |

## 主题变量（Design Token）

ConfigProvider 本身不定义组件专属样式 token，而是为全局 Seed、Map、Alias token、预设算法（`default`、`dark`、`compact`）和各组件 token 覆盖提供作用域。v6 token 体系与示例参见[定制主题](/docs/customize-theme)。
