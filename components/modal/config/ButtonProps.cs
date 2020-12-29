using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// button props
    /// </summary>
    public class ButtonProps
    {
        public bool Block { get; set; } = false;

        public bool Ghost { get; set; } = false;

        public bool Search { get; set; } = false;

        public bool Loading { get; set; } = false;

        public string Type { get; set; } = ButtonType.Default;

        public string Shape { get; set; } = null;

        public string Size { get; set; } = AntSizeLDSType.Default;

        public string Icon { get; set; }

        public bool Disabled { get; set; }

        private bool? _danger;
        public bool? Danger { get => _danger; set => _danger = value; }

        internal bool IsDanger
        {
            get
            {
                if (Danger.HasValue) return Danger.Value;
                return false;
            }
        }

        public OneOf<string, RenderFragment>? ChildContent { get; set; } = null;
    }
}
