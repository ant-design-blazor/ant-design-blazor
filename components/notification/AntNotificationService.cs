using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace AntBlazor
{
    /// <summary>
    /// AntNotification Service
    /// </summary>
    public class AntNotificationService:IDisposable
    {
        public AntNotificationService(IJSRuntime jsRuntime,
            HtmlRenderService htmlRender)
        {
            _jsRuntime = jsRuntime;
            _htmlRenderer = htmlRender;
        }

        private bool _isInit = false;
        private bool _isDisposed = true;

        #region DI

        private readonly IJSRuntime _jsRuntime;

        private readonly HtmlRenderService _htmlRenderer;

        #endregion

        #region JSInvokable

        [JSInvokable]
        public Task NotificationClose(string key)
        {
            if (_configDict.TryGetValue(key, out AntNotificationConfig config))
            {
                _configDict.Remove(key);
                config.InvokeOnClose();
            }
            return Task.CompletedTask;
        }

        [JSInvokable]
        public Task NotificationClick(string key)
        {
            if (_configDict.TryGetValue(key, out AntNotificationConfig config))
            {
                config.InvokeOnClick();
            }
            return Task.CompletedTask;
        }

        #endregion

        #region InvokeJS
        private const string JsInstanceName = "notification";
        private const string NotificationContainerCls = "ant-notification";

        private async Task JsInitInstanceAsync()
        {
            _isDisposed = false;
            await _jsRuntime.InitCsInstance(DotNetObjectReference.Create(this), JsInstanceName);
        }

        private async Task JsDelInstanceAsync()
        {
            await _jsRuntime.DelCsInstance(JsInstanceName);
        }

        private async Task CreateContainerAsync(string htmlStr)
        {
            await _jsRuntime.AppendHtml(htmlStr);
        }

        private async Task AddNotificationAsync(string notificationHtmlStr,
            string container, string key, double? seconds)
        {
            await _jsRuntime.InvokeVoidAsync(JSInteropConstants.addNotification,
                notificationHtmlStr,
                container, key, seconds);
        }

        private async Task CloseAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync(JSInteropConstants.removeNotification, key);
        }

        private async Task DestroyAsync()
        {
            Dispose(true);
            _configDict = null;
            _containerDict.Clear();
            await _jsRuntime.RemoveElement("."+ NotificationContainerCls);
            await JsDelInstanceAsync();
            GC.SuppressFinalize(this);
        }
        
        #endregion

        #region Init Checker
        
        /// <summary>
        /// 初始化检查
        /// </summary>
        /// <returns></returns>
        private async Task CheckInit()
        {
            if (!_isInit)
            {
                await JsInitInstanceAsync();
            }
        }

        #endregion

        #region CommentRender

        /// <summary>
        /// 将组件渲染为html字符串
        /// </summary>
        /// <param name="renderFragment"></param>
        /// <returns></returns>
        private async ValueTask<string> RenderAsync(RenderFragment renderFragment)
        {
            return await _htmlRenderer.RenderAsync(renderFragment);
        }


        #endregion

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

        /// <summary>
        /// 全局默认配置修改
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

            if (string.IsNullOrWhiteSpace(config.Key))
            {
                config.Key = Guid.NewGuid().ToString();
            }

            return config;
        }

        #endregion

        #region 

        /// <summary>
        /// notification key to config dictionary
        /// </summary>
        private Dictionary<string, AntNotificationConfig> _configDict = new Dictionary<string, AntNotificationConfig>();

        /// <summary>
        /// notification container class name
        /// </summary>
        private Dictionary<AntNotificationPlacement, string> _containerDict = new Dictionary<AntNotificationPlacement, string>();

        #endregion

        #region Create Html String

        /// <summary>
        /// get notification container, if not existed it will create a new container
        /// </summary>
        /// <param name="placement"></param>
        /// <returns></returns>
        private async ValueTask<string> GetContainer(AntNotificationPlacement placement)
        {
            if (!_containerDict.ContainsKey(placement))
            {
                Dictionary<string, object> attributes = new Dictionary<string, object>();
                attributes.Add("IsRtl", _isRtl);
                attributes.Add("Top", _top);
                attributes.Add("Bottom", _bottom);
                attributes.Add("Placement", placement);

                RenderFragment renderFragment = (builder) =>
                {
                    builder.OpenComponent<AntNotification>(0);
                    builder.AddMultipleAttributes(1, attributes);
                    builder.CloseComponent();
                };
                string htmlStr = await RenderAsync(renderFragment);
                await CreateContainerAsync(htmlStr);

                string placementStr = placement.ToString();
                placementStr = placementStr[0] == 'T'
                    ? "t" + placementStr.Substring(1)
                    : "b" + placementStr.Substring(1);
                string className = ".ant-notification.ant-notification-" + placementStr;

                _containerDict.Add(placement, className);
            }
            return _containerDict[placement];
        }

        /// <summary>
        /// create notification box html from NotificationItem Component
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private async ValueTask<string> CreateNotificationItem(AntNotificationConfig config)
        {
            RenderFragment renderFragment = (builder) =>
            {
                builder.OpenComponent<AntNotificationItem>(0);
                builder.AddAttribute(1, "Config", config);
                builder.CloseComponent();
            };
            string htmlStr = await RenderAsync(renderFragment);
            return htmlStr;
        }

        #endregion

        /// <summary>
        /// Open a notification box
        /// </summary>
        /// <param name="config"></param>
        public async Task Open([NotNull]AntNotificationConfig config)
        {
            if (config == null)
            {
                return;
            }

            await CheckInit();

            if (_isDisposed)
            {
                throw new ObjectDisposedException("AntNotificationService had disposed");
            }
            
            config = ExtendConfig(config);
            Debug.Assert(config.Placement != null);
            string container = await GetContainer(config.Placement.Value);
            string notificationHtmlStr = await CreateNotificationItem(config);

            await AddNotificationAsync(notificationHtmlStr,
                container, config.Key, config.Duration);
            _configDict.Add(config.Key, config);
        }

        #region Api

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public async Task Success(AntNotificationConfig config)
        {
            if (config != null)
            {
                config.NotificationType = AntNotificationType.Success;
                await Open(config);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public async Task Error(AntNotificationConfig config)
        {
            if (config != null)
            {
                config.NotificationType = AntNotificationType.Error;
                await Open(config);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public async Task Info(AntNotificationConfig config)
        {
            if (config != null)
            {
                config.NotificationType = AntNotificationType.Info;
                await Open(config);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public async Task Warning(AntNotificationConfig config)
        {
            if (config != null)
            {
                config.NotificationType = AntNotificationType.Warning;
                await Open(config);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public async Task Warn(AntNotificationConfig config)
        {
            await Warning(config);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task Close(string key)
        {
            await CloseAsync(key);
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task Destroy()
        {
            await DestroyAsync();
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
        }

        public void Dispose()
        {
            Destroy();
        }
    }

}
