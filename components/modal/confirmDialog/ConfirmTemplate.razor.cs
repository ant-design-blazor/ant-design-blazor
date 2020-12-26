using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// ConfirmTemplate
    /// </summary>
    /// <typeparam name="TComponentOptions"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class ConfirmTemplate<TComponentOptions, TResult> : TemplateComponentBase<TComponentOptions>, IModalTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public ConfirmRef<TResult> ConfirmRef { get; set; }

        /// <summary>
        /// Emit Ok and return values
        /// </summary>
        /// <returns></returns>
        public async Task OnOkAsync(TResult result)
        {
            await (ConfirmRef.OnOk?.Invoke(result) ?? Task.CompletedTask);
        }

        /// <summary>
        /// Emit Cancel and return values
        /// </summary>
        /// <returns></returns>
        public async Task OnCancelAsync(TResult result)
        {
            await (ConfirmRef.OnCancel?.Invoke(result) ?? Task.CompletedTask);
        }

        /// <summary>
        /// just Close the Modal and OnOk and OnCancel callback are not triggered
        /// </summary>
        /// <returns></returns>
        protected async Task CloseAsync()
        {
            await ConfirmRef.CloseAsync();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ConfirmRef.ModalTemplate = this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual Task CancelAsync(ModalClosingEventArgs args)
        {
            return Task.CompletedTask;
        }

        public virtual Task OkAsync(ModalClosingEventArgs args)
        {
            return Task.CompletedTask;
        }

    }
}
