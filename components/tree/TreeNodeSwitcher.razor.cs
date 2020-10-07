using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class TreeNodeSwitcher
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

        private bool IsShowLineIcon => !SelfNode.IsLeaf && TreeComponent.ShowLine;

        private bool IsShowSwitchIcon => !SelfNode.IsLeaf && !TreeComponent.ShowLine;

        /// <summary>
        /// 节点是否处于展开状态
        /// </summary>
        private bool IsSwitcherOpen => SelfNode.IsExpanded && !SelfNode.IsLeaf;

        /// <summary>
        /// 节点是否处于关闭状态
        /// </summary>
        private bool IsSwitcherClose => !SelfNode.IsExpanded && !SelfNode.IsLeaf;

        protected ClassMapper ClassMapper { get; } = new ClassMapper();

        private void SetClassMap()
        {
            ClassMapper.Clear().Add("ant-tree-switcher")
                .If("ant-tree-switcher-noop", () => SelfNode.IsLeaf)
                .If("ant-tree-switcher_open", () => IsSwitcherOpen)
                .If("ant-tree-switcher_close", () => IsSwitcherClose);
        }

        protected override void OnInitialized()
        {
            SetClassMap();
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            SetClassMap();
            base.OnParametersSet();
        }

        [Parameter]
        public EventCallback<MouseEventArgs> OnSwitcherClick { get; set; }

        private async Task OnClick(MouseEventArgs args)
        {
            if (OnSwitcherClick.HasDelegate)
                await OnSwitcherClick.InvokeAsync(args);
        }
    }
}
