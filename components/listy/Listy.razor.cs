using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace AntDesign;

/// <summary>Renders a grouped, scrollable list of items.</summary>
[Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "", Title = "Listy", SubTitle = "分组列表")]
public partial class Listy<TItem> : AntDomComponentBase
{
    [Parameter] public IEnumerable<TItem> DataSource { get; set; }
    [Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }
    [Parameter] public RenderFragment<string> GroupHeaderTemplate { get; set; }
    [Parameter] public System.Func<TItem, string> GroupBy { get; set; }
    [Parameter] public int Height { get; set; }
    [Parameter] public int ItemHeight { get; set; } = 32;
    [Parameter] public RenderFragment ChildContent { get; set; }

    private IEnumerable<ListyGroup> GetGroups()
    {
        var indexed = (DataSource ?? Enumerable.Empty<TItem>()).Select((value, index) => new ListyItem(index, value));
        return indexed.GroupBy(item => GroupBy?.Invoke(item.Value) ?? string.Empty)
            .Select(group => new ListyGroup(group.Key, group.ToList()));
    }

    private sealed record ListyItem(int Index, TItem Value);
    private sealed record ListyGroup(string Key, List<ListyItem> Items);
}
