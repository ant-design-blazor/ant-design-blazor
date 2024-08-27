// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class CalendarHeaderRenderArgs
    {
        public DateTime Value { get; set; }
        public string Type { get; set; }
        public EventCallback<DateTime> OnChange { get; set; }
        public EventCallback<string> OnTypeChange { get; set; }
    }
}
