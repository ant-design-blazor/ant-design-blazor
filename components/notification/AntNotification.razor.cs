using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntNotification
    {
        public AntNotification()
        {
            Instance = this;
        }

        internal static AntNotification Instance { get; private set; }

        private readonly ConcurrentDictionary<AntNotificationPlacement,
                List<AntNotificationConfig>>
            _configDict = new ConcurrentDictionary<AntNotificationPlacement, List<AntNotificationConfig>>();

        private readonly ConcurrentDictionary<string,
                AntNotificationConfig>
            _configKeyDict = new ConcurrentDictionary<string, AntNotificationConfig>();

        private string GetClassName(AntNotificationPlacement placement)
        {
            string placementStr = placement.ToString();
            placementStr = placementStr[0] == 'T'
                ? "t" + placementStr.Substring(1)
                : "b" + placementStr.Substring(1);
            string className = $"{ClassPrefix} {ClassPrefix}-" + placementStr;

            if (_isRtl)
            {
                className += $" {ClassPrefix}-rtl";
            }

            return className;
        }

        private string GetStyle(AntNotificationPlacement placement)
        {
            switch (placement)
            {
                case AntNotificationPlacement.TopRight:
                    return $"right: 0px; top:{_top}px; bottom: auto;";
                case AntNotificationPlacement.TopLeft:
                    return $"left: 0px; top:{_top}px; bottom: auto;";
                case AntNotificationPlacement.BottomLeft:
                    return $"left: 0px; top: auto; bottom: {_bottom}px;";
                default:
                    return $"right: 0px; top: auto; bottom: {_bottom}px;";
            }
        }

        #region GlobalConfig

        private double _top = 24;
        private double _bottom = 24;
        private bool _isRtl;
        private AntNotificationPlacement _defaultPlacement = AntNotificationPlacement.TopRight;
        private double _defaultDuration = 4.5;

        private RenderFragment _defaultCloseIcon = (builder) =>
        {
            builder.OpenComponent<AntIcon>(0);
            builder.AddAttribute(1, "Type", "close");
            builder.CloseComponent();
        };

        /// <summary>
        /// modify global config
        /// </summary>
        /// <param name="defaultConfig"></param>
        public void Config(
            [NotNull]AntNotificationGlobalConfig defaultConfig)
        {
            if (defaultConfig == null)
            {
                return;
            }

            if (defaultConfig.Bottom != null)
            {
                _bottom = defaultConfig.Bottom.Value;
            }
            if (defaultConfig.Top != null)
            {
                _top = defaultConfig.Top.Value;
            }
            if (defaultConfig.Rtl != null)
            {
                _isRtl = defaultConfig.Rtl.Value;
            }
            if (defaultConfig.Placement != null)
            {
                _defaultPlacement = defaultConfig.Placement.Value;
            }
            if (defaultConfig.Duration != null)
            {
                _defaultDuration = defaultConfig.Duration.Value;
            }
            if (defaultConfig.CloseIcon != null)
            {
                _defaultCloseIcon = defaultConfig.CloseIcon;
            }
        }

        private AntNotificationConfig ExtendConfig(AntNotificationConfig config)
        {
            config.Placement ??= _defaultPlacement;
            config.Duration ??= _defaultDuration;
            config.CloseIcon ??= _defaultCloseIcon;

            return config;
        }

        #endregion


        internal async Task NotifyAsync(AntNotificationConfig option)
        {
            option = ExtendConfig(option);

            Debug.Assert(option.Placement != null, "option.Placement != null");
            var placement = option.Placement.Value;
            bool canAdd = true;
            if (!_configDict.ContainsKey(placement))
            {
                canAdd = _configDict.TryAdd(placement, new List<AntNotificationConfig>());
            }
            if (canAdd)
            {
                if (!string.IsNullOrWhiteSpace(option.Key))
                {
                    if (_configKeyDict.TryGetValue(option.Key, out var oldConfig))
                    {
                        oldConfig.Message = option.Message;
                        oldConfig.Description = option.Description;
                        StateHasChanged();
                        return;
                    }
                    canAdd = _configKeyDict.TryAdd(option.Key, option);
                }

                if (canAdd)
                {
                    _configDict[placement].Add(option);
                    StateHasChanged();
                    await Remove(option);
                }
            }
        }

        private async Task Remove(AntNotificationConfig option)
        {
            if (option.Duration > 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(option.Duration.Value));
                await RemoveItem(option);
            }
        }

        private Task RemoveItem(AntNotificationConfig option)
        {
            //avoid user do click and option.Duration toggle twice
            if (option.AnimationClass == AnimationType.Enter)
            {
                option.AnimationClass = AnimationType.Leave;
                StateHasChanged();
                option.InvokeOnClose();

                Debug.Assert(option.Placement != null, "option.Placement != null");
                if (_configDict.TryGetValue(option.Placement.Value, out var configList))
                {
                    configList.Remove(option);
                }

                if (!string.IsNullOrWhiteSpace(option.Key))
                {
                    _configKeyDict.TryRemove(option.Key, out var _);
                }

                //when next notification item fade out or add new notice item, item will toggle StateHasChanged
                //StateHasChanged();
            }

            return Task.CompletedTask;
        }

        internal void Destroy()
        {
            _configDict.Clear();
            _configKeyDict.Clear();
        }

        internal async Task CloseAsync(string key)
        {
            if (_configKeyDict.TryGetValue(key, out var config))
            {
                await RemoveItem(config);
            }
        }
    }
}
