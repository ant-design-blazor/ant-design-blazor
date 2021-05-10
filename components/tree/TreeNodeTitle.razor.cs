using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{

    public partial class TreeNodeTitle<TItem> : ComponentBase
    {
        /// <summary>
        /// 树控件本身
        /// </summary>
        [CascadingParameter(Name = "Tree")]
        public Tree<TItem> TreeComponent { get; set; }

        /// <summary>
        /// 当前节点
        /// </summary>
        [CascadingParameter(Name = "SelfNode")]
        public TreeNode<TItem> SelfNode { get; set; }

        private bool CanDraggable => TreeComponent.Draggable && !SelfNode.Disabled;

        private bool IsSwitcherOpen => SelfNode.Expanded && !SelfNode.IsLeaf;

        private bool IsSwitcherClose => !SelfNode.Expanded && !SelfNode.IsLeaf;

        protected ClassMapper TitleClassMapper { get; } = new ClassMapper();

        private void SetTitleClassMapper()
        {
            TitleClassMapper.Clear().Add("ant-tree-node-content-wrapper")
                .If("draggable", () => CanDraggable)
                .If("ant-tree-node-content-wrapper-open", () => IsSwitcherOpen)
                .If("ant-tree-node-content-wrapper-close", () => IsSwitcherClose)
                .If("ant-tree-node-selected", () => SelfNode.Selected);
        }

        protected override void OnInitialized()
        {
            SetTitleClassMapper();
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            SetTitleClassMapper();
            base.OnParametersSet();
        }

        /// <summary>
        /// 点击选择
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task OnClick(MouseEventArgs args)
        {
            SelfNode.SetSelected(!SelfNode.Selected);
            if (TreeComponent.OnClick.HasDelegate && args.Button == 0)
                await TreeComponent.OnClick.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode, args));
            else if (TreeComponent.OnContextMenu.HasDelegate && args.Button == 2)
                await TreeComponent.OnContextMenu.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode, args));
        }

        /// <summary>
        /// 双击回调
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task OnDblClick(MouseEventArgs args)
        {
            if (TreeComponent.OnDblClick.HasDelegate && args.Button == 0)
                await TreeComponent.OnDblClick.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode, args));
        }

        /// <summary>
        /// 拖拽开始
        /// </summary>
        /// <param name="e"></param>
        private void OnDragStart(DragEventArgs e)
        {
            TreeComponent.DragItem = SelfNode;
            if (TreeComponent.OnDragStart.HasDelegate)
                TreeComponent.OnDragStart.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode));
        }

        /// <summary>
        /// 离开可释放目标
        /// </summary>
        /// <param name="e"></param>
        private void OnDragLeave(DragEventArgs e)
        {
            TreeComponent.DragTargetItem = null;
            if (TreeComponent.OnDragLeave.HasDelegate)
                TreeComponent.OnDragLeave.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode));
        }

        /// <summary>
        /// 进入可释放目标
        /// </summary>
        /// <param name="e"></param>
        private void OnDragEnter(DragEventArgs e)
        {
            SelfNode.DragTarget = true;
            SelfNode.DragTargetBottom = true;
            TreeComponent.DragTargetItem = SelfNode;


            if (TreeComponent.OnDragEnter.HasDelegate)
                TreeComponent.OnDragEnter.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode));
        }

        /// <summary>
        /// 落入目标
        /// </summary>
        /// <param name="e"></param>
        private void OnDrop(DragEventArgs e)
        {
            var parentNode = SelfNode.ParentNode;




            if (TreeComponent.OnDragEnter.HasDelegate)
                TreeComponent.OnDragEnter.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, TreeComponent.DragItem, parentNode));
        }


        /// <summary>
        /// 拖拽结束
        /// </summary>
        /// <param name="e"></param>
        private void OnDragEnd(DragEventArgs e)
        {
            TreeComponent.DragTargetItem = null;
            TreeComponent.DragItem = null;
            if (TreeComponent.OnDragEnd.HasDelegate)
                TreeComponent.OnDragEnd.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, TreeComponent.DragItem));
        }
    }
}
