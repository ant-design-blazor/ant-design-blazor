using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class ColorPicker : OverlayTrigger
    {
        /*
         * 第一种是光谱模式
         * 第二种是表格模式
         * 第三种是调色板模式
         */

        #region Parameter

        [Parameter]
        public string Title { get; set; } = string.Empty;

        [Parameter]
        public string Content { get; set; } = string.Empty;

        [Parameter]
        public bool ArrowPointAtCenter { get; set; } = false;

        [Parameter]
        public double MouseEnterDelay { get; set; } = 0.1;

        [Parameter]
        public double MouseLeaveDelay { get; set; } = 0.1;

        [Parameter]
        public AntColor Value
        {
            get => _value;
            set => SetColorAsync(value);
        }

        [Parameter]
        public string TextValue
        {
            get => _value.ToString(AntColorOutputFormats.HexA);
            set => _value = new AntColor(value);
        }

        [Parameter]
        public EventCallback<string> TextValueChanged { get; set; }

        #endregion

        private double _selectorX;
        private double _selectorY;
        private double _selctorSize = 26.0;
        private double _maxY = 250;
        private double _maxX = 312;
        private AntColor _value;
        private bool _close = false;

        private static Dictionary<int, (Func<int, int> r, Func<int, int> g, Func<int, int> b, string dominantColorPart)> _rgbToHueMapper = new()
        {
            { 0, ((x) => 255, x => x, x => 0, "rb") },
            { 1, ((x) => 255 - x, x => 255, x => 0, "gb") },
            { 2, ((x) => 0, x => 255, x => x, "gr") },
            { 3, ((x) => 0, x => 255 - x, x => 255, "br") },
            { 4, ((x) => x, x => 0, x => 255, "bg") },
            { 5, ((x) => 255, x => 0, x => 255 - x, "rg") },
        };

        private string GetSelectorLocation() => $"translate({Math.Round(_selectorX, 2).ToString()}px, {Math.Round(_selectorY, 2).ToString()}px);";

        public ColorPicker()
        {
            PrefixCls = "ant-popover";
            Placement = Placement.Top;
            _value = new AntColor(0, 0, 0, 255);
        }

        private void OnSelectorClicked(MouseEventArgs e)
        {
            SetSelectorBasedOnMouseEvents(e, false);
            HandleColorOverlayClicked();
        }

        private void SetSelectorBasedOnMouseEvents(MouseEventArgs e, bool offsetIsAbsolute)
        {
#if NET5_0_OR_GREATER
            _selectorX = (offsetIsAbsolute == true ? e.OffsetX : (e.OffsetX - _selctorSize / 2.0) + _selectorX).EnsureRange(_maxX);
            _selectorY = (offsetIsAbsolute == true ? e.OffsetY : (e.OffsetY - _selctorSize / 2.0) + _selectorY).EnsureRange(_maxY);
#endif
        }

        //关闭
        private void OnColorOverlayClick(MouseEventArgs e)
        {
            SetSelectorBasedOnMouseEvents(e, true);
            HandleColorOverlayClicked();
        }

        private void HandleColorOverlayClicked()
        {
            UpdateColorBaseOnSelection();
        }

        //底色改变
        public void UpdateBaseColorSlider(double value) {
            _value = _value.SetH(value);
            UpdateBaseColor();
        } 

        //透明度改变
        public void SetAlpha(double value) {
            _value = _value.SetAlpha((int)value);
        } 

        //设置新的颜色
        private async Task SetColorAsync(AntColor value)
        {
            if (value == null) { return; }

            var rgbChanged = value != _value;
            var hslChanged = _value != null && value.HslChanged(_value);
            var shouldUpdateBinding = _value != null
                                      && (rgbChanged || hslChanged);
            _value = value;

            if (rgbChanged)
            {
                UpdateBaseColor();
                UpdateColorSelectorBasedOnRgb();
            }
        }

        //根据RGB值更改
        private void UpdateColorSelectorBasedOnRgb()
        {
            var hueValue = (int)MathExtensions.Map(0, 360, 0, 6 * 255, _value.H);
            var index = hueValue / 255;
            if (index == 6)
            {
                index = 5;
            }

            var section = _rgbToHueMapper[index];

            var colorValues = section.dominantColorPart switch
            {
                "rb" => (_value.R, _value.B),
                "rg" => (_value.R, _value.G),
                "gb" => (_value.G, _value.B),
                "gr" => (_value.G, _value.R),
                "br" => (_value.B, _value.R),
                "bg" => (_value.B, _value.G),
                _ => (255, 255)
            };

            var primaryDiff = 255 - colorValues.Item1;
            var primaryDiffDelta = colorValues.Item1 / 255.0;

            _selectorY = MathExtensions.Map(0, 255, 0, _maxY, primaryDiff);

            var secondaryColorX = colorValues.Item2 * (1.0 / primaryDiffDelta);
            var relation = (255 - secondaryColorX) / 255.0;

            _selectorX = relation * _maxX;
        }

        //更改Base颜色
        private void UpdateBaseColor()
        {
            var index = (int)_value.H / 60;
            if (index == 6)
            {
                index = 5; 
            }

            var valueInDeg = (int)_value.H - (index * 60);
            var value = (int)(MathExtensions.Map(0, 60, 0, 255, valueInDeg));
            var section = _rgbToHueMapper[index];

            _value = new(section.r(value), section.g(value), section.b(value), 255);
        }

        //根据鼠标选择内容，进行更新
        private void UpdateColorBaseOnSelection()
        {
            var x = _selectorX / _maxX;

            var r_x = 255 - (int)((255 - _value.R) * x);
            var g_x = 255 - (int)((255 - _value.G) * x);
            var b_x = 255 - (int)((255 - _value.B) * x);

            var y = 1.0 - _selectorY / _maxY;

            var r = r_x * y;
            var g = g_x * y;
            var b = b_x * y;
            _value = new AntColor((byte)r, (byte)g, (byte)b, _value.A);
        }

        //颜色值输入
        public void SetInputString(string input)
        {
            AntColor color;
            try
            {
                color = new AntColor(input);
            }
            catch (Exception)
            {
                return;
            }

            _value = color;
        }

        //Overlay进入的class
        internal override string GetOverlayEnterClass()
        {
            return "ant-zoom-big-enter ant-zoom-big-enter-active ant-zoom-big";
        }

        //Overlay离开的Class
        internal override string GetOverlayLeaveClass()
        {
            return "ant-zoom-big-leave ant-zoom-big-leave-active ant-zoom-big";
        }

        //显示
        internal override async Task Show(int? overlayLeft = null, int? overlayTop = null)
        {
            if (Trigger.Contains(AntDesign.Trigger.Hover))
            {
                await Task.Delay((int)(MouseEnterDelay * 1000));
            }
            _close = false;
            await base.Show(overlayLeft, overlayTop);
        }

        //关闭
        internal override async Task Hide(bool force = false)
        {
            if (Trigger.Contains(AntDesign.Trigger.Hover))
            {
                await Task.Delay((int)(MouseLeaveDelay * 1000));
            }
            if(_close)
            {
                await base.Hide(force);
            }
        }

        private void ClickClose()
        {
            _close = true;
            TextValue = _value.ToString(AntColorOutputFormats.HexA);
            OnTextValueChanged(_value.ToString(AntColorOutputFormats.HexA));
            Hide();
        }

        private void OnTextValueChanged(string value)
        {
            if (TextValueChanged.HasDelegate)
            {
                TextValueChanged.InvokeAsync(value);
            }
        }
    }
}
