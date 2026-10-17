// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class TimelineItem : AntDomComponentBase
    {
        /// <summary>
        /// The content of the timeline item.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The dot of the timeline item.
        /// </summary>
        [Parameter]
        public RenderFragment Dot { get; set; }

        /// <summary>
        /// The color of the timeline item.
        /// </summary>
        /// <default value="TimelineDotColor.Blue" />
        private OneOf<TimelineDotColor, string> _color = TimelineDotColor.Blue;
        private bool _hasColor;

        [Parameter]
        public OneOf<TimelineDotColor, string> Color
        {
            get => _color;
            set
            {
                _color = value;
                _hasColor = true;
                TryUpdateCustomColor();
            }
        }

        /// <summary>
        /// The label of the timeline item.
        /// </summary>
        [Parameter]
        public string Label { get; set; }

        [CascadingParameter]
        private Timeline ParentTimeline { get; set; }

        internal ClassMapper _headClassMapper = new ClassMapper();

        internal bool IsLast { get; set; } = false;

        internal TimelineMode? Position { get; set; }

        internal string HeadStyle { get; set; } = "";

        internal string ItemClass => ClassMapper.Class;

        protected override void Dispose(bool disposing)
        {
            ParentTimeline?.RemoveItem(this);
            base.Dispose(disposing);
        }

        protected override void OnInitialized()
        {
            ParentTimeline?.AddItem(this);
            this.TryUpdateCustomColor();
            base.OnInitialized();
            SetClassMap();
        }

        protected override void OnParametersSet()
        {
            this.TryUpdateCustomColor();
            base.OnParametersSet();
        }

        private void TryUpdateCustomColor()
        {
            HeadStyle = Color.IsT1 ? $"--ant-cmp-steps-item-icon-dot-color:{Color.AsT1};" : "";
        }

        internal void SetClassMap()
        {
            ClassMapper.Clear()
                .Add("ant-steps-item ant-steps-item-finish ant-timeline-item")
                .Add($"ant-timeline-item-placement-{(Position == TimelineMode.Right ? "end" : "start")}")
                .If("ant-steps-item-active", () => IsLast)
                .If("ant-steps-item-empty-header", () => string.IsNullOrEmpty(Label))
                .GetIf(() => $"ant-timeline-item-color-{Color.AsT0.ToString().ToLowerInvariant()}", () => _hasColor && Color.IsT0);

            _headClassMapper.Clear()
                .Add("ant-steps-item-icon ant-wave-target ant-timeline-item-icon");
        }

        internal void SetChildContent(RenderFragment childContent)
        {
            this.ChildContent = childContent;
        }

        internal void SetDot(RenderFragment dot)
        {
            this.Dot = dot;
        }
    }
}
