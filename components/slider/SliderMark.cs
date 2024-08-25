// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/* Unmerged change from project 'AntDesign(net5.0)'
Before:
using Microsoft.AspNetCore.Components;
After:
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
*/
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class SliderMark
    {
        public double Key { get; }
        public RenderFragment Value { get; }
        public string Style { get; }

        public SliderMark(double key, string value)
        {
            Key = key;
            Value = (b) => b.AddContent(0, value);
        }

        public SliderMark(double key, RenderFragment value, string style)
        {
            Key = key;
            Value = value;
            Style = style;
        }
    }
}
