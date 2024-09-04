// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
    <para>Provide a placeholder while you wait for content to load, or to visualise content that doesn't exist yet.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>When resource need long time loading, like low network speed.</item>
        <item>The component contains much information, such as List or Card.</item>
        <item>Only works when loading data for the first time.</item>
        <item>Could be replaced by Spin in any situation, but can provide a better user experience.</item>
    </list>
    </summary>
    <seealso cref="SkeletonElement"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Feedback, "https://gw.alipayobjects.com/zos/alicdn/KpcciCJgv/Skeleton.svg", Columns = 1, Title = "Skeleton", SubTitle = "骨架屏")]
    public partial class Skeleton : AntDomComponentBase
    {
        #region Parameters

        /// <summary>
        /// Display active animation or not
        /// </summary>
        [Parameter]
        public bool Active { get; set; }

        /// <summary>
        /// Display skeleton when true
        /// </summary>
        [Parameter]
        public bool Loading { get; set; } = true;

        /// <summary>
        /// Show title placeholder
        /// </summary>
        [Parameter]
        public bool Title { get; set; } = true;

        /// <summary>
        /// Width of the title placeholder
        /// </summary>
        [Parameter]
        public OneOf<int?, string> TitleWidth { get; set; }

        /// <summary>
        /// Show avatar in placeholder
        /// </summary>
        [Parameter]
        public bool Avatar { get; set; }

        /// <summary>
        /// Avatar size
        /// </summary>
        [Parameter]
        public OneOf<int?, string> AvatarSize { get; set; }

        /// <summary>
        /// Avatar shape
        /// </summary>
        [Parameter]
        public string AvatarShape { get; set; }

        /// <summary>
        /// Show paragraph skeleton
        /// </summary>
        [Parameter]
        public bool Paragraph { get; set; } = true;

        /// <summary>
        /// Number of rows for paragraph skeleton
        /// </summary>
        [Parameter]
        public int? ParagraphRows { get; set; }

        /// <summary>
        /// Width of paragraph skeleton
        /// </summary>
        [Parameter]
        public OneOf<int?, string, IList<OneOf<int?, string>>> ParagraphWidth { get; set; }

        #endregion Parameters

        /// <summary>
        /// Content to display when skeleton is not active
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private IList<OneOf<int?, string>> _paragraphRowsList;

        private void SetClassMap()
        {
            ClassMapper
                .Add("ant-skeleton")
                .If("ant-skeleton-with-avatar", () => this.Avatar)
                .If("ant-skeleton-active", () => this.Active)
                .If("ant-skeleton-rtl", () => RTL);
        }

        protected override void OnInitialized()
        {
            SetClassMap();
            UpdateProps();
            base.OnInitialized();
        }

        private static string ToCSSUnit(OneOf<int?, string> value)
        {
            if (value.IsT0)
                return StyleHelper.ToCssPixel(value.AsT0.Value);
            else
                return StyleHelper.ToCssPixel(value.AsT1);
        }

        private void UpdateProps()
        {
            SetTitleProps();
            SetAvatarProps();
            SetParagraphProps();
        }

        private void SetTitleProps()
        {
            if (Title == false) return;

            if (TitleWidth.Value == null)
            {
                if (!Avatar && Paragraph)
                {
                    TitleWidth = "38%";
                }
                else if (Avatar && Paragraph)
                {
                    TitleWidth = "50%";
                }
                else
                {
                    TitleWidth = "";
                }
            }
        }

        private void SetAvatarProps()
        {
            if (Avatar == false) return;

            if (AvatarShape == null)
            {
                AvatarShape = AntDesign.AvatarShape.Circle;
            }

            if (AvatarSize.Value == null)
            {
                AvatarSize = SkeletonElementSize.Default;
            }
        }

        private void SetParagraphProps()
        {
            if (Paragraph == false) return;

            int rows;
            if (ParagraphRows.HasValue)
            {
                rows = ParagraphRows.Value - 1;
            }
            else if (!Avatar && Title)
            {
                rows = 2;
            }
            else
            {
                rows = 1;
            }

            if (ParagraphWidth.Value == null)
            {
                _paragraphRowsList = CreateRowsList(rows, "61%");
            }
            else
            {
                _paragraphRowsList = ParagraphWidth.Match(
                      f0 => CreateRowsList(rows, f0),
                      f1 => CreateRowsList(rows, f1),
                      f2 => f2);
            }
        }

        private static List<OneOf<int?, string>> CreateRowsList(int rows, OneOf<int?, string> lastWidth)
        {
            var rowlist = new List<OneOf<int?, string>>();
            for (int i = 0; i < rows; i++)
            {
                rowlist.Add("");
            }
            rowlist.Add(lastWidth);
            return rowlist;
        }
    }
}
