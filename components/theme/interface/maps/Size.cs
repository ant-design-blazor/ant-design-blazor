using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class GlobalToken
    {
        public int SizeXXL
        {
            get => (int)_tokens["sizeXXL"];
            set => _tokens["sizeXXL"] = value;
        }

        public int SizeXL
        {
            get => (int)_tokens["sizeXL"];
            set => _tokens["sizeXL"] = value;
        }

        public int SizeLG
        {
            get => (int)_tokens["sizeLG"];
            set => _tokens["sizeLG"] = value;
        }

        public int SizeMD
        {
            get => (int)_tokens["sizeMD"];
            set => _tokens["sizeMD"] = value;
        }

        public int SizeMS
        {
            get => (int)_tokens["sizeMS"];
            set => _tokens["sizeMS"] = value;
        }

        public int Size
        {
            get => (int)_tokens["size"];
            set => _tokens["size"] = value;
        }

        public int SizeSM
        {
            get => (int)_tokens["sizeSM"];
            set => _tokens["sizeSM"] = value;
        }

        public int SizeXS
        {
            get => (int)_tokens["sizeXS"];
            set => _tokens["sizeXS"] = value;
        }

        public int SizeXXS
        {
            get => (int)_tokens["sizeXXS"];
            set => _tokens["sizeXXS"] = value;
        }

    }

    public partial class GlobalToken
    {
        public int ControlHeightXS
        {
            get => (int)_tokens["controlHeightXS"];
            set => _tokens["controlHeightXS"] = value;
        }

        public int ControlHeightSM
        {
            get => (int)_tokens["controlHeightSM"];
            set => _tokens["controlHeightSM"] = value;
        }

        public int ControlHeightLG
        {
            get => (int)_tokens["controlHeightLG"];
            set => _tokens["controlHeightLG"] = value;
        }

    }

}
