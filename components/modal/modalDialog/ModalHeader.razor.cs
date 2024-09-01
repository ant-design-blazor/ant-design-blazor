// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign.Internal.ModalDialog
{
    public partial class ModalHeader
    {
        [CascadingParameter]
        public DialogOptions Config { get; set; }

        [CascadingParameter(Name = "Parent")]
        public Dialog Parent { get; set; }
    }
}
