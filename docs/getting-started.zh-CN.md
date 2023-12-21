---
order: 1
title: 快速上手
---

Ant Design of Blazor 致力于提供给程序员**愉悦**的开发体验。

> 在开始之前，推荐先学习 [Blazor](https://docs.microsoft.com/zh-cn/aspnet/core/blazor/?WT.mc_id=DT-MVP-5003987) 和 [.Net Core](https://docs.microsoft.com/zh-cn/dotnet?WT.mc_id=DT-MVP-5003987)，并正确安装和配置了 [.NET Core SDK](https://dotnet.microsoft.com/download?WT.mc_id=DT-MVP-5003987) 3.1.300 或以上。官方指南假设你已了解关于 HTML、CSS 和 JavaScript 的初级知识，并且已经完全掌握了 Blazor 的正确开发方式。如果你刚开始学习.NET 或者 Blazor，将 UI 框架作为你的第一步可能不是最好的主意。


## 第一个本地实例

实际项目开发中，你会需要对 C# 代码的构建、调试、代理、打包部署等一系列工程化的需求。
我们强烈建议使用官方的 Visual Studio 2022 或者 VS Code 进行开发，下面我们用一个简单的实例来说明。


### 创建一个 Blazor WebAssembly 项目

> 在创建项目之前，请确保 `.NET Core SDK 8.0.100+` 已被成功安装。

执行以下命令，`dotnet cli` 会在当前目录下新建一个名称为 `PROJECT-NAME` 的文件夹，并自动安装好相应依赖。

```bash
$ dotnet new blazorwasm -o PROJECT-NAME
```

### 开发调试

一键启动调试，运行成功后显示模板页面。

```bash
$ dotnet run
```

### 构建和部署

```bash
$ dotnet publish -c release -o dist
```

入口文件会构建到 `dist/wwwroot` 目录中，你可以自由部署到不同环境中进行引用。


### 安装组件

```bash
$ dotnet add package AntDesign
```

### 注册依赖

在 `Startup.cs` 文件中注册 AntDesign 服务

```cs
public void ConfigureServices(IServiceCollection services)
{
  ...
  services.AddAntDesign();
}
```

- 在 `_Imports.razor` 中加入命名空间

  ```
  @using AntDesign
  ```

- 为了动态地显示弹出组件，需要在 `App.razor` 中添加一个 `<AntContainer />` 组件。

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

```html
<Button Type="primary">Primary</Button>
```