# OverlayService

## Overview

`OverlayService` is a general-purpose overlay service that allows you to dynamically render Blazor render fragments at any position. It is particularly useful for scenarios where you need to display content at specific coordinates, such as context menus, tooltips, etc.

## Features

- ✅ Display Blazor components at any coordinates
- ✅ Support custom containers (defaults to body)
- ✅ Automatically adjust position to fit viewport
- ✅ Support multiple concurrent overlays
- ✅ Lifecycle management (open/close)
- ✅ Full unit test coverage

## Usage

### 1. Register Service

The service is automatically registered in `AddAntDesign()`, no additional configuration needed:

```csharp
builder.Services.AddAntDesign();
```

### 2. Inject Service

Inject `IOverlayService` in your component:

```razor
@inject IOverlayService OverlayService
```

### 3. Open Overlay

#### Async Way

```csharp
var overlayRef = await OverlayService.OpenAsync(content, x, y, container);
```

#### Sync Way

```csharp
var overlayRef = OverlayService.Open(content, x, y, container);
```

**Parameters:**
- `content` (RenderFragment): Content to render
- `x` (double): X coordinate (pixels from left of container)
- `y` (double): Y coordinate (pixels from top of container)
- `container` (string, optional): Container selector, defaults to "body"

### 4. Close Overlay

```csharp
// Close async
await overlayRef.CloseAsync();

// Close sync
overlayRef.Close();

// Close all overlays
OverlayService.CloseAll();
```

## Examples

### Context Menu Example

```razor
@inject IOverlayService OverlayService

<div @oncontextmenu="HandleContextMenu" @oncontextmenu:preventDefault>
    Right-click here
</div>

@code {
    private OverlayReference _currentOverlay;

    private async Task HandleContextMenu(MouseEventArgs e)
    {
        // Close previous menu
        if (_currentOverlay != null && !_currentOverlay.IsClosed)
        {
            await _currentOverlay.CloseAsync();
        }

        // Create menu content
        RenderFragment menuContent = builder =>
        {
            builder.OpenComponent<Menu>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(menuBuilder =>
            {
                menuBuilder.OpenComponent<MenuItem>(0);
                menuBuilder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Option 1")));
                menuBuilder.CloseComponent();

                menuBuilder.OpenComponent<MenuItem>(10);
                menuBuilder.AddAttribute(11, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Option 2")));
                menuBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        };

        // Open menu at mouse position
        _currentOverlay = await OverlayService.OpenAsync(menuContent, e.ClientX, e.ClientY);
    }

    public void Dispose()
    {
        _currentOverlay?.Close();
    }
}
```

### Custom Position Tooltip

```razor
@inject IOverlayService OverlayService

<Button @onclick="ShowTooltip">Show Tooltip</Button>

@code {
    private async Task ShowTooltip()
    {
        RenderFragment tooltip = builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "style", "background: white; padding: 10px; border-radius: 4px; box-shadow: 0 2px 8px rgba(0,0,0,0.15);");
            builder.AddContent(2, "This is a custom tooltip");
            builder.CloseElement();
        };

        var overlayRef = await OverlayService.OpenAsync(tooltip, 100, 100);

        // Auto close after 3 seconds
        _ = Task.Delay(3000).ContinueWith(async _ => await overlayRef.CloseAsync());
    }
}
```

## Advanced Usage

### Display in Custom Container

```csharp
// Display in element with ID "my-container"
var overlayRef = await OverlayService.OpenAsync(content, 50, 50, "#my-container");
```

### Manage Multiple Overlays

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

## API Reference

### IOverlayService

| Method | Description |
|--------|-------------|
| `Task<OverlayReference> OpenAsync(RenderFragment content, double x, double y, string container = "body")` | Open overlay asynchronously |
| `OverlayReference Open(RenderFragment content, double x, double y, string container = "body")` | Open overlay synchronously |
| `void CloseAll()` | Close all open overlays |

### OverlayReference

| Property/Method | Description |
|----------------|-------------|
| `string Id { get; }` | Unique identifier of the overlay |
| `bool IsClosed { get; }` | Whether the overlay is closed |
| `Task CloseAsync()` | Close the overlay asynchronously |
| `void Close()` | Close the overlay synchronously |

## Notes

1. **Auto Position Adjustment**: The overlay automatically adjusts its position to ensure it's fully visible within the viewport
2. **Z-Index Management**: The overlay's z-index is automatically set to the current page's highest value + 1
3. **Memory Management**: Remember to close overlays when components are destroyed to avoid memory leaks
4. **Container Selector**: The container parameter supports any valid CSS selector

## Related Components

- [Dropdown](../dropdown) - Dropdown menu component
- [Popover](../popover) - Popover card
- [Tooltip](../tooltip) - Text tooltip

## Demo

For a complete demo, please see the [Dropdown Context Menu Example](../../Components/Dropdown#components-dropdown-demo-context-menu-trigger).
