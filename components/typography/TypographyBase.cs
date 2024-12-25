// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

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
        SubTitle = "排版",
        OutputApi = false)]
    public abstract partial class TypographyBase : AntDomComponentBase
    {
        /// <summary>
        /// Code style
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Code { get; set; } = false;

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
        public EventCallback<string> OnChange { get; set; }

        /// <summary>
        /// Content type - styles text. Possible values: secondary, warning, danger
        /// </summary>
        [Parameter]
        public TextElementType? Type { get; set; }

        /// <summary>
        /// Content to wrap
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected bool Editing => _editing;

        private bool _editing = false;

        protected abstract string HtmlType { get; }

        protected virtual bool IsKeyboard => false;

        private const string PrefixName = "ant-typography";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }

        private void SetClassMap()
        {
            var hashId = UseStyle(PrefixName, TypographyStyle.UseComponentStyle);
            ClassMapper
                .Add(PrefixName)
                .Add(hashId)
                .GetIf(() => $"{PrefixName}-{Type.ToString().ToLowerInvariant()}", () => Type.HasValue)
                .GetIf(() => $"{PrefixName}-{HtmlType}", () => Editing)
                .If($"{PrefixName}-disabled", () => Disabled)
                .If($"{PrefixName}-edit-content", () => Editing)
                .If($"{PrefixName}-rtl", () => RTL);
        }

        private RenderFragment CopyIcon()
        {
            return builder =>
            {
                builder.OpenElement(12, "div");
                builder.AddAttribute(13, "onclick", Copy);
                builder.AddAttribute(14, "style", "border: 0px; background: transparent; padding: 0px; line-height: inherit; display: inline-block;");
                builder.AddAttribute(15, "class", $"{PrefixName}-copy");
                builder.AddAttribute(16, "tabindex", 0);
                builder.AddAttribute(17, "role", "button");
                builder.OpenComponent<Icon>(18);
                builder.AddAttribute(19, "Type", "copy");
                builder.AddAttribute(20, "Theme", IconThemeType.Outline);
                builder.CloseComponent();
                builder.CloseElement();
            };
        }

        private RenderFragment EditIcon()
        {
            return builder =>
            {
                builder.OpenElement(21, "div");
                builder.AddAttribute(22, "class", $"{PrefixName}-edit");
                builder.AddAttribute(23, "role", "button");
                builder.AddAttribute(24, "tabindex", 0);
                builder.AddAttribute(25, "aria-label", "edit");
                builder.AddAttribute(26, "style", "border: 0px; background: transparent; padding: 0px; line-height: inherit; display: inline-block;");
                builder.AddAttribute(27, "onclick", ToggleEditing);
                builder.OpenComponent<Icon>(28);
                builder.AddAttribute(29, "Type", "edit");
                builder.AddAttribute(30, "Theme", IconThemeType.Outline);
                builder.CloseComponent();
                builder.CloseElement();
            };
        }

        private RenderFragment EditTextArea()
        {
            return builder =>
            {
                builder.OpenComponent<TextArea>(4);
                builder.AddAttribute(5, nameof(TextArea.Rows), (uint)1);
                builder.AddAttribute(6, nameof(TextArea.AutoSize), EditConfig.AutoSize);
                builder.AddAttribute(7, nameof(TextArea.AutoFocus), true);
                builder.AddAttribute(8, "Value", EditConfig.Text);
                builder.AddAttribute(9, "ValueChanged", EventCallback.Factory.Create<string>(this, OnEditableValueChange));
                builder.AddAttribute(10, nameof(TextArea.OnBlur), EventCallback.Factory.Create<FocusEventArgs>(this, ToggleEditing));
                builder.CloseComponent();

                builder.OpenComponent<Icon>(11);
                builder.AddAttribute(12, "Class", $"{PrefixName}-edit-content-confirm");
                builder.AddAttribute(13, "Style", "border: 0px; background: transparent; padding: 0px; line-height: inherit;");
                builder.AddAttribute(14, "Type", "enter");
                builder.CloseElement();
            };
        }

        private RenderFragment GetContent()
        {
            return builder =>
            {
                if (IsKeyboard) builder.OpenElement(4, "kbd");
                if (Mark) builder.OpenElement(5, "mark");
                if (Code) builder.OpenElement(6, "code");
                if (Delete) builder.OpenElement(7, "del");
                if (Underline) builder.OpenElement(8, "u");
                if (Strong) builder.OpenElement(9, "strong");

                builder.AddContent(10, ChildContent);

                if (Strong) builder.CloseElement();
                if (Underline) builder.CloseElement();
                if (Delete) builder.CloseElement();
                if (Code) builder.CloseElement();
                if (Mark) builder.CloseElement();
                if (IsKeyboard) builder.CloseElement();
            };
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(1, _editing ? "div" : HtmlType);
            builder.AddAttribute(2, "class", ClassMapper.Class);
            builder.AddAttribute(3, "style", Style);

            if (Editing && EditConfig != null)
            {
                builder.AddContent(10, EditTextArea());
            }
            else
            {
                builder.AddContent(10, GetContent());
            }

            if (Editable && !Editing)
            {
                builder.AddContent(30, EditIcon());
            }

            if (Copyable && !Editing)
            {
                builder.AddContent(40, CopyIcon());
            }

            builder.AddElementReferenceCapture(47, r => Ref = r);
            builder.CloseElement();
            _styleContent(builder);
        }

        private async Task Copy()
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

        private void ToggleEditing()
        {
            _editing = !_editing;
        }

        private void OnEditableValueChange(string value)
        {
            if (EditConfig == null)
            {
                return;
            }
            EditConfig.Text = value;
            EditConfig.OnChange?.Invoke(value);

            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(value);
            }
        }
    }

    public class TypographyCopyableConfig
    {
        public string Text { get; set; } = string.Empty;

        public Action OnCopy { get; set; } = null;
    }

    public class TypographyEditableConfig
    {
        public string Text { get; set; }

        public bool AutoSize { get; set; } = true;

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
