using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntMenuItemBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool disabled { get; set; } = false;

        [Parameter]
        public bool selected { get; set; } = false;

        [Parameter]
        public int? paddingLeft { get; set; }

        [Parameter]
        public bool matchRouterExact { get; set; } = false;

        [Parameter]
        public bool matchRouter { get; set; } = false;

        [CascadingParameter]
        public AntMenu Menu { get; set; }

        [CascadingParameter]
        public AntSubMenu SubMenu { get; set; }

        private int originalPadding;

        private void SetClassMap()
        {
            string prefixName = Menu.isInDropDown ? "ant-dropdown-menu-item" : "ant-menu-item";
            ClassMapper.Clear()
                .Add(prefixName)
                .If($"{prefixName}-selected", () => selected)
                .If($"{prefixName}-disabled", () => disabled);
        }

        internal void SelectedChanged(bool value)
        {
            this.selected = value;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (this is AntMenuItem item)
            {
                Menu?.MenuItems.Add(item);
                SubMenu?.Items.Add(item);
            }

            if (Attributes?.TryGetValue("style", out var style) == true)
            {
            }

            int? padding = null;
            if (Menu.mode == AntDirectionVHIType.inline)
            {
                if (paddingLeft != null)
                {
                    padding = paddingLeft;
                }
                else
                {
                    int level = SubMenu?.Level + 1 ?? 1;
                    padding = level * this.Menu.inlineIndent;
                }
            }
            else
            {
                padding = originalPadding;
            }

            if (padding != null)
            {
                Style += $"padding-left:{padding}px;";
            }

            SetClassMap();
        }
    }
}