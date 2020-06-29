using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class TreeNode
    {
        const string PREFIX_CLS = Tree.PREFIX_CLS;

        static long _nextNodeId;

        long _nodeId;

        public TreeNode()
        {
            _nodeId = Interlocked.Increment(ref _nextNodeId);
        }

        public string Key { get; set; }

        public string Avatar { get; set; }

        public string Text { get; set; }

        public string SwitcherIcon { get; set; }

        public RenderFragment<TreeNode> SwitcherIconTemplate { get; set; }

        public string IconType { get; set; }

        public RenderFragment<TreeNode> IconTemplate { get; set; }

        public RenderFragment<TreeNode> NodeTemplate { get; set; }

        public bool IsExpanded { get; set; }

        public bool IsSelected { get; set; }

        public bool IsChecked { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsVisible { get; set; } = true;

        public bool LoadDelay { get; set; }


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

        List<TreeNode> _nodelist;
        public List<TreeNode> Nodes
        {
            get
            {
                if (_nodelist == null) _nodelist = new List<TreeNode>();
                return _nodelist;
            }
        }

        bool? _expandedAnimateState;

        async Task ToggleExpandedAnimatedAsync()
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

        internal void UpdateCheckedStateRecursive(bool checkStrictly)
        {
            if (!HasChildNodes)
            {
                _checkstate = IsChecked;
                return;
            }

            if (checkStrictly || IsDisabled)
            {
                _checkstate = IsChecked;
                foreach (var subnode in _nodelist)
                    subnode?.UpdateCheckedStateRecursive(checkStrictly);
                return;
            }

            int checkedCount = 0;
            int uncheckedCount = 0;
            int nullCount = 0;

            foreach (var subnode in _nodelist)
            {
                if (subnode == null)
                    continue;

                subnode.UpdateCheckedStateRecursive(checkStrictly);

                if (subnode.IsDisabled)
                    continue;

                bool? snc = subnode.CheckedState;
                if (snc == null)
                {
                    nullCount++;
                }
                else if (snc == true)
                {
                    checkedCount++;
                }
                else //if (snc == false)
                {
                    uncheckedCount++;
                }
            }

            if (nullCount == 0 && checkedCount == 0 && uncheckedCount == 0)
            {
                _checkstate = IsChecked;
                return;
            }

            if (nullCount != 0)
                _checkstate = null;
            else if (checkedCount != 0 && uncheckedCount != 0)
                _checkstate = null;
            else if (checkedCount != 0)
                _checkstate = true;
            else    // if (uncheckedCount != 0)
                _checkstate = false;

            IsChecked = _checkstate == true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:check argument", Justification = "<Ignore>")]
        internal RenderFragment RenderRecursive(Tree ownerTree, Stack<string> pstack)
        {
            return (builder) =>
            {
                RenderRecursive(builder, ownerTree, pstack);
            };
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:check argument", Justification = "<Ignore>")]
        internal void RenderRecursive(RenderTreeBuilder builder, Tree ownerTree, Stack<string> pstack)
        {
            builder.OpenElement(1, "div");

            builder.SetKey(_nodeId);//Important

            bool expanded = _expandedAnimateState.GetValueOrDefault(IsExpanded);

            var cssclass = $"{PREFIX_CLS}-treenode {PREFIX_CLS}-treenode-switcher-" + (expanded ? "open" : "close")
                + (IsChecked ? $" {PREFIX_CLS}-treenode-checkbox-checked" : "")
                + (IsSelected ? $" {PREFIX_CLS}-treenode-selected" : "")
                + (IsDisabled ? $" {PREFIX_CLS}-treenode-disabled" : "")
                ;


            builder.AddAttribute(2, "class", cssclass);

            if (pstack.Count != 0)
            {
                builder.OpenElement(1, "span");
                builder.AddAttribute(2, "class", $"{PREFIX_CLS}-indent");
                builder.AddAttribute(3, "aria-hidden", "true");
                int pindex = 0;
                foreach (string ptype in pstack.Reverse())
                {
                    string htmlcode = "<span class='" + ptype + "'></span>";

                    builder.AddContent(4 + pindex, (MarkupString)htmlcode);
                    pindex++;
                }
                builder.CloseElement();
            }

            if (HasChildNodes || LoadDelay)
            {
                builder.OpenElement(1, "span");
                builder.AddAttribute(2, "class", $"{PREFIX_CLS}-switcher {PREFIX_CLS}-switcher_" + (expanded ? "open" : "close"));


                if (!HasChildNodes && ownerTree._loadDelayNode == this)
                {
                    builder.OpenComponent<Icon>(4);
                    builder.AddAttribute(5, "Class", $"{PREFIX_CLS}-switcher-loading-icon");
                    builder.AddAttribute(6, "Type", "loading");
                    builder.CloseComponent();
                }
                else
                {
                    if (!HasChildNodes && LoadDelay)
                    {
                        builder.AddAttribute(3, "onclick", EventCallback.Factory.Create<MouseEventArgs>(ownerTree, async (me) =>
                        {
                            ownerTree._loadDelayNode = this;
                            await ownerTree.OnNodeLoadDelay.InvokeAsync(new TreeEventArgs(ownerTree, this));
                            ownerTree._loadDelayNode = null;
                            if (HasChildNodes && !IsExpanded)
                            {
                                //ownerTree.CallStateHasChanged();
                                await this.ToggleExpandedAnimatedAsync();
                            }
                        }));
                    }
                    else
                    {
                        builder.AddAttribute(3, "onclick", EventCallback.Factory.Create<MouseEventArgs>(ownerTree, async (me) =>
                        {
                            await this.ToggleExpandedAnimatedAsync();
                        }));
                    }

                    RenderFragment<TreeNode> sit = this.SwitcherIconTemplate ?? ownerTree.SwitcherIconTemplate;
                    if (sit != null)
                    {
                        builder.AddContent(4, sit(this));
                    }
                    else
                    {
                        string si = this.SwitcherIcon ?? ownerTree.SwitcherIcon;

                        //always use <Icon/>
                        //if (!string.IsNullOrEmpty(si))si = "caret-down";

                        if (!string.IsNullOrEmpty(si))
                        {
                            builder.OpenComponent<Icon>(4);
                            builder.AddAttribute(5, "Class", $"{PREFIX_CLS}-switcher-icon");
                            builder.AddAttribute(6, "Type", si);
                            builder.CloseComponent();
                        }
                        else
                        {
                            //static html , faster
                            if (!ownerTree.ShowLine)
                            {
                                builder.AddContent(4, (MarkupString)$"<span role='img' aria-label='caret-down' class='anticon anticon-caret-down {PREFIX_CLS}-switcher-icon'><svg viewBox='0 0 1024 1024' focusable='false' class='' data-icon='caret-down' width='1em' height='1em' fill='currentColor' aria-hidden='true'><path d='M840.4 300H183.6c-19.7 0-30.7 20.8-18.5 35l328.4 380.8c9.4 10.9 27.5 10.9 37 0L858.9 335c12.2-14.2 1.2-35-18.5-35z'></path></svg></span>");
                            }
                            else if (IsExpanded)
                            {
                                builder.AddContent(4, (MarkupString)$"<span role='img' aria-label='minus-square' class='anticon anticon-minus-square {PREFIX_CLS}-switcher-line-icon'><svg viewBox='64 64 896 896' focusable='false' class='' data-icon='minus-square' width='1em' height='1em' fill='currentColor' aria-hidden='true'><path d='M328 544h368c4.4 0 8-3.6 8-8v-48c0-4.4-3.6-8-8-8H328c-4.4 0-8 3.6-8 8v48c0 4.4 3.6 8 8 8z'></path><path d='M880 112H144c-17.7 0-32 14.3-32 32v736c0 17.7 14.3 32 32 32h736c17.7 0 32-14.3 32-32V144c0-17.7-14.3-32-32-32zm-40 728H184V184h656v656z'></path></svg></span>");
                            }
                            else
                            {
                                builder.AddContent(4, (MarkupString)$"<span role='img' aria-label='plus-square' class='anticon anticon-plus-square {PREFIX_CLS}-switcher-line-icon'><svg viewBox='64 64 896 896' focusable='false' class='' data-icon='plus-square' width='1em' height='1em' fill='currentColor' aria-hidden='true'><path d='M328 544h152v152c0 4.4 3.6 8 8 8h48c4.4 0 8-3.6 8-8V544h152c4.4 0 8-3.6 8-8v-48c0-4.4-3.6-8-8-8H544V328c0-4.4-3.6-8-8-8h-48c-4.4 0-8 3.6-8 8v152H328c-4.4 0-8 3.6-8 8v48c0 4.4 3.6 8 8 8z'></path><path d='M880 112H144c-17.7 0-32 14.3-32 32v736c0 17.7 14.3 32 32 32h736c17.7 0 32-14.3 32-32V144c0-17.7-14.3-32-32-32zm-40 728H184V184h656v656z'></path></svg></span>");
                            }
                        }
                    }

                }

                builder.CloseElement();
            }
            else if (LoadDelay)
            {
                builder.OpenElement(1, "span");
                builder.AddAttribute(2, "class", $"{PREFIX_CLS}-switcher {PREFIX_CLS}-switcher_" + (expanded ? "open" : "close"));


                builder.CloseElement();
            }
            else
            {
                if (ownerTree.ShowLine)
                {
                    builder.AddContent(3, (MarkupString)$"<span class='{PREFIX_CLS}-switcher {PREFIX_CLS}-switcher-noop'><span role='img' aria-label='file' class='anticon anticon-file {PREFIX_CLS}-switcher-line-icon'><svg viewBox='64 64 896 896' focusable='false' class='' data-icon='file' width='1em' height='1em' fill='currentColor' aria-hidden='true'><path d='M854.6 288.6L639.4 73.4c-6-6-14.1-9.4-22.6-9.4H192c-17.7 0-32 14.3-32 32v832c0 17.7 14.3 32 32 32h640c17.7 0 32-14.3 32-32V311.3c0-8.5-3.4-16.7-9.4-22.7zM790.2 326H602V137.8L790.2 326zm1.8 562H232V136h302v216a42 42 0 0042 42h216v494z'></path></svg></span></span>");
                }
                else
                {
                    builder.AddContent(3, (MarkupString)$"<span class='{PREFIX_CLS}-switcher {PREFIX_CLS}-switcher-noop'></span>");
                }

            }

            if (ownerTree.Checkable)
            {
                builder.OpenElement(1, "span");

                bool? cs = CheckedState;
                string checkcls;
                if (cs == null) checkcls = "indeterminate";
                else if (cs == true) checkcls = "checked";
                else checkcls = "unchecked";

                builder.AddAttribute(2, "class", $"{PREFIX_CLS}-checkbox" + (IsDisabled ? $" {PREFIX_CLS}-checkbox-disabled" : "") + $" {PREFIX_CLS}-checkbox-" + checkcls);
                if (!IsDisabled)
                    builder.AddAttribute(3, "onclick", EventCallback.Factory.Create<MouseEventArgs>(ownerTree, async (me) =>
                    {
                        if (ownerTree.CheckStrictly)
                            IsChecked = !IsChecked;
                        else
                            SetCheckedAll(CheckedState != true);
                        if (ownerTree.OnCheckedStateChanged.HasDelegate)
                        {
                            await ownerTree.OnCheckedStateChanged.InvokeAsync(new TreeEventArgs(ownerTree, this));
                        }
                    }));
                builder.AddContent(4, (MarkupString)$"<span class='{PREFIX_CLS}-checkbox-inner'></span>");
                builder.CloseElement();
            }

            builder.OpenElement(1, "span");
            builder.AddAttribute(2, "class", $"{PREFIX_CLS}-node-content-wrapper {PREFIX_CLS}-node-content-wrapper-normal" + (IsSelected ? $" {PREFIX_CLS}-node-selected" : ""));
            if (!IsDisabled)
                builder.AddAttribute(3, "onclick", EventCallback.Factory.Create<MouseEventArgs>(ownerTree, async (me) =>
                 {
                     if (this.IsSelected)
                     {
                         this.IsSelected = false;
                     }
                     else
                     {
                         if (!me.CtrlKey)//TODO: ownerTree.MultiSelect ?
                             ownerTree.DeselectAll();
                         this.IsSelected = true;
                     }
                     if (ownerTree.OnNodeSelectedChanged.HasDelegate)
                     {
                         await ownerTree.OnNodeSelectedChanged.InvokeAsync(new TreeEventArgs(ownerTree, this));
                     }
                 }));

            if (ownerTree.ShowIcon)
            {
                builder.OpenElement(1, "span");
                builder.AddAttribute(2, "class", $"{PREFIX_CLS}-iconEle {PREFIX_CLS}-icon__customize");

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
                        builder.OpenComponent<Icon>(1);
                        builder.AddAttribute(2, "Type", iconType);
                        builder.CloseComponent();
                    }
                }


                builder.CloseElement();
            }

            var itemtemplate = this.NodeTemplate ?? ownerTree.NodeTemplate;
            if (itemtemplate != null)
            {
                builder.OpenElement(1, "span");
                builder.AddAttribute(2, "class", $"{PREFIX_CLS}-title");
                builder.AddContent(3, itemtemplate(this));
                builder.CloseElement();
            }
            else
            {
                string html = $"<span class='{PREFIX_CLS}-title'>" + System.Text.Encodings.Web.HtmlEncoder.Default.Encode(this.Text ?? string.Empty) + "</span>";
                builder.AddContent(1, (MarkupString)html);
            }

            builder.CloseElement(); //content-wrapper

            builder.CloseElement();


            if (!HasChildNodes)
                return;

            if (_expandedAnimateState != null)
            {
                //Note: bad implementation , all sub nodes will rerender , however ang-design react do this way

                builder.OpenElement(1, "div");

                string id = "_tree_animate_" + _nodeId;

                ownerTree._expandedAnimateRequested = id;

                builder.SetKey(id);

                builder.AddAttribute(3, "id", id);

                string clspart = (_expandedAnimateState == true ? "appear" : "leave");

                builder.AddAttribute(4, "class", $"{PREFIX_CLS}-treenode-motion ant-motion-collapse ant-motion-collapse-{clspart}");

                if (_expandedAnimateState == true)
                {
                    builder.AddAttribute(5, "style", "height:0px");
                }

                RenderSubNodes(builder, ownerTree, pstack);

                builder.CloseElement();
            }
            else if (IsExpanded)
            {
                RenderSubNodes(builder, ownerTree, pstack);
            }

        }

        private void RenderSubNodes(RenderTreeBuilder builder, Tree ownerTree, Stack<string> pstack)
        {
            List<TreeNode> list = _nodelist;
            if (!_nodelist.All(v => v != null && v.IsVisible))
            {
                list = _nodelist.Where(v => v != null && v.IsVisible).ToList();
                if (list.Count == 0)
                    return;
            }

            TreeNode last = list[list.Count - 1];

            foreach (TreeNode node in list)
            {
                if (node == last)
                    pstack.Push($"{PREFIX_CLS}-indent-unit {PREFIX_CLS}-indent-unit-end");
                else if (node == list[0])
                    pstack.Push($"{PREFIX_CLS}-indent-unit {PREFIX_CLS}-indent-unit-start");
                else
                    pstack.Push($"{PREFIX_CLS}-indent-unit");
                node?.RenderRecursive(builder, ownerTree, pstack);
                pstack.Pop();
            }
        }
    }
}
