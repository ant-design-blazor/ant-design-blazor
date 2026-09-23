using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign;

/// <summary>Color selection control backed by the browser's native color input.</summary>
[Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "", Title = "ColorPicker", SubTitle = "颜色选择器")]
public partial class ColorPicker : AntDomComponentBase
{
    [Parameter] public string Value { get; set; } = "#1677ff";
    [Parameter] public EventCallback<string> ValueChanged { get; set; }
    [Parameter] public bool Open { get; set; }
    [Parameter] public EventCallback<bool> OpenChanged { get; set; }
    [Parameter] public bool ShowText { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string AriaLabel { get; set; } = "Choose color";

    private string CurrentValue => string.IsNullOrWhiteSpace(Value) ? "#1677ff" : Value;

    private async Task Toggle(MouseEventArgs _)
    {
        if (!Disabled)
        {
            Open = !Open;
            await OpenChanged.InvokeAsync(Open);
        }
    }

    private Task OnInput(ChangeEventArgs args)
    {
        Value = args.Value?.ToString() ?? CurrentValue;
        return ValueChanged.InvokeAsync(Value);
    }
}
