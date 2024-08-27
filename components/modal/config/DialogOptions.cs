// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// dialog options
    /// </summary>
    public class DialogOptions : DialogOptionsBase
    {
        /// <summary>
        /// trigger after Dialog is closed
        /// </summary>
        public Func<Task> OnClosed { get; set; }

        /// <summary>
        /// ant-modal-body style
        /// </summary>
        public string BodyStyle { get; set; }

        /// <summary>
        /// show ant-modal-closer
        /// </summary>
        public bool Closable { get; set; } = true;

        /// <summary>
        /// Draggable modal
        /// </summary>
        public bool Draggable { get; set; }

        /// <summary>
        /// Drag and drop only within the Viewport
        /// </summary>
        public bool DragInViewport { get; set; } = true;

        /// <summary>
        /// closer icon RenderFragment, the default is a "X"
        /// </summary>
        public RenderFragment? CloseIcon { get; set; } = DefaultCloseIcon;

        /// <summary>
        /// Whether to apply loading visual effect for OK button or not
        /// </summary>
        public bool ConfirmLoading { get; set; }

        /// <summary>
        /// modal header
        /// </summary>
        public RenderFragment Header { get; set; } = DefaultHeader;

        /// <summary>
        /// modal footer
        /// </summary>
        public OneOf<string, RenderFragment>? Footer { get; set; } = DefaultFooter;

        /// <summary>
        /// The class name of the container of the modal dialog
        /// </summary>
        public string WrapClassName { get; set; }

        /// <summary>
        /// ChildContent
        /// </summary>
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// the class name of the element of ".ant-modal"
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// for OK-Cancel Confirm dialog, cancel button clicked callback.
        /// It's only trigger in Confirm created by ModalService mode
        /// </summary>
        public Func<MouseEventArgs, Task> OnCancel { get; set; }

        /// <summary>
        /// for OK-Cancel Confirm dialog, OK button clicked callback.
        /// It's only trigger in Confirm created by ModalService mode
        /// </summary>
        public Func<MouseEventArgs, Task> OnOk { get; set; }

        /// <summary>
        /// max modal body content height
        /// </summary>
        public string MaxBodyHeight { get; set; } = null;

        /// <summary>
        /// show modal maximize button
        /// </summary>
        public bool Maximizable { get; set; } = false;

        /// <summary>
        /// The icon of the maximize button when the modal is in normal state
        /// </summary>
        public RenderFragment MaximizeBtnIcon { get; set; } = DefaultMaximizeIcon;

        /// <summary>
        /// The icon of the maximize button when the modal is maximized
        /// </summary>
        public RenderFragment RestoreBtnIcon { get; set; } = DefaultRestoreIcon;

        /// <summary>
        /// Maximize the dialog during component initialization, and it will ignore the Maximizable value.
        /// </summary>
        public bool DefaultMaximized { get; set; } = false;

        /// <summary>
        /// Resizable (Horizontal direction only)
        /// </summary>
        public bool Resizable { get; set; }

        /// <summary>
        /// Whether to remove Modal from DOM after the Modal closed
        /// </summary>
        public bool DestroyOnClose { get; set; }

        /// <summary>
        /// Whether to force render the Modal dom before opening.   
        /// </summary>
        public bool ForceRender { get; set; }

        #region internal

        internal string GetHeaderStyle()
        {
            if (Draggable)
            {
                return "cursor: move;";
            }
            return "";
        }

        internal string GetWidth()
        {
            if (Width.IsT0)
            {
                return $"width:{Width.AsT0};";
            }
            else
            {
                return $"width:{Width.AsT1}px;";
            }
        }

        internal string GetWrapClassNameExtended(ModalStatus status)
        {
            var classNameArray = new List<string>();

            classNameArray.AddIf(
                    !string.IsNullOrWhiteSpace(WrapClassName),
                    WrapClassName)
                .AddIf(Centered && status != ModalStatus.Max, $"{PrefixCls}-centered")
                .AddIf(Rtl, $"{PrefixCls}-wrap-rtl");

            return string.Join(' ', classNameArray);
        }

        #endregion internal
    }
}
