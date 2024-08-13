using System;
using System.Collections;
using System.Collections.Generic;

namespace AntDesign.Core.HashCodes
{
    /// <summary>
    /// HashCode provider
    /// <para>It is used to calculate the parameter value of HashCode</para>
    /// <para>The collection type computes the HashCode for each element,
    /// and the other types simply return the value raised by the default GetHashCode method of the object
    /// </para>
    /// <para>For the consideration of reflection performance, the complex model will not disassemble and calculate the HashCode of its attributes, so it is suggested that the model rewrite the GetHashCode method by itself</para>
    /// </summary>
    internal abstract class HashCodeProvider
    {
        /// <summary>
        /// Gets the hash value of the parameter value
        /// </summary>
        /// <param name="parameter">Parameter type</param>
        /// <returns></returns>
        public abstract int GetHashCode(object parameter);

        /// <summary>
        /// Create the appropriate hash provider for the parameter
        /// </summary>
        /// <param name="parameterType">Parameter type</param>
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
        /// The hash provider for the IEnumerable type
        /// </summary>
        private class EnumerableHashCodeProvider : HashCodeProvider
        {
            public static HashCodeProvider Instance { get; } = new EnumerableHashCodeProvider();

            public override int GetHashCode(object parameter)
            {
                if (parameter is not IEnumerable enumerable)
                {
                    return 0;
                }

                var hashCode = 0;
                var enumerator = enumerable.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    hashCode = HashCode.Combine(hashCode, OtherHashCodeProvider.Instance.GetHashCode(enumerator.Current));
                }
                return hashCode;
            }
        }

        /// <summary>
        /// The hash providers for dictionary types
        /// </summary>
        private class DictionaryHashCodeProvider : HashCodeProvider
        {
            public static HashCodeProvider Instance { get; } = new DictionaryHashCodeProvider();

            public override int GetHashCode(object parameter)
            {
                if (parameter is not IDictionary<string, object> dic)
                {
                    return 0;
                }

                var hashCode = 0;
                foreach (var item in dic)
                {
                    hashCode = HashCode.Combine(hashCode, OtherHashCodeProvider.Instance.GetHashCode(item.Key));
                    hashCode = HashCode.Combine(hashCode, OtherHashCodeProvider.Instance.GetHashCode(item.Value));
                }
                return hashCode;
            }
        }

        /// <summary>
        /// The hash providers for other types
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
