// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// AntNotification全局配置
    /// </summary>
    public class NotificationGlobalConfig
    {
        /// <summary>
        /// 消息从底部弹出时，距离底部的位置，单位像素。
        /// </summary>
        public double? Bottom { get; set; } = null;

        /// <summary>
        /// 消息从顶部弹出时，距离顶部的位置，单位像素。	
        /// </summary>
        public double? Top { get; set; } = null;

        /// <summary>
        /// 是否开启 RTL 模式	
        /// </summary>
        public bool? Rtl { get; set; } = null;

        /// <summary>
        /// 自定义关闭图标
        /// </summary>
        public RenderFragment CloseIcon { get; set; } = null;

        /// <summary>
        /// 自动关闭的延时，单位为秒。默认 4.5 秒后自动关闭，配置为 null 则不自动关闭
        ///  </summary>
        public double? Duration { get; set; } = null;

        /// <summary>
        /// 弹出位置
        /// </summary>
        public NotificationPlacement? Placement { get; set; } = null;
    }
}
