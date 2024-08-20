---
order: 7.1
title: 动态主题（实验性）
---

除了 [less 定制主题](/docs/react/customize-theme) 外，我们还提供了 CSS Variable 版本以支持动态切换主题能力。你可以在 [ConfigProvider](/components/config-provider/#components-config-provider-demo-theme) 进行体验。

## 注意事项

- 该功能通过动态修改 CSS Variable 实现，因而在 IE 中页面将无法正常展示。请先确认你的用户环境是否需要支持 IE。
- 该功能在 `antd@4.17.0-alpha.0` 版本起支持。

## 如何使用

### 引入 antd.variable.min.css

替换当前项目引入样式文件为 CSS Variable 版本：

```diff
-- import 'antd/dist/antd.min.css';
++ import 'antd/dist/antd.variable.min.css';
```
