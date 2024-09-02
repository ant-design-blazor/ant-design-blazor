// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class CardGrid : AntDomComponentBase
    {
        /// <summary>
        /// Content for that section of the grid
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// If the section should be hoverable. Adds dropshadow and lifts up when hovered if true.
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Hoverable { get; set; }

        [CascadingParameter]
        private Card Parent { get; set; }

        protected override void OnInitialized()
        {
            Parent?.MarkHasGrid();

            ClassMapper.Add("ant-card-grid")
                .If("ant-card-hoverable", () => Hoverable);

            base.OnInitialized();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
