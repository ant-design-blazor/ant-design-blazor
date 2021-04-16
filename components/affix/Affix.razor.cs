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
        public int OffsetBottom { get; set; }

        /// <summary>
        /// Offset from the top of the viewport (in pixels)
        /// </summary>
        [Parameter]
        public int OffsetTop { get; set; }

        [Parameter]
        public string TargetSelector { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<bool> OnChange { get; set; }

        #endregion Parameters

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper
               .If(PrefixCls, () => _affixed);
        }

        public async override Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
        }

        protected async override Task OnFirstAfterRenderAsync()
        {
            await base.OnFirstAfterRenderAsync();

            if (ChildContent == null)
            {
                return;
            }

            var domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _childRef);
            _hiddenStyle = $"width: {domRect.width}px; height: {domRect.height}px;";

            await RenderAffixAsync();
            if (!_rootListened && string.IsNullOrEmpty(TargetSelector))
            {
                DomEventService.AddEventListener(RootScollSelector, "scroll", OnWindowScroll, false);
                DomEventService.AddEventListener(RootScollSelector, "resize", OnWindowResize, false);

                _rootListened = true;
            }
            else if (!string.IsNullOrEmpty(TargetSelector))
            {
                DomEventService.AddEventListener(TargetSelector, "scroll", OnTargetScroll);
                DomEventService.AddEventListener(TargetSelector, "resize", OnTargetResize);
                _targetListened = true;
            }
        }

        private async void OnWindowScroll(JsonElement obj) => await RenderAffixAsync();

        private async void OnWindowResize(JsonElement obj) => await RenderAffixAsync();

        private async void OnTargetScroll(JsonElement obj) => await RenderAffixAsync();

        private async void OnTargetResize(JsonElement obj) => await RenderAffixAsync();

        private async Task RenderAffixAsync()
        {
            var originalAffixStyle = _affixStyle;
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
            if (string.IsNullOrEmpty(TargetSelector))
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
                containerRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, TargetSelector);
            }

            var topDist = containerRect.top + OffsetTop;
            var bottomDist = containerRect.bottom - OffsetBottom;

            if (OffsetBottom > 0) // only affix bottom
            {
                if (domRect.bottom > bottomDist)
                {
                    _affixStyle = _hiddenStyle + $"bottom: { window.innerHeight - bottomDist}px; position: fixed;";
                    Affixed = true;
                }
                else
                {
                    _affixStyle = string.Empty;
                    Affixed = false;
                }
            }
            else if (domRect.top < topDist)
            {
                _affixStyle = _hiddenStyle + $"top: {topDist}px; position: fixed;";
                Affixed = true;
            }
            else
            {
                _affixStyle = string.Empty;
                Affixed = false;
            }

            if (originalAffixStyle != _affixStyle)
            {
                StateHasChanged();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (string.IsNullOrEmpty(TargetSelector))
            {
                DomEventService.RemoveEventListerner<JsonElement>(RootScollSelector, "scroll", OnWindowScroll);
                DomEventService.RemoveEventListerner<JsonElement>(RootScollSelector, "resize", OnWindowResize);
            }
            else
            {
                DomEventService.RemoveEventListerner<JsonElement>(TargetSelector, "scroll", OnWindowScroll);
                DomEventService.RemoveEventListerner<JsonElement>(TargetSelector, "resize", OnWindowResize);
            }
        }
    }
}
