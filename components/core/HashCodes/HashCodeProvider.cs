// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace AntDesign.Core.HashCodes
{
    /// <summary>
    /// 表示hashCode提供者抽象类
    /// 用于计算参数值的HashCode
    /// 集合类型则计算每个元素的HashCode
    /// 其它类型则直接返回对象的默认GetHashCode方法所提值
    /// 出于反射性能考虑，复杂模型不会进行拆解计算其各属性的HashCode，建议模型自己重写GetHashCode方法
    /// </summary>
    abstract class HashCodeProvider
    {
        /// <summary>
        /// 获取参数值的哈希值
        /// </summary>
        /// <param name="parameter">参数值</param>
        /// <returns></returns>
        public abstract int GetHashCode(object parameter);

        /// <summary>
        /// 为参数创建合适的哈希提供者
        /// </summary>
        /// <param name="parameterType">参数类型</param>
        /// <returns></returns>
        public static HashCodeProvider Create(Type parameterType)
        {
            if (typeof(IDictionary<string, object>).IsAssignableFrom(parameterType))
            {
                return DictionaryHashCodeProvider.Instance;
            }

            if (typeof(IEnumerable).IsAssignableFrom(parameterType))
            {
                return EnumerableHashCodeProvider.Instance;
            }

            return OtherHashCodeProvider.Instance;
        }

        /// <summary>
        /// IEnumerable类型的哈希提供者
        /// </summary>
        private class EnumerableHashCodeProvider : HashCodeProvider
        {
            public static HashCodeProvider Instance { get; } = new EnumerableHashCodeProvider();

            public override int GetHashCode(object parameter)
            {
                if (!(parameter is IEnumerable enumerable))
                {
                    return 0;
                }

                var hashCode = 0;
                var enumerator = enumerable.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    hashCode ^= OtherHashCodeProvider.Instance.GetHashCode(enumerator.Current);
                }
                return hashCode;
            }
        }


        /// <summary>
        /// 字典类型的哈希提供者
        /// </summary>
        private class DictionaryHashCodeProvider : HashCodeProvider
        {
            public static HashCodeProvider Instance { get; } = new DictionaryHashCodeProvider();

            public override int GetHashCode(object parameter)
            {
                if (!(parameter is IDictionary<string, object> dic))
                {
                    return 0;
                }

                var hashCode = 0;
                foreach (var item in dic)
                {
                    hashCode ^= OtherHashCodeProvider.Instance.GetHashCode(item.Key);
                    hashCode ^= OtherHashCodeProvider.Instance.GetHashCode(item.Value);
                }
                return hashCode;
            }
        }


        /// <summary>
        /// 其它类型的哈希提供者
        /// </summary>
        private class OtherHashCodeProvider : HashCodeProvider
        {
            public static HashCodeProvider Instance { get; } = new OtherHashCodeProvider();

            public override int GetHashCode(object parameter)
            {
                return parameter == null ? 0 : parameter.GetHashCode();
            }
        }
    }
}
