// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public interface IAutoCompleteInput
    {
        [CascadingParameter]
        public IAutoCompleteRef Component { get; set; }
        public void SetValue(object value);
    }
}
