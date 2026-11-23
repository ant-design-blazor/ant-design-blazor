using System.Collections.Generic;
using System.Linq;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Xunit;

namespace AntDesign.Tests.Transfer;

public sealed class TransferSidePanelTests : AntDesignTestBase
{
    public TransferSidePanelTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void Child_content_receives_each_side_data_and_direction()
    {
        var data = new[]
        {
            new TransferItem { Key = "one", Title = "One" },
            new TransferItem { Key = "two", Title = "Two" }
        };
        var contexts = new List<AntDesign.Internal.TransferSidePanelContext<TransferItem>>();

        var transfer = RenderComponent<AntDesign.Transfer>(parameters => parameters
            .Add(component => component.DataSource, data)
            .Add(component => component.TargetKeys, new[] { "two" })
            .Add(component => component.ChildContent, panel => builder =>
            {
                contexts.Add(panel);
                builder.AddContent(0, $"{panel.Direction}:{string.Join(",", panel.DataSource.Select(item => item.Key))}");
            }));

        var panels = transfer.FindAll(".ant-transfer-list-body-customize-wrapper");
        Assert.Equal(2, panels.Count);
        Assert.Contains("Left:one", panels[0].TextContent);
        Assert.Contains("Right:two", panels[1].TextContent);
        Assert.Contains(contexts, context => context.Direction == TransferDirection.Left && context.DataSource.Single().Key == "one");
        Assert.Contains(contexts, context => context.Direction == TransferDirection.Right && context.DataSource.Single().Key == "two");
    }

    [Fact]
    public void Custom_panel_selection_updates_transfer_and_moves_selected_item()
    {
        var data = new[] { new TransferItem { Key = "one", Title = "One" } };
        TransferSelectChangeArgs selection = null;
        TransferChangeArgs change = null;
        var transfer = RenderComponent<AntDesign.Transfer>(parameters => parameters
            .Add(component => component.DataSource, data)
            .Add(component => component.TargetKeys, System.Array.Empty<string>())
            .Add(component => component.ChildContent, panel => builder =>
            {
                if (panel.DataSource.Count == 0)
                    return;

                var key = panel.DataSource[0].Key;
                builder.OpenElement(0, "button");
                builder.AddAttribute(1, "onclick", EventCallback.Factory.Create(this, () => panel.SelectItemAsync(key, true)));
                builder.AddContent(2, "Select first item");
                builder.CloseElement();
            })
            .Add(component => component.OnSelectChange,
                EventCallback.Factory.Create<TransferSelectChangeArgs>(this, args => selection = args))
            .Add(component => component.OnChange,
                EventCallback.Factory.Create<TransferChangeArgs>(this, args => change = args)));

        transfer.Find(".ant-transfer-list-body-customize-wrapper button").Click();
        Assert.Equal(new[] { "one" }, selection.SourceSelectedKeys);

        transfer.Find(".ant-transfer-operation button").Click();

        Assert.Equal(new[] { "one" }, change.TargetKeys);
        Assert.Empty(selection.SourceSelectedKeys);
        Assert.Empty(selection.TargetSelectedKeys);
    }

    [Fact]
    public void Selected_keys_can_be_controlled_externally()
    {
        var data = new[]
        {
            new TransferItem { Key = "one", Title = "One" },
            new TransferItem { Key = "two", Title = "Two" }
        };
        var transfer = RenderComponent<AntDesign.Transfer>(parameters => parameters
            .Add(component => component.DataSource, data)
            .Add(component => component.TargetKeys, System.Array.Empty<string>())
            .Add(component => component.SelectedKeys, new[] { "one" }));

        var checkboxes = transfer.FindAll(".ant-transfer-list-content-item input[type=checkbox]");
        Assert.True(checkboxes[0].HasAttribute("checked"));
        checkboxes[1].Change(true);

        checkboxes = transfer.FindAll(".ant-transfer-list-content-item input[type=checkbox]");
        Assert.True(checkboxes[0].HasAttribute("checked"));
        Assert.False(checkboxes[1].HasAttribute("checked"));
    }
}
