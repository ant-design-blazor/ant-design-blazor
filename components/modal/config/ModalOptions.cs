using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// The options of Modal dialog box
    /// </summary>
    public class ModalOptions : DialogOptions
    {
        public ModalOptions()
        {
            _onCancel = DefaultOnCancelOrOk;
            _onOk = DefaultOnCancelOrOk;
            Width = 520;
            MaskClosable = true;
        }

        internal ModalRef ModalRef { get; set; }

        /// <summary>
        /// ant-modal style
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Visible { get; set; } = true;


        private Func<MouseEventArgs, Task> _onCancel;

        /// <summary>
        /// Specify a function that will be called when a user clicks mask, close button on top right or Cancel button.
        /// </summary>
        public override Func<MouseEventArgs, Task> OnCancel { get => _onCancel; set => _onCancel = value; }

        private Func<MouseEventArgs, Task> _onOk;

        /// <summary>
        /// Specify a function that will be called when a user clicks the OK button
        /// </summary>
        public override Func<MouseEventArgs, Task> OnOk { get => _onOk; set => _onOk = value; }


        #region internal

        public async Task DefaultOnCancelOrOk(MouseEventArgs e)
        {
            await (ModalRef?.CloseAsync() ?? Task.CompletedTask);
        }

        #endregion
    }
}
