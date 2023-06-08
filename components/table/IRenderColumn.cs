// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Table
{
    public interface IRenderColumn
    {
        RenderFragment RenderPlaceholder();

        RenderFragment RenderMeasure();

        RenderFragment RenderColGroup();

        RenderFragment RenderHeader();

        RenderFragment RenderBody<TItem>(RowData<TItem> rowData);
    }
}
