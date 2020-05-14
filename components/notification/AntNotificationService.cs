using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace AntBlazor
{
    /// <summary>
    /// AntNotification Service
    /// </summary>
    public class AntNotificationService
    {

        /// <summary>
        /// Open a notification box
        /// </summary>
        /// <param name="config"></param>
        public async Task Open([NotNull]AntNotificationConfig config)
        {
            if (config == null)
            {
                return;
            }

            if (AntNotification.Instance != null)
            {
                await AntNotification.Instance.NotifyAsync(config);
            }
        }

        #region Api

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public async Task Success(AntNotificationConfig config)
        {
            if (config != null)
            {
                config.NotificationType = AntNotificationType.Success;
                await Open(config);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public async Task Error(AntNotificationConfig config)
        {
            if (config != null)
            {
                config.NotificationType = AntNotificationType.Error;
                await Open(config);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public async Task Info(AntNotificationConfig config)
        {
            if (config != null)
            {
                config.NotificationType = AntNotificationType.Info;
                await Open(config);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public async Task Warning(AntNotificationConfig config)
        {
            if (config != null)
            {
                config.NotificationType = AntNotificationType.Warning;
                await Open(config);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public async Task Warn(AntNotificationConfig config)
        {
            await Warning(config);
        }

        /// <summary>
        /// close notification by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task Close(string key)
        {
            if (AntNotification.Instance != null)
            {
                await AntNotification.Instance.CloseAsync(key);
            }
        }

        /// <summary>
        /// destroy
        /// </summary>
        public void Destroy()
        {
            if (AntNotification.Instance != null)
            {
                AntNotification.Instance.Destroy();
            }
        }

        #endregion

    }

}
