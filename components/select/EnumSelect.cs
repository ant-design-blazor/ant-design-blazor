// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;

namespace AntDesign
{
    public class EnumSelect<TEnum> : Select<TEnum, EnumLabelValue<TEnum>>
    {
        public EnumSelect()
        {
            if (typeof(TEnum).IsEnum)
            {
                DataSource = Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Select(t => new EnumLabelValue<TEnum>(Enum.GetName(typeof(TEnum), t), t))
                .OrderBy(t => t.Value)
                .ToList();
                LabelName = nameof(EnumLabelValue<TEnum>.Label);
                ValueName = nameof(EnumLabelValue<TEnum>.Value);
            }
        }
    }
}
