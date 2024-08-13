// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.Core.Extensions
{
    internal static class ObjectExtensions
    {
        public static void SetValue(this object obj, string name, object value)
        {
            var property = obj.GetType().GetProperty(name);
            property?.SetValue(obj, value);
        }

        public static bool AllNullOrEquals(this object obj, object other)
        {
            if (obj == null && other == null) return true;

            if (obj == null || other == null) return false;

            return obj.Equals(other);
        }
    }
}
