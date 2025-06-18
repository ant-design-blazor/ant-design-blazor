// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Core
{
#if NET7_0_OR_GREATER
#else
    [EventHandler("onmouseleave", typeof(EventArgs))]
    [EventHandler("onmouseenter", typeof(EventArgs))]

    public static class EventHandlers
    {
    }
#endif
}
