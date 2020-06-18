using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Anchor : AntDomComponentBase, IAnchor
    {
        public List<AnchorLink> Links { get; } = new List<AnchorLink>();

        #region Parameters

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        #endregion
    }
}
