using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class ModalTemplate<TComponentOptions, TResult> : TemplateComponentBase<TComponentOptions>, IModalTemplate
    {
        [Parameter]
        public ModalRef<TResult> ModalRef { get; set; }


        /// <summary>
        /// Emit Ok and return values
        /// </summary>
        /// <returns></returns>
        public async Task OnOkAsync(TResult result)
        {
            await ModalRef.OnOk?.Invoke(result);
        }

        /// <summary>
        /// Emit Cancel and return values
        /// </summary>
        /// <returns></returns>
        public async Task OnCancelAsync(TResult result)
        {
            await ModalRef.OnCancel?.Invoke(result);
        }


        /// <summary>
        /// Close the Modal
        /// </summary>
        /// <returns></returns>
        protected async Task CloseAsync()
        {
            await ModalRef.CloseAsync();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ModalRef.ModalTemplate = this;
        }


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
