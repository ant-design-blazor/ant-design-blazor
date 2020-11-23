// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /// <summary>
    /// 
    /// </summary>
    public class ResizeHandleEvent
    {
        /// <summary>
        /// 方位
        /// </summary>
        public string Direction { get; set; }
        /// <summary>
        /// 事件参数
        /// </summary>
        public MouseEventArgs MouseEvent { get; set; }
    }
}
