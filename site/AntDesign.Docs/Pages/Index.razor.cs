// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Threading.Tasks;
using AntDesign.Docs.Services;
using AntDesign.Extensions.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace AntDesign.Docs.Pages
{
    public partial class Index : ComponentBase, IDisposable
    {
        private static readonly CssStyleSheet _styles = CssInJs.CreateStyles("docs-home", new System.Collections.Generic.Dictionary<string, object>
        {
            ["root"] = new System.Collections.Generic.Dictionary<string, object>
            {
                ["& h1.ant-typography, & h2.ant-typography, & h3.ant-typography, & h4.ant-typography"] = new System.Collections.Generic.Dictionary<string, object> { ["fontWeight"] = 400 },
                ["& img"] = new System.Collections.Generic.Dictionary<string, object> { ["objectFit"] = "cover" },
                ["& .home-block-content"] = new System.Collections.Generic.Dictionary<string, object> { ["margin"] = "88px 40px 124px" },
                ["& .home-block-content h2.ant-typography"] = new System.Collections.Generic.Dictionary<string, object> { ["marginBottom"] = 56 },
                ["& .home-block-content-extra"] = new System.Collections.Generic.Dictionary<string, object> { ["float"] = "right", ["paddingTop"] = 12, ["fontWeight"] = 200, ["fontSize"] = 16 },
                ["html.rtl & .home-block-content-extra"] = new System.Collections.Generic.Dictionary<string, object> { ["float"] = "left" },
                ["& .home-link-arrow"] = new System.Collections.Generic.Dictionary<string, object> { ["position"] = "relative", ["marginLeft"] = "0.3em", ["fontSize"] = "0.85em", ["opacity"] = 0.75, ["transition"] = "all 0.2s" },
                ["& .home-link-arrow-rtl"] = new System.Collections.Generic.Dictionary<string, object> { ["marginRight"] = "0.3em", ["marginLeft"] = 0 },
                ["& a:hover .home-link-arrow"] = new System.Collections.Generic.Dictionary<string, object> { ["transform"] = "translateX(3px)", ["opacity"] = 1 },
                ["& a:hover .home-link-arrow-rtl"] = new System.Collections.Generic.Dictionary<string, object> { ["transform"] = "translateX(-3px)", ["opacity"] = 1 },
                ["@media (max-width: 576px)"] = new System.Collections.Generic.Dictionary<string, object> { ["& .home-block-content"] = new System.Collections.Generic.Dictionary<string, object> { ["marginTop"] = 64, ["marginBottom"] = 96 }, ["& .home-block-content h2.ant-typography"] = new System.Collections.Generic.Dictionary<string, object> { ["marginBottom"] = 44 } },
                ["@media only screen and (max-width: 767.99px)"] = new System.Collections.Generic.Dictionary<string, object> { ["& .home-block-content"] = new System.Collections.Generic.Dictionary<string, object> { ["marginRight"] = 24, ["marginLeft"] = 24 } }
            },
            ["contentWidth"] = new System.Collections.Generic.Dictionary<string, object> { ["maxWidth"] = 1256, ["margin"] = "0 auto" },
            ["sectionTitle"] = new System.Collections.Generic.Dictionary<string, object> { ["fontWeight"] = "lighter", ["color"] = "rgb(49, 70, 89)" },
            ["recommendRow"] = new System.Collections.Generic.Dictionary<string, object> { ["margin"] = "-12px -12px -36px" },
            ["nestedRecommendRow"] = new System.Collections.Generic.Dictionary<string, object> { ["margin"] = "-12px -12px 12px" },
            ["recommendColumn"] = new System.Collections.Generic.Dictionary<string, object> { ["padding"] = 12 },
            ["productsContent"] = new System.Collections.Generic.Dictionary<string, object> { ["marginBottom"] = -28 },
            ["miniPanels"] = new System.Collections.Generic.Dictionary<string, object>
            {
                ["margin"] = "40px -20px 20px",
                ["& .ant-card-body"] = new System.Collections.Generic.Dictionary<string, object> { ["padding"] = "16px 16px 20px" },
                ["& .ant-card-meta-title"] = new System.Collections.Generic.Dictionary<string, object> { ["marginBottom"] = 4, ["fontSize"] = 18 },
                ["@media (max-width: 576px)"] = new System.Collections.Generic.Dictionary<string, object> { ["marginTop"] = "-23px !important" }
            },
            ["productColumn"] = new System.Collections.Generic.Dictionary<string, object> { ["padding"] = 20 },
            ["miniPanel"] = new System.Collections.Generic.Dictionary<string, object>
            {
                ["&:hover"] = new System.Collections.Generic.Dictionary<string, object> { ["boxShadow"] = "0 3px 6px -4px rgba(0, 0, 0, 0.12), 0 6px 16px 0 rgba(0, 0, 0, 0.08), 0 9px 28px 8px rgba(0, 0, 0, 0.05)" },
                ["& .ant-card-meta-title"] = new System.Collections.Generic.Dictionary<string, object> { ["color"] = "#0d1a26", ["fontWeight"] = 400, ["fontSize"] = 20 },
                ["& .ant-card-body"] = new System.Collections.Generic.Dictionary<string, object> { ["padding"] = "16px 20px 20px" },
                ["& .ant-card-meta-description"] = new System.Collections.Generic.Dictionary<string, object> { ["color"] = "#697b8c" }
            },
            ["sponsorRow"] = new System.Collections.Generic.Dictionary<string, object> { ["margin"] = "-16px -12px 16px" },
            ["sponsorColumn"] = new System.Collections.Generic.Dictionary<string, object> { ["padding"] = "16px 12px" },
            ["sponsorCard"] = new System.Collections.Generic.Dictionary<string, object> { ["marginTop"] = 16 },
            ["bannerTitle"] = new System.Collections.Generic.Dictionary<string, object> { ["fontSize"] = "3.7em" },
            ["banner"] = new System.Collections.Generic.Dictionary<string, object>
            {
                ["position"] = "relative", ["height"] = 512, ["overflow"] = "hidden", ["background"] = "url(https://gw.alipayobjects.com/zos/basement_prod/a8cf2dfe-ff6a-4a2e-ab73-f72dced0df99.svg)", ["backgroundSize"] = "cover",
                ["& .home-banner-holder"] = new System.Collections.Generic.Dictionary<string, object> { ["position"] = "relative", ["zIndex"] = 1, ["marginTop"] = 160 },
                ["& .home-banner-content"] = new System.Collections.Generic.Dictionary<string, object> { ["margin"] = "0 auto", ["padding"] = "0 16px", ["fontSize"] = 14, ["textAlign"] = "center" },
                ["& .home-banner-content p"] = new System.Collections.Generic.Dictionary<string, object> { ["margin"] = "16px 0 0", ["fontWeight"] = "lighter" },
                ["& .home-banner-content-operations"] = new System.Collections.Generic.Dictionary<string, object> { ["marginTop"] = 72, ["whiteSpace"] = "nowrap" },
                ["& .home-banner-content-operations a"] = new System.Collections.Generic.Dictionary<string, object> { ["margin"] = "0 8px" },
                ["& .home-banner-content-operations button.ant-btn.ant-btn-color-primary.ant-btn-variant-solid"] = new System.Collections.Generic.Dictionary<string, object> { ["backgroundColor"] = "#1668dc", ["borderColor"] = "rgba(0,0,0,0)", ["boxShadow"] = "0 2px 0 rgba(23,117,249,0.31)" },
                ["& .home-banner-background"] = new System.Collections.Generic.Dictionary<string, object> { ["position"] = "absolute", ["top"] = 0, ["right"] = 0, ["bottom"] = 0, ["left"] = 0 },
                ["& .home-banner-background > *"] = new System.Collections.Generic.Dictionary<string, object> { ["width"] = "100%", ["height"] = "100%", ["verticalAlign"] = "top" },
                ["& .home-banner-background > img"] = new System.Collections.Generic.Dictionary<string, object> { ["display"] = "block" },
                ["& .home-banner-background > svg"] = new System.Collections.Generic.Dictionary<string, object> { ["display"] = "none" },
                ["& .home-card-logo"] = new System.Collections.Generic.Dictionary<string, object> { ["position"] = "relative", ["display"] = "inline-block", ["maxWidth"] = 500, ["margin"] = "0 32px", ["cursor"] = "pointer" },
                ["& .home-card-logo .home-banner-mini"] = new System.Collections.Generic.Dictionary<string, object> { ["display"] = "block", ["maxWidth"] = "100%" },
                ["& .home-card-logo .home-banner-normal"] = new System.Collections.Generic.Dictionary<string, object> { ["display"] = "none" },
                ["& .home-card-logo .home-banner-mini, & .home-card-logo .home-banner-normal"] = new System.Collections.Generic.Dictionary<string, object> { ["fontWeight"] = 600, ["fontFamily"] = "PuHuiTi, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, 'Noto Sans', sans-serif, 'Apple Color Emoji', 'Segoe UI Emoji', 'Segoe UI Symbol', 'Noto Color Emoji', sans-serif", ["letterSpacing"] = "-0.18px" },
                ["@media (min-width: 576px)"] = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["height"] = 448,
                    ["& .home-banner-holder"] = new System.Collections.Generic.Dictionary<string, object> { ["marginTop"] = 130 },
                    ["& .home-banner-content"] = new System.Collections.Generic.Dictionary<string, object> { ["fontSize"] = 16 },
                    ["& .home-banner-content p"] = new System.Collections.Generic.Dictionary<string, object> { ["marginTop"] = 24 },
                    ["& .home-banner-content-operations"] = new System.Collections.Generic.Dictionary<string, object> { ["marginTop"] = 40 },
                    ["& .home-banner-background > img"] = new System.Collections.Generic.Dictionary<string, object> { ["display"] = "none" },
                    ["& .home-banner-background > svg"] = new System.Collections.Generic.Dictionary<string, object> { ["display"] = "block" },
                    ["& .home-card-logo"] = new System.Collections.Generic.Dictionary<string, object> { ["height"] = 90, ["verticalAlign"] = "top" },
                    ["& .home-card-logo .home-banner-mini"] = new System.Collections.Generic.Dictionary<string, object> { ["display"] = "none" },
                    ["& .home-card-logo .home-banner-normal"] = new System.Collections.Generic.Dictionary<string, object> { ["display"] = "block" }
                },
                ["@media (max-width: 575.99px)"] = new System.Collections.Generic.Dictionary<string, object> { ["& .home-banner-holder"] = new System.Collections.Generic.Dictionary<string, object> { ["marginTop"] = 160 } }
            }
        });

        [Parameter]
        public string Locale { get; set; }

        private Recommend[] _recommends = { };

        private Product[] _products = { };

        private MoreProps[] _moreArticles = { };

        private Sponsor[] _sponsors = { };
        [Inject] private DemoService DemoService { get; set; }
        [Inject] private ILocalizationService Language { get; set; }

        [Inject] private IStringLocalizer Localizer { get; set; }

        private bool _rendered;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override async Task OnInitializedAsync()
        {
            await FetchData();
            await base.OnInitializedAsync();

            Language.LanguageChanged += HandleLanguageChanged;
        }

        private void HandleLanguageChanged(object _, CultureInfo culture)
        {
            _rendered = true;
            _ = FetchData();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                _rendered = true;
                StateHasChanged();
                return;
            }

            //if (_rendered)
            //{
            //    _rendered = false;
            //    _ = FetchData();
            //}
        }

        private async Task FetchData()
        {
            _recommends = await DemoService.GetRecommend();
            _products = await DemoService.GetProduct();
            //_moreArticles = await DemoService.GetMore();
            _sponsors = await DemoService.GetSponsors();
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            Language.LanguageChanged -= HandleLanguageChanged;
        }
    }
}
