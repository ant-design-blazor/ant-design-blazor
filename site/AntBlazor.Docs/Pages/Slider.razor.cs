using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntBlazor.Docs.Pages
{
    public partial class Slider
    {
        private int _inputValue1 = 1;

        private void UpdateValue1(double value)
        {
            _inputValue1 = (int)value;
        }

        private double _inputValue2 = 0;

        private void UpdateValue2(double value)
        {
            _inputValue2 = value;
        }

        private bool _disabled;

        private void OnSwitch(bool args)
        {
            _disabled = args;
        }

        private void OnChange1(double args)
        {
        }

        private void OnAfterChange1()
        {
        }

        private void OnChange2(double args)
        {
        }

        private void OnAfterChange2()
        {
        }

        private AntSliderMark[] _marks1 =
        {
            new AntSliderMark(0, "0℃"),
            new AntSliderMark(26, "26℃"),
            new AntSliderMark(37, "37℃"),
            new AntSliderMark(100, (b)=>{
                b.OpenElement(0,"strong");
                b.AddContent(1,"100℃");
                b.CloseElement();
            }, "color: #f50;")
        };

        private bool _reversed = true;

        private void OnSwitchReverse(bool args)
        {
            _reversed = args;
        }
    }
}
