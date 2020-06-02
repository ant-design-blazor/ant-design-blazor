using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntDesign
{
    /// <summary>
    ///
    /// </summary>
    public class Input : AntInputComponentBase<string>
    {
        protected const string PrefixCls = "ant-input";

        private bool _allowClear;
        protected string AffixWrapperClass { get; set; } = $"{PrefixCls}-affix-wrapper";
        protected string GroupWrapperClass { get; set; } = $"{PrefixCls}-group-wrapper";

        //protected string ClearIconClass { get; set; }
        protected static readonly EventCallbackFactory CallbackFactory = new EventCallbackFactory();

        [Parameter]
        public string Type { get; set; } = "text";

        [Parameter]
        public RenderFragment AddOnBefore { get; set; }

        [Parameter]
        public RenderFragment AddOnAfter { get; set; }

        [Parameter]
        public string Size { get; set; } = InputSize.Default;

        [Parameter]
        public string Placeholder { get; set; }

        [Parameter]
        public bool AutoFocus { get; set; }

        [Parameter]
        public string DefaultValue { get; set; }

        [Parameter]
        public int MaxLength { get; set; } = -1;

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool AllowClear { get; set; }

        [Parameter]
        public RenderFragment Prefix { get; set; }

        [Parameter]
        public RenderFragment Suffix { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> OnChange { get; set; }

        [Parameter]
        public EventCallback<KeyboardEventArgs> OnPressEnter { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> OnInput { get; set; }

        [Parameter]
        public EventCallback<FocusEventArgs> OnBlur { get; set; }

        public Dictionary<string, object> Attributes { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!string.IsNullOrEmpty(DefaultValue) && string.IsNullOrEmpty(Value))
            {
                Value = DefaultValue;
            }

            SetClasses();
        }

        protected virtual void SetClasses()
        {
            ClassMapper.Clear()
                .If($"{PrefixCls}", () => Type != "number")
                .If($"{PrefixCls}-lg", () => Size == InputSize.Large)
                .If($"{PrefixCls}-sm", () => Size == InputSize.Small);

            if (Attributes is null)
            {
                Attributes = new Dictionary<string, object>();
            }

            AffixWrapperClass = $"{PrefixCls}-affix-wrapper";
            GroupWrapperClass = $"{PrefixCls}-group-wrapper";

            if (MaxLength >= 0)
            {
                Attributes?.Add("maxlength", MaxLength);
            }

            if (Disabled)
            {
                // TODO: disable element
                AffixWrapperClass = string.Join(" ", AffixWrapperClass, $"{PrefixCls}-affix-wrapper-disabled");
                ClassMapper.Add($"{PrefixCls}-disabled");
            }

            if (AllowClear)
            {
                _allowClear = true;
                //ClearIconClass = $"{PrefixCls}-clear-icon";
                ToggleClearBtn();
            }

            if (Size == InputSize.Large)
            {
                AffixWrapperClass = string.Join(" ", AffixWrapperClass, $"{PrefixCls}-affix-wrapper-lg");
                GroupWrapperClass = string.Join(" ", GroupWrapperClass, $"{PrefixCls}-group-wrapper-lg");
            }
            else if (Size == InputSize.Small)
            {
                AffixWrapperClass = string.Join(" ", AffixWrapperClass, $"{PrefixCls}-affix-wrapper-sm");
                GroupWrapperClass = string.Join(" ", GroupWrapperClass, $"{PrefixCls}-group-wrapper-sm");
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            SetClasses();
        }

        public async Task Focus()
        {
            await JsInvokeAsync(JSInteropConstants.focus, Ref);
        }

        protected async Task OnChangeAsync(ChangeEventArgs args)
        {
            if (OnChange.HasDelegate)
            {
                await OnChange.InvokeAsync(args);
            }
        }

        protected async Task OnPressEnterAsync(KeyboardEventArgs args)
        {
            if (args != null && args.Key == "Enter" && OnPressEnter.HasDelegate)
            {
                await OnPressEnter.InvokeAsync(args);
            }
        }

        private async Task OnBlurAsync(FocusEventArgs e)
        {
            if (OnBlur.HasDelegate)
            {
                await OnBlur.InvokeAsync(e);
            }
        }

        private void ToggleClearBtn()
        {
            Suffix = (builder) =>
            {
                builder.OpenComponent<AntIcon>(31);
                builder.AddAttribute(32, "type", "close-circle");
                if (string.IsNullOrEmpty(Value))
                {
                    builder.AddAttribute(33, "style", "visibility: hidden;");
                }
                else
                {
                    builder.AddAttribute(33, "style", "visibility: visible;");
                }
                builder.AddAttribute(34, "onclick", CallbackFactory.Create<MouseEventArgs>(this, (args) =>
                {
                    Value = string.Empty;
                    ToggleClearBtn();
                }));
                builder.CloseComponent();
            };
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (this.AutoFocus)
            {
                await this.Focus();
            }
        }

        /// <summary>
        /// Invoked when user add/remove content
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual async void OnInputAsync(ChangeEventArgs args)
        {
            bool flag = !(!string.IsNullOrEmpty(Value) && args != null && !string.IsNullOrEmpty(args.Value.ToString()));

            // AntInputComponentBase.Value will be empty, use args.Value
            Value = args?.Value.ToString();
            if (_allowClear && flag)
            {
                ToggleClearBtn();
            }

            if (OnInput.HasDelegate)
            {
                await OnInput.InvokeAsync(args);
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder != null)
            {
                base.BuildRenderTree(builder);

                string container = "input";
                int i = 0;

                if (AddOnBefore != null || AddOnAfter != null)
                {
                    container = "groupWrapper";
                    builder.OpenElement(i++, "span");
                    builder.AddAttribute(i++, "class", GroupWrapperClass);
                    builder.AddAttribute(i++, "style", Style);
                    builder.OpenElement(i++, "span");
                    builder.AddAttribute(i++, "class", $"{PrefixCls}-wrapper {PrefixCls}-group");
                }

                if (AddOnBefore != null)
                {
                    // addOnBefore
                    builder.OpenElement(i++, "span");
                    builder.AddAttribute(i++, "class", $"{PrefixCls}-group-addon");
                    builder.AddContent(i++, AddOnBefore);
                    builder.CloseElement();
                }

                if (Prefix != null || Suffix != null)
                {
                    builder.OpenElement(i++, "span");
                    builder.AddAttribute(i++, "class", AffixWrapperClass);
                    if (container == "input")
                    {
                        container = "affixWrapper";
                        builder.AddAttribute(i++, "style", Style);
                    }
                }

                if (Prefix != null)
                {
                    // prefix
                    builder.OpenElement(i++, "span");
                    builder.AddAttribute(i++, "class", $"{PrefixCls}-prefix");
                    builder.AddContent(i++, Prefix);
                    builder.CloseElement();
                }

                // input
                builder.OpenElement(i++, "input");
                builder.AddAttribute(i++, "class", ClassMapper.Class);
                if (container == "input")
                {
                    builder.AddAttribute(i++, "style", Style);
                }

                if (Attributes != null)
                {
                    foreach (KeyValuePair<string, object> pair in Attributes)
                    {
                        builder.AddAttribute(i++, pair.Key, pair.Value);
                    }
                }

                builder.AddAttribute(i++, "Id", Id);
                if (Type != "number")
                {
                    builder.AddAttribute(i++, "type", Type);
                }

                builder.AddAttribute(i++, "placeholder", Placeholder);
                builder.AddAttribute(i++, "value", Value);
                builder.AddAttribute(i++, "onchange", CallbackFactory.Create(this, OnChangeAsync));
                builder.AddAttribute(i++, "onkeypress", CallbackFactory.Create(this, OnPressEnterAsync));
                builder.AddAttribute(i++, "oninput", CallbackFactory.Create(this, OnInputAsync));
                builder.AddAttribute(i++, "onblur", CallbackFactory.Create(this, OnBlurAsync));
                builder.AddElementReferenceCapture(i++, r => Ref = r);
                builder.CloseElement();

                if (Suffix != null)
                {
                    // suffix
                    builder.OpenElement(i++, "span");
                    builder.AddAttribute(i++, "class", $"{PrefixCls}-suffix");
                    builder.AddContent(i++, Suffix);
                    builder.CloseElement();
                }

                if (Prefix != null || Suffix != null)
                {
                    builder.CloseElement();
                }

                if (AddOnAfter != null)
                {
                    // addOnAfter
                    builder.OpenElement(i++, "span");
                    builder.AddAttribute(i++, "class", $"{PrefixCls}-group-addon");
                    builder.AddContent(i++, AddOnAfter);
                    builder.CloseElement();
                }

                if (AddOnBefore != null || AddOnAfter != null)
                {
                    builder.CloseElement();
                    builder.CloseElement();
                }
            }
        }
    }
}
