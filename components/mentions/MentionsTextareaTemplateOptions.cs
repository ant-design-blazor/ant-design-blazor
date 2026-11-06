// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class MentionsTextareaTemplateOptions
    {
        public string Value { get; set; }
        public ForwardRef RefBack { get; set; }
        public Func<KeyboardEventArgs, Task> OnKeyDown { get; set; }
        public Func<ChangeEventArgs, Task> OnInput { get; set; }
    }
}
