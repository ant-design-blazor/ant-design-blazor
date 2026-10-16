// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace AntDesign
{
    public class AdvancedFilterInputResolver
    {
        internal IList<AdvancedFilterInputRule> Rules { get; } = new List<AdvancedFilterInputRule>
        {
            new() { PropertyType = IsDate, CompareOperator = op => op == TableFilterCompareOperator.Between, InputKind = AdvancedFilterValueInputKind.RangePicker },
            new() { PropertyType = IsDate, InputKind = AdvancedFilterValueInputKind.DatePicker },
            new() { PropertyType = field => field.UnderlyingType == typeof(bool), InputKind = AdvancedFilterValueInputKind.Switch },
            new() { PropertyType = field => field.UnderlyingType.IsEnum, InputKind = AdvancedFilterValueInputKind.EnumSelect },
            new() { PropertyType = field => field.UnderlyingType.IsNumericType(), InputKind = AdvancedFilterValueInputKind.Number },
            new() { PropertyType = field => field.UnderlyingType == typeof(string), InputKind = AdvancedFilterValueInputKind.Text },
            new() { PropertyType = field => field.UnderlyingType == typeof(Guid), InputKind = AdvancedFilterValueInputKind.Text },
        };

        public AdvancedFilterInputResolver Map(
            Func<FilterFieldDescriptor, bool> propertyType,
            Func<TableFilterCompareOperator, bool> compareOperator,
            AdvancedFilterValueInputKind inputKind)
        {
            Rules.Insert(0, new AdvancedFilterInputRule
            {
                PropertyType = propertyType,
                CompareOperator = compareOperator,
                InputKind = inputKind,
            });

            return this;
        }

        public virtual AdvancedFilterValueInputKind Resolve(FilterFieldDescriptor field, TableFilterCompareOperator compareOperator)
        {
            if (field == null)
                return AdvancedFilterValueInputKind.Auto;

            if (field.CustomInput != null)
                return AdvancedFilterValueInputKind.Custom;

            if (field.InputComponent.HasValue && field.InputComponent.Value != AdvancedFilterValueInputKind.Auto)
                return field.InputComponent.Value;

            if (field.ValueOptions?.Count > 0)
                return AdvancedFilterValueInputKind.Select;

            return Rules.FirstOrDefault(rule => rule.Matches(field, compareOperator))?.InputKind
                ?? AdvancedFilterValueInputKind.Auto;
        }

        private static bool IsDate(FilterFieldDescriptor field)
        {
            return field?.UnderlyingType?.IsDateType() == true;
        }
    }
}
