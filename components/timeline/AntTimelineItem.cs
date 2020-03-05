using Microsoft.AspNetCore.Components;
using System.Linq;

namespace AntBlazor
{
    public class AntTimelineItem : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter] public RenderFragment Dot { get; set; }

        [Parameter] public string color { get; set; } = "blue";

        [CascadingParameter] public AntTimeline ParentTimeline { get; set; }

        internal ClassMapper HeadClassMapper = new ClassMapper();

        internal bool isLast { get; set; } = false;

        //'left' | 'alternate' | 'right'
        internal string position { get; set; } = "";

        internal string headStyle { get; set; } = "";

        internal string Class => ClassMapper.Class;

        private readonly string[] defaultColors = new[] { "blue", "red", "green", "gray" };

        protected override void OnInitialized()
        {
            ParentTimeline.AddItem(this);
            this.tryUpdateCustomColor();
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            this.setClassMap();
            this.tryUpdateCustomColor();
            base.OnParametersSet();
        }

        private void tryUpdateCustomColor()
        {
            headStyle = !defaultColors.Contains(color) ? $"border-color:{color}" : "";
        }

        internal void setClassMap()
        {
            var prefix = "ant-timeline-item";
            ClassMapper.Clear().Add(prefix)
                .If($"{prefix}-right", () => position == "right")
                .If($"{prefix}-left", () => position == "left")
                .If($"{prefix}-last", () => isLast);

            var headPrefix = "ant-timeline-item-head";
            HeadClassMapper.Clear().Add(headPrefix)
                .If($"{headPrefix}-{color}", () => defaultColors.Contains(color))
                .If($"{headPrefix}-custom", () => Dot != null);
        }
    }
}