using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class ModalService
    {
        internal event Func<ModalRef, Task> OnOpenEvent;

        internal event Func<ModalRef, Task> OnCloseEvent;

        internal event Func<ModalRef, Task> OnUpdateEvent;

        internal event Func<ModalRef, Task> OnDestroyEvent;

        internal event Func<Task> OnDestroyAllEvent;

        #region SimpleConfirm

        public ModalRef Confirm(ConfirmOptions props)
        {
            ModalRef modalRef = new ModalRef(props, this);
            modalRef.TaskCompletionSource = new TaskCompletionSource<ConfirmResult>();
            OnOpenEvent?.Invoke(modalRef);
            return modalRef;
        }

        public ModalRef Info(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            options.ConfirmIcon = ConfirmIcon.Info;
            options.OkCancel = false;
            return Confirm(options);
        }

        public ModalRef Success(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            options.ConfirmIcon = ConfirmIcon.Success;
            options.OkCancel = false;
            return Confirm(options);
        }

        public ModalRef Error(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            options.ConfirmIcon = ConfirmIcon.Error;
            options.OkCancel = false;
            return Confirm(options);
        }

        public ModalRef Warning(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            options.ConfirmIcon = ConfirmIcon.Warning;
            options.OkCancel = false;
            return Confirm(options);
        }

        public async Task<bool> ConfirmAsync(ConfirmOptions props)
        {
            ModalRef modalRef = new ModalRef(props, this);
            modalRef.TaskCompletionSource = new TaskCompletionSource<ConfirmResult>();
            if (OnOpenEvent != null)
            {
                await OnOpenEvent.Invoke(modalRef);
            }

            return await modalRef.TaskCompletionSource.Task
                .ContinueWith(t =>
                {
                    return t.Result == ConfirmResult.OK;
                }, TaskScheduler.Default);
        }

        public Task<bool> InfoAsync(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            options.ConfirmIcon = ConfirmIcon.Info;
            options.OkCancel = false;
            return ConfirmAsync(options);
        }

        public Task<bool> SuccessAsync(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            options.ConfirmIcon = ConfirmIcon.Success;
            options.OkCancel = false;
            return ConfirmAsync(options);
        }

        public Task<bool> ErrorAsync(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            options.ConfirmIcon = ConfirmIcon.Error;
            options.OkCancel = false;
            return ConfirmAsync(options);
        }

        public Task<bool> WarningAsync(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            options.ConfirmIcon = ConfirmIcon.Warning;
            options.OkCancel = false;
            return ConfirmAsync(options);
        }

        #endregion SimpleConfirm

        public Task Update(ModalRef modalRef)
        {
            return OnUpdateEvent?.Invoke(modalRef);
        }

        public Task Destroy(ModalRef modalRef)
        {
            return OnDestroyEvent?.Invoke(modalRef);
        }

        public Task DestroyAll()
        {
            return OnDestroyAllEvent?.Invoke();
        }

        /// <summary>
        /// Create and open a Moal
        /// </summary>
        /// <param name="config">Options</param>
        /// <returns></returns>
        public async Task<ModalRef> CreateAsync(ConfirmOptions config)
        {
            CheckIsNull(config);
            ModalRef modalRef = new ModalRef(config, this);
            OnOpenEvent.Invoke(modalRef);
            return modalRef;
        }

        /// <summary>
        /// Create and open template modal
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TComponentOptions"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="config"></param>
        /// <param name="componentOptions"></param>
        /// <returns></returns>
        public async Task<ModalRef<TResult>> CreateAsync<TComponent, TComponentOptions, TResult>(ConfirmOptions config, TComponentOptions componentOptions) where TComponent : ModalTemplate<TComponentOptions, TResult>
        {
            CheckIsNull(config);

            ModalRef<TResult> modalRef = new ModalRef<TResult>(config, this);
            OnOpenEvent.Invoke(modalRef);

            RenderFragment child = (builder) =>
            {
                builder.OpenComponent<TComponent>(0);
                builder.AddAttribute(1, "ModalRef", modalRef);
                builder.AddAttribute(2, "Options", componentOptions);
                builder.CloseComponent();
            };
            config.Content = child;

            return modalRef;
        }

        internal Task OpenAsync(ModalRef modalRef)
        {
            if (OnOpenEvent != null)
            {
                OnOpenEvent.Invoke(modalRef);
            }
            return Task.CompletedTask;
        }

        internal Task CloseAsync(ModalRef modalRef)
        {
            if (OnCloseEvent != null)
            {
                return OnCloseEvent.Invoke(modalRef);
            }
            return Task.CompletedTask;
        }

        private static void CheckIsNull(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
        }
    }
}
