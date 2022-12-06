// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign
{
    /// <summary>
    /// Can be used to conditionally block closing events
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
        /// If true, the component will be prevented from closing
        /// </summary>
        public bool Cancel { get; set; }
    }
}
