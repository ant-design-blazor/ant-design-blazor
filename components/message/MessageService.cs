using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// Message Service
    /// </summary>
    public class MessageService
    {
        internal event Action<MessageGlobalConfig> OnConfig;
        internal event Func<MessageConfig, Task> OnOpening;
        internal event Action OnDestroy;

        #region API

        public Task Open([NotNull]MessageConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var task = OnOpening?.Invoke(config);
            return task;
        }

        public Task Success(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            return PreOpen(MessageType.Success, content, duration, onClose);
        }

        public Task Error(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            return PreOpen(MessageType.Error, content, duration, onClose);
        }

        public Task Info(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            var task = PreOpen(MessageType.Info, content, duration, onClose);
            return task;
        }

        public Task Warning(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            return PreOpen(MessageType.Warning, content, duration, onClose); ;
        }

        public Task Warn(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            return Warning(content, duration, onClose);
        }

        public Task Loading(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
        {
            return PreOpen(MessageType.Loading, content, duration, onClose);
        }

        private Task PreOpen(MessageType type, OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null)
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
            return Open(config);
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
