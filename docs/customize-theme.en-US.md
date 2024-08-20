---
order: 7
title: Customize Theme
---

Ant Design allows you to customize our design tokens to satisfy UI diversity from business or brand requirements, including primary color, border radius, border color, etc.

![customized themes](https://zos.alipayobjects.com/rmsportal/zTFoszBtDODhXfLAazfSpYbSLSEeytoG.png)

## Ant Design Less variables

We are using [Less](http://lesscss.org/) as the development language for styling. A set of less variables are defined for each design aspect that can be customized to your needs.

There are some major variables below, all less variables could be found in [Default Variables](https://github.com/ant-design/ant-design/blob/4.x-stable/components/style/themes/default.less).

```less
@primary-color: #1890ff; // primary color for all components
@link-color: #1890ff; // link color
@success-color: #52c41a; // success state color
@warning-color: #faad14; // warning state color
@error-color: #f5222d; // error state color
@font-size-base: 14px; // major text font size
@heading-color: rgba(0, 0, 0, 0.85); // heading text color
@text-color: rgba(0, 0, 0, 0.65); // major text color
@text-color-secondary: rgba(0, 0, 0, 0.45); // secondary text color
@disabled-color: rgba(0, 0, 0, 0.25); // disable state color
@border-radius-base: 2px; // major border radius
@border-color-base: #d9d9d9; // major border color
@box-shadow-base: 0 3px 6px -4px rgba(0, 0, 0, 0.12), 0 6px 16px 0 rgba(0, 0, 0, 0.08),
  0 9px 28px 8px rgba(0, 0, 0, 0.05); // major shadow for layers
```

## How to do it

We will use [modifyVars](http://lesscss.org/usage/#using-less-in-the-browser-modify-variables) provided by less.js to override the default values of the variables, You can use this [example](https://github.com/ant-design/create-react-app-antd) as a live playground. We now introduce some popular way to do it depends on different workflow.

### Customize in webpack

We take a typical `webpack.config.js` file as example to customize its [less-loader](https://github.com/webpack-contrib/less-loader) options.

```diff
// webpack.config.js
module.exports = {
  rules: [{
    test: /\.less$/,
    use: [{
      loader: 'style-loader',
    }, {
      loader: 'css-loader', // translates CSS into CommonJS
    }, {
      loader: 'less-loader', // compiles Less to CSS
+     options: {
+       lessOptions: { // If you are using less-loader@5 please spread the lessOptions to options directly
+         modifyVars: {
+           'primary-color': '#1DA57A',
+           'link-color': '#1DA57A',
+           'border-radius-base': '2px',
+         },
+         javascriptEnabled: true,
+       },
+     },
    }],
    // ...other rules
  }],
  // ...other config
}
```

Note:

1. Don't exclude `node_modules/antd` when using less-loader.
2. `lessOptions` usage is supported at [less-loader@6.0.0](https://github.com/webpack-contrib/less-loader/releases/tag/v6.0.0).

### Customize in less file

Another approach to customize theme is creating a `less` file within variables to override `antd.less`.

```css
@import '~antd/es/style/themes/default.less';
@import '~antd/dist/antd.less'; // Import Ant Design styles by less entry
@import 'your-theme-file.less'; // variables to override above
```

Note: This way will load the styles of all components, regardless of your demand, which cause `style` option of `babel-plugin-import` not working.


### Dynamic theme

Runtime update theme color please [ref this doc](/docs/react/customize-theme-variable).

## Official Themes 🌈

We have some official themes, try them out and give us some feedback!

- 🌑 Dark Theme (supported in 4.0.0+)
- 📦 Compact Theme (supported in 4.1.0+)
- ☁️ [Aliyun Console Theme (Beta)](https://github.com/ant-design/ant-design-aliyun-theme)


### Use dark or compact theme
