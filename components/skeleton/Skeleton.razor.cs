using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{

    public partial class Skeleton : AntDomComponentBase
    {
        #region Parameters


        [Parameter]
        public bool Active { get; set; }

        [Parameter]
        public bool Loading { get; set; } = true;

        /// <summary>
        /// 是否显示标题占位图
        /// </summary>
        [Parameter]
        public bool Title { get; set; } = true;
        [Parameter]
        public OneOf<int?, string> TitleWidth { get; set; }


        /// <summary>
        /// 是否显示头像占位图
        /// </summary>
        [Parameter]
        public bool Avatar { get; set; }

        [Parameter]
        public OneOf<int?, string> AvatarSize { get; set; }

        [Parameter]
        public string AvatarShape { get; set; }


        /// <summary>
        /// 是否显示段落占位图
        /// </summary>
        [Parameter]
        public bool Paragraph { get; set; } = true;
        [Parameter]
        public int? ParagraphRows { get; set; }
        [Parameter]
        public OneOf<int?, string, IList<OneOf<int?, string>>> ParagraphWidth { get; set; }

        #endregion Parameters

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private IList<OneOf<int?, string>> _paragraphRowsList;

        private void SetClassMap()
        {
            ClassMapper.Clear().Add("ant-skeleton")
                .If("ant-skeleton-with-avatar", () => this.Avatar)
                .If("ant-skeleton-active", () => this.Active);
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

            Console.WriteLine(TitleWidth.Value);

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

        public void SetAvatarProps()
        {
            if (Avatar == false) return;

            if (AvatarShape == null)
            {
                AvatarShape = SkeletonAvatarShape.Circle;
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
