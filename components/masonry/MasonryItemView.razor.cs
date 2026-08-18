// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    internal interface IMasonryItemViewParent
    {
        void SetItemRef(object key, ElementReference element);

        void OnItemViewDisposed(object key);
    }

    public partial class MasonryItemView : ComponentBase, IDisposable
    {
        [Parameter] public string Class { get; set; }

        [Parameter] public string Style { get; set; }

        [Parameter] public RenderFragment Content { get; set; }

        [Parameter] public object ItemKey { get; set; }

        [CascadingParameter] private IMasonryItemViewParent Parent { get; set; }

        private ElementReference _elementReference;
        private bool _disposed;

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            Parent?.SetItemRef(ItemKey, _elementReference);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            Parent?.OnItemViewDisposed(ItemKey);
            GC.SuppressFinalize(this);
        }
    }
}
