---
order: 10
title:
  zh-CN: 服务方式创建
  en-US: Drawer's service
---

## zh-CN

Drawer 的 service 用法，示例中演示了用户自定义模板、自定义 component。

模板代码：DrawerTemplateDemo.razor

```csharp
@namespace AntDesign
@inherits FeedbackComponent<string, string>

<div>
    value: <Input @bind-Value="value" />
    <br/>
    <br/>
    <Button Type="ButtonType.Primary" OnClick="OnClose">Confirm</Button>
</div>

@code{

    string value;

    protected override void OnInitialized()
    {
        value = base.Options;
        base.OnInitialized();
    }

    async Task OnClose()
    {
        DrawerRef<string> drawerRef = base.FeedbackRef as DrawerRef<string>;
        await drawerRef!.CloseAsync(value);
    }
}
```

## en-US

Usage of Drawer's service, examples demonstrate user-defined templates, custom components.

Template code: DrawerTemplateDemo.razor

```csharp
@namespace AntDesign
@inherits FeedbackComponent<string, string>

<div>
    value: <Input @bind-Value="value" />
    <br/>
    <br/>
    <Button Type="ButtonType.Primary" OnClick="OnClose">Confirm</Button>
</div>

@code{

    string value;

    protected override void OnInitialized()
    {
        value = base.Options;
        base.OnInitialized();
    }

    async Task OnClose()
    {
        DrawerRef<string> drawerRef = base.FeedbackRef as DrawerRef<string>;
        await drawerRef!.CloseAsync(value);
    }
}
```
