using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace AntDesign.Docs.Shared;

/// <summary>
/// An action column that adds reusable row sorting behavior to a Table.
/// </summary>
public sealed class TableDragSortingColumn : ActionColumn, IAsyncDisposable
{
    private IJSObjectReference? _module;
    private DotNetObjectReference<TableDragSortingColumn>? _dotNetReference;
    private bool _sortableInitialized;

    [Parameter, EditorRequired]
    public ForwardRef TableRef { get; set; } = default!;

    [Parameter, EditorRequired]
    public EventCallback<(string ActiveKey, string OverKey)> OnDrop { get; set; }

    /// <summary>
    /// Whether to render a drag handle column. When false, the rows remain draggable
    /// from anywhere and the behavior column does not occupy table space.
    /// </summary>
    [Parameter]
    public bool ShowHandle { get; set; } = true;

    /// <summary>
    /// Whether dragging can start only from the rendered handle.
    /// </summary>
    [Parameter]
    public bool HandleOnly { get; set; }

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    protected override void OnInitialized()
    {
        if (ShowHandle)
        {
            Width ??= "80";
            Align = ColumnAlign.Center;
            ChildContent = builder =>
            {
                builder.OpenComponent<Button>(0);
                builder.AddAttribute(1, nameof(Button.Type), ButtonType.Text);
                builder.AddAttribute(2, nameof(Button.Size), ButtonSize.Small);
                builder.AddAttribute(3, nameof(Button.Icon), "holder");
                builder.AddAttribute(4, nameof(Button.Style), "cursor:move");
                builder.CloseComponent();
            };
        }
        else
        {
            Hidden = true;
        }

        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        // Table columns are rendered once for registration and again for the actual
        // table. Initialize after registration; firstRender can still be the
        // registration pass, so it must not gate this setup.
        if (_module is not null || IsInitialize || TableRef?.Current is null)
        {
            return;
        }

        _module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/AntDesign.Docs/js/table-sortable.js?v=20261118.6");
        _dotNetReference = DotNetObjectReference.Create(this);
        _sortableInitialized = await _module.InvokeAsync<bool>("initializeTableSortable", TableRef.Current, _dotNetReference, HandleOnly);
    }

    [JSInvokable]
    public Task OnSortableDrop(string activeKey, string overKey)
        => OnDrop.InvokeAsync((activeKey, overKey));

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            try
            {
                if (_sortableInitialized && TableRef?.Current is not null)
                {
                    await _module.InvokeVoidAsync("disposeTableSortable", TableRef.Current);
                }

                await _module.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // The browser has already disposed the Blazor circuit.
            }
        }

        _dotNetReference?.Dispose();
    }
}
