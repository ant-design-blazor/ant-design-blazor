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
        private int inputValue1 = 1;

        private void UpdateValue1(double value)
        {
            inputValue1 = (int)value;
        }

        private double inputValue2 = 0;

        private void UpdateValue2(double value)
        {
            inputValue2 = value;
        }

        private bool disabled;

        private void OnSwitch(bool args)
        {
            disabled = args;
        }

        private void OnChange1(double args)
        {
        }

        private void OnAfterChange1(object o, MouseEventArgs args)
        {
        }

        private void OnChange2(double args)
        {
        }

        private void OnAfterChange2(object o, MouseEventArgs args)
        {
        }

        private AntSliderMark[] marks1 = new AntSliderMark[]
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

        private bool reversed = true;

        private void OnSwitchReverse(bool args)
        {
            reversed = args;
        }
    }
}