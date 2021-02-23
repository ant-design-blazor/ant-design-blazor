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
        [Parameter]
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

        private async Task OnClick(MouseEventArgs args)
        {
            SelfNode.SetSelected(!SelfNode.Selected);
            if (TreeComponent.OnClick.HasDelegate && args.Button == 0)
                await TreeComponent.OnClick.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode, args));
            else if (TreeComponent.OnContextMenu.HasDelegate && args.Button == 2)
                await TreeComponent.OnContextMenu.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode, args));
        }

        private async Task OnDblClick(MouseEventArgs args)
        {
            if (TreeComponent.OnDblClick.HasDelegate && args.Button == 0)
                await TreeComponent.OnDblClick.InvokeAsync(new TreeEventArgs<TItem>(TreeComponent, SelfNode, args));
        }
    }
}
