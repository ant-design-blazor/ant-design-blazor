using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    public partial class Tree : AntDomComponentBase
    {
        internal const string PREFIX_CLS = "ant-tree";

        internal void CallStateHasChanged()
        {
            StateHasChanged();
        }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        //[Parameter]
        //public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Checkable { get; set; }

        [Parameter]
        public bool CheckStrictly { get; set; } = false;

        [Parameter]
        public bool ShowIcon { get; set; }

        [Parameter]
        public bool ShowLine { get; set; }

        [Parameter]
        public string SwitcherIcon { get; set; }

        [Parameter]
        public RenderFragment<TreeNode> SwitcherIconTemplate { get; set; }

        [Parameter]
        public string IconType { get; set; }

        [Parameter]
        public EventCallback<TreeEventArgs> OnCheckedStateChanged { get; set; }

        [Parameter]
        public EventCallback<TreeEventArgs> OnNodeSelectedChanged { get; set; }

        [Parameter]
        public EventCallback<TreeEventArgs> OnNodeLoadDelay { get; set; }

        internal TreeNode _loadDelayNode;

        [Parameter]
        public RenderFragment<TreeNode> IconTemplate { get; set; }

        [Parameter]
        public RenderFragment<TreeNode> NodeTemplate { get; set; }

        private List<TreeNode> _nodelist;

        public List<TreeNode> NodeList
        {
            get
            {
                if (_nodelist == null) _nodelist = new List<TreeNode>();
                return _nodelist;
            }
        }

        [Parameter]
        public IReadOnlyCollection<TreeNode> Nodes
        {
            get
            {
                if (_nodelist != null)
                    return _nodelist;
                return Array.Empty<TreeNode>();
            }
            set
            {
                if (value == null || value.Count == 0)
                {
                    _nodelist = null;
                    return;
                }
                if (_nodelist == null) _nodelist = new List<TreeNode>();
                else if (_nodelist.Count != 0) _nodelist.Clear();
                _nodelist.AddRange(value);
            }
        }

        public void DeselectAll()
        {
            foreach (var node in Nodes)
                node?.DeselectAll();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ClassMapper.Clear()
                .Add(PREFIX_CLS)
                .If($"{PREFIX_CLS}-icon-hide", () => !ShowIcon)
                .If($"{PREFIX_CLS}-show-line", () => ShowLine)
                ;
        }

        internal string _expandedAnimateRequested;

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (_expandedAnimateRequested != null)
            {
                string id = _expandedAnimateRequested;
                _expandedAnimateRequested = null;

#pragma warning disable CA2012 // Use ValueTasks correctly
                _ = JSRuntime.InvokeVoidAsync("eval", "var div=document.getElementById('" + id + "');"
                    + @"
if(div.style.height=='0px'){
    div.style.height=div.scrollHeight+'px';
    div.classList.add('ant-motion-collapse-appear-active')
}
else{
    div.style.height=div.offsetHeight+'px';
    setTimeout(function(){
        div.style.height='0px';
        div.classList.add('ant-motion-collapse-leave-active')
    },16);
}
"
);
#pragma warning restore CA2012 // Use ValueTasks correctly
            }
        }
    }
}
