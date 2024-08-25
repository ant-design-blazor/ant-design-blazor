// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

namespace AntDesign
{
    public class CascaderNode
    {
        public string Label { get; set; }

        public string Value { get; set; }

        public bool Disabled { get; set; }

        internal int Level { get; set; }

        internal CascaderNode ParentNode { get; set; }

        internal bool HasChildren { get { return Children?.Any() == true; } }

        public IEnumerable<CascaderNode> Children { get; set; }
    }

    internal enum SelectedTypeEnum
    {
        Click,
        Hover
    }
}
