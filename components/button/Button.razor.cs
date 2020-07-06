using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
        public string Type { get; set; } = ButtonType.Default;

        [Parameter]
        public string HtmlType { get; set; } = "button";

        [Parameter]
        public string Shape { get; set; } = null;

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

                Size = value;
            }
        }

        [Parameter]
        public string Size { get; set; } = AntSizeLDSType.Default;

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

        [Inject] private NavigationManager NavigationManger { get; set; }

        public IList<Icon> Icons { get; set; } = new List<Icon>();

        protected string IconStyle { get; set; }

        private bool _loading = false;

        private System.Timers.Timer _delayTimer;
        private static int _timer = 0;   //解决重入的问题

        protected void SetClassMap()
        {
            string prefixName = "ant-btn";

            ClassMapper.Clear()
                .Add("ant-btn")
                .If($"{prefixName}-{this.Type}", () => !string.IsNullOrEmpty(Type))
                .If($"{prefixName}-dangerous", () => Danger)
                .If($"{prefixName}-{Shape}", () => !string.IsNullOrEmpty(Shape))
                .If($"{prefixName}-lg", () => Size == "large")
                .If($"{prefixName}-sm", () => Size == "small")
                .If($"{prefixName}-loading", () => Loading)
                .If($"{prefixName}-icon-only", () => !string.IsNullOrEmpty(this.Icon) && !this.Search && this.ChildContent == null)
                .If($"{prefixName}-background-ghost", () => Ghost)
                .If($"{prefixName}-block", () => this.Block)
                .If($"ant-input-search-button", () => this.Search)
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

            await Task.Run(() =>
            {
                Thread.Sleep(500);
                this._animating = false;
                //Console.WriteLine("Time 1 end"+ DateTime.Now);
            });
            await InvokeAsync(StateHasChanged);
        }
    }
}
