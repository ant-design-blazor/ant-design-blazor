﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.AspNetCore.Components;

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
        [Parameter]
        public string Color { get; set; } = "blue";

        /// <summary>
        /// The label of the timeline item.
        /// </summary>
        [Parameter]
        public string Label { get; set; }

        [CascadingParameter]
        private Timeline ParentTimeline { get; set; }

        internal ClassMapper _headClassMapper = new ClassMapper();

        internal bool IsLast { get; set; } = false;

        //'left' | 'alternate' | 'right'
        internal string Position { get; set; } = "";

        internal string HeadStyle { get; set; } = "";

        internal string ItemClass => ClassMapper.Class;

        private readonly string[] _defaultColors = new[] { "blue", "red", "green", "gray" };

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
