using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public partial class AntTree : AntDomComponentBase
    {
        [Parameter]
        public bool Checkable { get; set; }

        [Parameter]
        public bool ShowIcon { get; set; }

        [Parameter]
        public string IconType { get; set; }

        [Parameter]
        public EventCallback<AntTreeEventArgs> OnCheckedStateChanged { get; set; }

        [Parameter]
        public EventCallback<AntTreeEventArgs> OnNodeSelected { get; set; }

        public RenderFragment<AntTreeNode> IconTemplate { get; set; }

        [Parameter]
        public RenderFragment<AntTreeNode> NodeTemplate { get; set; }

        public AntTree()
        {
            Class = "ant-tree";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:check argument", Justification = "<Ignore>")]
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {

            builder.OpenElement(1, "div");
            builder.AddAttribute(2, "class", ClassMapper.Class);

            builder.OpenElement(4, "div");
            builder.AddAttribute(5, "class", "ant-tree-list-holder-inner");
            builder.AddAttribute(6, "style", "display: flex; flex-direction: column;");

            if (_nodelist != null)
            {
                if (Checkable)
                {
                    foreach (AntTreeNode node in _nodelist)
                    {
                        node?.UpdateCheckedStateRecursive();
                    }
                }

                int totalcount = _nodelist.Count;
                int index = 0;
                foreach (AntTreeNode node in _nodelist)
                {
                    node?.RenderRecursive(builder, this, 0, index++, totalcount);
                }
            }

            builder.CloseElement();
            builder.CloseElement();
        }

        List<AntTreeNode> _nodelist;
        public List<AntTreeNode> NodeList
        {
            get
            {
                if (_nodelist == null) _nodelist = new List<AntTreeNode>();
                return _nodelist;
            }
        }

        [Parameter]
        public IReadOnlyCollection<AntTreeNode> Nodes
        {
            get
            {
                if (_nodelist != null)
                    return _nodelist;
                return Array.Empty<AntTreeNode>();
            }
            set
            {
                if (value == null || value.Count == 0)
                {
                    _nodelist = null;
                    return;
                }
                if (_nodelist == null) _nodelist = new List<AntTreeNode>();
                else if (_nodelist.Count != 0) _nodelist.Clear();
                _nodelist.AddRange(value);
            }
        }

        public void DeselectAll()
        {
            foreach (var node in Nodes)
                node?.DeselectAll();
        }
    }


}
