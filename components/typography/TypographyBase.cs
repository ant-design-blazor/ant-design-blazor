// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /**
    <summary>
    <para>Basic text writing, including headings, body text, lists, and more.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>When need to display a title or paragraph contents in Articles/Blogs/Notes.</item>
        <item>When you need copyable/editable/ellipsis texts.</item>
    </list>
    </summary>
    <seealso cref="Text"/>
    <seealso cref="Title"/>
    <seealso cref="Paragraph"/>
    */
    [Documentation(DocumentationCategory.Components, 
        DocumentationType.General, 
        "https://gw.alipayobjects.com/zos/alicdn/GOM1KQ24O/Typography.svg", 
        Columns = 1, 
        Title = "Typography",
        OutputApi = false)]
    public abstract class TypographyBase : AntDomComponentBase
    {
        [Inject]
        public HtmlRenderService Service { get; set; }

        /// <summary>
        /// If the text can be copied or not. Will add a copy icon to end of text if true.
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Copyable { get; set; } = false;

        /// <summary>
        /// Configure what happens when copying
        /// </summary>
        [Parameter]
        public TypographyCopyableConfig CopyConfig { get; set; }

        /// <summary>
        /// Strikethrough style
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Delete { get; set; } = false;

        /// <summary>
        /// Disabled style
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Disabled { get; set; } = false;

        /// <summary>
        /// Control if editable or not
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Editable { get; set; } = false;

        /// <summary>
        /// Configure editing
        /// </summary>
        [Parameter]
        public TypographyEditableConfig EditConfig { get; set; }

        /// <summary>
        /// Display ellipsis when text overflows. Should set width when ellipsis needed
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Ellipsis { get; set; } = false;

        /// <summary>
        /// Configure ellipsis
        /// </summary>
        [Parameter]
        public TypographyEllipsisConfig EllipsisConfig { get; set; }

        /// <summary>
        /// Highlight the text
        /// </summary>
        [Parameter]
        public bool Mark { get; set; } = false;

        /// <summary>
        /// Underline the text
        /// </summary>
        [Parameter]
        public bool Underline { get; set; } = false;

        /// <summary>
        /// Bold the text
        /// </summary>
        [Parameter]
        public bool Strong { get; set; } = false;

        /// <summary>
        /// Callback executed when the user edits the content
        /// </summary>
        [Parameter]
        public Action OnChange { get; set; }

        /// <summary>
        /// Content type - styles text. Possible values: secondary, warning, danger
        /// </summary>
        [Parameter]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Content to wrap
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Copy the contents, if enabled with <see cref="Copyable"/>
        /// </summary>
        public async Task Copy()
        {
            if (!Copyable)
            {
                return;
            }
            if (string.IsNullOrEmpty(CopyConfig?.Text))
            {
                await this.JsInvokeAsync<object>(JSInteropConstants.CopyElement, Ref);
            }
            else
            {
                await this.JsInvokeAsync<object>(JSInteropConstants.Copy, CopyConfig.Text);
            }
            CopyConfig?.OnCopy?.Invoke();
        }
    }

    public class TypographyCopyableConfig
    {
        public string Text { get; set; } = string.Empty;

        public Action OnCopy { get; set; } = null;
    }

    public class TypographyEditableConfig
    {
        public Action OnStart { get; set; }

        public Action<string> OnChange { get; set; }
    }

    public class TypographyEllipsisConfig
    {
        public string Suffix { get; set; } = "...";

        public int Rows { get; set; }

        public Action OnExpand { get; set; }
    }
}
