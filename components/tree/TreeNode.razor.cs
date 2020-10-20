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
    public partial class TreeNode : AntDomComponentBase
    {

        #region Node

        /// <summary>
        /// 树控件本身
        /// </summary>
        [CascadingParameter(Name = "Tree")]
        public Tree TreeComponent { get; set; }

        /// <summary>
        /// 上一级节点
        /// </summary>
        [CascadingParameter(Name = "Node")]
        public TreeNode ParentNode { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        [Parameter]
        public RenderFragment Nodes { get; set; }
        public List<TreeNode> ChildNodes { get; set; } = new List<TreeNode>();

        public bool HasChildNodes => ChildNodes?.Count > 0;

        /// <summary>
        /// 当前节点级别
        /// </summary>
        public int TreeLevel => (ParentNode?.TreeLevel ?? -1) + 1;//因为第一层是0，所以默认是-1

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name=""></param>
        internal void AddNode(TreeNode treeNode)
        {
            ChildNodes.Add(treeNode);
            IsLeaf = false;
        }


        #endregion

        #region TreeNode

        static long _nextNodeId;

        internal long NodeId { get; private set; }



        public TreeNode()
        {
            NodeId = Interlocked.Increment(ref _nextNodeId);
        }

        private string _name;
        /// <summary>
        /// 指定当前节点的唯一标识符名称。
        /// </summary>
        [Parameter]
        public string Name
        {
            get
            {
                if (TreeComponent.NameExpression != null)
                    return TreeComponent.NameExpression(this);
                else
                    return _name;
            }
            set
            {
                _name = value;
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
            if (TreeComponent.OnNodeLoadDelayAsync.HasDelegate)
            {
                //如果支持异步载入，那么在展开时是调用异步载入代码
                this.IsLoading = true;
                await TreeComponent.OnNodeLoadDelayAsync.InvokeAsync(new TreeEventArgs(TreeComponent, this, args));
                this.IsLoading = false;
            }
            this.IsExpanded = !this.IsExpanded;
            if (TreeComponent.OnExpandChanged.HasDelegate)
                await TreeComponent.OnExpandChanged.InvokeAsync(new TreeEventArgs(TreeComponent, this, args));
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
                await TreeComponent.OnCheckBoxChanged.InvokeAsync(new TreeEventArgs(TreeComponent, this, args));
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
        public object DataItem { get; set; }

        private IEnumerable ChildDataItems
        {
            get
            {
                if (TreeComponent.ChildrenExpression != null)
                    return TreeComponent.ChildrenExpression(this) ?? Enumerable.Empty<object>();
                else
                    return Enumerable.Empty<object>();
            }
        }


        #endregion
        /// <summary>
        /// 查找节点
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="recursive">是否递归查找</param>
        /// <returns></returns>
        public TreeNode FindFirstOrDefaultNode(Func<TreeNode,bool> predicate, bool recursive = true)
        {
            foreach (var child in ChildNodes)
            {
                if (predicate.Invoke(child))
                {
                    return child;
                }
                else if(recursive)
                {
                    return child.FindFirstOrDefaultNode(predicate, recursive);
                }
            }
            return null;
        }
      


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


    }
}



