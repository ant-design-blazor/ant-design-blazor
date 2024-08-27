// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    /// <summary>
    /// config the confirm button's properties
    /// </summary>
    public class ConfirmButtonOptions
    {
        /// <summary>
        /// the leftmost button properties in LTR layout
        /// </summary>
        public ButtonProps Button1Props { get; set; }

        /// <summary>
        /// the secondary button properties in LTR layout
        /// </summary>
        public ButtonProps Button2Props { get; set; }

        /// <summary>
        /// the third button properties in LTR layout
        /// </summary>
        public ButtonProps Button3Props { get; set; }
    }
}
