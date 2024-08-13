// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class DrawerService
    {
        internal event Func<DrawerRef, Task> OnOpenEvent;

        internal event Func<DrawerRef, Task> OnCloseEvent;

        internal event Func<DrawerRef, Task> OnUpdateEvent;

        /// <summary>
        /// Create and open a simple drawer without result
        /// </summary>
        /// <param name="options">drawer options</param>
        /// <returns>The reference of drawer</returns>
        public async Task<DrawerRef> CreateAsync(DrawerOptions options)
        {
            CheckIsNull(options);
            var drawerRef = new DrawerRef<object>(options, this);
            await (OnOpenEvent?.Invoke(drawerRef) ?? Task.CompletedTask);
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
        public async Task<DrawerRef<TResult>> CreateAsync<TComponent, TComponentOptions, TResult>(DrawerOptions config, TComponentOptions options) where TComponent : FeedbackComponent<TComponentOptions, TResult>
        {
            CheckIsNull(config);

            DrawerRef<TResult> drawerRef = new DrawerRef<TResult>(config, this);

            RenderFragment child = (builder) =>
            {
                builder.OpenComponent<TComponent>(0);
                builder.AddAttribute(1, "FeedbackRef", drawerRef);
                builder.AddAttribute(2, "Options", options);
                builder.CloseComponent();
            };
            config.ChildContent = child;

            await (OnOpenEvent?.Invoke(drawerRef) ?? Task.CompletedTask);
            return drawerRef;
        }

        /// <summary>
        /// Update a drawer
        /// </summary>
        /// <param name="drawerRef"></param>
        /// <returns></returns>
        public async Task UpdateAsync(DrawerRef drawerRef)
        {
            await (OnUpdateEvent?.Invoke(drawerRef) ?? Task.CompletedTask);
        }

        /// <summary>
        /// Create and open a drawer
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TComponentOptions"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="config"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<TResult> CreateDialogAsync<TComponent, TComponentOptions, TResult>(DrawerOptions config, TComponentOptions options) where TComponent : FeedbackComponent<TComponentOptions, TResult>
        {
            CheckIsNull(config);
            DrawerRef<TResult> drawerRef = new DrawerRef<TResult>(config, this);

            drawerRef.TaskCompletionSource = new TaskCompletionSource<TResult>();

            RenderFragment child = (builder) =>
            {
                builder.OpenComponent<TComponent>(0);
                builder.AddAttribute(1, "FeedbackRef", drawerRef);
                builder.AddAttribute(2, "Options", options);
                builder.CloseComponent();
            };
            config.ChildContent = child;

            await (OnOpenEvent?.Invoke(drawerRef) ?? Task.CompletedTask);
            return await drawerRef.TaskCompletionSource.Task;
        }

        public async Task<TResult> CreateDialogAsync<TComponent, TComponentOptions, TResult>(TComponentOptions options,
            bool closable = true,
            bool maskClosable = true,
            string title = null,
            int width = 256,
            bool mask = true,
             string placement = "right") where TComponent : FeedbackComponent<TComponentOptions, TResult>
        {
            var config = new DrawerOptions()
            {
                Closable = closable,
                MaskClosable = maskClosable,
                Title = title,
                Width = width,
                Mask = mask,
                Placement = placement,
            };
            return await CreateDialogAsync<TComponent, TComponentOptions, TResult>(config, options);
        }

        internal Task OpenAsync(DrawerRef drawerRef)
        {
            if (OnOpenEvent != null)
            {
                return OnOpenEvent.Invoke(drawerRef);
            }
            return Task.CompletedTask;
        }

        internal Task CloseAsync(DrawerRef drawerRef)
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
