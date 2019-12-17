using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntCardBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment Body { get; set; }

        [Parameter]
        public bool bordered { get; set; } = false;

        [Parameter]
        public bool hoverable { get; set; } = false;

        [Parameter]
        public bool loading { get; set; } = false;

        [Parameter]
        public string bodyStyle { get; set; }

        [Parameter]
        public RenderFragment Cover { get; set; }

        [Parameter]
        public IList<RenderFragment> Actions { get; set; } = new List<RenderFragment>();

        [Parameter]
        public string type { get; set; }

        [Parameter]
        public string size { get; set; }

        [Parameter]
        public RenderFragment Title { get; set; }

        [Parameter]
        public RenderFragment Extra { get; set; }

        [Parameter]
        public RenderFragment AntCardTab { get; set; }

        internal IList<AntCardGrid> grids { get; set; } = new List<AntCardGrid>();

        protected void setClassMap()
        {
            this.ClassMapper.Clear()
                .Add("ant-card")
                .If("ant-card-loading", () => loading)
                .If("ant-card-bordered", () => bordered)
                .If("ant-card-hoverable", () => hoverable)
                .If("ant-card-small", () => size == "small")
                .If("ant-card-contain-grid", () => grids.Count > 0)
                .If("ant-card-type-inner", () => type == "inner")
                .If("ant-card-contain-tabs", () => AntCardTab != null)
                ;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            setClassMap();
        }
    }
}