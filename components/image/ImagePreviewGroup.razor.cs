// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ImagePreviewGroup : IDisposable
    {
        /// <summary>
        /// Content for group. Typically contains <see cref="Image"/> elements.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Whether to open the preview image. Two-way binding.	
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool PreviewVisible
        {
            get => _previewVisible;
            set
            {
                if (_previewVisible != value)
                {
                    _previewVisible = value;
                    HandleVisibleChange(_previewVisible);
                }
            }
        }

        /// <summary>
        /// Callback executed when <see cref="PreviewVisible"/> changes
        /// </summary>
        [Parameter]
        public EventCallback<bool> PreviewVisibleChanged { get; set; }

        [Inject]
        private ImageService ImageService { get; set; }

        internal IList<Image> Images => _images;

        private IList<Image> _images;

        private ImageRef _imageRef;
        private bool _previewVisible = true;

        public void AddImage(Image image)
        {
            _images ??= new List<Image>();
            _images.Add(image);
        }

        public void Remove(Image image)
        {
            _images.Remove(image);
        }

        public void HandleVisibleChange(bool visible)
        {
            if (visible)
            {
                _imageRef = ImageService.OpenImages(_images);
                if (_imageRef == null)
                {
                    return;
                }
                _imageRef.SwitchTo(0);
                _imageRef.OnClosed += OnPreviewClose;
            }
            else
            {
                if (_imageRef != null)
                {
                    _imageRef.OnClosed -= OnPreviewClose;
                    ImageService.CloseImage(_imageRef);
                }
            }

            if (PreviewVisibleChanged.HasDelegate)
            {
                PreviewVisibleChanged.InvokeAsync(visible);
            }
        }

        private void OnPreviewClose()
        {
            PreviewVisible = false;
        }

        public void Dispose()
        {
            if (_imageRef != null)
            {
                _imageRef.OnClosed -= OnPreviewClose;
            }
        }
    }
}
