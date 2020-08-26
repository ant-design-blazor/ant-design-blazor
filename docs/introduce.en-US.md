---
order: 0
title: Ant Design of Blazor
---

Following the Ant Design specification, we developed a Blazor Components library `ant-design-blazor` that contains a set of high quality components and demos for building rich, interactive user interfaces.

<div class="pic-plus">
  <img width="150" src="https://gw.alipayobjects.com/zos/rmsportal/KDpgvguMpGfqaHPjicRK.svg">
  <span>+</span>
  <img height="150" src="/docs/assets/blazor.svg">
</div>

<style>
.pic-plus > * {
  display: inline-block !important;
  vertical-align: middle;
}
.pic-plus span {
  font-size: 30px;
  color: #aaa;
  margin: 0 20px;
}
</style>

## ✨ Features

- 🌈 Enterprise-class UI interactive language and visual style.
- 📦 Out-of-the-box, high-quality Razor components that can be shared in a variety of hosting models.
- 💕 Supports WebAssembly-based client-side and SignalR-based server-side UI event interaction.
- 🎨 Supports Progressive Web Applications (PWA).
- 🛡 Build with C#, a multi-paradigm static language for an efficient development experience.
- ⚙️ .NET Standard 2.1 based, with direct reference to the rich .NET ecosystem.
- 🎁 Seamless integration with existing ASP.NET Core MVC and Razor Pages projects.

## 🌈 Online Examples

WebAssembly static hosting examples:

- [Gitee](https://ant-design-blazor.gitee.io/)
- [GitHub](https://ant-design-blazor.github.io/)

## 🖥 Environment Support

- .NET Core 3.1
- Blazor WebAssembly 3.2 Release
- Supports two-way binding on the server side
- Supports WebAssembly static file deployment
- Support 4 major browsers engines, and Internet Explorer 11+ ([Blazor Server](https://docs.microsoft.com/en-us/aspnet/core/blazor/supported-platforms?view=aspnetcore-3.1) only)
- Run directly on [Electron](http://electron.atom.io/) and other Web standards-based environments

| [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/edge/edge_48x48.png" alt="IE / Edge" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br> Edge / IE | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/firefox/firefox_48x48.png" alt="Firefox" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Firefox | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/chrome/chrome_48x48.png" alt="Chrome" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Chrome | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/safari/safari_48x48.png" alt="Safari" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Safari | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/opera/opera_48x48.png" alt="Opera" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Opera | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/electron/electron_48x48.png" alt="Electron" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Electron |
| :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: |
|                                                                                                 Edge 16 / IE 11†                                                                                                 |                                                                                                        522                                                                                                        |                                                                                                      57                                                                                                       |                                                                                                      11                                                                                                       |                                                                                                    44                                                                                                     |                                                                                                      Chromium 57                                                                                                      |

> Due to [WebAssembly](https://webassembly.org) restriction, Blazor WebAssembly doesn't support IE browser, but Blazor Server supports IE 11† with additional polyfills. See [official documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/supported-platforms?view=aspnetcore-3.1)

## 💿 Current Version

- Release: [![AntDesign](https://img.shields.io/nuget/v/AntDesign.svg?color=red&style=flat-square)](https://www.nuget.org/packages/AntDesign/)
- Development: [![AntDesign](https://img.shields.io/nuget/vpre/AntDesign.svg?color=red&style=flat-square)](https://www.nuget.org/packages/AntDesign/)

## 🎨 Design Specification

Regularly synchronize with Official Ant Design specifications, you can check the [sync logs](https://github.com/ant-design-blazor/ant-design-blazor/actions?query=workflow%3A%22Style+sync+Bot%22) online.

## 📦 Installation Guide

- Install [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1) 3.1.300 or later

### Create a new project from the dotnet new template

We have provided the `dotnet new` template to create a [Boilerplate](https://github.com/ant-design-blazor/ant-design-pro-blazor) project out of the box：

- Install the template

  ```bash
  $ dotnet new --install AntDesign.Templates::0.1.0-*
  ```

- Create the Boilerplate project with the template

  ```bash
  $ dotnet new antdesign -o MyAntDesignApp
  ```

Options for the template：

| Options          | Description                                         | Type     | Default    |
| ---------------- | -------------------------------------------- | ------ |  --------- |
| `-f` \| `--full`  | If specified, generates all pages of ant design pro | bool  |  false    |
| `-ho` \| `--host`   | Specify the hosting model  | 'wasm' \| 'server' \| 'hosted'    |'wasm'      |
| `--no-restore` |  If specified, skips the automatic restore of the project on create  | bool    | false |



### Import Ant Design Blazor into an existing project

- Go to the project folder of the application and install the Nuget package reference

  ```bash
  $ dotnet add package AntDesign --version 0.1.0-*
  ```

- Register the services

  ```csharp
  services.AddAntDesign();
  ```

- Link the static files in `wwwroot/index.html` (WebAssembly) or `Pages/_Host.cshtml` (Server)

  ```html
  <link href="_content/AntDesign/css/ant-design-blazor.css" rel="stylesheet" />
  <script src="_content/AntDesign/js/ant-design-blazor.js"></script>
  ```

- Add namespace in `_Imports.razor`

  ```csharp
  @using AntDesign
  ```

- To display the pop-up component dynamically, you need to add the `<AntContainer />` component in `App.razor`. 

  ```
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

  <AntContainer />   <-- add this component ✨
  ```

- Finally, it can be referenced in the `.razor' component!

  ```html
  <Button Type="primary">Hello World!</Button>
  ```

## 🔨 Local Development

- Install [.NET Core SDK](https://dotnet.microsoft.com/download) 3.1.300 or later.
- Install Node.js (only for building style files and interoperable TypeScript files)
- Clone to local development

  ```bash
  $ git clone git@github.com:ant-design-blazor/ant-design-blazor.git
  $ cd ant-design-blazor
  $ npm install
  $ npm start
  ```

- Visit https://localhost:5001 in your supported browser and check [local development documentation](https://github.com/ant-design-blazor/ant-design-blazor/wiki) for details.

  > Visual Studio 2019 is recommended for development.

## 🔗 Links

- [Ant Design Blazor Documentation](https://ant-design-blazor.gitee.io)
- [Official Blazor Documentation](https://blazor.net)

## 🗺 Roadmap

Check out this [issue](https://github.com/ant-design-blazor/ant-design-blazor/issues/21) to learn about our development plans for 2020.

## 🤝 Contributing

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](https://github.com/ant-design-blazor/ant-design-blazor/pulls)

If you would like to contribute, feel free to create a [Pull Request](https://github.com/ant-design-blazor/ant-design-blazor/pulls), or give us [Bug Report](https://github.com/ant-design-blazor/ant-design-blazor/issues/new).

## ❓ Community Support

If you encounter any problems in the process, feel free to ask for help via following channels. We also encourage experienced users to help newcomers.

- [![Slack Group](https://img.shields.io/badge/Slack-AntBlazor-blue.svg?style=flat-square&logo=slack)](https://join.slack.com/t/AntBlazor/shared_invite/zt-etfaf1ww-AEHRU41B5YYKij7SlHqajA) (Chinese & English)
- [![Ding Talk Group](https://img.shields.io/badge/DingTalk-AntBlazor-blue.svg?style=flat-square)](https://h5.dingtalk.com/circle/healthCheckin.html?corpId=dingccf128388c3ea40eda055e4784d35b88&2f46=c9b80ba5&origin=11) (Chinese)

<details>
  <summary>Scan QR Code with DingTalk</summary>
  <img src="./docs/assets/dingtalk.jpg" width="300">
</details>

## ☀️ License

[![AntDesign](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/LICENSE)
