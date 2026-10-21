using System.Linq;
using AntDesign.Docs.Services;
using Xunit;

namespace AntDesign.Tests.Docs;

public class TreeSelectSemanticCatalogTests
{
    [Fact]
    public void Selectors_target_the_blazor_tree_select_dom()
    {
        var selectors = SemanticDomCatalog.Get("tree-select")
            .ToDictionary(node => node.Name, node => node.Selector);

        Assert.Equal(".ant-select.ant-tree-select", selectors["root"]);
        Assert.Equal(".ant-select-input", selectors["input"]);
        Assert.Equal(".ant-select-selection-item", selectors["item"]);
        Assert.Equal(".ant-select-dropdown.ant-tree-select-dropdown", selectors["popup.root"]);
        Assert.Equal(".ant-tree-treenode.ant-select-tree-treenode", selectors["popup.item"]);
        Assert.Equal(".ant-tree-title.ant-select-tree-title", selectors["popup.itemTitle"]);
        Assert.Equal(".ant-tree-switcher.ant-select-tree-switcher", selectors["popup.itemSwitcher"]);
    }
}
