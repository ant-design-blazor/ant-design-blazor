using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Notification
    {
        [Inject] private NotificationService NotificationService { get; set; }

        #region override

        protected override void OnInitialized()
        {
            if (NotificationService != null)
            {
                NotificationService.OnNoticing += NotifyAsync;
                NotificationService.OnClosing += CloseAsync;
                NotificationService.OnDestroying += Destroying;
                NotificationService.OnConfiging += Config;
            }
        }

        protected override void Dispose(bool disposing)
        {
            NotificationService.OnNoticing -= NotifyAsync;
            NotificationService.OnClosing -= CloseAsync;
            NotificationService.OnDestroying -= Destroying;
            NotificationService.OnConfiging -= Config;

            base.Dispose(disposing);
        }

        #endregion

        private readonly ConcurrentDictionary<NotificationPlacement,
                List<NotificationConfig>>
            _configDict = new ConcurrentDictionary<NotificationPlacement, List<NotificationConfig>>();

        private readonly ConcurrentDictionary<string, NotificationConfig>
            _configKeyDict = new ConcurrentDictionary<string, NotificationConfig>();

        private string GetClassName(NotificationPlacement placement)
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

        private string GetStyle(NotificationPlacement placement)
        {
            switch (placement)
            {
                case NotificationPlacement.TopRight:
                    return $"right: 0px; top:{_top}px; bottom: auto;";
                case NotificationPlacement.TopLeft:
                    return $"left: 0px; top:{_top}px; bottom: auto;";
                case NotificationPlacement.BottomLeft:
                    return $"left: 0px; top: auto; bottom: {_bottom}px;";
                default:
                    return $"right: 0px; top: auto; bottom: {_bottom}px;";
            }
        }

        #region GlobalConfig

        private double _top = 24;
        private double _bottom = 24;
        private bool _isRtl;
        private NotificationPlacement _defaultPlacement = NotificationPlacement.TopRight;
        private double _defaultDuration = 4.5;

        private RenderFragment _defaultCloseIcon = (builder) =>
        {
            builder.OpenComponent<Icon>(0);
            builder.AddAttribute(1, "Type", "close");
            builder.CloseComponent();
        };

        /// <summary>
        /// modify global config
        /// </summary>
        /// <param name="defaultConfig"></param>
        private void Config(
            [NotNull] NotificationGlobalConfig defaultConfig)
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

        private NotificationConfig ExtendConfig(NotificationConfig config)
        {
            config.Placement ??= _defaultPlacement;
            config.Duration ??= _defaultDuration;
            config.CloseIcon ??= _defaultCloseIcon;

            return config;
        }

        #endregion GlobalConfig

        private async Task NotifyAsync(NotificationConfig option)
        {
            if (option == null)
            {
                return;
            }

            option = ExtendConfig(option);

            Debug.Assert(option.Placement != null, "option.Placement != null");
            var placement = option.Placement.Value;
            bool canAdd = true;
            if (!_configDict.ContainsKey(placement))
            {
                canAdd = _configDict.TryAdd(placement, new List<NotificationConfig>());
            }

            if (canAdd)
            {
                if (!string.IsNullOrWhiteSpace(option.Key))
                {
                    if (_configKeyDict.TryGetValue(option.Key, out var oldConfig))
                    {
                        oldConfig.Message = option.Message;
                        oldConfig.Description = option.Description;
                        await InvokeAsync(StateHasChanged);
                        return;
                    }

                    canAdd = _configKeyDict.TryAdd(option.Key, option);
                }

                if (canAdd)
                {
                    _configDict[placement].Add(option);
                    await InvokeAsync(StateHasChanged);
                    await Remove(option);
                }
            }
        }

        private async Task Remove(NotificationConfig option)
        {
            if (option.Duration > 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(option.Duration.Value));
                await RemoveItem(option);
            }
        }

        private async Task RemoveItem(NotificationConfig option)
        {
            //avoid user do click and option.Duration toggle twice
            if (option.AnimationClass == AnimationType.Enter)
            {
                option.AnimationClass = AnimationType.Leave;
                StateHasChanged();

                option.InvokeOnClose();

                await Task.Delay(500);
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

            //return Task.CompletedTask;
        }

        private void Destroying()
        {
            _configDict.Clear();
            _configKeyDict.Clear();
        }

        private async Task CloseAsync(string key)
        {
            if (_configKeyDict.TryGetValue(key, out var config))
            {
                await RemoveItem(config);
            }
        }
    }
}
