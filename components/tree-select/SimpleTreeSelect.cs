// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class SimpleTreeSelect<TItem> : TreeSelect<TItem> where TItem : class
    {
        protected IEnumerable<TItem> RootData => ChildrenMethodExpression?.Invoke(DataSource, RootValue);

        /// <summary>
        /// Specifies a method  to return a child node
        /// </summary>
        [Parameter]
        public Func<IEnumerable<TItem>, string, IList<TItem>> ChildrenMethodExpression { get; set; }

        //protected override Func<TItem, IEnumerable<TItem>> TreeNodeChildrenExpression => node => ChildrenMethodExpression(DataSource, TreeNodeKeyExpression(node));
    }
}
