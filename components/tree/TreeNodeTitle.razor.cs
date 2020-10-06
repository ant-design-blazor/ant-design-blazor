using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class TreeNodeTitle
    {
        /// <summary>
        /// 树控件本身
        /// </summary>
        [CascadingParameter(Name = "Tree")]
        public Tree TreeComponent { get; set; }

        /// <summary>
        /// 当前节点
        /// </summary>
        [CascadingParameter(Name = "SelfNode")]
        public TreeNode SelfNode { get; set; }

        [Parameter]
        public bool ShowIcon { get; set; }
        [Parameter]
        public string Icon { get; set; }
        [Parameter]
        public string Title { get; set; }
        [Parameter]
        public bool IsLoading { get; set; }
        [Parameter]
        public bool IsSelected { get; set; }
        [Parameter]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// title是否包含nzSearchValue(搜索使用)
        /// </summary>
        [Parameter]
        public bool IsMatched { get; set; }
        [Parameter]
        public bool IsExpanded { get; set; }
        [Parameter]
        public bool IsLeaf { get; set; }

        private bool CanDraggable => TreeComponent.Draggable && !IsDisabled;

        private bool IsSwitcherOpen => IsExpanded && !IsLeaf;

        private bool IsSwitcherClose => !IsExpanded && !IsLeaf;

        protected ClassMapper TitleClassMapper { get; } = new ClassMapper();

        private void SetTitleClassMapper()
        {
            TitleClassMapper.Clear().Add("ant-tree-node-content-wrapper")
                .If("draggable", () => CanDraggable)
                .If("ant-tree-node-content-wrapper-open", () => IsSwitcherOpen)
                .If("ant-tree-node-content-wrapper-close", () => IsSwitcherClose)
                .If("ant-tree-node-selected", () => IsSelected);
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
            SelfNode.SetSelected(!SelfNode.IsSelected);
            if (TreeComponent.OnClick.HasDelegate && args.Button == 0)
                await TreeComponent.OnClick.InvokeAsync(new TreeEventArgs(TreeComponent, SelfNode, args));
            else if (TreeComponent.OnContextMenu.HasDelegate && args.Button == 2)
                await TreeComponent.OnContextMenu.InvokeAsync(new TreeEventArgs(TreeComponent, SelfNode, args));
        }

        private async Task OnDblClick(MouseEventArgs args)
        {
            if (TreeComponent.OnDblClick.HasDelegate && args.Button == 0)
                await TreeComponent.OnDblClick.InvokeAsync(new TreeEventArgs(TreeComponent, SelfNode, args));
        }


    }
}
