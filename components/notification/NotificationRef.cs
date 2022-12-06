// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// NotificationRef
    /// </summary>
    public class NotificationRef : FeedbackRefBase
    {
        /// <summary>
        /// the notification box's config
        /// </summary>
        public NotificationConfig Config { get; private set; }

        private readonly NotificationService _service;

        internal NotificationRef(NotificationService service,NotificationConfig config)
        {
            _service = service;
            Config = config;
        }

        /// <summary>
        /// open the notification box
        /// </summary>
        /// <returns></returns>
        public override async Task OpenAsync()
        {
            await _service.InternalOpen(Config);
            if (OnOpen != null) await OnOpen.Invoke();
        }

        /// <summary>
        /// After modifying the Config property, update the notification box
        /// </summary>
        /// <returns></returns>
        public override async Task UpdateConfigAsync()
        {
            await _service.UpdateAsync(Config.Key, Config.Description, Config.Message);
        }

        /// <summary>
        /// update the notification box's description
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task UpdateConfigAsync(OneOf<string, RenderFragment> description)
        {
            Config.Description = description;
            await UpdateConfigAsync();
        }

        /// <summary>
        /// update the notification box's description and message
        /// </summary>
        /// <param name="description"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task UpdateConfigAsync(OneOf<string, RenderFragment> description, OneOf<string, RenderFragment> message)
        {
            Config.Description = description;
            Config.Message = message;
            await UpdateConfigAsync();
        }

        /// <summary>
        /// close the notification box
        /// </summary>
        /// <returns></returns>
        public override async Task CloseAsync()
        {
            await _service.Close(Config.Key);
            if (OnClose != null) await OnClose.Invoke();
        }
    }
}
