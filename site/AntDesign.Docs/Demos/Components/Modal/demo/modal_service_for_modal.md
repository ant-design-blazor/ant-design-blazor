---
order: 130
title:
  zh-CN: Modal模板组件
  en-US: ModalTemplate
---

## zh-CN

通过 ModalService 创建一个 Modal 对话框，示例中演示了自定义组件。

模板代码：ModalTemplateDemo.razor

``` c#
@inherits FeedbackComponent<Form.demo.Basic.Model>

<Form Model="@_model"
      LabelCol="new ColLayoutParam { Span = 8 }"
      WrapperCol="new ColLayoutParam { Span = 16 }"
      OnFinish="OnFinish"
      OnFinishFailed="OnFinishFailed">
    <FormItem Label="Username">
        <Input @bind-Value="@context.Username" />
    </FormItem>
    <FormItem Label="Password">
        <InputPassword @bind-Value="@context.Password" />
    </FormItem>
    <FormItem WrapperCol="new ColLayoutParam{ Offset = 8, Span = 16 }">
        <Checkbox @bind-Value="context.RememberMe">Remember me</Checkbox>
    </FormItem>
    <FormItem WrapperCol="new ColLayoutParam{ Offset = 8, Span = 16 }">
        <Button Type="@ButtonType.Primary" HtmlType="submit">
            Submit
        </Button>
    </FormItem>
</Form>

<br />
<br />


@code{


    private Form.demo.Basic.Model _model;

    protected override void OnInitialized()
    {
        _model = base.Options ?? new Form.demo.Basic.Model();
        base.OnInitialized();
    }

    private void OnFinish(EditContext editContext)
    {
        Console.WriteLine($"Success:{JsonSerializer.Serialize(_model)}");
        _ = base.FeedbackRef.CloseAsync();
    }

    private void OnFinishFailed(EditContext editContext)
    {
        Console.WriteLine($"Failed:{JsonSerializer.Serialize(_model)}");
    }
}

```

## en-US

Create a Modal dialog box through ModalService, examples demonstrate custom components.

Template code: ModalTemplateDemo.razor

``` c#
@inherits FeedbackComponent<Form.demo.Basic.Model>

<Form Model="@_model"
      LabelCol="new ColLayoutParam { Span = 8 }"
      WrapperCol="new ColLayoutParam { Span = 16 }"
      OnFinish="OnFinish"
      OnFinishFailed="OnFinishFailed">
    <FormItem Label="Username">
        <Input @bind-Value="@context.Username" />
    </FormItem>
    <FormItem Label="Password">
        <InputPassword @bind-Value="@context.Password" />
    </FormItem>
    <FormItem WrapperCol="new ColLayoutParam{ Offset = 8, Span = 16 }">
        <Checkbox @bind-Value="context.RememberMe">Remember me</Checkbox>
    </FormItem>
    <FormItem WrapperCol="new ColLayoutParam{ Offset = 8, Span = 16 }">
        <Button Type="@ButtonType.Primary" HtmlType="submit">
            Submit
        </Button>
    </FormItem>
</Form>

<br />
<br />


@code{


    private Form.demo.Basic.Model _model;

    protected override void OnInitialized()
    {
        _model = base.Options ?? new Form.demo.Basic.Model();
        base.OnInitialized();
    }

    private void OnFinish(EditContext editContext)
    {
        Console.WriteLine($"Success:{JsonSerializer.Serialize(_model)}");
        _ = base.FeedbackRef.CloseAsync();
    }

    private void OnFinishFailed(EditContext editContext)
    {
        Console.WriteLine($"Failed:{JsonSerializer.Serialize(_model)}");
    }
}

```