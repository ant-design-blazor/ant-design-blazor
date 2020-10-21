using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Tree : AntDomComponentBase
    {
        #region Tree

        /// <summary>
        /// 节点前添加展开图标
        /// </summary>
        [Parameter]
        public bool ShowExpand { get; set; } = true;

        /// <summary>
        /// 是否展示连接线
        /// </summary>
        [Parameter]
        public bool ShowLine { get; set; }

        /// <summary>
        /// 是否展示 TreeNode title 前的图标
        /// </summary>
        [Parameter]
        public bool ShowIcon { get; set; }

        /// <summary>
        /// 是否节点占据一行
        /// </summary>
        [Parameter]
        public bool BlockNode { get; set; }

        /// <summary>
        /// 设置节点可拖拽
        /// </summary>
        [Parameter]
        public bool Draggable { get; set; }


        private void SetClassMapper()
        {
            ClassMapper.Clear().Add("ant-tree")
                .If("ant-tree-show-line", () => ShowLine)
                .If("ant-tree-icon-hide", () => ShowIcon)
                .If("ant-tree-block-node", () => BlockNode)
                .If("draggable-tree", () => Draggable);
        }

        #endregion

        #region Node

        [Parameter]
        public RenderFragment Nodes { get; set; }

        [Parameter]
        public List<TreeNode> ChildNodes { get; set; } = new List<TreeNode>();

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name=""></param>
        internal void AddNode(TreeNode treeNode)
        {
            ChildNodes.Add(treeNode);

        }

        #endregion

        #region Selected

        /// <summary>
        /// 支持点选多个节点（节点本身）
        /// </summary>
        [Parameter]
        public bool Multiple { get; set; }

        /// <summary>
        /// 选中的树节点
        /// </summary>
        internal Dictionary<long, TreeNode> SelectedNodesDictionary { get; set; } = new Dictionary<long, TreeNode>();

        public List<TreeNode> SelectedNodes => SelectedNodesDictionary.Select(x => x.Value).ToList();

        public List<string> SelectedNames => SelectedNodesDictionary.Select(x => x.Value.Name).ToList();

        public List<string> SelectedTitles => SelectedNodesDictionary.Select(x => x.Value.Title).ToList();

        internal void SelectedNodeAdd(TreeNode treeNodes)
        {
            if (SelectedNodesDictionary.ContainsKey(treeNodes.NodeId) == false)
                SelectedNodesDictionary.Add(treeNodes.NodeId, treeNodes);
        }
        internal void SelectedNodeRemove(TreeNode treeNodes)
        {
            if (SelectedNodesDictionary.ContainsKey(treeNodes.NodeId) == true)
                SelectedNodesDictionary.Remove(treeNodes.NodeId);
        }
     
        public void DeselectAll()
        {
            foreach (var item in SelectedNodesDictionary.Select(x => x.Value).ToList())
            {
                item.SetSelected(false);
            }
        }

        #endregion

        #region Checkable

        /// <summary>
        /// 节点前添加 Checkbox 复选框
        /// </summary>
        [Parameter]
        public bool Checkable { get; set; }

        public List<TreeNode> CheckedNodes => GetCheckedNodes(ChildNodes);

        public List<string> CheckedNames => GetCheckedNodes(ChildNodes).Select(x => x.Name).ToList();

        public List<string> CheckedTitles => GetCheckedNodes(ChildNodes).Select(x => x.Title).ToList();

        private List<TreeNode> GetCheckedNodes(List<TreeNode> childs)
        {
            List<TreeNode> checkeds = new List<TreeNode>();
            foreach (var item in childs)
            {
                if (item.IsChecked) checkeds.Add(item);
                checkeds.AddRange(GetCheckedNodes(item.ChildNodes));
            }
            return checkeds;
        }

        //取消所有选择项目
        public void DecheckedAll()
        {
            foreach (var item in ChildNodes)
            {
                item.SetChecked(false);
            }
        }

        #endregion

        #region Search

        public string _searchValue;
        /// <summary>
        /// 按需筛选树,双向绑定
        /// </summary>
        [Parameter]
        public string SearchValue
        {
            get => _searchValue;
            set
            {
                if (_searchValue == value) return;
                _searchValue = value;
                if (string.IsNullOrEmpty(value)) return;
                foreach (var item in ChildNodes)
                {
                    SearchNode(item);
                }
            }
        }

        /// <summary>
        /// 返回一个值是否是页节点
        /// </summary>
        [Parameter]
        public Func<TreeNode, bool> SearchExpression { get; set; }

        /// <summary>
        /// 查询节点
        /// </summary>
        /// <param name="treeNode"></param>
        /// <returns></returns>
        private bool SearchNode(TreeNode treeNode)
        {
            if (SearchExpression != null)
                treeNode.IsMatched = SearchExpression(treeNode);
            else
                treeNode.IsMatched = treeNode.Title.Contains(SearchValue);

            var hasChildMatched = treeNode.IsMatched;
            foreach (var item in treeNode.ChildNodes)
            {
                var itemMatched = SearchNode(item);
                hasChildMatched = hasChildMatched || itemMatched;
            }
            treeNode.HasChildMatched = hasChildMatched;

            return hasChildMatched;
        }

        #endregion

        #region DataBind

        [Parameter]
        public IEnumerable DataSource { get; set; }

        /// <summary>
        /// 指定一个方法，该表达式返回节点的文本。
        /// </summary>
        [Parameter]
        public Func<TreeNode, string> TitleExpression { get; set; }

        /// <summary>
        /// 指定一个返回节点名称的方法。
        /// </summary>
        [Parameter]
        public Func<TreeNode, string> NameExpression { get; set; }

        /// <summary>
        /// 指定一个返回节点名称的方法。
        /// </summary>
        [Parameter]
        public Func<TreeNode, string> IconExpression { get; set; }

        /// <summary>
        /// 返回一个值是否是页节点
        /// </summary>
        [Parameter]
        public Func<TreeNode, bool> IsLeafExpression { get; set; }

        /// <summary>
        /// 返回子节点的方法
        /// </summary>
        [Parameter]
        public Func<TreeNode, IEnumerable> ChildrenExpression { get; set; }

        #endregion

        #region Event

        /// <summary>
        /// 延迟加载
        /// </summary>
        /// <remarks>必须使用async，且返回类型为Task，否则可能会出现载入时差导致显示问题</remarks>
        [Parameter]
        public EventCallback<TreeEventArgs> OnNodeLoadDelayAsync { get; set; }

        /// <summary>
        /// 点击树节点触发
        /// </summary>
        [Parameter]
        public EventCallback<TreeEventArgs> OnClick { get; set; }

        /// <summary>
        /// 双击树节点触发
        /// </summary>
        [Parameter]
        public EventCallback<TreeEventArgs> OnDblClick { get; set; }

        /// <summary>
        /// 右键树节点触发
        /// </summary>
        [Parameter]
        public EventCallback<TreeEventArgs> OnContextMenu { get; set; }

        /// <summary>
        /// 点击树节点 Checkbox 触发
        /// </summary>
        [Parameter]
        public EventCallback<TreeEventArgs> OnCheckBoxChanged { get; set; }

        /// <summary>
        /// 点击展开树节点图标触发
        /// </summary>
        [Parameter]
        public EventCallback<TreeEventArgs> OnExpandChanged { get; set; }

        /// <summary>
        /// 搜索节点时调用(与SearchValue配合使用)
        /// </summary>
        [Parameter]
        public EventCallback<TreeEventArgs> OnSearchValueChanged { get; set; }

        ///// <summary>
        ///// 开始拖拽时调用
        ///// </summary>
        //public EventCallback<TreeEventArgs> OnDragStart { get; set; }

        ///// <summary>
        ///// dragenter 触发时调用
        ///// </summary>
        //public EventCallback<TreeEventArgs> OnDragEnter { get; set; }

        ///// <summary>
        ///// dragover 触发时调用
        ///// </summary>
        //public EventCallback<TreeEventArgs> OnDragOver { get; set; }

        ///// <summary>
        ///// dragleave 触发时调用
        ///// </summary>
        //public EventCallback<TreeEventArgs> OnDragLeave { get; set; }

        ///// <summary>
        ///// drop 触发时调用
        ///// </summary>
        //public EventCallback<TreeEventArgs> OnDrop { get; set; }

        ///// <summary>
        ///// dragend 触发时调用
        ///// </summary>
        //public EventCallback<TreeEventArgs> OnDragEnd { get; set; }

        #endregion

        #region Template

        /// <summary>
        /// 缩进模板
        /// </summary>
        [Parameter]
        public RenderFragment<TreeNode> IndentTemplate { get; set; }

        /// <summary>
        /// 标题模板
        /// </summary>
        [Parameter]
        public RenderFragment<TreeNode> TitleTemplate { get; set; }

        /// <summary>
        /// 图标模板
        /// </summary>
        [Parameter]
        public RenderFragment<TreeNode> TitleIconTemplate { get; set; }

        /// <summary>
        /// 切换图标模板
        /// </summary>
        [Parameter]
        public RenderFragment<TreeNode> SwitcherIconTemplate { get; set; }

    #endregion

        /// <summary>
        /// Find Node
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <param name="recursive">Recursive Find</param>
        /// <returns></returns>
        public TreeNode FindFirstOrDefaultNode(Func<TreeNode, bool> predicate, bool recursive = true)
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
        /// from node expand to root
        /// </summary>
        /// <param name="node">Node</param>
        public void ExpandToNode(TreeNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            var parentNode = node.ParentNode;
            while (parentNode != null)
            {
                parentNode.Expand(true);
                parentNode = parentNode.ParentNode;
            }
        }


        protected override void OnInitialized()
        {
            SetClassMapper();
            base.OnInitialized();
        }
    }
}
