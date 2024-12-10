// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class SkeletonElement : AntDomComponentBase
    {
        #region Parameters

        /// <summary>
        /// If the skeleton is active
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Active { get; set; } = false;

        /// <summary>
        /// Type of the element. Possible values: input, avatar, button
        /// </summary>
        [Parameter]
        public SkeletonElementType Type { get; set; }

        /// <summary>
        /// Size of element. Possible values: large, small, default. If type is avatar then an integer can be provided as well.
        /// </summary>
        [Parameter]
        public OneOf<SkeletonElementSize, string> Size { get; set; } = SkeletonElementSize.Default;

        /// <summary>
        /// Shape of the avatar. Not used for input type.
        /// </summary>
        /// <default value="SkeletonElementShape.Default" />
        [Parameter]
        public SkeletonElementShape Shape { get; set; } = SkeletonElementShape.Default;

        #endregion Parameters

        private ClassMapper _spanClassMapper = new ClassMapper();

        private string _spanStyle = "";

        private void SetClassMap()
        {
            ClassMapper
                .Add("ant-skeleton")
                .If("ant-skeleton-element", () => true)
                .If("ant-skeleton-active", () => Active)
                .If("ant-skeleton-rtl", () => RTL);
        }

        protected override void OnInitialized()
        {
            SetClassMap();

            if (Type == SkeletonElementType.Button)
                SetButtonMap();
            else if (Type == SkeletonElementType.Avatar)
                SetAvatarMap();
            else if (Type == SkeletonElementType.Input)
                SetInputMap();
        }

        private void SetButtonMap()
        {
            _spanClassMapper.Clear().If("ant-skeleton-button", () => true)
                .If("ant-skeleton-button-round", () => Shape == SkeletonElementShape.Round)
                .If("ant-skeleton-button-circle", () => Shape == SkeletonElementShape.Circle)
                .If("ant-skeleton-button-lg", () => Size.AsT0 == SkeletonElementSize.Large)
                .If("ant-skeleton-button-sm", () => Size.AsT0 == SkeletonElementSize.Small);
        }

        private void SetAvatarMap()
        {
            _spanClassMapper.Clear().If("ant-skeleton-avatar", () => true)
                .If("ant-skeleton-avatar-square", () => Shape == SkeletonElementShape.Square)
                .If("ant-skeleton-avatar-circle", () => Shape == SkeletonElementShape.Circle)
                .If("ant-skeleton-avatar-lg", () => Size.AsT0 == SkeletonElementSize.Large)
                .If("ant-skeleton-avatar-sm", () => Size.AsT0 == SkeletonElementSize.Small);
        }

        private void SetInputMap()
        {
            _spanClassMapper.Clear().If("ant-skeleton-input", () => true)
               .If("ant-skeleton-input-lg", () => Size.AsT0 == SkeletonElementSize.Large)
               .If("ant-skeleton-input-sm", () => Size.AsT0 == SkeletonElementSize.Small);
        }

        protected override void OnParametersSet()
        {
            if (Type == SkeletonElementType.Avatar)
            {
                if (Size.IsT0 == true)
                {
                    var sideLength = $"{Size.IsT0}px";
                    Style = $"width: {sideLength}, height: {sideLength}, 'line-height': {sideLength} ";
                }
            }
            StateHasChanged();
            base.OnParametersSet();
        }
    }
}
