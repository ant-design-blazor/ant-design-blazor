// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /**
    <summary>
    <para>Used to feed back the results of a series of operational tasks.</para>

    <h2>When To Use</h2>

    <para>Use when important operations need to inform the user to process the results and the feedback is more complicated.</para>
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Feedback, "https://gw.alipayobjects.com/zos/alicdn/9nepwjaLa/Result.svg", Columns = 1, Title = "Result", SubTitle = "结果")]
    public partial class Result : AntDomComponentBase
    {
        /// <summary>
        /// Title
        /// </summary>
        [Parameter]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Title. Takes priority over Title.
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// Sub Title
        /// </summary>
        [Parameter]
        public string SubTitle { get; set; } = string.Empty;

        /// <summary>
        /// Sub Title. Takes priority over SubTitle.
        /// </summary>
        [Parameter]
        public RenderFragment SubTitleTemplate { get; set; }

        /// <summary>
        /// Extra content displayed under all other content
        /// </summary>
        [Parameter]
        public RenderFragment Extra { get; set; }

        /// <summary>
        /// Type of result. Influences styles and default image/icon. Possible values: success, error, info, warning, 404, 403, 500
        /// </summary>
        /// <default value="info" />
        [Parameter]
        public string Status { get; set; } = "info";

        /// <summary>
        /// Custom icon. Format: "{type}-{theme}"
        /// </summary>
        [Parameter]
        public string Icon { get; set; }

        /// <summary>
        /// Show icon or not
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool IsShowIcon { get; set; } = true;

        /// <summary>
        /// Child content. Displayed between title/subtitle and extra.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject]
        private IconService IconService { get; set; }

        private const string PrefixCls = "ant-result";
        private string _svgImage;

        private ClassMapper IconClassMapper { get; set; } = new ClassMapper();

        private RenderFragment BuildIcon => builder =>
        {
            var iconType = DetermineIconType();

            builder.OpenComponent<Icon>(1);
            builder.AddAttribute(2, "Type", iconType.type);
            builder.AddAttribute(2, "Theme", iconType.theme);
            builder.CloseComponent();
        };

        private (string type, string theme) DetermineIconType()
        {
            if (!string.IsNullOrEmpty(Icon))
            {
                var separatorIndex = Icon.LastIndexOf("-", StringComparison.CurrentCultureIgnoreCase);
                var type = Icon.Substring(0, separatorIndex);
                var theme = Icon.Substring(separatorIndex + 1, Icon.Length - separatorIndex - 1);
                return (type, theme);
            }

            if (Status == "error")
                return ("close-circle", "fill");

            if (Status == "success")
                return ("check-circle", "fill");

            if (Status == "warning")
                return ("warning", "fill");

            if (Status == "403")
                return ("unauthorized", "internal");

            if (Status == "404")
                return ("not-found", "internal");

            if (Status == "500")
                return ("bad-request", "internal");

            return ("info-circle", "fill");
        }

        private bool IsImage => Status.IsIn("403", "404", "500");

        private void LoadImage()
        {
            if (!IsImage)
                return;

            var iconType = DetermineIconType();

            _svgImage = IconService.GetIconImg(iconType.type, iconType.theme);
        }

        private void SetClass()
        {
            ClassMapper.Add(PrefixCls)
                .Get(() => $"{PrefixCls}-{Status}")
                .If($"{PrefixCls}-rtl", () => RTL);

            IconClassMapper.Get(() => $"{PrefixCls}-{(IsImage ? "image" : "icon")}");
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            SetClass();
            LoadImage();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            LoadImage();
        }
    }
}
