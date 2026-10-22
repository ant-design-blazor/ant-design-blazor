using AntDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Internal;

public sealed record TransferSideSelection(TransferDirection Direction, string Key, bool Selected);

public sealed record TransferSideSelectAll(TransferDirection Direction, bool Selected);

public sealed class TransferSidePanelContext<TItem>
{
    private readonly Func<string, bool, Task> _selectItem;

    public TransferSidePanelContext(IReadOnlyList<TItem> dataSource, TransferDirection direction,
        IEnumerable<string> selectedKeys, Func<TItem, string> keySelector, Func<string, bool, Task> selectItem,
        IEnumerable<string> targetKeys = null, bool disabled = false, RenderFragment defaultContent = null)
    {
        DataSource = dataSource;
        Direction = direction;
        SelectedKeys = selectedKeys.ToHashSet(StringComparer.Ordinal);
        KeySelector = keySelector;
        _selectItem = selectItem;
        TargetKeys = targetKeys?.ToHashSet(StringComparer.Ordinal) ?? new HashSet<string>(StringComparer.Ordinal);
        Disabled = disabled;
        DefaultContent = defaultContent;
    }

    public IReadOnlyList<TItem> DataSource { get; }
    public TransferDirection Direction { get; }
    public IReadOnlySet<string> SelectedKeys { get; }
    public IReadOnlySet<string> TargetKeys { get; }
    public Func<TItem, string> KeySelector { get; }
    public bool Disabled { get; }
    public RenderFragment DefaultContent { get; }
    public bool IsSelected(TItem item) => SelectedKeys.Contains(KeySelector(item));
    public Task SelectItemAsync(string key, bool selected) => _selectItem(key, selected);
}
