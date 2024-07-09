---
order: 4.1
title: 本地化
---

本文档站点的本地化基于 AntDesign.Extensions.Localization 类库实现，主要提供可交互本地化服务，能够集成官方和第三方的本地化提供者实现在运行时无刷新切换语言。另外还实现了简单的嵌入 JSON 提供者。

## 安装

```shell
dotnet add package AntDesign.Extensions.Localization
```

### 使用可交互本地化组件

- 在 `Program.cs` 文件中添加以下代码：

    ```csharp
    builder.Services.AddInteractiveStringLocalizer();
    services.AddLocalization(options =>
    {
        options.ResourcesPath = "Resources";
    });

    ```

- 在项目的 `Resources` 目录下创建多语言文件，格式为 `{ResourceName}.{language}.resx`，例如 `Index.en-US.resx` 和 `Index.zh-CN.resx`。
  
  Index.en-US.resx:
  
  | 键 | 值 |
  | ---- | ---- |
  | Hello | Hello! |
  | Goodbye | Goodbye! |

  Index.zh-CN.resx:
  
  | 键 | 值 |
  | ---- | ---- |
  | Hello | Hello! |
  | Goodbye | Goodbye! |

要注意的是，`IStringLocalizer<T>` 需要指定泛型类型参数作为 resx 文件的定位，因此需要确保资源文件有对应的公开的类型。或如下手动调整 csproj 文件：

```xml
  <ItemGroup>
    <Compile Update="Resources\Resources.Designer.cs">
      <DesignTime>True</DesignTime>
      <AutoGen>True</AutoGen>
      <DependentUpon>Resources.resx</DependentUpon>
    </Compile>
  </ItemGroup>

  <ItemGroup>
    <EmbeddedResource Update="Resources\Resources.resx">
      <Generator>PublicResXFileCodeGenerator</Generator>
      <LastGenOutput>Resources.Designer.cs</LastGenOutput>
    </EmbeddedResource>
  </ItemGroup>
```
  
- 使用时在 razor 注入 IStringLocalizer<T> 服务，例如：

    ```html
    @inject IStringLocalizer<Index> localizer

    <p>@localizer["Hello"]</p>
    <p>@localizer["Goodbye"]</p>
    ```

- 在需要刷新语言的页面订阅语言变更事件，即可切换 UI 上的语言

    ```html
    @implements IDisposable
    @inject ILocalizationService LocalizationService

    <Button OnClick="()=>LocalizationService.SetLanguage("en-US")" >English</Button>
    <Button OnClick="()=>LocalizationService.SetLanguage("zh-CN")" >中文</Button>

    @code {

        protected override void OnInitialized()
        {
            LocalizationService.LanguageChanged += OnLanguageChanged;
        }
        private void OnLanguageChanged(object sender, CultureInfo args)
        {
            InvokeAsync(StateHasChanged);
        }
        public void Dispose()
        {
            LocalizationService.LanguageChanged -= OnLanguageChanged;
        }
    }
    ```


- 需要使用第三方 Localization 提供者，或者其他配置，可参考 [官方文档](https://learn.microsoft.com/zh-cn/aspnet/core/blazor/globalization-localization?view=aspnetcore-8.0&WT.mc_id=DT-MVP-5003987)。

### 表单验证消息的本地化

Form 组件的默认验证器是 [ObjectGraphDataAnnotationsValidator](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/validation?view=aspnetcore-8.0&WT.mc_id=DT-MVP-5003987#nested-models-collection-types-and-complex-types)，支持[ DataAnnotations 的本地化](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization/make-content-localizable?view=aspnetcore-8.0&WT.mc_id=DT-MVP-5003987#dataannotations-localization)。

示例：[Form 表单 - 本地化](https://antblazor.com/en-US/components/form#components-form-demo-localization)

注意：暂不支持 AntDesign locales 中的验证信息配置。

### DisplayAttribute 支持本地化

组件中有部分 UI 绑定利用了实体模型或者枚举类型的 DisplayAttribute 特性用作 Label 显示，如 FormItem、EnumSelect、EnumRadioGroup、EnumCheckboxGroup 等。

  ```cs
    public class Model
    {
        [Display(Name = nameof(Resources.App.UserName), ResourceType = typeof(Resources.App))]
        public string Username { get; set; }
    }

    public enum Province
    {
        [Display(Name = nameof(Resources.App.Shanghai), ResourceType = typeof(Resources.App))]
        Shanghai,

        Jiangsu
    }
  ```

### 使用简单嵌入 JSON 提供者

虽然我们推荐官方的本地化方案，但是也可以用本组件文档站点的方案，利用简单嵌入 JSON 提供者实现多语言。需要注意的是，它仅支持直接注入 IStringLocalizer，不支持其泛型，本地化文件也仅支持单一文件。

- 在 `Program.cs` 文件中添加以下代码，此方法内部已调用 `AddInteractiveStringLocalizer`。

    ```csharp
    builder.Services.AddSimpleEmbeddedJsonLocalization(options =>
    {
        options.ResourcesPath = "Resources";
    });
    ```

- 在项目的 `Resources` 目录下创建多语言文件，格式为 `{language}.json`，例如 `en-US.json` 和 `zh-CN.json`。

    ```json
    // en-US.json
    {
        "Hello": "Hello!",
        "Goodbye": "Goodbye!"
    }

    // zh-CN.json
    {
        "Hello": "你好!",
        "Goodbye": "再见!"
    }
    ```

- 把JSON文件的生成操作设置为嵌入的资源
  
  ```xml
    <ItemGroup>
        <EmbeddedResource Include="Resources\*.json" />
    </ItemGroup>
  ```
  
- 使用时在 razor 注入 IStringLocalizer 服务（没有泛型），例如：

    ```razor
    @inject IStringLocalizer localizer

    <p>@localizer["Hello"]</p>
    <p>@localizer["Goodbye"]</p>
    ```

### 实现路由上的语言标识

从本站点建立之初，就实现了路由的语言标记，有如下特点：

1. 进入页面时可根据浏览器环境，自动在路由上添加 `{locale}` 参数，例如 `/en-US/Index`。
2. 可以根据路由上已有标识切换语言，因此要切换语言时只需跳转到对应语言的路径即可。

以下是实现方式：

- 首先，在 `Routes.razor` 文件实现 `Router` 组件的 `OnNavigateAsync` 方法，调用 `LocalizationService.SetLanguage` 方法切换语言。

```razor
@using System.Reflection
@using System.Globalization

<ConfigProvider>
    <Router AppAssembly="typeof(App).Assembly" OnNavigateAsync="OnNavigateAsync">
        <Found Context="routeData">
            <RouteView RouteData="routeData" DefaultLayout="typeof(MainLayout)" />
            <FocusOnNavigate RouteData="routeData" Selector="h1" />
        </Found>
        <NotFound>
            <Result Status="404" />
        </NotFound>
    </Router>
    <AntContainer />
</ConfigProvider>

@inject ILocalizationService LocalizationService;
@inject NavigationManager NavigationManager;
@code{
    async Task OnNavigateAsync(NavigationContext navigationContext)
    {
        var relativeUri = navigationContext.Path;
        var currentCulture = LocalizationService.CurrentCulture;

        var segment = relativeUri.IndexOf('/') > 0 ? relativeUri.Substring(0, relativeUri.IndexOf('/')) : relativeUri;

        if (string.IsNullOrWhiteSpace(segment))
        {
            NavigationManager.NavigateTo($"{currentCulture.Name}/{relativeUri}");
            return;
        }
        else
        {
            if (segment.IsIn("zh-CN", "en-US"))
            {
                LocalizationService.SetLanguage(CultureInfo.GetCultureInfo(segment));
            }
            else if (currentCulture.Name.IsIn("zh-CN", "en-US"))
            {
                NavigationManager.NavigateTo($"{currentCulture.Name}/{relativeUri}");
            }
            else
            {
                NavigationManager.NavigateTo($"en-US/{relativeUri}");
                return;
            }
        }
    }
}
```

- 最后，给页面组件统一添加 `{locale}` 参数，使切换后的路由匹配对应的页面。

```razor
@page "/{locale}/Index"

@inject IStringLocalizer<Index> localizer

<p>@localizer["Hello"]</p>

@code {

    [Parameter]
    public string Locale { get; set; }
}

```
