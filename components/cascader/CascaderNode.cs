using System.Collections.Generic;

namespace AntDesign
{
    public class CascaderNode
    {
        public string Label { get; set; }

        public string Value { get; set; }

        public bool Disabled { get; set; }

        internal int Level { get; set; }

        internal CascaderNode ParentNode { get; set; }

        internal bool HasChildren { get { return Children?.Count > 0; } }

        public IReadOnlyCollection<CascaderNode> Children { get; set; }
    }

    internal enum SelectedTypeEnum
    {
        Click,
        Hover
    }
}
