// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign.Core.JsInterop.ObservableApi;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign.Core.Component.ResizeObserver
{
    public partial class ResizeObserver : AntComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public EventCallback<DomRect> OnResize { get; set; }

        [Inject] public DomEventService DomEventService { get; set; }

        private IDomEventListener _domEventListener;

        protected override void OnInitialized()
        {
            _domEventListener = DomEventService.CreateDomEventListerner();
            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (RefBack.Current.Id != null)
                {
                    await _domEventListener.AddResizeObserver(RefBack.Current, OnResizeCallback);
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        protected virtual void OnResizeCallback(List<ResizeObserverEntry> entries)
        {
            if (OnResize.HasDelegate)
            {
                OnResize.InvokeAsync(entries[0].ContentRect);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _domEventListener?.Dispose();

            base.Dispose(disposing);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.AddContent(0, ChildContent);
        }
    }
}
