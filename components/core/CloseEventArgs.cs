// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    /// <summary>
    /// 包含取消属性的事件参数，用于一些连锁事件时，前面的事件可以取消后面事件的触发，比如tag组件的关闭事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CloseEventArgs<T> where T : EventArgs
    {
        public CloseEventArgs(T eventArgs)
        {
            EventArgs = eventArgs;
        }

        public T EventArgs { get; set; }
        /// <summary>
        /// 设置为true时取消后续事件
        /// </summary>
        public bool Cancel { get; set; }
    }
}
