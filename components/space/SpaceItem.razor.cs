using System.Collections;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class SpaceItem : ComponentBase
    {
        [CascadingParameter]
        public Space Parent { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter] public ForwardRef RefBack { get; set; } = new ForwardRef();

        private ElementReference _ref;

        /// <summary>
        /// Returned ElementRef reference for DOM element.
        /// </summary>
        public virtual ElementReference Ref
        {
            get => _ref;
            set
            {
                _ref = value;
                RefBack?.Set(value);
            }
        }

        private static readonly Hashtable _spaceSize = new Hashtable()
        {
            ["small"] = 8,
            ["middle"] = 16,
            ["large"] = 24
        };

        private string _marginStyle = "";

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (Parent == null)
                return;

            var size = Parent.Size;
            var direction = Parent.Direction;

            var marginSize = size.IsIn("small", "middle", "large") ? _spaceSize[size] : size;

            _marginStyle = direction == "horizontal" ? $"margin-right:{marginSize}px;" : $"margin-bottom:{marginSize}px;";
        }
    }
}
