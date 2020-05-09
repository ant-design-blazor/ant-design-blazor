using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AntBlazor
{
    public class AntNotificationService
    {
        /// <summary>
        /// 全局默认配置修改
        /// </summary>
        /// <param name="defaultConfig"></param>
        public void Config([NotNull]AntNotificationGlobalConfig defaultConfig)
        {
            if (defaultConfig == null)
            {
                return;
            }

            AntNotification.Instance.SetGlobalConfig(defaultConfig);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public async Task Open([NotNull]AntNotificationConfig config)
        {
            if (config != null)
            {
                await AntNotification.Instance.NotifyAsync(config);
            }
        }

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
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void Close(string key)
        {
            AntNotification.Instance.Close(key);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Destroy()
        {
            AntNotification.Instance.Destroy();
        }
    }

}
