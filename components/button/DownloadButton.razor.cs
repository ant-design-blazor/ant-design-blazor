// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace AntDesign
{
    /// <summary>
    /// Use to download a file. inherits <see cref="Button"/>
    /// </summary>
    [Documentation(DocumentationCategory.Components, DocumentationType.General, null)]
    public partial class DownloadButton : Button
    {
        /// <summary>
        /// The download url of a file
        /// </summary>
        [Parameter]
        public string Url { get; set; }

        /// <summary>
        /// Name of the file
        /// </summary>
        [Parameter]
        public string FileName { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        #region Hidden properties/methods of the parent

        //public new EventCallback<MouseEventArgs> OnClick { get; set; }

        //public new bool OnClickStopPropagation { get; set; }

        #endregion Hidden properties/methods of the parent

        protected override void OnInitialized()
        {
            base.OnInitialized();
            base.OnClick = new(null, TriggerDownloadAsync);
        }

        private ValueTask TriggerDownloadAsync(MouseEventArgs _)
        {
            if (string.IsNullOrEmpty(Url))
            {
                Console.WriteLine($"{nameof(DownloadButton)}.{nameof(TriggerDownloadAsync)}: Empty URL");
#if NET5_0_OR_GREATER
                return ValueTask.CompletedTask;
#else
                return default;
#endif
            }
            return JSRuntime.InvokeVoidAsync(JSInteropConstants.TriggerFileDownload, FileName, Url);
        }
    }
}
