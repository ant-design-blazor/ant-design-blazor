// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public class Component : ComponentBase
    {
        private static Assembly _antAssembly;

        [Parameter]
        public Type Type { get; set; }

        [Parameter]
        public string TypeName
        {
#if NET5_0_OR_GREATER
            [RequiresUnreferencedCode("Component uses Type.GetType for dynamic component loading which is not trim-safe")]
#endif
            set
            {
                if (Type != null) return;
                _antAssembly ??= Assembly.GetExecutingAssembly();
                Type componentType =
                    _antAssembly.GetType($"AntDesign.{value}") ??
                    _antAssembly.GetType(value) ??
                    Type.GetType(value);
                if (componentType == null)
                {
                    throw new ArgumentException($"Not found the component with the name \"{value}\"", nameof(TypeName));
                }
                Type = componentType;
            }
        }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> Parameters { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent(0, Type);
            if (Parameters != null)
            {
                builder.AddMultipleAttributes(1, Parameters);
            }
            builder.CloseComponent();
        }
    }
}
