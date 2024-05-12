---
category: Charts
type: 文档
title: Introduce
subtitle: 介绍
cols: 1
cover: 
---

Ant Design Charts Blazor 图表库基于 G2Plot 开发，

GitHub: https://github.com/ant-design-blazor/ant-design-charts-blazor

## 📦 安装

- 进入应用的项目文件夹，安装 NuGet 包引用

  ```bash
  $ dotnet add package AntDesign.Charts
  ```
  
- 在 `wwwroot/index.html`(WebAssembly) 或 `Pages/_Host.razor`(Server) 中引入静态文件:

  ```html
  <script src="https://unpkg.com/@antv/g2plot@1.1.28/dist/g2plot.js"></script>
  <script src="_content/AntDesign.Charts/ant-design-charts-blazor.js"></script>
  ```
  
- 在 `_Imports.razor` 中加入命名空间

  ```csharp
  @using AntDesign.Charts
  ```
  
- 最后就可以在`.razor`组件中引用啦！

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
  
## 🔗 链接

- [Blazor 官方文档](https://blazor.net)

## 🤝 如何贡献

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](https://github.com/ant-design-blazor/ant-design-charts-blazor/pulls)

如果你希望参与贡献，欢迎 [Pull Request](https://github.com/ant-design-blazor/ant-design-charts-blazor/pulls)，或给我们 [报告 Bug](https://github.com/ant-design-blazor/ant-design-charts-blazor/issues/new) 。
