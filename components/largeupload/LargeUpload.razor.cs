// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    public partial class LargeUpload
    {
        /// <summary>
        /// 上传到的服务器地址
        /// </summary>
        [Parameter]
        public string ServerUrl { get; set; }
        [Inject]
        public IJSRuntime JsRuntime { get; set; }
        /// <summary>
        /// 上传文件的
        /// </summary>
        internal ElementReference InputFileElement { get; set; }
        /// <summary>
        /// 进度条的
        /// </summary>
        internal ElementReference ProgressElement { get; set; }
        /// <summary>
        /// 上传完成回调
        /// </summary>
        [Parameter]
        public EventCallback<string> UploadFinish { get; set; }
        [Parameter]
        public double UploadPercent { get; set; }
        public async Task StartUpload()
        {
            await JsRuntime.InvokeVoidAsync("largeStart", InputFileElement, ProgressElement, ServerUrl, DotNetObjectReference.Create(this));
        }
        [JSInvokable("OnUploadSuccess")]
        public Task OnUploadSuccess(string file)
        {
            return UploadFinish.InvokeAsync(file);
        }
        [JSInvokable("ShowPercent")]
        public void ShowPercent(double jd)
        {
            UploadPercent = jd;
        }
    }
}
