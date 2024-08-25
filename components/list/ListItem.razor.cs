// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ListItem : AntDomComponentBase
    {
        public string PrefixName { get; set; } = "ant-list-item";

        [Parameter] public string Content { get; set; }

        [Parameter] public RenderFragment Extra { get; set; }

        [Parameter] public RenderFragment[] Actions { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public string ColStyle { get; set; }

        [Parameter] public int ItemCount { get; set; }

        [Parameter] public EventCallback OnClick { get; set; }

        [Parameter] public bool NoFlex { get; set; }

        [CascadingParameter]
        private IAntList AntList { get; set; }

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        public bool IsVerticalAndExtra => AntList?.ItemLayout == ListItemLayout.Vertical && this.Extra != null;
        private ListGridType Grid => AntList.Grid;

        protected override async Task OnInitializedAsync()
        {
            SetClassMap();
            await base.OnInitializedAsync();
        }

        protected void SetClassMap()
        {
            ClassMapper.Clear()
                .Add(PrefixName)
                .If($"{PrefixName}-no-flex", () => NoFlex);
        }

        private void HandleClick()
        {
            if (OnClick.HasDelegate)
            {
                OnClick.InvokeAsync(this);
            }
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener?.Dispose();
            base.Dispose(disposing);
        }
    }
}
