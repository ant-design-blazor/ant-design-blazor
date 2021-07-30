// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign.FilterExpression
{
    public class FilterExpressionResolver<T>
    {
        private readonly StringFilterExpression _stringFilter = new StringFilterExpression();
        private readonly NumberFilterExpression _numberFilter = new NumberFilterExpression(typeof(T));
        private readonly DateFilterExpression _dateFilter = new DateFilterExpression();
        private readonly EnumFilterExpression _enumFilter = new EnumFilterExpression();
        private readonly GuidFilterExpression _guidFilter = new GuidFilterExpression();

        public FilterExpressionResolver()
        {
        }

        public IFilterExpression GetFilterExpression()
        {
            if (THelper.GetUnderlyingType<T>().IsEnum)
            {
                return _enumFilter;
            }
            if (THelper.IsNumericType<T>())
            {
                return _numberFilter;
            }
            var underlyingType = THelper.GetUnderlyingType<T>();
            if (underlyingType == typeof(DateTime))
            {
                return _dateFilter;
            }
            if (underlyingType == typeof(string))
            {
                return _stringFilter;
            }
            if (underlyingType == typeof(Guid))
            {
                return _guidFilter;
            }
            throw new NotImplementedException();
        }
    }
}
