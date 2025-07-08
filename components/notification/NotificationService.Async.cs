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
    /// AntNotification Service - Asynchronous Methods
    /// </summary>
    public partial class NotificationService
    {

        /// <summary>
        /// update a existed notification box asynchronously
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
        /// Open a notification box asynchronously
        /// </summary>
        /// <param name="config"></param>
        public async Task<NotificationRef> OpenAsync([NotNull] NotificationConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var notificationRef = CreateRef(config);
            await notificationRef.OpenAsync();
            return notificationRef;
        }

        /// <summary>
        /// open a notification box with NotificationType.Success style asynchronously
        /// </summary>
        /// <param name="config"></param>
        public async Task<NotificationRef> SuccessAsync(NotificationConfig config)
        {
            if (config == null)
            {
                return null;
            }

            config.NotificationType = NotificationType.Success;
            return await OpenAsync(config);
        }

        /// <summary>
        /// open a notification box with NotificationType.Error style asynchronously
        /// </summary>
        /// <param name="config"></param>
        public async Task<NotificationRef> ErrorAsync(NotificationConfig config)
        {
            if (config == null)
            {
                return null;
            }

            config.NotificationType = NotificationType.Error;
            return await OpenAsync(config);
        }

        /// <summary>
        /// open a notification box with NotificationType.Info style asynchronously
        /// </summary>
        /// <param name="config"></param>
        public async Task<NotificationRef> InfoAsync(NotificationConfig config)
        {
            if (config == null)
            {
                return null;
            }

            config.NotificationType = NotificationType.Info;
            return await OpenAsync(config);
        }

        /// <summary>
        /// open a notification box with NotificationType.Warning style asynchronously
        /// </summary>
        /// <param name="config"></param>
        public async Task<NotificationRef> WarningAsync(NotificationConfig config)
        {
            if (config == null)
            {
                return null;
            }

            config.NotificationType = NotificationType.Warning;
            return await OpenAsync(config);
        }

        /// <summary>
        /// Equivalent to Warning method asynchronously
        /// </summary>
        /// <param name="config"></param>
        public async Task<NotificationRef> WarnAsync(NotificationConfig config)
        {
            return await WarningAsync(config);
        }

        /// <summary>
        /// close notification by key asynchronously
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task CloseAsync(string key)
        {
            if (OnClosing != null)
            {
                await OnClosing.Invoke(key);
            }
        }

        internal async Task InternalOpenAsync(NotificationConfig config)
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
    }
} 
