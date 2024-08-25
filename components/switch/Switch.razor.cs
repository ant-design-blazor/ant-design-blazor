// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Switch : AntInputBoolComponentBase
    {
        [Parameter]
        public bool Loading { get; set; }

        [Parameter]
        public string CheckedChildren { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment CheckedChildrenTemplate { get; set; }

        /// <summary>
        /// The status of Switch is completely up to the user and no longer 
        /// automatically changes the data based on the click event.
        /// </summary>
        [Parameter]
        public bool Control { get; set; }

        [Parameter]
        public EventCallback OnClick { get; set; }

        [Parameter]
        public string UnCheckedChildren { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment UnCheckedChildrenTemplate { get; set; }

        private bool _clickAnimating = false;

        private string _prefixCls = "ant-switch";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClass();
        }

        protected void SetClass()
        {
            ClassMapper.Clear()
                .Add(_prefixCls)
                .If($"{_prefixCls}-checked", () => CurrentValue)
                .If($"{_prefixCls}-disabled", () => Disabled || Loading)
                .If($"{_prefixCls}-loading", () => Loading)
                .If($"{_prefixCls}-small", () => Size == "small")
                .If($"{_prefixCls}-rtl", () => RTL);
        }

        private async Task HandleClick(MouseEventArgs e)
        {
            if (!Control)
                await base.ChangeValue(!CurrentValue);
            if (OnClick.HasDelegate)
                await OnClick.InvokeAsync(null);
        }

        private void HandleMouseOver(MouseEventArgs e) => _clickAnimating = true;

        private void HandleMouseOut(MouseEventArgs e) => _clickAnimating = false;

    }
}
