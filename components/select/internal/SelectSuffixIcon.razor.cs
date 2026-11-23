// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign.Select.Internal
{
    public partial class SelectSuffixIcon<TItemValue, TItem>
    {
        [Parameter]
        public RenderFragment SuffixIcon { get; set; }

        [Parameter]
        public RenderFragment SearchIcon { get; set; }

        [Parameter] public bool IsOverlayShow { get; set; }

        /// <summary>
        /// Whether show search input in single mode.
        /// </summary>
        [Parameter]
        public bool ShowSearchIcon { get; set; }

        [Parameter]
        public bool ShowArrowIcon { get; set; }
        
        [Parameter]
        public bool Loading { get; set; }

        [Parameter] public bool ShowClearIcon { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClearClick { get; set; }

        [CascadingParameter(Name = "ParentSelect")] internal SelectBase<TItemValue, TItem> ParentSelect { get; set; }

        [CascadingParameter(Name = "LoadingIcon")] internal RenderFragment LoadingIcon { get; set; }

    }
}
