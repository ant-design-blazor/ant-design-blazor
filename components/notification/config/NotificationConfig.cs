// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public class NotificationConfig
    {
        /// <summary>
        /// 控制出现与消失的动画
        /// </summary>
        internal string AnimationClass { get; set; } = AnimationType.Enter;

        /// <summary>
        /// 自定义关闭按钮
        /// </summary>
        public RenderFragment Btn { get; set; } = null;

        /// <summary>
        /// 自定义 CSS class
        /// </summary>
        public string ClassName { get; set; } = null;

        /// <summary>
        /// 自定义关闭图标
        /// </summary>
        public RenderFragment CloseIcon { get; set; } = null;

        /// <summary>
        /// 通知提醒标题，必选，string 或者 RenderFragment
        /// </summary>
        [NotNull]
        public OneOf<string, RenderFragment> Message { get; set; }

        /// <summary>
        /// 通知提醒内容，必选，string 或者 RenderFragment
        /// </summary>
        [NotNull]
        public OneOf<string, RenderFragment> Description { get; set; }

        /// <summary>
        /// 自动关闭的延时，单位为秒。默认 4.5 秒后自动关闭，配置为 0 则不自动关闭
        ///  </summary>
        public double? Duration { get; set; } = null;

        /// <summary>
        /// 自定义图标	
        /// </summary>
        public RenderFragment Icon { get; set; } = null;

        /// <summary>
        /// 当前通知唯一标志		
        /// </summary>
        public string Key { get; set; } = null;

        /// <summary>
        /// 当通知关闭时触发	
        /// </summary>
        public event Action OnClose;

        internal void InvokeOnClose()
        {
            OnClose?.Invoke();
        }


        /// <summary>
        /// 点击通知时触发的回调函数	
        /// </summary>
        public event Action OnClick;

        internal void InvokeOnClick()
        {
            OnClick?.Invoke();
        }


        /// <summary>
        /// 自定义内联样式	
        /// </summary>
        public string Style { get; set; } = null;

        /// <summary>
        /// 弹出位置
        /// </summary>
        public NotificationPlacement? Placement { get; set; } = null;

        /// <summary>
        /// 通知提醒框左侧的图标类型
        /// </summary>
        public NotificationType NotificationType { get; set; } = NotificationType.None;

        internal CancellationTokenSource Cts { get; set; }
    }
}
