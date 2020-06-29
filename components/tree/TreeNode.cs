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

        internal long GetNodeId() => _nodeId;

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

        internal bool? _expandedAnimateState;

        internal async Task ToggleExpandedAnimatedAsync()
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

    }

}
