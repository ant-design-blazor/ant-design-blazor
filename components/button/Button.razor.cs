using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Button : AntDomComponentBase
    {
        private string _formSize;

        [CascadingParameter(Name = "FormSize")]
        public string FormSize
        {
            get
            {
                return _formSize;
            }
            set
            {
                _formSize = value;

                Size = value;
            }
        }

        /// <summary>
        /// Set the color of the button.
        /// </summary>
        [Parameter]
        public Color Color { get; set; } = Color.None;

        /// <summary>
        /// Option to fit button width to its parent width
        /// </summary>
        [Parameter]
        public bool Block { get; set; } = false;

        /// <summary>
        /// Content of the button.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Set the danger status of button.
        /// </summary>
        [Parameter]
        public bool Danger { get; set; }

        /// <summary>
        ///  Whether the `Button` is disabled.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Make background transparent and invert text and border colors
        /// </summary>
        [Parameter]
        public bool Ghost { get; set; } = false;

        /// <summary>
        /// Set the original html type of the button element.
        /// </summary>
        [Parameter]
        public string HtmlType { get; set; } = "button";

        /// <summary>
        /// Set the icon component of button.
        /// </summary>
        [Parameter]
        public string Icon { get; set; }

        /// <summary>
        ///  Show loading indicator. You have to write the loading logic on your own. 
        /// </summary>
        [Parameter]
        public bool Loading
        {
            get => _loading;
            set
            {
                if (_loading != value)
                {
                    _loading = value;
                    UpdateIconDisplay(_loading);
                }
            }
        }

        /// <summary>
        /// Callback when `Button` is clicked
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        /// <summary>
        /// Do not propagate events when button is clicked.
        /// </summary>
        [Parameter]
        public bool OnClickStopPropagation { get; set; }

        /// <summary>
        /// Can set button shape: `circle` | `round` or `null` (default, which is rectangle).
        /// </summary>
        [Parameter]
        public string Shape { get; set; } = null;

        /// <summary>
        /// Set the size of button.
        /// </summary>
        [Parameter]
        public string Size { get; set; } = AntSizeLDSType.Default;

        /// <summary>
        ///  Type of the button.
        /// </summary>
        [Parameter]
        public string Type { get; set; } = ButtonType.Default;

        public IList<Icon> Icons { get; set; } = new List<Icon>();

        protected string IconStyle { get; set; }

        private bool _animating = false;

        private string _btnWave = "--antd-wave-shadow-color: rgb(255, 120, 117);";
        private bool _loading = false;

        protected void SetClassMap()
        {
            var prefixName = "ant-btn";

            ClassMapper.Clear()
                .Add(prefixName)
                .GetIf(() => $"{prefixName}-{this.Type}", () => !string.IsNullOrEmpty(Type))
                .If($"{prefixName}-dangerous", () => Danger)
                .GetIf(() => $"{prefixName}-{Shape}", () => !string.IsNullOrEmpty(Shape))
                .If($"{prefixName}-lg", () => Size == "large")
                .If($"{prefixName}-sm", () => Size == "small")
                .If($"{prefixName}-loading", () => Loading)
                .If($"{prefixName}-icon-only", () => !string.IsNullOrEmpty(this.Icon) && this.ChildContent == null)
                .If($"{prefixName}-background-ghost", () => Ghost)
                .If($"{prefixName}-block", () => this.Block)
                .If($"{prefixName}-rtl", () => RTL)
                ;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
            SetButtonColorStyle();
            UpdateIconDisplay(_loading);
        }

        private void UpdateIconDisplay(bool loading)
        {
            IconStyle = $"display:{(loading ? "none" : "inline-block")}";
        }

        private async Task HandleOnClick(MouseEventArgs args)
        {
            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(args);
            }
        }

        private async Task OnMouseUp(MouseEventArgs args)
        {
            if (args.Button != 0 || this.Type == ButtonType.Link) return; //remove animating from Link Button
            this._animating = true;

            await Task.Run(async () =>
            {
                await Task.Delay(500);
                this._animating = false;

                await InvokeAsync(StateHasChanged);
            });
        }

        private void SetButtonColorStyle()
        {
            if (Color != Color.None)
            {
                Style += ColorHelper.GetBackgroundStyle(Color);
            }
        }
    }
}
