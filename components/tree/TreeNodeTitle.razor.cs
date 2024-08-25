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
        /// 
        /// </summary>
        private double _dragTargetClientX = 0;

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
            TitleClassMapper
                .Add("ant-tree-node-content-wrapper")
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task OnClick(MouseEventArgs args)
        {
            if (SelfNode.Disabled)
                return;
            if (TreeComponent.ExpandOnClickNode && !SelfNode.IsLeaf)
            {
                await SelfNode.Expand(!SelfNode.Expanded);
            }
            if (TreeComponent.CheckOnClickNode && TreeComponent.Checkable)
            {
                SelfNode.SetChecked(!SelfNode.Checked);
            }
            else
            {
                SelfNode.SetSelected(!SelfNode.Selected);
            }
            if (TreeComponent.OnClick.HasDelegate && args.Button == 0)
                await TreeComponent.OnClick.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode, args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task OnDblClick(MouseEventArgs args)
        {
            if (TreeComponent.OnDblClick.HasDelegate && args.Button == 0)
                await TreeComponent.OnDblClick.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode, args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task OnContextMenu(MouseEventArgs args)
        {
            if (TreeComponent.OnContextMenu.HasDelegate)
                await TreeComponent.OnContextMenu.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode, args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void OnDragStart(DragEventArgs e)
        {
            TreeComponent.DragItem = SelfNode;
            _ = SelfNode.Expand(false);
            if (TreeComponent.OnDragStart.HasDelegate)
                TreeComponent.OnDragStart.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode));
        }

        /// <summary>
        /// Leaving releases the target
        /// </summary>
        /// <param name="e"></param>
        private void OnDragLeave(DragEventArgs e)
        {
            SelfNode.DragTarget = false;
            SelfNode.SetParentTargetContainer();
            if (TreeComponent.OnDragLeave.HasDelegate)
                TreeComponent.OnDragLeave.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void OnDragEnter(DragEventArgs e)
        {
            if (TreeComponent.DragItem == SelfNode) return;
            SelfNode.DragTarget = true;
            _dragTargetClientX = e.ClientX;

            System.Diagnostics.Debug.WriteLine($"OnDragEnter {SelfNode.Title}  {System.Text.Json.JsonSerializer.Serialize(e)}");

            if (TreeComponent.OnDragEnter.HasDelegate)
                TreeComponent.OnDragEnter.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode));
        }

        /// <summary>
        /// Can be treated as a child if the target is moved to the right beyond the OffsetX distance  
        /// </summary>
        /// <param name="e"></param>
        private void OnDragOver(DragEventArgs e)
        {
            if (TreeComponent.DragItem == SelfNode) return;
            if (e.ClientX - _dragTargetClientX > OffSETX)
            {
                SelfNode.SetTargetBottom();
                SelfNode.SetParentTargetContainer();
                _ = SelfNode.Expand(true);
            }
            else
            {
                SelfNode.SetTargetBottom(true);
                SelfNode.SetParentTargetContainer(true);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void OnDrop(DragEventArgs e)
        {
            SelfNode.DragTarget = false;
            SelfNode.SetParentTargetContainer();
            if (SelfNode.DragTargetBottom)
                TreeComponent.DragItem.DragMoveDown(SelfNode);
            else
                TreeComponent.DragItem.DragMoveInto(SelfNode);

            if (TreeComponent.OnDrop.HasDelegate)
                TreeComponent.OnDrop.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, TreeComponent.DragItem, e, SelfNode.DragTargetBottom) { TargetNode = SelfNode });
        }

        /// <summary>
        /// Drag the end
        /// </summary>
        /// <param name="e"></param>
        private void OnDragEnd(DragEventArgs e)
        {
            if (TreeComponent.OnDragEnd.HasDelegate)
                TreeComponent.OnDragEnd.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, TreeComponent.DragItem) { TargetNode = SelfNode });
        }
    }
}
