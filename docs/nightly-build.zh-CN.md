---
order: 6
title: 获取每日构建
---

如何获取 Ant Design Blazor 的每日构建
=======================================

每日构建包括最新的源代码更改，Nuget 包被发布到了 MyGet 中。它们不支持生产使用，并且会经常发生变化，但我们努力确保夜间构建的功能正常。

如果你想下载最新的每日构建版本并在项目中使用它，那么你需要进行以下操作：

- 在你的项目目录中添加一个 NuGet.Config 配置文件，内容如下：

  ```xml
  <?xml version="1.0" encoding="utf-8"?>
  <configuration>
      <packageSources>
          <clear />
          <add key="AntDesign-Nightly" value="https://www.myget.org/F/ant-design-blazor/api/v3/index.json" />
          <add key="NuGet.org" value="https://api.nuget.org/v3/index.json" />
      </packageSources>
  </configuration>
  ```

  *NOTE: This NuGet.Config should be with your application unless you want nightly packages to potentially start being restored for other apps on the machine.*
  *注意：这个 NuGet.Config 应该和你的应用程序一起使用，不要直接在 Visual Studio 中添加Nuget源，除非你想让机器上的其他应用程序也的每日构建包也被还原。*

#### 使用 Visual Studio 调试每日构建

* 应在 Visual Studio 中 *启用 Source Link 支持*。
* 应在 Visual Studio 中 *启用 Source Server 支持*。
* 取消 *只启用我的代码* 选项
* 在符号下启用 *Microsoft Symbol Servers* 设置。