---
order: 0
title: Ant Design of Blazor
---

这里是 Ant Design 的 Blazor 实现，开发和服务于企业级后台产品。

<div class="pic-plus">
  <img width="150" src="https://gw.alipayobjects.com/zos/rmsportal/KDpgvguMpGfqaHPjicRK.svg">
  <span>+</span>
  <img height="150" src="https://raw.githubusercontent.com/ant-design-blazor/ant-design-blazor/master/docs/assets/blazor.svg">
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

## ✨ 特性

- 🌈 提炼自企业级中后台产品的交互语言和视觉风格。
- 📦 开箱即用的高质量 Blazor 组件，可在多种托管方式共享。
- 💕 支持基于 WebAssembly 的前端和基于 SignalR 的服务端 UI 即时交互。
- 🎨 支持渐进式 Web 应用（PWA）
- 🛡 使用 C# 构建，多范式静态语言带来高效的开发体验。
- ⚙️ 支持 .NET Core 3.1 以上所有版本，可直接引用丰富的 .NET 类库。
- 🌍 数十个国际化语言支持。
- 💴 基于 MIT 开源协议，.NET 基金会项目，商业应用免费。

## 🌈 在线示例

由 WebAssembly 构建，托管在 Azure Static WebApp https://antblazor.com/

## 🖥 支持环境

- 兼容 .NET Core 3.1 / .NET 5 / .NET 6 / .NET 7 / .NET 8。
- 支持 WebAssembly 静态文件部署。
- 支持服务端双向绑定。
- 支持 WebAssembly 静态文件部署。
- 主流 4 款现代浏览器，以及 Internet Explorer 11+ （使用 [Blazor Server](https://docs.microsoft.com/zh-cn/aspnet/core/blazor/supported-platforms?view=aspnetcore-6.0&WT.mc_id=DT-MVP-5003987)）。
- 可直接运行在 [.NET MAUI](https://dotnet.microsoft.com/zh-cn/apps/maui?WT.mc_id=DT-MVP-5003987)、[WPF](https://docs.microsoft.com/en-us/aspnet/core/blazor/hybrid/tutorials/wpf?view=aspnetcore-6.0&WT.mc_id=DT-MVP-5003987)、[Windows Forms](https://docs.microsoft.com/en-us/aspnet/core/blazor/hybrid/tutorials/windows-forms?view=aspnetcore-6.0) 等 Blazor 混合客户端环境中。
- 可直接运行在 [Electron](http://electron.atom.io/) 等基于 Web 标准的环境上

> 由于 [WebAssembly](https://webassembly.org) 的缘故，Blazor WebAssembly 不支持 IE 浏览器，但 Blazor Server 支持 IE 11†。 详见[官网说明](https://docs.microsoft.com/en-us/aspnet/core/blazor/supported-platforms?view=aspnetcore-3.1&WT.mc_id=DT-MVP-5003987)。

> 从 .NET 5 开始，Blazor 不再官方支持 IE 11。详见 [Blazor: Updated browser support](https://docs.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/5.0/blazor-browser-support-updated)。社区项目 [Blazor.Polyfill](https://github.com/Daddoon/Blazor.Polyfill) 提供了非官方支持。

## 💿 当前版本

- 正式发布: [![AntDesign](https://img.shields.io/nuget/v/AntDesign.svg?color=red&style=flat-square)](https://www.nuget.org/packages/AntDesign/)
- 开发构建： [![AntDesign](https://img.shields.io/nuget/vpre/AntDesign.svg?color=red&style=flat-square)](https://www.nuget.org/packages/AntDesign/)

## 🎨 设计规范

与 Ant Design 设计规范定期同步，你可以在线查看[同步日志](https://github.com/ant-design-blazor/ant-design-blazor/actions?query=workflow%3A%22Style+sync+Bot%22)。

**在 1.0 版本发布之前，我们将只同步 antd 4.x 的样式。**

## 📦 安装

### 先决条件

- 先安装 [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1?WT.mc_id=DT-MVP-5003987) 3.1 以上版本，推荐 .NET 8。

### 选择一：从模板创建一个新项目

我们提供了 `dotnet new` 模板来创建一个开箱即用的 [Ant Design Pro](https://github.com/ant-design-blazor/ant-design-pro-blazor) 新项目：

- 安装模板

  ```bash
  $ dotnet new --install AntDesign.Templates
  ```

- 从模板创建 Ant Design Blazor Pro 项目

  ```bash
  $ dotnet new antdesign -o MyAntDesignApp
  ```

模板的参数：

| 参数              | 说明                                             | 类型                           | 认 值  |
| ----------------- | ------------------------------------------------ | ------------------------------ | ------ |
| `-f` \| `--full`  | 如果设置这个参数，会生成所有 Ant Design Pro 页面 | bool                           | false  |
| `-ho` \| `--host` | 指定托管模型                                     | 'wasm' \| 'server' \| 'hosted' | 'wasm' |
| `--styles`        | 指定样式构建类型                                 | `css` \| `less`                | `css`  |
| `--no-restore`    | 如果设置这个参数，就不会自动恢复包引用           | bool                           | false  |

### 选择二：在已有项目中引入 Ant Design Blazor

- 进入应用的项目文件夹，安装 Nuget 包引用

  ```bash
  $ dotnet add package AntDesign
  ```

- 在项目的 `Program.cs` 中注册相关服务：

  ```csharp
  builder.Services.AddAntDesign();
  ```

  或者在 `Startup.cs` 中：

  ```csharp
  services.AddAntDesign();
  ```

- 在 `_Imports.razor` 中加入命名空间

  ```
  @using AntDesign
  ```

- 为了动态地显示弹出组件，需要在 `App.razor` 中添加一个 `<AntContainer />` 组件。

  - 对于 Blazor WebApp 项目，还需要为 `Routes` 指定渲染模式来支持交互性。

  ```diff
  <Routes @rendermode="RenderMode.InteractiveAuto" />           <-- 指定渲染模式 ✨
  + <AntContainer @rendermode="RenderMode.InteractiveAuto" />   <-- 在这里添加容器 ✨
  ```

  - 对于旧版本的项目，则只需加一行代码:

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

  <AntContainer />   <-- 在这里添加 ✨
  ```

- 最后就可以在`.razor`组件中引用啦！

  ```
  <Button Type="primary">Hello World!</Button>
  ```

## 🔨 开发


### Gitpod

点击下方按钮，免费创建一个新的工作区用于开发。

[![Open in Gitpod](https://gitpod.io/button/open-in-gitpod.svg)](https://gitpod.io/#https://github.com/ant-design-blazor/ant-design-blazor)

### 本地

- 先安装 [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet/8.0?WT.mc_id=DT-MVP-5003987) 8.0.100 以上版本
- 安装 Node.js（只用于样式文件和互操作所需 TS 文件的构建）
- 克隆到本地开发

  ```
  $ git clone git@github.com:ant-design-blazor/ant-design-blazor.git
  $ cd ant-design-blazor
  $ npm install
  $ dotnet build ./site/AntDesign.Docs.Build/AntDesign.Docs.Build.csproj
  $ npm start
  ```

  打开浏览器访问 https://localhost:5001 ，详情参考[本地开发文档](https://github.com/ant-design-blazor/ant-design-blazor/wiki)。

  > 推荐使用 Visual Studio 2022 开发。

## 🔗 链接

- [Blazor 官方文档](https://docs.microsoft.com/zh-cn/aspnet/core/blazor/?WT.mc_id=DT-MVP-5003987)
- [MS Learn 平台 Blazor 教程](https://docs.microsoft.com/zh-cn/learn/modules/build-blazor-webassembly-visual-studio-code/?WT.mc_id=DT-MVP-5003987)

## 🤝 如何贡献

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](https://github.com/ant-design-blazor/ant-design-blazor/pulls)

如果你希望参与贡献，欢迎 [Pull Request](https://github.com/ant-design-blazor/ant-design-blazor/pulls)，或给我们 [报告 Bug](https://github.com/ant-design-blazor/ant-design-blazor/issues/new) 。

## ❓ 社区互助

如果您在使用的过程中碰到问题，可以通过以下途径寻求帮助，同时我们也鼓励资深用户通过下面的途径给新人提供帮助。

- [![钉钉群](https://img.shields.io/badge/钉钉-AntBlazor-blue.svg?style=flat-square&logo=data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBzdGFuZGFsb25lPSJubyI/Pg0KPHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIGNsYXNzPSJpY29uIiB2aWV3Qm94PSIwIDAgMTAyNCAxMDI0IiBmaWxsPSIjZmZmZmZmIj4NCiAgPHBhdGggZD0iTTU3My43IDI1Mi41QzQyMi41IDE5Ny40IDIwMS4zIDk2LjcgMjAxLjMgOTYuN2MtMTUuNy00LjEtMTcuOSAxMS4xLTE3LjkgMTEuMS01IDYxLjEgMzMuNiAxNjAuNSA1My42IDE4Mi44IDE5LjkgMjIuMyAzMTkuMSAxMTMuNyAzMTkuMSAxMTMuN1MzMjYgMzU3LjkgMjcwLjUgMzQxLjljLTU1LjYtMTYtMzcuOSAxNy44LTM3LjkgMTcuOCAxMS40IDYxLjcgNjQuOSAxMzEuOCAxMDcuMiAxMzguNCA0Mi4yIDYuNiAyMjAuMSA0IDIyMC4xIDRzLTM1LjUgNC4xLTkzLjIgMTEuOWMtNDIuNyA1LjgtOTcgMTIuNS0xMTEuMSAxNy44LTMzLjEgMTIuNSAyNCA2Mi42IDI0IDYyLjYgODQuNyA3Ni44IDEyOS43IDUwLjUgMTI5LjcgNTAuNSAzMy4zLTEwLjcgNjEuNC0xOC41IDg1LjItMjQuMkw1NjUgNzQzLjFoODQuNkw2MDMgOTI4bDIwNS4zLTI3MS45SDcwMC44bDIyLjMtMzguN2MuMy41LjQuOC40LjhTNzk5LjggNDk2LjEgODI5IDQzMy44bC42LTFoLS4xYzUtMTAuOCA4LjYtMTkuNyAxMC0yNS44IDE3LTcxLjMtMTE0LjUtOTkuNC0yNjUuOC0xNTQuNXoiLz4NCjwvc3ZnPg0K)](https://h5.dingtalk.com/circle/joinCircle.html?corpId=ding82cf43d1ccc223faffe93478753d9884&token=872d544429893379155dfd32580ca02a&groupCode=v1,k1,OgitB0lpZuBX9trsVcblH+V+oKhafALwKQ6AmgviopA=&from=group&ext=%7B%22channel%22%3A%22QR_GROUP_NORMAL%22%2C%22extension%22%3A%7B%22groupCode%22%3A%22v1%2Ck1%2COgitB0lpZuBX9trsVcblH%2BV%2BoKhafALwKQ6AmgviopA%3D%22%2C%22groupFrom%22%3A%22group%22%7D%2C%22inviteId%22%3A137894871%2C%22orgId%22%3A163097163%2C%22shareType%22%3A%22GROUP%22%7D&origin=11) (中文)

  <img src="https://raw.githubusercontent.com/ant-design-blazor/ant-design-blazor/master/docs/assets/dingtalk.jpg" width="300">

- 另外，我还创立了面向中文开发者的 Blazor 中文社区，高手如云，只讨论技术，无卖课广告。可以加我微信（JamesYeungMVP）拉进微信群，另外也有一个 QQ 群 1012762441。广告勿扰。


## 行为准则

本项目采用了《贡献者公约》所定义的行为准则，以明确我们社区的预期行为。
更多信息请见 [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).


## ☀️ 授权协议

[![AntDesign](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/LICENSE)
