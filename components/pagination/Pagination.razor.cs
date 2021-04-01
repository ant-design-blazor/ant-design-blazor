#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign.Internal;
using AntDesign.Locales;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    public partial class Pagination : AntDomComponentBase
    {
        [Parameter]
        public int Total { get; set; } = 0;

        [Parameter]
        public int DefaultCurrent { get; set; } = InitCurrent;

        [Parameter]
        public bool Disabled { get; set; } = false;

        [Parameter]
        public int Current
        {
            get => _current;
            set
            {
                if (value == 0)
                {
                    return;
                }

                if (value != _current)
                {
                    _current = value;
                    _currentInputValue = _current;
                }
            }
        }

        [Parameter]
        public int DefaultPageSize { get; set; } = InitPageSize;

        [Parameter]
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value == 0)
                {
                    return;
                }

                if (value != _pageSize)
                {
                    var current = _current;
                    var newCurrent = CalculatePage(value, _pageSize, Total);
                    current = current > newCurrent ? newCurrent : current;

                    _current = current;
                    _currentInputValue = current;

                    _pageSize = value;
                }
            }
        }

        [Parameter]
        public EventCallback<PaginationEventArgs> OnChange { get; set; }

        [Parameter]
        public bool HideOnSinglePage { get; set; } = false;

        [Parameter]
        public bool ShowSizeChanger { get; set; } = false;

        [Parameter]
        public int[] PageSizeOptions { get; set; } = PaginationOptions.DefaultPageSizeOptions;

        [Parameter]
        public EventCallback<PaginationEventArgs> OnShowSizeChange { get; set; }

        [Parameter]
        public bool ShowQuickJumper { get; set; } = false;

        [Parameter]
        public RenderFragment? GoButton { get; set; }

        [Parameter]
        public bool ShowTitle { get; set; } = true;

        [Parameter]
        public OneOf<Func<PaginationTotalContext, string>, RenderFragment<PaginationTotalContext>>? ShowTotal { get; set; }

        [Parameter]
        public string Size { get; set; } = "default";

        [Parameter]
        public bool Responsive { get; set; } = true;

        [Parameter]
        public bool Simple { get; set; } = false;

        [Parameter]
        public PaginationLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Pagination;

        [Parameter]
        public RenderFragment<PaginationItemRenderContext>? ItemRender { get; set; } = context => context.OriginalElement(context);

        [Parameter]
        public bool ShowLessItems { get; set; } = false;

        [Parameter]
        public bool ShowPrevNextJumpers { get; set; } = true;

        [Parameter]
        public string Direction { get; set; } = "ltr";

        [Parameter]
        public RenderFragment<PaginationItemRenderContext>? PrevIcon { get; set; }

        [Parameter]
        public RenderFragment<PaginationItemRenderContext>? NextIcon { get; set; }

        [Parameter]
        public RenderFragment<PaginationItemRenderContext>? JumpPrevIcon { get; set; }

        [Parameter]
        public RenderFragment<PaginationItemRenderContext>? JumpNextIcon { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? UnmatchedAttributes { get; set; }

        private const string PrefixCls = "ant-pagination";

        private ClassMapper _prevClass = new();

        private ClassMapper _nextClass = new();

        private ClassMapper _jumpPrevClass = new();

        private ClassMapper _jumpNextClass = new();

        private int _current = InitCurrent;

        private int _pageSize = InitPageSize;

        private const int InitCurrent = 1;

        private const int InitPageSize = 10;

        private int _currentInputValue;

        private bool IsSmall => !Simple && Size == "small";

        protected override void OnInitialized()
        {
            SetClass();
            SetIcon();
            var hasOnChange = OnChange.HasDelegate;
            var hasCurrent = _current != 0;
            if (hasCurrent && !hasOnChange)
            {
                Console.WriteLine("Warning: You provided a `current` prop to a Pagination component without an `onChange` handler. This will render a read-only component.");
            }

            var current = DefaultCurrent;
            if (Current != 0)
            {
                current = Current;
            }

            var pageSize = DefaultPageSize;
            if (PageSize != 0)
            {
                pageSize = PageSize;
            }

            current = Math.Min(current, CalculatePage(pageSize, PageSize, Total));

            Current = current;
            _currentInputValue = current;
            PageSize = pageSize;

            base.OnInitialized();
        }

        private void SetClass()
        {
            ClassMapper
               .Add(PrefixCls)
               .If($"{PrefixCls}-simple", () => Simple)
               .If($"{PrefixCls}-disabled", () => Disabled)
               .If("mini", () => !Simple && Size == "small")
               .If($"{PrefixCls}-rtl", () => RTL);

            _prevClass
               .Add($"{PrefixCls}-prev")
               .If($"{PrefixCls}-disabled", () => !HasPrev());

            _nextClass
               .Add($"{PrefixCls}-next")
               .If($"{PrefixCls}-disabled", () => !HasNext());

            _jumpPrevClass
               .Add($"{PrefixCls}-jump-prev")
               .If($"{PrefixCls}-jump-prev-custom-icon", () => JumpPrevIcon != null);

            _jumpNextClass
               .Add($"{PrefixCls}-jump-next")
               .If($"{PrefixCls}-jump-next-custom-icon", () => JumpNextIcon != null);
        }

        private int CalculatePage(int? p, int pageSize, int total)
        {
            var size = p ?? pageSize;
            return (int)Math.Floor((double)(total - 1) / size) + 1;
        }

        private int GetJumpPrevPage()
        {
            return Math.Max(1, _current - (ShowLessItems ? 3 : 5));
        }

        private int GetJumpNextPage()
        {
            return Math.Min(CalculatePage(null, _pageSize, Total), _current + (ShowLessItems ? 3 : 5));
        }

        private int GetValidValue(string inputValue)
        {
            var allPages = CalculatePage(null, _pageSize, Total);
            var currentInputValue = _currentInputValue;
            int value;
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                value = default;
            }
            else if (int.TryParse(inputValue, out var inputNumber))
            {
                value = inputNumber >= allPages ? allPages : inputNumber;
            }
            else
            {
                value = currentInputValue;
            }

            return value;
        }

        private bool IsValid(int page) => page != _current;

        private bool ShouldDisplayQuickJumper() => ShowQuickJumper && Total > _pageSize;

        private void HandleKeyUp(EventArgs e)
        {
            if (e is KeyboardEventArgs ke)
            {
                var value = GetValidValue(ke.Key);
                _currentInputValue = value;

                if (ke.Key == "Enter")
                {
                    HandleChange(value);
                }
                else if (ke.Key == "ArrowUp")
                {
                    HandleChange(value - 1);
                }
                else if (ke.Key == "ArrowDown")
                {
                    HandleChange(value + 1);
                }
            }
        }

        private async Task ChangePageSize(int size)
        {
            var current = _current;
            var newCurrent = CalculatePage(size, _pageSize, Total);
            current = current > newCurrent ? newCurrent : current;

            // fix the issue:
            // Once 'total' is 0, 'current' in 'onShowSizeChange' is 0, which is not correct.
            if (newCurrent == 0)
            {
                current = _current;
            }

            PageSize = size;
            Current = current;

            if (OnShowSizeChange.HasDelegate)
            {
                await OnShowSizeChange.InvokeAsync(new(current, size));
            }

            if (OnChange.HasDelegate)
            {
                await OnChange.InvokeAsync(new(current, size));
            }
        }

        private async void HandleChange(int p)
        {
            var disabled = Disabled;

            var page = p;
            if (IsValid(page) && !disabled)
            {
                var currentPage = CalculatePage(null, _pageSize, Total);
                if (page > currentPage)
                {
                    page = currentPage;
                }
                else if (page < 1)
                {
                    page = 1;
                }

                Current = page;

                var pageSize = _pageSize;
                if (OnChange.HasDelegate)
                {
                    await OnChange.InvokeAsync(new(page, pageSize));
                }
            }
        }

        private void Prev()
        {
            if (HasPrev())
            {
                HandleChange(_current - 1);
            }
        }

        private void Next()
        {
            if (HasNext())
            {
                HandleChange(_current + 1);
            }
        }

        private void JumpPrev() => HandleChange(GetJumpPrevPage());

        private void JumpNext() => HandleChange(GetJumpNextPage());

        private bool HasPrev() => _current > 1;

        private bool HasNext() => _current < CalculatePage(null, _pageSize, Total);

        private void RunIfEnter(KeyboardEventArgs @event, Action callback)
        {
            if (@event.Code == "Enter")
            {
                callback();
            }
        }

        private void RunIfEnterPrev(KeyboardEventArgs e) => this.RunIfEnter(e, Prev);

        private void RunIfEnterNext(KeyboardEventArgs e) => this.RunIfEnter(e, Next);

        private void RunIfEnterJumpPrev(KeyboardEventArgs e) => this.RunIfEnter(e, JumpPrev);

        private void RunIfEnterJumpNext(KeyboardEventArgs e) => this.RunIfEnter(e, JumpNext);

        private void HandleGoTO(EventArgs e)
        {
            if (e is KeyboardEventArgs ke && ke.Key == "Enter" || e is MouseEventArgs me && me.Type == "click")
            {
                HandleChange(_currentInputValue);
            }
        }

        private RenderFragment RenderPrev(int prevPage)
        {
            var disabled = !this.HasPrev();
            var prevButton = ItemRender.Invoke(new(prevPage, PaginationItemType.Prev, GetItemIcon(PrevIcon, "prev page"), disabled));

            return prevButton;
        }

        private RenderFragment RenderNext(int nextPage)
        {
            var disabled = !this.HasNext();
            var nextButton = ItemRender.Invoke(new(nextPage, PaginationItemType.Next, GetItemIcon(NextIcon, "next page"), disabled));
            return nextButton;
        }
    }
}
