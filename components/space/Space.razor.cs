using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class Space : AntDomComponentBase
    {
        /// <summary>
        /// start | end |center |baseline
        /// </summary>
        [Parameter]
        public string Align { get; set; }

        [Parameter]
        public DirectionVHType Direction { get; set; } = DirectionVHType.Horizontal;

        [Parameter]
        public OneOf<string, (string, string)> Size
        {
            get { return _size; }
            set
            {
                _size = value;
                _items.ForEach(x => x.ChangeSize());
                StateHasChanged();
            }
        }

        [Parameter]
        public bool Wrap { get; set; }

        [Parameter]
        public RenderFragment Split { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal int SpaceItemMaxIndex => _items.Max(x => x.Index);

        private IList<SpaceItem> _items = new List<SpaceItem>();

        private bool HasAlign => Align.IsIn("start", "end", "center", "baseline");

        private const string PrefixCls = "ant-space";
        private OneOf<string, (string, string)> _size = "small";

        private string InnerStyle => Wrap && Direction == DirectionVHType.Horizontal ? "flex-wrap: wrap;" : "";

        public void SetClass()
        {
            ClassMapper
                .Add(PrefixCls)
                .GetIf(() => $"{PrefixCls}-{Direction.Name.ToLowerInvariant()}", () => Direction.IsIn(DirectionVHType.Horizontal, DirectionVHType.Vertical))
                .GetIf(() => $"{PrefixCls}-align-{Align}", () => HasAlign)
                .If($"{PrefixCls}-align-center", () => !HasAlign && Direction == DirectionVHType.Horizontal)
                .If($"{PrefixCls}-rtl", () => RTL);
        }

        protected override void OnInitialized()
        {
            SetClass();
            base.OnInitialized();
        }

        internal void AddSpaceItem(SpaceItem item)
        {
            _items.Add(item);
            item.SetIndex(_items.Count - 1);
            _items.ForEach(x => x.ChangeSize());
        }

        internal void RemoveSpaceItem(SpaceItem item)
        {
            if (_items.Contains(item))
            {
                _items.Remove(item);
                _items.ForEach(x => x.ChangeSize());
            }
        }
    }
}
