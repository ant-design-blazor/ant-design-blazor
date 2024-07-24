using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// The reference of the modal instance
    /// </summary>
    public class ModalRef : FeedbackRefWithOkCancelBase
    {
        public ModalOptions Config { get; private set; }
        private readonly ModalService _service;

        internal ModalRef(ModalOptions config, ModalService modalService)
        {
            Config = config;
            _service = modalService;
        }

        public RenderFragment Render => Modal.GetModalRender(this);

        /// <summary>
        /// open the Modal dialog
        /// </summary>
        /// <returns></returns>
        public override async Task OpenAsync()
        {
            if (!Config.Visible)
            {
                Config.Visible = true;
            }
            await _service.CreateOrOpenModalAsync(this);
        }

        /// <summary>
        /// close the Modal dialog
        /// </summary>
        /// <returns></returns>
        public override async Task CloseAsync()
        {
            if (Config.Visible)
            {
                Config.Visible = false;
            }
            await _service.CloseModalAsync(this);
        }

        /// <summary>
        /// Update modal
        /// </summary>
        /// <returns></returns>
        public override async Task UpdateConfigAsync()
        {
            await _service.UpdateModalAsync(this);
        }

        /// <summary>
        /// Set the loading state of the confirm button
        /// </summary>
        /// <param name="loading"></param>
        public void SetConfirmLoading(bool loading)
        {
            Config.ConfirmLoading = loading;
            _service.UpdateModal(this);
        }
    }

    /// <summary>
    /// ModalRef with return value
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class ModalRef<TResult> : ModalRef, IOkCancelRef<TResult>
    {
        internal ModalRef(ModalOptions config, ModalService modalService) : base(config, modalService)
        {
        }

        /// <inheritdoc />
        public new Func<TResult, Task> OnCancel { get; set; }

        /// <inheritdoc />
        public new Func<TResult, Task> OnOk { get; set; }

        /// <inheritdoc />
        public async Task OkAsync(TResult result)
        {
            await (OnOk?.Invoke(result) ?? Task.CompletedTask);
        }

        /// <inheritdoc />
        public async Task CancelAsync(TResult result)
        {
            await (OnOk?.Invoke(result) ?? Task.CompletedTask);
        }
    }
}
