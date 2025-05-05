// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
    <para>A long list can be divided into several pages using `Pagination`, and only one page will be loaded at a time.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>When it will take a long time to load/render all items.</item>
        <item>If you want to browse the data by navigating through pages.</item>
    </list>
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Navigation, "https://gw.alipayobjects.com/zos/alicdn/1vqv2bj68/Pagination.svg", Columns = 1, Title = "Pagination", SubTitle = "分页")]
    public partial class Pagination : AntDomComponentBase
    {
        /// <summary>
        /// Total number of data items	
        /// </summary>
        /// <default value="0" />
        [Parameter]
        public int Total { get; set; } = 0;

        /// <summary>
        /// Default initial page number	
        /// </summary>
        /// <default value="1" />
        [Parameter]
        public int DefaultCurrent { get; set; } = InitCurrent;

        /// <summary>
        /// Disable pagination
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Disabled { get; set; } = false;

        /// <summary>
        /// Current page number
        /// </summary>
        /// <default value="1" />
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

        /// <summary>
        /// Default number of data items per page
        /// </summary>
        /// <default value="10" />
        [Parameter]
        public int DefaultPageSize { get; set; } = InitPageSize;

        /// <summary>
        /// Number of data items per page
        /// </summary>
        /// <default value="10" />
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
        public EventCallback<int> OnPageSizeChanged { get; set; }

        /// <summary>
        /// Called when the page number is changed, and it takes the resulting page number and pageSize as its arguments
        /// </summary>
        [Parameter]
        public EventCallback<PaginationEventArgs> OnChange { get; set; }

        /// <summary>
        /// Whether to hide pager on single page
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool HideOnSinglePage { get; set; } = false;

        /// <summary>
        /// Determine whether to show PageSize select
        /// </summary>
        /// <default value="true when Total >= TotalBoundaryShowSizeChanger" />
        [Parameter]
        public bool ShowSizeChanger
        {
            get => GetShowSizeChanger();
            set
            {
                _showSizeChanger = value;
            }
        }

        /// <summary>
        /// Specify the sizeChanger options
        /// </summary>
        /// <default value="10, 20, 50, 100" />
        [Parameter]
        public int[] PageSizeOptions { get; set; } = PaginationOptions.DefaultPageSizeOptions;

        /// <summary>
        /// Called when PageSize is changed
        /// </summary>
        [Parameter]
        public EventCallback<PaginationEventArgs> OnShowSizeChange { get; set; }

        /// <summary>
        /// Determine whether you can jump to pages directly
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool ShowQuickJumper { get; set; } = false;

        /// <summary>
        /// Quick jumper confirm button render fragment
        /// </summary>
        [Parameter]
        public RenderFragment? GoButton { get; set; }

        /// <summary>
        /// Show page item's title
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool ShowTitle { get; set; } = true;

        /// <summary>
        /// To display the total number and range
        /// </summary>
        [Parameter]
        public OneOf<Func<PaginationTotalContext, string>, RenderFragment<PaginationTotalContext>>? ShowTotal { get; set; }

        /// <summary>
        /// Specify the size of Pagination, can be set to small.
        /// </summary>
        [Parameter]
        public PaginationSize Size { get; set; }

        /// <summary>
        /// (Not implemented) If Size is not specified, Pagination would resize according to the width of the window
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool Responsive { get; set; } = true;

        /// <summary>
        /// Whether to use simple mode
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Simple { get; set; } = false;

        /// <summary>
        /// Localization options
        /// </summary>
        /// <default value="LocaleProvider.CurrentLocale.Pagination" />
        [Parameter]
        public PaginationLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Pagination;

        /// <summary>
        /// Custom rendering for page item
        /// </summary>
        [Parameter]
        public RenderFragment<PaginationItemRenderContext>? ItemRender { get; set; } = context => context.OriginalElement(context);

        /// <summary>
        /// Show less page items
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool ShowLessItems { get; set; } = false;

        /// <summary>
        /// Show or hide the next/previous buttons
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool ShowPrevNextJumpers { get; set; } = true;

        /// <summary>
        /// Language direction
        /// </summary>
        /// <default value="ltr" />
        [Parameter]
        public string Direction { get; set; } = "ltr";

        /// <summary>
        /// Previous button
        /// </summary>
        [Parameter]
        public RenderFragment<PaginationItemRenderContext>? PrevIcon { get; set; }

        /// <summary>
        /// Next button
        /// </summary>
        [Parameter]
        public RenderFragment<PaginationItemRenderContext>? NextIcon { get; set; }

        /// <summary>
        /// Jump previous button
        /// </summary>
        [Parameter]
        public RenderFragment<PaginationItemRenderContext>? JumpPrevIcon { get; set; }

        /// <summary>
        /// Jump next icon
        /// </summary>
        [Parameter]
        public RenderFragment<PaginationItemRenderContext>? JumpNextIcon { get; set; }

        /// <summary>
        /// Used to determine if the size changer should show using the default logic. Ignored if ShowSizeChanger provided.
        /// </summary>
        /// <default value="50" />
        [Parameter]
        public int TotalBoundaryShowSizeChanger { get; set; } = 50;

        /// <summary>
        /// Any other parameters passed in get splatted onto the container element
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? UnmatchedAttributes { get; set; }

        private const string PrefixCls = "ant-pagination";

        private ClassMapper _prevClass = new();

        private ClassMapper _nextClass = new();

        private ClassMapper _jumpPrevClass = new();

        private ClassMapper _jumpNextClass = new();

        private int _current = 0;

        private int _pageSize = InitPageSize;

        private const int InitCurrent = 1;

        private const int InitPageSize = 10;

        private int _currentInputValue;

        private bool IsSmall => !Simple && Size == PaginationSize.Small;

        private bool? _showSizeChanger;

        protected override void OnInitialized()
        {
            SetClass();
            SetIcon();
            var hasOnChange = OnChange.HasDelegate;
            var hasCurrent = _current != 0;
            if (hasCurrent && !hasOnChange)
            {
                //Console.WriteLine("Warning: You provided a `current` prop to a Pagination component without an `onChange` handler. This will render a read-only component.");
            }

            var current = DefaultCurrent;
            if (_current != 0)
            {
                current = _current;
            }

            var pageSize = DefaultPageSize;
            if (PageSize != 0)
            {
                pageSize = PageSize;
            }

            current = Math.Min(current, CalculatePage(pageSize, PageSize, Total));

            _current = current;
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
               .If($"{PrefixCls}-mini", () => !Simple && Size == PaginationSize.Small)
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

        private async Task HandleChange(int p)
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

        private bool GetShowSizeChanger()
        {
            if (_showSizeChanger.HasValue)
            {
                return _showSizeChanger.Value;
            }

            return Total > TotalBoundaryShowSizeChanger;
        }

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
            var prevButton = ItemRender.Invoke(new(prevPage, PaginationItemType.Prev, GetItemIcon(RTL ? NextIcon : PrevIcon, "prev page"), disabled));

            return prevButton;
        }

        private RenderFragment RenderNext(int nextPage)
        {
            var disabled = !this.HasNext();
            var nextButton = ItemRender.Invoke(new(nextPage, PaginationItemType.Next, GetItemIcon(RTL ? PrevIcon : NextIcon, "next page"), disabled));
            return nextButton;
        }
    }
}
