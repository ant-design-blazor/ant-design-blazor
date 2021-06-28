---
order: 6
title: Getting nightly builds
---

How to get nightly builds of Ant Design Blazor
=======================================

Nightly builds include the latest source code changes. They are not supported for production use and are subject to frequent changes, but we strive to make sure nightly builds function correctly.

If you want to download the latest nightly build and use it in a project, then you need to:

- Add a NuGet.Config to your project directory with the following content:

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

#### To debug nightly builds using Visual Studio

* *Enable Source Link support* in Visual Studio should be enabled.
* *Enable source server support* in Visual should be enabled.
* *Enable Just My Code* should be disabled
* Under Symbols enable the *Microsoft Symbol Servers* setting.
