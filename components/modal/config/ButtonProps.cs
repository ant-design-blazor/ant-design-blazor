using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public class ButtonProps
    {
        public bool Block { get; set; } = false;

        public bool Ghost { get; set; } = false;

        public bool Search { get; set; } = false;

        public bool Loading { get; set; } = false;

        public string Type { get; set; } = AntButtonType.Default;

        public string Shape { get; set; } = null;

        public string Size { get; set; } = AntSizeLDSType.Default;

        public string Icon { get; set; }

        public bool Disabled { get; set; }

        public bool Danger { get; set; }
    }
}
