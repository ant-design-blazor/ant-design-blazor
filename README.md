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

## Characteristic

- Distilled from the interactive language and visual style of back-end products in the enterprise.
- Out-of-the-box, high-quality Razor components that can be shared in a variety of hosted ways.
- Supports WebAssembly-based client-side and SignalR-based server-side UI event interaction.
- Support for Progressive Web Applications (PWA)
- Build in C#, a multi-paradigm static language for an efficient development experience.
- NET Standard 2.1 based, with direct reference to the rich ecosystem of .NET
- Seamless integration with existing ASP.NET Core MVC, Razor Pages projects.

## 🌈 Online examples

由 WebAssembly build, hosted on Gitee Pages http://ant-design-blazor.gitee.io/

## 🖥 Support

- .NET Core 3.1
- Blazor WebAssembly 3.2 Preview 2
- Supports two-way binding on the server side
- Support for WebAssembly static file deployment
- Mainstream 4 modern browsers, and Internet Explorer 11+ （usage [Blazor Server](https://angular.io/guide/browser-support)）
- Can be run directly on [Electron](http://electron.atom.io/) Web standards-based environments such as

| [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/edge/edge_48x48.png" alt="IE / Edge" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br> Edge / IE | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/firefox/firefox_48x48.png" alt="Firefox" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Firefox | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/chrome/chrome_48x48.png" alt="Chrome" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Chrome | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/safari/safari_48x48.png" alt="Safari" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Safari | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/opera/opera_48x48.png" alt="Opera" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Opera | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/electron/electron_48x48.png" alt="Electron" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Electron |
| :---------: | :---------: | :---------: | :---------: | :---------: | :---------: |
| Edge 16 / IE 11† | 522 | 57 | 11 | 44 | Chromium 57

> Due to [WebAssembly](https://webassembly.org) Blazor WebAssembly does not support the IE browser, but Blazor Server supports IE 11† for this reason. [Official website description](https://docs.microsoft.com/en-us/aspnet/core/blazor/supported-platforms?view=aspnetcore-3.1) 

## 💿 Current Version

- Development and construction： [![AntBlazor](https://img.shields.io/nuget/v/AntBlazor.svg?color=red&style=flat-square)](https://www.nuget.org/packages/AntBlazor/)

- 0.1.0：Basic implementation of components after release

## 🎨 Design Specification

与 Regularly synced with the Ant Design specification, you can view the [sync log online](https://github.com/ElderJames/ant-design-blazor/actions?query=workflow%3A%22Style+sync+Bot%22).

## 📦 Installation Guide

- Install [.NET Core SDK](https://dotnet.microsoft.com/download) 3.1.102 or above
- Install Blazor WebAssembly Templates
  ```
  $ dotnet new -i Microsoft.AspNetCore.Components.WebAssembly.Templates::3.2.0-preview2.20160.5
  ```   
- Create Blazor WebAssembly Project
  ```
  $ dotnet new blazorwasm -o MyAntBlazorApp
  ```
- Go to the project folder of the application and install the Nuget package reference
  ```bash
  $ cd MyAntBlazorApp
  $ dotnet add package AntBlazor --version 0.0.1-nightly-55111624
  ```
- Register the services:
  ```
  services.AddAntBlazor();
  ```
- In `wwwroot/index.html`(WebAssembly) or `Pages/_Host.razor`(Server) link the static files:
  ```
    <link href="_content/AntBlazor/css/ant-design-blazor.css" rel="stylesheet">
    <script src="_content/AntBlazor/js/ant-design-blazor.js"></script>
  ```
- In `_Imports.razor` add the namespace
  ```
  @using AntBlazor
  ```
- Finally, it can be referenced in the `.razor' component!
  ```
  <AntButton type="primary">Hello World!</AntButton>
  ```

## 🔨 Local development

- Install [.NET Core SDK](https://dotnet.microsoft.com/download) 3.1.102 or later.
- Install Node.js (only for style file and interoperable TS file builds)
- Cloning to local development
  ```
  $ git clone git@github.com:ElderJames/ant-design-blazor.git
  $ cd ant-design-blazor
  $ npm install
  $ npm start
  ```
  Open your browser to https://localhost:5001 and refer to [local development documentation] (https://github.com/ElderJames/ant-design-blazor/wiki) for details.
  
  > Visual Studio 2019 is recommended for development and currently requires the `AntBlazor.Docs.ClientApp` project to run for breakpoint debugging.

## 🔗 Links

- [Ant Design Blazor Documentation](https://ant-design-blazor.gitee.io)
- [Official Blazor Documentation](https://blazor.net)

## 🗺 Roadmap

Check out [this issue](https://github.com/ElderJames/ant-design-blazor/issues/21) to learn about our development plans for 2020.

## 🤝 How to contribute

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](https://github.com/ElderJames/ant-design-blazor/pulls)


If you would like to contribute, feel free to create a [Pull Request](https://github.com/ElderJames/ant-design-blazor/pulls), or give us [Report Bug](https://github.com/ElderJames/ant-design-blazor/issues/new).

## ❓ Community support

If you encounter any problems in the process, you can ask for help at [Nailing Group](https://h5.dingtalk.com/circle/healthCheckin.html?corpId=dingccf128388c3ea40eda055e4784d35b88&2f46=c9b80ba5&origin=11), and we also encourage experienced users to help newcomers through the following channels

<img src="./docs/assets/dingtalk.jpg" width="300">

## ☀️ License agreement

[![AntBlazor](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](https://github.com/ElderJames/ant-design-blazor/blob/master/LICENSE)