using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign;

/// <summary>A sequence of real Input controls for entering a one-time code.</summary>
public partial class InputOTP : AntDomComponentBase
{
    [Parameter] public int Length { get; set; } = 6;
    [Parameter] public string Value { get; set; }
    [Parameter] public string DefaultValue { get; set; }
    [Parameter] public EventCallback<string> ValueChanged { get; set; }
    /// <summary>Fires when every cell is filled and the completed value changes.</summary>
    [Parameter] public EventCallback<string> OnChange { get; set; }
    [Parameter] public EventCallback<string[]> OnInput { get; set; }
    [Parameter] public Func<string, string> Formatter { get; set; }
    [Parameter] public InputSize Size { get; set; }
    [Parameter] public InputVariant? Variant { get; set; }
    [Parameter] public FormValidateStatus? Status { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool AutoFocus { get; set; }
    [Parameter] public InputType Type { get; set; } = InputType.Text;
    [Parameter] public string AutoComplete { get; set; }
    [Parameter] public string InputMode { get; set; }
    [Parameter] public OneOf<bool, string> Mask { get; set; }
    [Parameter] public RenderFragment<int> Separator { get; set; }
    [Parameter] public string SeparatorText { get; set; }
    [Parameter] public IReadOnlyDictionary<string, string> ClassNames { get; set; }
    [Parameter] public IReadOnlyDictionary<string, string> Styles { get; set; }

    private readonly Dictionary<int, Input<string>> _inputs = new();
    private string[] _cells = Array.Empty<string>();
    private bool _initialized;
    private bool _controlled;
    private int _focusIndex = -1;
    private int EffectiveLength => Math.Max(1, Length);
    private bool IsMasked => Mask.IsT0 ? Mask.AsT0 : !string.IsNullOrEmpty(Mask.AsT1);
    private string MaskCharacter => Mask.IsT1 ? Mask.AsT1 : "•";
    private string RootClass => SlotClass("root", $"ant-otp{(Size == InputSize.Large ? " ant-otp-lg" : Size == InputSize.Small ? " ant-otp-sm" : "")}{(RTL ? " ant-otp-rtl" : "")} {Class}".Trim());
    private string CellValue(int index) => index < _cells.Length ? _cells[index] : string.Empty;
    private string Format(string value) => Formatter?.Invoke(value) ?? value;
    private string SlotClass(string slot, string initial) => string.Join(" ", new[] { initial, ClassNames != null && ClassNames.TryGetValue(slot, out var value) ? value : null }.Where(value => !string.IsNullOrEmpty(value)));
    private string SlotStyle(string slot, string initial = null) => string.Join(";", new[] { Styles != null && Styles.TryGetValue(slot, out var value) ? value : null, initial }.Where(value => !string.IsNullOrEmpty(value))) is { Length: > 0 } style ? style : null;

    public override Task SetParametersAsync(ParameterView parameters)
    {
        _controlled = parameters.TryGetValue<string>(nameof(Value), out _);
        return base.SetParametersAsync(parameters);
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (_controlled || !_initialized)
            _cells = Format((_controlled ? Value : DefaultValue) ?? string.Empty).Take(EffectiveLength).Select(c => c.ToString()).ToArray();
        if (_cells.Length > EffectiveLength) _cells = _cells.Take(EffectiveLength).ToArray();
        _initialized = true;
    }

    private Dictionary<string, object> InputAttributes(int index)
    {
        var attributes = new Dictionary<string, object> { ["size"] = 1, ["aria-label"] = $"OTP Input {index + 1}" };
        if (!string.IsNullOrEmpty(AutoComplete)) attributes["autocomplete"] = AutoComplete;
        if (!string.IsNullOrEmpty(InputMode)) attributes["inputmode"] = InputMode;
        return attributes;
    }

    private async Task ChangeCell(int index, string text)
    {
        if (Disabled) return;
        var previous = string.Concat(_cells);
        var next = _cells.ToList();
        while (next.Count <= index) next.Add(string.Empty);
        if (text.Length <= 1) next[index] = text;
        else next = next.Take(index).Concat(text.Select(c => c.ToString())).Take(EffectiveLength).ToList();
        while (next.Count > 0 && string.IsNullOrEmpty(next[next.Count - 1])) next.RemoveAt(next.Count - 1);
        var formatted = Format(string.Concat(next.Select(c => string.IsNullOrEmpty(c) ? " " : c)));
        _cells = formatted.Take(EffectiveLength).Select((c, i) => c == ' ' && (i >= next.Count || string.IsNullOrEmpty(next[i])) ? string.Empty : c.ToString()).ToArray();
        var value = string.Concat(_cells);
        await OnInput.InvokeAsync(_cells.ToArray());
        await ValueChanged.InvokeAsync(value);
        if (_cells.Length == EffectiveLength && _cells.All(c => !string.IsNullOrEmpty(c)) && value != previous)
            await OnChange.InvokeAsync(value);
        if (text.Length > 0) _focusIndex = Math.Min(index + text.Length, EffectiveLength - 1);
    }

    private Task KeyDown(int index, KeyboardEventArgs args)
    {
        if (args.Key == "ArrowLeft" || args.Key == "Backspace" && CellValue(index).Length == 0) _focusIndex = index - 1;
        else if (args.Key == "ArrowRight") _focusIndex = index + 1;
        return Task.CompletedTask;
    }

    private Task FocusCell(int index)
    {
        for (var cell = 0; cell < index; cell++)
            if (CellValue(cell).Length == 0) { _focusIndex = cell; break; }
        return Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (_focusIndex >= 0 && _focusIndex < EffectiveLength && _inputs.TryGetValue(_focusIndex, out var input))
        {
            _focusIndex = -1;
            await input.Focus(FocusBehavior.FocusAndSelectAll);
        }
    }

    public Task Focus() => _inputs.TryGetValue(0, out var first) ? first.Focus(FocusBehavior.FocusAndSelectAll) : Task.CompletedTask;
    public async Task Blur() { foreach (var input in _inputs.Values) await input.Blur(); }
}
