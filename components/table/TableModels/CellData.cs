// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.TableModels
{
    public class CellData
    {
        public RowData RowData { get; }

        public CellData(RowData rowData)
        {
            RowData = rowData;
        }
    }
}
