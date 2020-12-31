// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign
{
    public interface IRendered
    {
        /// <summary>
        /// 渲染完成后
        /// </summary>
        Action OnRendered { get; set; }

        /// <summary>
        /// 新节点数据，用于展开并选择新节点
        /// </summary>
        object NewChildData { get; set; }
    }
}
