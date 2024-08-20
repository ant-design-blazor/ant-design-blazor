---
order: 7
title: 定制主题
---

Ant Design 设计规范和技术上支持灵活的样式定制，以满足业务和品牌上多样化的视觉需求，包括但不限于全局样式（主色、圆角、边框）和指定组件的视觉定制。

![一些配置好的主题](https://zos.alipayobjects.com/rmsportal/zTFoszBtDODhXfLAazfSpYbSLSEeytoG.png)

## Ant Design Blazor 的样式变量

Ant Design Blazor 的样式沿用 antd 使用了 [Less](http://lesscss.org/) 作为开发语言，并定义了一系列全局/组件的样式变量，你可以根据需求进行相应调整。

以下是一些最常用的通用变量，所有样式变量可以在 [这里](https://github.com/ant-design/ant-design/blob/4.x-stable/components/style/themes/default.less) 找到。

```less
@primary-color: #1890ff; // 全局主色
@link-color: #1890ff; // 链接色
@success-color: #52c41a; // 成功色
@warning-color: #faad14; // 警告色
@error-color: #f5222d; // 错误色
@font-size-base: 14px; // 主字号
@heading-color: rgba(0, 0, 0, 0.85); // 标题色
@text-color: rgba(0, 0, 0, 0.65); // 主文本色
@text-color-secondary: rgba(0, 0, 0, 0.45); // 次文本色
@disabled-color: rgba(0, 0, 0, 0.25); // 失效色
@border-radius-base: 2px; // 组件/浮层圆角
@border-color-base: #d9d9d9; // 边框色
@box-shadow-base: 0 3px 6px -4px rgba(0, 0, 0, 0.12), 0 6px 16px 0 rgba(0, 0, 0, 0.08),
  0 9px 28px 8px rgba(0, 0, 0, 0.05); // 浮层阴影
```

如果以上变量不能满足你的定制需求，可以给我们提 issue。

## 定制方式

原理上是使用 less 提供的 [modifyVars](http://lesscss.org/usage/#using-less-in-the-browser-modify-variables) 的方式进行覆盖变量，可以在本地运行查看定制效果。

### 配置 less 变量文件

在 AntDesign 的Nuget包中已包含所有组件的less文件，在发布后，会在输出目录生成。如生成目录是 `publish`，则 less 文件位于 `publish\wwwroot\_content\AntDesign\less`。

一种方式是建立一个单独的 `less` 变量文件，引入这个文件覆盖 `antd.less` 里的变量。

```css
@import '~antd/es/style/themes/default.less';
@import '~antd/dist/antd.less'; // 引入官方提供的 less 样式入口文件
@import 'your-theme-file.less'; // 用于覆盖上面定义的变量
```

### 动态主题色

在运行时调整主题色请[参考此处](/docs/react/customize-theme-variable)。


## 官方主题 🌈

我们提供了一些官方主题，欢迎在项目中试用，并且给我们提供反馈。

- 🌑 暗黑主题（4.0.0+ 支持）
- 📦 紧凑主题（4.1.0+ 支持）
- ☁️ [阿里云控制台主题（Beta）](https://github.com/ant-design/ant-design-aliyun-theme)

### 使用暗黑主题和紧凑主题

![](https://gw.alipayobjects.com/mdn/rms_08e378/afts/img/A*mYU9R4YFxscAAAAAAAAAAABkARQnAQ)
