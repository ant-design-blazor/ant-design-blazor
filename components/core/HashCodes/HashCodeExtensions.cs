// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign.Core.HashCodes
{
    /// <summary>
    /// 提供组件参数的HashCode计算等功能
    /// </summary>
    static class HashCodeExtensions
    {
        /// <summary>
        /// 计算所有参数的HashCode
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="component">组件</param>
        /// <returns></returns>
        public static int GetParametersHashCode<TComponent>(this TComponent component) where TComponent : ComponentBase
        {
            var hashCode = 0;
            var descriptors = ParameterDescriptor<TComponent>.Descriptors;
            foreach (var descriptor in descriptors)
            {
                hashCode ^= descriptor.GetValueHashCode(component);
            }
            return hashCode;
        }
    }
}
