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
    public abstract class TypographyBase : AntDomComponentBase
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
        /// Italic style
        /// </summary>
        [Parameter]
        public bool Italic { get; set; }

        /// <summary>
        /// Native title attribute shown by the browser on hover.
        /// </summary>
        [Parameter]
        public string Title { get; set; }

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
        private bool _ellipsisExpanded;
        private TypographyEllipsisConfig _lastEllipsisConfig;
        private string _editingText;
        private string _editingOriginalText;

        protected abstract string HtmlType { get; }

        protected virtual bool IsKeyboard => false;

        protected virtual void AddElementAttributes(RenderTreeBuilder builder, int sequence) { }

        private const string PrefixName = "ant-typography";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (!ReferenceEquals(_lastEllipsisConfig, EllipsisConfig))
            {
                _lastEllipsisConfig = EllipsisConfig;
                _ellipsisExpanded = EllipsisConfig?.Expanded ?? EllipsisConfig?.DefaultExpanded ?? false;
            }
            else if (EllipsisConfig?.Expanded is bool expanded)
            {
                _ellipsisExpanded = expanded;
            }
        }

        private void SetClassMap()
        {
            ClassMapper
                .Add(PrefixName)
                .GetIf(() => $"{PrefixName}-{Type.ToString().ToLowerInvariant()}", () => Type.HasValue)
                .GetIf(() => $"{PrefixName}-{HtmlType}", () => Editing)
                .If($"{PrefixName}-disabled", () => Disabled)
                .If($"{PrefixName}-ellipsis", () => IsEllipsisEnabled && !IsEllipsisExpanded)
                .If($"{PrefixName}-ellipsis-single-line", () => IsEllipsisEnabled && !IsEllipsisExpanded && HtmlType == "span")
                .If($"{PrefixName}-ellipsis-multiple-line", () => IsEllipsisEnabled && !IsEllipsisExpanded && HtmlType != "span")
                .If($"{PrefixName}-edit-content", () => Editing)
                .If($"{PrefixName}-rtl", () => RTL);
        }

        private bool IsEllipsisEnabled => Ellipsis || EllipsisConfig != null;

        private bool IsEllipsisExpanded => EllipsisConfig?.Expanded ?? _ellipsisExpanded;

        private bool HasCustomEllipsisSuffix => !string.IsNullOrWhiteSpace(EllipsisConfig?.Suffix)
            && EllipsisConfig.Suffix != "...";

        private string TypographyStyle
        {
            get
            {
                if (!IsEllipsisEnabled || IsEllipsisExpanded)
                {
                    return HasCustomEllipsisSuffix && HtmlType == "span"
                        ? AppendStyle(Style, "display: inline-flex; min-width: 0; max-width: 100%; vertical-align: bottom;")
                        : Style;
                }

                var ellipsisStyle = HasCustomEllipsisSuffix && HtmlType == "span"
                    ? "display: inline-flex; min-width: 0; max-width: 100%; vertical-align: bottom;"
                    : HtmlType == "span"
                        ? "display: inline-block; max-width: 100%; overflow: hidden; white-space: nowrap; text-overflow: ellipsis;"
                        : $"-webkit-line-clamp: {Math.Max(1, EllipsisConfig?.Rows ?? 1)}; -webkit-box-orient: vertical; overflow: hidden;";

                if (HasCustomEllipsisSuffix && HtmlType != "span")
                {
                    ellipsisStyle += " position: relative;";
                }

                return AppendStyle(Style, ellipsisStyle);
            }
        }

        private static string AppendStyle(string current, string addition)
        {
            if (string.IsNullOrWhiteSpace(current)) return addition;

            var normalized = current.TrimEnd();
            return $"{normalized}{(normalized.EndsWith(";", StringComparison.Ordinal) ? "" : ";")} {addition}";
        }

        private RenderFragment CopyIcon()
        {
            return builder =>
            {
                builder.OpenElement(12, "div");
                builder.AddAttribute(13, "onclick", Copy);
                builder.AddAttribute(14, "style", "border: 0px; background: transparent; padding: 0px; line-height: inherit; display: inline-block;");
                builder.AddAttribute(15, "class", $"{PrefixName}-copy {PrefixName}-action");
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
                builder.AddAttribute(22, "class", $"{PrefixName}-edit {PrefixName}-action");
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
                builder.AddAttribute(5, nameof(TextArea.Class), $"{PrefixName}-textarea");
                builder.AddAttribute(6, nameof(TextArea.Rows), (uint)1);
                builder.AddAttribute(7, nameof(TextArea.AutoSize), EditConfig.AutoSize);
                builder.AddAttribute(8, nameof(TextArea.MaxLength), EditConfig.MaxLength);
                builder.AddAttribute(9, nameof(TextArea.AutoFocus), true);
                builder.AddAttribute(10, nameof(TextArea.BindOnInput), true);
                builder.AddAttribute(11, "Value", _editingText);
                builder.AddAttribute(12, nameof(TextArea.OnInput), EventCallback.Factory.Create<ChangeEventArgs>(this, OnEditableInput));
                builder.AddAttribute(13, "ValueChanged", EventCallback.Factory.Create<string>(this, OnEditableValueChange));
                builder.AddAttribute(14, nameof(TextArea.OnBlur), EventCallback.Factory.Create<FocusEventArgs>(this, ConfirmEdit));
                builder.AddAttribute(15, nameof(TextArea.OnKeyUp), EventCallback.Factory.Create<KeyboardEventArgs>(this, OnEditKeyUp));
                builder.CloseComponent();

                builder.OpenElement(16, "span");
                builder.AddAttribute(17, "class", $"{PrefixName}-edit-content-confirm");
                builder.AddAttribute(18, "onclick", EventCallback.Factory.Create(this, ConfirmEdit));
                builder.OpenComponent<Icon>(19);
                builder.AddAttribute(20, "Style", "border: 0px; background: transparent; padding: 0px; line-height: inherit;");
                builder.AddAttribute(21, "Type", "enter");
                builder.CloseComponent();
                builder.CloseElement();
            };
        }

        private RenderFragment GetContent()
        {
            return builder =>
            {
                if (Italic) builder.OpenElement(3, "i");
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
                if (Italic) builder.CloseElement();
            };
        }

        private RenderFragment GetEllipsisContent()
        {
            return builder =>
            {
                if (HasCustomEllipsisSuffix && HtmlType == "span")
                {
                    builder.OpenElement(60, "span");
                    builder.AddAttribute(61, "class", "ant-typography-ellipsis-content");
                    if (!IsEllipsisExpanded)
                    {
                        builder.AddAttribute(62, "style", "flex: 1 1 auto; min-width: 0; overflow: hidden; white-space: nowrap; text-overflow: ellipsis;");
                    }
                    else
                    {
                        builder.AddAttribute(62, "style", "flex: 1 1 auto; min-width: 0;");
                    }
                    builder.AddContent(63, GetContent());
                    builder.CloseElement();
                    builder.OpenElement(64, "span");
                    builder.AddAttribute(65, "class", "ant-typography-ellipsis-suffix");
                    builder.AddAttribute(66, "style", "flex: none;");
                    builder.AddContent(67, EllipsisConfig.Suffix);
                    builder.CloseElement();
                    return;
                }

                builder.AddContent(60, GetContent());
                if (HasCustomEllipsisSuffix)
                {
                    builder.OpenElement(64, "span");
                    builder.AddAttribute(65, "class", "ant-typography-ellipsis-suffix");
                    if (!IsEllipsisExpanded)
                    {
                        builder.AddAttribute(66, "style", "position: absolute; right: 0; bottom: 0; padding-inline-start: 0.25em; background: var(--ant-color-bg-container, #fff);");
                    }
                    builder.AddContent(67, EllipsisConfig.Suffix);
                    builder.CloseElement();
                }
            };
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(1, _editing ? "div" : HtmlType);
            AddElementAttributes(builder, 2);
            builder.AddAttribute(4, "class", ClassMapper.Class);
            builder.AddAttribute(5, "style", TypographyStyle);
            builder.AddAttribute(6, "title", Title);

            if (Editing && EditConfig != null)
            {
                builder.AddContent(10, EditTextArea());
            }
            else
            {
                builder.AddContent(10, GetEllipsisContent());

                if (CanToggleEllipsis)
                {
                    builder.OpenElement(32, "button");
                    builder.AddAttribute(33, "type", "button");
                    builder.AddAttribute(34, "class", IsEllipsisExpanded ? "ant-typography-collapse" : "ant-typography-expand");
                    builder.AddAttribute(35, "onclick", EventCallback.Factory.Create(this, ToggleEllipsis));
                    builder.AddContent(36, IsEllipsisExpanded ? EllipsisConfig.CollapseSymbol : EllipsisConfig.Symbol);
                    builder.CloseElement();
                }
            }

            if (Editable && !Editing)
            {
                builder.OpenElement(28, "span");
                builder.AddAttribute(29, "class", $"{PrefixName}-actions");
                builder.AddContent(30, EditIcon());
                if (Copyable) builder.AddContent(31, CopyIcon());
                builder.CloseElement();
            }
            else if (Copyable && !Editing)
            {
                builder.OpenElement(38, "span");
                builder.AddAttribute(39, "class", $"{PrefixName}-actions");
                builder.AddContent(40, CopyIcon());
                builder.CloseElement();
            }

            builder.AddElementReferenceCapture(47, r => Ref = r);
            builder.CloseElement();
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
            if (_editing || EditConfig == null) return;

            _editingOriginalText = EditConfig.Text;
            _editingText = EditConfig.Text;
            _editing = true;
            EditConfig.OnStart?.Invoke();
        }

        private bool CanToggleEllipsis => EllipsisConfig?.Expandable == true
            && (!IsEllipsisExpanded || EllipsisConfig.Collapsible);

        private async Task ToggleEllipsis()
        {
            var expanded = !IsEllipsisExpanded;
            if (!EllipsisConfig.Expanded.HasValue)
            {
                _ellipsisExpanded = expanded;
            }

            EllipsisConfig?.OnExpand?.Invoke();
            if (EllipsisConfig?.OnExpandedChange.HasDelegate == true)
            {
                await EllipsisConfig.OnExpandedChange.InvokeAsync(expanded);
            }
        }

        private void OnEditableValueChange(string value)
        {
            _editingText = value?.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        private void OnEditableInput(ChangeEventArgs args)
        {
            _editingText = args?.Value?.ToString()?.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        private void OnEditKeyUp(KeyboardEventArgs args)
        {
            if (args.Key == "Escape" || args.Key == "Esc")
            {
                CancelEdit();
            }
            else if (args.Key == "Enter")
            {
                ConfirmEdit();
                EditConfig?.OnEnd?.Invoke();
            }
        }

        private void ConfirmEdit()
        {
            if (!_editing || EditConfig == null) return;

            var value = (_editingText ?? string.Empty).Trim();
            EditConfig.Text = value;
            _editing = false;
            EditConfig.OnChange?.Invoke(value);

            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(value);
            }
        }

        private void CancelEdit()
        {
            if (!_editing || EditConfig == null) return;

            EditConfig.Text = _editingOriginalText;
            _editingText = _editingOriginalText;
            _editing = false;
            EditConfig.OnCancel?.Invoke();
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

        public Action OnCancel { get; set; }

        public Action OnEnd { get; set; }

        public int MaxLength { get; set; } = -1;
    }

    public class TypographyEllipsisConfig
    {
        public string Suffix { get; set; } = "...";

        public int Rows { get; set; }

        public bool Expandable { get; set; }

        public bool Collapsible { get; set; }

        public bool DefaultExpanded { get; set; }

        public bool? Expanded { get; set; }

        public string Symbol { get; set; } = "more";

        public string CollapseSymbol { get; set; } = "less";

        public Action OnExpand { get; set; }

        public EventCallback<bool> OnExpandedChange { get; set; }
    }
}
