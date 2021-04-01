using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

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

        [Parameter]
        public string Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    _presetColor = IsPresetColor(_color);
                }
            }
        }

        [Parameter]
        public bool Closable { get; set; }

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
            if (!result) result = Regex.IsMatch(color, "^#([0-9a-fA-F]{6}|[0-9a-fA-F]{3})$");
            return result;
        }

        private void UpdateClassMap()
        {
            string prefix = "ant-tag";
            this.ClassMapper.Add(prefix)
                .If($"{prefix}-has-color", () => !string.IsNullOrEmpty(Color) && !_presetColor)
                .If($"{prefix}-hidden", () => Visible == false)
                .GetIf(() => $"{prefix}-{Color}", () => _presetColor)
                .If($"{prefix}-checkable", () => Mode == "checkable")
                .If($"{prefix}-checkable-checked", () => Checked)
                .If($"{prefix}-rtl", () => RTL)
                ;
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
