using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// the options of Modal dialog box
    /// </summary>
    public class ModalOptions : DialogOptionsBase
    {
        public ModalOptions()
        {
            _onCancel = DefaultOnCancelOrOk;
            _onOk = DefaultOnCancelOrOk;
            Width = 520;
            MaskClosable = true;
        }

        internal ModalRef ModalRef;

        /// <summary>
        /// trigger after Dialog is closed
        /// </summary>
        public Func<Task> AfterClose { get; set; } = () => Task.CompletedTask;

        /// <summary>
        /// ant-modal style
        /// </summary>
        public string Style { get; set; }

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
        public RenderFragment CloseIcon { get; set; } = DialogOptions.DefaultCloseIcon;

        /// <summary>
        /// Whether to apply loading visual effect for OK button or not
        /// </summary>
        public bool ConfirmLoading
        {
            get
            {
                if (OkButtonProps != null)
                {
                    return OkButtonProps.Loading;
                }
                return false;
            }
            set
            {
                if (OkButtonProps == null)
                {
                    OkButtonProps = new ButtonProps();
                }

                OkButtonProps.Loading = true;
            }
        }

        /// <summary>
        /// Whether to remove Modal from DOM after the Modal closed
        /// </summary>
        public bool DestroyOnClose { get; set; }

        /// <summary>
        /// Modal footer. If Footer==null, the dialog will not have a footer
        /// </summary>
        public OneOf<string, RenderFragment>? Footer { get; set; } = DialogOptions.DefaultFooter;

        /// <summary>
        /// 
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// The class name of the container of the modal dialog	
        /// </summary>
        public string WrapClassName { get; set; }

        private Func<MouseEventArgs, Task> _onCancel;

        /// <summary>
        /// Specify a function that will be called when a user clicks mask, close button on top right or Cancel button.
        /// </summary>
        public Func<MouseEventArgs, Task> OnCancel { get => _onCancel; set => _onCancel = value; }

        private Func<MouseEventArgs, Task> _onOk;

        /// <summary>
        /// Specify a function that will be called when a user clicks the OK button
        /// </summary>
        public Func<MouseEventArgs, Task> OnOk { get => _onOk; set => _onOk = value; }

        /// <summary>
        /// ChildContent
        /// </summary>
        public RenderFragment Content { get; set; } = null;

        #region internal

        internal async Task DefaultOnCancelOrOk(MouseEventArgs e)
        {
            await (ModalRef?.CloseAsync() ?? Task.CompletedTask);
        }

        #endregion
    }
}
