// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /**
    <summary>
        <para>To trigger an operation.</para>

        <h2>When To Use</h2>

        <para>A button means an operation (or a series of operations). Clicking a button will trigger corresponding business logic.</para>
        <para>In Ant Design we provide 4 types of button.</para>

        <list type="bullet">
            <item>Primary button: indicate the main action, one primary button at most in one section.</item>
            <item>Default button: indicate a series of actions without priority.</item>
            <item>Dashed button: used for adding action commonly.</item>
            <item>Link button: used for external links.</item>
        </list>

        <para>And 4 other properties additionally.</para>

        <list type="bullet">
            <item><c>Danger</c>: used for actions of risk, like deletion or authorization.</item>
            <item><c>Ghost</c>: used in situations with complex background, home pages usually.</item>
            <item><c>Disabled</c>: when actions is not available.</item>
            <item><c>Loading</c>: add loading spinner in button, avoiding multiple submits too.</item>
        </list>
    </summary>
    <seealso cref="DownloadButton" />
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.General, "https://gw.alipayobjects.com/zos/alicdn/fNUKzY1sk/Button.svg", Title = "Button", SubTitle = "按钮")]
    public partial class Button : AntDomComponentBase
    {
        private FormSize? _formSize;

        private const int RemoveAnimationAfter = 500;

        [CascadingParameter(Name = "FormSize")]
        public FormSize? FormSize
        {
            get
            {
                return _formSize;
            }
            set
            {
                _formSize = value;

                if (_formSize.HasValue)
                {
                    Size = _formSize.Value switch
                    {
                        AntDesign.FormSize.Large => ButtonSize.Large,
                        AntDesign.FormSize.Small => ButtonSize.Small,
                        _ => ButtonSize.Default,
                    };
                }
                else
                    Size = ButtonSize.Default;
            }
        }

        /// <summary>
        /// Sets the value of the aria-label attribute
        /// </summary>
        [Parameter]
        public string AriaLabel { get; set; }

        /// <summary>
        /// Set the color of the button.
        /// </summary>
        /// <default value="Color.None" />
        [Parameter]
        public Color Color { get; set; } = Color.None;

        /// <summary>
        /// Option to fit button width to its parent width
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Block { get; set; } = false;

        /// <summary>
        /// Content of the button.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Set the danger status of button.
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Danger { get; set; }

        /// <summary>
        /// Whether the `Button` is disabled.
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Make background transparent and invert text and border colors
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Ghost { get; set; }

        /// <summary>
        /// Set the original html type of the button element.
        /// </summary>
        /// <default value="button" />
        [Parameter]
        public string HtmlType { get; set; } = "button";

        /// <summary>
        /// Set the icon component of button.
        /// </summary>
        [Parameter]
        public string Icon { get; set; }

        /// <summary>
        /// Show loading indicator. You have to write the loading logic on your own.
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Loading { get; set; }

        /// <summary>
        /// Whether to trigger and keep the loading state until the event callback is done.
        /// </summary>
        [Parameter]
        public bool AutoLoading { get; set; }

        /// <summary>
        /// Callback when `Button` is clicked
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        /// <summary>
        /// Do not propagate events when button is clicked.
        /// </summary>
        [Parameter]
        public bool OnClickStopPropagation { get; set; }

        /// <summary>
        /// Can set button shape: `circle` | `round` or `null` (default, which is rectangle).
        /// </summary>
        /// <default value="null" />
        [Parameter]
        public ButtonShape Shape { get; set; } = ButtonShape.Rectangle;

        /// <summary>
        /// Set the size of button.
        /// </summary>
        /// <default value="ButtonSize.Default" />
        [Parameter]
        public ButtonSize Size { get; set; } = ButtonSize.Default;

        /// <summary>
        /// Type of the button.
        /// </summary>
        /// <default value="ButtonType.Default" />
        [Parameter]
        public ButtonType? Type { get; set; } = ButtonType.Default;

        private static readonly Dictionary<ButtonType, string> _typeMap = new()
        {
            [ButtonType.Default] = "default",
            [ButtonType.Primary] = "primary",
            [ButtonType.Dashed] = "dashed",
            [ButtonType.Link] = "link",
            [ButtonType.Text] = "text",
        };

        /// <summary>
        /// Do not wrap with &lt;span&gt;
        /// </summary>
        [Parameter]
        public bool NoSpanWrap { get; set; }

        private bool _animating = false;

        private const string BtnWave = "--antd-wave-shadow-color: rgb(255, 120, 117);";

        protected void SetClassMap()
        {
            var prefixName = "ant-btn";

            ClassMapper.Clear()
                .Add(prefixName)
                .GetIf(() => $"{prefixName}-{_typeMap[Type.GetValueOrDefault(ButtonType.Default)]}", () => Type.HasValue)
                .If($"{prefixName}-dangerous", () => Danger)
                .GetIf(() => $"{prefixName}-{Shape.ToString().ToLowerInvariant()}", () => Shape != ButtonShape.Rectangle)
                .If($"{prefixName}-lg", () => Size == ButtonSize.Large)
                .If($"{prefixName}-sm", () => Size == ButtonSize.Small)
                .If($"{prefixName}-loading", () => Loading)
                .If($"{prefixName}-icon-only", () => !string.IsNullOrEmpty(this.Icon) && this.ChildContent == null)
                .If($"{prefixName}-background-ghost", () => Ghost)
                .If($"{prefixName}-block", () => this.Block)
                .If($"{prefixName}-rtl", () => RTL)
                ;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
            SetButtonColorStyle();
        }

        private async Task HandleOnClick(MouseEventArgs args)
        {
            if (Loading)
                return;

            if (OnClick.HasDelegate)
            {
                if (AutoLoading)
                {
                    Loading = true;
                    StateHasChanged();
                    await OnClick.InvokeAsync(args);
                    Loading = false;
                }
                else
                {
                    _ = OnClick.InvokeAsync(args);
                }
            }
        }

        private async Task OnMouseUp(MouseEventArgs args)
        {
            if (args.Button != 0 || this.Type == ButtonType.Link) return; //remove animating from Link Button
            this._animating = true;

            await Task.Delay(RemoveAnimationAfter);
            this._animating = false;

            await InvokeAsync(StateHasChanged);
        }

        private void SetButtonColorStyle()
        {
            if (Color != Color.None)
            {
                Style += ColorHelper.GetBackgroundStyle(Color);
            }
        }
    }
}
