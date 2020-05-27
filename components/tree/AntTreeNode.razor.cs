using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntBlazor
{
    public partial class AntTreeNode
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [CascadingParameter]
        public AntTree Tree { get; set; }

        [CascadingParameter]
        public AntTreeNode ParentNode { get; set; }

        [Parameter]
        public string Key { get; set; }

        [Parameter]
        public string Avatar { get; set; }

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public string IconType { get; set; }

        [Parameter]
        public RenderFragment<AntTreeNode> IconTemplate { get; set; }

        [Parameter]
        public RenderFragment<AntTreeNode> NodeTemplate { get; set; }

        [Parameter]
        public bool IsExpanded { get; set; }

        [Parameter]
        public EventCallback<bool> IsExpandedChanged { get; set; }

        [Parameter]
        public bool IsSelected { get; set; }

        [Parameter]
        public EventCallback<bool> IsSelectedChanged { get; set; }

        [Parameter]
        public bool IsChecked { get; set; }

        [Parameter]
        public EventCallback<bool> IsCheckedChanged { get; set; }

        [Parameter]
        public bool IsDisabled { get; set; }

        internal List<AntTreeNode> _renderChildren;

        internal void RendeBegin()
        {
            _renderChildren = null;

            ClassMapper.Clear()
                .Add("ant-tree-treenode")
                .If("ant-tree-treenode-switcher-open", () => IsExpanded)
                .If("ant-tree-treenode-switcher-close", () => !IsExpanded)
                .If("ant-tree-treenode-checkbox-checked", () => IsChecked)
                .If("ant-tree-treenode-selected", () => IsSelected)
                .If("ant-tree-treenode-disabled", () => IsDisabled)
                ;

            List<AntTreeNode> plist;
            if (ParentNode != null)
            {
                if (ParentNode._renderChildren == null)
                    ParentNode._renderChildren = new List<AntTreeNode>();
                plist = ParentNode._renderChildren;
            }
            else
            {
                if (Tree._renderChildren == null)
                    Tree._renderChildren = new List<AntTreeNode>();
                plist = Tree._renderChildren;
            }
            plist.Add(this);
            _indexOrParent = plist.Count;
        }

        internal int _indexOrParent;

        internal int CalcLevel()
        {
            if (ParentNode == null)
                return 0;
            return ParentNode.CalcLevel() + 1;
        }

        void ToggleSelect()
        {
            if (IsDisabled)
                return;

            var prevNode = Tree._selectedNode;
            if (prevNode == this)
            {
                Tree._selectedNode = null;
                IsSelected = false;
                _ = IsSelectedChanged.InvokeAsync(false);
                return;
            }

            if (prevNode != null && prevNode != this)
            {
                prevNode.IsSelected = false;
                _ = prevNode.IsSelectedChanged.InvokeAsync(false);
                prevNode.StateHasChanged();
            }

            Tree._selectedNode = this;
            IsSelected = true;
            _ = IsSelectedChanged.InvokeAsync(true);

            StateHasChanged();
        }

        void ToggleExpandedWithAnimate()
        {
            IsExpanded = !IsExpanded;
            _ = IsExpandedChanged.InvokeAsync(IsExpanded);
            StateHasChanged();
        }

        void ToggleCheckedState()
        {
            if (IsDisabled)
                return;

            if (!Tree.CascadingCheckState || _renderChildren == null || _renderChildren.Count == 0)
            {
                IsChecked = !IsChecked;
                _ = IsCheckedChanged.InvokeAsync(IsChecked);
                StateHasChanged();
                return;
            }

            bool AllChecked(List<AntTreeNode> nodes)
            {
                int validChildCount = 0;
                foreach (AntTreeNode node in nodes)
                {
                    if (node == null || node.IsDisabled)
                        continue;
                    validChildCount++;
                    if (node._renderChildren == null || node._renderChildren.Count == 0)
                    {
                        if (!node.IsChecked)
                            return false;
                    }
                    else
                    {
                        if (!AllChecked(node._renderChildren))
                            return false;
                    }
                }
                if (validChildCount != 0)
                    return true;
                return this.IsChecked;
            }

            void SetCheckedRecursive(List<AntTreeNode> nodes, bool value)
            {
                foreach (AntTreeNode node in nodes)
                {
                    if (node == null || node.IsDisabled)
                        continue;

                    Tree.JSRuntime.InvokeVoidAsync("console.log", node.Text, value);

                    if (node.IsChecked != value)
                    {
                        node.IsChecked = value;
                        _ = node.IsCheckedChanged.InvokeAsync(value);
                    }

                    if (node._renderChildren != null)
                        SetCheckedRecursive(node._renderChildren, value);
                }
            }

            IsChecked = !AllChecked(_renderChildren);
            _ = IsCheckedChanged.InvokeAsync(IsChecked);
            SetCheckedRecursive(_renderChildren, IsChecked);

            StateHasChanged();
        }


    }
}
