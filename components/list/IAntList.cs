﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    internal interface IAntList
    {
        internal ListGridType Grid { get; }

        internal ListItemLayout ItemLayout { get; }

        internal double ColumnWidth { get; }
    }
}
