using System.Collections.Generic;
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
            }
        }

        [Parameter]
        public bool Wrap { get; set; }

        [Parameter]
        public RenderFragment Split { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal int SpaceItemCount { get; set; }

        private IList<SpaceItem> _items = new List<SpaceItem>();

        private bool HasAlign => Align.IsIn("start", "end", "center", "baseline");

        private const string PrefixCls = "ant-space";
        private OneOf<string, (string, string)> _size = "small";

        private string InnerStyle => Wrap && Direction == DirectionVHType.Horizontal ? "flex-wrap: wrap;" : "";

        private string _gapStyle;
        private string _verticalStyle;

        private static readonly Dictionary<string, string> _spaceSize = new()
        {
            ["small"] = "8",
            ["middle"] = "16",
            ["large"] = "24"
        };

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

        protected override void OnParametersSet()
        {
            ChangeSize();
            base.OnParametersSet();
        }

        private void ChangeSize()
        {
            var size = Size;
            var direction = Direction;

            size.Switch(sigleSize =>
            {
                _gapStyle = $"gap: {GetSize(sigleSize)}";
            },
            arraySize =>
            {
                _gapStyle = $"gap: {GetSize(arraySize.Item2)} {GetSize(arraySize.Item1)};";
            });

            _verticalStyle = direction == DirectionVHType.Vertical ? "display: flex;" : "";
        }

        private CssSizeLength GetSize(string size)
        {
            var originalSize = size.IsIn(_spaceSize.Keys) ? _spaceSize[size] : size;
            if (Split != null)
            {
                return ((CssSizeLength)originalSize).Value / 2;
            }

            return originalSize;
        }
    }
}
