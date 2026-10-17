// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// Context supplied to a custom pagination size changer.
    /// </summary>
    public sealed class PaginationSizeChangerContext
    {
        public int Value { get; }
        public bool Disabled { get; }
        public string Class { get; }
        public EventCallback<int> OnChange { get; }

        public PaginationSizeChangerContext(int value, bool disabled, string @class, EventCallback<int> onChange)
        {
            Value = value;
            Disabled = disabled;
            Class = @class;
            OnChange = onChange;
        }
    }
}
