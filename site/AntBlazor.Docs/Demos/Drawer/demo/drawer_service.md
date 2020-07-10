---
order: 10
title:
  zh-CN: 服务方式创建
  en-US: Drawer's service
---

## zh-CN

Drawer 的 service 用法，示例中演示了用户自定义模板、自定义component。

模板代码：Drawer_service.razor
@namespace AntDesign
@inherits DrawerTemplate<string, string>

<div>
    value: <Input @bind-Value="value" />
    <br/>
    <br/>
    <Button Type="primary" OnClick="OnClose">Confirm</Button>
</div>

@code{

    string value;

    protected override void OnInitialized()
    {
        value = this.Config;
        base.OnInitialized();
    }


    async void OnClose()
    {
        await this.CloseAsync(value);
    }
}

## en-US

Usage of Drawer's service, examples demonstrate user-defined templates, custom components.
