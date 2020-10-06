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
    public partial class TreeNodeCheckbox
    {
        [Parameter]
        public bool IsChecked { get; set; }
        [Parameter]
        public bool IsHalfChecked { get; set; }
        [Parameter]
        public bool IsDisabled { get; set; }
        [Parameter]
        public bool IsDisableCheckbox { get; set; }

        protected ClassMapper ClassMapper { get; } = new ClassMapper();

        private void SetClassMap()
        {
            ClassMapper.Clear().Add("ant-tree-checkbox")
                .If("ant-tree-checkbox-checked", () => IsChecked)
                .If("ant-tree-checkbox-indeterminate", () => IsHalfChecked)
                .If("ant-tree-checkbox-disabled", () => IsDisabled || IsDisableCheckbox)
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
