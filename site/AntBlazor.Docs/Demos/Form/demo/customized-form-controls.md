---
order: 6.1
title:
  zh-CN: 自定义表单控件
  en-US: Customized Form Controls
---

## zh-CN

自定义或第三方的表单控件，也可以与 Form 组件一起使用。只要该组件遵循以下的约定：

> - 继承AntInputComponentBase
> - 当需要给控件值（Value）赋值时，改为给CurrentValue赋值（Value和CurrentValue均来自AntInputComponentBase）
> - 详情可参考：参考：[如何实现支持Form表单的组件？](https://github.com/ant-design-blazor/ant-design-blazor/wiki/%E5%A6%82%E4%BD%95%E5%AE%9E%E7%8E%B0%E6%94%AF%E6%8C%81Form%E8%A1%A8%E5%8D%95%E7%9A%84%E7%BB%84%E4%BB%B6%EF%BC%9F)

Price:
```C#
public class Price
{
    public int Number { get; set; }
    public string Currency { get; set; }
}
```

PriceInput.razor:
```C#
@namespace AntDesign.Docs
@inherits AntInputComponentBase<Price>

<span>
    <Input @bind-Value="@Value.Number"
           style="width: 100px" />
  
    <RadioGroup @bind-Value="@Value.Currency"
                style="width: 280px; margin: 0 8px">
        <Radio RadioButton Value=@("rmb")>RMB</Radio>
        <Radio RadioButton Value=@("dollar")>Dollar</Radio>
    </RadioGroup>
</span>
```

## en-US

Customized or third-party form controls can be used in Form, too. Controls must follow these conventions:

> - Inherits AntInputComponentBase
> - Assign value to [CurrentValue] instead of [Value]([CurrentValue] and [Value] are from AntInputComponentBase)

Price:
```C#
public class Price
{
    public int Number { get; set; }
    public string Currency { get; set; }
}
```

PriceInput.razor:
```C#
@namespace AntDesign.Docs
@inherits AntInputComponentBase<Price>

<span>
    <Input @bind-Value="@Value.Number"
           style="width: 100px" />
  
    <RadioGroup @bind-Value="@Value.Currency"
                style="width: 280px; margin: 0 8px">
        <Radio RadioButton Value=@("rmb")>RMB</Radio>
        <Radio RadioButton Value=@("dollar")>Dollar</Radio>
    </RadioGroup>
</span>
```
