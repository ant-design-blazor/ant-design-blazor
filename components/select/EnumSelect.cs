// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

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

        [Parameter]
        public override TEnum Value
        {
            get => base.Value;
            set
            {
                if (EnumHelper<TEnum>.IsFlags)
                {
                    base.Values = EnumHelper<TEnum>.Split(value).ToArray();
                }

                base.Value = value;
            }
        }

        [Parameter]
        public override IEnumerable<TEnum> Values
        {
            get => base.Values;
            set
            {
                if (EnumHelper<TEnum>.IsFlags)
                {
                    base.CurrentValue = (TEnum)EnumHelper<TEnum>.Combine(value) ?? default;
                }

                base.Values = value;
            }
        }

        protected override string GetLabel(TEnum item)
        {
            return EnumHelper<TEnum>.GetDisplayName(item);
        }
    }
}
