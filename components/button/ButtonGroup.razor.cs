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

        private string _size;

        [Parameter]
        public string Size
        {
            get => _size;
            set
            {
                this._size = value;
            }
        }

        private readonly bool _isInDropdown = false;

        private void SetClassMap()
        {
            var (hashId, _) = StyleUtil.UseToken();
            string prefixName = "ant-btn-group";
            ClassMapper.Add(prefixName)
                .Add(hashId)
                .If("ant-dropdown-button", () => _isInDropdown)
                .If($"{prefixName}-lg", () => this._size == "large")
                .If($"{prefixName}-sm", () => this._size == "small")
                .If($"{prefixName}-rtl", () => RTL);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }
    }
}
