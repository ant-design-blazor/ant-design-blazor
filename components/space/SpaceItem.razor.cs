using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class SpaceItem : AntDomComponentBase
    {
        [CascadingParameter]
        public Space Parent { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private static readonly Dictionary<string, string> _spaceSize = new()
        {
            ["small"] = "8",
            ["middle"] = "16",
            ["large"] = "24"
        };

        private string _marginStyle = "";
        private int _index;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper.Add("ant-space-item");
        }

        protected override void OnParametersSet()
        {
            _index = Parent.SpaceItemCount++;
            base.OnParametersSet();
        }

        private void ChangeSize()
        {
            var size = Parent.Size;
            var direction = Parent.Direction;

            size.Switch(sigleSize =>
            {
                _marginStyle = direction == DirectionVHType.Horizontal ? (_index != Parent.SpaceItemCount - 1 ? $"margin-right:{GetSize(sigleSize)};" : "") : $"margin-bottom:{GetSize(sigleSize)};";
            },
            arraySize =>
            {
                _marginStyle = (_index != Parent.SpaceItemCount - 1 ? $"margin-right:{GetSize(arraySize.Item1)};" : "") + $"margin-bottom:{GetSize(arraySize.Item2)};";
            });
        }

        private CssSizeLength GetSize(string size)
        {
            var originalSize = size.IsIn(_spaceSize.Keys) ? _spaceSize[size] : size;
            if (Parent?.Split != null)
            {
                return ((CssSizeLength)originalSize).Value / 2;
            }

            return originalSize;
        }
    }
}
