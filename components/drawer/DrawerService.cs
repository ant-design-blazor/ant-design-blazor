using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class DrawerService
    {
        internal event Func<IDrawerRef, Task> OnOpenEvent;

        internal event Func<IDrawerRef, Task> OnCloseEvent;

        /// <summary>
        /// 创建并打开一个简单抽屉，没有返回值
        /// </summary>
        /// <param name="config">抽屉参数</param>
        /// <returns></returns>
        public async Task<IDrawerRef> CreateAsync(DrawerOptions config)
        {
            CheckIsNull(config);
            IDrawerRef drawerRef = new DrawerRef<object>(config, this);
            await OnOpenEvent.Invoke(drawerRef);
            return drawerRef;
        }

        /// <summary>
        /// 创建并打开一个模板抽屉
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TContentParams"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="config"></param>
        /// <param name="contentParams"></param>
        /// <returns></returns>
        public async Task<DrawerRef<TResult>> CreateAsync<TComponent, TContentParams, TResult>(DrawerOptions config, TContentParams contentParams) where TComponent : DrawerTemplate<TContentParams, TResult>
        {
            CheckIsNull(config);

            DrawerRef<TResult> drawerRef = new DrawerRef<TResult>(config, this);
            await OnOpenEvent.Invoke(drawerRef);

            RenderFragment child = (builder) =>
            {
                builder.OpenComponent<TComponent>(0);
                builder.AddAttribute(1, "DrawerRef", drawerRef);
                builder.AddAttribute(2, "Config", contentParams);
                builder.CloseComponent();
            };
            config.ChildContent = child;

            return drawerRef;
        }

        public async Task<TResult> CreateDialogAsync<TComponent, TContentParams, TResult>(DrawerOptions config, TContentParams contentParams) where TComponent : DrawerTemplate<TContentParams, TResult>
        {
            CheckIsNull(config);
            DrawerRef<TResult> drawerRef = new DrawerRef<TResult>(config, this);

            drawerRef.TaskCompletionSource = new TaskCompletionSource<TResult>(); ;
            await OnOpenEvent.Invoke(drawerRef);

            RenderFragment child = (builder) =>
            {
                builder.OpenComponent<TComponent>(0);
                builder.AddAttribute(1, "DrawerRef", drawerRef);
                builder.AddAttribute(2, "Config", contentParams);
                builder.CloseComponent();
            };
            config.ChildContent = child;

            return await drawerRef.TaskCompletionSource.Task;
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
