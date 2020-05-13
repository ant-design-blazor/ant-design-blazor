using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;

namespace AntBlazor
{
    public class AntTreeNode
    {
        //while AntDesign react use this way
        public string Key { get; set; }

        public string Avatar { get; set; }

        public string Text { get; set; }

        public string IconType { get; set; }

        public RenderFragment<AntTreeNode> IconTemplate { get; set; }

        public RenderFragment<AntTreeNode> NodeTemplate { get; set; }

        public bool IsExpanded { get; set; }

        public bool IsSelected { get; set; }

        public bool IsChecked { get; set; }

        public bool IsDisabled { get; set; }


        public bool HasChildNodes
        {
            get
            {
                return _nodelist?.Count > 0;
            }
        }
        public int ChildNodeCount
        {
            get
            {
                return _nodelist?.Count ?? 0;
            }
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


        public Task ToggleExpandedAnimatedAsync()
        {
            return ToggleExpandedAnimatedAsync(null);
        }

        bool? _expandedAnimateState;

        async Task ToggleExpandedAnimatedAsync(AntTree tree)
        {
            if (_expandedAnimateState != null)
                return;
            _expandedAnimateState = !IsExpanded;

            if (_expandedAnimateState == false) IsExpanded = false;

            await Task.Delay(200);

            if (_expandedAnimateState == true) IsExpanded = true;

            _expandedAnimateState = null;
        }

        public void DeselectAll()
        {
            IsSelected = false;
            if (HasChildNodes)
                foreach (var subnode in _nodelist)
                    subnode?.DeselectAll();
        }

        public void SetCheckedAll(bool check)
        {
            if (IsDisabled)
                return;
            this.IsChecked = check;
            if (HasChildNodes)
            {
                foreach (var subnode in _nodelist)
                    subnode?.SetCheckedAll(check);
            }
        }

        bool? _checkstate;
        public bool? CheckedState
        {
            get
            {
                if (!HasChildNodes)
                    return IsChecked;
                return _checkstate;
            }
        }

        internal void UpdateCheckedStateRecursive()
        {
            if (!HasChildNodes)
            {
                _checkstate = IsChecked;
                return;
            }

            int checkedCount = 0;
            int uncheckedCount = 0;

            foreach (var subnode in _nodelist)
            {
                subnode.UpdateCheckedStateRecursive();

                bool? snc = subnode.CheckedState;
                if (snc == null)
                {
                    _checkstate = null;
                    return;
                }
                if (snc == true)
                {
                    if (uncheckedCount != 0)
                    {
                        _checkstate = null;
                        return;
                    }
                    checkedCount++;
                }
                if (snc == false)
                {
                    if (checkedCount != 0)
                    {
                        _checkstate = null;
                        return;
                    }
                    uncheckedCount++;
                }
            }

            Debug.Assert(checkedCount == 0 || uncheckedCount == 0);

            if (checkedCount != 0)
                _checkstate = true;
            else if (uncheckedCount != 0)
                _checkstate = false;
            else
                _checkstate = IsChecked;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:check argument", Justification = "<Ignore>")]
        internal void RenderRecursive(RenderTreeBuilder builder, AntTree ownerTree, int level, int index, int pcount)
        {
            //ant-tree-treenode ant-tree-treenode-switcher-open ant-tree-treenode-checkbox-checked ant-tree-treenode-selected
            var cssclass = "ant-tree-treenode ant-tree-treenode-switcher-" + (IsExpanded ? "open" : "close")
                + (IsChecked ? " ant-tree-treenode-checkbox-checked" : "")
                + (IsChecked ? " ant-tree-treenode-checkbox-checked" : "")
                + (IsSelected ? " ant-tree-treenode-selected" : "")
                + (IsDisabled ? " ant-tree-treenode-disabled" : "")
                ;

            builder.OpenElement(1, "div");
            builder.AddAttribute(2, "class", cssclass);

            if (level != 0)
            {
                builder.OpenElement(1, "span");
                builder.AddAttribute(2, "class", "ant-tree-indent");
                builder.AddAttribute(3, "aria-hidden", "true");
                for (int i = 0; i < level; i++)
                {
                    string unitcls = "ant-tree-indent-unit";
                    if (i + 1 == level)
                    {
                        //NOTE: while allow null node, this way is not fully correct
                        unitcls = "ant-tree-indent-unit"
                            + (index == 0 ? " ant-tree-indent-unit-start" : "")
                            + (index + 1 == pcount ? " ant-tree-indent-unit-end" : "");
                    }
                    string htmlcode = "<span class='" + unitcls + "'></span>";
                    builder.AddContent(4 + i, (MarkupString)htmlcode);
                }
                builder.CloseElement();
            }

            if (HasChildNodes)
            {
                builder.OpenElement(1, "span");
                builder.AddAttribute(2, "class", "ant-tree-switcher ant-tree-switcher_" + (IsExpanded ? "open" : "close"));
                builder.AddAttribute(3, "onclick", EventCallback.Factory.Create<MouseEventArgs>(ownerTree, async (me) =>
                 {
                     await this.ToggleExpandedAnimatedAsync(ownerTree);
                 }));
                builder.AddContent(4, (MarkupString)"<span role='img' aria-label='caret-down' class='anticon anticon-caret-down ant-tree-switcher-icon'><svg viewBox='0 0 1024 1024' focusable='false' class='' data-icon='caret-down' width='1em' height='1em' fill='currentColor' aria-hidden='true'><path d='M840.4 300H183.6c-19.7 0-30.7 20.8-18.5 35l328.4 380.8c9.4 10.9 27.5 10.9 37 0L858.9 335c12.2-14.2 1.2-35-18.5-35z'></path></svg></span>");
                builder.CloseElement();
            }
            else
            {
                builder.AddContent(3, (MarkupString)"<span class='ant-tree-switcher ant-tree-switcher-noop'></span>");
            }

            if (ownerTree.Checkable)
            {
                builder.OpenElement(1, "span");
                bool? cs = CheckedState;
                string checkcls;
                if (cs == null) checkcls = "indeterminate";
                else if (cs == true) checkcls = "checked";
                else checkcls = "unchecked";
                builder.AddAttribute(2, "class", "ant-tree-checkbox ant-tree-checkbox-" + checkcls);
                if (!IsDisabled)
                    builder.AddAttribute(3, "onclick", EventCallback.Factory.Create<MouseEventArgs>(ownerTree, async (me) =>
                    {
                        SetCheckedAll(CheckedState != true);
                        if (ownerTree.OnCheckedStateChanged.HasDelegate)
                        {
                            await ownerTree.OnCheckedStateChanged.InvokeAsync(new AntTreeEventArgs(ownerTree, this));
                        }
                    }));
                builder.AddContent(4, (MarkupString)"<span class='ant-tree-checkbox-inner'></span>");
                builder.CloseElement();
            }

            builder.OpenElement(1, "span");
            builder.AddAttribute(2, "class", "ant-tree-node-content-wrapper ant-tree-node-content-wrapper-normal" + (IsSelected ? " ant-tree-node-selected" : ""));
            if (!IsDisabled)
                builder.AddAttribute(3, "onclick", EventCallback.Factory.Create<MouseEventArgs>(ownerTree, async (me) =>
                 {
                     if (this.IsSelected)
                     {
                         this.IsSelected = false;
                     }
                     else
                     {
                         ownerTree.DeselectAll();
                         this.IsSelected = true;
                     }
                     if (ownerTree.OnNodeSelected.HasDelegate)
                     {
                         await ownerTree.OnNodeSelected.InvokeAsync(new AntTreeEventArgs(ownerTree, this));
                     }
                 }));

            if (ownerTree.ShowIcon)
            {
                builder.OpenElement(1, "span");
                builder.AddAttribute(2, "class", "ant-tree-iconEle ant-tree-icon__customize");

                var template = this.IconTemplate ?? ownerTree.IconTemplate;
                if (template != null)
                {
                    builder.AddContent(3, template(this));
                }
                else
                {
                    string iconType = this.IconType ?? ownerTree.IconType;
                    if (!string.IsNullOrEmpty(iconType))
                    {
                        builder.OpenComponent<AntIcon>(1);
                        builder.AddAttribute(2, "Type", iconType);
                        builder.CloseComponent();
                    }
                }


                builder.CloseElement();
            }

            var nodetemplate = this.NodeTemplate ?? ownerTree.NodeTemplate;
            if (nodetemplate != null)
            {
                builder.OpenElement(1, "span");
                builder.AddAttribute(2, "class", "ant-tree-title");
                builder.AddContent(3, nodetemplate(this));
                builder.CloseElement();
            }
            else
            {
                string html = "<span class='ant-tree-title'>" + System.Text.Encodings.Web.HtmlEncoder.Default.Encode(this.Text ?? string.Empty) + "</span>";
                builder.AddContent(1, (MarkupString)html);
            }

            builder.CloseElement(); //content-wrapper

            builder.CloseElement();


            if (!HasChildNodes)
                return;

            int subindex = 0;

            if (_expandedAnimateState != null)
            {
                //TODO: expand/collapse motion 
                builder.OpenElement(1, "div");
                string clspart = (_expandedAnimateState == true ? "appear" : "leave");
                builder.AddAttribute(2, "class", $"ant-tree-treenode-motion ant-motion-collapse ant-motion-collapse-{clspart} ant-motion-collapse-{clspart}-active");

                //foreach (AntTreeNode node in _nodelist)
                //    node?.RenderRecursive(builder, ownerTree, level + 1, subindex++, _nodelist.Count);

                builder.CloseElement();
            }
            else if (IsExpanded)
            {
                foreach (AntTreeNode node in _nodelist)
                    node?.RenderRecursive(builder, ownerTree, level + 1, subindex++, _nodelist.Count);
            }

        }

    }
}
