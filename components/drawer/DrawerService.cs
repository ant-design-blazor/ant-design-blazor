using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public class DrawerService
    {
        internal event Func<IDrawerRef, Task> OnOpenEvent;

        internal event Func<IDrawerRef, Task> OnCloseEvent;

        /// <summary>
        /// Create and open a simple drawer without result
        /// </summary>
        /// <param name="options">drawer options</param>
        /// <returns>The reference of drawer</returns>
        public async Task<IDrawerRef> CreateAsync(DrawerOptions options)
        {
            CheckIsNull(options);
            IDrawerRef drawerRef = new DrawerRef<object>(options, this);
            await OnOpenEvent.Invoke(drawerRef);
            return drawerRef;
        }

        /// <summary>
        /// Create and open a drawer with the template
        /// </summary>
        /// <typeparam name="TComponent">The type of DrawerTemplate implement</typeparam>
        /// <typeparam name="TComponentOptions">The </typeparam>
        /// <typeparam name="TResult">The type of return value</typeparam>
        /// <param name="config"></param>
        /// <param name="options"></param>
        /// <returns>The reference of drawer</returns>
        public async Task<DrawerRef<TResult>> CreateAsync<TComponent, TComponentOptions, TResult>(DrawerOptions config, TComponentOptions options) where TComponent : DrawerTemplate<TComponentOptions, TResult>
        {
            CheckIsNull(config);

            DrawerRef<TResult> drawerRef = new DrawerRef<TResult>(config, this);
            await OnOpenEvent.Invoke(drawerRef);

            RenderFragment child = (builder) =>
            {
                builder.OpenComponent<TComponent>(0);
                builder.AddAttribute(1, "DrawerRef", drawerRef);
                builder.AddAttribute(2, "Options", options);
                builder.CloseComponent();
            };
            config.ChildContent = child;

            return drawerRef;
        }

        public async Task<TResult> CreateDialogAsync<TComponent, TComponentOptions, TResult>(DrawerOptions config, TComponentOptions options) where TComponent : DrawerTemplate<TComponentOptions, TResult>
        {
            CheckIsNull(config);
            DrawerRef<TResult> drawerRef = new DrawerRef<TResult>(config, this);

            drawerRef.TaskCompletionSource = new TaskCompletionSource<TResult>(); ;
            await OnOpenEvent.Invoke(drawerRef);

            RenderFragment child = (builder) =>
            {
                builder.OpenComponent<TComponent>(0);
                builder.AddAttribute(1, "DrawerRef", drawerRef);
                builder.AddAttribute(2, "Options", options);
                builder.CloseComponent();
            };
            config.ChildContent = child;

            return await drawerRef.TaskCompletionSource.Task;
        }


        public async Task<TResult> CreateDialogAsync<TComponent, TComponentOptions, TResult>(TComponentOptions options,
            bool closable = true,
            bool maskClosable = true,
            string title = null,
            int width = 256,
            bool mask = true,
            bool noAnimation = false,
             string placement = "right") where TComponent : DrawerTemplate<TComponentOptions, TResult>
        {
            var config = new DrawerOptions()
            {
                Closable = closable,
                MaskClosable = maskClosable,
                Title = title,
                Width = width,
                Mask = mask,
                NoAnimation = noAnimation,
                Placement = placement,
            };
            return await CreateDialogAsync<TComponent, TComponentOptions, TResult>(config, options);
        }

        internal Task OpenAsync(IDrawerRef drawerRef)
        {
            if (OnOpenEvent != null)
            {
                return OnOpenEvent.Invoke(drawerRef);
            }
            return Task.CompletedTask;
        }

        internal Task CloseAsync(IDrawerRef drawerRef)
        {
            if (OnCloseEvent != null)
            {
                return OnCloseEvent.Invoke(drawerRef);
            }
            return Task.CompletedTask;
        }

        private static void CheckIsNull(DrawerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
        }
    }
}
