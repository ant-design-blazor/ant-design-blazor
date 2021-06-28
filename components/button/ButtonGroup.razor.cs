using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ButtonGroup : AntDomComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

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
            string prefixName = "ant-btn-group";
            ClassMapper.Add(prefixName)
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
