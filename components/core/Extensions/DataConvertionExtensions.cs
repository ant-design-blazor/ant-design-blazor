using System.Runtime.CompilerServices;

namespace AntDesign.core.Extensions
{
    public static class DataConvertionExtensions
    {
        /// <summary>
        /// 将泛型类型TFrom转换为指定的TTo类型
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
