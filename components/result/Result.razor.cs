// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
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
        /// <default value="ResultStatus.Info" />
        [Parameter]
        public ResultStatus Status { get; set; } = ResultStatus.Info;

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

        private static Dictionary<ResultStatus, string> _typeMap = new()
        {
            [ResultStatus.Info] = "info",
            [ResultStatus.Success] = "success",
            [ResultStatus.Warning] = "warning",
            [ResultStatus.Error] = "error",
            [ResultStatus.Http403] = "403",
            [ResultStatus.Http404] = "404",
            [ResultStatus.Http500] = "500"
        };

        private RenderFragment BuildIcon => builder =>
        {
            var iconType = DetermineIconType();

            builder.OpenComponent<Icon>(1);
            builder.AddAttribute(2, "Type", iconType.type);
            builder.AddAttribute(2, "Theme", iconType.theme);
            builder.CloseComponent();
        };

        private (string type, IconThemeType theme) DetermineIconType()
        {
            if (!string.IsNullOrEmpty(Icon))
            {
                var separatorIndex = Icon.LastIndexOf("-", StringComparison.CurrentCultureIgnoreCase);
                var type = Icon.Substring(0, separatorIndex);
                var themeString = Icon.Substring(separatorIndex + 1, Icon.Length - separatorIndex - 1);

                IconThemeType theme;

                switch (themeString)
                {
                    case "fill":
                        theme = IconThemeType.Fill;
                        break;

                    case "twotone":
                        theme = IconThemeType.TwoTone;
                        break;

                    case "outline":
                    default:
                        theme = IconThemeType.Outline;
                        break;
                }

                return (type, theme);
            }

            switch (Status)
            {
                case ResultStatus.Error:
                    return ("close-circle", IconThemeType.Fill);

                case ResultStatus.Success:
                    return ("check-circle", IconThemeType.Fill);

                case ResultStatus.Warning:
                    return ("warning", IconThemeType.Fill);

                case ResultStatus.Http403:
                    return ("__unauthorized", default);

                case ResultStatus.Http404:
                    return ("__not-found", default);

                case ResultStatus.Http500:
                    return ("__bad-request", default);

                case ResultStatus.Info:
                default:
                    return ("info-circle", IconThemeType.Fill);
            }
        }

        private bool IsImage => Status.IsIn(ResultStatus.Http403, ResultStatus.Http404, ResultStatus.Http500);

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
                .Get(() => $"{PrefixCls}-{_typeMap[Status]}")
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
