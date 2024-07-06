using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    /// <summary>
    /// create and open a Modal dialog
    /// </summary>
    public partial class ModalService : IDisposable
    {
        internal event Func<ModalRef, Task> OnModalOpenEvent;
        internal event Func<ModalRef, Task> OnModalCloseEvent;
        internal event Func<ModalRef, Task> OnModalUpdateEvent;

        /// <summary>
        /// Create and open a Modal
        /// </summary>
        /// <returns></returns>
        public ModalRef CreateModal(ModalOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            ModalRef modalRef = new ModalRef(options, this);
            options.ModalRef = modalRef;
            options.CreateByService = true;
            return CreateOrOpenModal(modalRef);
        }

        /// <summary>
        /// Create and open a Modal
        /// </summary>
        /// <returns></returns>
        public ModalRef<TResult> CreateModal<TResult>(ModalOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            var modalRef = new ModalRef<TResult>(options, this);
            options.ModalRef = modalRef;
            options.CreateByService = true;
            return CreateOrOpenModal(modalRef);
        }

        /// <summary>
        /// Create and open a Modal with template
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TComponentOptions"></typeparam>
        /// <param name="options">The modal options</param>
        /// <param name="componentOptions">Set options for template compoennt</param>
        /// <returns></returns>
        public ModalRef CreateModal<TComponent, TComponentOptions>(ModalOptions options, TComponentOptions componentOptions)
            where TComponent : FeedbackComponent<TComponentOptions>
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            ModalRef modalRef = new ModalRef(options, this);

            void Child(RenderTreeBuilder builder)
            {
                builder.OpenComponent<TComponent>(0);
                builder.AddAttribute(1, "FeedbackRef", modalRef);
                builder.AddAttribute(2, "Options", componentOptions);
                builder.CloseComponent();
            }
            options.Content = Child;
            options.ModalRef = modalRef;
            options.CreateByService = true;
            return CreateOrOpenModal(modalRef);
        }

        /// <summary>
        /// Create and open a Modal with template component
        /// </summary>
        /// <typeparam name="TComponent">The type of the template component.</typeparam>
        /// <typeparam name="TComponentOptions">The type of the template component options.</typeparam>
        /// <typeparam name="TResult">The result returned from the template component.</typeparam>
        /// <param name="options">The modal options</param>
        /// <param name="componentOptions">Set options for template compoennt</param>
        /// <returns></returns>
        public ModalRef<TResult> CreateModal<TComponent, TComponentOptions, TResult>(ModalOptions options, TComponentOptions componentOptions)
            where TComponent : FeedbackComponent<TComponentOptions, TResult>
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            var modalRef = new ModalRef<TResult>(options, this);

            void Child(RenderTreeBuilder builder)
            {
                builder.OpenComponent<TComponent>(0);
                builder.AddAttribute(1, "FeedbackRef", modalRef);
                builder.AddAttribute(2, "Options", componentOptions);
                builder.CloseComponent();
            }
            options.Content = Child;
            options.ModalRef = modalRef;
            options.CreateByService = true;
            return CreateOrOpenModal(modalRef);
        }

        /// <summary>
        /// Create and open a Modal
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use the CreateModal instead.")]
        public Task<ModalRef> CreateModalAsync(ModalOptions options)
        {
            return Task.FromResult(CreateModal(options));
        }

        /// <summary>
        /// Create and open a Modal
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use the CreateModal instead.")]
        public Task<ModalRef<TResult>> CreateModalAsync<TResult>(ModalOptions options)
        {
            return Task.FromResult(CreateModal<TResult>(options));
        }

        /// <summary>
        /// Create and open a Modal with template
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TComponentOptions"></typeparam>
        /// <param name="options"></param>
        /// <param name="componentOptions"></param>
        /// <returns></returns>
        [Obsolete("Use the CreateModal instead.")]
        public Task<ModalRef> CreateModalAsync<TComponent, TComponentOptions>(ModalOptions options, TComponentOptions componentOptions)
            where TComponent : FeedbackComponent<TComponentOptions>
        {
            return Task.FromResult(CreateModal<TComponent, TComponentOptions>(options, componentOptions));
        }

        /// <summary>
        /// Create and open a Modal with template
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TComponentOptions"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="options"></param>
        /// <param name="componentOptions"></param>
        /// <returns></returns>
        [Obsolete("Use the CreateModal instead.")]
        public Task<ModalRef<TResult>> CreateModalAsync<TComponent, TComponentOptions, TResult>(ModalOptions options, TComponentOptions componentOptions)
            where TComponent : FeedbackComponent<TComponentOptions, TResult>
        {
            return Task.FromResult(CreateModal<TComponent, TComponentOptions, TResult>(options, componentOptions));
        }

        /// <summary>
        /// create or open a Modal dialog
        /// </summary>
        /// <param name="modalRef"></param>
        /// <returns></returns>
        internal Task<ModalRef> CreateOrOpenModalAsync(ModalRef modalRef)
        {
            OnModalOpenEvent?.Invoke(modalRef);
            return Task.FromResult(modalRef);
        }

        internal ModalRef CreateOrOpenModal(ModalRef modalRef)
        {
            OnModalOpenEvent?.Invoke(modalRef);
            return modalRef;
        }

        /// <summary>
        /// create or open a Modal dialog
        /// </summary>
        /// <param name="modalRef"></param>
        /// <returns></returns>
        internal Task<ModalRef<TResult>> CreateOrOpenModalAsync<TResult>(ModalRef<TResult> modalRef)
        {
            OnModalOpenEvent?.Invoke(modalRef);
            return Task.FromResult(modalRef);
        }

        internal ModalRef<TResult> CreateOrOpenModal<TResult>(ModalRef<TResult> modalRef)
        {
            OnModalOpenEvent?.Invoke(modalRef);
            return modalRef;
        }

        /// <summary>
        /// close modal dialog
        /// </summary>
        /// <param name="modalRef"></param>
        /// <returns></returns>
        internal Task CloseModalAsync(ModalRef modalRef)
        {
            return OnModalCloseEvent?.Invoke(modalRef) ?? Task.CompletedTask;
        }

        internal async Task UpdateModalAsync(ModalRef modalRef)
        {
            await (OnModalUpdateEvent?.Invoke(modalRef) ?? Task.CompletedTask);
        }

        internal void UpdateModal(ModalRef modalRef)
        {
            OnModalUpdateEvent?.Invoke(modalRef);
        }

        /// <summary>
        /// Implement the interface IDisposable
        /// </summary>
        void IDisposable.Dispose()
        {
        }
    }
}
