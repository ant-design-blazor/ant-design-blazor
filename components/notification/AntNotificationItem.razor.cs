using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AntBlazor.core.JsInterop.EventArg;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public partial class AntNotificationItem
    {
        [Parameter]
        public AntNotificationConfig Config { get; set; }

        [Parameter]
        public EventCallback<AntNotificationConfig> OnClose { get; set; }

        private string GetIconClassName()
        {
            if (Config.NotificationType != AntNotificationType.None
                || Config.Icon != null
                )
            {
                return "ant-notification-notice-with-icon";
            }

            return "";
        }

        private string GetClassName()
        {
            if (Config.ClassName != null)
            {
                return Config.ClassName + Config.AnimationClass;
            }
            return Config.AnimationClass;
        }

        private async Task Close(MouseEventArgs e)
        {
            if (OnClose.HasDelegate)
            {
                await OnClose.InvokeAsync(Config);
            }
        }

        private void OnClick()
        {
            Config.OnClick?.Invoke();
        }
    }
}
