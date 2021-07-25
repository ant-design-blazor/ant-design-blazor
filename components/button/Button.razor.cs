using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Button : AntDomComponentBase
    {
        private string _formSize;

        private static Dictionary<ButtonColor, string> _buttonColors = new Dictionary<ButtonColor, string>()
        {
            { ButtonColor.Red1, "background-color: #fff1f0; border-color: #fff1f0; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Red2, "background-color: #ffccc7; border-color: #ffccc7; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Red3, "background-color: #ffa39e; border-color: #ffa39e; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Red4, "background-color: #ff7875; border-color: #ff7875; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Red5, "background-color: #ff4d4f; border-color: #ff4d4f; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Red6, "background-color: #f5222d; border-color: #f5222d; color: rgb(255,255,255);" },
            { ButtonColor.Red7, "background-color: #cf1322; border-color: #cf1322; color: rgb(255,255,255);" },
            { ButtonColor.Red8, "background-color: #a8071a; border-color: #a8071a; color: rgb(255,255,255);" },
            { ButtonColor.Red9, "background-color: #820014; border-color: #820014; color: rgb(255,255,255);" },
            { ButtonColor.Red10, "background-color: #5c0011; border-color: #5c0011; color: rgb(255,255,255);" },
            { ButtonColor.Volcano1, "background-color: #fff2e8; border-color: #fff2e8; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Volcano2, "background-color: #ffd8bf; border-color: #ffd8bf; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Volcano3, "background-color: #ffbb96; border-color: #ffbb96; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Volcano4, "background-color: #ff9c6e; border-color: #ff9c6e; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Volcano5, "background-color: #ff7a45; border-color: #ff7a45; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Volcano6, "background-color: #fa541c; border-color: #fa541c; color: rgb(255,255,255);" },
            { ButtonColor.Volcano7, "background-color: #d4380d; border-color: #d4380d; color: rgb(255,255,255);" },
            { ButtonColor.Volcano8, "background-color: #ad2102; border-color: #ad2102; color: rgb(255,255,255);" },
            { ButtonColor.Volcano9, "background-color: #871400; border-color: #871400; color: rgb(255,255,255);" },
            { ButtonColor.Volcano10, "background-color: #610b00; border-color: #610b00; color: rgb(255,255,255);" },
            { ButtonColor.Orange1, "background-color: #fff7e6; border-color: #fff7e6; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Orange2, "background-color: #ffe7ba; border-color: #ffe7ba; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Orange3, "background-color: #ffd591; border-color: #ffd591; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Orange4, "background-color: #ffc069; border-color: #ffc069; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Orange5, "background-color: #ffa940; border-color: #ffa940; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Orange6, "background-color: #fa8c16; border-color: #fa8c16; color: rgb(255,255,255);" },
            { ButtonColor.Orange7, "background-color: #d46b08; border-color: #d46b08; color: rgb(255,255,255);" },
            { ButtonColor.Orange8, "background-color: #ad4e00; border-color: #ad4e00; color: rgb(255,255,255);" },
            { ButtonColor.Orange9, "background-color: #873800; border-color: #873800; color: rgb(255,255,255);" },
            { ButtonColor.Orange10, "background-color: #612500; border-color: #612500; color: rgb(255,255,255);" },
            { ButtonColor.Gold1, "background-color: #fffbe6; border-color: #fffbe6; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Gold2, "background-color: #fff1b8; border-color: #fff1b8; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Gold3, "background-color: #ffe58f; border-color: #ffe58f; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Gold4, "background-color: #ffd666; border-color: #ffd666; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Gold5, "background-color: #ffc53d; border-color: #ffc53d; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Gold6, "background-color: #faad14; border-color: #faad14; color: rgb(255,255,255);" },
            { ButtonColor.Gold7, "background-color: #d48806; border-color: #d48806; color: rgb(255,255,255);" },
            { ButtonColor.Gold8, "background-color: #ad6800; border-color: #ad6800; color: rgb(255,255,255);" },
            { ButtonColor.Gold9, "background-color: #874d00; border-color: #874d00; color: rgb(255,255,255);" },
            { ButtonColor.Gold10, "background-color: #613400; border-color: #613400; color: rgb(255,255,255);" },
            { ButtonColor.Yellow1, "background-color: #feffe6; border-color: #feffe6; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Yellow2, "background-color: #ffffb8; border-color: #ffffb8; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Yellow3, "background-color: #fffb8f; border-color: #fffb8f; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Yellow4, "background-color: #fff566; border-color: #fff566; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Yellow5, "background-color: #ffec3d; border-color: #ffec3d; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Yellow6, "background-color: #fadb14; border-color: #fadb14; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Yellow7, "background-color: #d4b106; border-color: #d4b106; color: rgb(255,255,255);" },
            { ButtonColor.Yellow8, "background-color: #ad8b00; border-color: #ad8b00; color: rgb(255,255,255);" },
            { ButtonColor.Yellow9, "background-color: #876800; border-color: #876800; color: rgb(255,255,255);" },
            { ButtonColor.Yellow10, "background-color: #614700; border-color: #614700; color: rgb(255,255,255);" },
            { ButtonColor.Lime1, "background-color: #fcffe6; border-color: #fcffe6; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Lime2, "background-color: #f4ffb8; border-color: #f4ffb8; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Lime3, "background-color: #eaff8f; border-color: #eaff8f; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Lime4, "background-color: #d3f261; border-color: #d3f261; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Lime5, "background-color: #bae637; border-color: #bae637; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Lime6, "background-color: #a0d911; border-color: #a0d911; color: rgb(255,255,255);" },
            { ButtonColor.Lime7, "background-color: #7cb305; border-color: #7cb305; color: rgb(255,255,255);" },
            { ButtonColor.Lime8, "background-color: #5b8c00; border-color: #5b8c00; color: rgb(255,255,255);" },
            { ButtonColor.Lime9, "background-color: #3f6600; border-color: #3f6600; color: rgb(255,255,255);" },
            { ButtonColor.Lime10, "background-color: #254000; border-color: #254000; color: rgb(255,255,255);" },
            { ButtonColor.Green1, "background-color: #f6ffed; border-color: #f6ffed; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Green2, "background-color: #d9f7be; border-color: #d9f7be; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Green3, "background-color: #b7eb8f; border-color: #b7eb8f; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Green4, "background-color: #95de64; border-color: #95de64; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Green5, "background-color: #73d13d; border-color: #73d13d; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Green6, "background-color: #52c41a; border-color: #52c41a; color: rgb(255,255,255);" },
            { ButtonColor.Green7, "background-color: #389e0d; border-color: #389e0d; color: rgb(255,255,255);" },
            { ButtonColor.Green8, "background-color: #237804; border-color: #237804; color: rgb(255,255,255);" },
            { ButtonColor.Green9, "background-color: #135200; border-color: #135200; color: rgb(255,255,255);" },
            { ButtonColor.Green10, "background-color: #092b00; border-color: #092b00; color: rgb(255,255,255);" },
            { ButtonColor.Cyan1, "background-color: #e6fffb; border-color: #e6fffb; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Cyan2, "background-color: #b5f5ec; border-color: #b5f5ec; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Cyan3, "background-color: #87e8de; border-color: #87e8de; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Cyan4, "background-color: #5cdbd3; border-color: #5cdbd3; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Cyan5, "background-color: #36cfc9; border-color: #36cfc9; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Cyan6, "background-color: #13c2c2; border-color: #13c2c2; color: rgb(255,255,255);" },
            { ButtonColor.Cyan7, "background-color: #08979c; border-color: #08979c; color: rgb(255,255,255);" },
            { ButtonColor.Cyan8, "background-color: #006d75; border-color: #006d75; color: rgb(255,255,255);" },
            { ButtonColor.Cyan9, "background-color: #00474f; border-color: #00474f; color: rgb(255,255,255);" },
            { ButtonColor.Cyan10, "background-color: #002329; border-color: #002329; color: rgb(255,255,255);" },
            { ButtonColor.Blue1, "background-color: #e6f7ff; border-color: #e6f7ff; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Blue2, "background-color: #bae7ff; border-color: #bae7ff; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Blue3, "background-color: #91d5ff; border-color: #91d5ff; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Blue4, "background-color: #69c0ff; border-color: #69c0ff; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Blue5, "background-color: #40a9ff; border-color: #40a9ff; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Blue6, "background-color: #1890ff; border-color: #1890ff; color: rgb(255,255,255);" },
            { ButtonColor.Blue7, "background-color: #096dd9; border-color: #096dd9; color: rgb(255,255,255);" },
            { ButtonColor.Blue8, "background-color: #0050b3; border-color: #0050b3; color: rgb(255,255,255);" },
            { ButtonColor.Blue9, "background-color: #003a8c; border-color: #003a8c; color: rgb(255,255,255);" },
            { ButtonColor.Blue10, "background-color: #002766; border-color: #002766; color: rgb(255,255,255);" },
            { ButtonColor.Geekblue1, "background-color: #f0f5ff; border-color: #f0f5ff; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Geekblue2, "background-color: #d6e4ff; border-color: #d6e4ff; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Geekblue3, "background-color: #adc6ff; border-color: #adc6ff; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Geekblue4, "background-color: #85a5ff; border-color: #85a5ff; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Geekblue5, "background-color: #597ef7; border-color: #597ef7; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Geekblue6, "background-color: #2f54eb; border-color: #2f54eb; color: rgb(255,255,255);" },
            { ButtonColor.Geekblue7, "background-color: #1d39c4; border-color: #1d39c4; color: rgb(255,255,255);" },
            { ButtonColor.Geekblue8, "background-color: #10239e; border-color: #10239e; color: rgb(255,255,255);" },
            { ButtonColor.Geekblue9, "background-color: #061178; border-color: #061178; color: rgb(255,255,255);" },
            { ButtonColor.Geekblue10, "background-color: #030852; border-color: #030852; color: rgb(255,255,255);" },
            { ButtonColor.Purple1, "background-color: #f9f0ff; border-color: #f9f0ff; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Purple2, "background-color: #efdbff; border-color: #efdbff; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Purple3, "background-color: #d3adf7; border-color: #d3adf7; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Purple4, "background-color: #b37feb; border-color: #b37feb; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Purple5, "background-color: #9254de; border-color: #9254de; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Purple6, "background-color: #722ed1; border-color: #722ed1; color: rgb(255,255,255);" },
            { ButtonColor.Purple7, "background-color: #531dab; border-color: #531dab; color: rgb(255,255,255);" },
            { ButtonColor.Purple8, "background-color: #391085; border-color: #391085; color: rgb(255,255,255);" },
            { ButtonColor.Purple9, "background-color: #22075e; border-color: #22075e; color: rgb(255,255,255);" },
            { ButtonColor.Purple10, "background-color: #120338; border-color: #120338; color: rgb(255,255,255);" },
            { ButtonColor.Magenta1, "background-color: #fff0f6; border-color: #fff0f6; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Magenta2, "background-color: #ffd6e7; border-color: #ffd6e7; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Magenta3, "background-color: #ffadd2; border-color: #ffadd2; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Magenta4, "background-color: #ff85c0; border-color: #ff85c0; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Magenta5, "background-color: #f759ab; border-color: #f759ab; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Magenta6, "background-color: #eb2f96; border-color: #eb2f96; color: rgb(255,255,255);" },
            { ButtonColor.Magenta7, "background-color: #c41d7f; border-color: #c41d7f; color: rgb(255,255,255);" },
            { ButtonColor.Magenta8, "background-color: #9e1068; border-color: #9e1068; color: rgb(255,255,255);" },
            { ButtonColor.Magenta9, "background-color: #780650; border-color: #780650; color: rgb(255,255,255);" },
            { ButtonColor.Magenta10, "background-color: #520339; border-color: #520339; color: rgb(255,255,255);" },
            { ButtonColor.Gray1, "background-color: #ffffff; border-color: #ffffff; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Gray2, "background-color: #fafafa; border-color: #fafafa; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Gray3, "background-color: #f5f5f5; border-color: #f5f5f5; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Gray4, "background-color: #f0f0f0; border-color: #f0f0f0; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Gray5, "background-color: #d9d9d9; border-color: #d9d9d9; color: rgba(0,0,0,0.85);" },
            { ButtonColor.Gray6, "background-color: #bfbfbf; border-color: #bfbfbf; color: rgb(255,255,255);" },
            { ButtonColor.Gray7, "background-color: #8c8c8c; border-color: #8c8c8c; color: rgb(255,255,255);" },
            { ButtonColor.Gray8, "background-color: #595959; border-color: #595959; color: rgb(255,255,255);" },
            { ButtonColor.Gray9, "background-color: #434343; border-color: #434343; color: rgb(255,255,255);" },
            { ButtonColor.Gray10, "background-color: #262626; border-color: #262626; color: rgb(255,255,255);" },
            { ButtonColor.Gray11, "background-color: #1f1f1f; border-color: #1f1f1f; color: rgb(255,255,255);" },
            { ButtonColor.Gray12, "background-color: #141414; border-color: #141414; color: rgb(255,255,255);" },
            { ButtonColor.Gray13, "background-color: #000000; border-color: #000000; color: rgb(255,255,255);" }
        };

        [CascadingParameter(Name = "FormSize")]
        public string FormSize
        {
            get
            {
                return _formSize;
            }
            set
            {
                _formSize = value;

                Size = value;
            }
        }

        /// <summary>
        /// Set the color of the button.
        /// </summary>
        [Parameter]
        public ButtonColor Color { get; set; } = ButtonColor.None;

        /// <summary>
        /// Option to fit button width to its parent width
        /// </summary>
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
        [Parameter]
        public bool Danger { get; set; }

        /// <summary>
        ///  Whether the `Button` is disabled.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Make background transparent and invert text and border colors
        /// </summary>
        [Parameter]
        public bool Ghost { get; set; } = false;

        /// <summary>
        /// Set the original html type of the button element.
        /// </summary>
        [Parameter]
        public string HtmlType { get; set; } = "button";

        /// <summary>
        /// Set the icon component of button.
        /// </summary>
        [Parameter]
        public string Icon { get; set; }

        /// <summary>
        ///  Show loading indicator. You have to write the loading logic on your own. 
        /// </summary>
        [Parameter]
        public bool Loading
        {
            get => _loading;
            set
            {
                if (_loading != value)
                {
                    _loading = value;
                    UpdateIconDisplay(_loading);
                }
            }
        }

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
        [Parameter]
        public string Shape { get; set; } = null;

        /// <summary>
        /// Set the size of button.
        /// </summary>
        [Parameter]
        public string Size { get; set; } = AntSizeLDSType.Default;

        /// <summary>
        ///  Type of the button.
        /// </summary>
        [Parameter]
        public string Type { get; set; } = ButtonType.Default;

        public IList<Icon> Icons { get; set; } = new List<Icon>();

        protected string IconStyle { get; set; }

        private bool _animating = false;

        private string _btnWave = "--antd-wave-shadow-color: rgb(255, 120, 117);";
        private bool _loading = false;

        protected void SetClassMap()
        {
            var prefixName = "ant-btn";

            ClassMapper.Clear()
                .Add(prefixName)
                .GetIf(() => $"{prefixName}-{this.Type}", () => !string.IsNullOrEmpty(Type))
                .If($"{prefixName}-dangerous", () => Danger)
                .GetIf(() => $"{prefixName}-{Shape}", () => !string.IsNullOrEmpty(Shape))
                .If($"{prefixName}-lg", () => Size == "large")
                .If($"{prefixName}-sm", () => Size == "small")
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
            UpdateIconDisplay(_loading);
        }

        private void UpdateIconDisplay(bool loading)
        {
            IconStyle = $"display:{(loading ? "none" : "inline-block")}";
        }

        private async Task HandleOnClick(MouseEventArgs args)
        {
            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(args);
            }
        }

        private async Task OnMouseUp(MouseEventArgs args)
        {
            if (args.Button != 0 || this.Type == ButtonType.Link) return; //remove animating from Link Button
            this._animating = true;

            await Task.Run(async () =>
            {
                await Task.Delay(500);
                this._animating = false;

                await InvokeAsync(StateHasChanged);
            });
        }

        private void SetButtonColorStyle() => Style += _buttonColors.GetValueOrDefault(Color);
    }
}
