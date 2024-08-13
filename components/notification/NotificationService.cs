using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// AntNotification Service
    /// </summary>
    public class NotificationService : INotificationService
    {
        internal event Action<NotificationGlobalConfig> OnConfiging;
        internal event Func<NotificationConfig, Task> OnNoticing;
        internal event Func<string, OneOf<string, RenderFragment>, OneOf<string, RenderFragment>?, Task> OnUpdating;
        internal event Func<string, Task> OnClosing;
        internal event Action OnDestroying;

        /// <summary>
        /// Provide default configuration for all notifications
        /// </summary>
        /// <param name="config"></param>
        public void Config(NotificationGlobalConfig config)
        {
            OnConfiging?.Invoke(config);
        }

        /// <summary>
        /// just create a NotificationRef without open it
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public Task<NotificationRef> CreateRefAsync([NotNull] NotificationConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            var notificationRef = new NotificationRef(this, config);
            return Task.FromResult(notificationRef);
        }

        internal async Task InternalOpen(NotificationConfig config)
        {
            if (OnNoticing != null)
            {
                if (string.IsNullOrWhiteSpace(config.Key))
                {
                    config.Key = Guid.NewGuid().ToString();
                }
                await OnNoticing.Invoke(config);
            }
        }

        /// <summary>
        /// update a existed notification box
        /// </summary>
        /// <param name="key"></param>
        /// <param name="description"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task UpdateAsync(string key, OneOf<string, RenderFragment> description, OneOf<string, RenderFragment>? message = null)
        {
            if (OnUpdating != null && !string.IsNullOrWhiteSpace(key))
            {
                await OnUpdating.Invoke(key, description, message);
            }
        }

        /// <summary>
        /// Open a notification box
        /// </summary>
        /// <param name="config"></param>
        public async Task<NotificationRef> Open([NotNull] NotificationConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var notificationRef = await CreateRefAsync(config);
            await notificationRef.OpenAsync();
            return notificationRef;
        }

        #region Api

        /// <summary>
        /// open a notification box with NotificationType.Success style
        /// </summary>
        /// <param name="config"></param>
        public async Task<NotificationRef> Success(NotificationConfig config)
        {
            if (config == null)
            {
                return null;
            }

            config.NotificationType = NotificationType.Success;
            return await Open(config);

        }

        /// <summary>
        /// open a notification box with NotificationType.Error style
        /// </summary>
        /// <param name="config"></param>
        public async Task<NotificationRef> Error(NotificationConfig config)
        {
            if (config == null)
            {
                return null;
            }

            config.NotificationType = NotificationType.Error;
            return await Open(config);

        }

        /// <summary>
        /// open a notification box with NotificationType.Info style
        /// </summary>
        /// <param name="config"></param>
        public async Task<NotificationRef> Info(NotificationConfig config)
        {
            if (config == null)
            {
                return null;
            }

            config.NotificationType = NotificationType.Info;
            return await Open(config);

        }

        /// <summary>
        /// open a notification box with NotificationType.Warning style
        /// </summary>
        /// <param name="config"></param>
        public async Task<NotificationRef> Warning(NotificationConfig config)
        {
            if (config == null)
            {
                return null;
            }

            config.NotificationType = NotificationType.Warning;
            await Open(config);

            return null;
        }

        /// <summary>
        /// Equivalent to Warning method
        /// </summary>
        /// <param name="config"></param>
        public async Task<NotificationRef> Warn(NotificationConfig config)
        {
            return await Warning(config);
        }

        /// <summary>
        /// close notification by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task Close(string key)
        {
            if (OnClosing != null)
            {
                await OnClosing.Invoke(key);
            }
        }

        /// <summary>
        /// destroy all Notification box
        /// </summary>
        public void Destroy()
        {
            OnDestroying?.Invoke();
        }

        #endregion
    }

}
