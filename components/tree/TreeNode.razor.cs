// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.Core.Documentation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class TreeNode<TItem> : AntDomComponentBase
    {
        #region Node

        /// <summary>
        /// Tree
        /// </summary>
        [CascadingParameter(Name = "Tree")]
        private Tree<TItem> TreeComponent { get; set; }

        /// <summary>
        /// Parent Node
        /// </summary>
        [CascadingParameter(Name = "Node")]
        internal TreeNode<TItem> ParentNode { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public RenderFragment Nodes { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal List<TreeNode<TItem>> ChildNodes { get; set; } = new List<TreeNode<TItem>>();

        /// <summary>
        /// Whether child nodes exist
        /// </summary>
        internal bool HasChildNodes => ChildNodes?.Count > 0;

        /// <summary>
        /// Current Node Level
        /// </summary>
        [PublicApi("1.0.0")]
        public int TreeLevel => (ParentNode?.TreeLevel ?? -1) + 1;

        /// <summary>
        /// Determine if it is the last node in the same level nodes.
        /// </summary>
        internal bool IsLastNode()
        {
            if (ParentNode is not null)
            {
                var nodeIndex = ParentNode.ChildNodes.IndexOf(this);
                return nodeIndex == ParentNode.ChildNodes.Count - 1;

            }
            else
            {
                var nodeIndex = TreeComponent.ChildNodes.IndexOf(this);
                return nodeIndex == TreeComponent.ChildNodes.Count - 1;
            }
        }

        /// <summary>
        /// add node to parent node
        /// </summary>
        /// <param name="treeNode"></param>
        internal void AddNode(TreeNode<TItem> treeNode)
        {
            ChildNodes.Add(treeNode);
            IsLeaf = false;
        }

        /// <summary>
        /// Find a node
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <param name="recursive">Recursive Find</param>
        /// <returns></returns>
        [PublicApi("1.0.0")]
        public TreeNode<TItem> FindFirstOrDefaultNode(Func<TreeNode<TItem>, bool> predicate, bool recursive = true)
        {
            foreach (var child in ChildNodes)
            {
                if (predicate.Invoke(child))
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

        /// <summary>
        /// Get the sibling nodes
        /// </summary>
        /// <returns></returns>
        [PublicApi("1.0.0")]
        public List<TreeNode<TItem>> GetSiblingNodes()
        {
            if (ParentNode != null)
                return ParentNode.ChildNodes;
            else
                return TreeComponent.ChildNodes;
        }

        /// <summary>
        /// Get the previous node
        /// </summary>
        /// <returns></returns>
        [PublicApi("1.0.0")]
        public TreeNode<TItem> GetPreviousNode()
        {
            var parentNodes = GetSiblingNodes();
            var index = parentNodes.IndexOf(this);
            if (index == 0) return null;
            else return parentNodes[index - 1];
        }

        /// <summary>
        /// Get the next node 
        /// </summary>
        /// <returns></returns>
        [PublicApi("1.0.0")]
        public TreeNode<TItem> GetNextNode()
        {
            var parentNodes = GetSiblingNodes();
            var index = parentNodes.IndexOf(this);
            if (index == parentNodes.Count - 1) return null;
            else return parentNodes[index + 1];
        }

        #endregion Node

        #region TreeNode

        private static long _nextNodeId;

        internal long NodeId { get; private set; }

        public TreeNode()
        {
            NodeId = Interlocked.Increment(ref _nextNodeId);
        }

        private string _key;

        /// <summary>
        /// Specifies the unique identifier name of the current node。
        /// </summary>
        [Parameter]
        public string Key
        {
            get
            {
                if (TreeComponent.KeyExpression != null)
                    return TreeComponent.KeyExpression(this);
                else
                    return _key;
            }
            set
            {
                _key = value;
            }
        }

        private bool _disabled;

        /// <summary>
        /// The disabled state is subject to the parent node
        /// </summary>
        [Parameter]
        public bool Disabled
        {
            get { return _disabled || (ParentNode?.Disabled ?? false); }
            set { _disabled = value; }
        }

        private bool _actualSelected;

        private bool _selected;

        /// <summary>
        /// Selected or not
        /// </summary>
        [Parameter]
        public bool Selected
        {
            get => _actualSelected;
            set => _selected = value;
        }

        /// <summary>
        /// Triggered when the selected state changes
        /// </summary>
        [Parameter]
        public EventCallback<bool> SelectedChanged { get; set; }

        /// <summary>
        /// Setting Selection State
        /// </summary>
        /// <param name="value"></param>
        [PublicApi("1.0.0")]
        public void SetSelected(bool value)
        {
            if (!Selectable) return;
            DoSelect(value, false, true);
            TreeComponent.UpdateSelectedKeys();
        }

        internal void DoSelect(bool value, bool isMulti, bool isManual)
        {
            if (Disabled && !TreeComponent.Multiple)
            {
                _actualSelected = false;
            }
            else
            {
                value = (!Disabled || !isManual) ? value : _actualSelected;
                if (_actualSelected == value) return;
                if (value == true)
                {
                    if (!(TreeComponent.Multiple && (TreeComponent.IsCtrlKeyDown || isMulti)))
                    {
                        TreeComponent.DoDeselectAll(isManual);
                    }
                    TreeComponent.TriggerOnSelect(this);
                }
                else
                {
                    TreeComponent.TriggerOnUnselect(this);
                }
                _actualSelected = value;
            }
            if (SelectedChanged.HasDelegate)
                SelectedChanged.InvokeAsync(_actualSelected);
            StateHasChanged();
            return;
        }

        /// <summary>
        /// Whether the load state is asynchronous (affects the display of the expansion icon)
        /// </summary>
        [Parameter]
        public bool Loading { get; set; }

        private bool _dragTarget;

        /// <summary>
        /// Whether or not to release the target
        /// </summary>
        internal bool DragTarget
        {
            get { return _dragTarget; }
            set
            {
                _dragTarget = value;
                StateHasChanged();
            }
        }

        /// <summary>
        ///
        /// </summary>
        internal bool DragTargetBottom { get; private set; }

        /// <summary>
        /// Sets the node to release the target location
        /// </summary>
        /// <param name="value"></param>
        internal void SetTargetBottom(bool value = false)
        {
            if (DragTargetBottom == value) return;
            DragTargetBottom = value;
            StateHasChanged();
        }

        /// <summary>
        ///
        /// </summary>
        private bool TargetContainer { get; set; }

        /// <summary>
        /// Sets the drag and drop target node container
        /// </summary>
        internal void SetParentTargetContainer(bool value = false)
        {
            if (ParentNode == null) return;
            if (ParentNode.TargetContainer == value) return;
            ParentNode.TargetContainer = value;
            ParentNode.StateHasChanged();
        }

        /// <summary>
        /// Gets the children of the parent node
        /// </summary>
        /// <returns></returns>
        private List<TreeNode<TItem>> GetParentChildNodes()
        {
            return ParentNode?.ChildNodes ?? TreeComponent.ChildNodes;
        }

        private void SetTreeNodeClassMapper()
        {
            ClassMapper
                .Add("ant-tree-treenode")
                .If("ant-tree-treenode-disabled", () => Disabled)
                .If("ant-tree-treenode-switcher-open", () => SwitcherOpen)
                .If("ant-tree-treenode-switcher-close", () => SwitcherClose)
                .If("ant-tree-treenode-checkbox-checked", () => Checked)
                .If("ant-tree-treenode-checkbox-indeterminate", () => Indeterminate)
                .If("ant-tree-treenode-selected", () => Selected)
                .If("ant-tree-treenode-loading", () => Loading)
                .If("drop-target", () => DragTarget)
                .If("drag-over-gap-bottom", () => DragTarget && DragTargetBottom)
                .If("drag-over", () => DragTarget && !DragTargetBottom)
                .If("drop-container", () => TargetContainer)
                .If("ant-tree-treenode-leaf-last", () => IsLastNode());
        }

        #endregion TreeNode

        #region Switcher

        private bool _isLeaf = true;

        /// <summary>
        /// Whether it is a leaf node
        /// </summary>
        [Parameter]
        public bool IsLeaf
        {
            get
            {
                if (TreeComponent.IsLeafExpression != null)
                    return TreeComponent.IsLeafExpression(this);
                else
                    return _isLeaf;
            }
            set
            {
                if (_isLeaf == value) return;
                _isLeaf = value;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Expand the node or not
        /// </summary>
        [Parameter]
        public bool Expanded
        {
            get => _actualExpanded;
            set => _expanded = value;
        }

        private bool _actualExpanded = false;

        private bool _expanded = false;

        [Parameter]
        public EventCallback<bool> ExpandedChanged { get; set; }

        /// <summary>
        /// Expand the node
        /// </summary>
        /// <param name="expanded"></param>
        public async Task Expand(bool expanded)
        {
            await DoExpand(expanded);
            await TreeComponent?.UpdateExpandedKeys();
            StateHasChanged();
        }

        internal async Task DoExpand(bool expanded)
        {
            if (_actualExpanded == expanded)
            {
                return;
            }
            _actualExpanded = expanded;
            if (ExpandedChanged.HasDelegate)
            {
                await ExpandedChanged.InvokeAsync(_actualExpanded);
            }
            await TreeComponent?.OnNodeExpand(this, _actualExpanded, new MouseEventArgs());
        }


        /// <summary>
        /// Expand all child nodes
        /// </summary>
        internal async Task ExpandAll()
        {
            await SwitchAllNodes(this, true);
            await TreeComponent?.UpdateExpandedKeys();
        }

        /// <summary>
        /// Collapse all child nodes
        /// </summary>
        internal async Task CollapseAll()
        {
            await SwitchAllNodes(this, false);
            await TreeComponent?.UpdateExpandedKeys();
        }

        /// <summary>
        /// 节点展开关闭
        /// </summary>
        /// <param name="node"></param>
        /// <param name="expanded"></param>
        private async Task SwitchAllNodes(TreeNode<TItem> node, bool expanded)
        {
            await node.DoExpand(expanded);
            node.ChildNodes.ForEach(n => _ = SwitchAllNodes(n, expanded));
        }

        /// <summary>
        /// The real expand state, as long as there is a expaneded node on the path, then all the folds below
        /// </summary>
        internal bool RealDisplay
        {
            get
            {
                if (Hidden) return false;
                if (ParentNode == null) return true;
                if (ParentNode.Expanded == false) return false;
                return ParentNode.RealDisplay;
            }
        }

        /// <summary>
        /// Nodes switch
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private void OnSwitcherClick(MouseEventArgs args)
        {
            _ = Expand(!_actualExpanded);
        }

        internal void SetLoading(bool loading)
        {
            Loading = loading;
        }

        /// <summary>
        /// switcher is opened
        /// </summary>
        private bool SwitcherOpen => Expanded && !IsLeaf;

        /// <summary>
        /// switcher is close
        /// </summary>
        private bool SwitcherClose => !Expanded && !IsLeaf;

        /// <summary>
        /// expaned parents
        /// </summary>
        internal void OpenPropagation(bool unhide = false)
        {
            _ = DoExpand(true);
            if (unhide)
                Hidden = false;
            if (ParentNode != null)
                ParentNode.OpenPropagation(unhide);
        }

        #endregion Switcher

        #region Checkbox

        /// <summary>
        /// Check the TreeNode or not 
        /// </summary>
        [Parameter]
        public bool Checked
        {
            get
            {
                return _actualChecked;
            }
            set
            {
                _checked = value;
            }
        }

        private bool _actualChecked = false;

        private bool _checked = false;

        [Parameter]
        public EventCallback<bool> CheckedChanged { get; set; }

        private bool _checkable = true;

        [Parameter]
        public bool Checkable
        {
            get => TreeComponent.Checkable && _checkable;
            set => _checkable = value;
        }

        private bool _selectable = true;

        [Parameter]
        public bool Selectable
        {
            get => TreeComponent.Selectable && _selectable;
            set => _selectable = value;
        }

        internal bool Indeterminate { get; set; }

        private bool _disableCheckbox;

        /// <summary>
        /// Disable checkbox
        /// </summary>
        [Parameter]
        public bool DisableCheckbox
        {
            get
            {
                return _disableCheckbox || (TreeComponent?.DisableCheckKeys?.Any(k => k == Key) ?? false);
            }
            set
            {
                _disableCheckbox = value;
            }
        }

        /// <summary>
        /// Triggered when the selection box is clicked
        /// </summary>
        private async Task OnCheckBoxClick(MouseEventArgs args)
        {
            if (Disabled || DisableCheckbox)
                return;
            SetChecked(!_actualChecked);
            if (TreeComponent.OnCheck.HasDelegate)
                await TreeComponent.OnCheck.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, this, args));
        }

        /// <summary>
        /// Set the checkbox state
        /// </summary>
        /// <param name="check"></param>
        [PublicApi("1.0.0")]
        public void SetChecked(bool check)
        {
            if (!DoCheck(check, false, true)) return;
            TreeComponent.UpdateCheckedKeys();
        }

        internal bool DoCheck(bool check, bool strict, bool isManual)
        {
            if (TreeComponent.CheckStrictly || strict)
            {
                if (!Checkable)
                {
                    return false;
                }
                _actualChecked = (!Disabled || !DisableCheckbox || !isManual) ? check : _actualChecked;
                Indeterminate = false;
                NotifyCheckedChanged();
            }
            else
            {
                SetChildChecked(this, check, isManual);
                ParentNode?.UpdateCheckState();
            }
            StateHasChanged();
            return true;
        }

        internal void DoCheckAllChildren()
        {
            SetChildChecked(this, true, true, true);
            StateHasChanged();
        }

        /// <summary>
        /// Checks all child nodes
        /// </summary>
        [PublicApi("1.0.0")]
        public void CheckAllChildren()
        {
            DoCheckAllChildren();
            TreeComponent?.UpdateCheckedKeys();
        }

        internal void DoUnCheckAllChildren()
        {
            SetChildChecked(this, false, true, true);
            StateHasChanged();
        }

        /// <summary>
        /// Unchecks all child nodes
        /// </summary>
        [PublicApi("1.0.0")]
        public void UnCheckAllChildren()
        {
            DoUnCheckAllChildren();
            TreeComponent?.UpdateCheckedKeys();
        }

        /// <summary>
        /// Sets the checkbox status of child nodes
        /// </summary>
        /// <param name="subnode"></param>
        /// <param name="check"></param>
        /// <param name="isManual"></param>
        /// <param name="forceRecursive"></param>
        private void SetChildChecked(TreeNode<TItem> subnode, bool check, bool isManual, bool forceRecursive = false)
        {
            if (!Checkable && !forceRecursive)
            {
                return;
            }
            var isChecked = ((!Disabled && !DisableCheckbox) || !isManual) ? check : _actualChecked;
            var hasChecked = false;
            var hasUnChecked = false;
            var childIndeterminate = false;
            if (subnode.HasChildNodes)
            {
                foreach (var child in subnode.ChildNodes)
                {
                    child.SetChildChecked(child, check, isManual, forceRecursive);
                    if (!child.Checkable)
                        continue;
                    if (child.Checked)
                        hasChecked = true;
                    else
                        hasUnChecked = true;
                    if (child.Indeterminate)
                        childIndeterminate = true;
                }
                if (hasChecked || hasUnChecked)
                    isChecked = !hasUnChecked;
            }
            if (Checkable)
            {
                Indeterminate = childIndeterminate || (hasChecked && hasUnChecked);
                if (Indeterminate)
                    isChecked = false;
            }
            else
            {
                Indeterminate = false;
                isChecked = false;
            }
            _actualChecked = isChecked;
            NotifyCheckedChanged();
        }

        /// <summary>
        /// Update check status
        /// </summary>
        /// <param name="halfChecked"></param>
        private void UpdateCheckState(bool? halfChecked = null)
        {
            if (!Checkable) return;
            if (halfChecked == true)
            {
                //If the child node is indeterminate, the parent node must is indeterminate.
                _actualChecked = false;
                Indeterminate = true;
            }
            else if (HasChildNodes == true)
            {
                //Determines the selection status of the current node
                bool hasChecked = false;
                bool hasUnchecked = false;

                foreach (var item in ChildNodes)
                {
                    if (!item.Checkable) continue;
                    if (item.Indeterminate)
                    {
                        hasChecked = true;
                        hasUnchecked = true;
                        break;
                    }
                    else if (item._actualChecked)
                    {
                        hasChecked = true;
                    }
                    else if (!item._actualChecked)
                    {
                        hasUnchecked = true;
                    }
                }

                if (hasChecked && !hasUnchecked)
                {
                    _actualChecked = true;
                    Indeterminate = false;
                }
                else if (!hasChecked && hasUnchecked)
                {
                    _actualChecked = false;
                    Indeterminate = false;
                }
                else if (hasChecked && hasUnchecked)
                {
                    _actualChecked = false;
                    Indeterminate = true;
                }
            }
            NotifyCheckedChanged();

            if (ParentNode != null)
                ParentNode.UpdateCheckState(Indeterminate);

            if (ParentNode == null)
                StateHasChanged();
        }

        private void NotifyCheckedChanged()
        {
            if (CheckedChanged.HasDelegate)
                CheckedChanged.InvokeAsync(_actualChecked);
        }

        #endregion Checkbox

        #region Title

        /// <summary>
        /// Whether the node is draggable
        /// </summary>
        [Parameter]
        public bool Draggable { get; set; }

        private string _icon;

        /// <summary>
        /// The icon in front of the node
        /// </summary>
        [Parameter]
        public string Icon
        {
            get
            {
                if (TreeComponent.IconExpression != null)
                    return TreeComponent.IconExpression(this);
                else
                    return _icon;
            }
            set
            {
                _icon = value;
            }
        }

        /// <summary>
        /// Customize the icon template
        /// </summary>
        [Parameter]
        public RenderFragment<TreeNode<TItem>> IconTemplate { get; set; }

        /// <summary>
        /// Specific the icon of the switcher
        /// </summary>
        [Parameter]
        public string SwitcherIcon { get; set; }

        /// <summary>
        /// Customize the switcher icon template
        /// </summary>
        [Parameter]
        public RenderFragment<TreeNode<TItem>> SwitcherIconTemplate { get; set; }

        private string _title;

        /// <summary>
        /// The title of the node
        /// </summary>
        [Parameter]
        public string Title
        {
            get
            {
                if (TreeComponent.TitleExpression != null)
                    return TreeComponent.TitleExpression(this);
                else
                    return _title;
            }
            set
            {
                _title = value;
            }
        }

        /// <summary>
        /// Customize the title template
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// title是否包含SearchValue(搜索使用)
        /// </summary>
        internal bool Matched { get; set; }

        internal bool Hidden { get; set; }

        /// <summary>
        /// 子节点存在满足搜索条件，所以夫节点也需要显示
        /// </summary>
        internal bool HasChildMatched { get; set; }

        #endregion Title

        #region Data Binding

        /// <summary>
        /// The data of the node, it's the data item in the data source
        /// </summary>
        [Parameter]
        public TItem DataItem { get; set; }

        private IList<TItem> ChildDataItems
        {
            get
            {
                if (TreeComponent.ChildrenExpression != null)
                {
                    var childItems = TreeComponent.ChildrenExpression(this);
                    if (childItems is IList<TItem> list)
                    {
                        return list;
                    }
                    return childItems?.ToList() ?? new List<TItem>();
                }
                else
                    return new List<TItem>();
            }
        }

        /// <summary>
        /// 获得上级数据集合
        /// </summary>
        /// <returns></returns>
        internal IList<TItem> GetParentChildDataItems()
        {
            if (ParentNode != null)
                return ParentNode.ChildDataItems;
            else
                return TreeComponent.DataSource as IList<TItem> ?? TreeComponent.DataSource.ToList();
        }

        #endregion Data Binding

        #region Node data operation

        /// <summary>
        /// Add child node
        /// </summary>
        /// <param name="dataItem"></param>
        internal void AddChildNode(TItem dataItem)
        {
            ChildDataItems.Add(dataItem);
        }

        /// <summary>
        /// Add a node next the node
        /// </summary>
        /// <param name="dataItem"></param>
        internal void AddNextNode(TItem dataItem)
        {
            var parentChildDataItems = GetParentChildDataItems();
            var index = parentChildDataItems.IndexOf(DataItem);
            parentChildDataItems.Insert(index + 1, dataItem);

            AddNodeAndSelect(dataItem);
        }

        /// <summary>
        /// Add a node before the node
        /// </summary>
        /// <param name="dataItem"></param>
        internal void AddPreviousNode(TItem dataItem)
        {
            var parentChildDataItems = GetParentChildDataItems();
            var index = parentChildDataItems.IndexOf(DataItem);
            parentChildDataItems.Insert(index, dataItem);

            AddNodeAndSelect(dataItem);
        }

        /// <summary>
        /// remove
        /// </summary>
        internal void Remove()
        {
            var parentChildDataItems = GetParentChildDataItems();
            parentChildDataItems.Remove(DataItem);
        }

        /// <summary>
        /// The node moves into the child node
        /// </summary>
        /// <param name="treeNode">target node</param>
        internal void MoveInto(TreeNode<TItem> treeNode)
        {
            if (treeNode == this || DataItem.Equals(treeNode.DataItem)) return;
            var parentChildDataItems = GetParentChildDataItems();
            parentChildDataItems.Remove(DataItem);
            treeNode.AddChildNode(DataItem);
        }

        /// <summary>
        /// Move up the nodes
        /// </summary>
        internal void MoveUp()
        {
            var parentChildDataItems = GetParentChildDataItems();
            var index = parentChildDataItems.IndexOf(DataItem);
            if (index == 0) return;
            parentChildDataItems.RemoveAt(index);
            parentChildDataItems.Insert(index - 1, DataItem);
        }

        /// <summary>
        /// Move down the node
        /// </summary>
        internal void MoveDown()
        {
            var parentChildDataItems = GetParentChildDataItems();
            var index = parentChildDataItems.IndexOf(DataItem);
            if (index == parentChildDataItems.Count - 1) return;
            parentChildDataItems.RemoveAt(index);
            parentChildDataItems.Insert(index + 1, DataItem);
        }

        /// <summary>
        ///
        /// </summary>
        internal void Downgrade()
        {
            var previousNode = GetPreviousNode();
            if (previousNode == null) return;
            var parentChildDataItems = GetParentChildDataItems();
            parentChildDataItems.Remove(DataItem);
            previousNode.AddChildNode(DataItem);
        }

        /// <summary>
        /// Upgrade nodes
        /// </summary>
        internal void Upgrade()
        {
            if (ParentNode == null) return;
            var parentChildDataItems = ParentNode.GetParentChildDataItems();
            var index = parentChildDataItems.IndexOf(ParentNode.DataItem);
            Remove();
            parentChildDataItems.Insert(index + 1, DataItem);
        }

        private void AddNodeAndSelect(TItem dataItem)
        {
            var tn = ChildNodes.FirstOrDefault(treeNode => treeNode.DataItem.Equals(dataItem));
            if (tn != null)
            {
                _ = Expand(true);
                tn.SetSelected(true);
            }
        }

        /// <summary>
        /// Drag and drop into child nodes
        /// </summary>
        /// <param name="treeNode">目标</param>
        internal void DragMoveInto(TreeNode<TItem> treeNode)
        {
            if (TreeComponent.DataSource == null || !TreeComponent.DataSource.Any())
                return;
            if (treeNode == this || DataItem.Equals(treeNode.DataItem)) return;

            Remove();

            treeNode.AddChildNode(DataItem);
            treeNode.IsLeaf = false;
            _ = treeNode.Expand(true);
        }

        /// <summary>
        /// Drag and drop to the bottom of the target
        /// </summary>
        /// <param name="treeNode">目标</param>
        internal void DragMoveDown(TreeNode<TItem> treeNode)
        {
            if (TreeComponent.DataSource == null || !TreeComponent.DataSource.Any())
                return;
            if (treeNode == this || DataItem.Equals(treeNode.DataItem)) return;
            Remove();
            treeNode.AddNextNode(DataItem);
        }

        #endregion Node data operation

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            var isExpandedChanged = parameters.IsParameterChanged(nameof(Expanded), _expanded);
            var isCheckedChanged = parameters.IsParameterChanged(nameof(Checked), _checked);
            var isSelectedChanged = parameters.IsParameterChanged(nameof(Selected), _selected);
            await base.SetParametersAsync(parameters);
            if (isExpandedChanged)
            {
                await Expand(_expanded);
            }
            if (isCheckedChanged)
            {
                SetChecked(_checked);
            }
            if (isSelectedChanged)
            {
                SetSelected(_selected);
            }
        }

        protected override void OnInitialized()
        {
            SetTreeNodeClassMapper();
            if (ParentNode != null)
                ParentNode.AddNode(this);
            else
            {
                TreeComponent.AddChildNode(this);
            }

            TreeComponent.AddNode(this);

            // Expand
            var isExpanded = false;
            if (_expanded)
            {
                isExpanded = true;
            }
            else
            {
                var expandedKeys = TreeComponent.CachedExpandedKeys ?? TreeComponent.DefaultExpandedKeys;
                if (expandedKeys != null)
                {
                    isExpanded = expandedKeys != null && expandedKeys.Contains(Key);
                }
                else
                {
                    isExpanded = TreeComponent.DefaultExpandAll || (ParentNode == null && TreeComponent.DefaultExpandParent);
                }
            }
            _ = DoExpand(isExpanded);

            if (TreeComponent.DisabledExpression != null)
                Disabled = TreeComponent.DisabledExpression(this);

            if (TreeComponent.CheckableExpression != null)
                Checkable = TreeComponent.CheckableExpression(this);

            if (Checkable)
            {
                var isChecked = false;
                var checkedKeys = TreeComponent.CachedCheckedKeys ?? TreeComponent.DefaultCheckedKeys;
                var ancestorKeys = GetAncestorKeys();
                if (_checked)
                {
                    isChecked = true;
                }
                else if (!TreeComponent.CheckStrictly && (checkedKeys != null) && ancestorKeys.Any(k => checkedKeys.Contains(k)))
                {
                    isChecked = true;
                }
                else
                {
                    if (checkedKeys != null)
                        isChecked = checkedKeys.Any(k => k == Key);
                }
                DoCheck(isChecked, false, false);
            }

            if (TreeComponent.SelectableExpression != null)
                Selectable = TreeComponent.SelectableExpression(this);

            if (Selectable)
            {
                var isSelected = false;
                if (_selected)
                {
                    isSelected = true;
                }
                else
                {
                    var selectedKeys = TreeComponent.CachedSelectedKeys ?? TreeComponent.DefaultSelectedKeys;
                    if (selectedKeys != null)
                        isSelected = selectedKeys.Any(k => k == Key);
                }
                DoSelect(isSelected, TreeComponent.Multiple, false);
            }
            base.OnInitialized();
        }

        private IEnumerable<string> GetAncestorKeys()
        {
            var ancestorKeys = new List<string>();
            var parentNode = ParentNode;
            while (parentNode != null)
            {
                ancestorKeys.Add(parentNode.Key);
                parentNode = parentNode.ParentNode;
            }

            return ancestorKeys;
        }

        protected override void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (ParentNode != null)
            {
                ParentNode.ChildNodes.Remove(this);
                ParentNode = null;
            }
            else
            {
                TreeComponent.ChildNodes.Remove(this);
            }

            TreeComponent.RemoveNode(this);
            base.Dispose(disposing);
        }
    }
}
