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

## 主题定制方式

Ant Design Blazor 提供了两种主题定制的方法：

### 1. 使用 Gulp 编译

本项目样式通过 Gulp 编译成 CSS，您可以通过在编译配置中修改变量来达到修改主题的目的。

在 AntDesign 的 Nuget 包中已包含所有组件的 less 文件，在发布后会在输出目录生成。如生成目录是 `publish`，则 less 文件位于 `publish\wwwroot\_content\AntDesign\less` 目录下。主题入口文件为 `ant-design-blazor.less`。以下是一个配置示例：

创建 `gulp.theme.js` 文件：

```javascript
const gulp = require("gulp");
const less = require("gulp-less");

gulp.task("theme", function () {
  const lessOptions = {
    modifyVars: {
      "primary-color": "#1DA57A",
      "link-color": "#1DA57A",
      "border-radius-base": "2px",
    },
    javascriptEnabled: true,
  };

  return gulp
    .src(
      "path/to/publish/wwwroot/_content/AntDesign/less/ant-design-blazor.less"
    )
    .pipe(less(lessOptions))
    .pipe(gulp.dest("./wwwroot/css"));
});
```

您可以通过修改 `modifyVars` 中的变量值来自定义主题。这些变量会覆盖 `default.less` 中的默认值。

### 2. 使用 CSS 变量

Ant Design Blazor 提供了 CSS 变量文件（`ant-design-blazor.variable.css`），允许您在运行时自定义主题。这种方法更加灵活，因为它不需要重新编译。

首先，在项目中引入 CSS 变量文件：

```html
<link
  href="_content/AntDesign/css/ant-design-blazor.variable.css"
  rel="stylesheet"
/>
```

然后您可以在自己的样式表中覆盖这些 CSS 变量：

```css
:root {
  --ant-primary-color: #1890ff;
  --ant-primary-color-hover: #40a9ff;
  --ant-primary-color-active: #096dd9;
  --ant-primary-color-outline: rgba(24, 144, 255, 0.2);
  --ant-success-color: #52c41a;
  --ant-warning-color: #faad14;
  --ant-error-color: #ff4d4f;
  --ant-font-size-base: 14px;
  --ant-heading-color: rgba(0, 0, 0, 0.85);
  --ant-text-color: rgba(0, 0, 0, 0.65);
  --ant-text-color-secondary: rgba(0, 0, 0, 0.45);
  --ant-disabled-color: rgba(0, 0, 0, 0.25);
  --ant-border-radius-base: 2px;
  --ant-border-color-base: #d9d9d9;
}
```

您还可以使用 JavaScript 动态更改这些变量：

```javascript
document.documentElement.style.setProperty("--ant-primary-color", "#1DA57A");
```

这种方法特别适用于：

- 实现动态主题切换
- 运行时调整主题
- 支持用户自定义主题

注意：CSS 变量方法提供了更好的浏览器兼容性和更容易的运行时修改，而 Gulp 编译方法更适合静态主题或需要支持较旧浏览器的场景。

## 官方主题 🌈

我们提供了一些官方主题，欢迎在项目中试用，并且给我们提供反馈。

![](https://gw.alipayobjects.com/mdn/rms_08e378/afts/img/A*mYU9R4YFxscAAAAAAAAAAABkARQnAQ)

想使用官方主题，只需替换对应的 css 文件即可。

默认主题：`_content/AntDesign/css/ant-design-blazor.css`
暗黑主题：`_content/AntDesign/css/ant-design-blazor.dark.css`
紧凑主题：`_content/AntDesign/css/ant-design-blazor.compact.css`
阿里云主题：`_content/AntDesign/css/ant-design-blazor.aliyun.css`
