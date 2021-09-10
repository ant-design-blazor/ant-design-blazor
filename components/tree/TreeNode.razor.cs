// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class TreeNode<TItem> : AntDomComponentBase
    {
        #region Node

        /// <summary>
        /// 树控件本身
        /// </summary>
        [CascadingParameter(Name = "Tree")]
        public Tree<TItem> TreeComponent { get; set; }

        /// <summary>
        /// 上一级节点
        /// </summary>
        [CascadingParameter(Name = "Node")]
        public TreeNode<TItem> ParentNode { get; set; }

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
        public int TreeLevel => (ParentNode?.TreeLevel ?? -1) + 1;

        /// <summary>
        /// record the index in children nodes list of parent node. 
        /// </summary>
        internal int NodeIndex { get; set; }

        /// <summary>
        /// Determine if it is the last node in the same level nodes.
        /// </summary>
        internal bool IsLastNode => NodeIndex == (ParentNode?.ChildNodes.Count ?? TreeComponent?.ChildNodes.Count) - 1;

        /// <summary>
        /// add node to parent node
        /// </summary>
        /// <param name="treeNode"></param>
        internal void AddNode(TreeNode<TItem> treeNode)
        {
            treeNode.NodeIndex = ChildNodes.Count;
            ChildNodes.Add(treeNode);
            IsLeaf = false;
        }

        /// <summary>
        /// Find a node
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <param name="recursive">Recursive Find</param>
        /// <returns></returns>
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
        /// Obtain the parent data set
        /// </summary>
        /// <returns></returns>
        public List<TreeNode<TItem>> GetParentNodes()
        {
            if (this.ParentNode != null)
                return this.ParentNode.ChildNodes;
            else
                return this.TreeComponent.ChildNodes;
        }

        public TreeNode<TItem> GetPreviousNode()
        {
            var parentNodes = GetParentNodes();
            var index = parentNodes.IndexOf(this);
            if (index == 0) return null;
            else return parentNodes[index - 1];
        }

        public TreeNode<TItem> GetNextNode()
        {
            var parentNodes = GetParentNodes();
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

        private bool _selected;

        /// <summary>
        /// Selected or not
        /// </summary>
        [Parameter]
        public bool Selected
        {
            get => _selected;
            set
            {
                if (_selected == value) return;
                SetSelected(value);
            }
        }

        /// <summary>
        /// Setting Selection State
        /// </summary>
        /// <param name="value"></param>
        public void SetSelected(bool value)
        {
            if (Disabled) return;
            if (!TreeComponent.Selectable && TreeComponent.Checkable)
            {
                SetChecked(!Checked);
                return;
            }
            if (_selected == value) return;
            _selected = value;
            if (value == true)
            {
                if (TreeComponent.Multiple == false) TreeComponent.DeselectAll();
                TreeComponent.SelectedNodeAdd(this);
            }
            else
            {
                TreeComponent.SelectedNodeRemove(this);
            }
            StateHasChanged();
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
        public void SetTargetBottom(bool value = false)
        {
            if (DragTargetBottom == value) return;
            this.DragTargetBottom = value;
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
            if (this.ParentNode == null) return;
            if (this.ParentNode.TargetContainer == value) return;
            this.ParentNode.TargetContainer = value;
            this.ParentNode.StateHasChanged();
        }

        /// <summary>
        /// Gets the children of the parent node
        /// </summary>
        /// <returns></returns>
        private List<TreeNode<TItem>> GetParentChildNodes()
        {
            return this.ParentNode?.ChildNodes ?? TreeComponent.ChildNodes;
        }

        /// <summary>
        /// Remove the current node
        /// </summary>
        public void RemoveNode()
        {
            GetParentChildNodes().Remove(this);
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
                .If("ant-tree-treenode-leaf-last", () => IsLastNode);
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
        /// Whether it has been expanded
        /// </summary>
        [Parameter]
        public bool Expanded { get; set; }

        /// <summary>
        /// Expand the node
        /// </summary>
        /// <param name="expanded"></param>
        public void Expand(bool expanded)
        {
            if (Expanded == expanded) return;
            Expanded = expanded;
        }

        /// <summary>
        /// The real expand state, as long as there is a expaneded node on the path, then all the folds below
        /// </summary>
        internal bool RealDisplay
        {
            get
            {
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
        private async Task OnSwitcherClick(MouseEventArgs args)
        {
            this.Expanded = !this.Expanded;

            await TreeComponent?.OnNodeExpand(this, this.Expanded, args);
        }

        internal void SetLoading(bool loading)
        {
            this.Loading = loading;
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
        internal void OpenPropagation()
        {
            this.Expand(true);
            if (this.ParentNode != null)
                this.ParentNode.OpenPropagation();
        }

        #endregion Switcher

        #region Checkbox

        /// <summary>
        /// According to check the
        /// </summary>
        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public bool Indeterminate { get; set; }

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
        private async void OnCheckBoxClick(MouseEventArgs args)
        {
            if (DisableCheckbox)
                return;
            SetChecked(!Checked);
            if (TreeComponent.OnCheck.HasDelegate)
                await TreeComponent.OnCheck.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, this, args));
        }

        /// <summary>
        /// Set the checkbox state
        /// </summary>
        /// <param name="check"></param>
        public void SetChecked(bool check)
        {
            if (!Disabled)
            {
                if (TreeComponent.CheckStrictly)
                {
                    this.Checked = check;
                }
                else
                {
                    SetChildChecked(this, check);
                    if (ParentNode != null)
                        ParentNode.UpdateCheckState();
                }
            }
            else
                TreeComponent.AddOrRemoveCheckNode(this);
            StateHasChanged();
        }

        /// <summary>
        /// Sets the checkbox status of child nodes
        /// </summary>
        /// <param name="subnode"></param>
        /// <param name="check"></param>
        private void SetChildChecked(TreeNode<TItem> subnode, bool check)
        {
            if (Disabled) return;
            this.Checked = DisableCheckbox ? false : check;
            this.Indeterminate = false;
            TreeComponent.AddOrRemoveCheckNode(this);
            if (subnode.HasChildNodes)
                foreach (var child in subnode.ChildNodes)
                    child?.SetChildChecked(child, check);
        }

        /// <summary>
        /// Update check status
        /// </summary>
        /// <param name="halfChecked"></param>
        private void UpdateCheckState(bool? halfChecked = null)
        {
            if (halfChecked == true)
            {
                //If the child node is indeterminate, the parent node must is indeterminate.
                this.Checked = false;
                this.Indeterminate = true;
            }
            else if (HasChildNodes == true && !DisableCheckbox)
            {
                //Determines the selection status of the current node
                bool hasChecked = false;
                bool hasUnchecked = false;

                foreach (var item in ChildNodes)
                {
                    if (!item.DisableCheckbox && !item.Disabled)
                    {
                        if (item.Indeterminate)
                        {
                            hasChecked = true;
                            hasUnchecked = true;
                            break;
                        }
                        else if (item.Checked)
                        {
                            hasChecked = true;
                        }
                        else if (!item.Checked)
                        {
                            hasUnchecked = true;
                        }
                    }
                }

                if (hasChecked && !hasUnchecked)
                {
                    this.Checked = true;
                    this.Indeterminate = false;
                }
                else if (!hasChecked && hasUnchecked)
                {
                    this.Checked = false;
                    this.Indeterminate = false;
                }
                else if (hasChecked && hasUnchecked)
                {
                    this.Checked = false;
                    this.Indeterminate = true;
                }
            }
            TreeComponent.AddOrRemoveCheckNode(this);

            if (ParentNode != null)
                ParentNode.UpdateCheckState(this.Indeterminate);

            if (ParentNode == null)
                StateHasChanged();
        }

        #endregion Checkbox

        #region Title

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

        [Parameter]
        public RenderFragment<TreeNode<TItem>> IconTemplate { get; set; }

        [Parameter]
        public string SwitcherIcon { get; set; }

        [Parameter]
        public RenderFragment<TreeNode<TItem>> SwitcherIconTemplate { get; set; }

        private string _title;

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

        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// title是否包含SearchValue(搜索使用)
        /// </summary>
        public bool Matched { get; set; }

        /// <summary>
        /// 子节点存在满足搜索条件，所以夫节点也需要显示
        /// </summary>
        internal bool HasChildMatched { get; set; }

        #endregion Title

        #region data binding

        [Parameter]
        public TItem DataItem { get; set; }

        private IList<TItem> ChildDataItems
        {
            get
            {
                if (TreeComponent.ChildrenExpression != null)
                    return TreeComponent.ChildrenExpression(this) ?? new List<TItem>();
                else
                    return new List<TItem>();
            }
        }

        /// <summary>
        /// 获得上级数据集合
        /// </summary>
        /// <returns></returns>
        public IList<TItem> GetParentChildDataItems()
        {
            if (this.ParentNode != null)
                return this.ParentNode.ChildDataItems;
            else
                return this.TreeComponent.DataSource.ToList();
        }

        #endregion data binding

        #region Node data operation

        /// <summary>
        /// Add child node
        /// </summary>
        /// <param name="dataItem"></param>
        public void AddChildNode(TItem dataItem)
        {
            ChildDataItems.Add(dataItem);
        }

        /// <summary>
        /// Add a node next the node
        /// </summary>
        /// <param name="dataItem"></param>
        public void AddNextNode(TItem dataItem)
        {
            var parentChildDataItems = GetParentChildDataItems();
            var index = parentChildDataItems.IndexOf(this.DataItem);
            parentChildDataItems.Insert(index + 1, dataItem);

            AddNodeAndSelect(dataItem);
        }

        /// <summary>
        /// Add a node before the node
        /// </summary>
        /// <param name="dataItem"></param>
        public void AddPreviousNode(TItem dataItem)
        {
            var parentChildDataItems = GetParentChildDataItems();
            var index = parentChildDataItems.IndexOf(this.DataItem);
            parentChildDataItems.Insert(index, dataItem);

            AddNodeAndSelect(dataItem);
        }

        /// <summary>
        /// remove
        /// </summary>
        public void Remove()
        {
            var parentChildDataItems = GetParentChildDataItems();
            parentChildDataItems.Remove(this.DataItem);
        }

        /// <summary>
        /// The node moves into the child node
        /// </summary>
        /// <param name="treeNode">target node</param>
        public void MoveInto(TreeNode<TItem> treeNode)
        {
            if (treeNode == this || this.DataItem.Equals(treeNode.DataItem)) return;
            var parentChildDataItems = GetParentChildDataItems();
            parentChildDataItems.Remove(this.DataItem);
            treeNode.AddChildNode(this.DataItem);
        }

        /// <summary>
        /// Move up the nodes
        /// </summary>
        public void MoveUp()
        {
            var parentChildDataItems = GetParentChildDataItems();
            var index = parentChildDataItems.IndexOf(this.DataItem);
            if (index == 0) return;
            parentChildDataItems.RemoveAt(index);
            parentChildDataItems.Insert(index - 1, this.DataItem);
        }

        /// <summary>
        /// Move down the node
        /// </summary>
        public void MoveDown()
        {
            var parentChildDataItems = GetParentChildDataItems();
            var index = parentChildDataItems.IndexOf(this.DataItem);
            if (index == parentChildDataItems.Count - 1) return;
            parentChildDataItems.RemoveAt(index);
            parentChildDataItems.Insert(index + 1, this.DataItem);
        }

        /// <summary>
        ///
        /// </summary>
        public void Downgrade()
        {
            var previousNode = GetPreviousNode();
            if (previousNode == null) return;
            var parentChildDataItems = GetParentChildDataItems();
            parentChildDataItems.Remove(this.DataItem);
            previousNode.AddChildNode(this.DataItem);
        }

        /// <summary>
        /// Upgrade nodes
        /// </summary>
        public void Upgrade()
        {
            if (this.ParentNode == null) return;
            var parentChildDataItems = this.ParentNode.GetParentChildDataItems();
            var index = parentChildDataItems.IndexOf(this.ParentNode.DataItem);
            Remove();
            parentChildDataItems.Insert(index + 1, this.DataItem);
        }

        private void AddNodeAndSelect(TItem dataItem)
        {
            var tn = ChildNodes.FirstOrDefault(treeNode => treeNode.DataItem.Equals(dataItem));
            if (tn != null)
            {
                this.Expand(true);
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
            if (treeNode == this || this.DataItem.Equals(treeNode.DataItem)) return;

            Remove();

            treeNode.AddChildNode(this.DataItem);
            treeNode.IsLeaf = false;
            treeNode.Expand(true);
        }

        /// <summary>
        /// Drag and drop to the bottom of the target
        /// </summary>
        /// <param name="treeNode">目标</param>
        internal void DragMoveDown(TreeNode<TItem> treeNode)
        {
            if (TreeComponent.DataSource == null || !TreeComponent.DataSource.Any())
                return;
            if (treeNode == this || this.DataItem.Equals(treeNode.DataItem)) return;
            Remove();
            treeNode.AddNextNode(this.DataItem);
        }

        #endregion Node data operation

        bool _defaultBinding;

        protected override void OnInitialized()

        {
            SetTreeNodeClassMapper();
            if (ParentNode != null)
                ParentNode.AddNode(this);
            else
            {
                TreeComponent.AddNode(this);
                if (!TreeComponent.DefaultExpandAll && TreeComponent.DefaultExpandParent)
                    Expand(true);
            }
            TreeComponent._allNodes.Add(this);

            if (TreeComponent.DisabledExpression != null)
                Disabled = TreeComponent.DisabledExpression(this);

            if (TreeComponent.DefaultExpandAll)
                Expand(true);
            else if (TreeComponent.ExpandedKeys != null)
            {
                Expand(TreeComponent.ExpandedKeys.Any(k => k == this.Key));
            }

            if (TreeComponent.Selectable && TreeComponent.SelectedKeys != null)
            {
                this.Selected = TreeComponent.SelectedKeys.Any(k => k == this.Key);
            }
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            DefaultBinding();
            base.OnParametersSet();
        }

        private void DefaultBinding()
        {
            if (!_defaultBinding)
            {
                _defaultBinding = true;
                if (this.Checked)
                    this.SetChecked(true);
                TreeComponent.DefaultCheckedKeys?.ForEach(k =>
                {
                    var node = TreeComponent._allNodes.FirstOrDefault(x => x.Key == k);
                    if (node != null)
                        node.SetChecked(true);
                });

                TreeComponent.DefaultSelectedKeys?.ForEach(k =>
                {
                    var node = TreeComponent._allNodes.FirstOrDefault(x => x.Key == k);
                    if (node != null)
                        node.SetSelected(true);
                });

                if (!TreeComponent.DefaultExpandAll)
                {
                    if (this.Expanded)
                        this.OpenPropagation();
                    TreeComponent.DefaultExpandedKeys?.ForEach(k =>
                    {
                        var node = TreeComponent._allNodes.FirstOrDefault(x => x.Key == k);
                        if (node != null)
                            node.OpenPropagation();
                    });
                }
            }
        }
    }
}
