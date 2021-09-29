// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public class PaginationOptions : IComponent
    {
        [Parameter]
        public int Total { get; set; }

        [Parameter]
        public int DefaultCurrent { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public int Current { get; set; }

        [Parameter]
        public int DefaultPageSize { get; set; }

        [Parameter]
        public int PageSize { get; set; }

        [Parameter]
        public EventCallback<PaginationEventArgs> OnChange { get; set; }

        [Parameter]
        public bool HideOnSinglePage { get; set; }

        [Parameter]
        public bool ShowSizeChanger { get; set; }

        [Parameter]
        public int[] PageSizeOptions { get; set; }

        [Parameter]
        public EventCallback<PaginationEventArgs> OnShowSizeChange { get; set; }

        [Parameter]
        public bool ShowQuickJumper { get; set; } = false;

        [Parameter]
        public RenderFragment GoButton { get; set; }

        [Parameter]
        public bool ShowTitle { get; set; } = true;

        [Parameter]
        public OneOf<Func<PaginationTotalContext, string>, RenderFragment<PaginationTotalContext>>? ShowTotal { get; set; }

        [Parameter]
        public string Size { get; set; }

        [Parameter]
        public bool Responsive { get; set; }

        [Parameter]
        public bool Simple { get; set; }

        [Parameter]
        public PaginationLocale Locale { get; set; }

        [Parameter]
        public RenderFragment<PaginationItemRenderContext> ItemRender { get; set; }

        [Parameter]
        public bool ShowLessItems { get; set; }

        [Parameter]
        public bool ShowPrevNextJumpers { get; set; }

        [Parameter]
        public string Direction { get; set; }

        [Parameter]
        public RenderFragment<PaginationItemRenderContext> PrevIcon { get; set; }

        [Parameter]
        public RenderFragment<PaginationItemRenderContext> NextIcon { get; set; }

        [Parameter]
        public RenderFragment<PaginationItemRenderContext> JumpPrevIcon { get; set; }

        [Parameter]
        public RenderFragment<PaginationItemRenderContext> JumpNextIcon { get; set; }

        [Parameter]
        public int TotalBoundaryShowSizeChanger { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public string Style { get; set; }

        [CascadingParameter(Name = "AntDesign.PaginationOptions.PaginationHost")]
        public IPaginationHost PaginationHost { get; set; }

        public void Attach(RenderHandle renderHandle)
        {
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            if (parameters.TryGetValue(nameof(PaginationHost), out IPaginationHost host))
            {
                host.PaginationAttributes = parameters.ToDictionary().ToDictionary(x => x.Key, x => x.Value);
                host.PaginationAttributes.Remove(nameof(PaginationHost));
            }
            return Task.CompletedTask;
        }
    }
}
