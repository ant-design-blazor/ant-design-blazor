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

        internal string GetWrapClassNameExtended(Modal modal = null)
        {
            var classNameArray = new List<string>();
            if (modal == null)
            {
                classNameArray.AddIf(
                        !string.IsNullOrWhiteSpace(WrapClassName),
                        WrapClassName)
                    .AddIf(Centered, $"{PrefixCls}-centered")
                    .AddIf(Rtl, $"{PrefixCls}-wrap-rtl");

                return string.Join(' ', classNameArray);
            }

            classNameArray.AddIf(
                    !string.IsNullOrWhiteSpace(modal.WrapClassName),
                    modal.WrapClassName)
                .AddIf(modal.Centered, $"{PrefixCls}-centered")
                .AddIf(modal.Rtl, $"{PrefixCls}-wrap-rtl");

            return string.Join(' ', classNameArray);
        }

        #endregion
    }
}
