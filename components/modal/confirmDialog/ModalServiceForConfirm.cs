using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ModalService
    {
        internal event Func<ConfirmRef, Task> OnConfirmOpenEvent;

        internal event Func<ConfirmRef, Task> OnConfirmUpdateEvent;

        internal event Func<ConfirmRef, Task> OnConfirmCloseEvent;

        internal event Func<Task> OnConfirmCloseAllEvent;

        #region SimpleConfirm

        /// <summary>
        /// create and open a OK-Cancel Confirm dialog
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        public ConfirmRef Confirm(ConfirmOptions props)
        {
            CheckConfirmOptionsIsNull(props);
            ConfirmRef confirmRef = new ConfirmRef(props, this);
            confirmRef.TaskCompletionSource = new TaskCompletionSource<ConfirmResult>();
            OnConfirmOpenEvent?.Invoke(confirmRef);
            return confirmRef;
        }

        /// <summary>
        /// create and open a OK-Cancel Confirm dialog with info icon
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
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

        /// <summary>
        /// create and open a OK-Cancel Confirm dialog with success icon
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
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

        /// <summary>
        /// create and open a OK-Cancel Confirm dialog with error icon
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
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

        /// <summary>
        /// create and open a OK-Cancel Confirm dialog with Warning icon
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
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

        #endregion

        #region Confirm with return the OK button is clicked

        /// <summary>
        /// create and open a OK-Cancel Confirm dialog,
        /// and return a bool value which indicates whether the OK button has been clicked
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        public async Task<bool> ConfirmAsync(ConfirmOptions props)
        {
            ConfirmRef confirmRef = new ConfirmRef(props, this);
            confirmRef.TaskCompletionSource = new TaskCompletionSource<ConfirmResult>();
            if (OnConfirmOpenEvent != null)
            {
                await OnConfirmOpenEvent.Invoke(confirmRef);
            }

            return await confirmRef.TaskCompletionSource.Task
                .ContinueWith(t => t.Result == ConfirmResult.OK, TaskScheduler.Default);
        }

        /// <summary>
        /// create and open a OK-Cancel Confirm dialog with info icon,
        /// and return a bool value which indicates whether the OK button has been clicked
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
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

        /// <summary>
        /// create and open a OK-Cancel Confirm dialog with success icon,
        /// and return a bool value which indicates whether the OK button has been clicked
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
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

        /// <summary>
        /// create and open a OK-Cancel Confirm dialog with error icon,
        /// and return a bool value which indicates whether the OK button has been clicked
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
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

        /// <summary>
        /// create and open a OK-Cancel Confirm dialog with warning icon,
        /// and return a bool value which indicates whether the OK button has been clicked
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
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

        /// <summary>
        /// update Confirm which Visible=true
        /// </summary>
        /// <param name="confirmRef"></param>
        /// <returns></returns>
        [Obsolete("Use the UpdateConfirmAsync method instead")]
        public Task Update(ConfirmRef confirmRef)
        {
            return UpdateConfirmAsync(confirmRef);
        }

        /// <summary>
        /// update Confirm which Visible=true
        /// </summary>
        /// <param name="confirmRef"></param>
        /// <returns></returns>
        public async Task UpdateConfirmAsync(ConfirmRef confirmRef)
        {
            await (OnConfirmUpdateEvent?.Invoke(confirmRef) ?? Task.CompletedTask);
        }

        /// <summary>
        /// close a Confirm dialog
        /// </summary>
        /// <param name="confirmRef"></param>
        /// <returns></returns>
        [Obsolete("Use the DestroyConfirmAsync method instead")]
        public Task Destroy(ConfirmRef confirmRef)
        {
            return DestroyConfirmAsync(confirmRef);
        }

        /// <summary>
        /// close the Confirm dialog
        /// </summary>
        /// <param name="confirmRef"></param>
        /// <returns></returns>
        public async Task DestroyConfirmAsync(ConfirmRef confirmRef)
        {
            await (OnConfirmCloseEvent?.Invoke(confirmRef) ?? Task.CompletedTask);
        }

        /// <summary>
        /// close all Confirm dialog
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use the DestroyAllConfirmAsync method instead")]
        public Task DestroyAll()
        {
            return DestroyAllConfirmAsync();
        }

        /// <summary>
        /// close all Confirm dialog
        /// </summary>
        /// <returns></returns>
        public async Task DestroyAllConfirmAsync()
        {
            await (OnConfirmCloseAllEvent?.Invoke() ?? Task.CompletedTask);
        }

        /// <summary>
        /// Create and open a OK-Cancel Confirm asynchronous
        /// </summary>
        /// <param name="config">Options</param>
        /// <returns></returns>
        [Obsolete("Use the CreateAsync method instead")]
        public Task<ConfirmRef> CreateAsync(ConfirmOptions config)
        {
            return CreateConfirmAsync(config);
        }

        /// <summary>
        /// Create and open a OK-Cancel Confirm asynchronous
        /// </summary>
        /// <param name="config">Options</param>
        /// <returns></returns>
        public Task<ConfirmRef> CreateConfirmAsync(ConfirmOptions config)
        {
            var confirmRef = Confirm(config);
            return Task.FromResult(confirmRef);
        }

        /// <summary>
        /// Create and open template Confirm dialog
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TComponentOptions"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="config"></param>
        /// <param name="componentOptions"></param>
        /// <returns></returns>
        [Obsolete("Use the CreateAsync method instead")]
        public Task<ConfirmRef<TResult>> CreateAsync<TComponent, TComponentOptions, TResult>
        (ConfirmOptions config, TComponentOptions componentOptions) where TComponent : FeedbackComponent<TComponentOptions, TResult>
        {
            return CreateConfirmAsync<TComponent, TComponentOptions, TResult>(config, componentOptions);
        }

        /// <summary>
        /// Create and open template Confirm dialog
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TComponentOptions"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="config"></param>
        /// <param name="componentOptions"></param>
        /// <returns></returns>
        public Task<ConfirmRef<TResult>> CreateConfirmAsync<TComponent, TComponentOptions, TResult>(ConfirmOptions config, TComponentOptions componentOptions) where TComponent : FeedbackComponent<TComponentOptions, TResult>
        {
            CheckConfirmOptionsIsNull(config);

            ConfirmRef<TResult> confirmRef = new ConfirmRef<TResult>(config, this);
            OnConfirmOpenEvent?.Invoke(confirmRef);

            RenderFragment child = (builder) =>
            {
                builder.OpenComponent<TComponent>(0);
                builder.AddAttribute(1, "FeedbackRef", confirmRef);
                builder.AddAttribute(2, "Options", componentOptions);
                builder.CloseComponent();
            };
            config.Content = child;

            return Task.FromResult(confirmRef);
        }

        /// <summary>
        /// open the Confirm dialog
        /// </summary>
        /// <param name="confirmRef"></param>
        /// <returns></returns>
        internal async Task OpenConfirmAsync(ConfirmRef confirmRef)
        {
            if (OnConfirmOpenEvent != null)
            {
                await OnConfirmOpenEvent.Invoke(confirmRef);
            }
        }

        /// <summary>
        /// check Confirm options is null
        /// </summary>
        /// <param name="options"></param>
        private static void CheckConfirmOptionsIsNull(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
        }
    }
}
