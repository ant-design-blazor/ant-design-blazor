// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class DirectionVHType : EnumValue<DirectionVHType>
    {
        public static readonly DirectionVHType Vertical = new DirectionVHType(nameof(Vertical), 0);
        public static readonly DirectionVHType Horizontal = new DirectionVHType(nameof(Horizontal), 1);

        private DirectionVHType(string name, int value) : base(name, value)
        {
        }
    }
}
