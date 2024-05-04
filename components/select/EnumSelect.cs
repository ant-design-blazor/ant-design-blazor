// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntDesign;
using AntDesign.Select.Internal;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Reflection;

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

        protected override void OnInitialized()
        {
            if (EnableFlags)
            {
                var attr = typeof(TEnum).GetCustomAttribute<FlagsAttribute>();
                if (attr != null)
                {
                    Mode = SelectModeExtensions.Multiple;
                }
                else
                {
                    EnableFlags = false;
                }
            }
            base.OnInitialized();
        }

        /// <summary>
        /// Set to true to support using '@bind-Value' instead of '@bind-Values' to get or set the values of multiple options
        /// </summary>
        [Parameter]
        public bool EnableFlags { get; set; } = false;

        /// <summary>
        /// Get or set the selected value.
        /// </summary>
        [Parameter]
        public override TEnum Value
        {
            get => base.Value;
            set
            {
                if (EnableFlags)
                {
                    List<TEnum> values = [];
                    var val = Convert.ToUInt64(value);
                    foreach (var item in DataSource)
                    {
                        var item_val = Convert.ToUInt64(item);
                        if ((item_val & val) == item_val)
                        {
                            values.Add(item);
                        }
                    }
                    base.Values = [.. values];
                }
                base.Value = value;
            }
        }

        /// <summary>
        /// Get or set the selected values.
        /// </summary>
        [Parameter]
        public override IEnumerable<TEnum> Values
        {
            get => base.Values;
            set
            {
                if (EnableFlags)
                {
                    var oldVal = Convert.ToUInt64(base.Value);
                    ulong newVal = 0;
                    foreach (var item in value)
                    {
                        newVal += Convert.ToUInt64(item);
                    }
                    if (newVal == 0)
                        base.Value = default;
                    else
                        base.Value = THelper.ChangeType<TEnum>(newVal);
                    if (oldVal != newVal)
                    {
                        ValueChanged.InvokeAsync(base.Value);
                    }
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
