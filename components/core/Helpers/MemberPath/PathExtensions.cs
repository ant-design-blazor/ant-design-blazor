// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace AntDesign.Core.Helpers.MemberPath
{
    internal static class PathExtensions
    {
        #region Get

        /// <summary>
        /// Get member value by path
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue PathGet<TItem, TValue>(this TItem item, string path)
        {
            var func = PathHelper.GetDelegate<TItem, TValue>(path);
            return func.Invoke(item);
        }

        /// <summary>
        /// Get member value by path
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue PathGet<TValue>(this object item, string path)
        {
            var func = PathHelper.GetDelegate<TValue>(path, item.GetType());
            return func.Invoke(item);
        }

        /// <summary>
        /// Get member value by path
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static object PathGet(this object item, string path)
        {
            var func = PathHelper.GetDelegate(path, item.GetType());
            return func.Invoke(item);
        }

        #endregion

        #region GetOrDefault

        /// <summary>
        /// Get nullable member value by path, it will catch all exceptions and return null
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue? PathGetOrDefault<TItem, TValue>(this TItem item, string path)
        {
            var func = PathHelper.GetDelegateDefault<TItem, TValue>(path);
            return func.Invoke(item);
        }

        /// <summary>
        /// Get nullable member value by path, it will catch all exceptions and return null
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue? PathGetOrDefault<TValue>(this object item, string path)
        {
            var func = PathHelper.GetDelegateDefault<TValue>(path, item.GetType());
            return func.Invoke(item);
        }

        /// <summary>
        /// Get nullable member value by path, it will catch all exceptions and return null
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static object? PathGetOrDefault(this object item, string path)
        {
            var func = PathHelper.GetDelegateDefault(path, item.GetType());
            return func.Invoke(item);
        }

        #endregion

        #region Set

        /// <summary>
        /// Set member value by path
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        /// <param name="value"></param>
        public static void PathSet(this object item, string path, object value)
        {
            var action = PathHelper.SetDelegate(path, item.GetType());
            action.Invoke(item, value);
        }

        /// <summary>
        /// Set member value by path
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        /// <param name="value"></param>
        /// <typeparam name="TITem"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        public static void PathSet<TITem, TValue>(this TITem item, string path, TValue value)
        {
            var action = PathHelper.SetDelegate<TITem, TValue>(path);
            action.Invoke(item, value);
        }

        /// <summary>
        /// Set member value by path
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        /// <param name="value"></param>
        /// <typeparam name="TValue"></typeparam>
        public static void PathSet<TValue>(this object item, string path, TValue value)
        {
            var action = PathHelper.SetDelegate<TValue>(path, item.GetType());
            action.Invoke(item, value);
        }

        #endregion
    }
}
