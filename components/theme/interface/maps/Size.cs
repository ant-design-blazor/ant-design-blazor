using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class GlobalToken
    {
        public double SizeXXL
        {
            get => (double)_tokens["sizeXXL"];
            set => _tokens["sizeXXL"] = value;
        }

        public double SizeXL
        {
            get => (double)_tokens["sizeXL"];
            set => _tokens["sizeXL"] = value;
        }

        public double SizeLG
        {
            get => (double)_tokens["sizeLG"];
            set => _tokens["sizeLG"] = value;
        }

        public double SizeMD
        {
            get => (double)_tokens["sizeMD"];
            set => _tokens["sizeMD"] = value;
        }

        public double SizeMS
        {
            get => (double)_tokens["sizeMS"];
            set => _tokens["sizeMS"] = value;
        }

        public double Size
        {
            get => (double)_tokens["size"];
            set => _tokens["size"] = value;
        }

        public double SizeSM
        {
            get => (double)_tokens["sizeSM"];
            set => _tokens["sizeSM"] = value;
        }

        public double SizeXS
        {
            get => (double)_tokens["sizeXS"];
            set => _tokens["sizeXS"] = value;
        }

        public double SizeXXS
        {
            get => (double)_tokens["sizeXXS"];
            set => _tokens["sizeXXS"] = value;
        }

    }

    public partial class GlobalToken
    {
        public double ControlHeightXS
        {
            get => (double)_tokens["controlHeightXS"];
            set => _tokens["controlHeightXS"] = value;
        }

        public double ControlHeightSM
        {
            get => (double)_tokens["controlHeightSM"];
            set => _tokens["controlHeightSM"] = value;
        }

        public double ControlHeightLG
        {
            get => (double)_tokens["controlHeightLG"];
            set => _tokens["controlHeightLG"] = value;
        }

    }

}