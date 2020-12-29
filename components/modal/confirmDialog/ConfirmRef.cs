using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfirmRef
    {
        #region internal

        internal IModalTemplate ModalTemplate { get; set; }
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
        internal Confirm Confirm { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected ModalService Service { get; set; }

        /// <summary>
        /// Confirm dialog options
        /// </summary>
        public ConfirmOptions Config { get; private set; }

        /// <summary>
        /// on Confirm open
        /// </summary>
        public Func<Task> OnOpen { get; set; }

        /// <summary>
        /// on Confirm close
        /// </summary>
        public Func<Task> OnClose { get; set; }

        /// <summary>
        /// open Confirm dialog
        /// </summary>
        /// <returns></returns>
        public async Task OpenAsync()
        {
            await (Service?.OpenConfirmAsync(this) ?? Task.CompletedTask);
        }

        /// <summary>
        /// close Confirm dialog
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync()
        {
            await (Service?.DestroyConfirmAsync(this) ?? Task.CompletedTask);
        }

        /// <summary>
        /// update Confirm dialog config which Visible=true
        /// </summary>
        /// <returns></returns>
        public async Task UpdateConfigAsync()
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
    }

    /// <summary>
    /// ConfirmRef for 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class ConfirmRef<TResult> : ConfirmRef
    {
        /// <summary>
        /// on Cancel button click
        /// </summary>
        public Func<TResult, Task> OnCancel { get; set; }

        /// <summary>
        /// on OK button click
        /// </summary>
        public Func<TResult, Task> OnOk { get; set; }

        internal ConfirmRef(ConfirmOptions config, ModalService service) : base(config, service)
        {

        }

        /// <summary>
        /// Trigger OK button
        /// </summary>
        /// <returns></returns>
        public async Task TriggerOkAsync(TResult result)
        {
            await base.CloseAsync();
            await (OnOk?.Invoke(result) ?? Task.CompletedTask);
        }

        /// <summary>
        /// Trigger cancel button
        /// </summary>
        /// <returns></returns>
        public async Task TriggerCancelAsync(TResult result)
        {
            await base.CloseAsync();
            await (OnCancel?.Invoke(result) ?? Task.CompletedTask);
        }
    }
}
