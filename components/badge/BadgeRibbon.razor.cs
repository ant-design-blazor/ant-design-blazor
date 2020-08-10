using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// Small numerical value or status descriptor for UI elements.
    /// </summary>
    public partial class BadgeRibbon : AntDomComponentBase
    {
        /// <summary>
        /// Customize ribbon color
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Set text contents of ribbon.
        /// </summary>
        [Parameter]
        public OneOf<string, RenderFragment> Text { get; set; }

        /// <summary>
        /// Set placement of ribbon.
        /// </summary>
        [Parameter]
        public string Placement { get; set; } = "end";

        /// <summary>
        /// Wrapping this item.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }


        private string PresetColor => Color.IsIn(_badgePresetColors) ? Color : null;

        private string colorStyle;

        private string cornerColorStyle;

        /// <summary>
        /// Sets the default CSS classes.
        /// </summary>
        private void SetClassMap()
        {
            var prefixName = "ant-ribbon";
            ClassMapper.Clear()
                .Add(prefixName)
                .Add($"{prefixName}-placement-{Placement}")
                //.If($"{prefixName}-rtl", () => Direction == "RTL" # Placeholder for when RTL support is added
                .If($"{prefixName}-color-{PresetColor}", () => Color.IsIn(_badgePresetColors))
                ;
        }


        private void SetStyle()
        {
            if (PresetColor == null && !string.IsNullOrWhiteSpace(Color))
            {
                colorStyle = $"background:{Color}; {Style}";
                cornerColorStyle = $"color:{Color}; {Style}";
            }
            else
            {
                colorStyle = Style;
                cornerColorStyle = Style;
            }
        }

        /// <summary>
        /// Startup code
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
            SetStyle();
        }

        /// <summary>
        /// Runs every time a parameter is set.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClassMap();
            SetStyle();
        }

        private readonly string[] _badgePresetColors =
        {
            "pink", "red", "yellow", "orange", "cyan", "green", "blue", "purple", "geekblue", "magenta", "volcano",
            "gold", "lime"
        };
    }
}
