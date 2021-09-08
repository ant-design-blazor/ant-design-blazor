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


        protected IEnumerable<TItem> RootData => ChildrenMethodExpression?.Invoke(RootValue);


        /// <summary>
        /// Specifies a method  to return a child node
        /// </summary>
        [Parameter]
        public Func<string, IList<TItem>> ChildrenMethodExpression { get; set; }

        protected override Func<TreeNode<TItem>, IList<TItem>> TreeNodeChildrenExpression => node => ChildrenMethodExpression(TreeNodeKeyExpression(node));


        protected override Dictionary<string, object> TreeAttributes
        {
            get
            {
                return new()
                {
                    { "DataSource", RootData },
                    { "TitleExpression", TreeNodeTitleExpression },
                    { "DefaultExpandAll", TreeDefaultExpandAll },
                    { "KeyExpression", TreeNodeKeyExpression },
                    { "ChildrenExpression", TreeNodeChildrenExpression },
                    { "DisabledExpression", TreeNodeDisabledExpression },
                    { "IsLeafExpression", TreeNodeIsLeafExpression }
                };
            }
        }
    }
}
