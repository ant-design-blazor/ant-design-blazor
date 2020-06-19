using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Option
    {
        [CascadingParameter] public Mentions Mentions { get; set; }
        [Parameter] public string Value { get; set; }

        [Parameter] public bool Disable { get; set; }

        [Parameter] public RenderFragment ChildContent  { get; set; }


        public bool Selected { get; set; }

        public bool Active { get; set; }



        protected override void OnInitialized()
        {
            if (this is Option option)
            {
                this.Mentions?.Options.Add(option);
            }

            SetClassMap();

            base.OnInitialized();
        }


        private void SetClassMap()
        {
            var prefixCls = "ant-mentions-dropdown-menu-item";
            this.ClassMapper.Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-disable", () => this.Disable)
                .If($"{prefixCls}-selected", () => this.Selected)
                .If($"{prefixCls}-active", () => this.Active)
                ;
        }


        private void OnMouseEnter(MouseEventArgs args)
        {

            this.Active = true;
            StateHasChanged();
        }

        private void OnMouseLeave(MouseEventArgs args)
        {
            this.Active = false;
            StateHasChanged();

        }


        private void OnClick(MouseEventArgs args)
        {
            if (args.Button == 0)   //left click
            {
                this.Selected = true;
            }
            else
            {
                this.Selected = false;
            }
            StateHasChanged();

        }

    }
}
