// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
#pragma warning disable 1591 // Disable missing XML comment

    public class Properties
    {
        public string Value { get; set; }

        public string Label { get; set; }

        public bool Closable { get; set; }

        public Action<MouseEventArgs> OnClose { get; set; }
    }
}
