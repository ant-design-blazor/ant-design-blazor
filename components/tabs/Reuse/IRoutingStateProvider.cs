// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

#if !NET8_0_OR_GREATER
namespace Microsoft.AspNetCore.Components.Routing;

public interface IRoutingStateProvider
{
    /// <summary>
    /// Gets RouteData
    /// </summary>
    public RouteData? RouteData { get; }
}
#endif
