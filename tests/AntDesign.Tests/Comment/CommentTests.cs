// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace AntDesign.Tests.Comment
{
    public class CommentTests : AntDesignTestBase
    {
        [Fact]
        public void ItShouldRenderWithStandardParameterValues()
        {
            var cut = Context.RenderComponent<AntDesign.Comment>(parameters => parameters
                .Add(x => x.Author, "Han Solo")
                .Add(x => x.Avatar, "AvatarUrl")
                .Add(x => x.Content, "Some comment")
                .Add(x => x.Datetime, "Yesterday"));

            cut.MarkupMatches(@"<div class=""ant-comment"" id:ignore>
                <div class=""ant-comment-inner"">
                    <div class=""ant-comment-avatar"">
                        <span class=""ant-avatar ant-avatar-image"" style="" "" id:ignore>
                            <img src=""AvatarUrl"">
                        </span>
                    </div>
                    <div class=""ant-comment-content"">
                        <div class=""ant-comment-content-author"">
                            <span class=""ant-comment-content-author-name"">
                                <a>Han Solo</a>
                            </span>
                            <span class=""ant-comment-content-author-time"">
                                <span>Yesterday</span>
                            </span>
                        </div>
                        <div class=""ant-comment-content-detail"">
                            <p>Some comment</p>
                        </div>
                    </div>
                </div>
                <div class=""ant-comment-nested"">
                </div>
            </div>");
        }

        [Fact]
        public void ItShouldRenderAvatarTemplate()
        {
            RenderFragment fragment = (builder) =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(1, "Avatar");
                builder.CloseElement();
            };

            var cut = Context.RenderComponent<AntDesign.Comment>(parameters => parameters
                .Add(x => x.Author, "Han Solo")
                .Add(x => x.AvatarTemplate, fragment)
                .Add(x => x.Content, "Some comment")
                .Add(x => x.Datetime, "Yesterday"));

            cut.Find(".ant-comment-avatar").MarkupMatches(@"<div class=""ant-comment-avatar"">
                <span>Avatar</span>
            </div>");
        }

        [Fact]
        public void ItShouldRenderAuthorTemplate()
        {
            RenderFragment fragment = (builder) =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(1, "Author");
                builder.CloseElement();
            };

            var cut = Context.RenderComponent<AntDesign.Comment>(parameters => parameters
                .Add(x => x.Avatar, "IconUrl")
                .Add(x => x.AuthorTemplate, fragment)
                .Add(x => x.Content, "Some comment")
                .Add(x => x.Datetime, "Yesterday"));

            cut.Find(".ant-comment-content-author-name").MarkupMatches(@"<span class=""ant-comment-content-author-name"">
                <span>Author</span>
            </span>");
        }

        [Fact]
        public void ItShouldRenderDatetimeTemplate()
        {
            RenderFragment fragment = (builder) =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(1, "Date");
                builder.CloseElement();
            };

            var cut = Context.RenderComponent<AntDesign.Comment>(parameters => parameters
                .Add(x => x.Avatar, "IconUrl")
                .Add(x => x.Author, "Han Solo")
                .Add(x => x.DatetimeTemplate, fragment)
                .Add(x => x.Content, "Some comment")
                .Add(x => x.Datetime, "Yesterday"));

            cut.Find(".ant-comment-content-author-time").MarkupMatches(@"<span class=""ant-comment-content-author-time"">
                <span>Date</span>
            </span>");
        }

        [Fact]
        public void ItShouldRenderContentTemplate()
        {
            RenderFragment fragment = (builder) =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(1, "Content Yay");
                builder.CloseElement();
            };

            var cut = Context.RenderComponent<AntDesign.Comment>(parameters => parameters
                .Add(x => x.Avatar, "IconUrl")
                .Add(x => x.Author, "Han Solo")
                .Add(x => x.ContentTemplate, fragment)
                .Add(x => x.Datetime, "Yesterday"));

            cut.Find(".ant-comment-content-detail").MarkupMatches(@"<div class=""ant-comment-content-detail"">
                <span>Content Yay</span>
            </div>");
        }

        [Fact]
        public void ItShouldRenderActionsList()
        {
            RenderFragment fragment = (builder) =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(1, "Action 1");
                builder.CloseElement();
            };

            RenderFragment fragmentTwo = (builder) =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(1, "Action 2");
                builder.CloseElement();
            };

            var cut = Context.RenderComponent<AntDesign.Comment>(parameters => parameters
                .Add(x => x.Avatar, "IconUrl")
                .Add(x => x.Author, "Han Solo")
                .Add(x => x.Content, "Comment here")
                .Add(x => x.Actions, new[] { fragment, fragmentTwo })
                .Add(x => x.Datetime, "Yesterday"));

            cut.Find(".ant-comment-actions").MarkupMatches(@"<ul class=""ant-comment-actions"">
                <li><span>Action 1</span></li>
                <li><span>Action 2</span></li>
            </ul>");
        }

        [Fact]
        public void ItShouldProperlyNestComments()
        {
            RenderFragment nestedCommentsFragment = (builder) =>
            {
                builder.OpenComponent<AntDesign.Comment>(0);
                builder.AddAttribute(1, "Author", "Obi Wan");
                builder.AddAttribute(1, "Avatar", "ObiWanImage");
                builder.AddAttribute(2, "Content", "Another comment, but in reply");
                builder.CloseComponent();
            };

            var cut = Context.RenderComponent<AntDesign.Comment>(parameters => parameters
                .Add(x => x.Author, "Han Solo")
                .Add(x => x.Avatar, "AvatarUrl")
                .Add(x => x.Content, "Some comment")
                .Add(x => x.Datetime, "Yesterday")
                .Add(x => x.ChildContent, nestedCommentsFragment));

            cut.MarkupMatches(@"<div class=""ant-comment"" id:ignore>
                <div class=""ant-comment-inner"">
                    <div class=""ant-comment-avatar"">
                        <span class=""ant-avatar ant-avatar-image"" style="" "" id:ignore>
                            <img src=""AvatarUrl"">
                        </span>
                    </div>
                    <div class=""ant-comment-content"">
                        <div class=""ant-comment-content-author"">
                            <span class=""ant-comment-content-author-name"">
                                <a>Han Solo</a>
                            </span>
                            <span class=""ant-comment-content-author-time"">
                                <span>Yesterday</span>
                            </span>
                        </div>
                        <div class=""ant-comment-content-detail"">
                            <p>Some comment</p>
                        </div>
                    </div>
                </div>
                <div class=""ant-comment-nested"">
                    <div class=""ant-comment"" id:ignore>
                        <div class=""ant-comment-inner"">
                            <div class=""ant-comment-avatar"">
                                <span class=""ant-avatar ant-avatar-image"" style="" "" id:ignore>
                                    <img src=""ObiWanImage"">
                                </span>
                            </div>
                            <div class=""ant-comment-content"">
                                <div class=""ant-comment-content-author"">
                                    <span class=""ant-comment-content-author-name"">
                                        <a>Obi Wan</a>
                                    </span>
                                </div>
                                <div class=""ant-comment-content-detail"">
                                    <p>Another comment, but in reply</p>
                                </div>
                            </div>
                        </div>
                        <div class=""ant-comment-nested"">
                        </div>
                    </div>
                </div>
            </div>");
        }
    }
}
