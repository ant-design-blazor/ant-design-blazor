using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    public class DrawerConfig
    {
        public OneOf<RenderFragment, string> Content { get; set; }

        public RenderFragment ChildContent { get; set; }

        public bool Closable { get; set; } = true;

        public bool MaskClosable { get; set; } = true;

        public bool Mask { get; set; } = true;

        public bool NoAnimation { get; set; } = false;

        public bool Keyboard { get; set; } = true;

        public OneOf<RenderFragment, string> Title { get; set; }

        /// <summary>
        /// "left" | "right" | "top" | "bottom"
        /// </summary>
        public string Placement { get; set; } = "right";

        public string MaskStyle { get; set; }

        public string BodyStyle { get; set; }

        public string WrapClassName { get; set; }

        public int Width { get; set; } = 256;

        public int Height { get; set; } = 256;

        public int ZIndex { get; set; } = 1000;

        public int OffsetX { get; set; } = 0;

        public int OffsetY { get; set; } = 0;

        public bool Visible { get; set; }

        public EventCallback OnViewInit { get; set; }

        public EventCallback OnClose { get; set; }

        internal DrawerService DrawerService { get; set; }

        internal async Task HandleOnClose()
        {
            await DrawerService.CloseAsync(this);
            if (OnClose.HasDelegate)
            {
                await OnClose.InvokeAsync(null);
            }
        }

    }
}
