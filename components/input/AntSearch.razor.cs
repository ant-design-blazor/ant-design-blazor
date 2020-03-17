using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace AntBlazor
{
    public partial class AntSearch : AntInputBase
    {
        private bool _isSearching;

        //<input class="@ClassMapper.Class" style="@Style" @attributes="Attributes" Id="@Id" type="@_type"
        //              placeholder = "@placeholder" value = "@Value"
        //              @onchange = "OnChangeAsync" @onkeypress = "OnPressEnterAsync"
        //              @oninput = "OnInputAsync" />
        private RenderFragment _inputFragment;

        private RenderFragment _btnAddonFragment;

        [Parameter]
        public EventCallback<EventArgs> onSearch { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _inputFragment = new RenderFragment((builder) =>
            {
                int i = 0;
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

            GenerateSearchBtn();
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

        private async void Search(MouseEventArgs args)
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

        private int i = 0;

        private void GenerateSearchBtn()
        {
            _btnAddonFragment = new RenderFragment((builder) =>
            {
                builder.OpenElement(i++, "span");
                builder.AddAttribute(i++, "class", "ant-input-group-addon");
                //<AntButton class="ant-input-search-button" type="primary" size="@size" icon="search" onclick="Search" />
                builder.OpenComponent<AntButton>(i++);
                builder.AddAttribute(i++, "class", "ant-input-search-button");
                builder.AddAttribute(i++, "type", "primary");
                builder.AddAttribute(i++, "size", size);
                if (_isSearching)
                {
                    builder.AddAttribute(i++, "loading", true);
                }
                else
                {
                    EventCallback<MouseEventArgs> e = new EventCallbackFactory().Create(this, Search);
                    builder.AddAttribute(i++, "onclick", e);
                }

                if (Attributes != null && Attributes.ContainsKey("enterButton"))
                {
                    if (Attributes["enterButton"].ToString() == true.ToString()) // show search icon
                    {
                        builder.AddAttribute(i++, "icon", "search");
                    }
                    else
                    {
                        //builder.AddContent(i++, Attributes["enterButton"].ToString());
                        builder.AddAttribute(i++, "ChildContent", new RenderFragment((b) =>
                        {
                            b.AddContent(i++, "Search");
                        }));
                    }
                }

                builder.CloseComponent();
                builder.CloseElement();
            });
        }
    }
}