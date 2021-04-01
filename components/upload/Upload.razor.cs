using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    public partial class Upload : AntDomComponentBase
    {
        [Parameter]
        public Func<UploadFileItem, bool> BeforeUpload { get; set; }

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public string Action { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public Dictionary<string, object> Data { get; set; }

        [Parameter]
        public string ListType { get; set; } = "text";

        [Parameter]
        public bool Directory { get; set; }

        [Parameter]
        public bool Multiple { get; set; }

        [Parameter]
        public string Accept { get; set; }

        [Parameter]
        public bool ShowUploadList { get; set; } = true;

        [Parameter]
        public List<UploadFileItem> FileList { get; set; } = new List<UploadFileItem>();

        [Parameter]
        public UploadLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Upload;

        [Parameter]
        public EventCallback<List<UploadFileItem>> FileListChanged { get; set; }

        [Parameter]
        public List<UploadFileItem> DefaultFileList { get; set; } = new List<UploadFileItem>();

        [Parameter]
        public Dictionary<string, string> Headers { get; set; }

        [Parameter]
        public EventCallback<UploadInfo> OnSingleCompleted { get; set; }

        [Parameter]
        public EventCallback<UploadInfo> OnCompleted { get; set; }

        [Parameter]
        public EventCallback<UploadInfo> OnChange { get; set; }

        [Parameter]
        public Func<UploadFileItem, Task<bool>> OnRemove { get; set; }

        [Parameter]
        public EventCallback<UploadFileItem> OnPreview { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool ShowButton { get; set; } = true;

        private bool IsText => ListType == "text";
        private bool IsPicture => ListType == "picture";
        private bool IsPictureCard => ListType == "picture-card";

        private ClassMapper _listClassMapper = new ClassMapper();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var prefixCls = "ant-upload";

            ClassMapper
                .Add(prefixCls)
                .If($"{prefixCls}-rtl", () => RTL);

            _listClassMapper
                .Add($"{prefixCls}-list")
                .Get(() => $"{prefixCls}-list-{ListType}")
                .If($"{prefixCls}-list-rtl", () => RTL);
        }

        protected override Task OnInitializedAsync()
        {
            FileList.InsertRange(0, DefaultFileList);
            return base.OnInitializedAsync();
        }

        private async Task RemoveFile(UploadFileItem item)
        {
            var canRemove = OnRemove == null || await OnRemove.Invoke(item);
            if (canRemove)
            {
                this.FileList.Remove(item);
                await this.FileListChanged.InvokeAsync(this.FileList);

                StateHasChanged();
            }
        }

        private async Task PreviewFile(UploadFileItem item)
        {
            if (item.State == UploadState.Success && OnPreview.HasDelegate)
            {
                await OnPreview.InvokeAsync(item);
            }
        }
    }
}
