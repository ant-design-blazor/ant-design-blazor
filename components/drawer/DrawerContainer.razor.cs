using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class DrawerContainer
    {
        [Inject]
        private DrawerService DrawerService { get; set; }

        protected override void OnInitialized()
        {
            DrawerService.OnCloseEvent += DrawerService_OnClose;
            DrawerService.OnCreateEvent += DrawerService_OnCreate;
            DrawerService.OnDestroyEvent += DrawerService_OnDestroy;
        }

        protected override void Dispose(bool disposing)
        {
            DrawerService.OnCloseEvent -= DrawerService_OnClose;
            DrawerService.OnCreateEvent -= DrawerService_OnCreate;
            DrawerService.OnDestroyEvent -= DrawerService_OnDestroy;
            base.Dispose(disposing);
        }

        private List<DrawerRef> _drawerRefs = new List<DrawerRef>();


        /// <summary>
        /// 创建并打开抽屉
        /// </summary>
        private async Task DrawerService_OnCreate(DrawerRef drawerRef)
        {
            drawerRef.Config.Visible = true;
            if (!_drawerRefs.Contains(drawerRef))
            {
                _drawerRefs.Add(drawerRef);
            }
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// 关闭抽屉
        /// </summary>
        private async Task DrawerService_OnClose(DrawerRef drawerRef)
        {
            drawerRef.Config.Visible = false;
            await InvokeAsync(StateHasChanged);
        }


        /// <summary>
        /// 销毁抽屉
        /// </summary>
        private async Task DrawerService_OnDestroy(DrawerRef drawerRef)
        {
            drawerRef.Config.Visible = false;
            if (!_drawerRefs.Contains(drawerRef))
            {
                _drawerRefs.Remove(drawerRef);
            }
            await InvokeAsync(StateHasChanged);
        }

    }
}
