// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    /// <summary>
    /// Rerender strategy
    /// </summary>
    public enum RerenderStrategy
    {
        /// <summary>
        /// Always to rerender
        /// </summary>
        Always,

        /// <summary>
        /// Rerender only when any of the component's parameter values are changed
        /// </summary>
        ParametersHashCodeChanged,
    }
}
