using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Text;

namespace AntDesign
{
    public partial class Tag : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Color
        {
            get => _color;
            set
            {
                _color = value;
                _isPresetColor = IsPresetColor(_color);
                _isCustomColor = !_isPresetColor; //if it's not a preset color, we can assume that the input is a HTML5 color or Hex or RGB value  
                _style = GetStyle();    
            }
        }

        [Parameter]
        public PresetColor? PresetColor 
        { 
            get 
            {
                object result;

                if (Enum.TryParse(typeof(PresetColor), _color, true, out result) == false) {
                    return null;
                }
            
                return (PresetColor)result;
            }
            set 
            { 
                Color = Enum.GetName(typeof(PresetColor), value).ToLowerInvariant();
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
        private bool _isPresetColor;
        private bool _isCustomColor;
        private bool _closed;
        private string _color;
        private string _style;

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

        private void UpdateClassMap()
        {
            string prefix = "ant-tag";
            this.ClassMapper.Add(prefix)
                .If($"{prefix}-has-color", () => _isCustomColor)
                .If($"{prefix}-hidden", () => Visible == false)
                .GetIf(() => $"{prefix}-{_color}", () => _isPresetColor)
                .If($"{prefix}-checkable", () => Checkable)
                .If($"{prefix}-checkable-checked", () => Checked)
                .If($"{prefix}-rtl", () => RTL)
                ;
        }

        private string GetStyle() {
            StringBuilder styleBuilder = new StringBuilder();

            styleBuilder.Append(Style);

            if (!string.IsNullOrEmpty(Style) && !Style.EndsWith(";")) {
                styleBuilder.Append(";");
            }

            if (_isCustomColor) {
                styleBuilder.Append($"background-color: {_color};");
            }

            return styleBuilder.ToString();
        }

        private async Task UpdateCheckedStatus()
        {
            if (!Checkable)
            {
                return;
            }

            this.Checked = !this.Checked;
            if (this.CheckedChange.HasDelegate)
            {
                await this.CheckedChange.InvokeAsync(this.Checked);
            }
        }

        private async Task CloseTag(MouseEventArgs e)
        {
            var closeEvent = new CloseEventArgs<MouseEventArgs>(e);

            if (OnClosing.HasDelegate)
            {
                await this.OnClosing.InvokeAsync(closeEvent);
            }

            if (closeEvent.Cancel)
            {
                return;
            }

            if (OnClose.HasDelegate)
            {
                await this.OnClose.InvokeAsync(e);
            }

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
