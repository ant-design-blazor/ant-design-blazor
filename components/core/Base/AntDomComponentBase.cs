using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public abstract class AntDomComponentBase : AntComponentBase
    {
        [Parameter]
        public string Id { get; set; } = IdGeneratorHelper.Generate("ant-blazor-");

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

        [CascadingParameter]
        public AntTheme AntTheme { get; set; }

        protected ClassMapper ClassMapper { get; } = new ClassMapper();

        protected AntDomComponentBase()
        {
            ClassMapper
                .Get(() => this.Class)
                .Get(() => this.AntTheme?.GetClass());
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
        /// Specifies an inline style for an DOM element.
        /// </summary>
        [Parameter]
        public string Style
        {
            get => _style;
            set
            {
                _style = value;
                this.StateHasChanged();
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
