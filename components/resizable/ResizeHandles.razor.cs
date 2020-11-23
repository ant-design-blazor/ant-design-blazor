// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public partial class ResizeHandles
    {
        private List<string> _resizeDirection = new List<string>{
            "top",
            "right",
            "bottom",
            "left",
            "topRight",
            "bottomRight",
            "bottomLeft",
            "topLeft"
        };
    }
}
