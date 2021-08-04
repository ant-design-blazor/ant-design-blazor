// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class TransferRenderProps
    {
        public TransferDirection Direction { get; set; }

        public IEnumerable<TransferItem> FilteredItems { get; set; }

        public EventCallback<(string[] Keys, bool Selected)> OnItemSelectAll { get; set; }

        public EventCallback<(string Key, bool Selected)> OnItemSelect { get; set; }

        public string[] SelectedKeys { get; set; }

        public bool Disabled { get; set; }
    }
}
