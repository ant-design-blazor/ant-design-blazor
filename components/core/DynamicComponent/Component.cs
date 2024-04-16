// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public class Component : ComponentBase
    {
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
        static Assembly _antAssembly;

        [Parameter]
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
        public Type Type { get; set; }

        [Parameter]

        public string TypeName { get; set; }

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

        public override Task SetParametersAsync(ParameterView parameters)
        {
            if (parameters.IsParameterChanged(nameof(TypeName), TypeName)) {
                SetupAssembly(parameters.GetValueOrDefault<string>(nameof(TypeName)));
            }
            return base.SetParametersAsync(parameters);
        }

#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
#endif
        private void SetupAssembly(string value)
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
}
