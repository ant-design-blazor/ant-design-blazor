---
order: 1
title: 快速上手
---

Ant Design Blazor 致力于提供给程序员**愉悦**的开发体验。

> 在开始之前，推荐先学习 [Blazor](https://blazor.net) 和 [.Net Core](https://dot.net/)，并正确安装和配置了 [.NET Core SDK](https://dotnet.microsoft.com/download) 3.1.300 或以上。官方指南假设你已了解关于 HTML、CSS 和 JavaScript 的初级知识，并且已经完全掌握了 Blazor 的正确开发方式。如果你刚开始学习.NET 或者 Blazor，将 UI 框架作为你的第一步可能不是最好的主意。


## 第一个本地实例

实际项目开发中，你会需要对 C# 代码的构建、调试、代理、打包部署等一系列工程化的需求。
我们强烈建议使用官方的 Visual Studio 2009 或者 VS Code 进行开发，下面我们用一个简单的实例来说明。


### 创建一个项目

> 在创建项目之前，请确保 `.NET Core SDK` 已被成功安装。

执行以下命令，`dotnet new blazorwasm -o ` 会在当前目录下新建一个名称为 `PROJECT-NAME` 的文件夹，并自动安装好相应依赖。

```bash
$ dotnet new blazorwasm -o  PROJECT-NAME
```

### 初始化配置

进入项目文件夹，执行以下命令后将自动完成 `AntDesign` 的 Nuget 包的引用。

```bash
$ dotnet add package AntDesign
```

### 开发调试

一键启动调试，运行成功后显示模板页面。

```bash
$ dotnet run
```

<img style="display: block;padding: 30px 30%;height: 260px;" src="https://img.alicdn.com/tfs/TB1X.qJJgHqK1RjSZFgXXa7JXXa-89-131.svg">


### 构建和部署

```bash
$ ng build --prod
```

入口文件会构建到 `dist` 目录中，你可以自由部署到不同环境中进行引用。

## 自行构建

如果想自己维护工作流，理论上你可以利用 Angular 生态圈中的 各种脚手架进行开发，如果遇到问题可参考我们所使用的 [配置](https://github.com/NG-ZORRO/ng-zorro-antd/tree/master/integration) 进行定制。

### 安装组件

```bash
$ npm install ng-zorro-antd --save
```

### 引入样式

#### 使用全部组件样式

该配置将包含组件库的全部样式，如果只想使用某些组件请查看 [使用特定组件样式](/docs/getting-started/zh#使用特定组件样式) 配置。

在 `angular.json` 中引入了

```json
{
  "styles": [
    "node_modules/ng-zorro-antd/ng-zorro-antd.min.css"
  ]
}
```

在 `style.css` 中引入预构建样式文件

```css
@import "~ng-zorro-antd/ng-zorro-antd.min.css";
```

在 `style.less` 中引入 less 样式文件

```less
@import "~ng-zorro-antd/ng-zorro-antd.less";
```

#### 使用特定组件样式

> 由于组件之间的样式也存在依赖关系，单独引入多个组件的 CSS 可能导致 CSS 的冗余。

使用特定组件样式时前需要先引入基本样式(所有组件的共用样式)。

在 `style.css` 中引入预构建样式文件

```css
@import "~ng-zorro-antd/style/index.min.css"; /* 引入基本样式 */
@import "~ng-zorro-antd/button/style/index.min.css"; /* 引入组件样式 */
```

在 `style.less` 中引入 less 样式文件
```less
@import "~ng-zorro-antd/style/entry.less"; /* 引入基本样式 */
@import "~ng-zorro-antd/button/style/entry.less"; /* 引入组件样式 */
```

### 引入组件模块

最后你需要将想要使用的组件模块引入到你的 `app.module.ts` 文件和[特性模块](https://angular.cn/guide/feature-modules)中。

以下面的 `NzButtonModule` 模块为例，先引入组件模块：

```ts
import { NgModule } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    NzButtonModule
  ]
})
export class AppModule { }
```

然后在模板中使用：

```html
<button nz-button nzType="primary">Primary</button>
```

## 其他

- [国际化配置](/docs/i18n/zh)
- [自定义主题](/docs/customize-theme/zh)
- [使用图标](/components/icon/zh)
