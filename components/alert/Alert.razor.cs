// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /// <summary>
    /// Alert component for feedback.
    /// </summary>
    public partial class Alert : AntDomComponentBase
    {
        /// <summary>
        /// Called when close animation is finished
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> AfterClose { get; set; }

        /// <summary>
        /// Whether to show as banner
        /// </summary>
        [Parameter]
        public bool Banner { get; set; } = false;

        /// <summary>
        /// Whether Alert can be closed
        /// </summary>
        [Parameter]
        public bool Closable { get; set; } = false;

        /// <summary>
        /// Close text to show
        /// </summary>
        [Parameter]
        public string CloseText { get; set; }

        /// <summary>
        /// Additional content of Alert
        /// </summary>
        [Parameter]
        public string Description { get; set; }

        /// <summary>
        /// Custom icon, effective when showIcon is true
        /// </summary>
        [Parameter]
        public RenderFragment Icon { get; set; }

        /// <summary>
        /// Content of Alert
        /// </summary>
        [Parameter]
        public string Message { get; set; }

        [Parameter]
        public RenderFragment MessageTemplate { get; set; }

        /// <summary>
        /// Whether to show icon.
        /// </summary>
        [Parameter]
        public bool? ShowIcon { get; set; }

        /// <summary>
        /// Type of Alert styles, options: success, info, warning, error
        /// </summary>
        [Parameter]
        public string Type { get; set; }

        /// <summary>
        /// Callback when Alert is closed.
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClose { get; set; }

        /// <summary>
        /// Additional Content
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Icon to show.
        /// </summary>
        protected string IconType => CalcType switch
        {
            AlertType.Success => "check-circle",
            AlertType.Info => "info-circle",
            AlertType.Warning => "exclamation-circle",
            AlertType.Error => "close-circle",
            _ => "exclamation-circle",
        };

        /// <summary>
        /// Indicator if the component is closed or not.
        /// </summary>
        private bool _isClosed;

        /// <summary>
        /// Just before we close the component we set this indicator to show a closing animation.
        /// </summary>
        private bool _isClosing;

        private int _motionStage;

        private decimal _height;

        private string _innerStyle = string.Empty;

        private bool IsShowIcon => (Banner && ShowIcon == null) ? true : ShowIcon == true;

        private string CalcType => Type ?? (Banner ? AlertType.Warning : AlertType.Info);

        /// <summary>
        /// Sets the default classes.
        /// </summary>
        private void SetClassMap()
        {
            string prefixName = "ant-alert";
            ClassMapper
                .Add("ant-alert")
                .GetIf(() => $"{prefixName}-{CalcType}", () => !string.IsNullOrEmpty(CalcType))
                .If($"{prefixName}-no-icon", () => !IsShowIcon)
                .If($"{prefixName}-closable", () => Closable)
                .If($"{prefixName}-banner", () => Banner)
                .If($"{prefixName}-with-description", () => !string.IsNullOrEmpty(Description) || ChildContent != null)
                .If($"{prefixName}-motion", () => _isClosing)
                .If($"{prefixName}-motion-leave", () => _isClosing)
                .If($"{prefixName}-motion-leave-active", () => _isClosing && _motionStage == 1)
                .If($"{prefixName}-rtl", () => RTL)
                ;
        }

        /// <summary>
        /// Start-up code.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetClassMap();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                HtmlElement element = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, Ref);
                _height = element.ClientHeight;
            }
        }

        /// <summary>
        /// Handles the close callback.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected async Task OnCloseHandler(MouseEventArgs args)
        {
            if (OnClose.HasDelegate)
            {
                await OnClose.InvokeAsync(args);
            }

            await PlayMotion();

            if (AfterClose.HasDelegate)
            {
                await AfterClose.InvokeAsync(args);
            }
        }

        private async Task PlayMotion()
        {
            _isClosing = true;
            _innerStyle = $"max-height:{_height}px;";
            await InvokeAsync(StateHasChanged);

            _motionStage = 1;
            await InvokeAsync(StateHasChanged);

            await Task.Delay(50);

            _innerStyle = string.Empty;
            await InvokeAsync(StateHasChanged);

            await Task.Delay(300);
            _isClosed = true;
        }
    }
}
