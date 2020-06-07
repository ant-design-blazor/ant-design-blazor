using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class Card : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RenderFragment Body { get; set; }

        [Parameter]
        public bool Bordered { get; set; } = false;

        [Parameter]
        public bool Hoverable { get; set; } = false;

        [Parameter]
        public bool Loading { get; set; } = false;

        [Parameter]
        public string BodyStyle { get; set; }

        [Parameter]
        public RenderFragment Cover { get; set; }

        [Parameter]
        public IList<RenderFragment> Actions { get; set; } = new List<RenderFragment>();

        [Parameter]
        public string Type { get; set; }

        [Parameter]
        public string Size { get; set; }

        [Parameter]
        public OneOf<string, RenderFragment> Title { get; set; }

        [Parameter]
        public RenderFragment Extra { get; set; }

        [Parameter]
        public RenderFragment AntCardTab { get; set; }

        internal IList<CardGrid> Grids { get; set; } = new List<CardGrid>();

        protected void SetClassMap()
        {
            this.ClassMapper.Clear()
                .Add("ant-card")
                .If("ant-card-loading", () => Loading)
                .If("ant-card-bordered", () => Bordered)
                .If("ant-card-hoverable", () => Hoverable)
                .If("ant-card-small", () => Size == "small")
                .If("ant-card-contain-grid", () => Grids.Count > 0)
                .If("ant-card-type-inner", () => Type == "inner")
                .If("ant-card-contain-tabs", () => AntCardTab != null)
                ;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }
    }
}
