// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public enum FormValidateMode
    {
        /// <summary>
        /// Validation will come from data attributes on the model provided to <see cref="Form{TModel}.Model"/>
        /// </summary>
        Default = 0,

        /// <summary>
        /// Validation will come from rules on FormItems in the form
        /// </summary>
        Rules = 1,

        /// <summary>
        /// Use both Default mode and Rules mode
        /// </summary>
        Complex = 3
    }
}
