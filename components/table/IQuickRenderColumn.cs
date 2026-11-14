// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    internal interface IQuickRenderColumn
    {
        RenderFragment RenderQuickBody(RowData rowData);
    }
}
