using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class DrawerService
    {
        internal event Func<DrawerRef, Task> OnCreateEvent;

        internal event Func<DrawerRef, Task> OnCloseEvent;

        internal event Func<DrawerRef, Task> OnDestroyEvent;


        /// <summary>
        /// 创建并打开一个简单抽屉，没有返回值
        /// </summary>
        /// <param name="config">抽屉参数</param>
        /// <returns></returns>
        public async Task<DrawerRef> CreateAsync(DrawerConfig config)
        {
            CheckIsNull(config);
            DrawerRef drawerRef = new DrawerRef(config, this);
            await OnCreateEvent.Invoke(drawerRef);
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
        public async Task<DrawerRef<TResult>> CreateAsync<TComponent, TContentParams, TResult>(DrawerConfig config, TContentParams contentParams) where TComponent : DrawerTemplate<TContentParams, TResult>
        {
            CheckIsNull(config);

            DrawerRef<TResult> drawerRef = new DrawerRef<TResult>(config, this);
            await OnCreateEvent.Invoke(drawerRef);

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

        internal Task OpenAsync(DrawerRef drawerRef)
        {
            if (OnCreateEvent != null)
            {
                return OnCreateEvent.Invoke(drawerRef);
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


        internal Task DestroyAsync(DrawerRef drawerRef)
        {
            if (OnDestroyEvent != null)
            {
                return OnDestroyEvent.Invoke(drawerRef);
            }
            return Task.CompletedTask;
        }

        private static void CheckIsNull(DrawerConfig options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
        }
    }
}
