// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class PropertyColumn<TItem, TProp> : Column<TProp>
    {
        /// <summary>
        /// Defines the value to be displayed in this column's cells.
        /// </summary>
        [Parameter] public Expression<Func<TItem, TProp>> Property { get; set; } = default!;

        protected override void OnInitialized()
        {
            if (IsHeader)
            {
                if (Property != null)
                {
                    if (Property.Body is not MemberExpression memberExp)
                    {
                        throw new ArgumentException("'Field' parameter must be child member");
                    }

                    var paramExp = Expression.Parameter(ItemType);
                    var bodyExp = Expression.MakeMemberAccess(paramExp, memberExp.Member);
                    GetFieldExpression = Expression.Lambda(bodyExp, paramExp);
                }
            }
            base.OnInitialized();
        }
    }
}
