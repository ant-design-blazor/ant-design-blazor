// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign.Docs.Shared
{
    public partial class MockBrowser
    {
        private ClassMapper ClassMapper { get; set; } = new ClassMapper();

        [Parameter]
        public int Height { get; set; }

        [Parameter]
        public string WithUrl { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            ClassMapper.Add("browser-mockup")
                .If("with-url", () => WithUrl != null);
        }
    }
}
