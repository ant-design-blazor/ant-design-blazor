# OverlayService

## 概述

`OverlayService` 是一个通用的覆盖层服务,允许你在任意位置动态渲染 Blazor 渲染片段。它特别适用于需要在特定坐标显示内容的场景,如右键菜单、工具提示等。

## 功能特性

- ✅ 在任意坐标位置显示 Blazor 组件
- ✅ 支持自定义容器(默认为 body)
- ✅ 自动调整位置以适应视口
- ✅ 支持多个并发覆盖层
- ✅ 提供生命周期管理(打开/关闭)
- ✅ 完整的单元测试覆盖

## 使用方法

### 1. 注册服务

服务已自动注册在 `AddAntDesign()` 中,无需额外配置:

```csharp
builder.Services.AddAntDesign();
```

### 2. 注入服务

在组件中注入 `IOverlayService`:

```razor
@inject IOverlayService OverlayService
```

### 3. 打开覆盖层

#### 异步方式

```csharp
var overlayRef = await OverlayService.OpenAsync(content, x, y, container);
```

#### 同步方式

```csharp
var overlayRef = OverlayService.Open(content, x, y, container);
```

**参数说明:**
- `content` (RenderFragment): 要渲染的内容
- `x` (double): X 坐标(相对于容器左侧的像素值)
- `y` (double): Y 坐标(相对于容器顶部的像素值)
- `container` (string, 可选): 容器选择器,默认为 "body"

### 4. 关闭覆盖层

```csharp
// 异步关闭
await overlayRef.CloseAsync();

// 同步关闭
overlayRef.Close();

// 关闭所有覆盖层
OverlayService.CloseAll();
```

## 示例

### 右键菜单示例

```razor
@inject IOverlayService OverlayService

<div @oncontextmenu="HandleContextMenu" @oncontextmenu:preventDefault>
    右键点击此处
</div>

@code {
    private OverlayReference _currentOverlay;

    private async Task HandleContextMenu(MouseEventArgs e)
    {
        // 关闭之前的菜单
        if (_currentOverlay != null && !_currentOverlay.IsClosed)
        {
            await _currentOverlay.CloseAsync();
        }

        // 创建菜单内容
        RenderFragment menuContent = builder =>
        {
            builder.OpenComponent<Menu>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(menuBuilder =>
            {
                menuBuilder.OpenComponent<MenuItem>(0);
                menuBuilder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "选项 1")));
                menuBuilder.CloseComponent();

                menuBuilder.OpenComponent<MenuItem>(10);
                menuBuilder.AddAttribute(11, "ChildContent", (RenderFragment)(b => b.AddContent(0, "选项 2")));
                menuBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        };

        // 在鼠标位置打开菜单
        _currentOverlay = await OverlayService.OpenAsync(menuContent, e.ClientX, e.ClientY);
    }

    public void Dispose()
    {
        _currentOverlay?.Close();
    }
}
```

### 自定义位置工具提示

```razor
@inject IOverlayService OverlayService

<Button @onclick="ShowTooltip">显示工具提示</Button>

@code {
    private async Task ShowTooltip()
    {
        RenderFragment tooltip = builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "style", "background: white; padding: 10px; border-radius: 4px; box-shadow: 0 2px 8px rgba(0,0,0,0.15);");
            builder.AddContent(2, "这是一个自定义工具提示");
            builder.CloseElement();
        };

        var overlayRef = await OverlayService.OpenAsync(tooltip, 100, 100);

        // 3秒后自动关闭
        _ = Task.Delay(3000).ContinueWith(async _ => await overlayRef.CloseAsync());
    }
}
```

## 高级用法

### 在自定义容器中显示

```csharp
// 在 ID 为 "my-container" 的元素中显示
var overlayRef = await OverlayService.OpenAsync(content, 50, 50, "#my-container");
```

### 管理多个覆盖层

```csharp
private List<OverlayReference> _overlays = new();

private async Task OpenMultiple()
{
    var ref1 = await OverlayService.OpenAsync(content1, 100, 100);
    var ref2 = await OverlayService.OpenAsync(content2, 200, 200);
    
    _overlays.Add(ref1);
    _overlays.Add(ref2);
}

private async Task CloseAll()
{
    foreach (var overlay in _overlays)
    {
        await overlay.CloseAsync();
    }
    _overlays.Clear();
}
```

## API 参考

### IOverlayService

| 方法 | 说明 |
|------|------|
| `Task<OverlayReference> OpenAsync(RenderFragment content, double x, double y, string container = "body")` | 异步打开覆盖层 |
| `OverlayReference Open(RenderFragment content, double x, double y, string container = "body")` | 同步打开覆盖层 |
| `void CloseAll()` | 关闭所有打开的覆盖层 |

### OverlayReference

| 属性/方法 | 说明 |
|----------|------|
| `string Id { get; }` | 覆盖层的唯一标识符 |
| `bool IsClosed { get; }` | 覆盖层是否已关闭 |
| `Task CloseAsync()` | 异步关闭覆盖层 |
| `void Close()` | 同步关闭覆盖层 |

## 注意事项

1. **自动位置调整**: 覆盖层会自动调整位置以确保完全显示在视口内
2. **Z-Index 管理**: 覆盖层的 z-index 会自动设置为当前页面最高值 + 1
3. **内存管理**: 记得在组件销毁时关闭覆盖层以避免内存泄漏
4. **容器选择器**: 容器参数支持任何有效的 CSS 选择器

## 相关组件

- [Dropdown](../dropdown) - 下拉菜单组件
- [Popover](../popover) - 气泡卡片
- [Tooltip](../tooltip) - 文字提示

## 演示

完整的演示请参见 [Dropdown 右键菜单示例](../../Components/Dropdown#components-dropdown-demo-context-menu-trigger)。
