using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class DrawerContainer
    {
        [Inject] private DrawerService DrawerService { get; set; }

        protected override void OnInitialized()
        {
            DrawerService.OnCloseEvent += DrawerService_OnClose;
            DrawerService.OnOpenEvent += DrawerService_OnCreate;
        }

        protected override void Dispose(bool disposing)
        {
            DrawerService.OnCloseEvent -= DrawerService_OnClose;
            DrawerService.OnOpenEvent -= DrawerService_OnCreate;
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
            await Task.Delay(300);
            if (_drawerRefs.Contains(drawerRef))
            {
                _drawerRefs.Remove(drawerRef);
            }
        }
    }
}
