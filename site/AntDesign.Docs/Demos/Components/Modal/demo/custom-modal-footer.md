---
order: 180
title:
  zh-CN: 自定义Modal的Footer
  en-US: Custom Modal's footer

---

## zh-CN

用户可以自定义 Modal 组件的 Footer，以实现任意按钮及其功能。为了方便用户使用，AntDesign 内置了 仅有一个 OK 按钮的组件(ModalFooter.DefaultOkFooter) 和 仅有一个 Cancel 按钮的组件(ModalFooter.DefaultCancelFooter).


``` c#
// DefaultOkFooter.razor
@{
    var okProps = ModalProps.OkButtonProps ?? new ButtonProps();
    okProps.Type = ModalProps.OkType;
    okProps.Loading = ModalProps.ConfirmLoading;
}

<div>
    <Button OnClick="@HandleOk"
            Loading="@okProps.Loading"
            Type="@okProps.Type"
            Block="@okProps.Block"
            Ghost="@okProps.Ghost"
            Shape="@okProps.Shape"
            Size="@okProps.Size"
            Icon="@okProps.Icon"
            Disabled="@okProps.Disabled"
            Danger="@okProps.IsDanger">
        @{
            if (ModalProps.OkText.IsT0)
            {
                @(ModalProps.OkText.AsT0)
            }
            else
            {
                @(ModalProps.OkText.AsT1)
            }
        }
    </Button>
</div>

@code{
    /// <summary>
    ///
    /// </summary>
    [CascadingParameter]
    public DialogOptions ModalProps { get; set; }

    private async Task HandleOk(MouseEventArgs e)
    {
        var onOk = ModalProps.OnOk;
        if (onOk != null)
        {
            await onOk.Invoke(e);
        }
    }
}
```


## en-US

Users can customize the Footer of Modal components to realize any button and its functions. To facilitate users' use, AntDesign has built-in footers with only one OK button (ModalFooter.DefaultOkFooter) and only one Cancel button (ModalFooter.DefaultCancelFooter).

``` c#
// DefaultOkFooter.razor
@{
    var okProps = ModalProps.OkButtonProps ?? new ButtonProps();
    okProps.Type = ModalProps.OkType;
    okProps.Loading = ModalProps.ConfirmLoading;
}

<div>
    <Button OnClick="@HandleOk"
            Loading="@okProps.Loading"
            Type="@okProps.Type"
            Block="@okProps.Block"
            Ghost="@okProps.Ghost"
            Shape="@okProps.Shape"
            Size="@okProps.Size"
            Icon="@okProps.Icon"
            Disabled="@okProps.Disabled"
            Danger="@okProps.IsDanger">
        @{
            if (ModalProps.OkText.IsT0)
            {
                @(ModalProps.OkText.AsT0)
            }
            else
            {
                @(ModalProps.OkText.AsT1)
            }
        }
    </Button>
</div>

@code{
    /// <summary>
    ///
    /// </summary>
    [CascadingParameter]
    public DialogOptions ModalProps { get; set; }

    private async Task HandleOk(MouseEventArgs e)
    {
        var onOk = ModalProps.OnOk;
        if (onOk != null)
        {
            await onOk.Invoke(e);
        }
    }
}
```
