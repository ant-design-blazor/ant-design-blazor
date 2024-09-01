// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class TreeNodeCheckbox<TItem> : ComponentBase
    {
        /// <summary>
        /// Root Tree
        /// </summary>
        [CascadingParameter(Name = "Tree")]
        public Tree<TItem> TreeComponent { get; set; }

        /// <summary>
        /// Current Node
        /// </summary>
        [CascadingParameter(Name = "SelfNode")]
        public TreeNode<TItem> SelfNode { get; set; }

        protected ClassMapper ClassMapper { get; } = new ClassMapper();

        private void SetClassMap()
        {
            ClassMapper
                .Add("ant-tree-checkbox")
                .If("ant-tree-checkbox-checked", () => SelfNode.Checked)
                .If("ant-tree-checkbox-indeterminate", () => SelfNode.Indeterminate)
                .If("ant-tree-checkbox-disabled", () => SelfNode.Disabled || SelfNode.DisableCheckbox);
        }

        protected override void OnInitialized()
        {
            SetClassMap();
            base.OnInitialized();
        }

        [Parameter]
        public EventCallback<MouseEventArgs> OnCheckBoxClick { get; set; }

        private async Task OnClick(MouseEventArgs args)
        {
            if (OnCheckBoxClick.HasDelegate)
                await OnCheckBoxClick.InvokeAsync(args);
        }
    }
}
