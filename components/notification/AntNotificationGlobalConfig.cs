using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    /// <summary>
    /// AntNotification全局配置
    /// </summary>
    public class AntNotificationGlobalConfig
    {
        /// <summary>
        /// 消息从底部弹出时，距离底部的位置，单位像素。
        /// </summary>
        public int? Bottom { get; set; } = null;

        /// <summary>
        /// 消息从顶部弹出时，距离顶部的位置，单位像素。	
        /// </summary>
        public int? Top { get; set; } = null;

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
        public AntNotificationPlacement? Placement { get; set; } = null;
    }
}
