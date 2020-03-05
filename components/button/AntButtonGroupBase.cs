using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntButtonGroupBase : AntDomComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        private string _size;

        [Parameter]
        public string size
        {
            get => _size;
            set
            {
                this._size = value;
                SetClassMap();
            }
        }

        public IList<AntButton> Buttons = new List<AntButton>();

        internal bool isInDropdown = false;

        public void SetClassMap()
        {
            var prefixName = "ant-btn-group";
            ClassMapper.Clear().Add(prefixName)
                .If("ant-dropdown-button", () => isInDropdown)
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