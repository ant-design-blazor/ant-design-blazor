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
    public  class Input : AntInputComponentBase<string>
    {
        protected const string PrefixCls = "ant-input";

        private bool _allowClear;
        protected string AffixWrapperClass { get; set; } = $"{PrefixCls}-affix-wrapper";
        protected string GroupWrapperClass { get; set; } = $"{PrefixCls}-group-wrapper";

        //protected string ClearIconClass { get; set; }
        protected static readonly EventCallbackFactory CallbackFactory = new EventCallbackFactory();

        protected ElementReference InputEl { get; set; }

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
            await JsInvokeAsync(JSInteropConstants.focus, this);
        }

        protected async Task Blur()
        {
            await JsInvokeAsync(JSInteropConstants.blur, this);
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
             base.OnAfterRenderAsync(firstRender);
        
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

                if (AddOnBefore != null || AddOnAfter != null)
                {
                    container = "groupWrapper";
                    builder.OpenElement(0, "span");
                    builder.AddAttribute(1, "class", GroupWrapperClass);
                    builder.AddAttribute(2, "style", Style);
                    builder.OpenElement(3, "span");
                    builder.AddAttribute(4, "class", $"{PrefixCls}-wrapper {PrefixCls}-group");
                }

                if (AddOnBefore != null)
                {
                    // addOnBefore
                    builder.OpenElement(5, "span");
                    builder.AddAttribute(6, "class", $"{PrefixCls}-group-addon");
                    builder.AddContent(7, AddOnBefore);
                    builder.CloseElement();
                }

                if (Prefix != null || Suffix != null)
                {
                    builder.OpenElement(8, "span");
                    builder.AddAttribute(9, "class", AffixWrapperClass);
                    if (container == "input")
                    {
                        container = "affixWrapper";
                        builder.AddAttribute(10, "style", Style);
                    }
                }

                if (Prefix != null)
                {
                    // prefix
                    builder.OpenElement(11, "span");
                    builder.AddAttribute(12, "class", $"{PrefixCls}-prefix");
                    builder.AddContent(13, Prefix);
                    builder.CloseElement();
                }

                // input
                builder.OpenElement(14, "input");
                builder.AddAttribute(15, "class", ClassMapper.Class);
                if (container == "input")
                {
                    builder.AddAttribute(16, "style", Style);
                }

                if (Attributes != null)
                {
                    foreach (KeyValuePair<string, object> pair in Attributes)
                    {
                        builder.AddAttribute(17, pair.Key, pair.Value);
                    }
                }

                builder.AddAttribute(18, "Id", Id);
                if (Type != "number")
                {
                    builder.AddAttribute(19, "type", Type);
                }

                builder.AddAttribute(20, "placeholder", Placeholder);
                builder.AddAttribute(21, "value", Value);
                builder.AddAttribute(22, "onchange", CallbackFactory.Create(this, OnChangeAsync));
                builder.AddAttribute(23, "onkeypress", CallbackFactory.Create(this, OnPressEnterAsync));
                builder.AddAttribute(24, "oninput", CallbackFactory.Create(this, OnInputAsync));          
                builder.CloseElement();

                if (Suffix != null)
                {
                    // suffix
                    builder.OpenElement(25, "span");
                    builder.AddAttribute(26, "class", $"{PrefixCls}-suffix");
                    builder.AddContent(27, Suffix);
                    builder.CloseElement();
                }

                if (Prefix != null || Suffix != null)
                {
                    builder.CloseElement();
                }

                if (AddOnAfter != null)
                {
                    // addOnAfter
                    builder.OpenElement(28, "span");
                    builder.AddAttribute(29, "class", $"{PrefixCls}-group-addon");
                    builder.AddContent(30, AddOnAfter);
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
