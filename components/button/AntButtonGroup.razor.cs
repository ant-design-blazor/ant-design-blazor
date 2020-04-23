using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntButtonGroup : AntDomComponentBase
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
                SetClassMap();
            }
        }

        public IList<AntButton> _buttons = new List<AntButton>();

        internal bool _isInDropdown = false;

        public void SetClassMap()
        {
            var prefixName = "ant-btn-group";
            ClassMapper.Clear().Add(prefixName)
                .If("ant-dropdown-button", () => _isInDropdown)
                .If($"{prefixName}-lg", () => this._size == "large")
                .If($"{prefixName}-sm", () => this._size == "small");
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }
    }
}
