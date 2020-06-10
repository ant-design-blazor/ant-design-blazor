---
category: Components
type: 通用
title: Icon
subtitle: 图标
---

语义化的矢量图形。

## 设计师专属

安装 Kitchen Sketch 插件 💎，就可以一键拖拽使用 Ant Design 和 Iconfont 的海量图标，还可以关联自有项目。

## 图标列表



## API

Common Icon

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| ClassName | 设置图标的样式名 |string         | -         |
| Style   | 设置图标的样式，例如 fontSize 和 color| Css propertities         |
| Spin | 是否有旋转动画 | boolean         |-       |
| Rotate |图标旋转角度（IE9 无效）| int  | -  |
| TwoToneColor |仅适用双色图标。设置双色图标的主要颜色| string  | -  |

We still have three different themes for icons, icon component name is the icon name suffixed by the theme name.

Custom Icon

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Style   | 设置图标的样式，例如 fontSize 和 color| CSSProperties         |
| Spin | 是否有旋转动画 | boolean         |-       |
| Rotate | 图标旋转角度（IE9 无效）| int  | -  |
| Component |控制如何渲染图标，通常是一个渲染根标签为 <svg> 的 React 组件|   | -  |
