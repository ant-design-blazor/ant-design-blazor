// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class MentionsOption
    {
        [CascadingParameter] public Mentions Mentions { get; set; }
        [Parameter] public string Value { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        public bool Active => Mentions?.ActiveOptionValue == Value;

        protected override async Task OnInitializedAsync()
        {
            Mentions?.AddOption(this);
            SetClassMap();
            await base.OnInitializedAsync();
        }

        private void SetClassMap()
        {
            var prefixCls = "ant-mentions-dropdown-menu-item";
            ClassMapper.Clear().Add(prefixCls).If("ant-mentions-dropdown-menu-item-active", () => Active);
            InvokeStateHasChanged();
        }

        internal void OnMouseOver()
        {
            Mentions.ActiveOptionValue = Value;
        }

        internal async Task OnClick(MouseEventArgs args)
        {
            if (args.Button == 0)   //left click
            {
                await Mentions.ItemClick(Value);
            }
        }
    }
}
