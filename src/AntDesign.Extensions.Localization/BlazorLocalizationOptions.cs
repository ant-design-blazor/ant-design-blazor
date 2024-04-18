// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Localization;

namespace AntDesign.Extensions.Localization
{
    public class BlazorLocalizationOptions : LocalizationOptions
    {
        public Assembly? ResourcesAssembly { get; set; }
    }
}
