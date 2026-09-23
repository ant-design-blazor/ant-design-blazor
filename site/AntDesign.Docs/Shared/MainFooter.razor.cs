// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using System.Collections.Generic;
using AntDesign.Docs.Services;
using AntDesign.Extensions.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace AntDesign.Docs.Shared
{
    public partial class MainFooter
    {
        private static readonly CssStyleSheet _footerStyles = CssInJs.CreateStyles("docs-footer", new Dictionary<string, object>
        {
            ["root"] = new Dictionary<string, object>
            {
                ["position"] = "relative", ["clear"] = "both", ["color"] = "rgba(255, 255, 255, 0.4)",
                ["fontSize"] = 14, ["lineHeight"] = 1.5, ["backgroundColor"] = "#000",
                ["& a"] = new Dictionary<string, object> { ["color"] = "rgba(255, 255, 255, 0.9)", ["textDecoration"] = "none", ["transition"] = "color 0.3s" },
                ["& a:hover"] = new Dictionary<string, object> { ["color"] = "#40a9ff" },
                ["&.rc-footer-light"] = new Dictionary<string, object> { ["color"] = "rgba(0, 0, 0, 0.85)", ["backgroundColor"] = "transparent" },
                ["&.rc-footer-light h2, &.rc-footer-light a"] = new Dictionary<string, object> { ["color"] = "rgba(0, 0, 0, 0.85)" },
                ["&.rc-footer-light .rc-footer-bottom-container"] = new Dictionary<string, object> { ["borderTopColor"] = "#e8e8e8" },
                ["&.rc-footer-light .rc-footer-item-separator, &.rc-footer-light .rc-footer-item-description"] = new Dictionary<string, object> { ["color"] = "rgba(0, 0, 0, 0.45)" },
                ["& .rc-footer-container"] = new Dictionary<string, object> { ["width"] = "100%", ["maxWidth"] = 1200, ["margin"] = "auto", ["padding"] = "80px 0 20px" },
                ["& .rc-footer-columns"] = new Dictionary<string, object> { ["display"] = "flex", ["justifyContent"] = "space-around" },
                ["& .rc-footer-column"] = new Dictionary<string, object> { ["marginBottom"] = 60 },
                ["& .rc-footer-column h2"] = new Dictionary<string, object> { ["position"] = "relative", ["margin"] = "0 auto", ["color"] = "#fff", ["fontSize"] = 16, ["fontWeight"] = 500 },
                ["& .rc-footer-column-icon, & .rc-footer-item-icon"] = new Dictionary<string, object> { ["position"] = "relative", ["top"] = -1, ["display"] = "inline-block", ["textAlign"] = "center", ["verticalAlign"] = "middle" },
                ["& .rc-footer-column-icon"] = new Dictionary<string, object> { ["width"] = 22, ["marginInlineEnd"] = "0.5em" },
                ["& .rc-footer-item-icon"] = new Dictionary<string, object> { ["width"] = 16, ["marginInlineEnd"] = "0.4em" },
                ["& .rc-footer-column-icon > span, & .rc-footer-column-icon > svg, & .rc-footer-column-icon img, & .rc-footer-item-icon > span, & .rc-footer-item-icon > svg, & .rc-footer-item-icon img"] = new Dictionary<string, object> { ["display"] = "block", ["width"] = "100%" },
                ["& .rc-footer-item"] = new Dictionary<string, object> { ["margin"] = "12px 0" },
                ["& .rc-footer-item-separator"] = new Dictionary<string, object> { ["margin"] = "0 0.3em" },
                ["& .rc-footer-bottom-container"] = new Dictionary<string, object> { ["width"] = "100%", ["maxWidth"] = 1200, ["margin"] = "0 auto", ["padding"] = "16px 0", ["borderTop"] = "1px solid rgba(255, 255, 255, 0.25)", ["fontSize"] = 16, ["lineHeight"] = "32px", ["textAlign"] = "center" },
                ["@media only screen and (max-width: 767.99px)"] = new Dictionary<string, object>
                {
                    ["textAlign"] = "center",
                    ["& .rc-footer-container"] = new Dictionary<string, object> { ["padding"] = "40px 0" },
                    ["& .rc-footer-columns"] = new Dictionary<string, object> { ["display"] = "block" },
                    ["& .rc-footer-column"] = new Dictionary<string, object> { ["display"] = "block", ["marginBottom"] = 40 },
                    ["& .rc-footer-column:last-child"] = new Dictionary<string, object> { ["marginBottom"] = 0 }
                }
            },
            ["container"] = new Dictionary<string, object>
            {
                ["width"] = "100%", ["maxWidth"] = 1200, ["margin"] = "auto", ["padding"] = "80px 0 20px",
                ["@media only screen and (max-width: 767.99px)"] = new Dictionary<string, object> { ["padding"] = "40px 0" }
            },
            ["columns"] = new Dictionary<string, object>
            {
                ["display"] = "flex", ["justifyContent"] = "space-around",
                ["@media only screen and (max-width: 767.99px)"] = new Dictionary<string, object> { ["display"] = "block" }
            },
            ["column"] = new Dictionary<string, object>
            {
                ["marginBottom"] = 60,
                ["@media only screen and (max-width: 767.99px)"] = new Dictionary<string, object> { ["display"] = "block", ["marginBottom"] = 40, ["&:last-child"] = new Dictionary<string, object> { ["marginBottom"] = 0 } }
            },
            ["columnHeading"] = new Dictionary<string, object> { ["position"] = "relative", ["margin"] = "0 auto", ["color"] = "#fff", ["fontSize"] = 16, ["fontWeight"] = 500 },
            ["columnIcon"] = new Dictionary<string, object>
            {
                ["position"] = "relative", ["top"] = -1, ["display"] = "inline-block", ["width"] = 22,
                ["textAlign"] = "center", ["verticalAlign"] = "middle", ["marginInlineEnd"] = "0.5em",
                ["& > span, & > svg, & img"] = new Dictionary<string, object> { ["display"] = "block", ["width"] = "100%" }
            },
            ["itemIcon"] = new Dictionary<string, object>
            {
                ["position"] = "relative", ["top"] = -1, ["display"] = "inline-block", ["width"] = 16,
                ["textAlign"] = "center", ["verticalAlign"] = "middle", ["marginInlineEnd"] = "0.4em",
                ["& > span, & > svg, & img"] = new Dictionary<string, object> { ["display"] = "block", ["width"] = "100%" }
            },
            ["item"] = new Dictionary<string, object> { ["margin"] = "12px 0" },
            ["separator"] = new Dictionary<string, object> { ["margin"] = "0 0.3em" },
            ["bottomContainer"] = new Dictionary<string, object>
            {
                ["width"] = "100%", ["maxWidth"] = 1200, ["margin"] = "0 auto", ["padding"] = "16px 0",
                ["borderTop"] = "1px solid rgba(255, 255, 255, 0.25)", ["fontSize"] = 16, ["lineHeight"] = "32px", ["textAlign"] = "center"
            },
            ["colorPickerItem"] = new Dictionary<string, object> { ["marginTop"] = 20 },
            ["colorPicker"] = new Dictionary<string, object> { ["width"] = "100%" },
            ["footerHeart"] = new Dictionary<string, object> { ["color"] = "rgb(255, 255, 255)" }
        });

        private string FooterClass(string key, string existingClass) => $"{existingClass} {_footerStyles[key]}";

        // region: color
        private static string _colorHex = "#1890ff";

        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private IMessageService Message { get; set; }

        [Inject] private ILocalizationService Language { get; set; }
        [Inject] private IStringLocalizer Localizer { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Language.LanguageChanged += (sender, args) =>
            {
                InvokeAsync(StateHasChanged);
            };
        }

        private async Task ChangeColor(ChangeEventArgs args)
        {
            _colorHex = args.Value.ToString();

            Message.Loading(Localizer["app.footer.primary-color-changing"].Value);

            await JS.InvokeVoidAsync("changeColor", _colorHex, DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public void OnColorChanged()
        {
            Message.Success(Localizer["app.footer.primary-color-changed"].Value);
        }
    }
}
