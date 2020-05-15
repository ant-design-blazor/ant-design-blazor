using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AntBlazor
{
    /// <summary>
    /// AntNotification Service
    /// </summary>
    public class NotificationService
    {

        /// <summary>
        /// Open a notification box
        /// </summary>
        /// <param name="config"></param>
        public async Task Open([NotNull]NotificationConfig config)
        {
            if (config == null)
            {
                return;
            }

            if (Notification.Instance != null)
            {
                await Notification.Instance.NotifyAsync(config);
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
            if (Notification.Instance != null)
            {
                await Notification.Instance.CloseAsync(key);
            }
        }

        /// <summary>
        /// destroy
        /// </summary>
        public void Destroy()
        {
            if (Notification.Instance != null)
            {
                Notification.Instance.Destroy();
            }
        }

        #endregion

    }

}
