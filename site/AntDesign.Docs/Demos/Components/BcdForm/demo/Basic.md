---
order: 0
title:
  zh-CN: 全部功能
  en-US: Full
---

## zh-CN

全部功能

```c#
@namespace AntDesign.Docs.Demos.Components.BcdForm
@inherits BcdForm

<h3>BcdForm Component</h3>

<div>
    <Button OnClick="EventUtil.AsNonRenderingEventHandler(Close)">Close</Button>
</div>

@code{

    protected override void InitComponent()
    {
        ShowMask = true;
        MaskClosable = true;
        MinimizeBox = true;
        MaximizeBox = true;
        Draggable = true;
    }

    private async void Close()
    {
        await base.CloseAsync();
    }
}
```

## en-US

Full usage

```c#
@namespace AntDesign.Docs.Demos.Components.BcdForm
@inherits BcdForm

<h3>BcdForm Component</h3>

<div>
    <Button OnClick="EventUtil.AsNonRenderingEventHandler(Close)">Close</Button>
</div>

@code{

    protected override void InitComponent()
    {
        ShowMask = true;
        MaskClosable = true;
        MinimizeBox = true;
        MaximizeBox = true;
        Draggable = true;
    }

    private async void Close()
    {
        await base.CloseAsync();
    }
}
```
