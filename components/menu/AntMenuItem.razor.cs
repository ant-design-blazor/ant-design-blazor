using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntMenuItem : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Disabled { get; set; } = false;

        [Parameter]
        public bool Selected { get; set; } = false;

        [Parameter]
        public int? PaddingLeft { get; set; }

        [Parameter]
        public bool MatchRouterExact { get; set; } = false;

        [Parameter]
        public bool MatchRouter { get; set; } = false;

        [CascadingParameter]
        public AntMenu Menu { get; set; }

        [CascadingParameter]
        public AntSubMenu SubMenu { get; set; }

        private int _originalPadding;

        private void SetClassMap()
        {
            string prefixName = Menu.IsInDropDown ? "ant-dropdown-menu-item" : "ant-menu-item";
            ClassMapper.Clear()
                .Add(prefixName)
                .If($"{prefixName}-selected", () => Selected)
                .If($"{prefixName}-disabled", () => Disabled);
        }

        internal void SelectedChanged(bool value)
        {
            this.Selected = value;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (this is AntMenuItem item)
            {
                Menu?._menuItems.Add(item);
                SubMenu?.Items.Add(item);
            }

            int? padding = null;
            if (Menu.Mode == AntDirectionVHIType.inline)
            {
                if (PaddingLeft != null)
                {
                    padding = PaddingLeft;
                }
                else
                {
                    int level = SubMenu?.Level + 1 ?? 1;
                    padding = level * this.Menu.InlineIndent;
                }
            }
            else
            {
                padding = _originalPadding;
            }

            if (padding != null)
            {
                Style += $"padding-left:{padding}px;";
            }

            SetClassMap();
        }
    }
}
