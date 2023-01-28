// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /**
    <summary>
    <para>Display global messages as feedback in response to user operations.</para>

    <h2>When To Use</h2>
    <list type="bullet">
        <item>To provide feedback such as success, warning, error etc.</item>
        <item>A message is displayed at top and center and will be dismissed automatically, as a non-interrupting light-weighted prompt.</item>
    </list>
    </summary>
    <seealso cref="IMessageService" />
    <seealso cref="MessageConfig" />
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Feedback, "https://gw.alipayobjects.com/zos/alicdn/hAkKTIW0K/Message.svg")]
    public partial class Message : AntDomComponentBase
    {
        [Inject]
        private MessageService MessageService { get; set; }

        protected override void OnInitialized()
        {
            if (MessageService != null)
            {
                MessageService.OnOpening += NotifyAsync;
                MessageService.OnDestroy += Destroy;
                MessageService.OnConfig += Config;
            }

            ClassMapper
                .Add(PrefixCls)
                .If($"{PrefixCls}-rtl", () => RTL);
        }

        protected override void Dispose(bool disposing)
        {
            MessageService.OnOpening -= NotifyAsync;
            MessageService.OnDestroy -= Destroy;
            MessageService.OnConfig -= Config;
            base.Dispose(disposing);
        }

        #region global config

        protected const string PrefixCls = "ant-message";

        private double _duration = 3;

        private int _maxCount = 0;

        private double _top = 24;

        private void Config(MessageGlobalConfig globalConfig)
        {
            _duration = globalConfig.Duration;
            _maxCount = globalConfig.MaxCount;
            _top = globalConfig.Top;
            //_rtl = globalConfig.Rtl;
        }

        private MessageConfig Extend(MessageConfig config)
        {
            config.Duration ??= _duration;
            if (string.IsNullOrWhiteSpace(config.Key))
            {
                config.Key = Guid.NewGuid().ToString();
            }
            return config;
        }

        #endregion global config

        private readonly List<MessageConfig> _configs
            = new List<MessageConfig>();

        private readonly Dictionary<string, MessageConfig> _configDict
            = new Dictionary<string, MessageConfig>();

        private Task NotifyAsync(MessageConfig config)
        {
            Debug.Assert(config != null);
            config = Extend(config);
            if (_maxCount > 0)
            {
                var count = _configDict.Count;
                if (count >= _maxCount)
                {
                    var removeConfig = _configs[0];
                    var firstKey = removeConfig.Key;

                    removeConfig.Cts.Cancel();
                    _configDict.Remove(firstKey);
                    _configs.Remove(removeConfig);
                }
            }

            if (_configDict.ContainsKey(config.Key))
            {
                var oldConfig = _configDict[config.Key];

                oldConfig.Cts?.Cancel();
                oldConfig.Type = config.Type;
                oldConfig.Content = config.Content;
                oldConfig.Duration = config.Duration;
            }
            else
            {
                _configDict.Add(config.Key, config);
                _configs.Add(config);
            }

            InvokeAsync(StateHasChanged);

            return TimingRemove(config);
        }

        private Task TimingRemove(MessageConfig config)
        {
            if (config.Duration > 0)
            {
                var cts = new CancellationTokenSource();
                config.Cts = cts;

                var task = Task.Delay(TimeSpan.FromSeconds(config.Duration.Value), cts.Token);

                return task.ContinueWith((result) =>
                    {
                        if (!cts.IsCancellationRequested)
                        {
                            RemoveItem(config);
                        }
                    }, TaskScheduler.Current);
            }
            else
            {
                return new Task(() =>
                {
                    RemoveItem(config);
                });
            }
        }

        private Task RemoveItem(MessageConfig config)
        {
            //avoid user do click and option.Duration toggle twice
            if (config.AnimationClass == MessageAnimationType.Enter)
            {
                config.AnimationClass = MessageAnimationType.Leave;
                InvokeAsync(StateHasChanged);
                config.InvokeOnClose();

                Task.Delay(500)
                    .ContinueWith((result) =>
                    {
                        _configDict.Remove(config.Key);
                        _configs.Remove(config);
                        InvokeAsync(StateHasChanged);
                    }, TaskScheduler.Current);
            }

            return Task.CompletedTask;
        }

        private void Destroy()
        {
            _configDict.Clear();
            _configs.Clear();
            InvokeAsync(StateHasChanged);
        }
    }
}
