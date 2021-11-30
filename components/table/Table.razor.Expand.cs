// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Table<TItem>
    {
        class ExpandedRowsDictionary : Dictionary<TItem, (bool expanded, ExpandedRowsDictionary children)>
        {
        }

        private ExpandedRowsDictionary _expandedRows = new();

        private void RowDataExpandedChanged(RowData<TItem> rowData, bool expanded)
        {
            var ancestors = rowData.GetAllAncestors();
            var expandedRowsDictionary = _expandedRows;
            foreach (var ancestor in ancestors)
            {
                if (!expandedRowsDictionary.ContainsKey(ancestor.Data))
                {
                    expandedRowsDictionary.Add(ancestor.Data, (false, new()));
                }
                expandedRowsDictionary = expandedRowsDictionary[ancestor.Data].children;
            }
            if (!expandedRowsDictionary.ContainsKey(rowData.Data))
            {
                expandedRowsDictionary.Add(rowData.Data, (false, new()));
            }
            if (expanded)
            {
                expandedRowsDictionary[rowData.Data] = (true, expandedRowsDictionary[rowData.Data].children);
            }
            else
            {
                expandedRowsDictionary[rowData.Data] = (false, expandedRowsDictionary[rowData.Data].children);
            }
        }
    }
}
