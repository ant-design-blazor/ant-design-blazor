// The Razor declaration pass for these target frameworks cannot consume source generator output.
// Keep the props model available to sibling components while the normal compile generates the
// Button component's IComponentPropsReceiver implementation.
#if NETSTANDARD2_1 || NET5_0
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public sealed class ButtonProps : ComponentProps
    {
        private string _ariaLabel;
        public string AriaLabel { get => _ariaLabel; set => SetProperty(ref _ariaLabel, value, nameof(AriaLabel)); }

        private Color _color = Color.None;
        public Color Color { get => _color; set => SetProperty(ref _color, value, nameof(Color)); }

        private bool _block;
        public bool Block { get => _block; set => SetProperty(ref _block, value, nameof(Block)); }

        private RenderFragment _childContent;
        public RenderFragment ChildContent { get => _childContent; set => SetProperty(ref _childContent, value, nameof(ChildContent)); }

        private bool _danger;
        public bool Danger { get => _danger; set => SetProperty(ref _danger, value, nameof(Danger)); }

        private bool _disabled;
        public bool Disabled { get => _disabled; set => SetProperty(ref _disabled, value, nameof(Disabled)); }

        private bool _ghost;
        public bool Ghost { get => _ghost; set => SetProperty(ref _ghost, value, nameof(Ghost)); }

        private string _htmlType = "button";
        public string HtmlType { get => _htmlType; set => SetProperty(ref _htmlType, value, nameof(HtmlType)); }

        private string _icon;
        public string Icon { get => _icon; set => SetProperty(ref _icon, value, nameof(Icon)); }

        private string _iconFont;
        public string IconFont { get => _iconFont; set => SetProperty(ref _iconFont, value, nameof(IconFont)); }

        private bool _loading;
        public bool Loading { get => _loading; set => SetProperty(ref _loading, value, nameof(Loading)); }

        private bool _autoLoading;
        public bool AutoLoading { get => _autoLoading; set => SetProperty(ref _autoLoading, value, nameof(AutoLoading)); }

        private EventCallback<MouseEventArgs> _onClick;
        public EventCallback<MouseEventArgs> OnClick { get => _onClick; set => SetProperty(ref _onClick, value, nameof(OnClick)); }

        private bool _onClickStopPropagation = true;
        public bool OnClickStopPropagation { get => _onClickStopPropagation; set => SetProperty(ref _onClickStopPropagation, value, nameof(OnClickStopPropagation)); }

        private ButtonShape _shape = ButtonShape.Rectangle;
        public ButtonShape Shape { get => _shape; set => SetProperty(ref _shape, value, nameof(Shape)); }

        private ButtonSize _size = ButtonSize.Default;
        public ButtonSize Size { get => _size; set => SetProperty(ref _size, value, nameof(Size)); }

        private ButtonType? _type = ButtonType.Default;
        public ButtonType? Type { get => _type; set => SetProperty(ref _type, value, nameof(Type)); }

        private bool _noSpanWrap;
        public bool NoSpanWrap { get => _noSpanWrap; set => SetProperty(ref _noSpanWrap, value, nameof(NoSpanWrap)); }
    }
}
#endif
