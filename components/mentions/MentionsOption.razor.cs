// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class MentionsOption
    {
        [CascadingParameter]
        private Mentions Mentions { get; set; }

        /// <summary>
        /// The value of option.
        /// </summary>
        [Parameter]
        public string Value { get; set; }

        /// <summary>
        /// The content of option.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public bool Hidden => Mentions == null || (Mentions.ShowOptions.Any() ? !Mentions.ShowOptions.Any(x => x.Value == Value) : !Mentions.OriginalOptions.Any(x => x.Value == Value));

        internal bool Active => Mentions?.ActiveOptionValue == Value;

        protected override async Task OnInitializedAsync()
        {
            Mentions?.AddOption(this);
            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClassMap();
        }

        private void SetClassMap()
        {
            var prefixCls = "ant-mentions-dropdown-menu-item";
            ClassMapper.Clear().Add(prefixCls).If("ant-mentions-dropdown-menu-item-active", () => Active);
        }

        internal void OnMouseOver()
        {
            if (Mentions != null)
            {
                Mentions.ActiveOptionValue = Value;
            }
        }

        internal async Task OnClick(MouseEventArgs args)
        {
            var isLeftClick = args.Button == 0;
            if (isLeftClick && Mentions != null)
            {
                await Mentions.ItemClick(Value);
            }
        }
    }
}
