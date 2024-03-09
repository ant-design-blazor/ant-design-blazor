using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public abstract class AntDomComponentBase : AntComponentBase
    {
        [Inject]
        private IComponentIdGenerator ComponentIdGenerator { get; set; }

        [Parameter]
        public string Id { get; set; }

        [CascadingParameter]
        public ConfigProvider ConfigProvider { get; set; }

        protected bool RTL => ConfigProvider?.Direction == "RTL";

        internal virtual string PrefixClassName { get; }

        //[Parameter(CaptureUnmatchedValues = true)]
        //public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

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

        internal ClassMapper ClassMapper { get; } = new ClassMapper();

        protected AntDomComponentBase()
        {
        }

        protected override void OnInitialized()
        {
            Id ??= ComponentIdGenerator.Generate(this);
            base.OnInitialized();
        }

        /// <summary>
        /// Specifies one or more class names for an DOM element.
        /// </summary>
        [Parameter]
        public string Class
        {
            get => _class;
            set
            {
                _class = value;
                ClassMapper.OriginalClass = value;
            }
        }

        /// <summary>
        /// Specifies an inline style for a DOM element.
        /// </summary>
        [Parameter]
        public string Style
        {
            get => _style;
            set
            {
                _style = value;
                if (!string.IsNullOrWhiteSpace(_style) && !_style.EndsWith(";"))
                {
                    _style += ";";
                }
                //this.StateHasChanged();
            }
        }

        protected virtual string GenerateStyle()
        {
            return Style;
        }

        private string _class;
        private string _style;
    }
}
