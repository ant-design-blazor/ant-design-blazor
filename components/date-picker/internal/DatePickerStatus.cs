// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign.Internal
{
    internal class DatePickerStatus
    {
        public string InitPicker { get; set; } = null;
        public bool IsValueSelected { get; set; }
        public DateTime? SelectedValue { get; set; }
    }
}
