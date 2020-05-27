using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace AntBlazor
{
    public partial class AntTree : AntDomComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Checkable { get; set; }

        [Parameter]
        public bool CascadingCheckState { get; set; } = true;

        [Parameter]
        public bool ShowIcon { get; set; }

        [Parameter]
        public string IconType { get; set; }

        [Parameter]
        public EventCallback<AntTreeEventArgs> OnCheckedStateChanged { get; set; }

        [Parameter]
        public EventCallback<AntTreeEventArgs> OnItemSelected { get; set; }

        [Parameter]
        public RenderFragment<AntTreeNode> NodeIconTemplate { get; set; }

        [Parameter]
        public RenderFragment<AntTreeNode> NodeTemplate { get; set; }

        [Parameter]
        public RenderFragment<AntTreeDataItem> ItemIconTemplate { get; set; }

        [Parameter]
        public RenderFragment<AntTreeDataItem> ItemTemplate { get; set; }

        protected override void OnInitialized()
        {
            ClassMapper.Clear()
                .Add("ant-tree");

            base.OnInitialized();
        }

        List<AntTreeDataItem> _itemlist;
        public List<AntTreeDataItem> ItemList
        {
            get
            {
                if (_itemlist == null) _itemlist = new List<AntTreeDataItem>();
                return _itemlist;
            }
        }

        [Parameter]
        public IReadOnlyCollection<AntTreeDataItem> Items
        {
            get
            {
                if (_itemlist != null)
                    return _itemlist;
                return Array.Empty<AntTreeDataItem>();
            }
            set
            {
                if (value == null || value.Count == 0)
                {
                    _itemlist = null;
                    return;
                }
                if (_itemlist == null) _itemlist = new List<AntTreeDataItem>();
                else if (_itemlist.Count != 0) _itemlist.Clear();
                _itemlist.AddRange(value);
            }
        }

        public void DeselectAll()
        {
            foreach (var item in Items)
                item?.DeselectAll();
        }

        internal AntTreeNode _selectedNode;
        internal List<AntTreeNode> _renderChildren;

        bool _scriptSent = false;

        void BeginRender()
        {
            _renderChildren = null;

            if (CascadingCheckState && !_scriptSent)
            {
                _scriptSent = true;
                JSRuntime.InvokeVoidAsync("eval", @"
window.set_check_class=function(e,c){e.className=c}
");
            }

        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (CascadingCheckState && _renderChildren != null)
            {
                //string json = JsonSerializer.Serialize(DebugNode.ToNodes(_renderChildren));
                JSRuntime.InvokeVoidAsync("console.warn", DebugNode.ToNodes(_renderChildren));
            }
        }

        [Serializable]
        public class DebugNode
        {
            static public DebugNode[] ToNodes(List<AntTreeNode> list)
            {
                return list.Select(v =>
                {
                    DebugNode dn = new DebugNode();
                    dn.Name = v.Text;
                    if (v._renderChildren != null)
                        dn.Nodes = ToNodes(v._renderChildren);
                    return dn;
                }).ToArray();
            }

            public string Name { get; set; }
            public DebugNode[] Nodes { get; set; }
        }

    }


}
