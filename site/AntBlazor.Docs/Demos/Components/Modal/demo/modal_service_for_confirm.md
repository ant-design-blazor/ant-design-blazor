---
order: 12
title:
  zh-CN: Confirm模板组件
  en-US: ConfirmTemplate
---

## zh-CN

通过 ModalService 创建一个 Confirm 对话框，示例中演示了用户自定义模板、自定义component。

模板代码：ConfirmTemplateDemo.razor

``` c#

@inherits ConfirmTemplate<string, string>

<div>
    <Text>Please input "@config"</Text>
    value: <Input @bind-Value="value" Placeholder="@config" />
</div>

@code{

    string config;

    string value;

    protected override void OnInitialized()
    {
        config = this.Options;
        base.OnInitialized();
    }


    public override async Task OkAsync(ModalClosingEventArgs args)
    {
        ConfirmRef.Config.OkButtonProps.Loading = true;
        await Task.Delay(1000);
        // only the input's value equals the initialized value, the OK button will close the confirm dialog box
        if (value != config)
            args.Cancel = true;
        else
            await this.OnOkAsync(value);

        await base.OkAsync(args);
    }
}

```
## en-US

Create a Confirm dialog box through ModalService, examples demonstrate user-defined templates, custom components.
