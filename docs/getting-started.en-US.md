---
order: 1
title: Getting Started
---

Ant Design of Blazor is dedicated to providing a **good development experience** for programmers.

> Before delving into Ant Design Blazor, a good knowledge of [Blazor](https://docs.microsoft.com/en-us/aspnet/core/blazor/?WT.mc_id=DT-MVP-5003987) and [.NET Core](https://docs.microsoft.com/en-us/dotnet?WT.mc_id=DT-MVP-5003987) is needed.

## First Local Development

During development, you may need to compile and debug .NET code, and even proxy some of the requests to mock data or other external services. All of these can be done with quick feedback provided through hot reloading of changes.

Such features, together with packaging the production version, are covered in this work flow.

### Installation

We strongly recommend to develop Blazor with the `.NET Core SDK`. You can install it from `https://dotnet.microsoft.com/download?WT.mc_id=DT-MVP-5003987`.

### Create a New Blazor WebAssembly Project

A new project can be created using .NET Core SDK.

```bash
$ dotnet new blazorwasm -o PROJECT-NAME
```

`dotnet cli` will run `dotnet restore` after a project is created. If it fails, you can run `dotnet restore` by yourself.

### Development & Debugging

Run your project now.

```bash
$ dotnet run
```

### Building & Deployment

```bash
$ dotnet publish -c release -o dist
```

Entry files will be built and generated in `dist/wwwroot` directory, where we can deploy it to different environments.

### Install Ant Design Blazor

```bash
$ dotnet add package AntDesign
```

### Register Dependencies

Add dependency registration in `Startup.cs`.

```cs
public void ConfigureServices(IServiceCollection services)
{
  ...
  services.AddAntDesign();
}
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