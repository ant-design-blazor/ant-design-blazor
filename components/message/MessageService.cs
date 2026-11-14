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
    /// Message Service
    /// </summary>
    public class MessageService : IMessageService
    {
        internal event Action<MessageGlobalConfig> OnConfig;
        internal event Func<MessageConfig, Task> OnOpening;
        internal event Action OnDestroy;

        #region API

        public void Open([NotNull] MessageConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (OnOpening != null)
            {
                config.WaitForAnimation = false;
                _ = OnOpening.Invoke(config);
            }
        }

        public Task OpenAsync([NotNull] MessageConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (OnOpening != null)
            {
                config.WaitForAnimation = true;
                return OnOpening.Invoke(config);
            }

            return Task.CompletedTask;
        }

        public void Success(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            PreOpen(MessageType.Success, content, duration, onClose, false);
        }

        public Task SuccessAsync(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            return PreOpen(MessageType.Success, content, duration, onClose, true);
        }

        public void Error(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            PreOpen(MessageType.Error, content, duration, onClose, false);
        }

        public Task ErrorAsync(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            return PreOpen(MessageType.Error, content, duration, onClose, true);
        }

        public void Info(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            PreOpen(MessageType.Info, content, duration, onClose, false);
        }

        public Task InfoAsync(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            return PreOpen(MessageType.Info, content, duration, onClose, true);
        }

        public void Warning(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            PreOpen(MessageType.Warning, content, duration, onClose, false);
        }

        public Task WarningAsync(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            return PreOpen(MessageType.Warning, content, duration, onClose, true);
        }

        public void Warn(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            Warning(content, duration, onClose);
        }

        public Task WarnAsync(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            return WarningAsync(content, duration, onClose);
        }

        public void Loading(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            PreOpen(MessageType.Loading, content, duration, onClose, false);
        }

        public Task LoadingAsync(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            return PreOpen(MessageType.Loading, content, duration, onClose, true);
        }

        private Task PreOpen(MessageType type, OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null, bool waitForAnimation = true)
        {
            MessageConfig config;
            if (content.IsT2)
            {
                config = content.AsT2;
            }
            else
            {
                config = new MessageConfig() { };
                OneOf<string, RenderFragment> configContent;
                if (content.IsT1)
                {
                    configContent = content.AsT1;
                }
                else
                {
                    configContent = content.AsT0;
                }

                config.Content = configContent;
                config.Duration = duration;

                if (onClose != null)
                {
                    config.OnClose += onClose;
                }
            }

            config.Type = type;
            config.WaitForAnimation = waitForAnimation;
            return OpenAsync(config);
        }

        #endregion

        public void Config(MessageGlobalConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            OnConfig?.Invoke(config);
        }

        public void Destroy()
        {
            OnDestroy?.Invoke();
        }
    }
}
