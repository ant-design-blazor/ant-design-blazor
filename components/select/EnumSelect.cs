// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public class EnumSelect<TEnum> : Select<TEnum, TEnum>
    {
        public EnumSelect()
        {
            if (THelper.GetUnderlyingType<TEnum>().IsEnum)
            {
                DataSource = EnumHelper<TEnum>.GetValueList();
            }
        }

        protected override string GetLabel(TEnum item)
        {
            return EnumHelper<TEnum>.GetDisplayName(item);
        }
    }
}
