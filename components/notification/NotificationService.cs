using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AntBlazor
{
    /// <summary>
    /// AntNotification Service
    /// </summary>
    public class NotificationService
    {
        internal event Action<NotificationConfig> OnNotice;
        internal event Action<string> OnClose;
        internal event Action OnDestroy;

        /// <summary>
        /// Open a notification box
        /// </summary>
        /// <param name="config"></param>
        public void Open([NotNull]NotificationConfig config)
        {
            if (config == null)
            {
                return;
            }
            OnNotice?.Invoke(config);
        }

        #region Api

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public void Success(NotificationConfig config)
        {
            if (config != null)
            {
                config.NotificationType = NotificationType.Success;
                Open(config);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public void Error(NotificationConfig config)
        {
            if (config != null)
            {
                config.NotificationType = NotificationType.Error;
                Open(config);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public void Info(NotificationConfig config)
        {
            if (config != null)
            {
                config.NotificationType = NotificationType.Info;
                Open(config);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public void Warning(NotificationConfig config)
        {
            if (config != null)
            {
                config.NotificationType = NotificationType.Warning;
                Open(config);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public void Warn(NotificationConfig config)
        {
            Warning(config);
        }

        /// <summary>
        /// close notification by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void Close(string key)
        {
            OnClose?.Invoke(key);
        }

        /// <summary>
        /// destroy
        /// </summary>
        public void Destroy()
        {
            OnDestroy?.Invoke();
        }

        #endregion

    }

}
