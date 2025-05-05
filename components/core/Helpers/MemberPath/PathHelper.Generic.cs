// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

namespace AntDesign.Core.Helpers.MemberPath
{
    public static partial class PathHelper
    {
        /// <summary>
        /// Get a get value delegate
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Func<TItem, TValue> GetDelegate<TItem, TValue>(string path)
        {
            var func = GetDelegate(path, typeof(TItem), typeof(TItem), typeof(TValue), false);
            return (Func<TItem, TValue>)func;
        }

        /// <summary>
        /// Get a get value delegate
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="path"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Func<object, TValue> GetDelegate<TValue>(string path, Type itemType)
        {
            var func = GetDelegate(path, itemType, typeof(object), typeof(TValue), false);
            return (Func<object, TValue>)func;
        }

        /// <summary>
        /// Get a get value delegate
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Func<object, object> GetDelegate(string path, Type itemType)
        {
            var func = GetDelegate(path, itemType, typeof(object), typeof(object), false);
            return (Func<object, object>)func;
        }

        /// <summary>
        /// Get a get value lambda expression
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Expression<Func<TItem, TValue>> GetLambda<TItem, TValue>(string path)
        {
            var lambda = GetLambda(path, typeof(TItem), typeof(TItem), typeof(TValue), false);
            return (Expression<Func<TItem, TValue>>)lambda;
        }

        /// <summary>
        /// Get a get value lambda expression
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="path"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Expression<Func<object, TValue>> GetLambda<TValue>(string path, Type itemType)
        {
            var lambdaExp = GetLambda(path, itemType, typeof(object), typeof(TValue), false);
            return (Expression<Func<object, TValue>>)lambdaExp;
        }

        /// <summary>
        /// Get a get value lambda expression
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Expression<Func<object, object>> GetLambda(string path, Type itemType)
        {
            var lambdaExp = GetLambda(path, itemType, typeof(object), typeof(object), false);
            return (Expression<Func<object, object>>)lambdaExp;
        }

        /// <summary>
        /// Get a get nullable value delegate
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Func<TItem, TValue> GetDelegateDefault<TItem, TValue>(string path)
        {
            var func = GetDelegate(path, typeof(TItem), typeof(TItem), typeof(TValue), true);
            return (Func<TItem, TValue>)func;
        }

        /// <summary>
        /// Get a get nullable value delegate
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="path"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Func<object, TValue> GetDelegateDefault<TValue>(string path, Type itemType)
        {
            var func = GetDelegate(path, itemType, typeof(object), typeof(TValue), true);
            return (Func<object, TValue>)func;
        }

        /// <summary>
        /// Get a get nullable value delegate
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Func<object, object?> GetDelegateDefault(string path, Type itemType)
        {
            var func = GetDelegate(path, itemType, typeof(object), typeof(object), true);
            return (Func<object, object?>)func;
        }

        /// <summary>
        /// Get a get nullable value lambda expression
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Expression<Func<TItem, TValue?>> GetLambdaDefault<TItem, TValue>(string path)
        {
            var lambda = GetLambda(path, typeof(TItem), typeof(TItem), typeof(TValue), true);
            return (Expression<Func<TItem, TValue?>>)lambda;
        }

        /// <summary>
        /// Get a get nullable value lambda expression
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="path"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Expression<Func<object, TValue?>> GetLambdaDefault<TValue>(string path, Type itemType)
        {
            var lambdaExp = GetLambda(path, itemType, typeof(object), typeof(TValue), true);
            return (Expression<Func<object, TValue?>>)lambdaExp;
        }

        /// <summary>
        /// Get a get nullable value lambda expression
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Expression<Func<object, object?>> GetLambdaDefault(string path, Type itemType)
        {
            var lambdaExp = GetLambda(path, itemType, typeof(object), typeof(object), true);
            return (Expression<Func<object, object?>>)lambdaExp;
        }

        /// <summary>
        /// Get a set value delegate
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Action<TItem, TValue> SetDelegate<TItem, TValue>(string path)
        {
            var action = SetDelegate(path, typeof(TItem), typeof(TItem), typeof(TValue));
            return (Action<TItem, TValue>)action;
        }

        /// <summary>
        /// Get a set value delegate
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="path"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Action<object, TValue> SetDelegate<TValue>(string path, Type itemType)
        {
            var action = SetDelegate(path, itemType, typeof(object), typeof(TValue));
            return (Action<object, TValue>)action;
        }

        /// <summary>
        /// Get a set value delegate
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Action<object, object> SetDelegate(string path, Type itemType)
        {
            var action = SetDelegate(path, itemType, typeof(object), typeof(object));
            return (Action<object, object>)action;
        }

        /// <summary>
        /// Get a set value lambda expression
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Expression<Action<TItem, TValue>> SetLambda<TItem, TValue>(string path)
        {
            var action = SetLambda(path, typeof(TItem), typeof(TItem), typeof(TValue));
            return (Expression<Action<TItem, TValue>>)action;
        }

        /// <summary>
        /// Get a set value lambda expression
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="path"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Expression<Action<object, TValue>> SetLambda<TValue>(string path, Type itemType)
        {
            var lambda = SetLambda(path, itemType, typeof(object), typeof(TValue));
            return (Expression<Action<object, TValue>>)lambda;
        }

        /// <summary>
        /// Get a set value lambda expression
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Expression<Action<object, object>> SetLambda(string path, Type itemType)
        {
            var lambda = SetLambda(path, itemType, typeof(object), typeof(object));
            return (Expression<Action<object, object>>)lambda;
        }
    }
}
