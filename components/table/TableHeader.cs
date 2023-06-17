// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public class TableHeader<TData> : Column<TData>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (ColSpan != 1) HeaderColSpan = ColSpan;
            if (ChildContent != null) TitleTemplate = ChildContent;
        }

        protected override void OnParametersSet()
        {
            if (ColSpan != 1) HeaderColSpan = ColSpan;
            if (ChildContent != null) TitleTemplate = ChildContent;
            base.OnParametersSet();
        }
    }
}
