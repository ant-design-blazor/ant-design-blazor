using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Button : AntDomComponentBase
    {
        [Parameter]
        public bool Block { get; set; } = false;

        [Parameter]
        public bool Ghost { get; set; } = false;

        [Parameter]
        public bool Search { get; set; } = false;

        [Parameter]
        public bool Loading { get; set; } = false;

        [Parameter]
        public string Type { get; set; } = ButtonType.Default;

        [Parameter]
        public string HtmlType { get; set; } = "button";

        [Parameter]
        public string Shape { get; set; } = null;

        private string _formSize;
        [CascadingParameter(Name = "FormSize")]
        public string FormSize
        {
            get
            {
                return _formSize;
            }
            set
            {
                _formSize = value;

                Size = value;
            }
        }

        [Parameter]
        public string Size { get; set; } = AntSizeLDSType.Default;

        [Parameter]
        public string Icon { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool Danger { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        [Inject] private NavigationManager NavigationManger { get; set; }

        public IList<Icon> Icons { get; set; } = new List<Icon>();

        //public AntNavLink Link { get; set; }

        protected string IconStyle { get; set; }

        private readonly bool _isInDropdown = false;

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
                .If($"{prefixName}-{this.Type}", () => !string.IsNullOrEmpty(Type))
                .If($"{prefixName}-dangerous", () => Danger)
                .If($"{prefixName}-{Shape}", () => !string.IsNullOrEmpty(Shape))
                .If($"{prefixName}-{sizeMap[this.Size]}", () => sizeMap.ContainsKey(Size))
                .If($"{prefixName}-loading", () => Loading)
                .If($"{prefixName}-icon-only", () => Icons.Count == 0 && !this.Search && !this._isInDropdown && this.ChildContent == null)
                .If($"{prefixName}-background-ghost", () => Ghost)
                .If($"{prefixName}-block", () => this.Block)
                .If($"ant-input-search-button", () => this.Search)
                ;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            //if (Link != null && string.IsNullOrEmpty(this.Type))
            //{
            //    this.Type = ButtonType.Link;
            //}
            SetClassMap();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClassMap();
            UpdateIconDisplay(this.Loading);
            if (Type == "link")
            {
            }
        }

        private void UpdateIconDisplay(bool vlaue)
        {
            IconStyle = $"display:{(vlaue ? "none" : "inline-block")}";
        }

        protected async Task HandleOnClick(MouseEventArgs args)
        {
            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(args);
            }
        }
    }
}
