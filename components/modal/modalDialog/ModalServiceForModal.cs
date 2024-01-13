﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace AntDesign
{
    /// <summary>
    /// create and open a Modal dialog
    /// </summary>
    public partial class ModalService: IDisposable
    {
        internal event Func<ModalRef, Task> OnModalOpenEvent;
        internal event Func<ModalRef, Task> OnModalCloseEvent;
        internal event Func<ModalRef, Task> OnModalUpdateEvent;


        /// <summary>
        /// Create and open a Modal
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
        /// Create and open a Modal
        /// </summary>
        /// <returns></returns>
        public Task<ModalRef<TResult>> CreateModalAsync<TResult>(ModalOptions config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            var modalRef = new ModalRef<TResult>(config, this);
            config.ModalRef = modalRef;
            return CreateOrOpenModalAsync(modalRef);
        }

        /// <summary>
        /// Create and open a Modal with template
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TComponentOptions"></typeparam>
        /// <param name="config"></param>
        /// <param name="componentOptions"></param>
        /// <returns></returns>
        public Task<ModalRef> CreateModalAsync<TComponent, TComponentOptions>(ModalOptions config, TComponentOptions componentOptions) where TComponent : FeedbackComponent<TComponentOptions>
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            ModalRef modalRef = new ModalRef(config, this);

            void Child(RenderTreeBuilder builder)
            {
                builder.OpenComponent<TComponent>(0);
                builder.AddAttribute(1, "FeedbackRef", modalRef);
                builder.AddAttribute(2, "Options", componentOptions);
                builder.CloseComponent();
            }
            config.Content = Child;
            config.ModalRef = modalRef;
            return CreateOrOpenModalAsync(modalRef);
        }

        /// <summary>
        /// Create and open a Modal with template
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TComponentOptions"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="config"></param>
        /// <param name="componentOptions"></param>
        /// <returns></returns>
        public Task<ModalRef<TResult>> CreateModalAsync<TComponent, TComponentOptions, TResult>(ModalOptions config, TComponentOptions componentOptions) where TComponent : FeedbackComponent<TComponentOptions, TResult>
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            var modalRef = new ModalRef<TResult>(config, this);

            void Child(RenderTreeBuilder builder)
            {
                builder.OpenComponent<TComponent>(0);
                builder.AddAttribute(1, "FeedbackRef", modalRef);
                builder.AddAttribute(2, "Options", componentOptions);
                builder.CloseComponent();
            }
            config.Content = Child;
            config.ModalRef = modalRef;
            return CreateOrOpenModalAsync(modalRef);
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


        /// <summary>
        /// close modal dialog
        /// </summary>
        /// <param name="modalRef"></param>
        /// <returns></returns>
        internal Task CloseModalAsync(ModalRef modalRef)
        {
            if (OnModalCloseEvent != null)
            {
                return OnModalCloseEvent.Invoke(modalRef);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Implement the interface IDisposable
        /// </summary>
        public void Dispose()
        {
        }


        public async Task UpdateModalAsync(ModalRef modalRef)
        {
            await(OnModalUpdateEvent?.Invoke(modalRef) ?? Task.CompletedTask);
        }
    }
}
