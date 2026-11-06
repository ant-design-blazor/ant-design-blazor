// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
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
    [Documentation(DocumentationCategory.Components, DocumentationType.Feedback, "https://gw.alipayobjects.com/zos/alicdn/hAkKTIW0K/Message.svg", Title = "Message", SubTitle = "全局提示")]
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

        private readonly ConcurrentDictionary<string, MessageConfig> _configDict
            = new ConcurrentDictionary<string, MessageConfig>();

        private static int _counter = 0;

        private Task NotifyAsync(MessageConfig config)
        {
            Debug.Assert(config != null);
            config = Extend(config);
            if (_maxCount > 0)
            {
                var count = _configDict.Count;
                if (count >= _maxCount)
                {
                    var removeConfig = _configDict.First().Value;
                    var firstKey = removeConfig.Key;

                    removeConfig.Cts.Cancel();
                    _configDict.TryRemove(firstKey, out _);
                }
            }

            if (_configDict.TryGetValue(config.Key, out var oldConfig))
            {
                oldConfig.Cts?.Cancel();
                oldConfig.Type = config.Type;
                oldConfig.Content = config.Content;
                oldConfig.Duration = config.Duration;
            }
            else
            {
                config.Order = Interlocked.Increment(ref _counter);
                _configDict.TryAdd(config.Key, config);
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

                if (config.WaitForAnimation)
                {
                    return Task.Delay(500)
                        .ContinueWith((result) =>
                        {
                            _configDict.TryRemove(config.Key, out _);
                            InvokeAsync(StateHasChanged);
                        }, TaskScheduler.Current);
                }
                else
                {
                    _ = Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        _configDict.TryRemove(config.Key, out _);
                        await InvokeAsync(StateHasChanged);
                    });
                    return Task.CompletedTask;
                }
            }

            return Task.CompletedTask;
        }

        private void Destroy()
        {
            _configDict.Clear();
            InvokeAsync(StateHasChanged);
        }

        private void OnMouseEnter(MessageItem item)
        {
            item.Config.Cts?.Cancel();
        }

        private void OnMouseLeave(MessageItem item)
        {
            if (item.Config.Duration > 0)
            {
                TimingRemove(item.Config);
            }
        }
    }
}
