using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Mentions
    {
        [Parameter] public bool AutoFocus { get; set; }
        [Parameter] public bool Disable { get; set; }
        [Parameter] public bool Readonly { get; set; }
        [Parameter] public string Prefix { get; set; } = "@,#";   //“@，#”
        [Parameter] public string Split { get; set; } = " ";
        [Parameter] public string DefaultValue { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public string Value { get; set; }
        [Parameter] public string Placement { get; set; } = "bottom";
        [Parameter] public string Direction { get; set; } = "ltr";
        [Parameter] public int Rows { get; set; } = 1;
        [Parameter] public bool Loading { get; set; } = false;
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public EventCallback<ChangeEventArgs> ValueChange { get; set; }
        [Parameter] public EventCallback<FocusEventArgs> OnFocus { get; set; }
        [Parameter] public EventCallback<FocusEventArgs> OnBlur { get; set; }
        [Parameter] public EventCallback<EventArgs> OnSearch { get; set; }

        [Inject]  private DomEventService DomEventService { get; set; }
        private List<MentionsOption> LstOriginalOptions { get; set; } = new List<MentionsOption>();
        private List<MentionsOption> LstOptions { get; set; } = new List<MentionsOption>();

        private bool Focused { get; set; }
        private bool ShowMentions { get; set; }

        private void SetClassMap()
        {
            var prefixCls = "ant-mentions";
            this.ClassMapper.Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-disable", () => this.Disable)
                .If($"{prefixCls}-focused", () => this.Focused)
                .If($"{prefixCls}-rtl", () => this.Direction == "rtl")
                ;
        }

        protected override void OnInitialized()
        {
            SetClassMap();
            base.OnInitialized();
        }

        private  void Keyup(KeyboardEventArgs args)
        {
            if (args == null) return;
            if (Prefix.Contains(args.Key, StringComparison.Ordinal))
            {
                ShowMentions = Prefix.Contains(args.Key, StringComparison.Ordinal);
                StateHasChanged();
                return;
            }

            if (args.Code == "Space")
            {
                ShowMentions = false;
                StateHasChanged();
                return;
            }


        }

        private void OnChange(ChangeEventArgs args)
        {
            Console.WriteLine(this.Value); 
            return;
        }

        private void OnKeyPress(KeyboardEventArgs args)
        {
          


        
            //this.LstOptions = IsIncluded(this.Value);

          

            return;
        }


        /// <summary>
        /// return a  new list of mentionsOption, when all item included the specificed text
        /// </summary>
        /// <param name="textValue"></param>
        /// <returns></returns>
        private List<MentionsOption> IsIncluded(string textValue)
        {

            if (string.IsNullOrEmpty(textValue))
            {
                return LstOriginalOptions;
            }

            List<MentionsOption> lst = new List<MentionsOption>();
            foreach (var item in LstOriginalOptions)
            {
                if (item.Value.Contains(textValue, StringComparison.Ordinal))
                {
                    lst.Add(item);
                }
            }
            return lst;
        }



        
        public void AddOption(MentionsOption option)
        {
            if (option == null) return;
            Console.WriteLine($"id={option.Id}");
           var opt = LstOriginalOptions.Find(x => x.Id == option.Id);
            if (opt == null)
            {
                LstOriginalOptions.Add(option);
            }



        }

        private void Onchange(ChangeEventArgs args)
        {
            if (args == null) return;
            if (ValueChange.HasDelegate)
                ValueChange.InvokeAsync(args);
        }
        private async void Onfocus(FocusEventArgs args)
        {
            if (args == null) return;

            Focused = true;
           if (OnFocus.HasDelegate)
               await OnFocus.InvokeAsync(args);

            await JsInvokeAsync(JSInteropConstants.focus, Ref);
            StateHasChanged();
        }

        private async void Onblur(FocusEventArgs args)
        {
            if (args == null) return;
            Focused = false;
            if (OnBlur.HasDelegate)
                await OnBlur.InvokeAsync(args);
                await JsInvokeAsync(JSInteropConstants.blur, Ref);
            StateHasChanged();
        }



    }
}
