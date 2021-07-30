﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Tree<TItem> : AntDomComponentBase
    {
        #region fields

        /// <summary>
        /// All of the node
        /// </summary>
        internal List<TreeNode<TItem>> _allNodes = new List<TreeNode<TItem>>();

        /// <summary>
        /// All the checked nodes
        /// </summary>
        private ConcurrentDictionary<long, TreeNode<TItem>> _checkedNodes = new ConcurrentDictionary<long, TreeNode<TItem>>();

        #endregion fields

        #region Tree

        /// <summary>
        /// Shows an expansion icon before the node
        /// </summary>
        [Parameter]
        public bool ShowExpand { get; set; } = true;

        /// <summary>
        /// Shows a connecting line
        /// </summary>
        [Parameter]
        public bool ShowLine
        {
            get => _showLine;
            set
            {
                _showLine = value;
                if (!_hasSetShowLeafIcon)
                {
                    ShowLeafIcon = _showLine;
                }
            }
        }

        /// <summary>
        /// show treeNode icon icon
        /// </summary>
        [Parameter]
        public bool ShowIcon { get; set; }

        /// <summary>
        /// Whether treeNode fill remaining horizontal space
        /// </summary>
        [Parameter]
        public bool BlockNode { get; set; }

        /// <summary>
        /// Whether the node allows drag and drop
        /// </summary>
        [Parameter]
        public bool Draggable { get; set; }

        /// <summary>
        /// The tree is disabled
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Displays the cotyledon icon
        /// </summary>
        [Parameter]
        public bool ShowLeafIcon
        {
            get => _showLeafIcon;
            set
            {
                _showLeafIcon = value;
                _hasSetShowLeafIcon = true;
            }
        }

        private bool _hasSetShowLeafIcon;

        /// <summary>
        /// Specific the Icon type of switcher 
        /// </summary>
        [Parameter]
        public string SwitcherIcon { get; set; }

        public bool Directory { get; set; }

        private void SetClassMapper()
        {
            ClassMapper
                .Add("ant-tree")
                .If("ant-tree-show-line", () => ShowLine)
                .If("ant-tree-icon-hide", () => ShowIcon)
                .If("ant-tree-block-node", () => BlockNode)
                .If("ant-tree-directory", () => Directory)
                .If("draggable-tree", () => Draggable)
                .If("ant-tree-unselectable", () => !Selectable)
                .If("ant-tree-rtl", () => RTL);
        }

        #endregion Tree

        #region Node

        [Parameter]
        public RenderFragment Nodes { get; set; }

        /// <summary>
        /// tree childnodes
        /// Add values when the node is initialized
        /// </summary>
        internal List<TreeNode<TItem>> ChildNodes { get; set; } = new List<TreeNode<TItem>>();

        /// <summary>
        /// Add a node
        /// </summary>
        /// <param name="treeNode"></param>
        internal void AddNode(TreeNode<TItem> treeNode)
        {
            treeNode.NodeIndex = ChildNodes.Count;
            ChildNodes.Add(treeNode);
        }

        #endregion Node

        #region Selected

        /// <summary>
        /// Whether can be selected
        /// </summary>
        [Parameter]
        public bool Selectable { get; set; } = true;

        /// <summary>
        /// Allows selecting multiple treeNodes
        /// </summary>
        [Parameter]
        public bool Multiple { get; set; }

        [Parameter]
        public string[] DefaultSelectedKeys { get; set; }

        /// <summary>
        /// The selected tree node
        /// </summary>
        internal Dictionary<long, TreeNode<TItem>> SelectedNodesDictionary { get; set; } = new Dictionary<long, TreeNode<TItem>>();

        internal List<string> SelectedTitles => SelectedNodesDictionary.Select(x => x.Value.Title).ToList();

        /// <summary>
        /// Add the selected node
        /// </summary>
        /// <param name="treeNode"></param>
        internal void SelectedNodeAdd(TreeNode<TItem> treeNode)
        {
            if (SelectedNodesDictionary.ContainsKey(treeNode.NodeId) == false)
                SelectedNodesDictionary.Add(treeNode.NodeId, treeNode);

            if (OnSelect.HasDelegate)
            {
                OnSelect.InvokeAsync(new TreeEventArgs<TItem>(this, treeNode));
            }

            UpdateBindData();
        }

        /// <summary>
        /// remove the selected node
        /// </summary>
        /// <param name="treeNode"></param>
        internal void SelectedNodeRemove(TreeNode<TItem> treeNode)
        {
            if (SelectedNodesDictionary.ContainsKey(treeNode.NodeId) == true)
                SelectedNodesDictionary.Remove(treeNode.NodeId);

            UpdateBindData();
        }

        /// <summary>
        /// Deselect all selections
        /// </summary>
        public void DeselectAll()
        {
            foreach (var item in SelectedNodesDictionary.Select(x => x.Value).ToList())
            {
                item.SetSelected(false);
            }
        }

        /// <summary>
        /// @bind-SelectedKey
        /// </summary>
        [Parameter]
        public string SelectedKey { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public EventCallback<string> SelectedKeyChanged { get; set; }

        /// <summary>
        /// @bind-SelectedNode
        /// </summary>
        [Parameter]
        public TreeNode<TItem> SelectedNode { get; set; }

        [Parameter]
        public EventCallback<TreeNode<TItem>> SelectedNodeChanged { get; set; }

        /// <summary>
        /// @bing-SelectedData
        /// </summary>
        [Parameter]
        public TItem SelectedData { get; set; }

        [Parameter]
        public EventCallback<TItem> SelectedDataChanged { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public string[] SelectedKeys { get; set; }

        [Parameter]
        public EventCallback<string[]> SelectedKeysChanged { get; set; }

        /// <summary>
        /// The collection of selected nodes
        /// </summary>
        [Parameter]
        public TreeNode<TItem>[] SelectedNodes { get; set; }

        /// <summary>
        /// The selected data set
        /// </summary>
        [Parameter]
        public TItem[] SelectedDatas { get; set; }

        /// <summary>
        /// Update binding data
        /// </summary>
        private void UpdateBindData()
        {
            if (SelectedNodesDictionary.Count == 0)
            {
                SelectedKey = null;
                SelectedNode = null;
                SelectedData = default(TItem);
                SelectedKeys = Array.Empty<string>();
                SelectedNodes = Array.Empty<TreeNode<TItem>>();
                SelectedDatas = Array.Empty<TItem>();
            }
            else
            {
                var selectedFirst = SelectedNodesDictionary.FirstOrDefault();
                SelectedKey = selectedFirst.Value?.Key;
                SelectedNode = selectedFirst.Value;
                SelectedData = selectedFirst.Value.DataItem;
                SelectedKeys = SelectedNodesDictionary.Select(x => x.Value.Key).ToArray();
                SelectedNodes = SelectedNodesDictionary.Select(x => x.Value).ToArray();
                SelectedDatas = SelectedNodesDictionary.Select(x => x.Value.DataItem).ToArray();
            }

            if (SelectedKeyChanged.HasDelegate) SelectedKeyChanged.InvokeAsync(SelectedKey);
            if (SelectedNodeChanged.HasDelegate) SelectedNodeChanged.InvokeAsync(SelectedNode);
            if (SelectedDataChanged.HasDelegate) SelectedDataChanged.InvokeAsync(SelectedData);
            if (SelectedKeysChanged.HasDelegate) SelectedKeysChanged.InvokeAsync(SelectedKeys);
        }

        #endregion Selected

        #region Checkable

        /// <summary>
        /// Add a Checkbox before the node
        /// </summary>
        [Parameter]
        public bool Checkable { get; set; }

        /// <summary>
        /// Check treeNode precisely; parent treeNode and children treeNodes are not associated
        /// </summary>
        [Parameter]
        public bool CheckStrictly { get; set; }

        /// <summary>
        /// Checked  keys
        /// </summary>
        [Parameter]
        public string[] CheckedKeys { get; set; } = Array.Empty<string>();

        /// <summary>
        ///  @bind-CheckedKeys
        /// </summary>
        [Parameter]
        public EventCallback<string[]> CheckedKeysChanged { get; set; }

        /// <summary>
        /// Dechecked all selected items
        /// </summary>
        public void CheckedAll()
        {
            foreach (var item in ChildNodes)
            {
                item.SetChecked(true);
            }
        }

        // Decheck all of the checked nodes
        public void DecheckedAll()
        {
            foreach (var item in ChildNodes)
            {
                item.SetChecked(false);
            }
        }

        /// <summary>
        /// Specifies the keys of the default checked treeNodes
        /// </summary>
        [Parameter]
        public string[] DefaultCheckedKeys { get; set; }

        /// <summary>
        /// Disable node Checkbox
        /// </summary>
        public string[] DisableCheckKeys { get; set; }

        /// <summary>
        /// Adds or removes a checkbox node
        /// </summary>
        /// <param name="treeNode"></param>
        internal void AddOrRemoveCheckNode(TreeNode<TItem> treeNode)
        {
            if (treeNode.Checked)
                _checkedNodes.TryAdd(treeNode.NodeId, treeNode);
            else
                _checkedNodes.TryRemove(treeNode.NodeId, out TreeNode<TItem> _);
            CheckedKeys = _checkedNodes.Select(x => x.Value.Key).ToArray();
            if (CheckedKeysChanged.HasDelegate) CheckedKeysChanged.InvokeAsync(CheckedKeys);
        }

        #endregion Checkable

        #region Search

        private string _searchValue;
        private bool _showLeafIcon;
        private bool _showLine;

        /// <summary>
        /// search value
        /// </summary>
        [Parameter]
        public string SearchValue
        {
            get => _searchValue;
            set
            {
                _searchValue = value;
                var allList = _allNodes.ToList();
                List<TreeNode<TItem>> searchDatas = null, exceptList = null;
                if (!String.IsNullOrEmpty(value))
                    searchDatas = allList.Where(x => x.Title.Contains(value)).ToList();
                if (searchDatas != null && searchDatas.Any())
                    exceptList = allList.Except(searchDatas).ToList();
                if (exceptList != null || searchDatas != null)
                {
                    exceptList?.ForEach(m => { m.Expand(false); m.Matched = false; });
                    searchDatas?.ForEach(node => { node.OpenPropagation(); node.Matched = true; });
                }
                else
                {
                    allList.ForEach(m => { m.Matched = false; });
                }
            }
        }

        ///// <summary>
        /////
        ///// </summary>
        //[Parameter]
        //public EventCallback<TreeEventArgs<TItem>> OnSearchValueChanged { get; set; }

        /// <summary>
        /// Search for matching text styles
        /// </summary>
        [Parameter]
        public string MatchedStyle { get; set; } = "";

        #endregion Search

        #region DataBind

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public IEnumerable<TItem> DataSource { get; set; }

        /// <summary>
        /// Specifies a method that returns the text of the node.
        /// </summary>
        [Parameter]
        public Func<TreeNode<TItem>, string> TitleExpression { get; set; }

        /// <summary>
        /// Specifies a method that returns the key of the node.
        /// </summary>
        [Parameter]
        public Func<TreeNode<TItem>, string> KeyExpression { get; set; }

        /// <summary>
        /// Specifies a method to return the node icon.
        /// </summary>
        [Parameter]
        public Func<TreeNode<TItem>, string> IconExpression { get; set; }

        /// <summary>
        /// Specifies a method that returns whether the expression is a leaf node.
        /// </summary>
        [Parameter]
        public Func<TreeNode<TItem>, bool> IsLeafExpression { get; set; }

        /// <summary>
        /// Specifies a method  to return a child node
        /// </summary>
        [Parameter]
        public Func<TreeNode<TItem>, IList<TItem>> ChildrenExpression { get; set; }

        /// <summary>
        /// Specifies a method to return a disabled node
        /// </summary>
        [Parameter]
        public Func<TreeNode<TItem>, bool> DisabledExpression { get; set; }

        #endregion DataBind

        #region Event

        /// <summary>
        /// Lazy load callbacks
        /// </summary>
        /// <remarks>You must use async and the return type is Task, otherwise you may experience load lag and display problems</remarks>
        [Parameter]
        public EventCallback<TreeEventArgs<TItem>> OnNodeLoadDelayAsync { get; set; }

        /// <summary>
        /// Click the tree node callback
        /// </summary>
        [Parameter]
        public EventCallback<TreeEventArgs<TItem>> OnClick { get; set; }

        /// <summary>
        /// Double-click the node callback
        /// </summary>
        [Parameter]
        public EventCallback<TreeEventArgs<TItem>> OnDblClick { get; set; }

        /// <summary>
        /// Right-click tree node callback
        /// </summary>
        [Parameter]
        public EventCallback<TreeEventArgs<TItem>> OnContextMenu { get; set; }

        /// <summary>
        /// checked the tree node callback
        /// </summary>
        [Parameter]
        public EventCallback<TreeEventArgs<TItem>> OnCheck { get; set; }

        [Parameter]
        public EventCallback<TreeEventArgs<TItem>> OnSelect { get; set; }

        /// <summary>
        /// Click the expansion tree node icon to call back
        /// </summary>
        [Parameter]
        public EventCallback<TreeEventArgs<TItem>> OnExpandChanged { get; set; }

        #endregion Event

        #region Template

        /// <summary>
        /// The indentation template
        /// </summary>
        [Parameter]
        public RenderFragment<TreeNode<TItem>> IndentTemplate { get; set; }

        /// <summary>
        /// Customize the header template
        /// </summary>
        [Parameter]
        public RenderFragment<TreeNode<TItem>> TitleTemplate { get; set; }

        /// <summary>
        ///  Customize the icon templates
        /// </summary>
        [Parameter]
        public RenderFragment<TreeNode<TItem>> TitleIconTemplate { get; set; }

        /// <summary>
        /// Customize toggle icon templates
        /// </summary>
        [Parameter]
        public RenderFragment<TreeNode<TItem>> SwitcherIconTemplate { get; set; }

        #endregion Template

        #region DragDrop

        /// <summary>
        /// 当前拖拽项
        /// </summary>
        internal TreeNode<TItem> DragItem { get; set; }

        /// <summary>
        /// Called when the drag and drop begins
        /// </summary>
        [Parameter]
        public EventCallback<TreeEventArgs<TItem>> OnDragStart { get; set; }

        /// <summary>
        /// Called when drag and drop into a releasable target
        /// </summary>
        [Parameter]
        public EventCallback<TreeEventArgs<TItem>> OnDragEnter { get; set; }

        /// <summary>
        /// Called when drag and drop away from a releasable target
        /// </summary>
        [Parameter]
        public EventCallback<TreeEventArgs<TItem>> OnDragLeave { get; set; }

        /// <summary>
        /// Triggered when drag-and-drop drops succeed
        /// </summary>
        [Parameter]
        public EventCallback<TreeEventArgs<TItem>> OnDrop { get; set; }

        /// <summary>
        /// Drag-and-drop end callback
        /// </summary>
        /// <remarks>this callback method must be set</remarks>
        [Parameter]
        public EventCallback<TreeEventArgs<TItem>> OnDragEnd { get; set; }

        #endregion DragDrop

        protected override void OnInitialized()
        {
            SetClassMapper();
            base.OnInitialized();
        }

        /// <summary>
        /// Find Node
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <param name="recursive">Recursive Find</param>
        /// <returns></returns>
        public TreeNode<TItem> FindFirstOrDefaultNode(Func<TreeNode<TItem>, bool> predicate, bool recursive = true)
        {
            foreach (var child in ChildNodes)
            {
                if (predicate != null && predicate.Invoke(child))
                {
                    return child;
                }

                if (recursive)
                {
                    var find = child.FindFirstOrDefaultNode(predicate, recursive);
                    if (find != null)
                    {
                        return find;
                    }
                }
            }
            return null;
        }

        #region Expand

        /// <summary>
        /// All tree nodes are expanded by default
        /// </summary>
        [Parameter]
        public bool DefaultExpandAll { get; set; }

        /// <summary>
        /// The parent node is expanded by default
        /// </summary>
        [Parameter]
        public bool DefaultExpandParent { get; set; }

        /// <summary>
        /// Expand the specified tree node by default
        /// </summary>
        [Parameter]
        public string[] DefaultExpandedKeys { get; set; }

        /// <summary>
        /// (Controlled) expands the specified tree node
        /// </summary>
        [Parameter]
        public string[] ExpandedKeys { get; set; }

        [Parameter]
        public EventCallback<string[]> ExpandedKeysChanged { get; set; }

        [Parameter]
        public EventCallback<(string[] ExpandedKeys, TreeNode<TItem> Node, bool Expanded)> OnExpand { get; set; }

        [Parameter]
        public bool AutoExpandParent { get; set; }

        /// <summary>
        /// Expand all nodes
        /// </summary>
        public void ExpandAll()
        {
            this.ChildNodes.ForEach(node => Switch(node, true));
        }

        /// <summary>
        /// Collapse all nodes
        /// </summary>
        public void CollapseAll()
        {
            this.ChildNodes.ForEach(node => Switch(node, false));
        }

        /// <summary>
        /// 节点展开关闭
        /// </summary>
        /// <param name="node"></param>
        /// <param name="expanded"></param>
        private void Switch(TreeNode<TItem> node, bool expanded)
        {
            node.Expand(expanded);
            node.ChildNodes.ForEach(n => Switch(n, expanded));
        }

        internal async Task OnNodeExpand(TreeNode<TItem> node, bool expanded, MouseEventArgs args)
        {
            var expandedKeys = _allNodes.Select(x => x.Key).ToArray();
            if (OnNodeLoadDelayAsync.HasDelegate && expanded == true)
            {
                node.SetLoading(true);
                await OnNodeLoadDelayAsync.InvokeAsync(new TreeEventArgs<TItem>(this, node, args));
                node.SetLoading(false);
            }

            if (OnExpandChanged.HasDelegate)
            {
                await OnExpandChanged.InvokeAsync(new TreeEventArgs<TItem>(this, node, args));
            }

            if (ExpandedKeysChanged.HasDelegate)
            {
                await ExpandedKeysChanged.InvokeAsync(expandedKeys);
            }

            if (OnExpand.HasDelegate)
            {
                await OnExpand.InvokeAsync((expandedKeys, node, expanded));
            }

            if (AutoExpandParent && expanded)
            {
                node.ParentNode?.Expand(true);
            }
        }

        #endregion Expand
    }
}
