<p align="center">
  <a href="https://yangshunjie.com/ant-design-blazor/">
    <img src="https://raw.githubusercontent.com/ant-design-blazor/ant-design-blazor/master/logo.svg?sanitize=true">
  </a>
</p>

<h1 align="center">Ant Design Blazor</h1>

<div align="center">

An enterprise-class UI components based on Ant Design and Blazor.

![](https://img.shields.io/github/workflow/status/ant-design-blazor/ant-design-blazor/Publish%20Docs?style=flat-square)
[![AntBlazor](https://img.shields.io/nuget/v/AntBlazor.svg?color=red&style=flat-square)](https://www.nuget.org/packages/AntBlazor/)
[![AntBlazor](https://img.shields.io/nuget/dt/AntBlazor.svg?style=flat-square)](https://www.nuget.org/packages/AntBlazor/)
[![AntBlazor](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/LICENSE)
[![Slack Group](https://img.shields.io/badge/Slack-AntBlazor-blue.svg?style=flat-square&logo=slack)](https://join.slack.com/t/antblazor/shared_invite/zt-cw1enker-xVw3s93cTf4uhY2lRGRyRw)
[![Ding Talk Group](https://img.shields.io/badge/DingTalk-AntBlazor-blue.svg?style=flat-square&logo=data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBzdGFuZGFsb25lPSJubyI/Pg0KPHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIGNsYXNzPSJpY29uIiB2aWV3Qm94PSIwIDAgMTAyNCAxMDI0IiBmaWxsPSIjZmZmZmZmIj4NCiAgPHBhdGggZD0iTTU3My43IDI1Mi41QzQyMi41IDE5Ny40IDIwMS4zIDk2LjcgMjAxLjMgOTYuN2MtMTUuNy00LjEtMTcuOSAxMS4xLTE3LjkgMTEuMS01IDYxLjEgMzMuNiAxNjAuNSA1My42IDE4Mi44IDE5LjkgMjIuMyAzMTkuMSAxMTMuNyAzMTkuMSAxMTMuN1MzMjYgMzU3LjkgMjcwLjUgMzQxLjljLTU1LjYtMTYtMzcuOSAxNy44LTM3LjkgMTcuOCAxMS40IDYxLjcgNjQuOSAxMzEuOCAxMDcuMiAxMzguNCA0Mi4yIDYuNiAyMjAuMSA0IDIyMC4xIDRzLTM1LjUgNC4xLTkzLjIgMTEuOWMtNDIuNyA1LjgtOTcgMTIuNS0xMTEuMSAxNy44LTMzLjEgMTIuNSAyNCA2Mi42IDI0IDYyLjYgODQuNyA3Ni44IDEyOS43IDUwLjUgMTI5LjcgNTAuNSAzMy4zLTEwLjcgNjEuNC0xOC41IDg1LjItMjQuMkw1NjUgNzQzLjFoODQuNkw2MDMgOTI4bDIwNS4zLTI3MS45SDcwMC44bDIyLjMtMzguN2MuMy41LjQuOC40LjhTNzk5LjggNDk2LjEgODI5IDQzMy44bC42LTFoLS4xYzUtMTAuOCA4LjYtMTkuNyAxMC0yNS44IDE3LTcxLjMtMTE0LjUtOTkuNC0yNjUuOC0xNTQuNXoiLz4NCjwvc3ZnPg0K)](https://h5.dingtalk.com/circle/healthCheckin.html?corpId=dingccf128388c3ea40eda055e4784d35b88&2f46=c9b80ba5&origin=11)

</div>

English | [简体中文](README-zh_CN.md)

## ✨ Features

- 🌈 Enterprise-class UI designed for web applications.
- 📦 A set of high-quality Blazor components out of the box.
- 💕 Supports WebAssembly-based client-side and SignalR-based server-side UI event interaction.
- 🎨 Supports Progressive Web Applications (PWA).
- 🛡 Build with C#, a multi-paradigm static language for an efficient development experience.
- ⚙️ .NET Standard 2.1 based, with direct reference to the rich .NET ecosystem.
- 🎁 Seamless integration with existing ASP.NET Core MVC and Razor Pages projects.

## 🌈 Online Examples

WebAssembly static hosting examples:

- [GitHub](https://ant-design-blazor.github.io/)
- [Gitee](https://ant-design-blazor.gitee.io/)

## 🖥 Environment Support

- .NET Core 3.1
- Blazor WebAssembly 3.2 RC 1
- Supports two-way binding on the server side
- Supports WebAssembly static file deployment
- Support 4 major browsers engines, and Internet Explorer 11+ ([Blazor Server](https://docs.microsoft.com/en-us/aspnet/core/blazor/supported-platforms?view=aspnetcore-3.1) only)
- Run directly on [Electron](http://electron.atom.io/) and other Web standards-based environments

| [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/edge/edge_48x48.png" alt="IE / Edge" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br> Edge / IE | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/firefox/firefox_48x48.png" alt="Firefox" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Firefox | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/chrome/chrome_48x48.png" alt="Chrome" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Chrome | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/safari/safari_48x48.png" alt="Safari" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Safari | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/opera/opera_48x48.png" alt="Opera" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Opera | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/electron/electron_48x48.png" alt="Electron" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Electron |
| :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: |
|                                                                                                 Edge 16 / IE 11†                                                                                                 |                                                                                                        522                                                                                                        |                                                                                                      57                                                                                                       |                                                                                                      11                                                                                                       |                                                                                                    44                                                                                                     |                                                                                                      Chromium 57                                                                                                      |

> Due to [WebAssembly](https://webassembly.org) restriction, Blazor WebAssembly doesn't support IE browser, but Blazor Server supports IE 11† with additional polyfills. See [official documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/supported-platforms?view=aspnetcore-3.1)

## 💿 Current Version

- Development: [![AntBlazor](https://img.shields.io/nuget/v/AntBlazor.svg?color=red&style=flat-square)](https://www.nuget.org/packages/AntBlazor/)

- 0.1.0：will be released after basic implementation of components

## 🎨 Design Specification

Regularly synchronize with Official Ant Design specifications, you can check the [sync logs](https://github.com/ant-design-blazor/ant-design-blazor/actions?query=workflow%3A%22Style+sync+Bot%22) online.

## 📦 Installation Guide

- Install [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1) 3.1.201 or later
- Install Blazor WebAssembly Templates

  ```bash
  $ dotnet new -i Microsoft.AspNetCore.Components.WebAssembly.Templates::3.2.0-rc1.20223.4
  ```

- Create Blazor WebAssembly Project

  ```bash
  $ dotnet new blazorwasm -o MyAntBlazorApp
  ```

- Go to the project folder of the application and install the Nuget package reference

  ```bash
  $ cd MyAntBlazorApp
  $ dotnet add package AntBlazor --version 0.1.0-*
  ```

- Register the services

  ```csharp
  services.AddAntBlazor();
  ```

- Link the static files in `wwwroot/index.html` (WebAssembly) or `Pages/_Host.razor` (Server)

  ```html
  <link href="_content/AntBlazor/css/ant-design-blazor.css" rel="stylesheet" />
  <script src="_content/AntBlazor/js/ant-design-blazor.js"></script>
  ```

- Add namespace in `_Imports.razor`

  ```csharp
  @using AntBlazor
  ```

- Finally, it can be referenced in the `.razor' component!

  ```html
  <AntButton type="primary">Hello World!</AntButton>
  ```

## ⌨️ Local Development

- Install [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1) 3.1.201 or later.
- Install Node.js (only for building style files and interoperable TypeScript files)
- Clone to local development

  ```bash
  $ git clone git@github.com:ant-design-blazor/ant-design-blazor.git
  $ cd ant-design-blazor
  $ npm install
  $ npm start
  ```

- Visit https://localhost:5001 in your supported browser and check [local development documentation](https://github.com/ant-design-blazor/ant-design-blazor/wiki) for details.

  > Visual Studio 2019 is recommended for development. Currently `AntBlazor.Docs.Server` project is required to run for breakpoint debugging.

## 🔗 Links

- [Ant Design Blazor Documentation](https://ant-design-blazor.gitee.io)
- [Official Blazor Documentation](https://blazor.net)

## 🗺 Roadmap

Check out this [issue](https://github.com/ant-design-blazor/ant-design-blazor/issues/21) to learn about our development plans for 2020.

## 🤝 Contributing

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](https://github.com/ant-design-blazor/ant-design-blazor/pulls)

If you would like to contribute, feel free to create a [Pull Request](https://github.com/ant-design-blazor/ant-design-blazor/pulls), or give us [Bug Report](https://github.com/ant-design-blazor/ant-design-blazor/issues/new).

### Contributors

This project exists thanks to all the people who contribute.

<a href="https://github.com/ant-design-blazor/ant-design-blazor/graphs/contributors"><img src="https://opencollective.com/ant-design-blazor/contributors.svg?width=890&button=false"/></a>

## 💕 Donation

This project is an MIT-licensed open source project. In order to achieve better and sustainable development of the project, we expect to gain more backers. We will use the proceeds for community operations and promotion. You can support us in any of the following ways:

- [OpenCollective](https://opencollective.com/ant-design-blazor)
- [Wechat](https://yangshunjie.com/images/qrcode/wepay.jpg)
- [Alipay](https://yangshunjie.com/images/qrcode/alipay.jpg)

We will put the detailed donation records on the [backer list](https://github.com/ant-design-blazor/ant-design-blazor/issues/62).

## ❓ Community Support

If you encounter any problems in the process, feel free to ask for help via following channels. We also encourage experienced users to help newcomers.

- [![Slack Group](https://img.shields.io/badge/Slack-AntBlazor-blue.svg?style=flat-square&logo=slack)](https://join.slack.com/t/antblazor/shared_invite/zt-cw1enker-xVw3s93cTf4uhY2lRGRyRw) (Chinese & English)
- [![Ding Talk Group](https://img.shields.io/badge/DingTalk-AntBlazor-blue.svg?style=flat-square&logo=data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBzdGFuZGFsb25lPSJubyI/Pg0KPHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIGNsYXNzPSJpY29uIiB2aWV3Qm94PSIwIDAgMTAyNCAxMDI0IiBmaWxsPSIjZmZmZmZmIj4NCiAgPHBhdGggZD0iTTU3My43IDI1Mi41QzQyMi41IDE5Ny40IDIwMS4zIDk2LjcgMjAxLjMgOTYuN2MtMTUuNy00LjEtMTcuOSAxMS4xLTE3LjkgMTEuMS01IDYxLjEgMzMuNiAxNjAuNSA1My42IDE4Mi44IDE5LjkgMjIuMyAzMTkuMSAxMTMuNyAzMTkuMSAxMTMuN1MzMjYgMzU3LjkgMjcwLjUgMzQxLjljLTU1LjYtMTYtMzcuOSAxNy44LTM3LjkgMTcuOCAxMS40IDYxLjcgNjQuOSAxMzEuOCAxMDcuMiAxMzguNCA0Mi4yIDYuNiAyMjAuMSA0IDIyMC4xIDRzLTM1LjUgNC4xLTkzLjIgMTEuOWMtNDIuNyA1LjgtOTcgMTIuNS0xMTEuMSAxNy44LTMzLjEgMTIuNSAyNCA2Mi42IDI0IDYyLjYgODQuNyA3Ni44IDEyOS43IDUwLjUgMTI5LjcgNTAuNSAzMy4zLTEwLjcgNjEuNC0xOC41IDg1LjItMjQuMkw1NjUgNzQzLjFoODQuNkw2MDMgOTI4bDIwNS4zLTI3MS45SDcwMC44bDIyLjMtMzguN2MuMy41LjQuOC40LjhTNzk5LjggNDk2LjEgODI5IDQzMy44bC42LTFoLS4xYzUtMTAuOCA4LjYtMTkuNyAxMC0yNS44IDE3LTcxLjMtMTE0LjUtOTkuNC0yNjUuOC0xNTQuNXoiLz4NCjwvc3ZnPg0K)](https://h5.dingtalk.com/circle/healthCheckin.html?corpId=dingccf128388c3ea40eda055e4784d35b88&2f46=c9b80ba5&origin=11) (Chinese)

<details>
  <summary>Scan QR Code with DingTalk</summary>
  <img src="./docs/assets/dingtalk.jpg" width="300">
</details>

## ☀️ License

[![AntBlazor](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/LICENSE)
