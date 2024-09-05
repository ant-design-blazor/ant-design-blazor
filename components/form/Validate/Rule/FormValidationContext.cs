// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign
{
    public class FormValidationContext
    {
        public FormValidateErrorMessages ValidateMessages { get; set; }
        public FormValidationRule Rule { get; set; }
        public object Value { get; set; }
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public Type FieldType { get; set; }

        public object Model { get; set; }
    }
}
