using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class GlobalToken
    {
        public int LineWidthBold
        {
            get => (int)_tokens["lineWidthBold"];
            set => _tokens["lineWidthBold"] = value;
        }

        public int BorderRadiusXS
        {
            get => (int)_tokens["borderRadiusXS"];
            set => _tokens["borderRadiusXS"] = value;
        }

        public int BorderRadiusSM
        {
            get => (int)_tokens["borderRadiusSM"];
            set => _tokens["borderRadiusSM"] = value;
        }

        public int BorderRadiusLG
        {
            get => (int)_tokens["borderRadiusLG"];
            set => _tokens["borderRadiusLG"] = value;
        }

        public int BorderRadiusOuter
        {
            get => (int)_tokens["borderRadiusOuter"];
            set => _tokens["borderRadiusOuter"] = value;
        }

    }

}
