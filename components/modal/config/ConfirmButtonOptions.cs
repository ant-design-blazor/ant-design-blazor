using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using OneOf;

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
