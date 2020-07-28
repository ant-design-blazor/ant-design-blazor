using Microsoft.AspNetCore.Components;
using System.Linq;

namespace AntDesign
{
    public partial class TimelineItem : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RenderFragment Dot { get; set; }

        [Parameter]
        public string Color { get; set; } = "blue";

        [CascadingParameter] public Timeline ParentTimeline { get; set; }

        internal ClassMapper _headClassMapper = new ClassMapper();

        internal bool IsLast { get; set; } = false;

        //'left' | 'alternate' | 'right'
        internal string Position { get; set; } = "";

        internal string HeadStyle { get; set; } = "";

        internal string ItemClass => ClassMapper.Class;

        private readonly string[] _defaultColors = new[] { "blue", "red", "green", "gray" };

        protected override void Dispose(bool disposing)
        {
            ParentTimeline?._items.Remove(this);
            base.Dispose(disposing);
        }

        protected override void OnInitialized()
        {
            ParentTimeline?.AddItem(this);
            this.TryUpdateCustomColor();
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            this.SetClassMap();
            this.TryUpdateCustomColor();
            base.OnParametersSet();
        }

        private void TryUpdateCustomColor()
        {
            HeadStyle = !_defaultColors.Contains(Color) ? $"border-color:{Color}" : "";
        }

        internal void SetClassMap()
        {
            var prefix = "ant-timeline-item";
            ClassMapper.Clear().Add(prefix)
                .If($"{prefix}-right", () => Position == "right")
                .If($"{prefix}-left", () => Position == "left")
                .If($"{prefix}-last", () => IsLast);

            var headPrefix = "ant-timeline-item-head";
            _headClassMapper.Clear().Add(headPrefix)
                .If($"{headPrefix}-{Color}", () => _defaultColors.Contains(Color))
                .If($"{headPrefix}-custom", () => Dot != null);
        }
    }
}
