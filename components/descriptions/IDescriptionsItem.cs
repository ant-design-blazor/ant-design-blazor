// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public interface IDescriptionsItem
    {
        internal ElementReference Ref { get; set; }

        string Title { get; set; }

        RenderFragment TitleTemplate { get; set; }

        int Span { get; set; }

        RenderFragment ChildContent { get; set; }

        public string LabelStyle { get; set; }

        public string ContentStyle { get; set; }

        public bool Colon { get; set; }

        public string Layout { get; set; }
    }
}
