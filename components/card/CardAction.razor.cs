using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class CardAction : AntDomComponentBase
    {
        [CascadingParameter] private Card Card { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            Card?.AddCardAction(this);

            base.OnInitialized();
        }
    }
}
