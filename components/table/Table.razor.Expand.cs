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
        private HashSet<TItem> _expandedRows = new();

        private void RowDataExpandedChanged(RowData rowData, bool expanded)
        {
            if (expanded)
            {
                _expandedRows.Add((rowData as RowData<TItem>).Data);
            }
            else
            {
                _expandedRows.Remove((rowData as RowData<TItem>).Data);
            }
        }
    }
}
