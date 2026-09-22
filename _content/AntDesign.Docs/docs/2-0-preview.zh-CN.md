---
order: 8
title: 2.0 预览版（antd v5）
---

大家好，我们很高兴地宣布 Ant Design Blazor 2.0 的早期版本已发布到 Nuget，这个版本实现了antd v5的样式，您可通过安装包进行试用。

以下是与 1.0 不同的安装步骤

.csproj:
```xml
<PackageReference Include="AntDesign" Version="2.0.0-nightly-*" />
```

_import.razor:

```cs
@using AntDesign
@using CssInCSharp
```

App.razor: 使用 `StyleOutlet` 替代 `HeadOutlet`。

```razor
    <link no-antblazor-css />
    <StyleOutlet @rendermode="InteractiveServer" />
    @* <HeadOutlet @rendermode="InteractiveServer" /> *@
```

目前 2.0 的分支暂时在 https://github.com/ant-design-blazor/ant-design-blazor/tree/compatibility-v5 , 欢迎大家的贡献!

请试用并提供反馈意见，我们会继续改进 Blazor 的开发体验。