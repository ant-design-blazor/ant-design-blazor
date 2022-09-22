﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Core.Extensions
{
    public static class ElementReferenceExtensions
    {
        public static string GetSelector(this ElementReference? elementReference)
        {
            return elementReference?.GetSelector();
        }

        public static string GetSelector(this ElementReference elementReference)
        {
#if NET5_0_OR_GREATER
            if (elementReference.Context is null)
            {
                return null;
            }

            return $"[_bl_{elementReference.Id}]";
#endif
            return $"[_bl_{elementReference.Id}]";
        }
    }
}
