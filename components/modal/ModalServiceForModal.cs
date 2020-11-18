using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    public partial class ModalService : IDisposable
    {
        internal event Func<ModalRef, Task> OnModalOpenEvent;

        internal event Func<ModalRef, Task> OnModalCloseEvent;

        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _jsRuntime;
        /// <summary>
        /// constructor
        /// </summary>
        public ModalService(NavigationManager navigationManager, IJSRuntime jsRuntime)
        {
            _navigationManager = navigationManager;
            _navigationManager.LocationChanged += NavigationManager_LocationChanged;
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Destroy all reused Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void NavigationManager_LocationChanged(object sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            if (Modal.ReusedModals.Count > 0)
            {
                // Since Modal cannot be captured, it can only be removed through JS
                await _jsRuntime.InvokeVoidAsync(JSInteropConstants.DestroyAllDialog);
                Modal.ReusedModals.Clear();
            }
        }

        /// <summary>
        /// Create and open a Modal
        /// </summary>
        /// <returns></returns>
        public Task<ModalRef> CreateModalAsync(ModalOptions config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            ModalRef modalRef = new ModalRef(config, this);
            config.ModalRef = modalRef;
            return CreateOrOpenModalAsync(modalRef);
        }

        /// <summary>
        /// Create and open template modal
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TComponentOptions"></typeparam>
        /// <param name="config"></param>
        /// <param name="componentOptions"></param>
        /// <returns></returns>
        public Task<ModalRef> CreateModalAsync<TComponent, TComponentOptions>(ModalOptions config, TComponentOptions componentOptions) where TComponent : ModalTemplate<TComponentOptions>
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            ModalRef modalRef = new ModalRef(config, this);

            void Child(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
            {
                builder.OpenComponent<TComponent>(0);
                builder.AddAttribute(1, "ModalRef", modalRef);
                builder.AddAttribute(2, "Options", componentOptions);
                builder.CloseComponent();
            }
            config.Content = Child;
            config.ModalRef = modalRef;
            return CreateOrOpenModalAsync(modalRef);
        }

        internal Task<ModalRef> CreateOrOpenModalAsync(ModalRef modalRef)
        {
            OnModalOpenEvent?.Invoke(modalRef);
            return Task.FromResult(modalRef);
        }

        internal Task CloseModalAsync(ModalRef modalRef)
        {
            if (OnModalCloseEvent != null)
            {
                return OnModalCloseEvent.Invoke(modalRef);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Implement the interface IDisposable
        /// </summary>
        public void Dispose()
        {
            _navigationManager.LocationChanged -= NavigationManager_LocationChanged;
            GC.SuppressFinalize(this);
        }
    }
}
