// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;

namespace AntDesign.TableModels
{
    public static class QueryableExtensions
    {
        public static IQueryable<TItem> ExecuteTableQuery<TItem>(this IQueryable<TItem> source, QueryModel<TItem> queryModel)
        {
            return queryModel.ExecuteQuery(source);
        }

        public static IQueryable<TItem> CurrentPagedRecords<TItem>(this IQueryable<TItem> source, QueryModel<TItem> queryModel)
        {
            return queryModel.CurrentPagedRecords(source);
        }
    }
}
