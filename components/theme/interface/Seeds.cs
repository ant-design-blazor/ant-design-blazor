using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class GlobalToken
    {
        // public string ColorPrimary
        // {
        //     get => (string)_tokens["colorPrimary"];
        //     set => _tokens["colorPrimary"] = value;
        // }

        // public string ColorSuccess
        // {
        //     get => (string)_tokens["colorSuccess"];
        //     set => _tokens["colorSuccess"] = value;
        // }

        // public string ColorWarning
        // {
        //     get => (string)_tokens["colorWarning"];
        //     set => _tokens["colorWarning"] = value;
        // }

        // public string ColorError
        // {
        //     get => (string)_tokens["colorError"];
        //     set => _tokens["colorError"] = value;
        // }

        // public string ColorInfo
        // {
        //     get => (string)_tokens["colorInfo"];
        //     set => _tokens["colorInfo"] = value;
        // }

        // public string ColorTextBase
        // {
        //     get => (string)_tokens["colorTextBase"];
        //     set => _tokens["colorTextBase"] = value;
        // }

        // public string ColorBgBase
        // {
        //     get => (string)_tokens["colorBgBase"];
        //     set => _tokens["colorBgBase"] = value;
        // }
        //
        // public string ColorLink
        // {
        //     get => (string)_tokens["colorLink"];
        //     set => _tokens["colorLink"] = value;
        // }

        public string FontFamily
        {
            get => (string)_tokens["fontFamily"];
            set => _tokens["fontFamily"] = value;
        }

        public string FontFamilyCode
        {
            get => (string)_tokens["fontFamilyCode"];
            set => _tokens["fontFamilyCode"] = value;
        }

        // public int FontSize
        // {
        //     get => (int)_tokens["fontSize"];
        //     set => _tokens["fontSize"] = value;
        // }

        public int LineWidth
        {
            get => (int)_tokens["lineWidth"];
            set => _tokens["lineWidth"] = value;
        }

        public string LineType
        {
            get => (string)_tokens["lineType"];
            set => _tokens["lineType"] = value;
        }

        public int BorderRadius
        {
            get => (int)_tokens["borderRadius"];
            set => _tokens["borderRadius"] = value;
        }

        public int SizeUnit
        {
            get => (int)_tokens["sizeUnit"];
            set => _tokens["sizeUnit"] = value;
        }

        public int SizeStep
        {
            get => (int)_tokens["sizeStep"];
            set => _tokens["sizeStep"] = value;
        }

        public int SizePopupArrow
        {
            get => (int)_tokens["sizePopupArrow"];
            set => _tokens["sizePopupArrow"] = value;
        }

        public int ControlHeight
        {
            get => (int)_tokens["controlHeight"];
            set => _tokens["controlHeight"] = value;
        }

        public int ZIndexBase
        {
            get => (int)_tokens["zIndexBase"];
            set => _tokens["zIndexBase"] = value;
        }

        public int ZIndexPopupBase
        {
            get => (int)_tokens["zIndexPopupBase"];
            set => _tokens["zIndexPopupBase"] = value;
        }

        public int OpacityImage
        {
            get => (int)_tokens["opacityImage"];
            set => _tokens["opacityImage"] = value;
        }

        public float MotionUnit
        {
            get => (float)_tokens["motionUnit"];
            set => _tokens["motionUnit"] = value;
        }

        public int MotionBase
        {
            get => (int)_tokens["motionBase"];
            set => _tokens["motionBase"] = value;
        }

        public string MotionEaseOutCirc
        {
            get => (string)_tokens["motionEaseOutCirc"];
            set => _tokens["motionEaseOutCirc"] = value;
        }

        public string MotionEaseInOutCirc
        {
            get => (string)_tokens["motionEaseInOutCirc"];
            set => _tokens["motionEaseInOutCirc"] = value;
        }

        public string MotionEaseInOut
        {
            get => (string)_tokens["motionEaseInOut"];
            set => _tokens["motionEaseInOut"] = value;
        }

        public string MotionEaseOutBack
        {
            get => (string)_tokens["motionEaseOutBack"];
            set => _tokens["motionEaseOutBack"] = value;
        }

        public string MotionEaseInBack
        {
            get => (string)_tokens["motionEaseInBack"];
            set => _tokens["motionEaseInBack"] = value;
        }

        public string MotionEaseInQuint
        {
            get => (string)_tokens["motionEaseInQuint"];
            set => _tokens["motionEaseInQuint"] = value;
        }

        public string MotionEaseOutQuint
        {
            get => (string)_tokens["motionEaseOutQuint"];
            set => _tokens["motionEaseOutQuint"] = value;
        }

        public string MotionEaseOut
        {
            get => (string)_tokens["motionEaseOut"];
            set => _tokens["motionEaseOut"] = value;
        }

        public bool Wireframe
        {
            get => (bool)_tokens["wireframe"];
            set => _tokens["wireframe"] = value;
        }

        public bool Motion
        {
            get => (bool)_tokens["motion"];
            set => _tokens["motion"] = value;
        }

    }

}
