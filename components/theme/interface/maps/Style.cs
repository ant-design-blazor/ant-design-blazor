using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class GlobalToken
    {
        public double LineWidthBold
        {
            get => (double)_tokens["lineWidthBold"];
            set => _tokens["lineWidthBold"] = value;
        }

        public double BorderRadiusXS
        {
            get => (double)_tokens["borderRadiusXS"];
            set => _tokens["borderRadiusXS"] = value;
        }

        public double BorderRadiusSM
        {
            get => (double)_tokens["borderRadiusSM"];
            set => _tokens["borderRadiusSM"] = value;
        }

        public double BorderRadiusLG
        {
            get => (double)_tokens["borderRadiusLG"];
            set => _tokens["borderRadiusLG"] = value;
        }

        public double BorderRadiusOuter
        {
            get => (double)_tokens["borderRadiusOuter"];
            set => _tokens["borderRadiusOuter"] = value;
        }

    }

}