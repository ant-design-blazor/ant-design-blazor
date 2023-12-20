<p align="center">
  <a href="https://yangshunjie.com/ant-design-blazor/">
    <img src="https://raw.githubusercontent.com/ant-design-blazor/ant-design-blazor/master/logo.svg?sanitize=true">
  </a>
</p>

<h1 align="center">Ant Design Blazor</h1>

<div align="center">

A rich set of enterprise-class UI components based on Ant Design and Blazor.

![Build](https://img.shields.io/github/actions/workflow/status/ant-design-blazor/ant-design-blazor/nightly-build.yml?style=flat-square)
[![AntDesign](https://img.shields.io/nuget/v/AntDesign.svg?color=red&style=flat-square)](https://www.nuget.org/packages/AntDesign/)
[![AntDesign](https://img.shields.io/nuget/dt/AntDesign.svg?style=flat-square)](https://www.nuget.org/packages/AntDesign/)
[![AntDesign.Templates](https://img.shields.io/nuget/v/AntDesign.Templates?color=%23512bd4&label=Templates&style=flat-square)](https://github.com/ant-design-blazor/ant-design-pro-blazor)
[![codecov](https://img.shields.io/codecov/c/github/ant-design-blazor/ant-design-blazor/master.svg?style=flat-square)](https://codecov.io/gh/ant-design-blazor/ant-design-blazor)
[![AntDesign](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/LICENSE)
[![Ding Talk Group](https://img.shields.io/badge/DingTalk-AntBlazor-blue.svg?style=flat-square&logo=data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBzdGFuZGFsb25lPSJubyI/Pg0KPHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIGNsYXNzPSJpY29uIiB2aWV3Qm94PSIwIDAgMTAyNCAxMDI0IiBmaWxsPSIjZmZmZmZmIj4NCiAgPHBhdGggZD0iTTU3My43IDI1Mi41QzQyMi41IDE5Ny40IDIwMS4zIDk2LjcgMjAxLjMgOTYuN2MtMTUuNy00LjEtMTcuOSAxMS4xLTE3LjkgMTEuMS01IDYxLjEgMzMuNiAxNjAuNSA1My42IDE4Mi44IDE5LjkgMjIuMyAzMTkuMSAxMTMuNyAzMTkuMSAxMTMuN1MzMjYgMzU3LjkgMjcwLjUgMzQxLjljLTU1LjYtMTYtMzcuOSAxNy44LTM3LjkgMTcuOCAxMS40IDYxLjcgNjQuOSAxMzEuOCAxMDcuMiAxMzguNCA0Mi4yIDYuNiAyMjAuMSA0IDIyMC4xIDRzLTM1LjUgNC4xLTkzLjIgMTEuOWMtNDIuNyA1LjgtOTcgMTIuNS0xMTEuMSAxNy44LTMzLjEgMTIuNSAyNCA2Mi42IDI0IDYyLjYgODQuNyA3Ni44IDEyOS43IDUwLjUgMTI5LjcgNTAuNSAzMy4zLTEwLjcgNjEuNC0xOC41IDg1LjItMjQuMkw1NjUgNzQzLjFoODQuNkw2MDMgOTI4bDIwNS4zLTI3MS45SDcwMC44bDIyLjMtMzguN2MuMy41LjQuOC40LjhTNzk5LjggNDk2LjEgODI5IDQzMy44bC42LTFoLS4xYzUtMTAuOCA4LjYtMTkuNyAxMC0yNS44IDE3LTcxLjMtMTE0LjUtOTkuNC0yNjUuOC0xNTQuNXoiLz4NCjwvc3ZnPg0K)](https://h5.dingtalk.com/circle/healthCheckin.html?corpId=dingf3df1949a4aa48627b0128d9a44ecb79&c5df5865-4f41-=be1b34c7-397b-&cbdbhh=qwertyuiop&origin=11)
[![Discord Server](https://img.shields.io/discord/753358910341251182?color=%237289DA&label=AntBlazor&logo=discord&logoColor=white&style=flat-square)](https://discord.com/invite/jqu3Xeq)

</div>

[![](https://gw.alipayobjects.com/mdn/rms_08e378/afts/img/A*Yl83RJhUE7kAAAAAAAAAAABkARQnAQ)](https://ant-design-blazor.github.io)

English | [简体中文](README-zh_CN.md)

## ✨ Features

- 🌈 Enterprise-class UI designed for web applications.
- 📦 A set of high-quality Blazor components out of the box.
- 💕 Supports WebAssembly-based client-side and SignalR-based server-side UI event interaction.
- 🎨 Supports Progressive Web Applications (PWA).
- 🛡 Build with C#, a multi-paradigm static language for an efficient development experience.
- 🌍 Internationalization support for dozens of languages.
- 🎁 Seamless integration with existing ASP.NET Core MVC and Razor Pages projects.

## 🌈 Online Examples

WebAssembly static hosting on:

- [Document site](https://antblazor.com/)
- [Enterprise system dashboard](https://pro.antblazor.com/)

## 🖥 Environment Support

- Supports .NET Core 3.1 / .NET 5 / .NET 6 / .Net 7 / .NET 8.
- Supports WebAssembly static file deployment.
- Supports 4 major browsers engines, and Internet Explorer 11+ ([Blazor Server](https://docs.microsoft.com/en-us/aspnet/core/blazor/supported-platforms?view=aspnetcore-6.0&WT.mc_id=DT-MVP-5003987) only)
- Supports [.NET MAUI](https://dotnet.microsoft.com/zh-cn/apps/maui?WT.mc_id=DT-MVP-5003987) / [WPF](https://docs.microsoft.com/en-us/aspnet/core/blazor/hybrid/tutorials/wpf?view=aspnetcore-6.0&WT.mc_id=DT-MVP-5003987) / [Windows Forms](https://docs.microsoft.com/en-us/aspnet/core/blazor/hybrid/tutorials/windows-forms?view=aspnetcore-6.0) and other Blazor Hybrid workloads.
- Supports [Electron](http://electron.atom.io/) and other Web standards-based environments.

> Due to [WebAssembly](https://webassembly.org) restriction, Blazor WebAssembly doesn't support IE browser, but Blazor Server supports IE 11† with additional polyfills. See [official documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/supported-platforms?view=aspnetcore-3.1&WT.mc_id=DT-MVP-5003987).

> From .NET 5, IE 11 is no longer officially supported. See [Blazor: Updated browser support](https://docs.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/5.0/blazor-browser-support-updated). Unofficial support is provided by [Blazor.Polyfill](https://github.com/Daddoon/Blazor.Polyfill) community project.

## 💿 Current Version

- Release: [![AntDesign](https://img.shields.io/nuget/v/AntDesign.svg?color=red&style=flat-square)](https://www.nuget.org/packages/AntDesign/)
- Nightly: [![AntDesign](https://img.shields.io/myget/ant-design-blazor/vpre/AntDesign?style=flat-square)](https://www.myget.org/feed/ant-design-blazor/package/nuget/AntDesign)

  _[Download our latest nightly builds](docs/nightly-build.en-US.md)_

## 🎨 Design Specification

Regularly synchronize with Official Ant Design specifications, you can check the [sync logs](https://github.com/ant-design-blazor/ant-design-blazor/actions?query=workflow%3A%22Style+sync+Bot%22) online.

Therefore, you can use the custom theme styles of Ant Design directly.

**Before the 1.0 release, we will only sync antd 4.x styles.**

## 📦 Installation Guide

### Prerequirement

- Install [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1?WT.mc_id=DT-MVP-5003987) 3.1.300 or later, .NET 8 is even better.

### Option 1: Create a new project from the dotnet new template [![AntDesign.Templates](https://img.shields.io/nuget/v/AntDesign.Templates?color=%23512bd4&label=Templates&style=flat-square)](https://github.com/ant-design-blazor/ant-design-pro-blazor)

We have provided the `dotnet new` template to create a [Boilerplate](https://github.com/ant-design-blazor/ant-design-pro-blazor) project out of the box：

![Pro Template](https://user-images.githubusercontent.com/8186664/44953195-581e3d80-aec4-11e8-8dcb-54b9db38ec11.png)

- Install the template

  ```bash
  $ dotnet new --install AntDesign.Templates
  ```

- Create the Boilerplate project with the template

  ```bash
  $ dotnet new antdesign -o MyAntDesignApp
  ```

Options for the template：

| Options           | Description                                                        | Type                           | Default |
| ----------------- | ------------------------------------------------------------------ | ------------------------------ | ------- |
| `-f` \| `--full`  | If specified, generates all pages of Ant Design Pro                | bool                           | false   |
| `-ho` \| `--host` | Specify the hosting model                                          | 'wasm' \| 'server' \| 'hosted' | 'wasm'  |
| `--styles`        | Whether use NodeJS and Less to compile your custom themes.         | `css` \| `less`                | `css`   |
| `--no-restore`    | If specified, skips the automatic restore of the project on create | bool                           | false   |

### Option 2: Import Ant Design Blazor into an existing project

- Go to the project folder of the application and install the Nuget package reference

  ```bash
  $ dotnet add package AntDesign
  ```

- Register the services in `Program.cs`

  ```csharp
  builder.Services.AddAntDesign();
  ```

  or `Startup.cs`

  ```csharp
  services.AddAntDesign();
  ```

- Add namespace in `_Imports.razor`

  ```csharp
  @using AntDesign
  ```

- To display the pop-up component dynamically, you need to add the `<AntContainer />` component in `App.razor`.

  - For Blazor WebApp, you also need to specify render mode  to `<Routes />` for interactivity.

  ```diff
  <Routes @rendermode="RenderMode.InteractiveAuto" />            <-- specify the rendermode ✨
  + <AntContainer @rendermode="RenderMode.InteractiveAuto" />    <-- add this component ✨
  ```
 
  - For legacy blazor apps just add a line of code:

  ```diff
  <Router AppAssembly="@typeof(MainLayout).Assembly">
      <Found Context="routeData">
          <RouteView RouteData="routeData" DefaultLayout="@typeof(MainLayout)" />
      </Found>
      <NotFound>
          <LayoutView Layout="@typeof(MainLayout)">
              <Result Status="404" />
          </LayoutView>
      </NotFound>
  </Router>

  +  <AntContainer />   <-- add this component ✨
  ```

- Finally, it can be referenced in the `.razor` component!

  ```razor
  <Button Type="@ButtonType.Primary">Hello World!</Button>
  ```

## 🔨 Development

### Gitpod

Click the button below to start a new workspace for development for free.

[![Open in Gitpod](https://gitpod.io/button/open-in-gitpod.svg)](https://gitpod.io/#https://github.com/ant-design-blazor/ant-design-blazor)

### Local

- Install [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet/8.0?WT.mc_id=DT-MVP-5003987) 8.0.100 or later.
- Install Node.js (only for building style files and interoperable TypeScript files)
- Clone to local development

  ```bash
  $ git clone https://github.com/ant-design-blazor/ant-design-blazor.git
  $ cd ant-design-blazor
  $ npm install
  $ dotnet build ./site/AntDesign.Docs.Build/AntDesign.Docs.Build.csproj
  $ npm start
  ```

- Visit https://localhost:5001 in your supported browser and check [local development documentation](https://github.com/ant-design-blazor/ant-design-blazor/wiki) for details.

  > Visual Studio 2022 is recommended for development.

## 🔗 Links

- [Ant Design Blazor Documentation](https://antblazor.com)
- [Official Blazor Documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/?WT.mc_id=DT-MVP-5003987)
- [MS Learn for Blazor Tutorial](https://docs.microsoft.com/en-us/learn/modules/build-blazor-webassembly-visual-studio-code/?WT.mc_id=DT-MVP-5003987)

## 🗺 Roadmap

Check out this [issue](https://github.com/ant-design-blazor/ant-design-blazor/issues/21) to learn about our development plans for the 1.0 release.

You can also find the [latest news](https://github.com/ant-design-blazor/ant-design-blazor/issues/2870) about the features we will implement in the future with antd5.0 style.

## 🤝 Contributing

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](https://github.com/ant-design-blazor/ant-design-blazor/pulls)

If you would like to contribute, feel free to create a [Pull Request](https://github.com/ant-design-blazor/ant-design-blazor/pulls), or give us [Bug Report](https://github.com/ant-design-blazor/ant-design-blazor/issues/new).

## 💕 Donation

This project is an MIT-licensed open source project. In order to achieve better and sustainable development of the project, we expect to gain more backers. We will use the proceeds for community operations and promotion. You can support us in any of the following ways:

- [OpenCollective](https://opencollective.com/ant-design-blazor)
- [Wechat](https://jamesyeung.cn/qrcode/wepay.jpg)
- [Alipay](https://jamesyeung.cn/qrcode/alipay.jpg)

We will put the detailed donation records on the [backer list](https://github.com/ant-design-blazor/ant-design-blazor/issues/62).

## ❓ Community Support

If you encounter any problems in the process, feel free to ask for help via following channels. We also encourage experienced users to help newcomers.

- [![Discord Server](https://img.shields.io/discord/753358910341251182?color=%237289DA&label=AntBlazor&logo=discord&logoColor=white&style=flat-square)](https://discord.com/invite/jqu3Xeq)

- [![钉钉群](https://img.shields.io/badge/钉钉-AntBlazor-blue.svg?style=flat-square&logo=data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBzdGFuZGFsb25lPSJubyI/Pg0KPHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIGNsYXNzPSJpY29uIiB2aWV3Qm94PSIwIDAgMTAyNCAxMDI0IiBmaWxsPSIjZmZmZmZmIj4NCiAgPHBhdGggZD0iTTU3My43IDI1Mi41QzQyMi41IDE5Ny40IDIwMS4zIDk2LjcgMjAxLjMgOTYuN2MtMTUuNy00LjEtMTcuOSAxMS4xLTE3LjkgMTEuMS01IDYxLjEgMzMuNiAxNjAuNSA1My42IDE4Mi44IDE5LjkgMjIuMyAzMTkuMSAxMTMuNyAzMTkuMSAxMTMuN1MzMjYgMzU3LjkgMjcwLjUgMzQxLjljLTU1LjYtMTYtMzcuOSAxNy44LTM3LjkgMTcuOCAxMS40IDYxLjcgNjQuOSAxMzEuOCAxMDcuMiAxMzguNCA0Mi4yIDYuNiAyMjAuMSA0IDIyMC4xIDRzLTM1LjUgNC4xLTkzLjIgMTEuOWMtNDIuNyA1LjgtOTcgMTIuNS0xMTEuMSAxNy44LTMzLjEgMTIuNSAyNCA2Mi42IDI0IDYyLjYgODQuNyA3Ni44IDEyOS43IDUwLjUgMTI5LjcgNTAuNSAzMy4zLTEwLjcgNjEuNC0xOC41IDg1LjItMjQuMkw1NjUgNzQzLjFoODQuNkw2MDMgOTI4bDIwNS4zLTI3MS45SDcwMC44bDIyLjMtMzguN2MuMy41LjQuOC40LjhTNzk5LjggNDk2LjEgODI5IDQzMy44bC42LTFoLS4xYzUtMTAuOCA4LjYtMTkuNyAxMC0yNS44IDE3LTcxLjMtMTE0LjUtOTkuNC0yNjUuOC0xNTQuNXoiLz4NCjwvc3ZnPg0K)](https://h5.dingtalk.com/circle/healthCheckin.html?corpId=dingf3df1949a4aa48627b0128d9a44ecb79&c5df5865-4f41-=be1b34c7-397b-&cbdbhh=qwertyuiop&origin=11)

  <details>
    <summary>Scan QR Code with DingTalk</summary>
    <img src="https://raw.githubusercontent.com/ant-design-blazor/ant-design-blazor/master/docs/assets/dingtalk.jpg" width="300">
  </details>

## Contributors

This project exists thanks to all the people who contribute.

<a href="https://github.com/ant-design-blazor/ant-design-blazor/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=ant-design-blazor/ant-design-blazor&max=1000&columns=15&anon=1" />
</a>


## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## ☀️ License

[![AntDesign](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/LICENSE)

## .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).
