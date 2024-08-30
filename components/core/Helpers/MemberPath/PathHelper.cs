// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AntDesign.Core.Helpers.MemberPath
{
    public static partial class PathHelper
    {
        #region Constant Names

        private const string NullableHasValue = "HasValue";

        private const string NullableValue = "Value";

        private const string GetItemMethod = "get_Item";

        private const string ContainsKeyMethod = "ContainsKey";

        private const string ListCount = "Count";

        #endregion

        #region Cache

        private static readonly PathConfigEqualityComparer _comparer = new();

        private static readonly ConcurrentDictionary<PathGetConfig, PathGetExpression> _getExpressionCache = new(_comparer);

        private static readonly ConcurrentDictionary<PathGetConfig, LambdaExpression> _getLambdaCache = new(_comparer);

        private static readonly ConcurrentDictionary<PathGetConfig, Delegate> _getDelegateCache = new(_comparer);

        private static readonly ConcurrentDictionary<PathSetConfig, PathSetExpression> _setExpressionCache = new(_comparer);

        private static readonly ConcurrentDictionary<PathSetConfig, LambdaExpression> _setLambdaCache = new(_comparer);

        private static readonly ConcurrentDictionary<PathSetConfig, Delegate> _setDelegateCache = new(_comparer);

        #endregion

        #region GetValue

        /// <summary>
        /// Get a get value delegate, the type of the delegate depends on <paramref name="paramType"/> and <paramref name="valueType"/>:<br/>
        /// 'Func&lt;TItem, TValue&gt;': <paramref name="paramType"/> = typeof(TItem), <paramref name="valueType"/> = typeof(TValue), <paramref name="checkNull"/> = false.<br/>
        /// 'Func&lt;object, TValue&gt;': <paramref name="paramType"/> = typeof(object), <paramref name="valueType"/> = typeof(TValue), <paramref name="checkNull"/> = false.<br/>
        /// 'Func&lt;object, object&gt;': <paramref name="paramType"/> = typeof(object), <paramref name="valueType"/> = typeof(object), <paramref name="checkNull"/> = false.<br/>
        /// 'Func&lt;TItem, TValue?&gt;': <paramref name="paramType"/> = typeof(TItem), <paramref name="valueType"/> = typeof(TValue), <paramref name="checkNull"/> = true.<br/>
        /// 'Func&lt;object, TValue?&gt;': <paramref name="paramType"/> = typeof(object), <paramref name="valueType"/> = typeof(TValue), <paramref name="checkNull"/> = true.<br/>
        /// 'Func&lt;object, object?&gt;': <paramref name="paramType"/> = typeof(object), <paramref name="valueType"/> = typeof(object), <paramref name="checkNull"/> = true.<br/>
        /// </summary>
        /// <param name="itemType">Type of access object</param>
        /// <param name="path">String path</param>
        /// <param name="paramType">If you want to pass in the item as an object, set it to true</param>
        /// <param name="valueType">If you want to get the value as an object, set it to true</param>
        /// <param name="checkNull">If true, the delegate will check the path is not null and return value or default value, if the type of return value is ValueType, the delegate will return Nullable&lt;T&gt;,
        /// if false, the delegate will not check the path not null, so that if the path contains null value or key not found, it will throw exception</param>
        /// <returns></returns>
        public static Delegate GetDelegate(string path, Type itemType, Type paramType, Type valueType, bool checkNull)
        {
            var func = _getDelegateCache.GetOrAdd(new(path, itemType, paramType, valueType, checkNull), _getDelegateFactory);
            return func;
        }

        private static readonly Func<PathGetConfig, Delegate> _getDelegateFactory = key => GetLambda(key.Path, key.ItemType, key.ParamType, key.ValueType, key.CheckNull).Compile();

        /// <summary>
        /// Get a get value lambda expression, the type of the lambda expression depends on <paramref name="paramType"/> and <paramref name="valueType"/>:<br/>
        /// 'Expression&lt;Func&lt;TItem, TValue&gt;&gt;': <paramref name="paramType"/> = typeof(TItem), <paramref name="valueType"/> = typeof(TValue), <paramref name="checkNull"/> = false.<br/>
        /// 'Expression&lt;Func&lt;object, TValue&gt;&gt;': <paramref name="paramType"/> = typeof(object), <paramref name="valueType"/> = typeof(TValue), <paramref name="checkNull"/> = false.<br/>
        /// 'Expression&lt;Func&lt;object, object&gt;&gt;': <paramref name="paramType"/> = typeof(object), <paramref name="valueType"/> = typeof(object), <paramref name="checkNull"/> = false.<br/>
        /// 'Expression&lt;Func&lt;TItem, TValue?&gt;&gt;': <paramref name="paramType"/> = typeof(TItem), <paramref name="valueType"/> = typeof(TValue), <paramref name="checkNull"/> = true.<br/>
        /// 'Expression&lt;Func&lt;object, TValue?&gt;&gt;': <paramref name="paramType"/> = typeof(object), <paramref name="valueType"/> = typeof(TValue), <paramref name="checkNull"/> = true.<br/>
        /// 'Expression&lt;Func&lt;object, object?&gt;&gt;': <paramref name="paramType"/> = typeof(object), <paramref name="valueType"/> = typeof(object), <paramref name="checkNull"/> = true.<br/>
        /// </summary>
        /// <param name="itemType">Type of access object</param>
        /// <param name="path">String path</param>
        /// <param name="paramType">If you want to pass in the item as an object, set it to true</param>
        /// <param name="valueType">If you want to get the value as an object, set it to true</param>
        /// <param name="checkNull">If true, the delegate will check the path is not null and return value or default value, if the type of return value is ValueType, the delegate will return Nullable&lt;T&gt;,
        /// if false, the delegate will not check the path not null, so that if the path contains null value or key not found, it will throw exception</param>
        /// <returns></returns>
        public static LambdaExpression GetLambda(string path, Type itemType, Type paramType, Type valueType, bool checkNull)
        {
            var lambdaExp = _getLambdaCache.GetOrAdd(new(path, itemType, paramType, valueType, checkNull), _getLambdaFactory);
            return lambdaExp;
        }

        private static readonly Func<PathGetConfig, LambdaExpression> _getLambdaFactory = key =>
        {
            var (body, parameterExp) = GetExpression(key.Path, key.ItemType, key.ParamType, key.ValueType, key.CheckNull);
            var lambda = Expression.Lambda(body, parameterExp);
            return lambda;
        };

        /// <summary>
        /// Get the get value expression
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="path"></param>
        /// <param name="paramType"></param>
        /// <param name="valueType"></param>
        /// <param name="checkNull"></param>
        /// <returns></returns>
        public static PathGetExpression GetExpression(string path, Type itemType, Type paramType, Type valueType, bool checkNull)
        {
            var cacheItem = _getExpressionCache.GetOrAdd(new(path, itemType, paramType, valueType, checkNull), _getExpressionFactory);
            return cacheItem;
        }

        private static readonly Func<PathGetConfig, PathGetExpression> _getExpressionFactory =
            key => GetExpressionImplement(Parse(key.Path), key.ItemType, key.ParamType, key.ValueType, key.CheckNull);

        private static PathGetExpression GetExpressionImplement(IEnumerable<PathNode> pathLink, Type itemType, Type paramType, Type valueType, bool checkNull)
        {
            var param = Expression.Parameter(paramType);
            Expression exp = param;
            if (itemType != paramType) // Convert object to original type
            {
                exp = Expression.Convert(exp, itemType);
            }

            var (body, test) = BuildExpression(pathLink, exp, checkNull);
            exp = body;
            if (checkNull)
            {
                var nullableType = exp.Type.IsClass || Nullable.GetUnderlyingType(exp.Type) != null ? exp.Type : typeof(Nullable<>).MakeGenericType(exp.Type);
                if (test != null)
                {
                    if (Nullable.GetUnderlyingType(exp.Type) != null && Nullable.GetUnderlyingType(valueType) == null) // Nullable<VT> to VT
                    {
                        var vtExp = Expression.Condition(Expression.NotEqual(exp, Expression.Constant(null, exp.Type)), Expression.Convert(exp, valueType), Expression.Default(valueType));
                        exp = Expression.Condition(test, vtExp, Expression.Default(valueType));
                    }
                    else
                    {
                        exp = Expression.Condition(test, exp.Type != valueType ? Expression.Convert(exp, valueType) : exp, Expression.Default(valueType));
                    }
                }
                else
                {
                    exp = exp.Type != nullableType ? Expression.Convert(exp, nullableType) : exp;
                }
            }

            if (exp.Type != valueType)
            {
                exp = Expression.Convert(exp, valueType);
            }

            return new(exp, param);
        }

        #endregion

        #region Set value

        private static readonly MethodInfo _createActionLambdaMethod =
            typeof(Expression).GetMethod(nameof(Expression.Lambda), 1, new[] { typeof(Expression), typeof(ParameterExpression[]) })
         ?? throw new NullReferenceException("Expression.Lambda<T>([Expression], [ParameterExpression]) method not found");

        /// <summary>
        /// Get a set value delegate, the type of the delegate depends on <paramref name="paramType"/> and <paramref name="valueType"/>:<br/>
        /// 'Action&lt;TItem, TValue&gt;': <paramref name="paramType"/> = typeof(TItem), <paramref name="valueType"/> = typeof(TValue).<br/>
        /// 'Action&lt;object, TValue&gt;': <paramref name="paramType"/> = typeof(object), <paramref name="valueType"/> = typeof(TValue).<br/>
        /// 'Action&lt;object, object&gt;': <paramref name="paramType"/> = typeof(object), <paramref name="valueType"/> = typeof(object).<br/>
        /// </summary>
        /// <param name="itemType">Type of item</param>
        /// <param name="path">Member path string</param>
        /// <param name="paramType">Type of incoming object</param>
        /// <param name="valueType">Type of the assignment object</param>
        /// <returns></returns>
        public static Delegate SetDelegate(string path, Type itemType, Type paramType, Type valueType)
        {
            var action = _setDelegateCache.GetOrAdd(new(path, itemType, paramType, valueType), _setDelegateFactory);
            return action;
        }

        private static readonly Func<PathSetConfig, Delegate> _setDelegateFactory = key => SetLambda(key.Path, key.ItemType, key.ParamType, key.ValueType).Compile();

        /// <summary>
        /// Get a set value lambda expression, the type of the lambda expression depends on <paramref name="paramType"/> and <paramref name="valueType"/>:<br/>
        /// 'Expression&lt;Action&lt;TItem, TValue&gt;&gt;': <paramref name="paramType"/> = typeof(TItem), <paramref name="valueType"/> = typeof(TValue).<br/>
        /// 'Expression&lt;Action&lt;object, TValue&gt;&gt;': <paramref name="paramType"/> = typeof(object), <paramref name="valueType"/> = typeof(TValue).<br/>
        /// 'Expression&lt;Action&lt;object, object&gt;&gt;': <paramref name="paramType"/> = typeof(object), <paramref name="valueType"/> = typeof(object).<br/>
        /// </summary>
        /// <param name="itemType">Type of item</param>
        /// <param name="path">Member path string</param>
        /// <param name="paramType">Type of incoming object</param>
        /// <param name="valueType">Type of the assignment object</param>
        /// <returns></returns>
        public static LambdaExpression SetLambda(string path, Type itemType, Type paramType, Type valueType)
        {
            var lambda = _setLambdaCache.GetOrAdd(new(path, itemType, paramType, valueType), _setLambdaFactory);
            return lambda;
        }

        private static readonly Func<PathSetConfig, LambdaExpression> _setLambdaFactory = key =>
        {
            var (body, itemParam, valueParam) = SetExpression(key.Path, key.ItemType, key.ParamType, key.ValueType);
            var actionType = typeof(Action<,>).MakeGenericType(key.ParamType, key.ValueType);
            return (LambdaExpression)_createActionLambdaMethod.MakeGenericMethod(actionType).Invoke(null, new object[] { body, new[] { itemParam, valueParam } })!;
        };

        /// <summary>
        /// Get the set value expression
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="path"></param>
        /// <param name="paramType"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public static PathSetExpression SetExpression(string path, Type itemType, Type paramType, Type valueType)
        {
            var cacheItem = _setExpressionCache.GetOrAdd(new(path, itemType, paramType, valueType), _setExpressionFactory);
            return cacheItem;
        }

        private static readonly Func<PathSetConfig, PathSetExpression> _setExpressionFactory =
            key => SetExpressionImplement(Parse(key.Path), key.ItemType, key.ParamType, key.ValueType);

        private static PathSetExpression SetExpressionImplement(IEnumerable<PathNode> pathLink, Type itemType, Type paramType, Type valueType)
        {
            var paramExp = Expression.Parameter(paramType);
            Expression exp = paramExp;
            if (itemType != paramType) // Convert object to original type
            {
                exp = Expression.Convert(exp, itemType);
            }

            (exp, _) = BuildExpression(pathLink, exp, false);
            var valueParam = Expression.Parameter(valueType);
            if (exp is MethodCallExpression me && me.Method.Name == GetItemMethod)
            {
                exp = new GetItemExpressionReplacer(valueParam).Visit(exp);
            }
            else
            {
                exp = Expression.Assign(exp, exp.Type == valueType ? valueParam : Expression.Convert(valueParam, exp.Type));
            }

            return new(exp, paramExp, valueParam);
        }

        #endregion

        #region Common Method

        private static (Expression body, Expression? test) BuildExpression(IEnumerable<PathNode> pathLink, Expression exp, bool checkNull)
        {
            Expression? test = null;
            foreach (var pathNode in pathLink)
            {
                while (Nullable.GetUnderlyingType(exp.Type) != null)
                {
                    if (checkNull)
                    {
                        var notNull = Expression.Property(exp, NullableHasValue);
                        test = test != null ? Expression.AndAlso(test, notNull) : notNull;
                    }

                    exp = Expression.Property(exp, NullableValue);
                }

                if (checkNull && exp.Type.IsClass)
                {
                    var notNull = Expression.NotEqual(exp, Expression.Constant(null, exp.Type));
                    test = test != null ? Expression.AndAlso(test, notNull) : notNull;
                }

                switch (pathNode.NodeType)
                {
                    case PathNodeType.Member:
                        {
                            exp = Expression.PropertyOrField(exp, pathNode.Name);
                            break;
                        }
                    case PathNodeType.StringIndex:
                        {
                            if (checkNull && exp.Type.IsClass)
                            {
                                var containsKeyMethod = exp.Type.GetMethod(ContainsKeyMethod, BindingFlags.Public | BindingFlags.Instance);
                                if (containsKeyMethod != null)
                                {
                                    var containsKeyTest = Expression.IsTrue(Expression.Call(exp, containsKeyMethod, Expression.Constant(pathNode.Name, typeof(string))));
                                    test = test != null ? Expression.AndAlso(test, containsKeyTest) : containsKeyTest;
                                }
                            }

                            var getMethod = exp.Type.GetMethod(GetItemMethod, BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(string) }, null)
                                         ?? throw new InvalidPathException($"'{GetItemMethod}' method is not implemented in '{exp.Type}'");
                            exp = Expression.Call(exp, getMethod, Expression.Constant(pathNode.Name));

                            break;
                        }
                    case PathNodeType.NumberIndex:
                        {
                            if (exp.Type.IsArray)
                            {
                                var index = Expression.Constant(int.Parse(pathNode.Name));
                                if (checkNull)
                                {
                                    var lengthAccess = Expression.ArrayLength(exp);
                                    var lengthTest = Expression.AndAlso(Expression.LessThan(index, lengthAccess), Expression.GreaterThanOrEqual(index, Expression.Constant(0, typeof(int))));
                                    test = test != null ? Expression.AndAlso(test, lengthTest) : lengthTest;
                                }

                                exp = Expression.ArrayAccess(exp, index);
                            }
                            else
                            {
                                var getMethod = exp.Type.GetMethod(GetItemMethod, BindingFlags.Public | BindingFlags.Instance)
                                             ?? throw new InvalidPathException($"'{GetItemMethod}' method is not implemented in '{exp.Type}'");
                                var indexParam = getMethod.GetParameters()[0];
                                var index = GetIndex(indexParam.ParameterType, pathNode.Name);

                                if (checkNull && exp.Type.IsClass)
                                {
                                    var countProperty = exp.Type.GetProperty(ListCount) ?? throw new InvalidPathException($"'{ListCount}' property is not implemented in '{exp.Type}'");
                                    var countAccess = Expression.Property(exp, countProperty);
                                    var lengthTest = Expression.AndAlso(Expression.LessThan(index, countAccess), Expression.GreaterThanOrEqual(index, Expression.Constant(0, index.Type)));
                                    test = test != null ? Expression.AndAlso(test, lengthTest) : lengthTest;
                                }

                                exp = Expression.Call(exp, getMethod, index);
                            }

                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return (exp, test);
        }

        private static Expression GetIndex(Type indexType, string indexKey)
        {
            // int at first, it is the most common key type
            if (indexType == typeof(int))
            {
                return Expression.Constant(int.Parse(indexKey));
            }

            if (indexType == typeof(float))
            {
                return Expression.Constant(float.Parse(indexKey));
            }

            if (indexType == typeof(long))
            {
                return Expression.Constant(long.Parse(indexKey));
            }

            if (indexType == typeof(double))
            {
                return Expression.Constant(double.Parse(indexKey));
            }

            if (indexType == typeof(sbyte))
            {
                return Expression.Constant(sbyte.Parse(indexKey));
            }

            if (indexType == typeof(byte))
            {
                return Expression.Constant(byte.Parse(indexKey));
            }

            if (indexType == typeof(short))
            {
                return Expression.Constant(short.Parse(indexKey));
            }

            if (indexType == typeof(ushort))
            {
                return Expression.Constant(ushort.Parse(indexKey));
            }

            if (indexType == typeof(uint))
            {
                return Expression.Constant(uint.Parse(indexKey));
            }

            if (indexType == typeof(ulong))
            {
                return Expression.Constant(ulong.Parse(indexKey));
            }

            if (indexType == typeof(decimal))
            {
                return Expression.Constant(decimal.Parse(indexKey));
            }

            throw new NotSupportedException($"{indexType.Name} type index is not supported");
        }

        public static List<PathNode> Parse(string stringPath)
        {
            var path = stringPath.AsSpan();
            var pathList = new List<PathNode>();
            var nameCache = new StringBuilder();
            var nameIsStringKey = false;
            var prevChar = PathCharState.Begin;
            for (int i = 0; i < path.Length; i++)
            {
                var c = path[i];
                if (c == '[')
                {
                    if (prevChar == PathCharState.StringKey)
                    {
                        // In string key, it is a normal character for string key
                        nameCache.Append(c);
                        nameIsStringKey = true;
                    }
                    else if (prevChar is PathCharState.Begin or PathCharState.MemberName or PathCharState.RightBracket) // starts of index key
                    {
                        if (prevChar == PathCharState.MemberName)
                        {
                            // Member name before, nameCache should be not empty, save nameCache as member name path node
                            Debug.Assert(nameCache.Length > 0);
                            var pathInfo = PathNode.NewMember(nameCache.ToString());
                            nameCache.Clear();
                            pathList.Add(pathInfo);
                        }
                        else
                        {
                            // If it is not followed by a member name, the nameCache should be empty
                            Debug.Assert(nameCache.Length == 0);
                        }

                        prevChar = PathCharState.LeftBracket;
                    }
                    else
                    {
                        // Either in the string key or the index key start character, nothing else is possible.
                        throw new InvalidPathException("Unexpected left bracket");
                    }
                }
                else if (c == ']')
                {
                    if (prevChar == PathCharState.StringKey)
                    {
                        // In string key, it is a normal character for string key
                        nameCache.Append(c);
                        nameIsStringKey = true;
                    }
                    else if (prevChar is PathCharState.SingleQuote or PathCharState.NumberKey)
                    {
                        // End of index key, nameCache should be not empty, save nameCache as member name path node
                        prevChar = PathCharState.RightBracket;
                        var pathInfo = PathNode.NewIndex(nameCache.ToString(), nameIsStringKey);
                        nameCache.Clear();
                        nameIsStringKey = false;
                        pathList.Add(pathInfo);
                    }
                    else
                    {
                        // Either in the string key or in the index key ending character, nothing else is possible.
                        throw new InvalidPathException("Unexpected right bracket");
                    }
                }
                else if (c == '\'')
                {
                    if (prevChar == PathCharState.LeftBracket)
                    {
                        // Pattern "['", it is the start of the string index key
                        prevChar = PathCharState.SingleQuote;
                        nameIsStringKey = true;
                    }
                    else if (prevChar == PathCharState.StringKey)
                    {
                        // Appears in string key
                        if (path[i + 1] == '\'') // Check next character
                        {
                            // Single quote appears twice in string key, means it is an escape character.
                            // Example: ['abc''def'], the real string key will be "abc'def"
                            nameCache.Append(c);
                            nameIsStringKey = true;
                            i++;
                        }
                        else if (path[i + 1] == ']') // Check next character
                        {
                            // Pattern "']", it is the end of string index key
                            prevChar = PathCharState.SingleQuote;
                            nameIsStringKey = true;
                        }
                        else
                        {
                            // If a single quote appears in a string key, it either appears twice at the same time, indicating a single quote escape,
                            // or it is the ending character of the string index key, and there are no other possibilities
                            throw new InvalidPathException(
                                "Single quote in string index key should use single quote to escape, example: actual key is [\"abc'def\"],"
                              + " the path string should be ['abc''def']");
                        }
                    }
                    else
                    {
                        // The single quotes are either after the left bracket or in the string key, there is no other possibility
                        throw new InvalidPathException("Unexpected single quote");
                    }
                }
                else if (c == '.')
                {
                    if (prevChar is PathCharState.StringKey or PathCharState.NumberKey)
                    {
                        // Dot appears in string key, it is a normal character
                        nameCache.Append(c);
                    }
                    else if (prevChar == PathCharState.MemberName) //
                    {
                        // Dot after member name, it is a member access operator, nameCache should be not empty, save nameCache as member name path node
                        Debug.Assert(nameCache.Length != 0);
                        var pathInfo = PathNode.NewMember(nameCache.ToString());
                        nameCache.Clear();
                        pathList.Add(pathInfo);
                        prevChar = PathCharState.Dot;
                    }
                    else if (prevChar == PathCharState.RightBracket)
                    {
                        // Pattern "].", access an index object
                        prevChar = PathCharState.Dot;
                    }
                    else
                    {
                        // The dot character can only appear in a string key, or after a member name, or after a right bracket, nothing else is possible
                        throw new InvalidPathException("Unexpected dot character");
                    }
                }
                else
                {
                    // read normal character
                    switch (prevChar)
                    {
                        case PathCharState.Begin:
                        case PathCharState.Dot:
                        case PathCharState.MemberName:
                            prevChar = PathCharState.MemberName;
                            break;
                        case PathCharState.SingleQuote:
                        case PathCharState.StringKey:
                            prevChar = PathCharState.StringKey;
                            break;
                        case PathCharState.LeftBracket:
                        case PathCharState.NumberKey:
                            prevChar = PathCharState.NumberKey;
                            break;
                        default:
                            throw new InvalidPathException($"Unexpected character '{c}' after {prevChar} character");
                    }

                    nameCache.Append(c);
                }
            }

            // Save member name path node if the path ends with a member name
            if (prevChar == PathCharState.MemberName)
            {
                Debug.Assert(nameCache.Length != 0);
                var pathInfo = PathNode.NewMember(nameCache.ToString());
                nameCache.Clear();
                pathList.Add(pathInfo);
                return pathList;
            }

            if (prevChar == PathCharState.RightBracket)
            {
                return pathList;
            }

            // Path can only end in member name or right bracket
            throw new InvalidPathException($"Unexpected path end character '{prevChar}', only member names or right brackets can be used as the end of a path");
        }

        #endregion

        private readonly struct PathGetConfig
        {
            public readonly string Path;

            public readonly Type ItemType;

            public readonly Type ParamType;

            public readonly Type ValueType;

            public readonly bool CheckNull;

            public PathGetConfig(string path, Type itemType, Type paramType, Type valueType, bool checkNull)
            {
                Path = path;
                ItemType = itemType;
                ParamType = paramType;
                ValueType = valueType;
                CheckNull = checkNull;
            }

            public bool Equals(PathGetConfig other)
            {
                return Path == other.Path
                    && ItemType == other.ItemType
                    && ParamType == other.ParamType
                    && ValueType == other.ValueType
                    && CheckNull == other.CheckNull;
            }

            public override bool Equals(object? obj)
            {
                return obj is PathGetConfig other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Path, ItemType, ParamType, ValueType, CheckNull);
            }

            public static bool operator ==(PathGetConfig x, PathGetConfig y)
            {
                return x.Equals(y);
            }

            public static bool operator !=(PathGetConfig x, PathGetConfig y)
            {
                return !x.Equals(y);
            }
        }

        private readonly struct PathSetConfig
        {
            public readonly string Path;

            public readonly Type ItemType;

            public readonly Type ParamType;

            public readonly Type ValueType;

            public PathSetConfig(string path, Type itemType, Type paramType, Type valueType)
            {
                Path = path;
                ItemType = itemType;
                ParamType = paramType;
                ValueType = valueType;
            }

            public bool Equals(PathSetConfig other)
            {
                return Path == other.Path
                    && ItemType == other.ItemType
                    && ParamType == other.ParamType
                    && ValueType == other.ValueType;
            }

            public override bool Equals(object? obj)
            {
                return obj is PathSetConfig other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Path, ItemType, ParamType, ValueType);
            }

            public static bool operator ==(PathSetConfig x, PathSetConfig y)
            {
                return x.Equals(y);
            }

            public static bool operator !=(PathSetConfig x, PathSetConfig y)
            {
                return !x.Equals(y);
            }
        }

        private class PathConfigEqualityComparer : IEqualityComparer<PathGetConfig>, IEqualityComparer<PathSetConfig>
        {
            public bool Equals(PathGetConfig x, PathGetConfig y) => x.Equals(y);

            public int GetHashCode(PathGetConfig obj) => obj.GetHashCode();

            public bool Equals(PathSetConfig x, PathSetConfig y) => x.Equals(y);

            public int GetHashCode(PathSetConfig obj) => obj.GetHashCode();
        }

        public readonly struct PathGetExpression
        {
            public readonly Expression Body;

            public readonly ParameterExpression ItemParameter;

            public PathGetExpression(Expression body, ParameterExpression itemParameter)
            {
                Body = body;
                ItemParameter = itemParameter;
            }

            public void Deconstruct(out Expression body, out ParameterExpression itemParameter)
            {
                body = Body;
                itemParameter = ItemParameter;
            }
        }

        public readonly struct PathSetExpression
        {
            public readonly Expression Body;

            public readonly ParameterExpression ItemParameter;

            public readonly ParameterExpression ValueParameter;

            public PathSetExpression(Expression body, ParameterExpression itemParameter, ParameterExpression valueParameter)
            {
                Body = body;
                ItemParameter = itemParameter;
                ValueParameter = valueParameter;
            }

            public void Deconstruct(out Expression body, out ParameterExpression itemParameter, out ParameterExpression valueParameter)
            {
                body = Body;
                itemParameter = ItemParameter;
                valueParameter = ValueParameter;
            }
        }
    }
}
