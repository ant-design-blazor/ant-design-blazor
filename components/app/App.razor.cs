// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public partial class App : AntContainerComponentBase
    {
        [Parameter] public Type Component { get; set; }

        protected override void OnInitialized()
        {
            ClassMapper.Add("ant-app");

            base.OnInitialized();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (Component != null)
            {
                builder.OpenComponent(0, Component);
            }
            else
            {
                builder.OpenElement(0, "div");
            }

            builder.AddAttribute(1, "class", ClassMapper.Class);
            builder.AddAttribute(2, "style", Style);
            builder.AddAttribute(3, "id", Id);

            if (Component != null)
            {
                builder.AddAttribute(4, nameof(RefBack), Ref);
                builder.CloseComponent();
            }
            else
            {
                builder.AddElementReferenceCapture(4, @ref => Ref = @ref);
                builder.CloseElement();
            }

            base.BuildRenderTree(builder);
        }
    }
}
