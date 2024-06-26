using System.Runtime.CompilerServices;

namespace AntDesign.core.Extensions
{
    public static class DataConversionExtensions
    {
        /// <summary>
        /// Converts the generic type TFrom to the specified TTo type
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="fromValue"></param>
        /// <returns></returns>
        public static TTo Convert<TFrom, TTo>(TFrom fromValue)
        {
            return Unsafe.As<TFrom, TTo>(ref fromValue);
        }
    }
}
