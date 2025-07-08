// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
    public partial class NotificationService : INotificationService
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
        /// just create a NotificationRef without open it synchronously
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public NotificationRef CreateRef([NotNull] NotificationConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            return new NotificationRef(this, config);
        }

        /// <summary>
        /// update a existed notification box synchronously
        /// </summary>
        /// <param name="key"></param>
        /// <param name="description"></param>
        /// <param name="message"></param>
        public void Update(string key, OneOf<string, RenderFragment> description, OneOf<string, RenderFragment>? message = null)
        {
            if (OnUpdating != null && !string.IsNullOrWhiteSpace(key))
            {
                _ = OnUpdating.Invoke(key, description, message);
            }
        }

        /// <summary>
        /// Open a notification box synchronously
        /// </summary>
        /// <param name="config"></param>
        public NotificationRef Open([NotNull] NotificationConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var notificationRef = CreateRef(config);
            _ = notificationRef.OpenAsync();
            return notificationRef;
        }

        /// <summary>
        /// open a notification box with NotificationType.Success style synchronously
        /// </summary>
        /// <param name="config"></param>
        public NotificationRef Success(NotificationConfig config)
        {
            if (config == null)
            {
                return null;
            }

            config.NotificationType = NotificationType.Success;
            return Open(config);
        }

        /// <summary>
        /// open a notification box with NotificationType.Error style synchronously
        /// </summary>
        /// <param name="config"></param>
        public NotificationRef Error(NotificationConfig config)
        {
            if (config == null)
            {
                return null;
            }

            config.NotificationType = NotificationType.Error;
            return Open(config);
        }

        /// <summary>
        /// open a notification box with NotificationType.Info style synchronously
        /// </summary>
        /// <param name="config"></param>
        public NotificationRef Info(NotificationConfig config)
        {
            if (config == null)
            {
                return null;
            }

            config.NotificationType = NotificationType.Info;
            return Open(config);
        }

        /// <summary>
        /// open a notification box with NotificationType.Warning style synchronously
        /// </summary>
        /// <param name="config"></param>
        public NotificationRef Warning(NotificationConfig config)
        {
            if (config == null)
            {
                return null;
            }

            config.NotificationType = NotificationType.Warning;
            return Open(config);
        }

        /// <summary>
        /// Equivalent to Warning method synchronously
        /// </summary>
        /// <param name="config"></param>
        public NotificationRef Warn(NotificationConfig config)
        {
            return Warning(config);
        }

        /// <summary>
        /// close notification by key synchronously
        /// </summary>
        /// <param name="key"></param>
        public void Close(string key)
        {
            if (OnClosing != null)
            {
                _ = OnClosing.Invoke(key);
            }
        }

        /// <summary>
        /// destroy all Notification box
        /// </summary>
        public void Destroy()
        {
            OnDestroying?.Invoke();
        }
    }
}
