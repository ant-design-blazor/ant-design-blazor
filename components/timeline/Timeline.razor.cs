// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
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

                    _pendingItem?.SetClassMap();
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

        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetClassMap();
        }

        private void SetItems()
        {
            var pitems = _pendingItem != null ? new[] { _pendingItem } : Array.Empty<TimelineItem>();
            _displayItems = Reverse ? pitems.Concat(UpdateChildren(_items.Reverse())).ToList() : UpdateChildren(_items).Concat(pitems).ToList();
        }

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
            var hashId = UseStyle(prefix, TimelineStyle.UseComponentStyle);
            ClassMapper.Clear()
                .Add(prefix)
                .Add(hashId)
                .If($"{prefix}-right", () => Mode == TimelineMode.Right && !_hasLabel)
                .If($"{prefix}-alternate", () => Mode == TimelineMode.Alternate && !_hasLabel)
                .If($"{prefix}-pending", () => Pending.Value != null)
                .If($"{prefix}-reverse", () => Reverse)
                .If($"{prefix}-label", () => _hasLabel)
                .If($"{prefix}-rtl", () => RTL);
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
                item.Position = _mode switch
                {
                    TimelineMode.Left => TimelineMode.Left,
                    TimelineMode.Right => TimelineMode.Right,
                    TimelineMode.Alternate => i % 2 == 0 ? TimelineMode.Left : TimelineMode.Right,
                    _ => null,
                };

                item.SetClassMap();

                yield return item;
            }
        }
    }
}
