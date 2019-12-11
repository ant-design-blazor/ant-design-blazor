using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntMenuBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public int nzInlineIndent { get; set; } = 24;

        [Parameter]
        public string nzTheme { get; set; } = "light";//'light' | 'dark' = 'light';

        [Parameter]
        public NzDirectionVHIType nzMode { get; set; } = NzDirectionVHIType.vertical;

        [Parameter]
        public bool nzInDropDown { get; set; } = false;

        [Parameter]
        public bool nzInlineCollapsed { get; set; } = false;

        [Parameter]
        public bool nzSelectable { get; set; } //= !this.nzMenuService.isInDropDown;

        [Parameter]
        public EventCallback<AntMenuItem> nzClick { get; set; }

        public IList<AntMenuItem> MenuItems = new List<AntMenuItem>();

        public IList<AntSubMenu> SubMenus = new List<AntSubMenu>();

        public IList<AntSubMenu> openedSubMenus = new List<AntSubMenu>();

        public bool isInDropDown { get; set; }

        private NzDirectionVHIType cacheMode;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
            isInDropDown = nzInDropDown;
            cacheMode = nzMode;
        }

        private void SetClassMap()
        {
            string prefixName = isInDropDown ? "ant-dropdown-menu" : "ant-menu";
            ClassMapper.Add(prefixName)
                .Add($"{prefixName}-root")
                .Add($"{prefixName}-{nzTheme}")
                .Add($"{prefixName}-{nzMode}")
                .If($"{prefixName}-inline-collapsed", () => nzInlineCollapsed);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            updateInLineCollapse();
        }

        private void updateInLineCollapse()
        {
            if (MenuItems.Any())
            {
                if (nzInlineCollapsed)
                {
                    openedSubMenus = this.SubMenus.Where(x => x.nzOpen).ToList();
                    foreach (var antSubMenu in this.SubMenus)
                    {
                        antSubMenu.SetOpenState(false);
                    }

                    this.nzMode = NzDirectionVHIType.vertical;
                }
                else
                {
                    foreach (var subMenu in openedSubMenus)
                    {
                        subMenu.SetOpenState(false);
                    }
                    openedSubMenus.Clear();
                    this.nzMode = this.cacheMode;
                }
            }
        }
    }
}