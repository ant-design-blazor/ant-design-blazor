---
order: 12
title:
  zh-CN: 模板
  en-US: Template
---

## zh-CN

Modal 的 service 用法，示例中演示了用户自定义模板、自定义component。

模板代码：ModalTemplateDemo.razor

    @inherits ModalTemplate<string, string>

    <div>
        <Text>Please input "@config"</Text>
        value: <Input @bind-Value="value" Placeholder="@config" />
    </div>

    @code{

        string config;

        string value;

        protected override void OnInitialized()
        {
            config = this.Config;
            base.OnInitialized();
        }


        public override async Task OkAsync(ModalClosingEventArgs args)
        {
            ModalRef.Config.OkButtonProps.Loading = true;
            await Task.Delay(1000);
            if (value != config)
                args.Cancel = true;
            else
                await this.OnOkAsync(value);

            await base.OkAsync(args);
        }
    }

## en-US

Usage of Modal's service, examples demonstrate user-defined templates, custom components.
