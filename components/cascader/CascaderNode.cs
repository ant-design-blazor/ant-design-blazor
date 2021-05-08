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
