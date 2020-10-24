// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
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
            return Component<TComponent>.GetParametersHashCode(component);
        }

        /// <summary>
        /// 提供组件参数哈希值计算
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        static class Component<TComponent>
        {
            private static readonly ParameterDescriptor<TComponent>[] _descriptors
                = typeof(TComponent)
                    .GetProperties()
                    .Select(item => new ParameterDescriptor<TComponent>(item))
                    .Where(item => item.IsParameter)
                    .ToArray();

            /// <summary>
            /// 返回组件的参数的HashCode
            /// </summary>
            /// <param name="component"></param>
            /// <returns></returns>
            public static int GetParametersHashCode(TComponent component)
            {
                var hashCode = 0;
                foreach (var descriptor in _descriptors)
                {
                    hashCode ^= descriptor.GetValueHashCode(component);
                }
                return hashCode;
            }
        }
    }
}
