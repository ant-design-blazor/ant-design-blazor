// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    /**
    <summary>
    
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/alicdn/f-SbcX2Lx/Table.svg", Columns = 1, Title = "Quick Table", SubTitle = "快速表格")]
    public class QuickTable<TItem> : Table<TItem>
    {
        public QuickTable()
        {
            Quick = true;
        }
    }
}
