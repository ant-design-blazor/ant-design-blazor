// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign
{
    public class CalendarHeaderRenderArgs
    {
        public DateTime Value { get; set; }
        public CalendarMode Type { get; set; }
        public Action<DateTime> OnChange { get; set; }
        public Action<CalendarMode> OnTypeChange { get; set; }
    }
}
