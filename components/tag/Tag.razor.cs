using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AntDesign.Core.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Tag : AntDomComponentBase
    {
        /// <summary>
        /// Animate transitions - closing and adding.
        /// </summary>
        [Parameter]
        public bool Animate { get; set; }

        /// <summary>
        /// Tag content
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Whether the Tag can be closed
        /// </summary>
        [Parameter]
        public bool Closable { get; set; }

        /// <summary>
        /// Whether the Tag can be checked
        /// </summary>
        [Parameter]
        public bool Checkable { get; set; }

        /// <summary>
        /// Checked status of Tag
        /// </summary>
        [Parameter]
        public bool Checked { get; set; }

        /// <summary>
        /// Callback executed when Tag is checked/unchecked
        /// </summary>
        [Parameter]
        public EventCallback<bool> CheckedChange { get; set; }

        /// <summary>
        /// Tag color. Can either be a predefined color (string)
        /// or hex color.
        /// </summary>
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
                    if (_presetColor)
                    {
                        _style = Style;
                    }
                    else
                    {
                        _style = $"background-color: {_color};{Style}";
                    }
                }
            }
        }

        /// <summary>
        /// Set the tag's icon 
        /// </summary>
        [Parameter]
        public string Icon { get; set; }

        /// <summary>
        /// Callback executed when tag is closed
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClose { get; set; }

        /// <summary>
        /// Triggered before true closing, can prevent the closing
        /// </summary>
        [Parameter]
        public EventCallback<CloseEventArgs<MouseEventArgs>> OnClosing { get; set; }

        /// <summary>
        /// Callback executed when tag is clicked (it is not called 
        /// when closing icon is clicked).
        /// </summary>
        [Parameter]
        public EventCallback OnClick { get; set; }

        /// <summary>
        /// Whether the Tag is closed or not
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;


        private bool _presetColor;
        private bool _closed;
        private string _color;
        private string _style;

        protected override void OnInitialized()
        {
            this.UpdateClassMap();
            base.OnInitialized();
            if (Animate)
            {
                AnimationCls = $"{_prefix}-animate-grow";
            }
        }

        protected string AnimationCls { get; set; } = "";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && Animate)
            {
                if (Js.IsBrowser())
                {
                    await Task.Delay(900);
                }
                AnimationCls = "";
                await InvokeAsync(StateHasChanged);

            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private static bool IsPresetColor(string color)
        {
            if (string.IsNullOrEmpty(color))
            {
                return false;
            }

            bool result = Regex.IsMatch(color, "^(pink|red|yellow|orange|cyan|green|blue|purple|geekblue|magenta|volcano|gold|lime|success|processing|error|warning|default)(-inverse)?$");
            return result;
        }
        private string _prefix = "ant-tag";
        private void UpdateClassMap()
        {
            this.ClassMapper.Add(_prefix)
                .If($"{_prefix}-has-color", () => !string.IsNullOrEmpty(Color) && !_presetColor)
                .If($"{_prefix}-hidden", () => Visible == false)
                .GetIf(() => $"{_prefix}-{Color}", () => _presetColor)
                .If($"{_prefix}-checkable", () => Checkable)
                .If($"{_prefix}-checkable-checked", () => Checked)
                .If($"{_prefix}-rtl", () => RTL)
                ;
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

            if (Animate)
            {
                AnimationCls = $"{_prefix}-animate-shrink";
                await Task.Delay(300);
            }

            this._closed = true;
            AnimationCls = "";

            if (OnClose.HasDelegate)
            {
                await this.OnClose.InvokeAsync(e);
            }
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
