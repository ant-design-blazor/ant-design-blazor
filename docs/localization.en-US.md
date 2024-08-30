---
order: 4.1
title: Localization
---

The localization of this site is through `AntDesign.Extensions.Localization` libraries to achieve, mainly provide interactive localization services.
It can integrate with official or third-party localization providers to switch languages at runtime without forced reload. In addition, it includes a simple embedded JSON provider.

## Installation

```shell
dotnet add package AntDesign.Extensions.Localization
```

### Use interactive localization service

- Add the following code to the 'Program.cs' file:

    ```csharp
    builder.Services.AddInteractiveStringLocalizer();
    services.AddLocalization(options =>
    {
        options.ResourcesPath = "Resources";
    });

    ```

- Create resource files in the `Resources` directory of the project in the format `{ResourceName}.{language}.resx`, for example `Index.en-US.resx` and `index.en-cn.resx`.

  Index.en-US.resx:
  
  | key | value |
  | ---- | ---- |
  | Hello | Hello! |
  | Goodbye | Goodbye! |

  Index.zh-CN.resx:
  
  | key | value |
  | ---- | ---- |
  | Hello | Hello! |
  | Goodbye | Goodbye! |

It should be noted that, `IStringLocalizer<T>`  needs to specify a generic type parameter as the positioning of the resx file, so you need to ensure that the resource file has a corresponding public type. 
Or manually modify the csproj file as follows:

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
  
  
- When in use, inject the `IStringLocalizer<T>` service in razor, for example:

    ```html
    @inject IStringLocalizer<Index> localizer

    <p>@localizer["Hello"]</p>
    <p>@localizer["Goodbye"]</p>
    ```

- Switch UI languages interactively

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

- To use a third-party Localization provider, or other configurations, refer to the [Official Documentation](https://learn.microsoft.com/zh-cn/aspnet/core/blazor/globalization-localization?view=aspnetcore-8.0&WT.mc_id=DT-MVP-5003987)。

### Localization of form validation messages

The default validator for the Form component is [ObjectGraphDataAnnotationsValidator](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/validation?view=aspnetcore-8.0&WT.mc_id=DT-MVP-5003987#nested-models-collection-types-and-complex-types), supports [DataAnnotations localization](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization/make-content-localizable?view=aspnetcore-8.0&WT.mc_id=DT-MVP-5003987#dataannotations-localization)。

Demo: [Form-localization ](https://antblazor.com/en-US/components/form#components-form-demo-localization)
=
Note: Configuration of validation information in AntDesign locales is not currently supported.

### DisplayAttribute  supports localization

Some UI bindings in the component use the DisplayAttribute feature of the entity model or enumeration type as a Label display, such as FormItem, EnumSelect, EnumRadioGroup, EnumCheckboxGroup, and so on.

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


### Use the simple embedded JSON provider

While we recommend the official localization solution, you can also use the solution of this documentation site to implement localization with a simple embedded JSON provider. It should be noted that it only supports direct injection of IStringLocalizer, not its generics (IStringLocalizer<T>), and localization files only support a single file.

- In `Program.cs` file, please add the following code, this method in internal has invoked the ` AddInteractiveStringLocalizer `.

    ```csharp
    builder.Services.AddSimpleEmbeddedJsonLocalization(options =>
    {
        options.ResourcesPath = "Resources";
    });
    ```

- Create resource files in the 'Resources' directory of the project in the format `{language}.json`, for example `en-US.json` and `zh-CN.json`.

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

- Set the build action for the JSON file, set to "embedded resource"
  
  ```xml
    <ItemGroup>
        <EmbeddedResource Include="Resources\*.json" />
    </ItemGroup>
  ```
  
- Inject the `IStringLocalizer` service (without generics) in razor when used, for example:

    ```razor
    @inject IStringLocalizer localizer

    <p>@localizer["Hello"]</p>
    <p>@localizer["Goodbye"]</p>
    ```




### Language identifier on the route

From the beginning of this site, we have implemented the language identifier of the route. It has the following features:

1. When you enter the page, it can automatically add a `{locale}` parameter to the route according to the browser environment, such as `/en-US/Index`.
2. It can also switch languages according to the existing identifier on the route, so when you want to switch languages, you only need to navigate to the path of the corresponding language.

Here's how it works:

- First, in `Routes.Razor` file, implementation the `OnNavigateAsync` method for `Router` components, called `LocalizationService.SetLanguage ` method to switch the language.

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

- Finally, you need to add the `{locale}` parameter to all page components, so that the switched route matches the corresponding page.

```razor
@page "/{locale}/Index"

@inject IStringLocalizer<Index> localizer

<p>@localizer["Hello"]</p>

@code {

    [Parameter]
    public string Locale { get; set; }
}

```
