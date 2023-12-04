// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign.Form.Dynamic
{
#if NET6_0_OR_GREATER
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DateTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTypeAttribute"/> class.
        /// </summary>
        /// <param name="inputDateType">
        /// The type of the date input.
        /// this determines the underlying HTML type attribute on the input tag.
        /// </param>
        public DateTypeAttribute(InputDateType inputDateType) => InputDateType = inputDateType;

        /// <summary>
        /// Gets the type of the date input.
        /// </summary>
        public InputDateType InputDateType { get; }
    }
#endif
}
