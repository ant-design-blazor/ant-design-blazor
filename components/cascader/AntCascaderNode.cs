using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public class AntCascaderNode
    {
        public string Label { get; set; }

        public string Value { get; set; }

        public bool Disabled { get; set; }

        internal int Level { get; set; }

        internal AntCascaderNode ParentCascaderNode { get; set; }

        public bool HasChildren { get { return Children?.Count > 0; } }

        public IReadOnlyCollection<AntCascaderNode> Children { get; set; }

        internal void RenderRecursive(RenderTreeBuilder builder, AntCascader ownerCascader)
        {
            builder.OpenElement(1, "li");
            bool isActive = ownerCascader._selectedNodes.Where(n => n.Label == Label).Any() || ownerCascader._hoverSelectedNodes.Where(n => n.Label == Label).Any();
            string activeClass = isActive ? "ant-cascader-menu-item-active" : string.Empty;
            string disabledClass = Disabled ? "ant-cascader-menu-item-disabled" : string.Empty;
            builder.AddAttribute(2, "class", $"ant-cascader-menu-item ant-cascader-menu-item-expand {disabledClass} {activeClass}");

            builder.AddAttribute(3, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, async (me) =>
            {
                if (Disabled) return;
                await ownerCascader.SetSelectedNode(this, SelectedTypeEnum.Click);
            }));
            if (ownerCascader.ExpandTrigger == "hover")
            {
                builder.AddAttribute(4, "onmouseover", EventCallback.Factory.Create<MouseEventArgs>(this, async (me) =>
                {
                    if (Disabled) return;
                    if (!HasChildren) return;

                    await ownerCascader.SetSelectedNode(this, SelectedTypeEnum.Hover);
                }));
            }
            builder.AddAttribute(5, "title", Label);
            builder.AddAttribute(6, "role", "menuitem");

            string html = Label;
            if (HasChildren)
                html += "<span class='ant-cascader-menu-item-expand-icon'><span role='img' aria-label='right' class='anticon anticon-right'><svg viewBox='64 64 896 896' focusable='false' class='' data-icon='right' width='1em' height='1em' fill='currentColor' aria-hidden='true'><path d='M765.7 486.8L314.9 134.7A7.97 7.97 0 00302 141v77.3c0 4.9 2.3 9.6 6.1 12.6l360 281.1-360 281.1c-3.9 3-6.1 7.7-6.1 12.6V883c0 6.7 7.7 10.4 12.9 6.3l450.8-352.1a31.96 31.96 0 000-50.4z'></path></svg></span></span>";
            builder.AddContent(8, (MarkupString)html);

            builder.CloseElement();
        }
    }
}
