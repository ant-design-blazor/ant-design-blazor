// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign
{
    [Flags]
    internal enum ComponentStatus : byte
    {
        Default = 0,

        /// <summary>
        /// 组件初始化中
        /// </summary>
        Initing = 1,

        /// <summary>
        /// 组件初始化完毕
        /// </summary>
        Inited = 2,

        /// <summary>
        /// 用于在 parameter 属性中设置
        /// </summary>
        Opening = 4,

        /// <summary>
        /// 一旦打开时经历过一次 OnAfterRenderAsync，就变成此状态
        /// </summary>
        Opened = 8,

        /// <summary>
        /// 用于在 parameter 属性中设置
        /// </summary>
        Closing = 16,

        /// <summary>
        /// 一旦关闭时经历过一次 OnAfterRenderAsync，就变成此状态
        /// </summary>
        Closed = 32,

        /// <summary>
        /// 组件销毁中
        /// </summary>
        Destroying = 64,

        /// <summary>
        /// 组件销毁完毕
        /// </summary>
        Destroyed = 128
    }

    internal static class ComponentStatusExt
    {
        /// <summary>
        /// return <paramref name="componentStatus"/> is <paramref name="status"/>
        /// </summary>
        /// <param name="componentStatus"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool Is(this ComponentStatus componentStatus, ComponentStatus status)
        {
            return componentStatus == status;
        }

        /// <summary>
        /// Is ComponentStatus.Opening or ComponentStatus.Opened
        /// </summary>
        /// <param name="componentStatus"></param>
        /// <returns></returns>
        public static bool IsOpen(this ComponentStatus componentStatus)
        {
            return componentStatus == ComponentStatus.Opening || componentStatus == ComponentStatus.Opened;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentStatus"></param>
        /// <returns></returns>
        public static bool IsNotOpen(this ComponentStatus componentStatus)
        {
            return !IsOpen(componentStatus);
        }

        /// <summary>
        /// Is ComponentStatus.Closing or ComponentStatus.Closed
        /// </summary>
        /// <param name="componentStatus"></param>
        /// <returns></returns>
        public static bool IsClose(this ComponentStatus componentStatus)
        {
            return componentStatus == ComponentStatus.Closing || componentStatus == ComponentStatus.Closed;
        }

    }
}
