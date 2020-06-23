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
            DrawerService.OnCreate += DrawerService_OnCreate;
            DrawerService.OnClose += DrawerService_OnClose;
        }

        protected override void Dispose(bool disposing)
        {
            DrawerService.OnCreate -= DrawerService_OnCreate;
            DrawerService.OnClose -= DrawerService_OnClose;
            base.Dispose(disposing);
        }


        private List<DrawerConfig> _drawerConfigs = new List<DrawerConfig>();


        private async Task DrawerService_OnClose(DrawerConfig arg)
        {
            arg.Visible = false;
            await InvokeAsync(StateHasChanged);
        }

        private async Task DrawerService_OnCreate(DrawerConfig arg)
        {
            arg.Visible = true;
            if (!_drawerConfigs.Contains(arg))
            {
                _drawerConfigs.Add(arg);
            }

            await InvokeAsync(StateHasChanged);
        }

    }
}
