// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

namespace AntDesign
{
    public class CascaderNode
    {
        /// <summary>
        /// Label displayed for value
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Value for when option is selected
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Disable the option or not
        /// </summary>
        /// <default value="false" />
        public bool Disabled { get; set; }

        internal int Level { get; set; }

        internal CascaderNode ParentNode { get; set; }

        internal bool HasChildren { get { return Children?.Any() == true; } }

        /// <summary>
        /// Options under this one
        /// </summary>
        public IEnumerable<CascaderNode> Children { get; set; }
    }

    internal enum SelectedTypeEnum
    {
        Click,
        Hover
    }
}
