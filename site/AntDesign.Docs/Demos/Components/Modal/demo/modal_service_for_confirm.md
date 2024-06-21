---
order: 120
title:
  zh-CN: Confirm模板组件
  en-US: ConfirmTemplate
---

## zh-CN

通过 ModalService 创建一个 Confirm 对话框，示例中演示了用户自定义模板、自定义component。

模板代码：ConfirmTemplateDemo.razor

``` c#
@inherits FeedbackComponent<string, string>

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


    public override async Task OnFeedbackOkAsync(ModalClosingEventArgs args)
    {
        if (FeedbackRef is ConfirmRef confirmRef)
        {
            confirmRef.Config.OkButtonProps.Loading = true;
            await confirmRef.UpdateConfigAsync();
        }
        else if (FeedbackRef is ModalRef modalRef)
        {
            modalRef.Config.ConfirmLoading = true;
            await modalRef.UpdateConfigAsync();
        }

        await Task.Delay(1000);
        // only the input's value equals the initialized value, the OK button will close the confirm dialog box
        if (value != config)
            args.Cancel = true;
        else
            // method 1(not recommended): await (FeedbackRef as ConfirmRef<string>)!.OkAsync(value);
            // method 2: await (FeedbackRef as IOkCancelRef<string>)!.OkAsync(value);
            await base.OkCancelRefWithResult!.OnOk(value);

        await base.OnFeedbackOkAsync(args);
    }

    /// <summary>
    /// If you want <b>Dispose</b> to take effect every time it is closed in Modal, which created by ModalService,
    /// set <b>ModalOptions.DestroyOnClose = true</b>
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
        Console.WriteLine("Dispose");
        base.Dispose(disposing);
    }
}

```
## en-US

Create a Confirm dialog box through ModalService, examples demonstrate user-defined templates, custom components.

Template code: ConfirmTemplateDemo.razor

``` c#

@inherits FeedbackComponent<string, string>

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


    public override async Task OnFeedbackOkAsync(ModalClosingEventArgs args)
    {
        if (FeedbackRef is ConfirmRef confirmRef)
        {
            confirmRef.Config.OkButtonProps.Loading = true;
            await confirmRef.UpdateConfigAsync();
        }
        else if (FeedbackRef is ModalRef modalRef)
        {
            modalRef.Config.ConfirmLoading = true;
            await modalRef.UpdateConfigAsync();
        }

        await Task.Delay(1000);
        // only the input's value equals the initialized value, the OK button will close the confirm dialog box
        if (value != config)
            args.Cancel = true;
        else
            // method 1(not recommended): await (FeedbackRef as ConfirmRef<string>)!.OkAsync(value);
            // method 2: await (FeedbackRef as IOkCancelRef<string>)!.OkAsync(value);
            await base.OkCancelRefWithResult!.OnOk(value);

        await base.OnFeedbackOkAsync(args);
    }

    /// <summary>
    /// If you want <b>Dispose</b> to take effect every time it is closed in Modal, which created by ModalService,
    /// set <b>ModalOptions.DestroyOnClose = true</b>
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
        Console.WriteLine("Dispose");
        base.Dispose(disposing);
    }
}

```
