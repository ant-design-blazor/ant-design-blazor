// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public class EnumSelect<TEnum> : Select<TEnum, EnumLabelValue<TEnum>>
    {
        public EnumSelect()
        {
            if (typeof(TEnum).IsEnum)
            {
                DataSource = EnumHelper<TEnum>.GetLabelValueList();
                LabelName = nameof(EnumLabelValue<TEnum>.Label);
                ValueName = nameof(EnumLabelValue<TEnum>.Value);
            }
        }
    }
}
