---
order: 8
title: Try 2.0 preview (antd v5)
---

Hello everyone, we are very happy that the early 2.0 version for Ant Design v5 have been released to Nuget, and you can try it by install the package:

.csproj:
```xml
<PackageReference Include="AntDesign" Version="2.0.0-nightly-*" />
```

_import.razor:
```cs
@using AntDesign
@using CssInCSharp
```

App.razor:
```razor
    <link no-antblazor-css />
    <StyleOutlet @rendermode="InteractiveServer" />
    @* <HeadOutlet @rendermode="InteractiveServer" /> *@
```

The branch of v5 is on https://github.com/ant-design-blazor/ant-design-blazor/tree/compatibility-v5 , welcome contributions!

Please try and give us your feedback. We will continue the improvment for the development experience.
