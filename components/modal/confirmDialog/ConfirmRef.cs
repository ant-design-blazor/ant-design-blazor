using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfirmRef : FeedbackRefWithOkCancelBase
    {
        #region internal

        internal bool IsCreateByModalService => Service != null;
        internal TaskCompletionSource<ConfirmResult> TaskCompletionSource { get; set; }

        internal ConfirmRef(ConfirmOptions config)
        {
            Config = config;
        }

        internal ConfirmRef(ConfirmOptions config, ModalService service)
        {
            Config = config;
            Service = service;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected ModalService Service { get; set; }

        /// <summary>
        /// Confirm dialog options
        /// </summary>
        public ConfirmOptions Config { get; private set; }

        #region base inheritdoc

        /// <summary>
        /// close Confirm dialog
        /// </summary>
        /// <returns></returns>
        public override async Task CloseAsync()
        {
            await (Service?.DestroyConfirmAsync(this) ?? Task.CompletedTask);
        }


        /// <summary>
        /// Open Confirm dialog
        /// </summary>
        /// <returns></returns>
        public override async Task OpenAsync()
        {
            await (Service?.OpenConfirmAsync(this) ?? Task.CompletedTask);
        }

        /// <summary>
        /// update Confirm dialog config which Visible=true
        /// </summary>
        /// <returns></returns>
        public override async Task UpdateConfigAsync()
        {
            await (Service?.UpdateConfirmAsync(this) ?? Task.CompletedTask);
        }

        /// <summary>
        /// update Confirm dialog config with a new ConfirmOptions
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task UpdateConfigAsync(ConfirmOptions config)
        {
            Config = config;
            await UpdateConfigAsync();
        }

        #endregion
    }


    /// <summary>
    /// ConfirmRef for 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class ConfirmRef<TResult> : ConfirmRef, IOkCancelRef<TResult>
    {
        internal ConfirmRef(ConfirmOptions config, ModalService service) : base(config, service)
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
            await (OnCancel?.Invoke(result) ?? Task.CompletedTask);
        }
    }
}
