using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace AntDesign
{
    public abstract class TypographyBase : AntDomComponentBase
    {
        [Inject] public HtmlRenderService Service { get; set; }

        [Parameter] public bool Copyable { get; set; } = false;
        [Parameter] public TypographyCopyableConfig CopyConfig { get; set; }

        [Parameter] public bool Delete { get; set; } = false;

        [Parameter] public bool Disabled { get; set; } = false;

        [Parameter] public bool Editable { get; set; } = false;

        [Parameter] public TypographyEditableConfig EditConfig { get; set; }

        [Parameter] public bool Ellipsis { get; set; } = false;

        [Parameter] public TypographyEllipsisConfig EllipsisConfig { get; set; }

        [Parameter] public bool Mark { get; set; } = false;

        [Parameter] public bool Underline { get; set; } = false;

        [Parameter] public bool Strong { get; set; } = false;

        [Parameter] public Action OnChange { get; set; }

        [Parameter] public string Type { get; set; } = string.Empty;

        [Parameter] public RenderFragment ChildContent { get; set; }

        public async Task Copy()
        {
            if (!Copyable)
            {
                return;
            }
            else if (CopyConfig is null)
            {
                await this.JsInvokeAsync<object>(JSInteropConstants.copy, await Service.RenderAsync(ChildContent));
            }
            else if (CopyConfig.OnCopy is null)
            {
                if (string.IsNullOrEmpty(CopyConfig.Text))
                {
                    await this.JsInvokeAsync<object>(JSInteropConstants.copy, await Service.RenderAsync(ChildContent));
                }
                else
                {
                    await this.JsInvokeAsync<object>(JSInteropConstants.copy, CopyConfig.Text);
                }
            }
            else
            {
                CopyConfig.OnCopy.Invoke();
            }
        }
    }

    public class TypographyCopyableConfig
    {
        public string Text { get; set; } = string.Empty;

        public Action OnCopy { get; set; } = null;
    }

    public class TypographyEditableConfig
    {
        public Action OnStart { get; set; }

        public Action<string> OnChange { get; set; }
    }

    public class TypographyEllipsisConfig
    {
        public string Suffix { get; set; } = "...";

        public int Rows { get; set; }

        public Action OnExpand { get; set; }
    }
}
