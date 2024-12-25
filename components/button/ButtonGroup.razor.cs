// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ButtonGroup : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public ButtonGroupSize? Size { get; set; }

        private readonly bool _isInDropdown = false;

        private void SetClassMap()
        {
            string prefixName = "ant-btn-group";
            ClassMapper.Add(prefixName)
                .If("ant-dropdown-button", () => _isInDropdown)
                .If($"{prefixName}-lg", () => Size.HasValue && Size.Value == ButtonGroupSize.Large)
                .If($"{prefixName}-sm", () => Size.HasValue && Size.Value == ButtonGroupSize.Small)
                .If($"{prefixName}-rtl", () => RTL);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }
    }
}
