using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class ModalService
    {
        internal event Func<ConfirmRef, Task> OnOpenEvent;

        internal event Func<ConfirmRef, Task> OnCloseEvent;

        internal event Func<ConfirmRef, Task> OnUpdateEvent;

        internal event Func<ConfirmRef, Task> OnDestroyEvent;

        internal event Func<Task> OnDestroyAllEvent;

        #region SimpleConfirm

        public ConfirmRef Confirm(ConfirmOptions props)
        {
            ConfirmRef confirmRef = new ConfirmRef(props, this);
            confirmRef.TaskCompletionSource = new TaskCompletionSource<ConfirmResult>();
            OnOpenEvent?.Invoke(confirmRef);
            return confirmRef;
        }

        public ConfirmRef Info(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            options.ConfirmIcon = ConfirmIcon.Info;
            options.OkCancel = false;
            return Confirm(options);
        }

        public ConfirmRef Success(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            options.ConfirmIcon = ConfirmIcon.Success;
            options.OkCancel = false;
            return Confirm(options);
        }

        public ConfirmRef Error(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            options.ConfirmIcon = ConfirmIcon.Error;
            options.OkCancel = false;
            return Confirm(options);
        }

        public ConfirmRef Warning(ConfirmOptions options)
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
            ConfirmRef confirmRef = new ConfirmRef(props, this);
            confirmRef.TaskCompletionSource = new TaskCompletionSource<ConfirmResult>();
            if (OnOpenEvent != null)
            {
                await OnOpenEvent.Invoke(confirmRef);
            }

            return await confirmRef.TaskCompletionSource.Task
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

        public Task Update(ConfirmRef confirmRef)
        {
            return OnUpdateEvent?.Invoke(confirmRef);
        }

        public Task Destroy(ConfirmRef confirmRef)
        {
            return OnDestroyEvent?.Invoke(confirmRef);
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
        public async Task<ConfirmRef> CreateAsync(ConfirmOptions config)
        {
            CheckIsNull(config);
            ConfirmRef confirmRef = new ConfirmRef(config, this);
            OnOpenEvent.Invoke(confirmRef);
            return confirmRef;
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
        public async Task<ConfirmRef<TResult>> CreateAsync<TComponent, TComponentOptions, TResult>(ConfirmOptions config, TComponentOptions componentOptions) where TComponent : ModalTemplate<TComponentOptions, TResult>
        {
            CheckIsNull(config);

            ConfirmRef<TResult> confirmRef = new ConfirmRef<TResult>(config, this);
            OnOpenEvent.Invoke(confirmRef);

            RenderFragment child = (builder) =>
            {
                builder.OpenComponent<TComponent>(0);
                builder.AddAttribute(1, "ConfirmRef", confirmRef);
                builder.AddAttribute(2, "Options", componentOptions);
                builder.CloseComponent();
            };
            config.Content = child;

            return confirmRef;
        }

        internal Task OpenAsync(ConfirmRef confirmRef)
        {
            if (OnOpenEvent != null)
            {
                OnOpenEvent.Invoke(confirmRef);
            }
            return Task.CompletedTask;
        }

        internal Task CloseAsync(ConfirmRef confirmRef)
        {
            if (OnCloseEvent != null)
            {
                return OnCloseEvent.Invoke(confirmRef);
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
