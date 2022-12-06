// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class CardAction : AntDomComponentBase
    {
        [CascadingParameter]
        private Card Card { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            Card?.AddCardAction(this);

            base.OnInitialized();
        }
    }
}
