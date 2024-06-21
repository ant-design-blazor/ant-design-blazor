---
category: Charts
type: Docs
title: Introduce
cols: 1
cover: 
---

A Blazor chart library, based on G2Plot.

GitHub: https://github.com/ant-design-blazor/ant-design-charts-blazor

## 📦 Installation Guide

- Go to the project folder of the application and install the NuGet package reference

  ```bash
  $ dotnet add package AntDesign.Charts
  ```
  
  - Link the static files in `wwwroot/index.html` (WebAssembly) or `Pages/_Host.razor` (Server)

  ```html
  <script src="https://unpkg.com/@antv/g2plot@1.1.28/dist/g2plot.js"></script>
  <script src="_content/AntDesign.Charts/ant-design-charts-blazor.js"></script>
  ```
  
  - Add namespace in `_Imports.razor`

  ```csharp
  @using AntDesign.Charts
  ```
  
- Finally, it can be referenced in the `.razor' component!

  ```razor
  <Line Data="data" Config="config" />

  @code {
      object[] data = new object[] {
          new  { year = "1991", value = 3 },
          new  { year = "1992", value = 4 },
          new  { year = "1993", value = 3.5 },
          new  { year = "1994", value = 5 },
          new  { year = "1995", value = 4.9 },
          new  { year = "1996", value = 6 },
          new  { year = "1997", value = 7 },
          new  { year = "1998", value = 9 },
          new  { year = "1999", value = 13 },
      };

      LineConfig config = new LineConfig()
          {
              Padding = "auto",
              XField = "year",
              YField = "value",
              Smooth = true,
          };
  }
  ```
  
  ## 🔗 Links

- [Official Blazor Documentation](https://blazor.net)


## 🤝 Contributing

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](https://github.com/ant-design-blazor/ant-design-charts-blazor/pulls)

If you would like to contribute, feel free to create a [Pull Request](https://github.com/ant-design-blazor/ant-design-charts-blazor/pulls), or give us [Bug Report](https://github.com/ant-design-blazor/ant-design-charts-blazor/issues/new).
