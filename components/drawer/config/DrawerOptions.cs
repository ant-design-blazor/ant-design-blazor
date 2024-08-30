// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public class DrawerOptions
    {
        /// <summary>
        /// The drawer body content.
        /// </summary>
        public OneOf<RenderFragment, string> Content { get; set; }

        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Whether a close (x) button is visible on top right of the Drawer dialog or not.	
        /// </summary>
        /// <default value="true" />
        public bool Closable { get; set; } = true;

        /// <summary>
        /// Clicking on the mask (area outside the Drawer) to close the Drawer or not.	
        /// </summary>
        /// <default value="true" />
        public bool MaskClosable { get; set; } = true;

        /// <summary>
        /// Whether to show mask or not.
        /// </summary>
        /// <default value="true" />
        public bool Mask { get; set; } = true;

        /// <summary>
        /// Whether to support keyboard esc off	
        /// </summary>
        /// <default value="true" />
        public bool Keyboard { get; set; } = true;

        /// <summary>
        /// The title for Drawer.
        /// </summary>
        public OneOf<RenderFragment, string> Title { get; set; }

        /// <summary>
        /// The placement of the Drawer - Possible values: "left", "right", "top", "bottom"
        /// </summary>
        /// <default value="right" />
        public string Placement { get; set; } = "right";

        /// <summary>
        /// Style for Drawer's mask element.
        /// </summary>
        public string MaskStyle { get; set; }

        /// <summary>
        /// Body style for Drawer body element. Such as height, padding etc.
        /// </summary>
        public string BodyStyle { get; set; }

        /// <summary>
        /// Header style for Drawer header element. Such as height, padding etc.
        /// </summary>
        public string HeaderStyle { get; set; }

        /// <summary>
        /// The class name of the container of the Drawer dialog.
        /// </summary>
        public string WrapClassName { get; set; }

        /// <summary>
        /// Width of the Drawer dialog.
        /// </summary>
        /// <default value="256" />
        public string Width { get; set; } = "256";

        /// <summary>
        /// Height of the Drawer dialog, only when placement is 'top' or 'bottom'.
        /// </summary>
        /// <default value="256" />
        public string Height { get; set; } = "256";

        /// <summary>
        /// The z-index of the Drawer.
        /// </summary>
        /// <default value="1000" />
        public int ZIndex { get; set; } = 1000;

        /// <summary>
        /// The the X coordinate offset(px), only when placement is 'left' or 'right'.
        /// </summary>
        /// <default value="0" />
        public int OffsetX { get; set; } = 0;

        /// <summary>
        /// The the Y coordinate offset(px), only when placement is 'top' or 'bottom'.
        /// </summary>
        /// <default value="0" />
        public int OffsetY { get; set; } = 0;

        /// <summary>
        /// If the drawer is visible or not
        /// </summary>
        /// <default value="false" />
        public bool Visible { get; set; }
    }
}
