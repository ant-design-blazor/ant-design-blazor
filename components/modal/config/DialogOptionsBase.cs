// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AntDesign.Internal.ModalDialog;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// ModalOptions, ConfirmOptions and DialogOptions base class
    /// </summary>
    public class DialogOptionsBase
    {
        #region static and const

        /// <summary>
        /// default Dialog close icon
        /// </summary>
        internal static readonly RenderFragment DefaultMaximizeIcon = (builder) =>
        {
            builder.OpenComponent<Icon>(0);
            builder.AddAttribute(1, "Type", "fullscreen");
            builder.AddAttribute(2, "Theme", "outline");
            builder.CloseComponent();
        };

        internal static readonly RenderFragment DefaultRestoreIcon = (builder) =>
        {
            builder.OpenComponent<Icon>(0);
            builder.AddAttribute(1, "Type", "fullscreen-exit");
            builder.AddAttribute(2, "Theme", "outline");
            builder.CloseComponent();
        };

        /// <summary>
        /// default Dialog close icon
        /// </summary>
        internal static readonly RenderFragment DefaultCloseIcon = (builder) =>
        {
            builder.OpenComponent<Icon>(0);
            builder.AddAttribute(1, "Type", "close");
            builder.AddAttribute(2, "Theme", "outline");
            builder.CloseComponent();
        };

        /// <summary>
        /// default modal header
        /// </summary>

        internal static readonly RenderFragment DefaultHeader = (builder) =>
        {
            builder.OpenComponent<ModalHeader>(0);
            builder.CloseComponent();
        };

        /// <summary>
        /// default modal footer
        /// </summary>
        internal static readonly RenderFragment DefaultFooter = (builder) =>
        {
            builder.OpenComponent<ModalFooter>(0);
            builder.CloseComponent();
        };

        #endregion static and const

        /// <summary>
        /// class name prefix
        /// </summary>
        public string PrefixCls { get; set; } = "ant-modal";

        /// <summary>
        /// Cancel Button's props
        /// </summary>
        public ButtonProps CancelButtonProps { get; set; }

        /// <summary>
        /// modal default footer cancel text
        /// </summary>
        public OneOf<string, RenderFragment> CancelText { get; set; } = "Cancel";

        /// <summary>
        /// whether center display
        /// </summary>
        public bool Centered { get; set; }

        /// <summary>
        /// get or set the modal parent DOM
        /// </summary>
        public ElementReference? GetContainer { get; set; } = null;

        /// <summary>
        /// Whether support press esc to close
        /// </summary>
        public bool Keyboard { get; set; } = true;

        /// <summary>
        /// Whether show mask or not
        /// </summary>
        public bool Mask { get; set; } = true;

        /// <summary>
        /// Whether to close the modal dialog when the mask (area outside the modal) is clicked
        /// </summary>
        public bool MaskClosable { get; set; }

        /// <summary>
        /// Style for dialog's mask element
        /// </summary>
        public string MaskStyle { get; set; }

        /// <summary>
        /// Ok Button's props
        /// </summary>
        public ButtonProps OkButtonProps { get; set; }

        /// <summary>
        /// Text of the OK button
        /// </summary>
        public OneOf<string, RenderFragment> OkText { get; set; } = "OK";

        /// <summary>
        /// Button type of the OK button
        /// </summary>
        public string OkType { get; set; } = ButtonType.Primary;

        /// <summary>
        /// The modal dialog's title of String
        /// </summary>
        public string Title { get; set; } = null;

        /// <summary>
        /// The modal dialog's title of RenderFragment
        /// </summary>
        public RenderFragment TitleTemplate { get; set; } = null;

        /// <summary>
        /// Width of the modal dialog
        /// </summary>
        public OneOf<string, double> Width { get; set; }

        /// <summary>
        /// The z-index of the Modal
        /// </summary>
        public int ZIndex { get; set; } = 1000;

        /// <summary>
        /// Is RTL
        /// </summary>
        public bool Rtl { get; set; } = false;

        internal bool CreateByService { get; set; }
    }
}
