using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntMenuItemBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool nzDisabled { get; set; } = false;

        [Parameter]
        public bool nzSelected { get; set; } = false;

        [Parameter]
        public int? nzPaddingLeft { get; set; }

        [Parameter]
        public bool nzMatchRouterExact { get; set; } = false;

        [Parameter]
        public bool nzMatchRouter { get; set; } = false;

        [CascadingParameter]
        public AntMenu Menu { get; set; }

        [CascadingParameter]
        public AntSubMenu SubMenu { get; set; }

        public bool isInDropDown { get; set; }

        private int originalPadding;

        public AntMenuItemBase()
        {
        }

        private void SetClassMap()
        {
            string prefixName = isInDropDown ? "ant-dropdown-menu-item" : "ant-menu-item";
            ClassMapper.Add(prefixName)
                .If($"{prefixName}-selected", () => nzSelected)
                .If($"{prefixName}-disabled", () => nzDisabled);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            int? padding = null;
            if (Menu.nzMode == NzDirectionVHIType.inline)
            {
                if (nzPaddingLeft != null)
                {
                    padding = nzPaddingLeft;
                }
                else
                {
                    int level = SubMenu?.Level + 1 ?? 1;
                    padding = level * this.Menu.nzInlineIndent;
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