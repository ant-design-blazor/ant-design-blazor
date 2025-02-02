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
        [Parameter]
        public OneOf<TimelineDotColor, string> Color { get; set; } = TimelineDotColor.Blue;

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
            HeadStyle = Color.IsT1 ? $"border-color:{Color.AsT1}" : "";
        }

        internal void SetClassMap()
        {
            var prefix = "ant-timeline-item";
            ClassMapper.Clear().Add(prefix)
                .If($"{prefix}-right", () => Position == TimelineMode.Right)
                .If($"{prefix}-left", () => Position == TimelineMode.Left)
                .If($"{prefix}-last", () => IsLast);

            var headPrefix = "ant-timeline-item-head";
            _headClassMapper.Add(headPrefix)
                .GetIf(() => $"{headPrefix}-{Color.AsT0.ToString().ToLowerInvariant()}", () => Color.IsT0)
                .If($"{headPrefix}-custom", () => Dot != null);
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
