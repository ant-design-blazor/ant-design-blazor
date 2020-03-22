using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace AntBlazor
{
    /// <summary>
    /// Alert component for feedback.
    /// </summary>
    public class AntAlertBase : AntDomComponentBase
    {
        /// <summary>
        /// Called when close animation is finished	
        /// </summary>
        [Parameter] public EventCallback<MouseEventArgs> AfterClose { get; set; }
        /// <summary>
        /// Whether to show as banner
        /// </summary>
        [Parameter] public bool Banner { get; set; } = false;
        /// <summary>
        /// Whether Alert can be closed	
        /// </summary>
        [Parameter] public bool Closable { get; set; } = false;
        /// <summary>
        /// Close text to show	
        /// </summary>
        [Parameter] public string CloseText { get; set; }
        /// <summary>
        /// Additional content of Alert	
        /// </summary>
        [Parameter] public string Description { get; set; }
        /// <summary>
        /// Custom icon, effective when showIcon is true	
        /// </summary>
        [Parameter] public string Icon { get; set; }
        /// <summary>
        /// Content of Aler
        /// </summary>
        [Parameter] public string Message { get; set; }
        /// <summary>
        /// Whether to show icon.
        /// </summary>
        [Parameter] public bool ShowIcon { get; set; }
        /// <summary>
        /// Type of Alert styles, options: success, info, warning, error	
        /// </summary>
        [Parameter] public string Type { get; set; } = AntAlertType.Default;
        /// <summary>
        /// Callback when Alert is closed.
        /// </summary>
        [Parameter] public EventCallback<MouseEventArgs> OnClose { get; set; }
        /// <summary>
        /// Additional Content
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Icon to show.
        /// </summary>
        protected string iconType => !string.IsNullOrEmpty(Icon) ? Icon 
                        : Type == AntAlertType.Success ? "check-circle"
                        : Type == AntAlertType.Info ? "info-circle"
                        : Type == AntAlertType.Warning ? "exclamation-circle"
                        : Type == AntAlertType.Error ? "close-circle" : null;

        /// <summary>
        /// Indicator if the component is closed or not.
        /// </summary>
        protected bool isClosed = false;
        /// <summary>
        /// Just before we close the component we set this indicator to show a closing animation.
        /// </summary>
        protected bool isClosing = false;
        /// <summary>
        /// Sets the default classes.
        /// </summary>
        protected void SetClassMap()
        {
            string prefixName = "ant-alert";
            ClassMapper.Clear()
                .Add("ant-alert")
                .If($"{prefixName}-{Type}", () => !string.IsNullOrEmpty(Type))
                .If($"{prefixName}-no-icon", () => !ShowIcon)
                .If($"{prefixName}-closable", () => Closable)
                .If($"{prefixName}-banner", () => Banner)
                .If($"{prefixName}-with-description", () => !string.IsNullOrEmpty(Description))
                .If($"{prefixName}-slide-up-leave", () => isClosing)
                ;
        }

        /// <summary>
        /// Triggered each time a parameter is changed.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClassMap();
        }

        /// <summary>
        /// Start-up code.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            CheckBannerMode();
            SetClassMap();
        }
        private void CheckBannerMode()
        {
            if (Banner && string.IsNullOrEmpty(Type))
            {
                ShowIcon = false;
            }
        }

        /// <summary>
        /// Handles the close callback.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected async Task OnCloseHandler(MouseEventArgs args)
        {
            isClosing = true;
            if (OnClose.HasDelegate)
            {
                await OnClose.InvokeAsync(args);
            }
            await Task.Delay(300);
            isClosed = true;
            await AfterCloseHandler(args);
        }
        /// <summary>
        /// Handles the after close callback.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected async Task AfterCloseHandler(MouseEventArgs args)
        {
            if (AfterClose.HasDelegate)
            {
                await AfterClose.InvokeAsync(args);
            }
        }
    }

}