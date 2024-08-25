using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ModalContainer
    {
        [Inject]
        private ModalService ModalService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private readonly List<ModalRef> _modalRefs = new List<ModalRef>();

        protected override void OnInitialized()
        {
            ModalService.OnModalOpenEvent += ModalService_OnModalOpenEvent;
            ModalService.OnModalCloseEvent += ModalService_OnModalCloseEvent;
            ModalService.OnModalUpdateEvent += ModalService_OnModalUpdateEvent;

            NavigationManager.LocationChanged += OnLocationChanged;
        }

        private void OnLocationChanged(object sender, EventArgs e)
        {
            _modalRefs.Clear();
            InvokeStateHasChanged();
        }

        private async Task ModalService_OnModalOpenEvent(ModalRef modalRef)
        {
            if (!_modalRefs.Contains(modalRef))
            {
                _modalRefs.Add(modalRef);
            }

            await InvokeAsync(StateHasChanged);
        }

        private async Task ModalService_OnModalCloseEvent(ModalRef modalRef)
        {
            if (modalRef.Config.Visible)
            {
                modalRef.Config.Visible = false;
                await InvokeAsync(StateHasChanged);
                await Task.Delay(250);
                if (modalRef.Config.DestroyOnClose && _modalRefs.Contains(modalRef))
                {
                    _modalRefs.Remove(modalRef);
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        private async Task ModalService_OnModalUpdateEvent(ModalRef arg)
        {
            await InvokeStateHasChangedAsync();
        }


        protected override void Dispose(bool disposing)
        {
            ModalService.OnModalOpenEvent -= ModalService_OnModalOpenEvent;
            ModalService.OnModalCloseEvent -= ModalService_OnModalCloseEvent;
            ModalService.OnModalUpdateEvent -= ModalService_OnModalUpdateEvent;
            NavigationManager.LocationChanged -= OnLocationChanged;

            base.Dispose(disposing);
        }
    }
}
