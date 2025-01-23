﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace AntDesign
{
    public interface ISelectionColumn : IColumn
    {
        public SelectionType Type { get; set; }

        public bool Disabled { get; }

        public string Key { get; }

        public bool Selected { get; }

        public bool CheckStrictly { get; set; }

        internal IList<ISelectionColumn> RowSelections { get; }

        internal void StateHasChanged();

        internal void ResetSelected();

        internal void OnDataSourceChange();
    }
}
