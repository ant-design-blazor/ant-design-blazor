// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class ListItemLayout : EnumValue<ListItemLayout>
    {
        public static ListItemLayout Horizontal = new ListItemLayout(nameof(Horizontal), 0);

        public static ListItemLayout Vertical = new ListItemLayout(nameof(Vertical), 1);

        public ListItemLayout(string name, int value) : base(name, value) { }
    }
}
