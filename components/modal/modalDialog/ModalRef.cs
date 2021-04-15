using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    /// <summary>
    /// 
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
    }

    /// <summary>
    /// 
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
