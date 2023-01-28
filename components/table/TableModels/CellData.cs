// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntDesign.Core.Helpers;

namespace AntDesign.TableModels
{
    public class CellData
    {
        public RowData RowData { get; }
        public string FormattedValue { get; set; }

        public CellData(RowData rowData, string formattedValue)
        {
            RowData = rowData;
            FormattedValue = formattedValue;
        }

        public CellData(RowData rowData)
        {
            RowData = rowData;
        }
    }

    public class CellData<TData> : CellData
    {
        public TData FieldValue { get; set; }

        public CellData(RowData rowData, TData fieldValue, string format) : base(rowData, Formatter<TData>.Format(fieldValue, format))
        {
            FieldValue = fieldValue;
        }
    }
}
