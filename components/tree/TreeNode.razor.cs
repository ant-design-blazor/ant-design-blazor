using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class TreeNode<TItem> : AntDomComponentBase, IRendered<TItem>
    {

        #region Node

        /// <summary>
        /// 树控件本身
        /// </summary>
        [CascadingParameter(Name = "Tree")]
        public Tree<TItem> TreeComponent { get; set; }

        [CascadingParameter(Name = "Tree")]
        public object GGG { get; set; }

        /// <summary>
        /// 上一级节点
        /// </summary>
        [CascadingParameter(Name = "Node")]
        public TreeNode<TItem> ParentNode { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        [Parameter]
        public RenderFragment Nodes { get; set; }
        public List<TreeNode<TItem>> ChildNodes { get; set; } = new List<TreeNode<TItem>>();

        public bool HasChildNodes => ChildNodes?.Count > 0;

        /// <summary>
        /// 当前节点级别
        /// </summary>
        public int TreeLevel => (ParentNode?.TreeLevel ?? -1) + 1;//因为第一层是0，所以默认是-1

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name=""></param>
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
        /// 获得上级数据集合
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


        #endregion

        #region TreeNode

        static long _nextNodeId;

        internal long NodeId { get; private set; }



        public TreeNode()
        {
            NodeId = Interlocked.Increment(ref _nextNodeId);
        }

        private string _key;
        /// <summary>
        /// 指定当前节点的唯一标识符名称。
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

        private bool _isDisabled = false;
        /// <summary>
        /// 是否禁用
        /// </summary>
        [Parameter]
        public bool IsDisabled
        {
            get { return _isDisabled || (ParentNode?.IsDisabled ?? false); }//禁用状态受制于父节点
            set { _isDisabled = value; }
        }

        public bool _isSelected;

        /// <summary>
        /// 是否已选中
        /// </summary>
        [Parameter]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                SetSelected(value);
            }
        }

        public void SetSelected(bool value)
        {
            if (_isSelected == value) return;
            if (IsDisabled == true) return;
            _isSelected = value;
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
        /// 是否异步加载状态(影响展开图标展示)
        /// </summary>
        [Parameter]
        public bool IsLoading { get; set; }

        protected ClassMapper TreeNodeClassMapper { get; } = new ClassMapper();
        private void SetTreeNodeClassMapper()
        {
            TreeNodeClassMapper.Clear().Add("ant-tree-treenode")
                .If("ant-tree-treenode-disabled", () => IsDisabled)
                .If("ant-tree-treenode-switcher-open", () => IsSwitcherOpen)
                .If("ant-tree-treenode-switcher-close", () => IsSwitcherClose)
                .If("ant-tree-treenode-checkbox-checked", () => IsChecked)
                .If("ant-tree-treenode-checkbox-indeterminate", () => IsHalfChecked)
                .If("ant-tree-treenode-selected", () => IsSelected)
                .If("ant-tree-treenode-loading", () => IsLoading);
        }

        #endregion

        #region Switcher

        private bool _isLeaf = true;
        /// <summary>
        /// 是否为叶子节点
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
        /// 是否已展开
        /// </summary>
        [Parameter]
        public bool IsExpanded { get; set; }

        /// <summary>
        /// 折叠节点
        /// </summary>
        /// <param name="isExpanded"></param>
        public void Expand(bool isExpanded)
        {
            IsExpanded = isExpanded;
        }

        /// <summary>
        /// 真实的展开状态，路径上只要存在折叠，那么下面的全部折叠
        /// </summary>
        internal bool IsRealDisplay
        {
            get
            {
                if (string.IsNullOrEmpty(TreeComponent.SearchValue))
                {//普通模式下节点显示规则
                    if (ParentNode == null) return true;//第一级节点默认显示
                    if (ParentNode.IsExpanded == false) return false;//上级节点如果是折叠的，必定折叠
                    return ParentNode.IsRealDisplay; //否则查找路径三的级节点显示情况
                }
                else
                {//筛选模式下不考虑节点是否展开，只要节点符合条件，或者存在符合条件的子节点是就展开显示
                    return IsMatched || HasChildMatched;

                }
            }
        }

        private async Task OnSwitcherClick(MouseEventArgs args)
        {
            this.IsExpanded = !this.IsExpanded;
            if (TreeComponent.OnNodeLoadDelayAsync.HasDelegate && this.IsExpanded == true)
            {
                //自有节点被展开时才需要延迟加载
                //如果支持异步载入，那么在展开时是调用异步载入代码
                this.IsLoading = true;
                await TreeComponent.OnNodeLoadDelayAsync.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, this, args));
                this.IsLoading = false;
            }
            if (TreeComponent.OnExpandChanged.HasDelegate)
                await TreeComponent.OnExpandChanged.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, this, args));
        }

        private bool IsSwitcherOpen => IsExpanded && !IsLeaf;

        private bool IsSwitcherClose => !IsExpanded && !IsLeaf;

        #endregion

        #region Checkbox

        [Parameter]
        public bool IsChecked { get; set; }
        [Parameter]
        public bool IsHalfChecked { get; set; }

        [Parameter]
        public bool IsDisableCheckbox { get; set; }//是否可以选择不受父节点控制

        /// <summary>
        /// 当点击选择框是触发
        /// </summary>
        private async void OnCheckBoxClick(MouseEventArgs args)
        {
            SetChecked(!IsChecked);
            if (TreeComponent.OnCheckBoxChanged.HasDelegate)
                await TreeComponent.OnCheckBoxChanged.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, this, args));
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        /// <param name="check"></param>
        public void SetChecked(bool check)
        {
            if (IsDisabled) return;
            this.IsChecked = check;
            this.IsHalfChecked = false;
            if (HasChildNodes)
            {
                foreach (var subnode in ChildNodes)
                    subnode?.SetChecked(check);
            }
            if (ParentNode != null)
                ParentNode.UpdateCheckState();
        }

        /// <summary>
        /// 更新选中状态
        /// </summary>
        /// <param name="halfChecked"></param>
        public void UpdateCheckState(bool? halfChecked = null)
        {
            if (halfChecked.HasValue && halfChecked.Value == true)
            {//如果子元素存在不确定状态，父元素必定存在不确定状态
                this.IsChecked = false;
                this.IsHalfChecked = true;
            }
            else if (HasChildNodes == true)
            {//判断当前节点的选择状态
                bool hasChecked = false;
                bool hasUnchecked = false;

                foreach (var item in ChildNodes)
                {
                    if (item.IsHalfChecked == true) break;
                    if (item.IsChecked == true) hasChecked = true;
                    if (item.IsChecked == false) hasUnchecked = true;
                }

                if (hasChecked && !hasUnchecked)
                {
                    this.IsChecked = true;
                    this.IsHalfChecked = false;
                }
                else if (!hasChecked && hasUnchecked)
                {
                    this.IsChecked = false;
                    this.IsHalfChecked = false;
                }
                else if (hasChecked && hasUnchecked)
                {
                    this.IsChecked = false;
                    this.IsHalfChecked = true;
                }
            }

            if (ParentNode != null)
                ParentNode.UpdateCheckState(this.IsHalfChecked);

            //当达到最顶级后进行刷新状态，避免每一级刷新的性能问题
            if (ParentNode == null)
                StateHasChanged();
        }

        #endregion

        #region Title

        [Parameter]
        public bool Draggable { get; set; }

        private string _icon;
        /// <summary>
        /// 节点前的图标，与 `ShowIcon` 组合使用
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

        private string _title;

        /// <summary>
        /// 文本
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
        /// title是否包含SearchValue(搜索使用)
        /// </summary>
        public bool IsMatched { get; set; }

        /// <summary>
        /// 子节点存在满足搜索条件，所以夫节点也需要显示
        /// </summary>
        internal bool HasChildMatched { get; set; }

        #endregion

        #region 数据绑定

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
                return this.TreeComponent.DataSource;
        }

        #endregion

        #region 节点数据操作

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="dataItem"></param>
        public void AddSon(TItem dataItem)
        {
            ChildDataItems.Add(dataItem);

            this.NewChildData = dataItem;
        }

        /// <summary>
        /// 节点后面添加节点
        /// </summary>
        /// <param name="dataItem"></param>
        public void AddNext(TItem dataItem)
        {
            var parentChildDataItems = GetParentChildDataItems();
            var index = parentChildDataItems.IndexOf(this.DataItem);
            parentChildDataItems.Insert(index + 1, dataItem);

            this.NewChildData = dataItem;
        }

        /// <summary>
        /// 节点前面添加节点
        /// </summary>
        /// <param name="dataItem"></param>
        public void AddPrevious(TItem dataItem)
        {
            var parentChildDataItems = GetParentChildDataItems();
            var index = parentChildDataItems.IndexOf(this.DataItem);
            parentChildDataItems.Insert(index, dataItem);

            this.NewChildData = dataItem;
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        public void RemoveSelf()
        {
            var parentChildDataItems = GetParentChildDataItems();
            parentChildDataItems.Remove(this.DataItem);
        }

        public void NodeMove(TreeNode<TItem> treeNode)
        {
            if (treeNode == this || this.DataItem.Equals(treeNode.DataItem)) return;
            var parentChildDataItems = GetParentChildDataItems();
            parentChildDataItems.Remove(this.DataItem);
            treeNode.AddSon(this.DataItem);
        }

        /// <summary>
        /// 上移节点
        /// </summary>
        public void NodeMoveUp()
        {
            var parentChildDataItems = GetParentChildDataItems();
            var index = parentChildDataItems.IndexOf(this.DataItem);
            if (index == 0) return;
            parentChildDataItems.RemoveAt(index);
            parentChildDataItems.Insert(index - 1, this.DataItem);

        }

        /// <summary>
        /// 下移节点
        /// </summary>
        public void NodeMoveDown()
        {
            var parentChildDataItems = GetParentChildDataItems();
            var index = parentChildDataItems.IndexOf(this.DataItem);
            if (index == parentChildDataItems.Count - 1) return;
            parentChildDataItems.RemoveAt(index);
            parentChildDataItems.Insert(index + 1, this.DataItem);
        }

        /// <summary>
        /// 降级节点
        /// </summary>
        public void NodeDowngrade()
        {
            var previousNode = GetPreviousNode();
            if (previousNode == null) return;
            var parentChildDataItems = GetParentChildDataItems();
            parentChildDataItems.Remove(this.DataItem);
            previousNode.AddSon(this.DataItem);
        }

        /// <summary>
        /// 升级节点
        /// </summary>
        public void NodeUpgrade()
        {
            if (this.ParentNode == null) return;
            var parentChildDataItems = this.ParentNode.GetParentChildDataItems();
            var index = parentChildDataItems.IndexOf(this.ParentNode.DataItem);
            RemoveSelf();
            parentChildDataItems.Insert(index + 1, this.DataItem);
        }




        #endregion 


        protected override void OnInitialized()
        {
            SetTreeNodeClassMapper();
            if (ParentNode != null)
                ParentNode.AddNode(this);
            else
                TreeComponent.AddNode(this);
            base.OnInitialized();
        }


        protected override void OnParametersSet()
        {
            SetTreeNodeClassMapper();
            base.OnParametersSet();

        }

        /// <summary>
        /// 渲染完成后
        /// </summary>
        public Action OnRendered { get; set; }

        /// <summary>
        /// 新节点数据，用于展开并选择新节点
        /// </summary>
        public TItem NewChildData { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (OnRendered != null)
            {
                OnRendered.Invoke();
                OnRendered = null;
            }

            if (NewChildData != null)
            {
                var tn = ChildNodes.FirstOrDefault(treeNode => treeNode.DataItem.Equals(NewChildData));
                if (tn != null)
                {
                    this.Expand(true);
                    tn.SetSelected(true);
                }

                NewChildData = default;
            }
        }
    }
}
