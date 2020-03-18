using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace AntBlazor
{
    public partial class AntSearch : AntInput
    {
        private bool _isSearching;

        [Parameter]
        public EventCallback<EventArgs> onSearch { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Attributes == null || !Attributes.ContainsKey("enterButton"))
            {
                suffix = new RenderFragment((builder) =>
                {
                    builder.OpenComponent<AntIcon>(_renderSequence++);
                    builder.AddAttribute(_renderSequence++, "class", $"{PrefixCls}-search-icon");
                    builder.AddAttribute(_renderSequence++, "type", "search");
                    builder.AddAttribute(_renderSequence++, "onclick", _callbackFactory.Create<MouseEventArgs>(this, Search));
                    builder.CloseComponent();
                });
            }
            else
            {
                addOnAfter = new RenderFragment((builder) =>
                {
                    builder.OpenComponent<AntButton>(_renderSequence++);
                    builder.AddAttribute(_renderSequence++, "class", $"{PrefixCls}-search-button");
                    builder.AddAttribute(_renderSequence++, "type", "primary");
                    builder.AddAttribute(_renderSequence++, "size", size);
                    if (_isSearching)
                    {
                        builder.AddAttribute(_renderSequence++, "loading", true);
                    }
                    else
                    {
                        EventCallback<MouseEventArgs> e = new EventCallbackFactory().Create(this, Search);
                        builder.AddAttribute(_renderSequence++, "onclick", e);
                    }

                    if (Attributes["enterButton"].ToString() == true.ToString()) // show search icon button
                    {
                        builder.AddAttribute(_renderSequence++, "icon", "search");
                    }
                    else // show search content button
                    {
                        builder.AddAttribute(_renderSequence++, "ChildContent", new RenderFragment((b) =>
                        {
                            b.AddContent(_renderSequence++, Attributes["enterButton"].ToString());
                        }));
                    }

                    builder.CloseComponent();
                });
            }
        }

        protected override void SetClasses()
        {
            base.SetClasses();

            if (size == AntInputSize.Large)
            {
                _groupWrapperClass = string.Join(" ", _groupWrapperClass, $"{PrefixCls}-search-large");
            }
            else if (size == AntInputSize.Small)
            {
                _groupWrapperClass = string.Join(" ", _groupWrapperClass, $"{PrefixCls}-search-small");
            }

            _affixWrapperClass = string.Join(" ", _affixWrapperClass, $"{PrefixCls}-search");
            _groupWrapperClass = string.Join(" ", _groupWrapperClass, $"{PrefixCls}-search");
            _groupWrapperClass = string.Join(" ", _groupWrapperClass, $"{PrefixCls}-search-enter-button");
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
    }
}