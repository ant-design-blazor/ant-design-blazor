﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using AntDesign.Core.Extensions;

namespace AntDesign
{
#if NET6_0_OR_GREATER
    [CascadingTypeParameter(nameof(TItem))]
#endif
    public partial class Tree<TItem> : AntDomComponentBase
    {
        [CascadingParameter(Name = "TreeSelect")]
        public ITreeSelect TreeSelect { get; set; }

        #region fields

        /// <summary>
        /// All of the node
        /// </summary>
        internal List<TreeNode<TItem>> _allNodes = new List<TreeNode<TItem>>();

        /// <summary>
        /// All the checked nodes
        /// </summary>
        private ConcurrentDictionary<long, TreeNode<TItem>> _checkedNodes = new ConcurrentDictionary<long, TreeNode<TItem>>();

        private bool _nodeHasChanged;

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

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// tree childnodes
        /// Add values when the node is initialized
        /// </summary>
        internal List<TreeNode<TItem>> ChildNodes { get; set; } = new List<TreeNode<TItem>>();

        /// <summary>
        /// Add a node
        /// </summary>
        /// <param name="treeNode"></param>
        internal void AddChildNode(TreeNode<TItem> treeNode)
        {
            treeNode.NodeIndex = ChildNodes.Count;
            ChildNodes.Add(treeNode);
        }

        internal void AddNode(TreeNode<TItem> treeNode)
        {
            _allNodes.Add(treeNode);
            _nodeHasChanged = true;
            CallAfterRender(() =>
            {
                if (_nodeHasChanged)
                {
                    _nodeHasChanged = false;
                    TreeSelect?.UpdateValueAfterDataSourceChanged();
                }
                return Task.CompletedTask;
            });
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

        [Parameter]
        public string DefaultSelectedKey
        {
            get => DefaultSelectedKeys?.FirstOrDefault();
            set
            {
                if (value == null)
                    DefaultSelectedKeys = [];
                DefaultSelectedKeys = [value];
            }
        }

        /// <summary>
        /// Trigger the event OnSelect
        /// </summary>
        /// <param name="treeNode"></param>
        internal void TriggerOnSelect(TreeNode<TItem> treeNode)
        {

            if (OnSelect.HasDelegate)
            {
                OnSelect.InvokeAsync(new TreeEventArgs<TItem>(this, treeNode));
            }
        }

        /// <summary>
        /// Trigger the event OnUnselect
        /// </summary>
        /// <param name="treeNode"></param>
        internal void TriggerOnUnselect(TreeNode<TItem> treeNode)
        {

            if (OnUnselect.HasDelegate)
            {
                OnUnselect.InvokeAsync(new TreeEventArgs<TItem>(this, treeNode));
            }
        }

        /// <summary>
        /// Deselect all selections
        /// </summary>
        public void DeselectAll()
        {
            if (!Selectable)
                return;
            DoDeselectAll(true);
            UpdateSelectedKeys();
            StateHasChanged();
        }

        internal void DoDeselectAll(bool isManual)
        {
            foreach (var item in _allNodes.Where(r => r.Selected))
            {
                item.DoSelect(false, false, isManual);
            }
        }

        /// <summary>
        /// @bind-SelectedKey
        /// </summary>
        [Parameter]
        public string SelectedKey
        {
            get => _selectedKey;
            set => _selectedKey = value;
        }

        private string _selectedKey;

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public EventCallback<string> SelectedKeyChanged { get; set; }

        /// <summary>
        /// @bind-SelectedNode
        /// </summary>
        [Parameter]
        public TreeNode<TItem> SelectedNode
        {
            get => _selectedNode;
            set => _selectedNode = value;
        }

        private TreeNode<TItem> _selectedNode;

        [Parameter]
        public EventCallback<TreeNode<TItem>> SelectedNodeChanged { get; set; }

        /// <summary>
        /// @bing-SelectedData
        /// </summary>
        [Parameter]
        public TItem SelectedData
        {
            get => _selectedData;
            set => _selectedData = value;
        }

        private TItem _selectedData;

        [Parameter]
        public EventCallback<TItem> SelectedDataChanged { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public string[] SelectedKeys
        {
            get => _selectedKeys;
            set => _selectedKeys = value;
        }

        private string[] _selectedKeys;

        [Parameter]
        public EventCallback<string[]> SelectedKeysChanged { get; set; }

        internal string[] CachedSelectedKeys { get; set; }

        /// <summary>
        /// The collection of selected nodes
        /// </summary>
        [Parameter]
        public TreeNode<TItem>[] SelectedNodes
        {
            get => _selectedNodes;
            set => _selectedNodes = value;
        }

        private TreeNode<TItem>[] _selectedNodes;

        [Parameter]
        public EventCallback<TreeNode<TItem>[]> SelectedNodesChanged { get; set; }

        /// <summary>
        /// The selected data set
        /// </summary>
        [Parameter]
        public TItem[] SelectedDatas
        {
            get => _selectedDatas;
            set => _selectedDatas = value;
        }

        private TItem[] _selectedDatas;

        [Parameter]
        public EventCallback<TItem[]> SelectedDatasChanged { get; set; }

        /// <summary>
        /// Update binding data
        /// </summary>
        internal void UpdateSelectedKeys()
        {
            var selectedNodes = _allNodes.Where(r => r.Selected).ToList();
            if (selectedNodes.Count == 0)
            {
                ResetSelectedKeys();
            }
            else
            {
                var selectedFirst = selectedNodes.FirstOrDefault();
                _selectedKey = selectedFirst?.Key;
                _selectedNode = selectedFirst;
                _selectedData = selectedFirst.DataItem;
                _selectedKeys = selectedNodes.Select(x => x.Key).ToArray();
                _selectedNodes = [.. selectedNodes];
                _selectedDatas = selectedNodes.Select(x => x.DataItem).ToArray();
            }

            if (SelectedKeyChanged.HasDelegate) SelectedKeyChanged.InvokeAsync(_selectedKey);
            if (SelectedNodeChanged.HasDelegate) SelectedNodeChanged.InvokeAsync(_selectedNode);
            if (SelectedDataChanged.HasDelegate) SelectedDataChanged.InvokeAsync(_selectedData);
            if (SelectedKeysChanged.HasDelegate) SelectedKeysChanged.InvokeAsync(_selectedKeys);
            if (SelectedNodesChanged.HasDelegate) SelectedNodesChanged.InvokeAsync(_selectedNodes);
            if (SelectedDatasChanged.HasDelegate) SelectedDatasChanged.InvokeAsync(_selectedDatas);
        }

        private void ResetSelectedKeys()
        {
            _selectedKey = null;
            _selectedNode = null;
            _selectedData = default;
            _selectedKeys = [];
            _selectedNodes = [];
            _selectedDatas = [];
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

        private string[] _checkedKeys = [];

        /// <summary>
        /// Checked  keys
        /// </summary>
        [Parameter]
        public string[] CheckedKeys
        {
            get => _checkedKeys;
            set => _checkedKeys = value;
        }

        internal string[] CachedCheckedKeys { get; set; }

        /// <summary>
        ///  @bind-CheckedKeys
        /// </summary>
        [Parameter]
        public EventCallback<string[]> CheckedKeysChanged { get; set; }

        internal void UpdateCheckedKeys()
        {
            _checkedKeys = _allNodes.Where(r => r.Checked).Select(r => r.Key).ToArray();
            if (CheckedKeysChanged.HasDelegate) CheckedKeysChanged.InvokeAsync(_checkedKeys);
        }

        public void CheckAll()
        {
            foreach (var item in ChildNodes)
            {
                item.DoCheck(true, false, true);
            }
            UpdateCheckedKeys();
            StateHasChanged();
        }

        /// <summary>
        /// Unchecks all nodes
        /// </summary>
        public void UncheckAll()
        {
            foreach (var item in ChildNodes)
            {
                item.DoCheck(false, false, true);
            }
            UpdateCheckedKeys();
            StateHasChanged();
        }

        public void SelectAll()
        {
            if (!Selectable || !Multiple)
                return;
            foreach (var item in ChildNodes)
            {
                item.DoSelect(true, true, true);
            }
            UpdateSelectedKeys();
            StateHasChanged();
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
                if (value != _searchValue)
                {
                    _searchValue = value;
                    SearchNodes();
                }
            }
        }

        [Parameter]
        public Func<TreeNode<TItem>, bool> SearchExpression { get; set; }

        /// <summary>
        /// Search for matching text styles
        /// </summary>
        [Parameter]
        public string MatchedStyle { get; set; } = "";

        [Parameter]
        public string MatchedClass { get; set; }

        [Parameter]
        public bool HideUnmatched { get; set; }

        private void SearchNodes()
        {
            if (string.IsNullOrWhiteSpace(_searchValue))
            {
                _allNodes.ForEach(m => { _ = m.Expand(true); m.Matched = false; m.Hidden = false; });
                return;
            }

            var allList = _allNodes.ToList();
            List<TreeNode<TItem>> searchDatas = null, exceptList = null;

            if (SearchExpression != null)
            {
                searchDatas = allList.Where(x => SearchExpression(x)).ToList();
            }
            else if (!string.IsNullOrWhiteSpace(_searchValue))
            {
                searchDatas = allList.Where(x => x.Title.Contains(_searchValue, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (searchDatas != null && searchDatas.Any())
                exceptList = allList.Except(searchDatas).ToList();

            if (exceptList?.Any() == true || searchDatas?.Any() == true)
            {
                exceptList?.ForEach(m =>
                {
                    _ = m.Expand(false);
                    m.Matched = false;
                    if (HideUnmatched)
                        m.Hidden = true;
                });

                foreach (TreeNode<TItem> matchedNode in searchDatas)
                {
                    matchedNode.OpenPropagation(true);
                    matchedNode.Matched = true;
                }
            }
            else
            {
                var hide = HideUnmatched && !string.IsNullOrWhiteSpace(_searchValue);
                var expand = DefaultExpandAll && string.IsNullOrWhiteSpace(_searchValue);
                allList.ForEach(m =>
                {
                    _ = m.Expand(expand);
                    m.Matched = false;
                    m.Hidden = hide;
                });
            }
        }

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
        public Func<TreeNode<TItem>, IEnumerable<TItem>> ChildrenExpression { get; set; }

        /// <summary>
        /// Specifies a method to return a disabled node
        /// </summary>
        [Parameter]
        public Func<TreeNode<TItem>, bool> DisabledExpression { get; set; }

        /// <summary>
        /// Specifies a method to return a checkable node
        /// </summary>
        [Parameter]
        public Func<TreeNode<TItem>, bool> CheckableExpression { get; set; }

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

        [Parameter]
        public EventCallback<TreeEventArgs<TItem>> OnUnselect { get; set; }

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

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            IsSelectedKeysChanged(parameters, out var newSelectedKeys);
            if (!parameters.IsParameterChanged(nameof(CheckedKeys), CheckedKeys, out var newCheckedKeys))
                newCheckedKeys = null;
            if (!parameters.IsParameterChanged(nameof(ExpandedKeys), ExpandedKeys, out var newExpandedKeys))
                newExpandedKeys = null;
            UpdateKeysByDataSource(parameters, ref newSelectedKeys, ref newCheckedKeys, ref newExpandedKeys);

            await base.SetParametersAsync(parameters);

            if (newExpandedKeys != null)
            {
                if (_allNodes.Count == 0)
                {
                    CachedExpandedKeys = newExpandedKeys;
                }
                else
                {
                    _allNodes.ForEach(n => _ = n.DoExpand(newExpandedKeys != null && newExpandedKeys.Contains(n.Key)));
                    await UpdateExpandedKeys();
                }
            }
            if (newSelectedKeys != null && Selectable)
            {
                if (_allNodes.Count == 0)
                {
                    CachedSelectedKeys = newSelectedKeys;
                }
                else
                {
                    _allNodes.ForEach(n => n.DoSelect(newSelectedKeys != null && newSelectedKeys.Contains(n.Key), Multiple, false));
                    UpdateSelectedKeys();
                }
            }
            if (newCheckedKeys != null && Checkable)
            {
                if (_allNodes.Count == 0)
                {
                    CachedCheckedKeys = newCheckedKeys;
                }
                else
                {
                    _allNodes.ForEach(n => n.DoCheck(false, true, false));
                    newCheckedKeys.ForEach(key => _allNodes.Where(n => n.Key == key).First()?.DoCheck(true, false, false));
                    UpdateCheckedKeys();
                }
            }
        }

        private bool _updateAllKeysAfterRender = true;

        private void UpdateKeysByDataSource(ParameterView parameters, ref string[] newSelectedKeys, ref string[] newCheckedKeys, ref string[] newExpandedKeys)
        {
            if (parameters.IsParameterChanged(nameof(DataSource), DataSource))
            {
                // SelectedKeys
                if (newSelectedKeys != null)
                {
                    CachedSelectedKeys = newSelectedKeys;
                    newSelectedKeys = null;
                }
                else
                {
                    CachedSelectedKeys = GetReassignedSelectedKeys(parameters);
                }
                // CheckedKeys
                if (newCheckedKeys != null)
                {
                    CachedCheckedKeys = newCheckedKeys;
                    newCheckedKeys = null;
                }
                else
                {
                    if (IsParameterReassigned(parameters, nameof(CheckedKeys), CheckedKeys))
                        CachedCheckedKeys = CheckedKeys;
                    else
                        CachedCheckedKeys = null;
                }
                // ExpandedKeys
                if (newExpandedKeys != null)
                {
                    CachedExpandedKeys = newExpandedKeys;
                    newExpandedKeys = null;
                }
                else
                {
                    if (IsParameterReassigned(parameters, nameof(ExpandedKeys), ExpandedKeys))
                        CachedExpandedKeys = ExpandedKeys;
                    else
                        CachedExpandedKeys = null;
                }
                _allNodes.Clear();
                ChildNodes.Clear();
                ResetSelectedKeys();
                CheckedKeys = [];
                ExpandedKeys = [];
                _updateAllKeysAfterRender = true;
            }
        }

        private bool IsParameterReassigned<T>(ParameterView parameters,
            string parameterName, T value)
        {
            var isChanged = parameters.IsParameterChanged(parameterName, value, out var newValue);
            return !isChanged && newValue != null;
        }

        private string[] GetReassignedSelectedKeys(ParameterView parameters)
        {
            if (IsParameterReassigned(parameters, nameof(SelectedKeys), SelectedKeys))
            {
                return SelectedKeys;
            }
            if (IsParameterReassigned(parameters, nameof(SelectedKey), SelectedKey))
            {
                return [SelectedKey];
            }
            return null;
        }

        private bool IsSelectedKeysChanged(ParameterView parameters, out string[] newSelectedKeys)
        {
            if (parameters.IsParameterChanged(nameof(SelectedKeys), SelectedKeys, out newSelectedKeys))
            {
                return true;
            }
            else if (parameters.IsParameterChanged(nameof(SelectedKey), SelectedKey, out var newSelectedKey))
            {
                if (newSelectedKey == null)
                    newSelectedKeys = null;
                else
                    newSelectedKeys = [newSelectedKey];
                return true;
            }
            newSelectedKeys = null;
            return false;
        }

        /// <summary>
        /// Get TreeNode from Key
        /// </summary>
        /// <param name="key">Key</param>
        public TreeNode<TItem> GetNode(string key) => _allNodes.FirstOrDefault(x => x.Key == key);

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

        internal string[] CachedExpandedKeys { get; set; }

        internal async Task UpdateExpandedKeys()
        {
            ExpandedKeys = _allNodes.Where(x => x.Expanded).Select(x => x.Key).ToArray();
            if (ExpandedKeysChanged.HasDelegate)
            {
                await ExpandedKeysChanged.InvokeAsync(ExpandedKeys);
            }
        }

        [Parameter]
        public EventCallback<string[]> ExpandedKeysChanged { get; set; }

        [Parameter]
        public EventCallback<(string[] ExpandedKeys, TreeNode<TItem> Node, bool Expanded)> OnExpand { get; set; }

        [Parameter]
        public bool AutoExpandParent { get; set; }

        /// <summary>
        /// Expand all nodes
        /// </summary>
        public void ExpandAll(Func<TreeNode<TItem>, bool> predicate = null, bool recursive = true)
        {
            if (predicate != null)
                _ = FindFirstOrDefaultNode(predicate, recursive).ExpandAll();
            else
                ChildNodes.ForEach(node => _ = node.ExpandAll());
        }

        /// <summary>
        /// Collapse all nodes
        /// </summary>
        public void CollapseAll(Func<TreeNode<TItem>, bool> predicate = null, bool recursive = true)
        {
            if (predicate != null)
                _ = FindFirstOrDefaultNode(predicate, recursive).CollapseAll();
            else
                ChildNodes.ForEach(node => _ = node.CollapseAll());
        }

        internal async Task OnNodeExpand(TreeNode<TItem> node, bool expanded, MouseEventArgs args)
        {
            var expandedKeys = _allNodes.Where(x => x.Expanded).Select(x => x.Key).ToArray();
            if (OnNodeLoadDelayAsync.HasDelegate && expanded == true)
            {
                node.SetLoading(true);
                await OnNodeLoadDelayAsync.InvokeAsync(new TreeEventArgs<TItem>(this, node, args));
                node.SetLoading(false);
                StateHasChanged();
            }

            if (OnExpandChanged.HasDelegate)
            {
                await OnExpandChanged.InvokeAsync(new TreeEventArgs<TItem>(this, node, args));
            }

            if (OnExpand.HasDelegate)
            {
                await OnExpand.InvokeAsync((expandedKeys, node, expanded));
            }

            if (AutoExpandParent && expanded)
            {
                node.ParentNode?.DoExpand(true);
            }
        }

        #endregion Expand

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        internal bool IsCtrlKeyDown { get; set; } = false;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (TreeSelect is not null)
                {
                    IsCtrlKeyDown = true;
                }
                else
                {
                    DomEventListener.AddShared<KeyboardEventArgs>("document", "keydown", OnKeyDown);
                    DomEventListener.AddShared<KeyboardEventArgs>("document", "keyup", OnKeyUp);
                }
            }
            if (_updateAllKeysAfterRender)
            {
                // Make sure keys updated after all nodes' OnInitialized.
                if (_allNodes.FirstOrDefault(n => n.Selected) != null)
                    UpdateSelectedKeys();
                if (_allNodes.FirstOrDefault(n => n.Checked) != null)
                    UpdateCheckedKeys();
                if (_allNodes.FirstOrDefault(n => n.Expanded) != null)
                    _ = UpdateExpandedKeys();
                _updateAllKeysAfterRender = false;
            }

            base.OnAfterRender(firstRender);
        }

        protected virtual void OnKeyDown(KeyboardEventArgs eventArgs)
        {
            HanldeCtrlKeyPress(eventArgs);
        }

        protected virtual void OnKeyUp(KeyboardEventArgs eventArgs)
        {
            HanldeCtrlKeyPress(eventArgs);
        }

        private void HanldeCtrlKeyPress(KeyboardEventArgs eventArgs)
        {
            IsCtrlKeyDown = eventArgs.CtrlKey || eventArgs.MetaKey;
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener?.Dispose();

            base.Dispose(disposing);
        }
    }
}
