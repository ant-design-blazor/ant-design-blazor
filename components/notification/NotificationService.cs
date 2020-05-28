using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AntDesign
{
    /// <summary>
    /// AntNotification Service
    /// </summary>
    public class NotificationService
    {
        internal event Action<NotificationGlobalConfig> OnConfiging;
        internal event Func<NotificationConfig, Task> OnNoticing;
        internal event Func<string, Task> OnClosing;
        internal event Action OnDestroying;

        public void Config(NotificationGlobalConfig config)
        {
            OnConfiging?.Invoke(config);
        }


        /// <summary>
        /// Open a notification box
        /// </summary>
        /// <param name="config"></param>
        public async Task Open([NotNull]NotificationConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            var task = OnNoticing?.Invoke(config);
            if (task != null)
            {
                await task;
            }
        }

        #region Api

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public async Task Success(NotificationConfig config)
        {
            if (config != null)
            {
                config.NotificationType = NotificationType.Success;
                await Open(config);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public async Task Error(NotificationConfig config)
        {
            if (config != null)
            {
                config.NotificationType = NotificationType.Error;
                await Open(config);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public async Task Info(NotificationConfig config)
        {
            if (config != null)
            {
                config.NotificationType = NotificationType.Info;
                await Open(config);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public async Task Warning(NotificationConfig config)
        {
            if (config != null)
            {
                config.NotificationType = NotificationType.Warning;
                await Open(config);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public async Task Warn(NotificationConfig config)
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
            var task = OnClosing?.Invoke(key);
            if (task != null) await task;
        }

        /// <summary>
        /// destroy
        /// </summary>
        public void Destroy()
        {
            OnDestroying?.Invoke();
        }

        #endregion

    }

}
