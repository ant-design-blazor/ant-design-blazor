using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class DrawerService
    {
        internal event Func<DrawerConfig, Task> OnCreate;

        internal event Func<DrawerConfig, Task> OnClose;

        /// <summary>
        /// Create a drawer from the config
        /// </summary>
        /// <param name="config">DrawerConfig</param>
        /// <returns>DrawerRef object</returns>
        public async Task<DrawerRef> CreateAsync(DrawerConfig config)
        {
            CheckIsNull(config);
            return await HandleCreate(config);
        }

        /// <summary>
        /// Create a drawer from an existing component template
        /// </summary>
        /// <typeparam name="T">compontent template type</typeparam>
        /// <typeparam name="TCompontentParameter">compontent template parameter object type</typeparam>
        /// <param name="config">DrawerConfig</param>
        /// <param name="parameter">compontent template parameter object</param>
        /// <returns>DrawerRef object</returns>
        public async Task<DrawerRef> CreateAsync<T, TCompontentParameter>(DrawerConfig config, TCompontentParameter parameter) where T : DrawerTemplate<TCompontentParameter>
        {
            CheckIsNull(config);

            RenderFragment child = (builder) =>
            {
                builder.OpenComponent<T>(0);
                builder.AddAttribute(1, "Config", parameter);
                builder.AddAttribute(2, "DrawerConfig", config);
                builder.CloseComponent();
            };
            config.ChildContent = child;

            return await HandleCreate(config);
        }


        private async Task<DrawerRef> HandleCreate(DrawerConfig config)
        {
            if (OnCreate != null)
            {
                config.DrawerService = this;
                await OnCreate.Invoke(config);
                DrawerRef drawerRef = new DrawerRef(config);
                return drawerRef;
            }
            return null;
        }

        internal Task CloseAsync(DrawerConfig options)
        {
            CheckIsNull(options);
            if (OnClose != null)
            {
                return OnClose.Invoke(options);
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
