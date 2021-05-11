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

        #region fields

        private const double OffSETX = 25;

        /// <summary>
        /// 可释放目标 ClientX
        /// </summary>
        private double _drag_target_clientx = 0;

        #endregion



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
        private async Task OnClick(MouseEventArgs args)
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
        private async Task OnDblClick(MouseEventArgs args)
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
            SelfNode.Expand(false);
            if (TreeComponent.OnDragStart.HasDelegate)
                TreeComponent.OnDragStart.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode));
        }

        /// <summary>
        /// 离开可释放目标
        /// </summary>
        /// <param name="e"></param>
        private void OnDragLeave(DragEventArgs e)
        {
            SelfNode.DragTarget = false;
            if (TreeComponent.OnDragLeave.HasDelegate)
                TreeComponent.OnDragLeave.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode));
        }

        /// <summary>
        /// 进入可释放目标
        /// </summary>
        /// <param name="e"></param>
        private void OnDragEnter(DragEventArgs e)
        {
            if (TreeComponent.DragItem == SelfNode) return;
            SelfNode.DragTarget = true;
            SelfNode.DragTargetBottom = true;
            _drag_target_clientx = e.ClientX;

            System.Diagnostics.Debug.WriteLine($"OnDragEnter {SelfNode.Title}  {System.Text.Json.JsonSerializer.Serialize(e)}");

            if (TreeComponent.OnDragEnter.HasDelegate)
                TreeComponent.OnDragEnter.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode));
        }

        /// <summary>
        /// 可释放目标上往右移动超过 OffSETX距离时视为子项
        /// </summary>
        /// <param name="e"></param>
        private void OnDragOver(DragEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"OnDragOver {SelfNode.Title}  {System.Text.Json.JsonSerializer.Serialize(e)}");
            if (e.ClientX - _drag_target_clientx > OffSETX)
                SelfNode.DragTargetBottom = false;
            else
                SelfNode.DragTargetBottom = true;
        }


        /// <summary>
        /// 落入目标
        /// </summary>
        /// <param name="e"></param>
        private void OnDrop(DragEventArgs e)
        {
            SelfNode.DragTarget = false;
            if (SelfNode.DragTargetBottom)
                TreeComponent.DragItem.DragMoveDown(SelfNode);
            else
                TreeComponent.DragItem.DragMoveInto(SelfNode);

            if (TreeComponent.OnDragEnter.HasDelegate)
                TreeComponent.OnDragEnter.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, TreeComponent.DragItem) { TargetNode = SelfNode });
        }

        /// <summary>
        /// 拖拽结束
        /// </summary>
        /// <param name="e"></param>
        private void OnDragEnd(DragEventArgs e)
        {
            StateHasChanged();
            if (TreeComponent.OnDragEnd.HasDelegate)
                TreeComponent.OnDragEnd.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, TreeComponent.DragItem) { TargetNode = SelfNode });
        }
    }
}
