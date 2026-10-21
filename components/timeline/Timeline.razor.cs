// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
    <para>Vertical display timeline.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>When a series of information needs to be ordered by time (ascending or descending).</item>
        <item>When you need a timeline to make a visual connection.</item>
    </list>
    </summary>
    <seealso cref="TimelineItem" />
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/antfincdn/vJmo00mmgR/Timeline.svg", Title = "Timeline", SubTitle = "时间轴")]
    public partial class Timeline : AntDomComponentBase
    {
        /// <summary>
        /// Visual style of the timeline nodes.
        /// </summary>
        /// <default value="TimelineVariant.Outlined" />
        [Parameter]
        public TimelineVariant Variant { get; set; } = TimelineVariant.Outlined;

        /// <summary>
        /// Layout direction of the timeline.
        /// </summary>
        /// <default value="TimelineOrientation.Vertical" />
        [Parameter]
        public TimelineOrientation Orientation { get; set; } = TimelineOrientation.Vertical;

        /// <summary>
        /// Width of the timeline title span. Numbers use the component's title spacing scale;
        /// strings accept CSS lengths or percentages.
        /// </summary>
        /// <default value="12" />
        [Parameter]
        public OneOf<double, string> TitleSpan { get; set; } = 12d;

        /// <summary>
        /// Semantic class names keyed by the Timeline v6 DOM slots.
        /// </summary>
        [Parameter]
        public IReadOnlyDictionary<string, string> ClassNames { get; set; }

        /// <summary>
        /// Semantic inline styles keyed by the Timeline v6 DOM slots.
        /// </summary>
        [Parameter]
        public IReadOnlyDictionary<string, string> Styles { get; set; }

        /// <summary>
        /// Data-driven timeline items. Use either Items or TimelineItem child components.
        /// </summary>
        [Parameter]
        public IReadOnlyList<TimelineItemData> Items
        {
            get => _dataItems;
            set
            {
                if (!ReferenceEquals(_dataItems, value))
                {
                    _dataItems = value;
                    _waitingItemUpdate = true;
                }
            }
        }

        /// <summary>
        /// Where the line will be in relation to the items - left, right or alternate
        /// </summary>
        /// <default value="TimelineMode.Left" />
        [Parameter]
        public TimelineMode? Mode
        {
            get => _mode;
            set
            {
                if (_mode != value)
                {
                    _mode = value;
                    _waitingItemUpdate = true;
                }
            }
        }

        /// <summary>
        /// Reverse nodes or not
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Reverse
        {
            get => _reverse;
            set
            {
                if (_reverse != value)
                {
                    _reverse = value;
                    _waitingItemUpdate = true;
                }
            }
        }

        /// <summary>
        /// Set the last ghost node's existence or its content
        /// </summary>
        [Parameter]
        public OneOf<string, RenderFragment> Pending
        {
            get => _pending;
            set
            {
                if (_pending.Value != value.Value)
                {
                    _pending = value;
                    if (value.Value == null)
                    {
                        _pendingItem = null;
                        SetItems();
                        return;
                    }
                    _pendingItem = _pending.Value == null ? null : new TimelineItem()
                    {
                        Class = "ant-timeline-item-pending"
                    };

                    _pendingItem?.SetDot(PendingDot ?? _loadingDot);

                    _pending.Switch(str =>
                    {
                        _pendingItem.SetChildContent(b =>
                       {
                           b.AddContent(0, str);
                       });
                    }, rf =>
                    {
                        _pendingItem.SetChildContent(rf);
                    });

                    _waitingItemUpdate = true;
                }
            }
        }

        private static readonly RenderFragment _loadingDot = builder =>
        {
            builder.OpenComponent<Icon>(0);
            builder.AddAttribute(1, nameof(Icon.Type), "loading");
            builder.CloseComponent();
        };

        private TimelineItem _pendingItem;

        /// <summary>
        /// Set the dot of the last ghost node when pending is true	
        /// </summary>
        [Parameter]
        public RenderFragment PendingDot { get; set; }

        /// <summary>
        /// Content of timeline. Should contain <c>TimelineItem</c> elements
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal IList<TimelineItem> _items = new List<TimelineItem>();

        protected IList<TimelineItem> _displayItems = new List<TimelineItem>();

        private OneOf<string, RenderFragment> _pending;
        private bool _reverse;
        private bool _waitingItemUpdate = false;
        private TimelineMode? _mode;
        private bool _hasLabel;
        private IReadOnlyList<TimelineItemData> _dataItems;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetClassMap();
        }

        private void SetItems()
        {
            var pitems = _pendingItem != null ? new[] { _pendingItem } : Array.Empty<TimelineItem>();
            var items = Items?.Select(CreateTimelineItem).ToList() ?? _items.ToList();
            _hasLabel = items.Any(item => !string.IsNullOrEmpty(item.Label) || item.TitleContent != null);
            SetClassMap();
            _displayItems = Reverse ? pitems.Concat(UpdateChildren(items.AsEnumerable().Reverse())).ToList() : UpdateChildren(items).Concat(pitems).ToList();
        }

        private static TimelineItem CreateTimelineItem(TimelineItemData data)
        {
            if (data == null) return new TimelineItem();

            var item = new TimelineItem
            {
                Label = data.Title,
                TitleContent = data.TitleContent ?? TextContent(data.Title),
                ChildContent = data.ContentContent ?? TextContent(data.Content),
                Dot = data.Icon ?? (data.Loading ? _loadingDot : null),
                Class = data.Class,
                Style = data.Style,
                Placement = data.Placement
            };

            if (data.Color.HasValue)
            {
                item.Color = data.Color.Value;
            }

            return item;
        }

        private static RenderFragment TextContent(string content) => string.IsNullOrEmpty(content)
            ? null
            : builder => builder.AddContent(0, content);

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender || _waitingItemUpdate)
            {
                _waitingItemUpdate = false;
                SetItems();
                StateHasChanged();
            }
        }

        protected void SetClassMap()
        {
            var prefix = "ant-timeline";
            ClassMapper.Clear()
                .Add(Orientation == TimelineOrientation.Horizontal
                    ? "ant-steps ant-steps-horizontal ant-steps-title-vertical"
                    : "ant-steps ant-steps-vertical ant-steps-title-horizontal")
                .Add(Variant == TimelineVariant.Filled ? "ant-steps-filled" : "ant-steps-outlined")
                .Add("ant-steps-dot")
                .Add(prefix)
                .Add("css-var-root ant-timeline-css-var")
                .If($"{prefix}-horizontal", () => Orientation == TimelineOrientation.Horizontal)
                .If($"{prefix}-layout-alternate", () => Mode == TimelineMode.Alternate || _hasLabel)
                .If($"{prefix}-pending", () => Pending.Value != null)
                .If($"{prefix}-reverse", () => Reverse)
                .If($"{prefix}-rtl", () => RTL);
        }

        protected string GetRootStyle()
        {
            const string stepsOffset = "--ant-cmp-steps-items-offset: 0;";
            var titleSpanStyle = TitleSpan.IsT0
                ? TitleSpan.AsT0 == 12 ? string.Empty : $"--ant-timeline-head-span: {TitleSpan.AsT0.ToString(CultureInfo.InvariantCulture)};"
                : $"--ant-timeline-head-span-ptg: {TitleSpan.AsT1};";
            return $"{stepsOffset}{titleSpanStyle}{Style}";
        }

        internal string SemanticClass(string slot, string baseClass)
        {
            if (ClassNames == null || !ClassNames.TryGetValue(slot, out var customClass) || string.IsNullOrWhiteSpace(customClass))
                return baseClass;
            return string.IsNullOrWhiteSpace(baseClass) ? customClass : $"{baseClass} {customClass}";
        }

        internal string SemanticStyle(string slot, string baseStyle = null)
        {
            var semanticStyle = Styles != null && Styles.TryGetValue(slot, out var value) ? value : null;
            if (string.IsNullOrWhiteSpace(baseStyle)) return semanticStyle;
            if (string.IsNullOrWhiteSpace(semanticStyle)) return baseStyle;
            return $"{baseStyle.Trim().TrimEnd(';')}; {semanticStyle}";
        }

        internal void RemoveItem(TimelineItem item)
        {
            this._items.Remove(item);
            _waitingItemUpdate = true;
        }

        internal void AddItem(TimelineItem item)
        {
            this._items.Add(item);
            _waitingItemUpdate = true;
            if (!string.IsNullOrEmpty(item.Label))
            {
                _hasLabel = true;
            }
            SetClassMap();
        }

        protected IEnumerable<TimelineItem> UpdateChildren(IEnumerable<TimelineItem> items)
        {
            if (!items.Any())
                yield break;

            var length = items.Count();
            for (int i = 0; i < length; i++)
            {
                var item = items.ElementAt(i);
                item.IsLast = i == length - 1;
                item.Position = item.Placement ?? _mode switch
                {
                    TimelineMode.Left => TimelineMode.Left,
                    TimelineMode.Right => TimelineMode.Right,
                    TimelineMode.Alternate => i % 2 == 0 ? TimelineMode.Left : TimelineMode.Right,
                    _ => TimelineMode.Left,
                };
                item.SetClassMap();

                yield return item;
            }
        }
    }
}
