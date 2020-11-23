// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace AntDesign
{
    /// <summary>
    /// 
    /// </summary>
    public static class ResizableService
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly static Subject<bool> MouseEntered = new Subject<bool>();
        /// <summary>
        /// 
        /// </summary>
        public readonly static Subject<ResizeHandleEvent> MouseDown = new Subject<ResizeHandleEvent>();
    }
}
