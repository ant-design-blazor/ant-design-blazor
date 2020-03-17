using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace AntBlazor
{
    public partial class AntSearch : AntInputBase
    {
        private bool _isSearching;

        [Parameter]
        public EventCallback<EventArgs> onSearch { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _inputFragment = new RenderFragment((builder) =>
            {
                int i = 0;
                //<input class="@ClassMapper.Class" style="@Style" @attributes="Attributes" Id="@Id" type="@_type"
                //              placeholder = "@placeholder" value = "@Value"
                //              @onchange = "OnChangeAsync" @onkeypress = "OnPressEnterAsync"
                //              @oninput = "OnInputAsync" />
                builder.OpenElement(i++, "input");
                builder.AddAttribute(i++, "class", ClassMapper.Class);
                builder.AddAttribute(i++, "style", Style);
                if (Attributes != null)
                {
                    foreach (var pair in Attributes)
                    {
                        builder.AddAttribute(i++, pair.Key, pair.Value);
                    }
                }
                builder.AddAttribute(i++, "Id", Id);
                builder.AddAttribute(i++, "type", _type);
                builder.AddAttribute(i++, "placeholder", placeholder);
                builder.AddAttribute(i++, "value", Value);
                builder.AddAttribute(i++, "onchange", onChange);
                builder.AddAttribute(i++, "onkeypress", onPressEnter);
                //builder.AddAttribute(i++, "oninput", OnInputAsync);
                builder.CloseElement();

            });

            GenerateInput();
        }

        protected override void SetClasses()
        {
            base.SetClasses();

            if (size == AntInputSize.Large)
            {
                _groupClass = string.Join(" ", _groupClass, $"{PrefixCls}-search-large");
            }
            else if (size == AntInputSize.Small)
            {
                _groupClass = string.Join(" ", _groupClass, $"{PrefixCls}-search-small");
            }
        }

        private async void Search()
        {
            _isSearching = true;
            StateHasChanged();
            if (onSearch.HasDelegate)
            {
                await onSearch.InvokeAsync(EventArgs.Empty);
            }
            await Task.Delay(TimeSpan.FromSeconds(10));
            _isSearching = false;
            StateHasChanged();
        }
    }
}