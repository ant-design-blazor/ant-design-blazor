using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntBlazor
{
    public class AntTabPane : AntComponentBase
    {
        /// <summary>
        /// Forced render of content in tabs, not lazy render after clicking on tabs
        /// </summary>
        [Parameter]
        public bool ForceRender { get; set; } = false;

        /// <summary>
        /// TabPane's key
        /// </summary>
        [Parameter]
        public string Key { get; set; }

        /// <summary>
        /// Show text in <see cref="AntTabPane"/>'s head
        /// </summary>
        [Parameter]
        public RenderFragment Tab { get; set; }
    }
}