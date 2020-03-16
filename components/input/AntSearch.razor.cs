using Microsoft.AspNetCore.Components;
using System;
namespace AntBlazor
{
    public partial class AntSearch : AntInputBase
    {
        [Parameter]
        public EventCallback<EventArgs> onSearch { get; set; }

        protected override void SetClasses()
        {
            base.SetClasses();

            if (!Attributes.ContainsKey("enterButton"))
            {
                suffix = "search";
            }
            else
            {
                if (Attributes["enterButton"].ToString() == "Search")
                {
                    AddOnAfter = new RenderFragment((builder) =>
                    {
                        //<AntButton type="primary" icon="search" /></AntButton>
                        builder.OpenElement(0, "AntButton");
                        builder.AddAttribute(1, "type", "primary");
                        builder.AddContent(2, "Search");
                        builder.CloseElement();
                    });

                    StateHasChanged();
                }
                else
                {
                    AddOnAfter = new RenderFragment((builder) =>
                    {
                        //<AntButton type="primary" icon="search" /></AntButton>
                        builder.OpenElement(0, "AntButton");
                        builder.AddAttribute(1, "type", "primary");
                        builder.AddAttribute(2, "icon", "search");
                        builder.CloseElement();
                    });

                    StateHasChanged();
                }
            }
        }

        private async void Search()
        {
            if (onSearch.HasDelegate)
            {
                await onSearch.InvokeAsync(EventArgs.Empty);
            }
        }
    }
}
