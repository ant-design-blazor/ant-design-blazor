// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Core.HashCodes
{
    /// <summary>
    /// Provide HashCode calculation of component parameters and other functions
    /// </summary>
    internal static class HashCodeExtensions
    {
        /// <summary>
        /// Compute the HashCode for all parameters
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="component">Component</param>
        /// <returns></returns>
        public static int GetParametersHashCode<TComponent>(this TComponent component) where TComponent : ComponentBase
        {
            var hashCode = 0;
            var descriptors = ParameterDescriptor<TComponent>.Descriptors;
            foreach (var descriptor in descriptors)
            {
                hashCode = HashCode.Combine(hashCode, descriptor.GetValueHashCode(component));
            }
            return hashCode;
        }
    }
}
