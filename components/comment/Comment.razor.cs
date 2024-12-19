// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /**
    <summary>
    <para>A comment displays user feedback and discussion to website content.</para>

    <h2>When To Use</h2>

    <para>Comments can be used to enable discussions on an entity such as a page, blog post, issue or other.</para>
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/alicdn/ILhxpGzBO/Comment.svg", Columns = 1, Title = "Comment", SubTitle = "评论")]
    public partial class Comment : AntDomComponentBase
    {
        /// <summary>
        /// Author string
        /// </summary>
        [Parameter]
        public string Author { get; set; }

        /// <summary>
        /// Author content. Takes priority over <see cref="Author"/>
        /// </summary>
        [Parameter]
        public RenderFragment AuthorTemplate { get; set; }

        /// <summary>
        /// Avatar string. Gets passed as the <see cref="Avatar.Src"/> to the <see cref="AntDesign.Avatar"/> component.
        /// </summary>
        [Parameter]
        public string Avatar { get; set; }

        /// <summary>
        /// Avatar content. Takes priority over <see cref="Avatar"/>
        /// </summary>
        [Parameter]
        public RenderFragment AvatarTemplate { get; set; }

        /// <summary>
        /// Content string for the comment
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Content for the comment. Takes priority over <see cref="Content"/>
        /// </summary>
        [Parameter]
        public RenderFragment ContentTemplate { get; set; }

        /// <summary>
        /// Used primarily for nesting comments for functionality such as replies
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Date string for the comment
        /// </summary>
        [Parameter]
        public string Datetime { get; set; }

        /// <summary>
        /// Date content for the comment. Takes priority over <see cref="Datetime"/>
        /// </summary>
        [Parameter]
        public RenderFragment DatetimeTemplate { get; set; }

        /// <summary>
        /// List of actions to show at the bottm of the comment
        /// </summary>
        [Parameter]
        public IList<RenderFragment> Actions { get; set; } = new List<RenderFragment>();

        /// <summary>
        /// Sets the direction of the comment with <see cref="CommentPlacement" />.
        /// </summary>
        /// <default value="left"/>
        [Parameter]
        public CommentPlacement Placement { get; set; } = CommentPlacement.Left;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ClassMapper.Clear()
                .Add("ant-comment")
                .If("ant-comment-right", () => Placement == CommentPlacement.Right)
                .If("ant-comment-rtl", () => RTL);
        }
    }
}
