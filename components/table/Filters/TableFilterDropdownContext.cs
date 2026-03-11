// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign.Filters
{
    /// <summary>
    /// Provides context for filter dropdown components, containing methods to control dropdown visibility.
    /// This context is passed to custom filter dropdowns to allow them to interact with the Table component.
    /// </summary>
    /// <param name="FilterClose">Action to close the filter dropdown.</param>
    /// <param name="PreventClose">Action to prevent the filter dropdown from closing automatically.</param>
    public record TableFilterDropdownContext(Action FilterClose, Action PreventClose);
}
