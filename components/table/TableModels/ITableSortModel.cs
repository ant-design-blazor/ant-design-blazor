// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;

namespace AntDesign.TableModels
{
    public interface ITableSortModel : ICloneable
    {
        [Obsolete("Use SortDirection instead")]
        public string Sort { get; }
        public int Priority { get; }

        public string FieldName { get; }

        public int ColumnIndex { get; }

        public SortDirection SortDirection { get; }

        internal void SetSortDirection(SortDirection sortDirection);

        internal IQueryable<TItem> SortList<TItem>(IQueryable<TItem> source);

        internal void BuildGetFieldExpression<TItem>();
    }
}
