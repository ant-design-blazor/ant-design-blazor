// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
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

        public bool Loading { get; set; } = false;

        public ButtonType Type { get; set; } = ButtonType.Default;

        public ButtonShape Shape { get; set; } = ButtonShape.Rectangle;

        public ButtonSize Size { get; set; } = ButtonSize.Default;

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
