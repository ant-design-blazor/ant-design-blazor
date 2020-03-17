<p align="center">
  <a href="https://yangshunjie.com/ant-design-blazor/">
    <img src="https://raw.githubusercontent.com/ElderJames/ant-design-blazor/master/logo.svg?sanitize=true">
  </a>
</p>

<h1 align="center">Ant Design Blazor</h1>

<div align="center">

An enterprise-class UI components based on Ant Design and Blazor.

![](https://img.shields.io/github/workflow/status/elderjames/ant-design-blazor/Publish%20Docs?style=flat-square)
[![AntBlazor](https://img.shields.io/nuget/v/AntBlazor.svg?color=red&style=flat-square)](https://www.nuget.org/packages/AntBlazor/)
[![AntBlazor](https://img.shields.io/nuget/dt/AntBlazor.svg?style=flat-square)](https://www.nuget.org/packages/AntBlazor/)
[![AntBlazor](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](https://github.com/ElderJames/ant-design-blazor/blob/master/LICENSE)
</div>

## ✨ 特性

- 提炼自企业级中后台产品的交互语言和视觉风格。
- 开箱即用的高质量 Razor 组件，可在多种托管方式共享。
- 支持基于 WebAssembly 的客户端和基于 SignalR 的服务端UI事件交互。
- 支持渐进式 Web 应用（PWA）
- 使用 C# 构建，多范式静态语言带来高效的开发体验。
- 基于 .NET Standard 2.1，可直接引用丰富的 .NET 类库。
- 可与已有的 ASP.NET Core MVC、Razor Pages 项目无缝集成。

## 🌈 在线示例

由 WebAssembly 构建，托管在 Gitee Pages http://ant-design-blazor.gitee.io/

## 🖥 支持环境

- .NET Core 3.1
- Blazor WebAssembly 3.2 preievew2
- 支持服务端双向绑定
- 支持 WebAssembly 静态文件部署
- 主流4款现代浏览器，以及 Internet Explorer 11+ （使用 [Blazor Server](https://angular.io/guide/browser-support)）
- 可直接运行在 [Electron](http://electron.atom.io/) 等基于 Web 标准的环境上

| [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/edge/edge_48x48.png" alt="IE / Edge" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br> Edge / IE | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/firefox/firefox_48x48.png" alt="Firefox" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Firefox | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/chrome/chrome_48x48.png" alt="Chrome" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Chrome | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/safari/safari_48x48.png" alt="Safari" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Safari | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/opera/opera_48x48.png" alt="Opera" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Opera | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/electron/electron_48x48.png" alt="Electron" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Electron |
| :---------: | :---------: | :---------: | :---------: | :---------: | :---------: |
| Edge 16 / IE 11† | 522 | 57 | 11 | 44 | Chromium 57

> 由于 [WebAssembly](https://webassembly.org) 的缘故，Blazor WebAssembly 不支持 IE 浏览器，但 Blazor Server 支持 IE 11†。 [官网说明](https://docs.microsoft.com/en-us/aspnet/core/blazor/supported-platforms?view=aspnetcore-3.1) 

## 💿 当前版本

- 开发构建： [![AntBlazor](https://img.shields.io/nuget/v/AntBlazor.svg?color=red&style=flat-square)](https://www.nuget.org/packages/AntBlazor/)

- 0.1.0：基本实现组件后发布

## 🎨 设计规范

与 Ant Design 设计规范定期同步，你可以在线查看[同步日志](https://github.com/ElderJames/ant-design-blazor/actions?query=workflow%3A%22Style+sync+Bot%22)。

## 📦 安装

- 先安装 [.NET Core SDK](https://dotnet.microsoft.com/download) 3.1.102 以上版本
- 安装 Blazor WebAssembly 模板
  ```
  $ dotnet new -i Microsoft.AspNetCore.Components.WebAssembly.Templates::3.2.0-preview2.20160.5
  ```   
- 创建 Blazor WebAssembly 项目
  ```
  $ dotnet new blazorwasm -o MyAntBlazorApp
  ```
- 进入应用的项目文件夹，安装 Nuget 包引用
  ```bash
  $ cd MyAntBlazorApp
  $ dotnet add package AntBlazor --version 0.0.1-nightly-55111624
  ```
- 在项目中注册:
  ```
  services.AddAntBlazor();
  ```
- 在 `wwwroot/index.html`(WebAssembly) 或 `Pages/_Host.razor`(Server) 中引入静态文件:
  ```
    <link href="_content/AntBlazor/css/ant-design-blazor.css" rel="stylesheet">
    <script src="_content/AntBlazor/js/ant-design-blazor.js"></script>
  ```
- 在 `_Imports.razor` 中加入命名空间
  ```
  @using AntBlazor
  ```
- 最后就可以在`.razor`组件中引用啦！
  ```
  <AntButton type="primary">Hello World!</AntButton>
  ```

## 🔨 本地开发

- 先安装 [.NET Core SDK](https://dotnet.microsoft.com/download) 3.1.102 以上版本
- 安装 Node.js（只用于样式文件和互操作所需 TS 文件的构建）
- 克隆到本地开发
  ```
  $ git clone git@github.com:ElderJames/ant-design-blazor.git
  $ cd ant-design-blazor
  $ npm install
  $ npm start
  ```
  打开浏览器访问 https://localhost:5001 ，详情参考[本地开发文档](https://github.com/ElderJames/ant-design-blazor/wiki)。
  
  > 推荐使用 Visual Studio 2019 开发，目前需运行 `AntBlazor.Docs.ClientApp` 项目才能进行断点调试。

## 🔗 链接

- [文档主页](https://ant-design-blazor.gitee.io)
- [Blazor 官方文档](https://blazor.net)

## 🗺 开发路线

查看 [这个 issue](https://github.com/ElderJames/ant-design-blazor/issues/21) 来了解我们 2020 年的开发计划。

## 🤝 如何贡献

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](https://github.com/ElderJames/ant-design-blazor/pulls)
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FElderJames%2Fant-design-blazor.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2FElderJames%2Fant-design-blazor?ref=badge_shield)


如果你希望参与贡献，欢迎 [Pull Request](https://github.com/ElderJames/ant-design-blazor/pulls)，或给我们 [报告 Bug](https://github.com/ElderJames/ant-design-blazor/issues/new) 。

## ❓ 社区互助

如果您在使用的过程中碰到问题，可以通过 [钉钉群](https://h5.dingtalk.com/circle/healthCheckin.html?corpId=dingccf128388c3ea40eda055e4784d35b88&2f46=c9b80ba5&origin=11) 寻求帮助，同时我们也鼓励资深用户通过下面的途径给新人提供帮助。

<img src="./docs/assets/dingtalk.jpg" width="300">

## ☀️ 授权协议

[![AntBlazor](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](https://github.com/ElderJames/ant-design-blazor/blob/master/LICENSE)

## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FElderJames%2Fant-design-blazor.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2FElderJames%2Fant-design-blazor?ref=badge_large)