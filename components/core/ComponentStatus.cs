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
        /// Component initialization in progress
        /// </summary>
        Initing = 1,

        /// <summary>
        /// 组件初始化完毕
        /// Component initialization completed
        /// </summary>
        Inited = 2,

        /// <summary>
        /// 用于在 parameter 属性中设置，表明组件正在第一次渲染中
        /// Used to set in the parameter property, Indicates that the component is rendering for the first time
        /// </summary>
        Opening = 4,

        /// <summary>
        /// 一旦打开时经历过一次 OnAfterRender/OnAfterRenderAsync，就变成此状态
        /// Once the component has experienced OnAfterRender/OnAfterRenderAsync once when it is opened, it becomes this state
        /// </summary>
        Opened = 8,

        /// <summary>
        /// 用于在 parameter 属性中设置，表明组件正在关闭中
        /// Used to set in the parameter property, indicating that the component is closing
        /// </summary>
        Closing = 16,

        /// <summary>
        /// 一旦关闭时经历过一次 OnAfterRender/OnAfterRenderAsync，就变成此状态
        /// Once OnAfterRender/OnAfterRenderAsync is experienced once when closing, it becomes this state
        /// </summary>
        Closed = 32,

        /// <summary>
        /// 用于在 parameter 属性中设置，表明组件正则销毁中、
        /// Used to set in the parameter attribute, indicating that the component is in regular destruction
        /// </summary>
        Destroying = 64,

        /// <summary>
        /// 一旦销毁时经历过一次 OnAfterRender，就变成此状态
        /// Once OnAfterRender/OnAfterRenderAsync is experienced once during destruction, it becomes this state
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
        /// Is not ComponentStatus.Opening and is not ComponentStatus.Opened
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
