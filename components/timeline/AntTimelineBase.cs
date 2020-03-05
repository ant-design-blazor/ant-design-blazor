using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntTimelineBase : AntDomComponentBase
    {
        /// <summary>
        /// 'left' | 'alternate' | 'right'
        /// </summary>
        [Parameter]
        public string mode { get; set; }

        [Parameter]
        public bool reverse { get; set; }

        [Parameter]
        public RenderFragment Pending { get; set; }

        protected virtual RenderFragment LoadingDot { get; }

        private AntTimelineItem PendingItem
        {
            get
            {
                if (this.Pending == null) return null;
                var item = new AntTimelineItem()
                {
                    ChildContent = !isPendingBoolean ? Pending : null,
                    Dot = PendingDot ?? LoadingDot,
                    @class = "ant-timeline-item-pending"
                };

                item.setClassMap();
                return item;
            }
        }

        [Parameter]
        public RenderFragment PendingDot { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected IList<AntTimelineItem> Items = new List<AntTimelineItem>();

        protected IList<AntTimelineItem> displayItems
        {
            get
            {
                var pitems = PendingItem != null ? new[] { PendingItem } : new AntTimelineItem[] { };
                if (reverse)
                {
                    return pitems.Concat(updateChildren(Items.Reverse())).ToList();
                }
                else
                {
                    return updateChildren(Items).Concat(pitems).ToList();
                }
            }
        }

        protected bool isPendingBoolean;

        protected override void OnInitialized()
        {
            setClassMap();
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            setClassMap();
            base.OnParametersSet();
        }

        protected void setClassMap()
        {
            var prefix = "ant-timeline";
            ClassMapper.Clear()
                .Add(prefix)
                .If($"{prefix}-right", () => mode == "right")
                .If($"{prefix}-alternate", () => mode == "alternate")
                .If($"{prefix}-pending", () => Pending != null)
                .If($"{prefix}-reverse", () => reverse);
        }

        internal void AddItem(AntTimelineItem item)
        {
            this.Items.Add(item);
            StateHasChanged();
        }

        protected IEnumerable<AntTimelineItem> updateChildren(IEnumerable<AntTimelineItem> items)
        {
            if (!items.Any())
                yield break;

            var length = items.Count();
            for (int i = 0; i < length; i++)
            {
                var item = items.ElementAt(i);
                item.isLast = i == length - 1;
                item.position =
                    this.mode == "left" || mode == null ? null
                    : this.mode == "right" ? "right"
                    : this.mode == "alternate" && i % 2 == 0 ? "left"
                    : "right";

                item.setClassMap();

                yield return item;
            }
        }
    }
}