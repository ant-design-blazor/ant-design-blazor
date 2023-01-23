﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;

namespace AntDesign.TableModels
{
    public interface ITableGroupModel
    {
        public int Priority { get; }

        public string FieldName { get; }

        public int ColumnIndex { get; }

        public IQueryable<GroupData<TItem>> GroupList<TItem>(IQueryable<TItem> source);
    }
}
