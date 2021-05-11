using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;
using System.Text;

namespace AntDesign
{
    public partial class Tag : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        ///  'default' | 'closeable' | 'checkable'
        /// </summary>
        [Parameter]
        public string Mode { get; set; } = "default";

        //Here we keep the orginal string so we can support custom colors and inverse
        [Parameter]
        public OneOf<string, TagColor> Color
        {
            get => _color;
            set
            {
                if (value.IsT0) {
                    _color = value.AsT0;
                    _presetColor = IsPresetColor(_color);
                    _customColor = IsCustomColor(_color);    
                } 
                else 
                {
                    _color = value.AsT1.ToString();
                    _presetColor = true;
                    _customColor = false;
                }
            }
        }

        [Parameter]
        public bool Closable { get; set; }
        
        [Parameter]
        public bool Checkable { get; set; }

        [Parameter]
        public bool Visible { get; set; } = true;

        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public string Icon { get; set; }

        [Parameter]
        public bool NoAnimation { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClose { get; set; }

        /// <summary>
        /// Triggered before true closing, can prevent the closing
        /// </summary>
        [Parameter]
        public EventCallback<CloseEventArgs<MouseEventArgs>> OnClosing { get; set; }

        [Parameter]
        public EventCallback<bool> CheckedChange { get; set; }

        [Parameter]
        public EventCallback OnClick { get; set; }

        private bool _presetColor;
        private bool _customColor;
        private bool _closed;
        private string _color;

        protected override void OnInitialized()
        {
            this.UpdateClassMap();
            base.OnInitialized();
        }

        private static bool IsPresetColor(string color)
        {
            if (string.IsNullOrEmpty(color))
            {
                return false;
            }

            bool result = Regex.IsMatch(color, "^(pink|red|yellow|orange|cyan|green|blue|purple|geekblue|magenta|volcano|gold|lime)(-inverse)?$");
            return result;
        }

        private static bool IsCustomColor(string color)
        {
            if (string.IsNullOrEmpty(color))
            {
                return false;
            }

            return color.StartsWith("#");
        }

        private void UpdateClassMap()
        {
            string prefix = "ant-tag";
            this.ClassMapper.Add(prefix)
                .If($"{prefix}-has-color", () => !string.IsNullOrEmpty(Color.AsT0) && !_presetColor)
                .If($"{prefix}-hidden", () => Visible == false)
                .GetIf(() => $"{prefix}-{_color}", () => _presetColor)
                .If($"{prefix}-checkable", () => Checkable)
                .If($"{prefix}-checkable-checked", () => Checked)
                .If($"{prefix}-rtl", () => RTL)
                ;
        }

        private string GetStyle() {
            StringBuilder style = new StringBuilder();

            style.Append(style);

            if (!string.IsNullOrEmpty(Style) && !Style.EndsWith(";")) {
                style.Append(";");
            }

            if (_customColor) {
                style.Append($"background-color: {_color};");
            }

            return style.ToString();
        }

        private async Task UpdateCheckedStatus()
        {
            if (Mode == "checkable")
            {
                this.Checked = !this.Checked;
                await this.CheckedChange.InvokeAsync(this.Checked);
            }
        }

        private async Task CloseTag(MouseEventArgs e)
        {
            var closeEvent = new CloseEventArgs<MouseEventArgs>(e);
            await this.OnClosing.InvokeAsync(closeEvent);
            if (closeEvent.Cancel)
            {
                return;
            }
            await this.OnClose.InvokeAsync(e);
            this._closed = true;
        }

        private async Task ClickTag(MouseEventArgs e)
        {
            await this.UpdateCheckedStatus();

            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(this);
            }
        }
    }
}
