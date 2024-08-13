// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Collections.Concurrent;

namespace AntDesign.Core.Helpers;

internal static class DynamicGroupByHelper
{
    // Cache for storing compiled functions
    private static readonly ConcurrentDictionary<string, Delegate> _functionCache = new ConcurrentDictionary<string, Delegate>();

    public static List<GroupResult<object, TEntity>> DynamicGroupBy<TEntity>(
        IQueryable<TEntity> source,
        params Expression<Func<TEntity, object>>[] keySelectors)
    {
        return DynamicGroupByInternal(source, keySelectors, 0);
    }

    private static List<GroupResult<object, TEntity>> DynamicGroupByInternal<TEntity>(
        IQueryable<TEntity> source,
        Expression<Func<TEntity, object>>[] keySelectors,
        int level)
    {
        if (level >= keySelectors.Length)
        {
            return new List<GroupResult<object, TEntity>>
            {
                new GroupResult<object, TEntity>
                {
                    Key = null,
                    Entities = source.ToList()
                }
            };
        }

        var keySelector = keySelectors[level];
        var keySelectorFunc = GetOrAddCachedFunction(keySelector);

        var groupedData = source.GroupBy(keySelectorFunc);

        var result = new List<GroupResult<object, TEntity>>();

        foreach (var group in groupedData)
        {
            var groupKey = group.Key;
            var children = DynamicGroupByInternal(group.AsQueryable(), keySelectors, level + 1);

            result.Add(new GroupResult<object, TEntity>
            {
                Key = groupKey,
                Children = children
            });
        }

        return result;
    }

    private static Func<TEntity, object> GetOrAddCachedFunction<TEntity>(Expression<Func<TEntity, object>> keySelector)
    {
        // Create a unique key by combining the expression string and the entity type
        var key = $"{typeof(TEntity).FullName}:{keySelector.ToString()}";
        if (!_functionCache.TryGetValue(key, out var cachedFunc))
        {
            cachedFunc = keySelector.Compile();
            _functionCache.TryAdd(key, cachedFunc);
        }
        return (Func<TEntity, object>)cachedFunc;
    }
}

internal class GroupResult<TKey, TEntity>
{
    public TKey Key { get; set; }
    public List<GroupResult<TKey, TEntity>> Children { get; set; } = new List<GroupResult<TKey, TEntity>>();
    public List<TEntity> Entities { get; set; } = new List<TEntity>();
}
