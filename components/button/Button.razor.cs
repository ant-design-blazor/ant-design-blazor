using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Button : AntDomComponentBase
    {
        [Parameter]
        public bool Block { get; set; } = false;

        [Parameter]
        public bool Ghost { get; set; } = false;

        [Parameter]
        public bool Search { get; set; } = false;

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

        [Parameter]
        public ButtonType Type { get; set; } = ButtonType.Default;

        [Parameter]
        public string HtmlType { get; set; } = "button";

        [Parameter]
        public ButtonShape? Shape { get; set; } = null;

        private bool _animating = false;

        private string _btnWave = "--antd-wave-shadow-color: rgb(255, 120, 117);";

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

                Size = value switch
                {
                    "large" => ButtonSize.Large,
                    "middle" => ButtonSize.Middle,
                    _ => ButtonSize.Middle,
                };
            }
        }

        [Parameter]
        public ButtonSize Size { get; set; } = ButtonSize.Middle;

        [Parameter]
        public string Icon { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool Danger { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        [Parameter]
        public bool OnClickStopPropagation { get; set; }

        public IList<Icon> Icons { get; set; } = new List<Icon>();

        protected string IconStyle { get; set; }

        private bool _loading = false;

        protected void SetClassMap()
        {
            var prefixName = "ant-btn";

            ClassMapper.Clear()
                .Add(prefixName)
                .GetIf(() => $"{prefixName}-{this.Type.ToDescription()}", () => !string.IsNullOrEmpty(Type.ToDescription()))
                .If($"{prefixName}-dangerous", () => Danger)
                .GetIf(() => $"{prefixName}-{Shape.ToDescription()}", () => !string.IsNullOrEmpty(Shape.ToDescription()))
                .If($"{prefixName}-lg", () => Size == ButtonSize.Large)
                .If($"{prefixName}-sm", () => Size == ButtonSize.Small)
                .If($"{prefixName}-loading", () => Loading)
                .If($"{prefixName}-icon-only", () => !string.IsNullOrEmpty(this.Icon) && !this.Search && this.ChildContent == null)
                .If($"{prefixName}-background-ghost", () => Ghost)
                .If($"{prefixName}-block", () => this.Block)
                .If($"ant-input-search-button", () => this.Search)
                .If($"{prefixName}-rtl", () => RTL)
                ;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
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
    }
}
