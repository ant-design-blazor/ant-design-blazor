// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace AntDesign
{
    public class DownloadButton : Button
    {
        [Inject]
        protected IJSRuntime IJSRuntime { get; set; }

        /// <summary>
        /// Relative path of the file
        /// </summary>
        [Parameter]
        public string FilePath { get; set; } = "";

        /// <summary>
        /// File name exported to browser
        /// </summary>
        [Parameter]
        public string ExportFileName { get; set; } = "";
        protected override async Task HandleOnClick(MouseEventArgs args)
        {
            try
            {
                await IJSRuntime.InvokeVoidAsync("downloadFile", ExportFileName,FilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                base.HandleOnClick(args);
                await OnClick.InvokeAsync(args);
            }
        }
    }
}
