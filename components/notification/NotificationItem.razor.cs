// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        public bool PurePanel { get; set; }

        [Parameter]
        public Func<NotificationConfig, Task> OnClose { get; set; }

        [Parameter]
        public EventCallback<NotificationItem> OnMouseEnter { get; set; }

        [Parameter]
        public EventCallback<NotificationItem> OnMouseLeave { get; set; }

        private string GetIconClassName()
        {
            var className = $"{ClassPrefix}-notice-icon";
            if (Config.NotificationType != NotificationType.None)
            {
                className += $" {ClassPrefix}-notice-icon-{Config.NotificationType.ToString().ToLowerInvariant()}";
            }
            return className;
        }

        private string GetClassName()
        {
            if (Config.ClassName != null)
            {
                return Config.ClassName + (PurePanel ? null : Config.AnimationClass);
            }
            return PurePanel ? null : Config.AnimationClass;
        }

        private string GetStyle()
        {
            if (PurePanel) return GetComponentStyle("Notification", "--notification-index: 0;");
            // React's rc-notification positions notices absolutely and calculates each
            // notice's coordinates at runtime. Blazor doesn't run that positioning
            // algorithm, so keep notices in the list's flex flow to prevent overlap.
            const string listLayoutStyle = "position: relative; inset: auto; width: 100%; transform: none;";
            var customStyle = Config.Style?.Trim();
            if (string.IsNullOrEmpty(customStyle))
            {
                return GetComponentStyle("Notification", listLayoutStyle);
            }

            return GetComponentStyle("Notification", $"{listLayoutStyle} {customStyle.TrimEnd(';')};");
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

        private void HandleMouseEnter()
        {
            OnMouseEnter.InvokeAsync(this);
        }

        private void HandleMouseLeave()
        {
            OnMouseLeave.InvokeAsync(this);
        }
    }
}
