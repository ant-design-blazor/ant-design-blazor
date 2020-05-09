using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntNotification
    {
        protected override void OnInitialized()
        {
            Instance = this;
            base.OnInitialized();
        }

        internal static AntNotification Instance { get; private set; }

        #region GlobalConfig

        private double _top = 24;
        private double _bottom = 24;
        private bool _isRtl = false;
        private AntNotificationPlacement _defaultPlacement = AntNotificationPlacement.TopRight;
        private double _defaultDuration = 4.5;

        private RenderFragment _defaultCloseIcon = (builder) =>
        {
            builder.OpenComponent<AntIcon>(0);
            builder.AddAttribute(1, "Type", "close");
            builder.CloseComponent();
        };
        
        internal void SetGlobalConfig(
            [NotNull]AntNotificationGlobalConfig defaultConfig)
        {
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

            StateHasChanged();
        }

        internal AntNotificationConfig ExtendConfig(AntNotificationConfig config)
        {
            if (config.Placement == null)
            {
                config.Placement = _defaultPlacement;
            }
            if (config.Duration == null)
            {
                config.Duration = _defaultDuration;
            }
            if (config.CloseIcon == null)
            {
                config.CloseIcon = _defaultCloseIcon;
            }

            return config;
        }
        
        #endregion
        
        private readonly Dictionary<string, AntNotificationConfig> _configDict = new Dictionary<string, AntNotificationConfig>();
        private readonly List<AntNotificationConfig> _topRightNoticeItems = new List<AntNotificationConfig>();
        private readonly List<AntNotificationConfig> _topLeftNoticeItems = new List<AntNotificationConfig>();
        private readonly List<AntNotificationConfig> _bottomLeftNoticeItems = new List<AntNotificationConfig>();
        private readonly List<AntNotificationConfig> _bottomRightNoticeItems = new List<AntNotificationConfig>();

        internal async Task NotifyAsync(AntNotificationConfig config)
        {
            if (config != null)
            {
                config = ExtendConfig(config);
                string key = config.Key;
                if (!string.IsNullOrWhiteSpace(key))
                {
                    if (_configDict.ContainsKey(key))
                    {
                        AntNotificationConfig oldValue = _configDict[key];
                        oldValue.Message = config.Message;
                        oldValue.Description = config.Description;
                        StateHasChanged();
                        return;
                    }
                    _configDict.Add(config.Key, config);
                }

                switch (config.Placement)
                {
                    case AntNotificationPlacement.TopLeft:
                        _topLeftNoticeItems.Add(config);
                        break;
                    case AntNotificationPlacement.BottomLeft:
                        _bottomLeftNoticeItems.Add(config);
                        break;
                    case AntNotificationPlacement.BottomRight:
                        _bottomRightNoticeItems.Add(config);
                        break;
                    default:
                        _topRightNoticeItems.Add(config);
                        break;
                }
               
                StateHasChanged();
                await RemoveTimer(config);
            }
        }

        private async Task RemoveTimer(AntNotificationConfig option)
        {
            if (option.Duration > 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(option.Duration.Value));
                await RemoveChild(option);
            }
        }

        private Task RemoveChild(AntNotificationConfig option)
        {
            //avoid use do click and option.KeepTime toggle twice
            if (string.IsNullOrEmpty(option.ClassName))
            {
                option.AnimationClass = AnimationType.Leave;
                StateHasChanged();
                option.OnClose?.Invoke();
                RemoveFromList(option);
            }

            return Task.CompletedTask;
        }

        private void RemoveFromList(AntNotificationConfig config)
        {
            switch (config.Placement)
            {
                case AntNotificationPlacement.TopLeft:
                    _topLeftNoticeItems.Remove(config);
                    break;
                case AntNotificationPlacement.BottomLeft:
                    _bottomLeftNoticeItems.Remove(config);
                    break;
                case AntNotificationPlacement.BottomRight:
                    _bottomRightNoticeItems.Remove(config);
                    break;
                default:
                    _topRightNoticeItems.Remove(config);
                    break;
            }

            string key = config.Key;
            if (!string.IsNullOrWhiteSpace(key) && _configDict.ContainsKey(key))
            {
                _configDict.Remove(key);
            }
        }

        internal void Destroy()
        {
            _topRightNoticeItems.Clear();
            _topLeftNoticeItems.Clear();
            _bottomLeftNoticeItems.Clear();
            _bottomRightNoticeItems.Clear();
            _configDict.Clear();
            StateHasChanged();
        }

        internal void Close(string key)
        {
            if (_configDict.ContainsKey(key))
            {
                AntNotificationConfig config = _configDict[key];
                _configDict.Remove(key);
                RemoveFromList(config);
                StateHasChanged();
            }
        }
    }
}
