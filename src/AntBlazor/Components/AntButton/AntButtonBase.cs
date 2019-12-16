using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public class AntButtonBase : AntDomComponentBase
    {
        [Parameter] public bool block { get; set; } = false;

        [Parameter] public bool ghost { get; set; } = false;

        [Parameter] public bool search { get; set; } = false;

        [Parameter] public bool loading { get; set; } = false;

        [Parameter] public string type { get; set; } = AntButtonType.Default;

        [Parameter] public string shape { get; set; } = null;

        [Parameter] public string size { get; set; } = NzSizeLDSType.Default;

        [Parameter] public string icon { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> onclick { get; set; }

        [Inject] private NavigationManager NavigationManger { get; set; }

        public IList<AntIcon> Icons { get; set; } = new List<AntIcon>();

        public AntNavLink Link { get; set; }

        protected string iconStyle { get; set; }

        private bool isInDropdown = false;
        private bool iconOnly = false;

        protected void SetClassMap()
        {
            string prefixName = "ant-btn";
            Hashtable sizeMap = new Hashtable()
            {
                ["large"] = "lg",
                ["small"] = "sm"
            };

            ClassMapper.Clear()
                .Add("ant-btn")
                .If($"{prefixName}-{this.type}", () => !string.IsNullOrEmpty(type))
                .If($"{prefixName}-{shape}", () => !string.IsNullOrEmpty(shape))
                .If($"{prefixName}-{sizeMap[this.size]}", () => sizeMap.ContainsKey(size))
                .If($"{prefixName}-loading", () => loading)
                .If($"{prefixName}-icon-only", () => Icons.Count == 0 && !this.search && !this.isInDropdown && this.ChildContent == null)
                .If($"{prefixName}-background-ghost", () => ghost)
                .If($"{prefixName}-block", () => this.block)
                .If($"ant-input-search-button", () => this.search)
                ;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Link != null && string.IsNullOrEmpty(this.type))
            {
                this.type = AntButtonType.Link;
            }
            SetClassMap();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClassMap();
            updateIconDisplay(this.loading);
            if (type == "link")
            {
            }
        }

        private void updateIconDisplay(bool vlaue)
        {
            iconStyle = $"display:{(vlaue ? "none" : "inline-block")}";
        }

        protected async Task OnClick(MouseEventArgs args)
        {
            if (Link != null)
            {
                NavigationManger.NavigateTo(Link.Href);
            }

            if (onclick.HasDelegate)
            {
                await onclick.InvokeAsync(args);
            }
        }
    }
}