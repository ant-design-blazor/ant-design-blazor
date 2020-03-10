using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntMenuBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public int inlineIndent { get; set; } = 24;

        [Parameter]
        public string theme { get; set; } = "light";//'light' | 'dark' = 'light';

        [Parameter]
        public AntDirectionVHIType mode { get; set; } = AntDirectionVHIType.vertical;

        [Parameter]
        public bool inDropDown { get; set; } = false;

        [Parameter]
        public bool inlineCollapsed { get; set; } = false;

        [Parameter]
        public bool selectable { get; set; }

        [Parameter]
        public EventCallback<AntMenuItem> click { get; set; }

        public IList<AntMenuItem> MenuItems = new List<AntMenuItem>();

        public IList<AntSubMenu> SubMenus = new List<AntSubMenu>();

        public IList<AntSubMenu> openedSubMenus = new List<AntSubMenu>();

        public bool isInDropDown { get; set; }

        private AntDirectionVHIType cacheMode;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
            isInDropDown = inDropDown;
            cacheMode = mode;
        }

        private void SetClassMap()
        {
            string prefixName = isInDropDown ? "ant-dropdown-menu" : "ant-menu";
            ClassMapper.Add(prefixName)
                .Add($"{prefixName}-root")
                .Add($"{prefixName}-{theme}")
                .Add($"{prefixName}-{mode}")
                .If($"{prefixName}-inline-collapsed", () => inlineCollapsed);
        }

        protected override async Task OnParametersSetAsync()
        {
            base.OnParametersSet();
            await updateInLineCollapse();
        }

        private async Task updateInLineCollapse()
        {
            if (MenuItems.Any())
            {
                if (inlineCollapsed)
                {
                    openedSubMenus = this.SubMenus.Where(x => x.open).ToList();
                    foreach (var antSubMenu in this.SubMenus)
                    {
                        await antSubMenu.SetOpenState(false);
                    }

                    this.mode = AntDirectionVHIType.vertical;
                }
                else
                {
                    foreach (var subMenu in openedSubMenus)
                    {
                        await subMenu.SetOpenState(false);
                    }
                    openedSubMenus.Clear();
                    this.mode = this.cacheMode;
                }
                StateHasChanged();
            }
        }
    }
}