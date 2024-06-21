using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ComfirmContainer
    {
        [Inject]
        private ModalService ModalService { get; set; }

        [Inject]
        private ConfirmService ConfirmService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private readonly List<ConfirmRef> _confirmRefs = new List<ConfirmRef>();

        #region override

        /// <summary>
        /// Registration events
        /// </summary>
        protected override void OnInitialized()
        {
            ModalService.OnConfirmOpenEvent += OnConfirmOpen;
            ModalService.OnConfirmCloseEvent += OnConfirmClose;
            ModalService.OnConfirmCloseAllEvent += OnConfirmCloseAll;
            ModalService.OnConfirmUpdateEvent += OnConfirmUpdate;

            ConfirmService.OnOpenEvent += OnConfirmOpen;

            NavigationManager.LocationChanged += OnLocationChanged;
        }

        #endregion

        private void OnLocationChanged(object sender, EventArgs e)
        {
            _confirmRefs.Clear();
            InvokeStateHasChanged();
        }

        /// <summary>
        /// create and open a Confirm dialog
        /// </summary>
        private async Task OnConfirmOpen(ConfirmRef confirmRef)
        {
            confirmRef.Config.Visible = true;
            if (!_confirmRefs.Contains(confirmRef))
            {
                confirmRef.Config.BuildButtonsDefaultOptions();
                _confirmRefs.Add(confirmRef);
            }
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// update Confirm dialog
        /// </summary>
        /// <param name="confirmRef"></param>
        /// <returns></returns>
        private async Task OnConfirmUpdate(ConfirmRef confirmRef)
        {
            if (confirmRef.Config.Visible)
            {
                await InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// destroy Confirm dialog
        /// </summary>
        /// <param name="confirmRef"></param>
        /// <returns></returns>
        private async Task OnConfirmClose(ConfirmRef confirmRef)
        {
            if (confirmRef.Config.Visible)
            {
                confirmRef.Config.Visible = false;
                await InvokeAsync(StateHasChanged);
                if (confirmRef.OnClose != null)
                {
                    await confirmRef.OnClose.Invoke();
                }
            }
        }

        /// <summary>
        /// after Confirm dialog remove from DOM, to remove it from _confirmRefs 
        /// </summary>
        /// <param name="confirmRef"></param>
        /// <returns></returns>
        private Task OnConfirmRemove(ConfirmRef confirmRef)
        {
            if (_confirmRefs.Contains(confirmRef))
            {
                _confirmRefs.Remove(confirmRef);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// destroy all Confirm dialog
        /// </summary>
        /// <returns></returns>
        private async Task OnConfirmCloseAll()
        {
            // avoid iterations the change of _confirmRefs affects the iterative process
            var confirmRefsTemp = new List<ConfirmRef>(_confirmRefs);
            foreach (var confirmRef in confirmRefsTemp)
            {
                await OnConfirmClose(confirmRef);
            }
        }

        /// <summary>
        /// Unregister events
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            ModalService.OnConfirmOpenEvent -= OnConfirmOpen;
            ModalService.OnConfirmCloseEvent -= OnConfirmClose;
            ModalService.OnConfirmCloseAllEvent -= OnConfirmCloseAll;
            ModalService.OnConfirmUpdateEvent -= OnConfirmUpdate;

            ConfirmService.OnOpenEvent -= OnConfirmOpen;
            NavigationManager.LocationChanged -= OnLocationChanged;

            base.Dispose(disposing);
        }
    }
}
