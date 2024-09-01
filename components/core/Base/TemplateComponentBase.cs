// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TComponentOptions"></typeparam>
    public abstract class TemplateComponentBase<TComponentOptions> : AntComponentBase
    {
        /// <summary>
        /// The options that allow you to pass in templates from the outside
        /// </summary>
        [Parameter]
        public TComponentOptions Options { get; set; }
    }
}
