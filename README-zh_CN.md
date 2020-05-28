<p align="center">
  <a href="https://yangshunjie.com/ant-design-blazor/">
    <img src="https://raw.githubusercontent.com/ant-design-blazor/ant-design-blazor/master/logo.svg?sanitize=true">
  </a>
</p>

<h1 align="center">Ant Design Blazor</h1>

<div align="center">

基于 Ant Design 和 Blazor 的企业级组件库

![](https://img.shields.io/github/workflow/status/ant-design-blazor/ant-design-blazor/Publish%20Docs?style=flat-square)
[![AntDesign](https://img.shields.io/nuget/v/AntDesign.svg?color=red&style=flat-square)](https://www.nuget.org/packages/AntDesign/)
[![AntDesign](https://img.shields.io/nuget/dt/AntDesign.svg?style=flat-square)](https://www.nuget.org/packages/AntDesign/)
[![AntDesign](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/LICENSE)
[![Slack Group](https://img.shields.io/badge/Slack-AntDesign-blue.svg?style=flat-square&logo=slack)](https://join.slack.com/t/AntDesign/shared_invite/zt-cw1enker-xVw3s93cTf4uhY2lRGRyRw)
[![Ding Talk Group](https://img.shields.io/badge/DingTalk-AntDesign-blue.svg?style=flat-square&logo=data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBzdGFuZGFsb25lPSJubyI/Pg0KPHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIGNsYXNzPSJpY29uIiB2aWV3Qm94PSIwIDAgMTAyNCAxMDI0IiBmaWxsPSIjZmZmZmZmIj4NCiAgPHBhdGggZD0iTTU3My43IDI1Mi41QzQyMi41IDE5Ny40IDIwMS4zIDk2LjcgMjAxLjMgOTYuN2MtMTUuNy00LjEtMTcuOSAxMS4xLTE3LjkgMTEuMS01IDYxLjEgMzMuNiAxNjAuNSA1My42IDE4Mi44IDE5LjkgMjIuMyAzMTkuMSAxMTMuNyAzMTkuMSAxMTMuN1MzMjYgMzU3LjkgMjcwLjUgMzQxLjljLTU1LjYtMTYtMzcuOSAxNy44LTM3LjkgMTcuOCAxMS40IDYxLjcgNjQuOSAxMzEuOCAxMDcuMiAxMzguNCA0Mi4yIDYuNiAyMjAuMSA0IDIyMC4xIDRzLTM1LjUgNC4xLTkzLjIgMTEuOWMtNDIuNyA1LjgtOTcgMTIuNS0xMTEuMSAxNy44LTMzLjEgMTIuNSAyNCA2Mi42IDI0IDYyLjYgODQuNyA3Ni44IDEyOS43IDUwLjUgMTI5LjcgNTAuNSAzMy4zLTEwLjcgNjEuNC0xOC41IDg1LjItMjQuMkw1NjUgNzQzLjFoODQuNkw2MDMgOTI4bDIwNS4zLTI3MS45SDcwMC44bDIyLjMtMzguN2MuMy41LjQuOC40LjhTNzk5LjggNDk2LjEgODI5IDQzMy44bC42LTFoLS4xYzUtMTAuOCA4LjYtMTkuNyAxMC0yNS44IDE3LTcxLjMtMTE0LjUtOTkuNC0yNjUuOC0xNTQuNXoiLz4NCjwvc3ZnPg0K)](https://h5.dingtalk.com/circle/healthCheckin.html?corpId=dingccf128388c3ea40eda055e4784d35b88&2f46=c9b80ba5&origin=11)

</div>

[English](./README.md) | 简体中文

## ✨ 特性

- 🌈 提炼自企业级中后台产品的交互语言和视觉风格。
- 📦 开箱即用的高质量 Blazor 组件，可在多种托管方式共享。
- 💕 支持基于 WebAssembly 的客户端和基于 SignalR 的服务端 UI 事件交互。
- 🎨 支持渐进式 Web 应用（PWA）
- 🛡 使用 C# 构建，多范式静态语言带来高效的开发体验。
- ⚙️ 基于 .NET Standard 2.1，可直接引用丰富的 .NET 类库。
- 🎁 可与已有的 ASP.NET Core MVC、Razor Pages 项目无缝集成。

## 🌈 在线示例

WebAssembly 静态托管页面示例

- [GitHub](https://ant-design-blazor.github.io)
- [Gitee](https://ant-design-blazor.gitee.io/)

## 🖥 支持环境

- .NET Core 3.1。
- Blazor WebAssembly 3.2 正式版。
- 支持服务端双向绑定。
- 支持 WebAssembly 静态文件部署。
- 主流 4 款现代浏览器，以及 Internet Explorer 11+（限 [Blazor Server](https://docs.microsoft.com/en-us/aspnet/core/blazor/supported-platforms?view=aspnetcore-3.1)）。
- 可直接运行在 [Electron](http://electron.atom.io/) 等基于 Web 标准的环境上。

| [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/edge/edge_48x48.png" alt="IE / Edge" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br> Edge / IE | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/firefox/firefox_48x48.png" alt="Firefox" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Firefox | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/chrome/chrome_48x48.png" alt="Chrome" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Chrome | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/safari/safari_48x48.png" alt="Safari" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Safari | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/opera/opera_48x48.png" alt="Opera" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Opera | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/electron/electron_48x48.png" alt="Electron" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Electron |
| :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: |
|                                                                                                 Edge 16 / IE 11†                                                                                                 |                                                                                                        522                                                                                                        |                                                                                                      57                                                                                                       |                                                                                                      11                                                                                                       |                                                                                                    44                                                                                                     |                                                                                                      Chromium 57                                                                                                      |

> 由于 [WebAssembly](https://webassembly.org) 的限制，Blazor WebAssembly 不支持 IE 浏览器，但 Blazor Server 支持 IE 11†。 详见[官网说明](https://docs.microsoft.com/en-us/aspnet/core/blazor/supported-platforms?view=aspnetcore-3.1)。

## 💿 当前版本

- 开发构建： [![AntDesign](https://img.shields.io/nuget/v/AntDesign.svg?color=red&style=flat-square)](https://www.nuget.org/packages/AntDesign/)
- 0.1.0：基本实现组件后发布。

## 🎨 设计规范

与 Ant Design 设计规范定期同步，你可以在线查看[同步日志](https://github.com/ant-design-blazor/ant-design-blazor/actions?query=workflow%3A%22Style+sync+Bot%22)。

## 📦 安装

- 先安装 [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1) 3.1.300 以上版本

- 创建 Blazor WebAssembly 项目

  ```bash
  $ dotnet new blazorwasm -o MyAntDesignApp
  ```

- 进入应用的项目文件夹，安装 Nuget 包引用

  ```bash
  $ cd MyAntDesignApp
  $ dotnet add package AntDesign --version 0.1.0-*
  ```

- 在项目中注册:

  ```csharp
  services.AddAntDesign();
  ```

- 在 `wwwroot/index.html`(WebAssembly) 或 `Pages/_Host.razor`(Server) 中引入静态文件:

  ```html
  <link href="_content/AntDesign/css/ant-design-blazor.css" rel="stylesheet" />
  <script src="_content/AntDesign/js/ant-design-blazor.js"></script>
  ```

- 在 `_Imports.razor` 中加入命名空间

  ```csharp
  @using AntDesign
  ```

- 最后就可以在`.razor`组件中引用啦！

  ```html
  <AntButton Type="primary">Hello World!</AntButton>
  ```

## ⌨️ 本地开发

- 先安装 [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1) 3.1.300 以上版本
- 安装 Node.js（只用于样式文件和互操作所需 TS 文件的构建）
- 克隆到本地开发

  ```bash
  $ git clone git@github.com:ant-design-blazor/ant-design-blazor.git
  $ cd ant-design-blazor
  $ npm install
  $ npm start
  ```

- 打开浏览器访问 https://localhost:5001 ，详情参考[本地开发文档](https://github.com/ant-design-blazor/ant-design-blazor/wiki)。

  > 推荐使用 Visual Studio 2019 开发。

## 🔗 链接

- [文档主页](https://ant-design-blazor.gitee.io)
- [Blazor 官方文档](https://blazor.net)

## 🗺 开发路线

查看[这个 issue](https://github.com/ant-design-blazor/ant-design-blazor/issues/21) 来了解我们 2020 年的开发计划。

## 🤝 如何贡献

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](https://github.com/ant-design-blazor/ant-design-blazor/pulls)

如果你希望参与贡献，欢迎 [Pull Request](https://github.com/ant-design-blazor/ant-design-blazor/pulls)，或给我们 [报告 Bug](https://github.com/ant-design-blazor/ant-design-blazor/issues/new) 。

### 贡献者

感谢所有为本项目做出过贡献的朋友。

<a href="https://github.com/ant-design-blazor/ant-design-blazor/graphs/contributors"><img src="https://opencollective.com/ant-design-blazor/contributors.svg?width=890&button=false"/></a>

## 💕 支持本项目

本项目以 MIT 协议开源，为了能得到够更好的且可持续的发展，我们期望获得更多的支持者，我们将把所得款项用于社区活动和推广。你可以通过如下任何一种方式支持我们:

- [OpenCollective](https://opencollective.com/ant-design-blazor)
- [微信](https://yangshunjie.com/images/qrcode/wepay.jpg)
- [支付宝](https://yangshunjie.com/images/qrcode/alipay.jpg)

我们会把详细的捐赠记录登记在 [捐赠者名单](https://github.com/ant-design-blazor/ant-design-blazor/issues/62)。

## ❓ 社区互助

如果您在使用的过程中碰到问题，可以通过以下途径寻求帮助，同时我们也鼓励资深用户通过下面的途径给新人提供帮助。

- [![Slack Group](https://img.shields.io/badge/Slack-AntDesign-blue.svg?style=flat-square&logo=slack)](https://join.slack.com/t/AntDesign/shared_invite/zt-etfaf1ww-AEHRU41B5YYKij7SlHqajA) (中文/英文)
- [![钉钉群](https://img.shields.io/badge/钉钉-AntDesign-blue.svg?style=flat-square&logo=data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBzdGFuZGFsb25lPSJubyI/Pg0KPHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIGNsYXNzPSJpY29uIiB2aWV3Qm94PSIwIDAgMTAyNCAxMDI0IiBmaWxsPSIjZmZmZmZmIj4NCiAgPHBhdGggZD0iTTU3My43IDI1Mi41QzQyMi41IDE5Ny40IDIwMS4zIDk2LjcgMjAxLjMgOTYuN2MtMTUuNy00LjEtMTcuOSAxMS4xLTE3LjkgMTEuMS01IDYxLjEgMzMuNiAxNjAuNSA1My42IDE4Mi44IDE5LjkgMjIuMyAzMTkuMSAxMTMuNyAzMTkuMSAxMTMuN1MzMjYgMzU3LjkgMjcwLjUgMzQxLjljLTU1LjYtMTYtMzcuOSAxNy44LTM3LjkgMTcuOCAxMS40IDYxLjcgNjQuOSAxMzEuOCAxMDcuMiAxMzguNCA0Mi4yIDYuNiAyMjAuMSA0IDIyMC4xIDRzLTM1LjUgNC4xLTkzLjIgMTEuOWMtNDIuNyA1LjgtOTcgMTIuNS0xMTEuMSAxNy44LTMzLjEgMTIuNSAyNCA2Mi42IDI0IDYyLjYgODQuNyA3Ni44IDEyOS43IDUwLjUgMTI5LjcgNTAuNSAzMy4zLTEwLjcgNjEuNC0xOC41IDg1LjItMjQuMkw1NjUgNzQzLjFoODQuNkw2MDMgOTI4bDIwNS4zLTI3MS45SDcwMC44bDIyLjMtMzguN2MuMy41LjQuOC40LjhTNzk5LjggNDk2LjEgODI5IDQzMy44bC42LTFoLS4xYzUtMTAuOCA4LjYtMTkuNyAxMC0yNS44IDE3LTcxLjMtMTE0LjUtOTkuNC0yNjUuOC0xNTQuNXoiLz4NCjwvc3ZnPg0K)](https://h5.dingtalk.com/circle/healthCheckin.html?corpId=dingccf128388c3ea40eda055e4784d35b88&2f46=c9b80ba5&origin=11) (中文)

<details>
  <summary>点击扫描钉钉二维码</summary>
  <img src="./docs/assets/dingtalk.jpg" width="300">
</details>

## ☀️ 授权协议

[![AntDesign](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/LICENSE)
