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
        internal string PrefixName { get; set; } = "ant-list-item";

        /// <summary>
        /// The extra content of list item. If itemLayout is vertical, shows the content on right, otherwise shows content on the far right.
        /// </summary>
        [Parameter]
        public RenderFragment Extra { get; set; }

        /// <summary>
        /// The actions content of list item. If itemLayout is vertical, shows the content on bottom, otherwise shows content on the far right.
        /// </summary>
        [Parameter]
        public RenderFragment[] Actions { get; set; }

        /// <summary>
        /// Main content for the item
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Callback executed when the item is clicked
        /// </summary>
        [Parameter]
        public EventCallback OnClick { get; set; }

        /// <summary>
        /// Whether to use flex for item or not. When true, will not use flex.
        /// </summary>
        [Parameter]
        public bool NoFlex { get; set; }

        [CascadingParameter]
        private IAntList AntList { get; set; }

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        internal bool IsVerticalAndExtra => AntList?.ItemLayout == ListItemLayout.Vertical && this.Extra != null;

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
