using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class Timeline : AntDomComponentBase
    {
        /// <summary>
        /// 'left' | 'alternate' | 'right'
        /// </summary>
        [Parameter]
        public string Mode
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

        [Parameter]
        public OneOf<string, RenderFragment> Pending
        {
            get => _pending;
            set
            {
                if (_pending.Value != value.Value)
                {
                    _pending = value;

                    _pendingItem = _pending.Value == null ? null : new TimelineItem()
                    {
                        Dot = PendingDot ?? _loadingDot,
                        Class = "ant-timeline-item-pending"
                    };

                    _pending.Switch(str =>
                    {
                        _pendingItem.ChildContent = b =>
                        {
                            b.AddContent(0, str);
                        };
                    }, rf =>
                    {
                        _pendingItem.ChildContent = rf;
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

        [Parameter]
        public RenderFragment PendingDot { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal IList<TimelineItem> _items = new List<TimelineItem>();

        protected IList<TimelineItem> _displayItems = new List<TimelineItem>();

        private readonly bool _isPendingBoolean = false;
        private OneOf<string, RenderFragment> _pending;
        private bool _reverse;
        private bool _waitingItemUpdate = false;
        private string _mode;

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
            ClassMapper.Clear()
                .Add(prefix)
                .If($"{prefix}-right", () => Mode == "right")
                .If($"{prefix}-alternate", () => Mode == "alternate")
                .If($"{prefix}-pending", () => Pending.Value != null)
                .If($"{prefix}-reverse", () => Reverse);
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
                item.Position =
                    this.Mode == "left" || Mode == null ? null
                    : this.Mode == "right" ? "right"
                    : this.Mode == "alternate" && i % 2 == 0 ? "left"
                    : "right";

                item.SetClassMap();

                yield return item;
            }
        }
    }
}
