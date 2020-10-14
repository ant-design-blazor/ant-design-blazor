using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ModalService
    {
        internal event Func<ModalRef, Task> OnModalOpenEvent;

        internal event Func<ModalRef, Task> OnModalCloseEvent;

        /// <summary>
        /// Create and open a Moal
        /// </summary>
        /// <returns></returns>
        public Task<ModalRef> CreateModalAsync(ModalOptions config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            ModalRef modalRef = new ModalRef(config, this);
            config.ModalRef = modalRef;
            return CreateOrOpenModalAsync(modalRef);
        }

        /// <summary>
        /// Create and open template modal
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TComponentOptions"></typeparam>
        /// <param name="config"></param>
        /// <param name="componentOptions"></param>
        /// <returns></returns>
        public Task<ModalRef> CreateModalAsync<TComponent, TComponentOptions>(ModalOptions config, TComponentOptions componentOptions) where TComponent : ModalTemplate<TComponentOptions>
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            ModalRef modalRef = new ModalRef(config, this);

            void Child(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
            {
                builder.OpenComponent<TComponent>(0);
                builder.AddAttribute(1, "ModalRef", modalRef);
                builder.AddAttribute(2, "Options", componentOptions);
                builder.CloseComponent();
            }
            config.Content = Child;
            config.ModalRef = modalRef;
            return CreateOrOpenModalAsync(modalRef);
        }

        internal Task<ModalRef> CreateOrOpenModalAsync(ModalRef modalRef)
        {
            OnModalOpenEvent?.Invoke(modalRef);
            return Task.FromResult(modalRef);
        }

        internal Task CloseModalAsync(ModalRef modalRef)
        {
            if (OnModalCloseEvent != null)
            {
                return OnModalCloseEvent.Invoke(modalRef);
            }
            return Task.CompletedTask;
        }
    }
}
