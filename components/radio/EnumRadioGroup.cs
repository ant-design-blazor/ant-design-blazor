﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntDesign
{
    public partial class EnumRadioGroup<TEnum> : RadioGroup<TEnum>
    {
        public EnumRadioGroup()
        {
            Options = EnumHelper<TEnum>.GetValueLabelList()
                .Select(x => new RadioOption<TEnum> { Value = x.Value, Label = x.Label })
                .ToArray();
        }
    }
}
