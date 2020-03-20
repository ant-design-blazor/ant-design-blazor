using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace AntBlazor
{
    public partial class AntTextArea : AntInput
    {
        private RenderFragment _hidden;

        private ElementReference _hiddenEle;

        [Parameter]
        public bool autoSize { get; set; }

        [Parameter]
        public EventCallback<object> onResize { get; set; }

        protected override async void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override async void OnInputAsync(ChangeEventArgs args)
        {
            // do not call base method to avoid lost focus
            //base.OnInputAsync(args);

            if (autoSize)
            {
                await ChangeSizeAsync();
            }
        }

        private async Task ChangeSizeAsync()
        {
            // Ant-design use a hidden textarea to calculate row height, totalHeight = rows * rowHeight
            // TODO: compare with maxheight
            Element element = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, inputEl);
            Style = $"height: {element.scrollHeight}px;overflow-y: hidden;";
        }

        protected async Task OnResizeAsync()
        {

        }
    }
}
