// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AntDesign.Table.Internal
{
    /// <summary>
    /// This class is used to create a instance of interface by DispatchProxy
    /// </summary>
    internal class TItemProxy : DispatchProxy
    {
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return default;
        }
    }
}
