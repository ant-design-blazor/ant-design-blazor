using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

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
                Dictionary<string, object> attrDict = new Dictionary<string, object>();
                attrDict.Add("class", "ant-input-search-icon");
                suffix = BuildAntIcon("search", attrDict);
            }
            else
            {
                if (Attributes["enterButton"].ToString() == "Search")
                {
                    addOnAfter = new RenderFragment((builder) =>
                    {
                        //<AntButton type="primary" icon="search" onlick="Search" />Search</AntButton>
                        builder.OpenComponent<AntButton>(0);
                        builder.AddAttribute(1, "class", "ant-input-search-button");
                        builder.AddAttribute(2, "type", "primary");
                        builder.AddContent(3, "Search");
                        builder.CloseComponent();
                    });

                    StateHasChanged();
                }
                else
                {
                    addOnAfter = new RenderFragment((builder) =>
                    {
                        //<AntButton type="primary" icon="search" /></AntButton>
                        builder.OpenComponent<AntButton>(0);
                        builder.AddAttribute(1, "class", "ant-input-search-button");
                        builder.AddAttribute(2, "type", "primary");
                        builder.AddAttribute(3, "icon", "search");
                        builder.CloseComponent();
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