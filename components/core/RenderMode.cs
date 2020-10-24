// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    /// <summary>
    /// 渲染模式
    /// </summary>
    public enum RenderMode
    {
        /// <summary>
        /// 总是渲染
        /// </summary>
        Always,

        /// <summary>
        /// 当参数值的hashCode变化才渲染
        /// </summary>
        ParametersHashCodeChanged,
    }
}
