using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class NotificationItem
    {
        [Parameter]
        public NotificationConfig Config { get; set; }

        [Parameter]
        public Func<NotificationConfig, Task> OnClose { get; set; }

        private string GetIconClassName()
        {
            if (Config.NotificationType != NotificationType.None
                || Config.Icon != null
            )
            {
                return $"{ClassPrefix}-notice-with-icon";
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
            var task = OnClose?.Invoke(Config);
            if (task != null)
            {
                await task;
            }
        }

        private void OnClick()
        {
            Config.InvokeOnClick();
        }
    }
}
