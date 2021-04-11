using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Affix : AntDomComponentBase
    {
        private const string PrefixCls = "ant-affix";
        private const string RootScollSelector = "window";
        //private const string RootRectSelector = "app";
        private bool _affixed;
        private bool _rootListened;
        private bool _targetListened;

        private bool Affixed
        {
            get => _affixed;
            set
            {
                if (_affixed != value)
                {
                    _affixed = value;
                    StateHasChanged();
                    if (OnChange.HasDelegate)
                    {
                        OnChange.InvokeAsync(_affixed);
                    }
                }
            }
        }

        private ElementReference _childRef;
        private string _hiddenStyle;
        private string _affixStyle;

        [Inject]
        private DomEventService DomEventService { get; set; }

        #region Parameters

        /// <summary>
        /// Offset from the bottom of the viewport (in pixels)
        /// </summary>
        [Parameter]
        public uint? OffsetBottom { get; set; }

        /// <summary>
        /// Offset from the top of the viewport (in pixels)
        /// </summary>
        [Parameter]
        public uint? OffsetTop { get; set; } = 0;

        /// <summary>
        /// Specifies the scrollable area DOM node
        /// </summary>
        [Parameter]
        public ElementReference Target { get; set; }

        [Parameter]
        public string TargetId { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<bool> OnChange { get; set; }

        #endregion Parameters

        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetClasses();
        }

        public async override Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
        }

        protected async override Task OnFirstAfterRenderAsync()
        {
            await base.OnFirstAfterRenderAsync();

            var domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _childRef);
            _hiddenStyle = $"width: {domRect.width}px; height: {domRect.height}px;";

            DomEventService.AddEventListener(RootScollSelector, "scroll", OnWindowScroll, false);
            DomEventService.AddEventListener(RootScollSelector, "resize", OnWindowResize, false);
            await RenderAffixAsync();
            if (!_rootListened && string.IsNullOrEmpty(TargetId))
            {
                _rootListened = true;
            }
            else if (!string.IsNullOrEmpty(TargetId))
            {
                var targetStr = $"#{TargetId}";
                DomEventService.AddEventListener(targetStr, "scroll", OnTargetScroll);
                DomEventService.AddEventListener(targetStr, "resize", OnTargetResize);
                _targetListened = true;
            }
        }
        private async void OnWindowScroll(JsonElement obj)
        {
            await RenderAffixAsync(true);
        }

        private async void OnWindowResize(JsonElement obj)
        {
            await RenderAffixAsync(true);
        }

        private async void OnTargetScroll(JsonElement obj)
        {
            await RenderAffixAsync();
        }
        private async void OnTargetResize(JsonElement obj)
        {
            await RenderAffixAsync();
        }

        private void SetClasses()
        {
            ClassMapper.Clear()
                .If(PrefixCls, () => _affixed);
        }

        private async Task RenderAffixAsync(bool windowscrolled = false)
        {
            if (windowscrolled && !string.IsNullOrEmpty(TargetId))
            {
                if (!Affixed)
                {
                    return;
                }
                _affixStyle = string.Empty;
                Affixed = false;
                StateHasChanged();
                return;
            }

            DomRect childRect = null;
            DomRect domRect = null;
            Window window = null;

            async Task GetWindow()
            {
                window = await JsInvokeAsync<Window>(JSInteropConstants.GetWindow);
            }

            async Task GetDomReact()
            {
                domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, Ref);
            }

            async Task GetChildReact()
            {
                childRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _childRef);
            }

            await Task.WhenAll(new[] { GetWindow(), GetDomReact(), GetChildReact() });
            if (childRect == null || domRect == null || window == null)
            {
                return;
            }

            _hiddenStyle = $"width: {childRect.width}px; height: {childRect.height}px;";

            DomRect containerRect;
            if (string.IsNullOrEmpty(TargetId))
            {
                containerRect = new DomRect()
                {
                    top = 0,
                    bottom = window.innerHeight,
                    height = window.innerHeight,
                };
            }
            else
            {
                containerRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, $"#{TargetId}");
            }
            // become affixed
            if (OffsetBottom.HasValue)
            {
                // domRect.bottom / domRect.top have the identical value here.
                var bottom = containerRect.bottom - OffsetBottom;
                if (domRect.bottom > bottom)
                {
                    _affixStyle = _hiddenStyle + $"bottom: { window.innerHeight - bottom}px; position: fixed;";
                    Affixed = true;
                }
                else
                {
                    _affixStyle = string.Empty;
                    Affixed = false;
                }
            }
            else if (OffsetTop.HasValue)
            {
                var top = containerRect.top + OffsetTop;
                if (domRect.top < top && top > 0)
                {
                    _affixStyle = _hiddenStyle + $"top: {top}px; position: fixed;";
                    Affixed = true;
                }
                else
                {
                    _affixStyle = string.Empty;
                    Affixed = false;
                }
            }

            StateHasChanged();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            DomEventService.RemoveEventListerner<JsonElement>(RootScollSelector, "scroll", OnWindowScroll);
            DomEventService.RemoveEventListerner<JsonElement>(RootScollSelector, "resize", OnWindowResize);
        }
    }
}
