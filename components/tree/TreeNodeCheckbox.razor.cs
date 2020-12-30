// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class TreeNodeCheckbox<TItem>
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

        protected ClassMapper ClassMapper { get; } = new ClassMapper();

        private void SetClassMap()
        {
            ClassMapper.Clear().Add("ant-tree-checkbox")
                .If("ant-tree-checkbox-checked", () => SelfNode.IsChecked)
                .If("ant-tree-checkbox-indeterminate", () => SelfNode.IsHalfChecked)
                .If("ant-tree-checkbox-disabled", () => SelfNode.IsDisabled || SelfNode.IsDisableCheckbox)
                ;
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
        public EventCallback<MouseEventArgs> OnCheckBoxClick { get; set; }

        private async Task OnClick(MouseEventArgs args)
        {
            if (OnCheckBoxClick.HasDelegate)
                await OnCheckBoxClick.InvokeAsync(args);
        }
    }
}
